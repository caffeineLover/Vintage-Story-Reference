using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui
{
	// Token: 0x02000018 RID: 24
	public static class EnumEditor<TEnum> where TEnum : struct, Enum
	{
		// Token: 0x06000234 RID: 564 RVA: 0x0000C260 File Offset: 0x0000A460
		public static TEnum Combo([Nullable(1)] string title, TEnum value)
		{
			int index = EnumEditor<TEnum>._keys[value];
			ImGui.Combo(MemoryExtensions.AsSpan(title), ref index, EnumEditor<TEnum>._names, EnumEditor<TEnum>._names.Length);
			return EnumEditor<TEnum>._values[index];
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
		public static TEnum Combo([Nullable(1)] string title, TEnum value, ref bool modified)
		{
			TEnum newValue = EnumEditor<TEnum>.Combo(title, value);
			if (EnumEditor<TEnum>._keys[newValue] != EnumEditor<TEnum>._keys[value])
			{
				modified = true;
			}
			return newValue;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C2D4 File Offset: 0x0000A4D4
		public static bool Combo([Nullable(1)] string title, ref TEnum value)
		{
			bool modified = false;
			value = EnumEditor<TEnum>.Combo(title, value, ref modified);
			return modified;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C2F8 File Offset: 0x0000A4F8
		public static TEnum Combo([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value)
		{
			int index = EnumEditor<TEnum>._keys[value];
			IEnumerable<string> names = EnumEditor<TEnum>._names;
			IEnumerable<string> filtered;
			IEnumerable<int> indexes;
			int selection;
			EditorsUtils.FilterElements(filter, names, out filtered, out indexes, ref index, out selection);
			ImGui.Combo(MemoryExtensions.AsSpan(title), ref selection, Enumerable.ToArray<string>(filtered), Enumerable.Count<string>(filtered));
			return EnumEditor<TEnum>._values[Enumerable.ToArray<int>(indexes)[selection]];
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000C358 File Offset: 0x0000A558
		public static TEnum Combo([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value, ref bool modified)
		{
			TEnum newValue = EnumEditor<TEnum>.Combo(title, filter, value);
			if (EnumEditor<TEnum>._keys[newValue] != EnumEditor<TEnum>._keys[value])
			{
				modified = true;
			}
			return newValue;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000C38C File Offset: 0x0000A58C
		[NullableContext(1)]
		public static bool Combo(string title, string filter, [Nullable(0)] ref TEnum value)
		{
			bool modified = false;
			value = EnumEditor<TEnum>.Combo(title, filter, value, ref modified);
			return modified;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		public static TEnum List([Nullable(1)] string title, TEnum value)
		{
			int index = EnumEditor<TEnum>._keys[value];
			ImGui.ListBox(MemoryExtensions.AsSpan(title), ref index, EnumEditor<TEnum>._names, EnumEditor<TEnum>._names.Length);
			return EnumEditor<TEnum>._values[index];
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		public static TEnum List([Nullable(1)] string title, TEnum value, ref bool modified)
		{
			TEnum newValue = EnumEditor<TEnum>.List(title, value);
			if (EnumEditor<TEnum>._keys[newValue] != EnumEditor<TEnum>._keys[value])
			{
				modified = true;
			}
			return newValue;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000C428 File Offset: 0x0000A628
		public static bool List([Nullable(1)] string title, ref TEnum value)
		{
			bool modified = false;
			value = EnumEditor<TEnum>.List(title, value, ref modified);
			return modified;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000C44C File Offset: 0x0000A64C
		public static TEnum List([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value)
		{
			int index = EnumEditor<TEnum>._keys[value];
			IEnumerable<string> names = EnumEditor<TEnum>._names;
			IEnumerable<string> filtered;
			IEnumerable<int> indexes;
			int selection;
			EditorsUtils.FilterElements(filter, names, out filtered, out indexes, ref index, out selection);
			ImGui.ListBox(MemoryExtensions.AsSpan(title), ref selection, Enumerable.ToArray<string>(filtered), Enumerable.Count<string>(filtered));
			return EnumEditor<TEnum>._values[Enumerable.ToArray<int>(indexes)[selection]];
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public static TEnum List([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value, ref bool modified)
		{
			TEnum newValue = EnumEditor<TEnum>.List(title, filter, value);
			if (EnumEditor<TEnum>._keys[newValue] != EnumEditor<TEnum>._keys[value])
			{
				modified = true;
			}
			return newValue;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
		[NullableContext(1)]
		public static bool List(string title, string filter, [Nullable(0)] ref TEnum value)
		{
			bool modified = false;
			value = EnumEditor<TEnum>.List(title, filter, value, ref modified);
			return modified;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C505 File Offset: 0x0000A705
		[NullableContext(1)]
		[return: Nullable(0)]
		public static TEnum ComboWithFilter(string title, string filterHolder, [Nullable(0)] TEnum value, uint maxFilterLength = 256U, string hint = "supports wildcards")
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter##" + title), MemoryExtensions.AsSpan(hint), ref filterHolder, maxFilterLength);
			return EnumEditor<TEnum>.Combo(title, filterHolder, value);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C52F File Offset: 0x0000A72F
		[NullableContext(1)]
		[return: Nullable(0)]
		public static TEnum ListWithFilter(string title, string filterHolder, [Nullable(0)] TEnum value, uint maxFilterLength = 256U, string hint = "supports wildcards")
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter##" + title), MemoryExtensions.AsSpan(hint), ref filterHolder, maxFilterLength);
			return EnumEditor<TEnum>.List(title, filterHolder, value);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C55C File Offset: 0x0000A75C
		// Note: this type is marked as 'beforefieldinit'.
		static EnumEditor()
		{
			EnumEditor<TEnum>._names = Enum.GetNames<TEnum>();
			EnumEditor<TEnum>._values = Enum.GetValues<TEnum>();
			EnumEditor<TEnum>._keys = Enumerable.ToDictionary(Enumerable.Select(EnumEditor<TEnum>._values, new Func<TEnum, int, <>f__AnonymousType0<TEnum, int>>(EnumEditor<TEnum>.<>c.<>9.<.cctor>b__17_0)), new Func<<>f__AnonymousType0<TEnum, int>, TEnum>(EnumEditor<TEnum>.<>c.<>9.<.cctor>b__17_1), new Func<<>f__AnonymousType0<TEnum, int>, int>(EnumEditor<TEnum>.<>c.<>9.<.cctor>b__17_2));
		}

		// Token: 0x04000043 RID: 67
		[Nullable(1)]
		private static readonly string[] _names;

		// Token: 0x04000044 RID: 68
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private static readonly TEnum[] _values;

		// Token: 0x04000045 RID: 69
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private static readonly Dictionary<TEnum, int> _keys;
	}
}
