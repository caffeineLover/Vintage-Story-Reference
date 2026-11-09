using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings
{
	// Token: 0x02000013 RID: 19
	internal class Type1CharstringDecryptedBytes
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000576C File Offset: 0x0000396C
		public IReadOnlyList<byte> Bytes { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00005774 File Offset: 0x00003974
		public int Index { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000577C File Offset: 0x0000397C
		public string Name { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00005784 File Offset: 0x00003984
		public Type1CharstringDecryptedBytes.SourceType Source { get; }

		// Token: 0x06000085 RID: 133 RVA: 0x0000578C File Offset: 0x0000398C
		public Type1CharstringDecryptedBytes(IReadOnlyList<byte> bytes, int index)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.Bytes = bytes;
			this.Index = index;
			this.Name = ".notdef";
			this.Source = 0;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000057C4 File Offset: 0x000039C4
		public Type1CharstringDecryptedBytes(string name, IReadOnlyList<byte> bytes, int index)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.Bytes = bytes;
			this.Index = index;
			this.Name = (name ?? index.ToString(CultureInfo.InvariantCulture));
			this.Source = 1;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005814 File Offset: 0x00003A14
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(9, 4);
			defaultInterpolatedStringHandler.AppendFormatted(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<Type1CharstringDecryptedBytes.SourceType>(this.Source);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Index);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Bytes.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x020000C0 RID: 192
		public enum SourceType
		{
			// Token: 0x04000451 RID: 1105
			Subroutine,
			// Token: 0x04000452 RID: 1106
			Charstring
		}
	}
}
