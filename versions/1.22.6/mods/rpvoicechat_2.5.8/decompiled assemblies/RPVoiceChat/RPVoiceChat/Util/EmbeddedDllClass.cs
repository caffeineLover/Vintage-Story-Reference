using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vintagestory.API.Config;

namespace RPVoiceChat.Util
{
	// Token: 0x02000025 RID: 37
	public class EmbeddedDllClass
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x000071D0 File Offset: 0x000053D0
		public static void ExtractEmbeddedDlls()
		{
			if (RuntimeEnv.OS != null)
			{
				return;
			}
			string[] manifestResourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
			for (int i = 0; i < manifestResourceNames.Length; i++)
			{
				EmbeddedDllClass.ExtractEmbeddedDll(manifestResourceNames[i]);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00007208 File Offset: 0x00005408
		public static void ExtractEmbeddedDll(string resourceName)
		{
			if (RuntimeEnv.OS != null)
			{
				return;
			}
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AssemblyName assemblyName = executingAssembly.GetName();
			if (EmbeddedDllClass.tempFolder == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(assemblyName.Name);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				defaultInterpolatedStringHandler.AppendFormatted<Version>(assemblyName.Version);
				EmbeddedDllClass.tempFolder = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			byte[] resourceBytes;
			using (Stream stream = executingAssembly.GetManifestResourceStream(resourceName))
			{
				MemoryStream memoryStream = new MemoryStream();
				stream.CopyTo(memoryStream);
				resourceBytes = memoryStream.ToArray();
			}
			string dirName = Path.Combine(Path.GetTempPath(), EmbeddedDllClass.tempFolder);
			if (!Directory.Exists(dirName))
			{
				Directory.CreateDirectory(dirName);
			}
			string[] resourceNameParts = resourceName.Split(".", 0);
			string dllName = resourceNameParts[resourceNameParts.Length - 2] + "." + resourceNameParts[resourceNameParts.Length - 1];
			string dllPath = Path.Combine(dirName, dllName);
			bool alreadyExtracted = false;
			if (File.Exists(dllPath))
			{
				byte[] existingResource = File.ReadAllBytes(dllPath);
				alreadyExtracted = Enumerable.SequenceEqual<byte>(resourceBytes, existingResource);
			}
			if (alreadyExtracted)
			{
				return;
			}
			File.WriteAllBytes(dllPath, resourceBytes);
		}

		// Token: 0x060000E5 RID: 229
		[DllImport("kernel32", CharSet = 3, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string fileName);

		// Token: 0x060000E6 RID: 230 RVA: 0x00007328 File Offset: 0x00005528
		public static void LoadDll(string dllName)
		{
			if (RuntimeEnv.OS != null)
			{
				return;
			}
			if (EmbeddedDllClass.tempFolder == null)
			{
				throw new Exception("Cannot load embedded dlls before extracting them");
			}
			if (EmbeddedDllClass.LoadLibrary(Path.Combine(Path.GetTempPath(), EmbeddedDllClass.tempFolder, dllName)) == IntPtr.Zero)
			{
				Exception innerException = new Win32Exception();
				Exception e = new DllNotFoundException("Unable to load library: " + dllName + " from " + EmbeddedDllClass.tempFolder, innerException);
				Logger client = Logger.client;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to load embedded DLL:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
				client.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0400005C RID: 92
		private static string tempFolder;
	}
}
