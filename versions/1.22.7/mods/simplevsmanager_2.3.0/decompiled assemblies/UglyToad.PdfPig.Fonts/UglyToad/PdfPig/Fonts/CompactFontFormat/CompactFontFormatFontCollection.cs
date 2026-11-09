using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000088 RID: 136
	public class CompactFontFormatFontCollection
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000322 RID: 802 RVA: 0x000166DB File Offset: 0x000148DB
		public CompactFontFormatHeader Header { get; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000323 RID: 803 RVA: 0x000166E3 File Offset: 0x000148E3
		public IReadOnlyDictionary<string, CompactFontFormatFont> Fonts { get; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000324 RID: 804 RVA: 0x000166EB File Offset: 0x000148EB
		public CompactFontFormatFont FirstFont { get; }

		// Token: 0x06000325 RID: 805 RVA: 0x000166F4 File Offset: 0x000148F4
		public CompactFontFormatFontCollection(CompactFontFormatHeader header, IReadOnlyDictionary<string, CompactFontFormatFont> fontSet)
		{
			this.Header = header;
			if (fontSet == null)
			{
				throw new ArgumentNullException("fontSet");
			}
			this.Fonts = fontSet;
			using (IEnumerator<KeyValuePair<string, CompactFontFormatFont>> enumerator = fontSet.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<string, CompactFontFormatFont> keyValuePair = enumerator.Current;
					this.FirstFont = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00016768 File Offset: 0x00014968
		public TransformationMatrix GetFirstTransformationMatrix()
		{
			using (IEnumerator<KeyValuePair<string, CompactFontFormatFont>> enumerator = this.Fonts.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<string, CompactFontFormatFont> keyValuePair = enumerator.Current;
					return keyValuePair.Value.FontMatrix;
				}
			}
			return TransformationMatrix.Identity;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000167C4 File Offset: 0x000149C4
		public PdfRectangle? GetCharacterBoundingBox(string characterName)
		{
			return this.FirstFont.GetCharacterBoundingBox(characterName);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000167D4 File Offset: 0x000149D4
		public string GetCharacterName(int characterCode)
		{
			CompactFontFormatFont firstFont = this.FirstFont;
			if (firstFont.Encoding != null)
			{
				return firstFont.Encoding.GetName(characterCode);
			}
			return ".notdef";
		}
	}
}
