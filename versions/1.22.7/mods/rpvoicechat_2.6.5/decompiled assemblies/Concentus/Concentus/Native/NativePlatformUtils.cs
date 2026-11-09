using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Concentus.Native
{
	// Token: 0x02000074 RID: 116
	internal static class NativePlatformUtils
	{
		// Token: 0x060002FE RID: 766 RVA: 0x000212E0 File Offset: 0x0001F4E0
		internal static OSAndArchitecture GetCurrentPlatform(TextWriter logger)
		{
			object mutex = NativePlatformUtils._mutex;
			OSAndArchitecture cachedPlatformInfo;
			lock (mutex)
			{
				if (!NativePlatformUtils._cachedPlatformInfoExists)
				{
					NativePlatformUtils._cachedPlatformInfo = NativePlatformUtils.GetCurrentPlatformInternal(logger);
				}
				cachedPlatformInfo = NativePlatformUtils._cachedPlatformInfo;
			}
			return cachedPlatformInfo;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00021334 File Offset: 0x0001F534
		internal static OSAndArchitecture GetCurrentPlatformInternal(TextWriter logger)
		{
			if (logger != null)
			{
				logger.WriteLine("Runtime ID is \"" + RuntimeInformation.RuntimeIdentifier + "\"");
			}
			OSAndArchitecture osandArchitecture = NativePlatformUtils.ParseRuntimeId(RuntimeInformation.RuntimeIdentifier);
			PlatformOperatingSystem platformOperatingSystem = osandArchitecture.OS;
			PlatformArchitecture platformArchitecture = osandArchitecture.Architecture;
			if (platformOperatingSystem == PlatformOperatingSystem.Unknown)
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					platformOperatingSystem = PlatformOperatingSystem.Windows;
				}
				else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ANDROID_STORAGE")))
				{
					platformOperatingSystem = PlatformOperatingSystem.Android;
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					platformOperatingSystem = PlatformOperatingSystem.MacOS;
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					platformOperatingSystem = PlatformOperatingSystem.Linux;
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
				{
					platformOperatingSystem = PlatformOperatingSystem.FreeBSD;
				}
			}
			if (platformOperatingSystem != PlatformOperatingSystem.Unknown && platformArchitecture == PlatformArchitecture.Unknown)
			{
				platformArchitecture = NativePlatformUtils.TryGetNativeArchitecture(platformOperatingSystem, logger);
			}
			if (platformArchitecture == PlatformArchitecture.Unknown)
			{
				switch (RuntimeInformation.ProcessArchitecture)
				{
				case 0:
					platformArchitecture = PlatformArchitecture.I386;
					break;
				case 1:
					platformArchitecture = PlatformArchitecture.X64;
					break;
				case 2:
					platformArchitecture = PlatformArchitecture.ArmV7;
					break;
				case 3:
					platformArchitecture = PlatformArchitecture.Arm64;
					break;
				case 5:
					platformArchitecture = PlatformArchitecture.S390x;
					break;
				case 6:
					platformArchitecture = PlatformArchitecture.Loongarch64;
					break;
				case 7:
					platformArchitecture = PlatformArchitecture.ArmV6;
					break;
				case 8:
					platformArchitecture = PlatformArchitecture.PowerPC64;
					break;
				}
			}
			return new OSAndArchitecture(platformOperatingSystem, platformArchitecture);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00021444 File Offset: 0x0001F644
		internal static NativeLibraryStatus PrepareNativeLibrary(string libraryName, TextWriter logger)
		{
			if (logger != null)
			{
				logger.WriteLine("Preparing native library \"{0}\"", libraryName);
			}
			OSAndArchitecture currentPlatform = NativePlatformUtils.GetCurrentPlatform(logger);
			if (logger != null)
			{
				logger.WriteLine("Detected current platform as \"{0}\"", currentPlatform);
			}
			string text = NativePlatformUtils.NormalizeLibraryName(libraryName, currentPlatform);
			object mutex = NativePlatformUtils._mutex;
			NativeLibraryStatus result;
			lock (mutex)
			{
				NativeLibraryStatus nativeLibraryStatus;
				if (NativePlatformUtils._loadedLibraries.TryGetValue(text, ref nativeLibraryStatus))
				{
					if (logger != null)
					{
						logger.WriteLine("Native library \"{0}\" has already been prepared; nothing to do", libraryName);
					}
					result = nativeLibraryStatus;
				}
				else if (currentPlatform.OS == PlatformOperatingSystem.Android)
				{
					if (logger != null)
					{
						logger.WriteLine("Probing for " + text + " within local Android .apk");
					}
					if (NativePlatformUtils.ProbeLibrary(text, currentPlatform, logger) != NativeLibraryStatus.Available)
					{
						if (logger != null)
						{
							logger.WriteLine("Native library \"{0}\" was not found in the local .apk.", libraryName);
						}
						result = NativeLibraryStatus.Unavailable;
					}
					else
					{
						result = NativeLibraryStatus.Available;
					}
				}
				else
				{
					if (logger != null)
					{
						logger.WriteLine("Probing for an already existing " + text);
					}
					NativeLibraryStatus nativeLibraryStatus2 = NativePlatformUtils.ProbeLibrary(text, currentPlatform, logger);
					if (nativeLibraryStatus2 == NativeLibraryStatus.Available)
					{
						if (logger != null)
						{
							logger.WriteLine("Native library \"{0}\" resolved to an already-existing library. Loading current file as-is.", libraryName);
						}
						NativePlatformUtils._loadedLibraries[text] = nativeLibraryStatus2;
						result = nativeLibraryStatus2;
					}
					else
					{
						NativePlatformUtils.DeleteLocalLibraryIfPresent(text, logger);
						string text2 = Path.Combine(Environment.CurrentDirectory, "runtimes");
						List<string> list = NativePlatformUtils.PermuteLibraryNames(libraryName, currentPlatform);
						foreach (string text3 in NativePlatformUtils.PermuteArchitectureSpecificDirectoryNames(currentPlatform))
						{
							DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text2, text3, "native"));
							if (directoryInfo.Exists)
							{
								foreach (string text4 in list)
								{
									FileInfo fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, text4));
									if (fileInfo.Exists)
									{
										if (currentPlatform.OS == PlatformOperatingSystem.Windows || currentPlatform.OS == PlatformOperatingSystem.Linux || currentPlatform.OS == PlatformOperatingSystem.MacOS)
										{
											FileInfo fileInfo2 = new FileInfo(Path.Combine(Environment.CurrentDirectory, text));
											try
											{
												if (logger != null)
												{
													logger.WriteLine("Resolved native library \"" + libraryName + "\" to " + fileInfo.FullName);
												}
												fileInfo.CopyTo(fileInfo2.FullName);
												NativePlatformUtils._loadedLibraries[text] = NativeLibraryStatus.Available;
												return NativeLibraryStatus.Available;
											}
											catch (Exception ex)
											{
												if (logger != null)
												{
													logger.WriteLine(ex.Message);
												}
												if (logger != null)
												{
													logger.WriteLine("Could not prepare native library \"" + libraryName + "\" (is the existing library file locked or in use?)");
												}
												NativePlatformUtils._loadedLibraries[text] = NativeLibraryStatus.Unknown;
												return NativeLibraryStatus.Unknown;
											}
										}
										DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
										defaultInterpolatedStringHandler.AppendLiteral("Don't know yet how to load libraries for ");
										defaultInterpolatedStringHandler.AppendFormatted<PlatformOperatingSystem>(currentPlatform.OS);
										throw new PlatformNotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
									}
								}
							}
						}
						if (logger != null)
						{
							logger.WriteLine("Failed to resolve native library \"{0}\".", libraryName);
						}
						NativePlatformUtils._loadedLibraries[text] = NativeLibraryStatus.Unavailable;
						result = NativeLibraryStatus.Unavailable;
					}
				}
			}
			return result;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00021794 File Offset: 0x0001F994
		internal static string GetRuntimeIdString(this PlatformArchitecture architecture)
		{
			switch (architecture)
			{
			case PlatformArchitecture.Unknown:
				return "unknown";
			case PlatformArchitecture.Any:
				return "any";
			case PlatformArchitecture.I386:
				return "x86";
			case PlatformArchitecture.X64:
				return "x64";
			case PlatformArchitecture.ArmV7:
				return "arm";
			case PlatformArchitecture.Arm64:
				return "arm64";
			case PlatformArchitecture.Armel:
				return "armel";
			case PlatformArchitecture.ArmV6:
				return "armv6";
			case PlatformArchitecture.Mips64:
				return "mips64";
			case PlatformArchitecture.PowerPC64:
				return "ppc64le";
			case PlatformArchitecture.RiscFive:
				return "riscv64";
			case PlatformArchitecture.S390x:
				return "s390x";
			case PlatformArchitecture.Loongarch64:
				return "loongarch64";
			case PlatformArchitecture.Itanium64:
				return "ia64";
			default:
				throw new PlatformNotSupportedException("No runtime ID defined for " + architecture.ToString());
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00021854 File Offset: 0x0001FA54
		internal static string GetRuntimeIdString(this PlatformOperatingSystem os)
		{
			switch (os)
			{
			case PlatformOperatingSystem.Unknown:
				return "unknown";
			case PlatformOperatingSystem.Any:
				return "any";
			case PlatformOperatingSystem.Windows:
				return "win";
			case PlatformOperatingSystem.Linux:
				return "linux";
			case PlatformOperatingSystem.MacOS:
				return "osx";
			case PlatformOperatingSystem.iOS:
				return "ios";
			case PlatformOperatingSystem.iOS_Simulator:
				return "iossimulator";
			case PlatformOperatingSystem.Android:
				return "android";
			case PlatformOperatingSystem.FreeBSD:
				return "freebsd";
			case PlatformOperatingSystem.Illumos:
				return "illumos";
			case PlatformOperatingSystem.Linux_Bionic:
				return "linux-bionic";
			case PlatformOperatingSystem.Linux_Musl:
				return "linux-musl";
			case PlatformOperatingSystem.MacCatalyst:
				return "maccatalyst";
			case PlatformOperatingSystem.Solaris:
				return "solaris";
			case PlatformOperatingSystem.TvOS:
				return "tvos";
			case PlatformOperatingSystem.TvOS_Simulator:
				return "tvossimulator";
			case PlatformOperatingSystem.Unix:
				return "unix";
			case PlatformOperatingSystem.Browser:
				return "browser";
			case PlatformOperatingSystem.Wasi:
				return "wasi";
			default:
				throw new PlatformNotSupportedException("No runtime ID defined for " + os.ToString());
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00021944 File Offset: 0x0001FB44
		internal static string[] GetInheritedRuntimeIds(string runtimeId)
		{
			string[] result;
			if (NativePlatformUtils.RidInheritanceMappings.TryGetValue(runtimeId, ref result))
			{
				return result;
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00021968 File Offset: 0x0001FB68
		internal static OSAndArchitecture ParseRuntimeId(ReadOnlySpan<char> runtimeId)
		{
			int num = MemoryExtensions.IndexOf<char>(runtimeId, '-');
			if (num < 0)
			{
				return new OSAndArchitecture(NativePlatformUtils.TryParseOperatingSystemString(runtimeId), PlatformArchitecture.Unknown);
			}
			return new OSAndArchitecture(NativePlatformUtils.TryParseOperatingSystemString(runtimeId.Slice(0, num)), NativePlatformUtils.TryParseArchitectureString(runtimeId.Slice(num + 1)));
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000219B1 File Offset: 0x0001FBB1
		internal static PlatformOperatingSystem TryParseOperatingSystemString(string os)
		{
			return NativePlatformUtils.TryParseOperatingSystemString(MemoryExtensions.AsSpan(os));
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000219C0 File Offset: 0x0001FBC0
		internal static PlatformOperatingSystem TryParseOperatingSystemString(ReadOnlySpan<char> os)
		{
			if (os.Length >= "win".Length && MemoryExtensions.Equals(os.Slice(0, "win".Length), MemoryExtensions.AsSpan("win"), 5))
			{
				return PlatformOperatingSystem.Windows;
			}
			if (os.Length >= "linux".Length && MemoryExtensions.Equals(os.Slice(0, "linux".Length), MemoryExtensions.AsSpan("linux"), 5))
			{
				return PlatformOperatingSystem.Linux;
			}
			if (os.Length >= "ubuntu".Length && MemoryExtensions.Equals(os.Slice(0, "ubuntu".Length), MemoryExtensions.AsSpan("ubuntu"), 5))
			{
				return PlatformOperatingSystem.Linux;
			}
			if (os.Length >= "debian".Length && MemoryExtensions.Equals(os.Slice(0, "debian".Length), MemoryExtensions.AsSpan("debian"), 5))
			{
				return PlatformOperatingSystem.Linux;
			}
			if (os.Length >= "osx".Length && MemoryExtensions.Equals(os.Slice(0, "osx".Length), MemoryExtensions.AsSpan("osx"), 5))
			{
				return PlatformOperatingSystem.MacOS;
			}
			if (os.Length >= "ios".Length && MemoryExtensions.Equals(os.Slice(0, "ios".Length), MemoryExtensions.AsSpan("ios"), 5))
			{
				return PlatformOperatingSystem.iOS;
			}
			if (os.Length >= "iossimulator".Length && MemoryExtensions.Equals(os.Slice(0, "iossimulator".Length), MemoryExtensions.AsSpan("iossimulator"), 5))
			{
				return PlatformOperatingSystem.iOS_Simulator;
			}
			if (os.Length >= "android".Length && MemoryExtensions.Equals(os.Slice(0, "android".Length), MemoryExtensions.AsSpan("android"), 5))
			{
				return PlatformOperatingSystem.Android;
			}
			if (os.Length >= "freebsd".Length && MemoryExtensions.Equals(os.Slice(0, "freebsd".Length), MemoryExtensions.AsSpan("freebsd"), 5))
			{
				return PlatformOperatingSystem.FreeBSD;
			}
			if (os.Length >= "illumos".Length && MemoryExtensions.Equals(os.Slice(0, "illumos".Length), MemoryExtensions.AsSpan("illumos"), 5))
			{
				return PlatformOperatingSystem.Illumos;
			}
			if (os.Length >= "linux-bionic".Length && MemoryExtensions.Equals(os.Slice(0, "linux-bionic".Length), MemoryExtensions.AsSpan("linux-bionic"), 5))
			{
				return PlatformOperatingSystem.Linux_Bionic;
			}
			if (os.Length >= "linux-musl".Length && MemoryExtensions.Equals(os.Slice(0, "linux-musl".Length), MemoryExtensions.AsSpan("linux-musl"), 5))
			{
				return PlatformOperatingSystem.Linux_Musl;
			}
			if (os.Length >= "maccatalyst".Length && MemoryExtensions.Equals(os.Slice(0, "maccatalyst".Length), MemoryExtensions.AsSpan("maccatalyst"), 5))
			{
				return PlatformOperatingSystem.MacCatalyst;
			}
			if (os.Length >= "solaris".Length && MemoryExtensions.Equals(os.Slice(0, "solaris".Length), MemoryExtensions.AsSpan("solaris"), 5))
			{
				return PlatformOperatingSystem.Solaris;
			}
			if (os.Length >= "tvos".Length && MemoryExtensions.Equals(os.Slice(0, "tvos".Length), MemoryExtensions.AsSpan("tvos"), 5))
			{
				return PlatformOperatingSystem.TvOS;
			}
			if (os.Length >= "tvossimulator".Length && MemoryExtensions.Equals(os.Slice(0, "tvossimulator".Length), MemoryExtensions.AsSpan("tvossimulator"), 5))
			{
				return PlatformOperatingSystem.TvOS_Simulator;
			}
			if (os.Length >= "unix".Length && MemoryExtensions.Equals(os.Slice(0, "unix".Length), MemoryExtensions.AsSpan("unix"), 5))
			{
				return PlatformOperatingSystem.Unix;
			}
			if (os.Length >= "browser".Length && MemoryExtensions.Equals(os.Slice(0, "browser".Length), MemoryExtensions.AsSpan("browser"), 5))
			{
				return PlatformOperatingSystem.Browser;
			}
			if (os.Length >= "wasi".Length && MemoryExtensions.Equals(os.Slice(0, "wasi".Length), MemoryExtensions.AsSpan("wasi"), 5))
			{
				return PlatformOperatingSystem.Wasi;
			}
			return PlatformOperatingSystem.Unknown;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00021E13 File Offset: 0x00020013
		internal static PlatformArchitecture TryParseArchitectureString(string arch)
		{
			return NativePlatformUtils.TryParseArchitectureString(MemoryExtensions.AsSpan(arch));
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00021E20 File Offset: 0x00020020
		internal static PlatformArchitecture TryParseArchitectureString(ReadOnlySpan<char> arch)
		{
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("any"), 5))
			{
				return PlatformArchitecture.Any;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("x86"), 5))
			{
				return PlatformArchitecture.I386;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("x64"), 5))
			{
				return PlatformArchitecture.X64;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("arm"), 5))
			{
				return PlatformArchitecture.ArmV7;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("arm64"), 5))
			{
				return PlatformArchitecture.Arm64;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("armel"), 5))
			{
				return PlatformArchitecture.Armel;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("armv6"), 5))
			{
				return PlatformArchitecture.ArmV6;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("mips64"), 5))
			{
				return PlatformArchitecture.Mips64;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("ppc64le"), 5))
			{
				return PlatformArchitecture.PowerPC64;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("riscv64"), 5))
			{
				return PlatformArchitecture.RiscFive;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("s390x"), 5))
			{
				return PlatformArchitecture.S390x;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("loongarch64"), 5))
			{
				return PlatformArchitecture.Loongarch64;
			}
			if (MemoryExtensions.Equals(arch, MemoryExtensions.AsSpan("ia64"), 5))
			{
				return PlatformArchitecture.Itanium64;
			}
			return PlatformArchitecture.Unknown;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00021F44 File Offset: 0x00020144
		internal static PlatformArchitecture TryGetNativeArchitecture(PlatformOperatingSystem os, TextWriter logger)
		{
			if (os != PlatformOperatingSystem.Windows)
			{
				if (os == PlatformOperatingSystem.Linux || os == PlatformOperatingSystem.Unix || os == PlatformOperatingSystem.Linux_Musl || os == PlatformOperatingSystem.Linux_Bionic || os == PlatformOperatingSystem.Android)
				{
					PlatformArchitecture? platformArchitecture = KernelInteropLinux.TryGetArchForUnix(logger);
					if (platformArchitecture != null)
					{
						return platformArchitecture.Value;
					}
				}
				return PlatformArchitecture.Unknown;
			}
			KernelInteropWindows.SYSTEM_INFO system_INFO = default(KernelInteropWindows.SYSTEM_INFO);
			KernelInteropWindows.GetSystemInfo(ref system_INFO);
			ushort wProcessorArchitecture = system_INFO.wProcessorArchitecture;
			if (wProcessorArchitecture != 0)
			{
				switch (wProcessorArchitecture)
				{
				case 5:
					return PlatformArchitecture.ArmV7;
				case 6:
					return PlatformArchitecture.Itanium64;
				case 7:
				case 8:
					break;
				case 9:
					goto IL_40;
				default:
					if (wProcessorArchitecture == 18)
					{
						return PlatformArchitecture.Arm64;
					}
					break;
				}
				return PlatformArchitecture.Unknown;
			}
			IL_40:
			if (Marshal.SizeOf<IntPtr>((IntPtr)0) != 4)
			{
				return PlatformArchitecture.X64;
			}
			return PlatformArchitecture.I386;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00021FD8 File Offset: 0x000201D8
		private static void DeleteLocalLibraryIfPresent(string normalizedLibraryName, TextWriter logger)
		{
			FileInfo fileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, normalizedLibraryName));
			if (fileInfo.Exists)
			{
				try
				{
					if (logger != null)
					{
						logger.WriteLine("Clobbering existing file " + fileInfo.FullName);
					}
					fileInfo.Delete();
				}
				catch (Exception)
				{
					if (logger != null)
					{
						logger.WriteLine("Failed to clean up \"" + fileInfo.FullName + "\" (is it locked or in use?)");
					}
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00022050 File Offset: 0x00020250
		private static List<string> PermuteArchitectureSpecificDirectoryNames(OSAndArchitecture platformInfo)
		{
			string text = platformInfo.OS.GetRuntimeIdString() + "-" + platformInfo.Architecture.GetRuntimeIdString();
			string[] inheritedRuntimeIds = NativePlatformUtils.GetInheritedRuntimeIds(text);
			List<string> list = new List<string>(inheritedRuntimeIds.Length + 1);
			list.Add(text);
			if (platformInfo.OS == PlatformOperatingSystem.Windows)
			{
				list.Add("win10-" + platformInfo.Architecture.GetRuntimeIdString());
				list.Add("win81-" + platformInfo.Architecture.GetRuntimeIdString());
				list.Add("win8-" + platformInfo.Architecture.GetRuntimeIdString());
				list.Add("win7-" + platformInfo.Architecture.GetRuntimeIdString());
				list.Add("win10");
				list.Add("win81");
				list.Add("win8");
				list.Add("win7");
			}
			list.AddRange(inheritedRuntimeIds);
			return list;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0002214C File Offset: 0x0002034C
		private static string LibraryNameWithoutExtension(string libraryName)
		{
			if (!libraryName.Contains("."))
			{
				return libraryName;
			}
			string text = libraryName.ToLowerInvariant();
			if (text.EndsWith(".dll") || text.EndsWith(".so") || text.EndsWith(".dylib"))
			{
				return libraryName.Substring(0, libraryName.LastIndexOf('.'));
			}
			return libraryName;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000221A8 File Offset: 0x000203A8
		private static string NormalizeLibraryName(string requestedName, OSAndArchitecture platformInfo)
		{
			string text = NativePlatformUtils.LibraryNameWithoutExtension(requestedName);
			if (platformInfo.OS == PlatformOperatingSystem.Windows)
			{
				return text + ".dll";
			}
			if (platformInfo.OS == PlatformOperatingSystem.Linux || platformInfo.OS == PlatformOperatingSystem.Android || platformInfo.OS == PlatformOperatingSystem.Linux_Bionic || platformInfo.OS == PlatformOperatingSystem.Linux_Musl || platformInfo.OS == PlatformOperatingSystem.Unix)
			{
				if (!text.StartsWith("lib", 4))
				{
					return "lib" + text + ".so";
				}
				return text + ".so";
			}
			else
			{
				if (platformInfo.OS != PlatformOperatingSystem.iOS && platformInfo.OS != PlatformOperatingSystem.iOS_Simulator && platformInfo.OS != PlatformOperatingSystem.MacOS && platformInfo.OS != PlatformOperatingSystem.MacCatalyst)
				{
					return requestedName;
				}
				if (!text.StartsWith("lib", 4))
				{
					return "lib" + text + ".dylib";
				}
				return text + ".dylib";
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00022288 File Offset: 0x00020488
		private static List<string> PermuteLibraryNames(string requestedName, OSAndArchitecture platformInfo)
		{
			List<string> list = new List<string>(16);
			string text = NativePlatformUtils.LibraryNameWithoutExtension(requestedName);
			if (platformInfo.OS == PlatformOperatingSystem.Windows)
			{
				list.Add(text + ".dll");
				list.Add("lib" + text + ".dll");
				if (platformInfo.Architecture == PlatformArchitecture.I386)
				{
					list.Add(text + "_x86.dll");
					list.Add("lib" + text + "_x86.dll");
					list.Add(text + "x86.dll");
					list.Add("lib" + text + "x86.dll");
					list.Add(text + "32.dll");
					list.Add("lib" + text + "32.dll");
					list.Add(text + "_32.dll");
					list.Add("lib" + text + "_32.dll");
					list.Add(text + "-32.dll");
					list.Add("lib" + text + "-32.dll");
				}
				if (platformInfo.Architecture == PlatformArchitecture.X64)
				{
					list.Add(text + "_x64.dll");
					list.Add("lib" + text + "_x64.dll");
					list.Add(text + "x64.dll");
					list.Add("lib" + text + "x64.dll");
					list.Add(text + "64.dll");
					list.Add("lib" + text + "64.dll");
					list.Add(text + "_64.dll");
					list.Add("lib" + text + "_64.dll");
					list.Add(text + "-64.dll");
					list.Add("lib" + text + "-64.dll");
				}
			}
			else if (platformInfo.OS == PlatformOperatingSystem.Linux || platformInfo.OS == PlatformOperatingSystem.Android || platformInfo.OS == PlatformOperatingSystem.Linux_Bionic || platformInfo.OS == PlatformOperatingSystem.Linux_Musl || platformInfo.OS == PlatformOperatingSystem.Unix)
			{
				list.Add(text + ".so");
				list.Add("lib" + text + ".so");
			}
			else if (platformInfo.OS == PlatformOperatingSystem.MacOS || platformInfo.OS == PlatformOperatingSystem.MacCatalyst || platformInfo.OS == PlatformOperatingSystem.iOS || platformInfo.OS == PlatformOperatingSystem.iOS_Simulator)
			{
				list.Add(text + ".dylib");
				list.Add("lib" + text + ".dylib");
			}
			else
			{
				list.Add(requestedName);
			}
			return list;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00022538 File Offset: 0x00020738
		internal static NativeLibraryStatus ProbeLibrary(string libName, OSAndArchitecture platformInfo, TextWriter logger)
		{
			try
			{
				if (platformInfo.OS == PlatformOperatingSystem.Windows)
				{
					IntPtr intPtr = IntPtr.Zero;
					try
					{
						if (logger != null)
						{
							logger.WriteLine("Attempting to load " + libName + " as a windows .dll");
						}
						KernelInteropWindows.GetLastError();
						intPtr = KernelInteropWindows.LoadLibraryExW(libName, IntPtr.Zero, 4096U);
						if (intPtr == IntPtr.Zero)
						{
							uint lastError = KernelInteropWindows.GetLastError();
							if (lastError != 0U)
							{
								uint num = 2147942400U | lastError;
								if (num == 2147942526U)
								{
									if (logger != null)
									{
										logger.WriteLine(string.Format("Win32 error 0x{0:X8}: File {1} not found", num, libName));
									}
								}
								else if (num == 2147942593U)
								{
									if (logger != null)
									{
										logger.WriteLine(string.Format("Win32 error 0x{0:X8}: Invalid binary format while loading library {1}", num, libName));
									}
								}
								else if (logger != null)
								{
									logger.WriteLine(string.Format("Win32 error 0x{0:X8} while loading library {1}", num, libName));
								}
							}
							else if (logger != null)
							{
								logger.WriteLine("Native library " + libName + " not found.");
							}
							return NativeLibraryStatus.Unavailable;
						}
						if (logger != null)
						{
							logger.WriteLine("Native library " + libName + " found!");
						}
						return NativeLibraryStatus.Available;
					}
					finally
					{
						if (intPtr != IntPtr.Zero)
						{
							KernelInteropWindows.FreeLibrary(intPtr);
						}
					}
				}
				if (platformInfo.OS == PlatformOperatingSystem.Linux || platformInfo.OS == PlatformOperatingSystem.Android || platformInfo.OS == PlatformOperatingSystem.Linux_Bionic || platformInfo.OS == PlatformOperatingSystem.Linux_Musl || platformInfo.OS == PlatformOperatingSystem.Unix)
				{
					IntPtr intPtr2 = IntPtr.Zero;
					try
					{
						if (logger != null)
						{
							logger.WriteLine("Attempting to load " + libName + " as a linux .so");
						}
						intPtr2 = KernelInteropLinux.dlopen(libName, 2);
						if (intPtr2 == IntPtr.Zero)
						{
							IntPtr intPtr3 = KernelInteropLinux.dlerror();
							if (intPtr3 != IntPtr.Zero)
							{
								string text = Marshal.PtrToStringAnsi(intPtr3);
								if (!string.IsNullOrEmpty(text))
								{
									if (logger != null)
									{
										logger.WriteLine(string.Format("Error while loading library {0}: {1}", libName, text));
									}
								}
								else if (logger != null)
								{
									logger.WriteLine(libName + " could not be loaded.");
								}
							}
							else if (logger != null)
							{
								logger.WriteLine(libName + " could not be loaded.");
							}
							return NativeLibraryStatus.Unavailable;
						}
						if (logger != null)
						{
							logger.WriteLine("Native library " + libName + " found!");
						}
						return NativeLibraryStatus.Available;
					}
					finally
					{
						if (intPtr2 != IntPtr.Zero)
						{
							KernelInteropLinux.dlclose(intPtr2);
						}
					}
				}
				if (platformInfo.OS == PlatformOperatingSystem.MacOS || platformInfo.OS == PlatformOperatingSystem.MacCatalyst || platformInfo.OS == PlatformOperatingSystem.iOS || platformInfo.OS == PlatformOperatingSystem.iOS_Simulator)
				{
					IntPtr intPtr4 = IntPtr.Zero;
					try
					{
						if (logger != null)
						{
							logger.WriteLine("Attempting to load " + libName + " as a macOS .dylib");
						}
						intPtr4 = KernelInteropMacOS.dlopen(libName, 2);
						if (intPtr4 == IntPtr.Zero)
						{
							IntPtr intPtr5 = KernelInteropMacOS.dlerror();
							if (intPtr5 != IntPtr.Zero)
							{
								string text2 = Marshal.PtrToStringAnsi(intPtr5);
								if (!string.IsNullOrEmpty(text2))
								{
									if (logger != null)
									{
										logger.WriteLine(string.Format("Error while loading library {0}: {1}", libName, text2));
									}
								}
								else if (logger != null)
								{
									logger.WriteLine(libName + " could not be loaded.");
								}
							}
							else if (logger != null)
							{
								logger.WriteLine(libName + " could not be loaded.");
							}
							return NativeLibraryStatus.Unavailable;
						}
						if (logger != null)
						{
							logger.WriteLine("Native library " + libName + " found!");
						}
						return NativeLibraryStatus.Available;
					}
					finally
					{
						if (intPtr4 != IntPtr.Zero)
						{
							KernelInteropMacOS.dlclose(intPtr4);
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (logger != null)
				{
					logger.WriteLine(ex);
				}
			}
			return NativeLibraryStatus.Unknown;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x000228CC File Offset: 0x00020ACC
		// Note: this type is marked as 'beforefieldinit'.
		static NativePlatformUtils()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			dictionary.Add("android", new string[]
			{
				"linux-bionic",
				"linux",
				"unix",
				"any"
			});
			dictionary.Add("android-arm", new string[]
			{
				"android",
				"linux-bionic-arm",
				"linux-bionic",
				"linux-arm",
				"linux",
				"unix-arm",
				"unix",
				"any"
			});
			dictionary.Add("android-arm64", new string[]
			{
				"android",
				"linux-bionic-arm64",
				"linux-bionic",
				"linux-arm64",
				"linux",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("android-x64", new string[]
			{
				"android",
				"linux-bionic-x64",
				"linux-bionic",
				"linux-x64",
				"linux",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("android-x86", new string[]
			{
				"android",
				"linux-bionic-x86",
				"linux-bionic",
				"linux-x86",
				"linux",
				"unix-x86",
				"unix",
				"any"
			});
			dictionary.Add("any", new string[0]);
			dictionary.Add("base", new string[0]);
			dictionary.Add("browser", new string[]
			{
				"any"
			});
			dictionary.Add("browser-wasm", new string[]
			{
				"browser",
				"any"
			});
			dictionary.Add("freebsd", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("freebsd-arm64", new string[]
			{
				"freebsd",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("freebsd-x64", new string[]
			{
				"freebsd",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("illumos", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("illumos-x64", new string[]
			{
				"illumos",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("ios", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("ios-arm", new string[]
			{
				"ios",
				"unix-arm",
				"unix",
				"any"
			});
			dictionary.Add("ios-arm64", new string[]
			{
				"ios",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("iossimulator", new string[]
			{
				"ios",
				"unix",
				"any"
			});
			dictionary.Add("iossimulator-arm64", new string[]
			{
				"iossimulator",
				"ios-arm64",
				"ios",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("iossimulator-x64", new string[]
			{
				"iossimulator",
				"ios-x64",
				"ios",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("iossimulator-x86", new string[]
			{
				"iossimulator",
				"ios-x86",
				"ios",
				"unix-x86",
				"unix",
				"any"
			});
			dictionary.Add("ios-x64", new string[]
			{
				"ios",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("ios-x86", new string[]
			{
				"ios",
				"unix-x86",
				"unix",
				"any"
			});
			dictionary.Add("linux", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("linux-arm", new string[]
			{
				"linux",
				"unix-arm",
				"unix",
				"any"
			});
			dictionary.Add("linux-arm64", new string[]
			{
				"linux",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("linux-armel", new string[]
			{
				"linux",
				"unix-armel",
				"unix",
				"any"
			});
			dictionary.Add("linux-armv6", new string[]
			{
				"linux",
				"unix-armv6",
				"unix",
				"any"
			});
			dictionary.Add("linux-bionic", new string[]
			{
				"linux",
				"unix",
				"any"
			});
			dictionary.Add("linux-bionic-arm", new string[]
			{
				"linux-bionic",
				"linux-arm",
				"linux",
				"unix-arm",
				"unix",
				"any"
			});
			dictionary.Add("linux-bionic-arm64", new string[]
			{
				"linux-bionic",
				"linux-arm64",
				"linux",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("linux-bionic-x64", new string[]
			{
				"linux-bionic",
				"linux-x64",
				"linux",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("linux-bionic-x86", new string[]
			{
				"linux-bionic",
				"linux-x86",
				"linux",
				"unix-x86",
				"unix",
				"any"
			});
			dictionary.Add("linux-loongarch64", new string[]
			{
				"linux",
				"unix-loongarch64",
				"unix",
				"any"
			});
			dictionary.Add("linux-mips64", new string[]
			{
				"linux",
				"unix-mips64",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl", new string[]
			{
				"linux",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-arm", new string[]
			{
				"linux-musl",
				"linux-arm",
				"linux",
				"unix-arm",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-arm64", new string[]
			{
				"linux-musl",
				"linux-arm64",
				"linux",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-armel", new string[]
			{
				"linux-musl",
				"linux-armel",
				"linux",
				"unix-armel",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-armv6", new string[]
			{
				"linux-musl",
				"linux-armv6",
				"linux",
				"unix-armv6",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-ppc64le", new string[]
			{
				"linux-musl",
				"linux-ppc64le",
				"linux",
				"unix-ppc64le",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-riscv64", new string[]
			{
				"linux-musl",
				"linux-riscv64",
				"linux",
				"unix-riscv64",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-s390x", new string[]
			{
				"linux-musl",
				"linux-s390x",
				"linux",
				"unix-s390x",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-x64", new string[]
			{
				"linux-musl",
				"linux-x64",
				"linux",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("linux-musl-x86", new string[]
			{
				"linux-musl",
				"linux-x86",
				"linux",
				"unix-x86",
				"unix",
				"any"
			});
			dictionary.Add("linux-ppc64le", new string[]
			{
				"linux",
				"unix-ppc64le",
				"unix",
				"any"
			});
			dictionary.Add("linux-riscv64", new string[]
			{
				"linux",
				"unix-riscv64",
				"unix",
				"any"
			});
			dictionary.Add("linux-s390x", new string[]
			{
				"linux",
				"unix-s390x",
				"unix",
				"any"
			});
			dictionary.Add("linux-x64", new string[]
			{
				"linux",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("linux-x86", new string[]
			{
				"linux",
				"unix-x86",
				"unix",
				"any"
			});
			dictionary.Add("maccatalyst", new string[]
			{
				"ios",
				"unix",
				"any"
			});
			dictionary.Add("maccatalyst-arm64", new string[]
			{
				"maccatalyst",
				"ios-arm64",
				"ios",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("maccatalyst-x64", new string[]
			{
				"maccatalyst",
				"ios-x64",
				"ios",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("osx", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("osx-arm64", new string[]
			{
				"osx",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("osx-x64", new string[]
			{
				"osx",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("solaris", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("solaris-x64", new string[]
			{
				"solaris",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("tvos", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("tvos-arm64", new string[]
			{
				"tvos",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("tvossimulator", new string[]
			{
				"tvos",
				"unix",
				"any"
			});
			dictionary.Add("tvossimulator-arm64", new string[]
			{
				"tvossimulator",
				"tvos-arm64",
				"tvos",
				"unix-arm64",
				"unix",
				"any"
			});
			dictionary.Add("tvossimulator-x64", new string[]
			{
				"tvossimulator",
				"tvos-x64",
				"tvos",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("tvos-x64", new string[]
			{
				"tvos",
				"unix-x64",
				"unix",
				"any"
			});
			dictionary.Add("unix", new string[]
			{
				"any"
			});
			dictionary.Add("unix-arm", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-arm64", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-armel", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-armv6", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-loongarch64", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-mips64", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-ppc64le", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-riscv64", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-s390x", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-x64", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("unix-x86", new string[]
			{
				"unix",
				"any"
			});
			dictionary.Add("wasi", new string[]
			{
				"any"
			});
			dictionary.Add("wasi-wasm", new string[]
			{
				"wasi",
				"any"
			});
			dictionary.Add("win", new string[]
			{
				"any"
			});
			dictionary.Add("win-arm", new string[]
			{
				"win",
				"any"
			});
			dictionary.Add("win-arm64", new string[]
			{
				"win",
				"any"
			});
			dictionary.Add("win-x64", new string[]
			{
				"win",
				"any"
			});
			dictionary.Add("win-x86", new string[]
			{
				"win",
				"any"
			});
			NativePlatformUtils.RidInheritanceMappings = dictionary;
		}

		// Token: 0x040003CC RID: 972
		private static bool _cachedPlatformInfoExists = false;

		// Token: 0x040003CD RID: 973
		private static OSAndArchitecture _cachedPlatformInfo = new OSAndArchitecture(PlatformOperatingSystem.Unknown, PlatformArchitecture.Unknown);

		// Token: 0x040003CE RID: 974
		private static readonly IDictionary<string, NativeLibraryStatus> _loadedLibraries = new Dictionary<string, NativeLibraryStatus>();

		// Token: 0x040003CF RID: 975
		private static readonly object _mutex = new object();

		// Token: 0x040003D0 RID: 976
		private static readonly IReadOnlyDictionary<string, string[]> RidInheritanceMappings;
	}
}
