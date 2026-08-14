using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Common;

namespace VSImGui.Debug
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	internal static class AssetLocationEditor
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00003194 File Offset: 0x00001394
		public static void Edit(string title, ref AssetLocation value, AssetLocation defaultValue)
		{
			try
			{
				AssetLocation assetLocation = value;
				if (assetLocation == null || !assetLocation.Valid)
				{
					value = defaultValue;
				}
			}
			catch (Exception)
			{
				value = defaultValue;
			}
			string domain = value.Domain;
			string text = Enumerable.Aggregate<string>(Enumerable.Where<string>(value.Path.Split('/', 0), (string _, int index) => index > 0), (string first, string second) => first + "/" + second);
			AssetCategory category = value.Category;
			ImGui.InputText(MemoryExtensions.AsSpan("Domain##" + title), ref domain, 256U);
			ImGui.InputText(MemoryExtensions.AsSpan("Path##" + title), ref text, 2048U);
			AssetCategoryEditor.Combo("Category##" + title, ref category);
			string text2 = category.Code + "/" + text;
			value = new AssetLocation(domain, text2);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000032A0 File Offset: 0x000014A0
		public static AssetLocation Edit(string title, AssetLocation value, AssetLocation defaultValue)
		{
			AssetLocationEditor.Edit(title, ref value, defaultValue);
			return value;
		}
	}
}
