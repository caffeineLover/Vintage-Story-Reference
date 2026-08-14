using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x0200000F RID: 15
	public class ALContextAttributes
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002311 File Offset: 0x00000511
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002319 File Offset: 0x00000519
		public int? Frequency { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002322 File Offset: 0x00000522
		// (set) Token: 0x0600004C RID: 76 RVA: 0x0000232A File Offset: 0x0000052A
		public int? MonoSources { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002333 File Offset: 0x00000533
		// (set) Token: 0x0600004E RID: 78 RVA: 0x0000233B File Offset: 0x0000053B
		public int? StereoSources { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002344 File Offset: 0x00000544
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000234C File Offset: 0x0000054C
		public int? Refresh { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002355 File Offset: 0x00000555
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000235D File Offset: 0x0000055D
		public bool? Sync { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002366 File Offset: 0x00000566
		// (set) Token: 0x06000054 RID: 84 RVA: 0x0000236E File Offset: 0x0000056E
		public int[] AdditionalAttributes { get; set; }

		// Token: 0x06000055 RID: 85 RVA: 0x00002377 File Offset: 0x00000577
		public ALContextAttributes()
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000237F File Offset: 0x0000057F
		public ALContextAttributes(int? frequency, int? monoSources, int? stereoSources, int? refresh, bool? sync)
		{
			this.Frequency = frequency;
			this.MonoSources = monoSources;
			this.StereoSources = stereoSources;
			this.Refresh = refresh;
			this.Sync = sync;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000023AC File Offset: 0x000005AC
		public int[] CreateAttributeArray()
		{
			int num = 10;
			int[] additionalAttributes = this.AdditionalAttributes;
			ALContextAttributes.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.attributeList = new int[num + ((additionalAttributes != null) ? additionalAttributes.Length : 0) + 1];
			CS$<>8__locals1.index = 0;
			ALContextAttributes.<CreateAttributeArray>g__AddAttribute|26_0(this.Frequency, AlcContextAttributes.Frequency, ref CS$<>8__locals1);
			ALContextAttributes.<CreateAttributeArray>g__AddAttribute|26_0(this.MonoSources, AlcContextAttributes.MonoSources, ref CS$<>8__locals1);
			ALContextAttributes.<CreateAttributeArray>g__AddAttribute|26_0(this.StereoSources, AlcContextAttributes.StereoSources, ref CS$<>8__locals1);
			ALContextAttributes.<CreateAttributeArray>g__AddAttribute|26_0(this.Refresh, AlcContextAttributes.Refresh, ref CS$<>8__locals1);
			if (this.Sync != null)
			{
				ALContextAttributes.<CreateAttributeArray>g__AddAttribute|26_0(new int?((this.Sync.GetValueOrDefault() > false) ? 1 : 0), AlcContextAttributes.Sync, ref CS$<>8__locals1);
			}
			if (this.AdditionalAttributes != null)
			{
				Array.Copy(this.AdditionalAttributes, 0, CS$<>8__locals1.attributeList, CS$<>8__locals1.index, this.AdditionalAttributes.Length);
				CS$<>8__locals1.index += this.AdditionalAttributes.Length;
			}
			int[] attributeList = CS$<>8__locals1.attributeList;
			int index = CS$<>8__locals1.index;
			CS$<>8__locals1.index = index + 1;
			attributeList[index] = 0;
			return CS$<>8__locals1.attributeList;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000024BC File Offset: 0x000006BC
		internal static ALContextAttributes FromArray(int[] attributeArray)
		{
			ALContextAttributes.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.extra = new List<int>();
			CS$<>8__locals1.attributes = new ALContextAttributes();
			for (int i = 0; i < attributeArray.Length - 1; i += 2)
			{
				ALContextAttributes.<FromArray>g__ParseAttribute|27_0(attributeArray[i], attributeArray[i + 1], ref CS$<>8__locals1);
			}
			CS$<>8__locals1.attributes.AdditionalAttributes = CS$<>8__locals1.extra.ToArray();
			return CS$<>8__locals1.attributes;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000251C File Offset: 0x0000071C
		private string GetOptionalString<[IsUnmanaged] T>(string title, T? value) where T : struct, ValueType
		{
			if (value == null)
			{
				return null;
			}
			return string.Format("{0}: {1}", title, value);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000253C File Offset: 0x0000073C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.GetOptionalString<int>("Frequency", this.Frequency),
				", ",
				this.GetOptionalString<int>("MonoSources", this.MonoSources),
				", ",
				this.GetOptionalString<int>("StereoSources", this.StereoSources),
				", ",
				this.GetOptionalString<int>("Refresh", this.Refresh),
				", ",
				this.GetOptionalString<bool>("Sync", this.Sync),
				(this.AdditionalAttributes != null) ? (", " + string.Join<int>(", ", this.AdditionalAttributes)) : string.Empty
			});
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002608 File Offset: 0x00000808
		[CompilerGenerated]
		internal static void <CreateAttributeArray>g__AddAttribute|26_0(int? value, AlcContextAttributes attribute, ref ALContextAttributes.<>c__DisplayClass26_0 A_2)
		{
			if (value != null)
			{
				int[] attributeList = A_2.attributeList;
				int index = A_2.index;
				A_2.index = index + 1;
				attributeList[index] = attribute;
				int[] attributeList2 = A_2.attributeList;
				index = A_2.index;
				A_2.index = index + 1;
				attributeList2[index] = value.GetValueOrDefault();
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002658 File Offset: 0x00000858
		[CompilerGenerated]
		internal static void <FromArray>g__ParseAttribute|27_0(int @enum, int value, ref ALContextAttributes.<>c__DisplayClass27_0 A_2)
		{
			switch (@enum)
			{
			case 4103:
				A_2.attributes.Frequency = new int?(value);
				return;
			case 4104:
				A_2.attributes.Refresh = new int?(value);
				return;
			case 4105:
				A_2.attributes.Sync = new bool?(value == 1);
				return;
			default:
				if (@enum == 4112)
				{
					A_2.attributes.MonoSources = new int?(value);
					return;
				}
				if (@enum != 4113)
				{
					A_2.extra.Add(@enum);
					A_2.extra.Add(value);
					return;
				}
				A_2.attributes.StereoSources = new int?(value);
				return;
			}
		}
	}
}
