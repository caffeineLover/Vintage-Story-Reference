using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000DA RID: 218
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SimpleKey
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00016818 File Offset: 0x00014A18
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x00016820 File Offset: 0x00014A20
		public bool IsPossible { get; private set; }

		// Token: 0x060005B2 RID: 1458 RVA: 0x00016829 File Offset: 0x00014A29
		public void MarkAsImpossible()
		{
			this.IsPossible = false;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00016832 File Offset: 0x00014A32
		public bool IsRequired { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0001683A File Offset: 0x00014A3A
		public int TokenNumber { get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00016842 File Offset: 0x00014A42
		public long Index
		{
			get
			{
				return this.cursor.Index;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0001684F File Offset: 0x00014A4F
		public long Line
		{
			get
			{
				return this.cursor.Line;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0001685C File Offset: 0x00014A5C
		public long LineOffset
		{
			get
			{
				return this.cursor.LineOffset;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00016869 File Offset: 0x00014A69
		public Mark Mark
		{
			get
			{
				return this.cursor.Mark();
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00016876 File Offset: 0x00014A76
		public SimpleKey()
		{
			this.cursor = new Cursor();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00016889 File Offset: 0x00014A89
		public SimpleKey(bool isRequired, int tokenNumber, Cursor cursor)
		{
			this.IsPossible = true;
			this.IsRequired = isRequired;
			this.TokenNumber = tokenNumber;
			this.cursor = new Cursor(cursor);
		}

		// Token: 0x040001B8 RID: 440
		private readonly Cursor cursor;
	}
}
