using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace VSImGui.Debug
{
	// Token: 0x02000009 RID: 9
	[NullableContext(1)]
	[Nullable(0)]
	internal static class AssetCategoryEditor
	{
		// Token: 0x0600003E RID: 62 RVA: 0x000032AC File Offset: 0x000014AC
		public static void Combo(string title, ref AssetCategory value)
		{
			string code = value.Code;
			Dictionary<string, AssetCategory> categories = AssetCategory.categories;
			string[] array = Enumerable.ToArray<string>(Enumerable.Select<KeyValuePair<string, AssetCategory>, string>(categories, (KeyValuePair<string, AssetCategory> entry, int _) => entry.Key));
			int num = ArrayExtensions.IndexOf<string>(array, code);
			ImGui.Combo(MemoryExtensions.AsSpan(title), ref num, array, array.Length);
			value = categories[array[num]];
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003317 File Offset: 0x00001517
		public static AssetCategory Combo(string title, AssetCategory value)
		{
			AssetCategoryEditor.Combo(title, ref value);
			return value;
		}
	}
}
