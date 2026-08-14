using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000EE RID: 238
	internal readonly struct ChunkHeader
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0001B4B8 File Offset: 0x000196B8
		public long Position { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0001B4C0 File Offset: 0x000196C0
		public int Length { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0001B4C8 File Offset: 0x000196C8
		public string Name { get; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0001B4D0 File Offset: 0x000196D0
		public bool IsCritical
		{
			get
			{
				return char.IsUpper(this.Name.get_Chars(0));
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0001B4E3 File Offset: 0x000196E3
		public bool IsPublic
		{
			get
			{
				return char.IsUpper(this.Name.get_Chars(1));
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0001B4F6 File Offset: 0x000196F6
		public bool IsSafeToCopy
		{
			get
			{
				return char.IsUpper(this.Name.get_Chars(3));
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001B50C File Offset: 0x0001970C
		public ChunkHeader(long position, int length, string name)
		{
			if (length < 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(69, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Length less than zero (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(length);
				defaultInterpolatedStringHandler.AppendLiteral(") encountered when reading chunk at position ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(position);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Position = position;
			this.Length = length;
			this.Name = name;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001B580 File Offset: 0x00019780
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 3);
			defaultInterpolatedStringHandler.AppendFormatted(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.Position);
			defaultInterpolatedStringHandler.AppendLiteral(" (length: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Length);
			defaultInterpolatedStringHandler.AppendLiteral(").");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
