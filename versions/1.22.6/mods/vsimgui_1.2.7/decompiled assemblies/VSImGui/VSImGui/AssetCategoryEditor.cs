using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace VSImGui
{
	// Token: 0x0200001C RID: 28
	[NullableContext(1)]
	[Nullable(0)]
	public static class AssetCategoryEditor
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000CA10 File Offset: 0x0000AC10
		public static void Combo(string title, ref AssetCategory value)
		{
			string code = value.Code;
			Dictionary<string, AssetCategory> categories = AssetCategory.categories;
			string[] codes = Enumerable.ToArray<string>(Enumerable.Select<KeyValuePair<string, AssetCategory>, string>(categories, (KeyValuePair<string, AssetCategory> entry, int _) => entry.Key));
			int index = ArrayExtensions.IndexOf<string>(codes, code);
			ImGui.Combo(MemoryExtensions.AsSpan(title), ref index, codes, codes.Length);
			value = categories[codes[index]];
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000CA7B File Offset: 0x0000AC7B
		public static AssetCategory Combo(string title, AssetCategory value)
		{
			AssetCategoryEditor.Combo(title, ref value);
			return value;
		}
	}
}
