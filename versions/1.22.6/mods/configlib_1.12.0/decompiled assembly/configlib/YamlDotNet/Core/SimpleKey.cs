using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CD RID: 461
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SimpleKey
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0002517F File Offset: 0x0002337F
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x00025187 File Offset: 0x00023387
		public bool IsPossible { get; private set; }

		// Token: 0x06000A75 RID: 2677 RVA: 0x00025190 File Offset: 0x00023390
		public void MarkAsImpossible()
		{
			this.IsPossible = false;
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00025199 File Offset: 0x00023399
		public bool IsRequired { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x000251A1 File Offset: 0x000233A1
		public int TokenNumber { get; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x000251A9 File Offset: 0x000233A9
		public int Index
		{
			get
			{
				return this.cursor.Index;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x000251B6 File Offset: 0x000233B6
		public int Line
		{
			get
			{
				return this.cursor.Line;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x000251C3 File Offset: 0x000233C3
		public int LineOffset
		{
			get
			{
				return this.cursor.LineOffset;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x000251D0 File Offset: 0x000233D0
		public Mark Mark
		{
			get
			{
				return this.cursor.Mark();
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x000251DD File Offset: 0x000233DD
		public SimpleKey()
		{
			this.cursor = new Cursor();
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x000251F0 File Offset: 0x000233F0
		public SimpleKey(bool isRequired, int tokenNumber, Cursor cursor)
		{
			this.IsPossible = true;
			this.IsRequired = isRequired;
			this.TokenNumber = tokenNumber;
			this.cursor = new Cursor(cursor);
		}

		// Token: 0x0400046E RID: 1134
		private readonly Cursor cursor;
	}
}
