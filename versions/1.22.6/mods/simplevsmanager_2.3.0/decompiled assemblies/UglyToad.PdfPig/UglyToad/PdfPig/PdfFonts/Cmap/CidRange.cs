using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000B2 RID: 178
	internal struct CidRange
	{
		// Token: 0x06000332 RID: 818 RVA: 0x00014C28 File Offset: 0x00012E28
		public CidRange(int firstCharacterCode, int lastCharacterCode, int cid)
		{
			if (lastCharacterCode < firstCharacterCode)
			{
				string text = "lastCharacterCode";
				string text2 = "The last character code cannot be lower than the first character code: ";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(22, 3);
				defaultInterpolatedStringHandler.AppendLiteral("First: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(firstCharacterCode);
				defaultInterpolatedStringHandler.AppendLiteral(", Last: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(lastCharacterCode);
				defaultInterpolatedStringHandler.AppendLiteral(", CID: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(cid);
				throw new ArgumentOutOfRangeException(text, text2 + defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.firstCharacterCode = firstCharacterCode;
			this.lastCharacterCode = lastCharacterCode;
			this.cid = cid;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00014CB0 File Offset: 0x00012EB0
		public bool Contains(int characterCode)
		{
			return this.firstCharacterCode <= characterCode && characterCode <= this.lastCharacterCode;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00014CC9 File Offset: 0x00012EC9
		public bool TryMap(int characterCode, out int cidValue)
		{
			cidValue = 0;
			if (this.Contains(characterCode))
			{
				cidValue = this.cid + (characterCode - this.firstCharacterCode);
				return true;
			}
			return false;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00014CEC File Offset: 0x00012EEC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(15, 3);
			defaultInterpolatedStringHandler.AppendLiteral("CID ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.cid);
			defaultInterpolatedStringHandler.AppendLiteral(": Code ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.firstCharacterCode);
			defaultInterpolatedStringHandler.AppendLiteral(" -> ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.lastCharacterCode);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040001AF RID: 431
		private readonly int firstCharacterCode;

		// Token: 0x040001B0 RID: 432
		private readonly int lastCharacterCode;

		// Token: 0x040001B1 RID: 433
		private readonly int cid;
	}
}
