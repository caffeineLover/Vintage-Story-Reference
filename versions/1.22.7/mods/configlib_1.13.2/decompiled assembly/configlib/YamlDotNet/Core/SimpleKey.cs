using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001C9 RID: 457
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SimpleKey
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00024837 File Offset: 0x00022A37
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x0002483F File Offset: 0x00022A3F
		public bool IsPossible { get; private set; }

		// Token: 0x06000A39 RID: 2617 RVA: 0x00024848 File Offset: 0x00022A48
		public void MarkAsImpossible()
		{
			this.IsPossible = false;
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00024851 File Offset: 0x00022A51
		public bool IsRequired { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00024859 File Offset: 0x00022A59
		public int TokenNumber { get; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00024861 File Offset: 0x00022A61
		public int Index
		{
			get
			{
				return this.cursor.Index;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0002486E File Offset: 0x00022A6E
		public int Line
		{
			get
			{
				return this.cursor.Line;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002487B File Offset: 0x00022A7B
		public int LineOffset
		{
			get
			{
				return this.cursor.LineOffset;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00024888 File Offset: 0x00022A88
		public Mark Mark
		{
			get
			{
				return this.cursor.Mark();
			}
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00024895 File Offset: 0x00022A95
		public SimpleKey()
		{
			this.cursor = new Cursor();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000248A8 File Offset: 0x00022AA8
		public SimpleKey(bool isRequired, int tokenNumber, Cursor cursor)
		{
			this.IsPossible = true;
			this.IsRequired = isRequired;
			this.TokenNumber = tokenNumber;
			this.cursor = new Cursor(cursor);
		}

		// Token: 0x04000440 RID: 1088
		private readonly Cursor cursor;
	}
}
