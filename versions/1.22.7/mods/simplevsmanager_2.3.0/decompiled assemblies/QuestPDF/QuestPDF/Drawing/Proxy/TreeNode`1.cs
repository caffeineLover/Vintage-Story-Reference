using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QuestPDF.Drawing.Proxy
{
	// Token: 0x0200011A RID: 282
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TreeNode<[Nullable(2)] T>
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x00019EB6 File Offset: 0x000180B6
		public T Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x00019EBE File Offset: 0x000180BE
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x00019EC6 File Offset: 0x000180C6
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public TreeNode<T> Parent
		{
			[CompilerGenerated]
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return this.<Parent>k__BackingField;
			}
			[CompilerGenerated]
			[param: Nullable(new byte[]
			{
				2,
				1
			})]
			set
			{
				this.<Parent>k__BackingField = value;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x00019ECF File Offset: 0x000180CF
		public ICollection<TreeNode<T>> Children
		{
			[CompilerGenerated]
			get
			{
				return this.<Children>k__BackingField;
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00019ED7 File Offset: 0x000180D7
		public TreeNode(T Value)
		{
			this.<Children>k__BackingField = new List<TreeNode<T>>();
			base..ctor();
			this.<Value>k__BackingField = Value;
		}
	}
}
