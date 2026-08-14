using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000B7 RID: 183
	public class LayoutContext : DependencyObject, ILayoutContextOverrides
	{
		// Token: 0x0600092D RID: 2349 RVA: 0x00020B19 File Offset: 0x0001ED19
		internal LayoutContext()
		{
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x00020B21 File Offset: 0x0001ED21
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x00020B29 File Offset: 0x0001ED29
		public object LayoutState
		{
			get
			{
				return this.LayoutStateCore;
			}
			set
			{
				this.LayoutStateCore = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x00020B32 File Offset: 0x0001ED32
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x00020B39 File Offset: 0x0001ED39
		protected virtual object LayoutStateCore
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x00020B40 File Offset: 0x0001ED40
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x00020B48 File Offset: 0x0001ED48
		object ILayoutContextOverrides.LayoutStateCore
		{
			get
			{
				return this.LayoutStateCore;
			}
			set
			{
				this.LayoutStateCore = value;
			}
		}
	}
}
