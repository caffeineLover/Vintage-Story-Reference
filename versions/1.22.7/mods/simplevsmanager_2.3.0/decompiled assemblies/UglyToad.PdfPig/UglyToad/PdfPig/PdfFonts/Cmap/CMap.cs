using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000B3 RID: 179
	internal class CMap
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00014D55 File Offset: 0x00012F55
		public CharacterIdentifierSystemInfo Info { get; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000337 RID: 823 RVA: 0x00014D5D File Offset: 0x00012F5D
		public int Type { get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00014D65 File Offset: 0x00012F65
		public string Name { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00014D6D File Offset: 0x00012F6D
		[CanBeNull]
		public string Version { get; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00014D75 File Offset: 0x00012F75
		[NotNull]
		public IReadOnlyDictionary<int, string> BaseFontCharacterMap { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00014D7D File Offset: 0x00012F7D
		[NotNull]
		public IReadOnlyList<CodespaceRange> CodespaceRanges { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00014D85 File Offset: 0x00012F85
		[NotNull]
		public IReadOnlyList<CidRange> CidRanges { get; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00014D8D File Offset: 0x00012F8D
		[NotNull]
		public IReadOnlyDictionary<int, CidCharacterMapping> CidCharacterMappings { get; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00014D95 File Offset: 0x00012F95
		public WritingMode WritingMode { get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00014D9D File Offset: 0x00012F9D
		public bool HasCidMappings
		{
			get
			{
				return this.CidCharacterMappings.Count > 0 || this.CidRanges.Count > 0;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00014DBD File Offset: 0x00012FBD
		public bool HasUnicodeMappings
		{
			get
			{
				return this.BaseFontCharacterMap.Count > 0;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00014DD0 File Offset: 0x00012FD0
		public CMap(CharacterIdentifierSystemInfo info, int type, int wMode, string name, string version, IReadOnlyDictionary<int, string> baseFontCharacterMap, IReadOnlyList<CodespaceRange> codespaceRanges, IReadOnlyList<CidRange> cidRanges, IReadOnlyList<CidCharacterMapping> cidCharacterMappings)
		{
			if (cidCharacterMappings == null)
			{
				throw new ArgumentNullException("cidCharacterMappings");
			}
			this.Info = info;
			this.Type = type;
			this.WritingMode = wMode;
			this.Name = name;
			this.Version = version;
			if (baseFontCharacterMap == null)
			{
				throw new ArgumentNullException("baseFontCharacterMap");
			}
			this.BaseFontCharacterMap = baseFontCharacterMap;
			if (codespaceRanges == null)
			{
				throw new ArgumentNullException("codespaceRanges");
			}
			this.CodespaceRanges = codespaceRanges;
			if (cidRanges == null)
			{
				throw new ArgumentNullException("cidRanges");
			}
			this.CidRanges = cidRanges;
			if (this.CodespaceRanges.Count > 0)
			{
				this.maxCodeLength = Enumerable.Max<CodespaceRange>(this.CodespaceRanges, (CodespaceRange x) => x.CodeLength);
				this.minCodeLength = Enumerable.Min<CodespaceRange>(this.CodespaceRanges, (CodespaceRange x) => x.CodeLength);
			}
			else
			{
				this.hasEmptyCodespace = true;
			}
			Dictionary<int, CidCharacterMapping> dictionary = new Dictionary<int, CidCharacterMapping>();
			foreach (CidCharacterMapping cidCharacterMapping in cidCharacterMappings)
			{
				dictionary[cidCharacterMapping.SourceCharacterCode] = cidCharacterMapping;
			}
			this.CidCharacterMappings = dictionary;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00014F2C File Offset: 0x0001312C
		public bool TryConvertToUnicode(int code, out string result)
		{
			return this.BaseFontCharacterMap.TryGetValue(code, ref result);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00014F3C File Offset: 0x0001313C
		public int ConvertToCid(int code)
		{
			CidCharacterMapping cidCharacterMapping;
			if (this.CidCharacterMappings.TryGetValue(code, ref cidCharacterMapping))
			{
				return cidCharacterMapping.DestinationCid;
			}
			foreach (CidRange cidRange in this.CidRanges)
			{
				int result;
				if (cidRange.TryMap(code, out result))
				{
					return result;
				}
			}
			return 0;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00014FB0 File Offset: 0x000131B0
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00014FB8 File Offset: 0x000131B8
		public int ReadCode(IInputBytes bytes)
		{
			if (this.hasEmptyCodespace)
			{
				byte[] array = new byte[this.minCodeLength];
				bytes.Read(array, default(int?));
				return array.ToInt(this.minCodeLength);
			}
			byte[] array2 = new byte[this.maxCodeLength];
			array2[0] = bytes.CurrentByte;
			int num = 1;
			while (num < this.minCodeLength && !bytes.IsAtEnd())
			{
				array2[num] = CMap.ReadByte(bytes);
				num++;
			}
			for (int i = this.minCodeLength - 1; i < this.maxCodeLength; i++)
			{
				int num2 = i + 1;
				using (IEnumerator<CodespaceRange> enumerator = this.CodespaceRanges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsFullMatch(array2, num2))
						{
							return CMap.ByteArrayToInt(array2, num2);
						}
					}
				}
				if (num2 < this.maxCodeLength)
				{
					array2[num2] = CMap.ReadByte(bytes);
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(48, 2);
			defaultInterpolatedStringHandler.AppendLiteral("CMap is invalid, min code length was ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.minCodeLength);
			defaultInterpolatedStringHandler.AppendLiteral(", max was ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.maxCodeLength);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00015114 File Offset: 0x00013314
		private static byte ReadByte(IInputBytes bytes)
		{
			if (!bytes.MoveNext())
			{
				throw new InvalidOperationException("Read byte called on input bytes which was at end of byte set. Current offset: " + bytes.CurrentOffset.ToString());
			}
			return bytes.CurrentByte;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00015150 File Offset: 0x00013350
		private static int ByteArrayToInt(byte[] data, int dataLen)
		{
			int num = 0;
			for (int i = 0; i < dataLen; i++)
			{
				num <<= 8;
				num |= (int)(data[i] & byte.MaxValue);
			}
			return num;
		}

		// Token: 0x040001BB RID: 443
		private readonly bool hasEmptyCodespace;

		// Token: 0x040001BC RID: 444
		private readonly int minCodeLength = 4;

		// Token: 0x040001BD RID: 445
		private readonly int maxCodeLength;
	}
}
