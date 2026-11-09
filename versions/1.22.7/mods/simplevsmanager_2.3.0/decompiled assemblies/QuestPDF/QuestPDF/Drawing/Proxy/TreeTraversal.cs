using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
	// Token: 0x0200011B RID: 283
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TreeTraversal
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x00019EF1 File Offset: 0x000180F1
		public static IEnumerable<TreeNode<T>> ExtractElementsOfType<[Nullable(0)] T>(this Element element) where T : Element
		{
			TreeTraversal.<ExtractElementsOfType>d__0<T> <ExtractElementsOfType>d__ = new TreeTraversal.<ExtractElementsOfType>d__0<T>(-2);
			<ExtractElementsOfType>d__.<>3__element = element;
			return <ExtractElementsOfType>d__;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00019F01 File Offset: 0x00018101
		public static IEnumerable<TreeNode<T>> Flatten<[Nullable(0)] T>(this TreeNode<T> element) where T : Element
		{
			TreeTraversal.<Flatten>d__1<T> <Flatten>d__ = new TreeTraversal.<Flatten>d__1<T>(-2);
			<Flatten>d__.<>3__element = element;
			return <Flatten>d__;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00019F11 File Offset: 0x00018111
		public static IEnumerable<TreeNode<T>> ExtractAncestors<[Nullable(2)] T>(this TreeNode<T> node)
		{
			TreeTraversal.<ExtractAncestors>d__2<T> <ExtractAncestors>d__ = new TreeTraversal.<ExtractAncestors>d__2<T>(-2);
			<ExtractAncestors>d__.<>3__node = node;
			return <ExtractAncestors>d__;
		}

		// Token: 0x020001FF RID: 511
		[CompilerGenerated]
		private static class <ExtractElementsOfType>O__0_0<T> where T : Element
		{
			// Token: 0x04000777 RID: 1911
			[Nullable(new byte[]
			{
				0,
				0,
				1,
				0,
				0
			})]
			public static Func<Element, IEnumerable<TreeNode<T>>> <0>__ExtractElementsOfType;
		}
	}
}
