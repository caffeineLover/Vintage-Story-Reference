using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Common;

namespace VSImGui
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	public static class AssetLocationEditor
	{
		// Token: 0x06000252 RID: 594 RVA: 0x0000C8C8 File Offset: 0x0000AAC8
		public static void Edit(string title, ref AssetLocation value, [Nullable(2)] AssetLocation defaultValue)
		{
			try
			{
				if (!value.Valid && defaultValue != null)
				{
					value = defaultValue.Clone();
				}
			}
			catch
			{
				if (defaultValue == null)
				{
					ImGui.Text(MemoryExtensions.AsSpan(title + ": invalid"));
					return;
				}
				value = defaultValue.Clone();
			}
			string domain = value.Domain;
			string path = Enumerable.Aggregate<string>(Enumerable.Where<string>(value.Path.Split('/', 0), (string _, int index) => index > 0), (string first, string second) => first + "/" + second);
			AssetCategory category = value.Category;
			ImGui.InputText(MemoryExtensions.AsSpan("Domain##" + title), ref domain, 256U);
			ImGui.InputText(MemoryExtensions.AsSpan("Path##" + title), ref path, 2048U);
			AssetCategoryEditor.Combo("Category##" + title, ref category);
			value.Domain = domain;
			value.Path = category.Code + "/" + path;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000CA04 File Offset: 0x0000AC04
		public static void Edit(string title, AssetLocation value)
		{
			AssetLocationEditor.Edit(title, ref value, null);
		}
	}
}
