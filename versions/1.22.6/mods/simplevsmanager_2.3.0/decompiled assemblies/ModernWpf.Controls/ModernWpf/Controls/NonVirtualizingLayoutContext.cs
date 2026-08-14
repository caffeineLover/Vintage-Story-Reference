using System;
using System.Collections.Generic;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000BC RID: 188
	public class NonVirtualizingLayoutContext : LayoutContext
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00020D35 File Offset: 0x0001EF35
		public IReadOnlyList<UIElement> Children
		{
			get
			{
				return this.ChildrenCore;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00020D3D File Offset: 0x0001EF3D
		protected virtual IReadOnlyList<UIElement> ChildrenCore
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00020D44 File Offset: 0x0001EF44
		internal VirtualizingLayoutContext GetVirtualizingContextAdapter()
		{
			if (this.m_contextAdapter == null)
			{
				this.m_contextAdapter = new LayoutContextAdapter(this);
			}
			return this.m_contextAdapter;
		}

		// Token: 0x040003E5 RID: 997
		private VirtualizingLayoutContext m_contextAdapter;
	}
}
