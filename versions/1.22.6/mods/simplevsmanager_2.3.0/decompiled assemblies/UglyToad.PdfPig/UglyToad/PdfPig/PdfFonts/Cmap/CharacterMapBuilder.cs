using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000B0 RID: 176
	internal class CharacterMapBuilder
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0001489C File Offset: 0x00012A9C
		// (set) Token: 0x06000314 RID: 788 RVA: 0x000148A4 File Offset: 0x00012AA4
		public CharacterIdentifierSystemInfo CharacterIdentifierSystemInfo { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000315 RID: 789 RVA: 0x000148AD File Offset: 0x00012AAD
		public CharacterIdentifierSystemInfoBuilder SystemInfoBuilder { get; } = new CharacterIdentifierSystemInfoBuilder();

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000316 RID: 790 RVA: 0x000148B5 File Offset: 0x00012AB5
		// (set) Token: 0x06000317 RID: 791 RVA: 0x000148BD File Offset: 0x00012ABD
		public int WMode { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000318 RID: 792 RVA: 0x000148C6 File Offset: 0x00012AC6
		// (set) Token: 0x06000319 RID: 793 RVA: 0x000148CE File Offset: 0x00012ACE
		public string Name { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600031A RID: 794 RVA: 0x000148D7 File Offset: 0x00012AD7
		// (set) Token: 0x0600031B RID: 795 RVA: 0x000148DF File Offset: 0x00012ADF
		public string Version { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600031C RID: 796 RVA: 0x000148E8 File Offset: 0x00012AE8
		// (set) Token: 0x0600031D RID: 797 RVA: 0x000148F0 File Offset: 0x00012AF0
		public int Type { get; set; } = -1;

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600031E RID: 798 RVA: 0x000148F9 File Offset: 0x00012AF9
		// (set) Token: 0x0600031F RID: 799 RVA: 0x00014901 File Offset: 0x00012B01
		public IReadOnlyList<CodespaceRange> CodespaceRanges { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0001490A File Offset: 0x00012B0A
		// (set) Token: 0x06000321 RID: 801 RVA: 0x00014912 File Offset: 0x00012B12
		public IReadOnlyList<CidCharacterMapping> CidCharacterMappings { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0001491B File Offset: 0x00012B1B
		public IReadOnlyList<CidRange> CidRanges
		{
			get
			{
				return this.cidRanges;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00014923 File Offset: 0x00012B23
		public Dictionary<int, string> BaseFontCharacterMap { get; } = new Dictionary<int, string>();

		// Token: 0x06000324 RID: 804 RVA: 0x0001492B File Offset: 0x00012B2B
		public void AddBaseFontCharacter(IReadOnlyList<byte> bytes, IReadOnlyList<byte> value)
		{
			this.AddBaseFontCharacter(bytes, CharacterMapBuilder.CreateStringFromBytes(Enumerable.ToArray<byte>(value)));
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00014940 File Offset: 0x00012B40
		public void AddBaseFontCharacter(IReadOnlyList<byte> bytes, string value)
		{
			int codeFromArray = this.GetCodeFromArray(bytes, bytes.Count);
			this.BaseFontCharacterMap[codeFromArray] = value;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00014968 File Offset: 0x00012B68
		public CMap Build()
		{
			return new CMap(this.GetCidSystemInfo(), this.Type, this.WMode, this.Name, this.Version, this.BaseFontCharacterMap ?? new Dictionary<int, string>(), this.CodespaceRanges ?? new CodespaceRange[0], this.CidRanges ?? new CidRange[0], this.CidCharacterMappings ?? new CidCharacterMapping[0]);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000149D8 File Offset: 0x00012BD8
		private CharacterIdentifierSystemInfo GetCidSystemInfo()
		{
			if (this.CharacterIdentifierSystemInfo.Registry != null)
			{
				return this.CharacterIdentifierSystemInfo;
			}
			if (this.SystemInfoBuilder.HasOrdering && this.SystemInfoBuilder.HasRegistry && this.SystemInfoBuilder.HasSupplement)
			{
				return new CharacterIdentifierSystemInfo(this.SystemInfoBuilder.Registry, this.SystemInfoBuilder.Ordering, this.SystemInfoBuilder.Supplement);
			}
			return this.CharacterIdentifierSystemInfo;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00014A50 File Offset: 0x00012C50
		public void UseCMap(CMap other)
		{
			this.CodespaceRanges = CharacterMapBuilder.Combine<CodespaceRange>(this.CodespaceRanges, other.CodespaceRanges);
			this.CidCharacterMappings = CharacterMapBuilder.Combine<CidCharacterMapping>(this.CidCharacterMappings, Enumerable.ToList<CidCharacterMapping>(other.CidCharacterMappings.Values));
			this.cidRanges.AddRange(other.CidRanges);
			if (other.BaseFontCharacterMap != null)
			{
				foreach (KeyValuePair<int, string> keyValuePair in other.BaseFontCharacterMap)
				{
					this.BaseFontCharacterMap[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00014B00 File Offset: 0x00012D00
		private static IReadOnlyList<T> Combine<T>(IReadOnlyList<T> a, IReadOnlyList<T> b)
		{
			if (a == null && b == null)
			{
				return new T[0];
			}
			if (a == null)
			{
				return b;
			}
			if (b == null)
			{
				return a;
			}
			List<T> list = new List<T>(a);
			list.AddRange(b);
			return list;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00014B28 File Offset: 0x00012D28
		private int GetCodeFromArray(IReadOnlyList<byte> data, int length)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				num <<= 8;
				num |= ((int)data[i] + 256) % 256;
			}
			return num;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00014B5E File Offset: 0x00012D5E
		private static string CreateStringFromBytes(byte[] bytes)
		{
			if (bytes.Length != 1)
			{
				return Encoding.BigEndianUnicode.GetString(bytes);
			}
			return OtherEncodings.BytesAsLatin1String(bytes);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00014B78 File Offset: 0x00012D78
		public void AddCidRange(CidRange range)
		{
			this.cidRanges.Add(range);
		}

		// Token: 0x040001AB RID: 427
		private List<CidRange> cidRanges = new List<CidRange>();
	}
}
