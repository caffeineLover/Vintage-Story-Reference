using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x02000047 RID: 71
	internal class LayoutPanelLayoutContext : NonVirtualizingLayoutContext
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x000097A4 File Offset: 0x000079A4
		public LayoutPanelLayoutContext(LayoutPanel owner)
		{
			this.m_owner = new WeakReference<LayoutPanel>(owner);
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x000097B8 File Offset: 0x000079B8
		protected override IReadOnlyList<UIElement> ChildrenCore
		{
			get
			{
				return new LayoutPanelLayoutContext.UIElementCollectionView(this.GetOwner().Children);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x000097CA File Offset: 0x000079CA
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x000097D7 File Offset: 0x000079D7
		protected override object LayoutStateCore
		{
			get
			{
				return this.GetOwner().LayoutState;
			}
			set
			{
				this.GetOwner().LayoutState = value;
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000097E8 File Offset: 0x000079E8
		private LayoutPanel GetOwner()
		{
			LayoutPanel result;
			this.m_owner.TryGetTarget(ref result);
			return result;
		}

		// Token: 0x0400010B RID: 267
		private readonly WeakReference<LayoutPanel> m_owner;

		// Token: 0x020000FD RID: 253
		private class UIElementCollectionView : IReadOnlyList<UIElement>, IEnumerable<UIElement>, IEnumerable, IReadOnlyCollection<UIElement>
		{
			// Token: 0x06000CA5 RID: 3237 RVA: 0x0002B2E8 File Offset: 0x000294E8
			public UIElementCollectionView(UIElementCollection collection)
			{
				this.m_collection = collection;
			}

			// Token: 0x17000232 RID: 562
			public UIElement this[int index]
			{
				get
				{
					return this.m_collection[index];
				}
			}

			// Token: 0x17000233 RID: 563
			// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0002B305 File Offset: 0x00029505
			public int Count
			{
				get
				{
					return this.m_collection.Count;
				}
			}

			// Token: 0x06000CA8 RID: 3240 RVA: 0x0002B312 File Offset: 0x00029512
			public IEnumerator<UIElement> GetEnumerator()
			{
				foreach (object obj in this.m_collection)
				{
					UIElement uielement = (UIElement)obj;
					yield return uielement;
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x06000CA9 RID: 3241 RVA: 0x0002B321 File Offset: 0x00029521
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.m_collection.GetEnumerator();
			}

			// Token: 0x0400056E RID: 1390
			private readonly UIElementCollection m_collection;
		}
	}
}
