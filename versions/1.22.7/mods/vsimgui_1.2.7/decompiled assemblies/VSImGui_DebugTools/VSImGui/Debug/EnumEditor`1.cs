using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui.Debug
{
	// Token: 0x02000005 RID: 5
	internal static class EnumEditor<TEnum> where TEnum : struct, Enum
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002BEC File Offset: 0x00000DEC
		public static TEnum Combo([Nullable(1)] string title, TEnum value)
		{
			int num = EnumEditor<TEnum>.mKeys[value];
			ImGui.Combo(MemoryExtensions.AsSpan(title), ref num, EnumEditor<TEnum>.mNames, EnumEditor<TEnum>.mNames.Length);
			return EnumEditor<TEnum>.mValues[num];
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C2C File Offset: 0x00000E2C
		public static TEnum Combo([Nullable(1)] string title, TEnum value, ref bool modified)
		{
			TEnum tenum = EnumEditor<TEnum>.Combo(title, value);
			modified = (EnumEditor<TEnum>.mKeys[tenum] == EnumEditor<TEnum>.mKeys[value]);
			return tenum;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C5C File Offset: 0x00000E5C
		public static bool Combo([Nullable(1)] string title, ref TEnum value)
		{
			bool result = false;
			value = EnumEditor<TEnum>.Combo(title, value, ref result);
			return result;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C80 File Offset: 0x00000E80
		public static TEnum Combo([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value)
		{
			int num = EnumEditor<TEnum>.mKeys[value];
			IEnumerable<string> enumerable = EnumEditor<TEnum>.mNames;
			IEnumerable<string> enumerable2;
			IEnumerable<int> enumerable3;
			int num2;
			EditorsUtils.FilterElements(filter, enumerable, out enumerable2, out enumerable3, ref num, out num2);
			ImGui.Combo(MemoryExtensions.AsSpan(title), ref num2, Enumerable.ToArray<string>(enumerable2), Enumerable.Count<string>(enumerable2));
			return EnumEditor<TEnum>.mValues[Enumerable.ToArray<int>(enumerable3)[num2]];
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public static TEnum Combo([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value, ref bool modified)
		{
			TEnum tenum = EnumEditor<TEnum>.Combo(title, filter, value);
			modified = (EnumEditor<TEnum>.mKeys[tenum] == EnumEditor<TEnum>.mKeys[value]);
			return tenum;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D14 File Offset: 0x00000F14
		[NullableContext(1)]
		public static bool Combo(string title, string filter, [Nullable(0)] ref TEnum value)
		{
			bool result = false;
			value = EnumEditor<TEnum>.Combo(title, filter, value, ref result);
			return result;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002D3C File Offset: 0x00000F3C
		public static TEnum List([Nullable(1)] string title, TEnum value)
		{
			int num = EnumEditor<TEnum>.mKeys[value];
			ImGui.ListBox(MemoryExtensions.AsSpan(title), ref num, EnumEditor<TEnum>.mNames, EnumEditor<TEnum>.mNames.Length);
			return EnumEditor<TEnum>.mValues[num];
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D7C File Offset: 0x00000F7C
		public static TEnum List([Nullable(1)] string title, TEnum value, ref bool modified)
		{
			TEnum tenum = EnumEditor<TEnum>.List(title, value);
			modified = (EnumEditor<TEnum>.mKeys[tenum] == EnumEditor<TEnum>.mKeys[value]);
			return tenum;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002DAC File Offset: 0x00000FAC
		public static bool List([Nullable(1)] string title, ref TEnum value)
		{
			bool result = false;
			value = EnumEditor<TEnum>.List(title, value, ref result);
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public static TEnum List([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value)
		{
			int num = EnumEditor<TEnum>.mKeys[value];
			IEnumerable<string> enumerable = EnumEditor<TEnum>.mNames;
			IEnumerable<string> enumerable2;
			IEnumerable<int> enumerable3;
			int num2;
			EditorsUtils.FilterElements(filter, enumerable, out enumerable2, out enumerable3, ref num, out num2);
			ImGui.ListBox(MemoryExtensions.AsSpan(title), ref num2, Enumerable.ToArray<string>(enumerable2), Enumerable.Count<string>(enumerable2));
			return EnumEditor<TEnum>.mValues[Enumerable.ToArray<int>(enumerable3)[num2]];
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002E30 File Offset: 0x00001030
		public static TEnum List([Nullable(1)] string title, [Nullable(1)] string filter, TEnum value, ref bool modified)
		{
			TEnum tenum = EnumEditor<TEnum>.List(title, filter, value);
			modified = (EnumEditor<TEnum>.mKeys[tenum] == EnumEditor<TEnum>.mKeys[value]);
			return tenum;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E64 File Offset: 0x00001064
		[NullableContext(1)]
		public static bool List(string title, string filter, [Nullable(0)] ref TEnum value)
		{
			bool result = false;
			value = EnumEditor<TEnum>.List(title, filter, value, ref result);
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002E89 File Offset: 0x00001089
		[NullableContext(1)]
		[return: Nullable(0)]
		public static TEnum ComboWithFilter(string title, string filterHolder, [Nullable(0)] TEnum value, uint maxFilterLength = 256U, string hint = "supports wildcards")
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter##" + title), MemoryExtensions.AsSpan(hint), ref filterHolder, maxFilterLength);
			return EnumEditor<TEnum>.Combo(title, filterHolder, value);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002EB3 File Offset: 0x000010B3
		[NullableContext(1)]
		[return: Nullable(0)]
		public static TEnum ListWithFilter(string title, string filterHolder, [Nullable(0)] TEnum value, uint maxFilterLength = 256U, string hint = "supports wildcards")
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter##" + title), MemoryExtensions.AsSpan(hint), ref filterHolder, maxFilterLength);
			return EnumEditor<TEnum>.List(title, filterHolder, value);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002EE0 File Offset: 0x000010E0
		// Note: this type is marked as 'beforefieldinit'.
		static EnumEditor()
		{
			EnumEditor<TEnum>.mNames = Enum.GetNames<TEnum>();
			EnumEditor<TEnum>.mValues = Enum.GetValues<TEnum>();
			EnumEditor<TEnum>.mKeys = Enumerable.ToDictionary(Enumerable.Select(EnumEditor<TEnum>.mValues, new Func<TEnum, int, <>f__AnonymousType0<TEnum, int>>(EnumEditor<TEnum>.<>c.<>9.<.cctor>b__17_0)), new Func<<>f__AnonymousType0<TEnum, int>, TEnum>(EnumEditor<TEnum>.<>c.<>9.<.cctor>b__17_1), new Func<<>f__AnonymousType0<TEnum, int>, int>(EnumEditor<TEnum>.<>c.<>9.<.cctor>b__17_2));
		}

		// Token: 0x04000004 RID: 4
		[Nullable(1)]
		private static readonly string[] mNames;

		// Token: 0x04000005 RID: 5
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private static readonly TEnum[] mValues;

		// Token: 0x04000006 RID: 6
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private static readonly Dictionary<TEnum, int> mKeys;
	}
}
