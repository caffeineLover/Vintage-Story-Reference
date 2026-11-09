using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui.Debug
{
	// Token: 0x02000004 RID: 4
	[NullableContext(1)]
	[Nullable(0)]
	public static class DebugWindowsManager
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002A1B File Offset: 0x00000C1B
		public static Dictionary<string, Dictionary<int, DebugWindowsManager.DrawEntry>> DrawEntries { get; } = new Dictionary<string, Dictionary<int, DebugWindowsManager.DrawEntry>>();

		// Token: 0x0600001B RID: 27 RVA: 0x00002A24 File Offset: 0x00000C24
		public static bool Draw()
		{
			bool result = false;
			foreach (KeyValuePair<string, HashSet<string>> keyValuePair in DebugWindowsManager.DrawEntry._categories)
			{
				string text;
				HashSet<string> hashSet;
				keyValuePair.Deconstruct(ref text, ref hashSet);
				string text2 = text;
				HashSet<string> categories = hashSet;
				ImGui.Begin(MemoryExtensions.AsSpan(text2));
				ImGui.BeginTabBar(MemoryExtensions.AsSpan(text2));
				DebugWindowsManager.DrawCategories(text2, categories, ref result);
				ImGui.EndTabBar();
				ImGui.End();
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002AAC File Offset: 0x00000CAC
		private static void DrawCategories(string domain, HashSet<string> categories, ref bool anyDrawn)
		{
			foreach (string category2 in Enumerable.Where<string>(categories, (string category) => ImGui.BeginTabItem(MemoryExtensions.AsSpan(category))))
			{
				DebugWindowsManager.DrawCategory(domain, category2, ref anyDrawn);
				ImGui.EndTabItem();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B20 File Offset: 0x00000D20
		private static void DrawCategory(string domain, string category, ref bool anyDrawn)
		{
			IEnumerable<KeyValuePair<int, DebugWindowsManager.DrawEntry>> enumerable = DebugWindowsManager.DrawEntries[domain];
			Func<KeyValuePair<int, DebugWindowsManager.DrawEntry>, bool> <>9__0;
			Func<KeyValuePair<int, DebugWindowsManager.DrawEntry>, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ((KeyValuePair<int, DebugWindowsManager.DrawEntry> entry) => entry.Value.Category == category));
			}
			foreach (DebugWindowsManager.DrawEntry drawEntry in Enumerable.Select<KeyValuePair<int, DebugWindowsManager.DrawEntry>, DebugWindowsManager.DrawEntry>(Enumerable.Where<KeyValuePair<int, DebugWindowsManager.DrawEntry>>(enumerable, func), (KeyValuePair<int, DebugWindowsManager.DrawEntry> entry) => entry.Value))
			{
				drawEntry.InvokeDrawCallback();
				anyDrawn = true;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002BC8 File Offset: 0x00000DC8
		public static void Clear()
		{
			DebugWindowsManager.DrawEntries.Clear();
			DebugWindowsManager.DrawEntry._categories.Clear();
		}

		// Token: 0x0200001F RID: 31
		[Nullable(0)]
		public class DrawEntry
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000074 RID: 116 RVA: 0x00003FAD File Offset: 0x000021AD
			public string Domain { get; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000075 RID: 117 RVA: 0x00003FB5 File Offset: 0x000021B5
			public string Category { get; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000076 RID: 118 RVA: 0x00003FBD File Offset: 0x000021BD
			public Action Delegate { get; }

			// Token: 0x06000077 RID: 119 RVA: 0x00003FC8 File Offset: 0x000021C8
			public DrawEntry(string domain, string category, Action drawDelegate)
			{
				this.Domain = domain;
				this.Category = category;
				this.Delegate = drawDelegate;
				if (!DebugWindowsManager.DrawEntry._categories.ContainsKey(domain))
				{
					DebugWindowsManager.DrawEntry._categories[domain] = new HashSet<string>();
				}
				if (!DebugWindowsManager.DrawEntry._categories[domain].Contains(category))
				{
					DebugWindowsManager.DrawEntry._categories[domain].Add(category);
				}
			}

			// Token: 0x06000078 RID: 120 RVA: 0x00004032 File Offset: 0x00002232
			public virtual void InvokeDrawCallback()
			{
				Action @delegate = this.Delegate;
				if (@delegate == null)
				{
					return;
				}
				@delegate.Invoke();
			}

			// Token: 0x0400004D RID: 77
			internal static readonly Dictionary<string, HashSet<string>> _categories = new Dictionary<string, HashSet<string>>();
		}
	}
}
