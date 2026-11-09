using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000E9 RID: 233
	public class CommandBarPanel : ToolBarPanel
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00028C0B File Offset: 0x00026E0B
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00028C13 File Offset: 0x00026E13
		internal bool HasChildren
		{
			get
			{
				return this.m_hasChildren;
			}
			set
			{
				if (this.m_hasChildren != value)
				{
					this.m_hasChildren = value;
					EventHandler hasChildrenChanged = this.HasChildrenChanged;
					if (hasChildrenChanged == null)
					{
						return;
					}
					hasChildrenChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06000BE7 RID: 3047 RVA: 0x00028C3C File Offset: 0x00026E3C
		// (remove) Token: 0x06000BE8 RID: 3048 RVA: 0x00028C74 File Offset: 0x00026E74
		internal event EventHandler HasChildrenChanged;

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00028CA9 File Offset: 0x00026EA9
		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			this.UpdateHasChildren();
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00028CBC File Offset: 0x00026EBC
		private void UpdateHasChildren()
		{
			bool hasChildren = false;
			for (int i = 0; i < base.InternalChildren.Count; i++)
			{
				if (base.InternalChildren[i] != null)
				{
					hasChildren = true;
					break;
				}
			}
			this.HasChildren = hasChildren;
		}

		// Token: 0x040004E4 RID: 1252
		private bool m_hasChildren;
	}
}
