using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace QuestPDF.Drawing
{
	// Token: 0x0200010F RID: 271
	[NullableContext(1)]
	[Nullable(0)]
	public static class FontManager
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x000184BC File Offset: 0x000166BC
		internal static SkTypefaceProvider TypefaceProvider { get; } = new SkTypefaceProvider();

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x000184C3 File Offset: 0x000166C3
		internal static SkFontManager CurrentFontManager
		{
			get
			{
				if (!Settings.UseEnvironmentFonts)
				{
					return SkFontManager.Local;
				}
				return SkFontManager.Global;
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000184D7 File Offset: 0x000166D7
		static FontManager()
		{
			SkNativeDependencyCompatibilityChecker.Test();
			FontManager.RegisterLibraryDefaultFonts();
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x000184ED File Offset: 0x000166ED
		[Obsolete("Since version 2022.8 this method has been renamed. Please use the RegisterFontWithCustomName method.")]
		[ExcludeFromCodeCoverage]
		public static void RegisterFontType(string fontName, Stream stream)
		{
			FontManager.RegisterFontWithCustomName(fontName, stream);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000184F8 File Offset: 0x000166F8
		public static void RegisterFontWithCustomName(string fontName, Stream stream)
		{
			using (SkData skData = SkData.FromStream(stream))
			{
				FontManager.TypefaceProvider.AddTypefaceFromData(skData, null);
				FontManager.TypefaceProvider.AddTypefaceFromData(skData, fontName);
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00018540 File Offset: 0x00016740
		public static void RegisterFont(Stream stream)
		{
			using (SkData skData = SkData.FromStream(stream))
			{
				FontManager.TypefaceProvider.AddTypefaceFromData(skData, null);
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001857C File Offset: 0x0001677C
		public static void RegisterFontFromEmbeddedResource(string pathName)
		{
			using (Stream manifestResourceStream = Assembly.GetCallingAssembly().GetManifestResourceStream(pathName))
			{
				if (manifestResourceStream == null)
				{
					throw new ArgumentException("Cannot load font file from an embedded resource. Please make sure that the resource is available or the path is correct: " + pathName);
				}
				FontManager.RegisterFont(manifestResourceStream);
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x000185CC File Offset: 0x000167CC
		private static void RegisterLibraryDefaultFonts()
		{
			foreach (string text in FontManager.<RegisterLibraryDefaultFonts>g__SearchFontFiles|10_0())
			{
				try
				{
					using (FileStream fileStream = File.OpenRead(text))
					{
						FontManager.RegisterFont(fileStream);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00018648 File Offset: 0x00016848
		[CompilerGenerated]
		internal static ICollection<string> <RegisterLibraryDefaultFonts>g__SearchFontFiles|10_0()
		{
			IEnumerable<string> fontDiscoveryPaths = Settings.FontDiscoveryPaths;
			Func<string, bool> func;
			if ((func = FontManager.<>O.<0>__Exists) == null)
			{
				func = (FontManager.<>O.<0>__Exists = new Func<string, bool>(Directory.Exists));
			}
			List<string> list = Enumerable.ToList<string>(Enumerable.Take<string>(Enumerable.SelectMany<ICollection<string>, string>(Enumerable.Select<string, ICollection<string>>(Enumerable.Where<string>(fontDiscoveryPaths, func), new Func<string, ICollection<string>>(FontManager.<RegisterLibraryDefaultFonts>g__TryEnumerateFiles|10_3)), (ICollection<string> file) => file), 100000));
			if (list.Count == 100000)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(264, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The library has reached the limit of ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(100000);
				defaultInterpolatedStringHandler.AppendLiteral(" files to scan for font files. Please adjust the ");
				defaultInterpolatedStringHandler.AppendFormatted("FontDiscoveryPaths");
				defaultInterpolatedStringHandler.AppendLiteral(" collection to include only the necessary directories. The reason of this exception is to prevent scanning too many files and avoid performance issues on the application startup.");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string[] supportedFontExtensions = new string[]
			{
				".ttf",
				".otf",
				".ttc",
				".pfb"
			};
			return Enumerable.ToList<string>(Enumerable.Where<string>(list, (string x) => Enumerable.Contains<string>(supportedFontExtensions, Path.GetExtension(x).ToLowerInvariant())));
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001876C File Offset: 0x0001696C
		[CompilerGenerated]
		internal static ICollection<string> <RegisterLibraryDefaultFonts>g__TryEnumerateFiles|10_3(string path)
		{
			ICollection<string> result;
			try
			{
				result = Enumerable.ToArray<string>(Enumerable.Take<string>(Directory.EnumerateFiles(path, "*.*", 1), 100000));
			}
			catch
			{
				result = Array.Empty<string>();
			}
			return result;
		}

		// Token: 0x020001F0 RID: 496
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000747 RID: 1863
			[Nullable(0)]
			public static Func<string, bool> <0>__Exists;
		}
	}
}
