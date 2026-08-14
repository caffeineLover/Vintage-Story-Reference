using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Config;

namespace VSImGui.API
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	public static class FontManager
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060002A1 RID: 673 RVA: 0x0000D7B8 File Offset: 0x0000B9B8
		// (remove) Token: 0x060002A2 RID: 674 RVA: 0x0000D7EC File Offset: 0x0000B9EC
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public static event FontManager.FontsLoadingDelegate BeforeFontsLoaded;

		// Token: 0x060002A3 RID: 675 RVA: 0x0000D81F File Offset: 0x0000BA1F
		[return: TupleElementNames(new string[]
		{
			"font",
			"size"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		public static IEnumerable<ValueTuple<string, int>> GetLoadedFonts()
		{
			return Enumerable.Select<KeyValuePair<ValueTuple<string, int>, ImFontPtr>, ValueTuple<string, int>>(FontManager.Loaded, (KeyValuePair<ValueTuple<string, int>, ImFontPtr> entry) => new ValueTuple<string, int>(entry.Key.Item1, entry.Key.Item2));
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000D84A File Offset: 0x0000BA4A
		internal static List<int> Sizes { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000D851 File Offset: 0x0000BA51
		internal static HashSet<string> Fonts { get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000D858 File Offset: 0x0000BA58
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		internal static Dictionary<ValueTuple<string, int>, ImFontPtr> Loaded { [return: Nullable(new byte[]
		{
			1,
			0,
			1
		})] get; }

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D860 File Offset: 0x0000BA60
		internal static void Load()
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (int num in FontManager.Sizes)
			{
				hashSet.Add(num);
			}
			HashSet<int> sizes = hashSet;
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (string text in FontManager.Fonts)
			{
				hashSet2.Add(text);
			}
			HashSet<string> fonts = hashSet2;
			FontManager.FontsLoadingDelegate beforeFontsLoaded = FontManager.BeforeFontsLoaded;
			if (beforeFontsLoaded != null)
			{
				beforeFontsLoaded(fonts, sizes);
			}
			FontManager.Loaded.Clear();
			FontManager.LoadDefault();
			ImGuiIOPtr io = ImGui.GetIO();
			foreach (string font in fonts)
			{
				foreach (int size in sizes)
				{
					IntPtr glyphRange = FontManager.GlyphRanges.ContainsKey(Lang.CurrentLocale) ? FontManager.GlyphRanges[Lang.CurrentLocale] : FontManager.GlyphRanges["default"];
					ImFontPtr ptr = io.Fonts.AddFontFromFileTTF(MemoryExtensions.AsSpan(font), (float)size, default(ImFontConfigPtr), glyphRange);
					FontManager.Loaded.TryAdd(new ValueTuple<string, int>(Path.GetFileNameWithoutExtension(font), size), ptr);
				}
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000DA24 File Offset: 0x0000BC24
		private static void LoadDefault()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			IntPtr glyphRange = FontManager.GlyphRanges.ContainsKey(Lang.CurrentLocale) ? FontManager.GlyphRanges[Lang.CurrentLocale] : FontManager.GlyphRanges["default"];
			io.Fonts.AddFontFromFileTTF(MemoryExtensions.AsSpan(FontManager._defaultFont), (float)FontManager._defaultSize, default(ImFontConfigPtr), glyphRange);
			io.Fonts.AddFontDefault();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000DAA4 File Offset: 0x0000BCA4
		// Note: this type is marked as 'beforefieldinit'.
		static FontManager()
		{
			List<int> list = new List<int>();
			list.Add(6);
			list.Add(8);
			list.Add(10);
			list.Add(14);
			list.Add(18);
			list.Add(24);
			list.Add(30);
			list.Add(36);
			list.Add(48);
			list.Add(60);
			FontManager.Sizes = list;
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Bold.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-BoldItalic.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Italic.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Almendra-Regular.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Bold.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-BoldItalic.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Italic.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Lora-Regular.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Bold.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Italic.ttf"));
			hashSet.Add(Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Regular.ttf"));
			FontManager.Fonts = hashSet;
			Dictionary<string, IntPtr> dictionary = new Dictionary<string, IntPtr>();
			dictionary.Add("default", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("en", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("ar", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("be", ImGui.GetIO().Fonts.GetGlyphRangesCyrillic());
			dictionary.Add("nl", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("cs", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("fr", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("de", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("eo", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("it", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("ja", ImGui.GetIO().Fonts.GetGlyphRangesJapanese());
			dictionary.Add("ko", ImGui.GetIO().Fonts.GetGlyphRangesKorean());
			dictionary.Add("pl", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("pt-pt", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("pt-br", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("ru", ImGui.GetIO().Fonts.GetGlyphRangesCyrillic());
			dictionary.Add("sr", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("es-es", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("es-419", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("sk", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("sv-se", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("th", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			dictionary.Add("uk", ImGui.GetIO().Fonts.GetGlyphRangesCyrillic());
			dictionary.Add("zh-cn", ImGui.GetIO().Fonts.GetGlyphRangesChineseFull());
			dictionary.Add("tr", ImGui.GetIO().Fonts.GetGlyphRangesDefault());
			FontManager.GlyphRanges = dictionary;
			FontManager.Loaded = new Dictionary<ValueTuple<string, int>, ImFontPtr>();
			FontManager._defaultFont = Path.Combine(GamePaths.AssetsPath, "game", "fonts", "Montserrat-Regular.ttf");
			FontManager._defaultSize = 18;
		}

		// Token: 0x0400005B RID: 91
		private static Dictionary<string, IntPtr> GlyphRanges;

		// Token: 0x0400005D RID: 93
		private static readonly string _defaultFont;

		// Token: 0x0400005E RID: 94
		private static readonly int _defaultSize;

		// Token: 0x02000037 RID: 55
		// (Invoke) Token: 0x060002CA RID: 714
		[NullableContext(0)]
		public delegate void FontsLoadingDelegate(HashSet<string> fonts, HashSet<int> sizes);
	}
}
