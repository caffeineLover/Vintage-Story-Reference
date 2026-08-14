using System;
using System.Runtime.InteropServices;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000049 RID: 73
	public class OpenALLibraryNameContainer
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000072F5 File Offset: 0x000054F5
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000072FC File Offset: 0x000054FC
		public static string OverridePath { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00007304 File Offset: 0x00005504
		public string Windows
		{
			get
			{
				return "openal32.dll";
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000FF RID: 255 RVA: 0x0000730B File Offset: 0x0000550B
		public string Linux
		{
			get
			{
				return "libopenal.so.1";
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00007312 File Offset: 0x00005512
		public string MacOS
		{
			get
			{
				return "/System/Library/Frameworks/OpenAL.framework/OpenAL";
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00007319 File Offset: 0x00005519
		public string Android
		{
			get
			{
				return "libopenal.so";
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00007320 File Offset: 0x00005520
		public string IOS
		{
			get
			{
				return this.MacOS;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007328 File Offset: 0x00005528
		public string GetLibraryName()
		{
			if (OpenALLibraryNameContainer.OverridePath != null)
			{
				return OpenALLibraryNameContainer.OverridePath;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID")))
			{
				return this.Android;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return this.Linux;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
			{
				return this.Linux;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return this.Windows;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("IOS")))
			{
				return this.IOS;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return this.MacOS;
			}
			throw new NotSupportedException("The library name couldn't be resolved for the given platform ('" + RuntimeInformation.OSDescription + "').");
		}
	}
}
