using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000B6 RID: 182
	public class Layout : DependencyObject
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00020878 File Offset: 0x0001EA78
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x00020880 File Offset: 0x0001EA80
		public string LayoutId { get; set; }

		// Token: 0x06000920 RID: 2336 RVA: 0x0002088C File Offset: 0x0001EA8C
		private VirtualizingLayoutContext GetVirtualizingLayoutContext(LayoutContext context)
		{
			VirtualizingLayoutContext virtualizingLayoutContext = context as VirtualizingLayoutContext;
			if (virtualizingLayoutContext != null)
			{
				return virtualizingLayoutContext;
			}
			NonVirtualizingLayoutContext nonVirtualizingLayoutContext = context as NonVirtualizingLayoutContext;
			if (nonVirtualizingLayoutContext != null)
			{
				return nonVirtualizingLayoutContext.GetVirtualizingContextAdapter();
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000208BC File Offset: 0x0001EABC
		private NonVirtualizingLayoutContext GetNonVirtualizingLayoutContext(LayoutContext context)
		{
			NonVirtualizingLayoutContext nonVirtualizingLayoutContext = context as NonVirtualizingLayoutContext;
			if (nonVirtualizingLayoutContext != null)
			{
				return nonVirtualizingLayoutContext;
			}
			VirtualizingLayoutContext virtualizingLayoutContext = context as VirtualizingLayoutContext;
			if (virtualizingLayoutContext != null)
			{
				return virtualizingLayoutContext.GetNonVirtualizingContextAdapter();
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x000208EC File Offset: 0x0001EAEC
		public void InitializeForContext(LayoutContext context)
		{
			IVirtualizingLayoutOverrides virtualizingLayoutOverrides = this as IVirtualizingLayoutOverrides;
			if (virtualizingLayoutOverrides != null)
			{
				VirtualizingLayoutContext virtualizingLayoutContext = this.GetVirtualizingLayoutContext(context);
				virtualizingLayoutOverrides.InitializeForContextCore(virtualizingLayoutContext);
				return;
			}
			INonVirtualizingLayoutOverrides nonVirtualizingLayoutOverrides = this as INonVirtualizingLayoutOverrides;
			if (nonVirtualizingLayoutOverrides != null)
			{
				NonVirtualizingLayoutContext nonVirtualizingLayoutContext = this.GetNonVirtualizingLayoutContext(context);
				nonVirtualizingLayoutOverrides.InitializeForContextCore(nonVirtualizingLayoutContext);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00020934 File Offset: 0x0001EB34
		public void UninitializeForContext(LayoutContext context)
		{
			IVirtualizingLayoutOverrides virtualizingLayoutOverrides = this as IVirtualizingLayoutOverrides;
			if (virtualizingLayoutOverrides != null)
			{
				VirtualizingLayoutContext virtualizingLayoutContext = this.GetVirtualizingLayoutContext(context);
				virtualizingLayoutOverrides.UninitializeForContextCore(virtualizingLayoutContext);
				return;
			}
			INonVirtualizingLayoutOverrides nonVirtualizingLayoutOverrides = this as INonVirtualizingLayoutOverrides;
			if (nonVirtualizingLayoutOverrides != null)
			{
				NonVirtualizingLayoutContext nonVirtualizingLayoutContext = this.GetNonVirtualizingLayoutContext(context);
				nonVirtualizingLayoutOverrides.UninitializeForContextCore(nonVirtualizingLayoutContext);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002097C File Offset: 0x0001EB7C
		public Size Measure(LayoutContext context, Size availableSize)
		{
			IVirtualizingLayoutOverrides virtualizingLayoutOverrides = this as IVirtualizingLayoutOverrides;
			if (virtualizingLayoutOverrides != null)
			{
				VirtualizingLayoutContext virtualizingLayoutContext = this.GetVirtualizingLayoutContext(context);
				return virtualizingLayoutOverrides.MeasureOverride(virtualizingLayoutContext, availableSize);
			}
			INonVirtualizingLayoutOverrides nonVirtualizingLayoutOverrides = this as INonVirtualizingLayoutOverrides;
			if (nonVirtualizingLayoutOverrides != null)
			{
				NonVirtualizingLayoutContext nonVirtualizingLayoutContext = this.GetNonVirtualizingLayoutContext(context);
				return nonVirtualizingLayoutOverrides.MeasureOverride(nonVirtualizingLayoutContext, availableSize);
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x000209C4 File Offset: 0x0001EBC4
		public Size Arrange(LayoutContext context, Size finalSize)
		{
			IVirtualizingLayoutOverrides virtualizingLayoutOverrides = this as IVirtualizingLayoutOverrides;
			if (virtualizingLayoutOverrides != null)
			{
				VirtualizingLayoutContext virtualizingLayoutContext = this.GetVirtualizingLayoutContext(context);
				return virtualizingLayoutOverrides.ArrangeOverride(virtualizingLayoutContext, finalSize);
			}
			INonVirtualizingLayoutOverrides nonVirtualizingLayoutOverrides = this as INonVirtualizingLayoutOverrides;
			if (nonVirtualizingLayoutOverrides != null)
			{
				NonVirtualizingLayoutContext nonVirtualizingLayoutContext = this.GetNonVirtualizingLayoutContext(context);
				return nonVirtualizingLayoutOverrides.ArrangeOverride(nonVirtualizingLayoutContext, finalSize);
			}
			throw new NotImplementedException();
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000926 RID: 2342 RVA: 0x00020A0C File Offset: 0x0001EC0C
		// (remove) Token: 0x06000927 RID: 2343 RVA: 0x00020A44 File Offset: 0x0001EC44
		public event TypedEventHandler<Layout, object> MeasureInvalidated;

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000928 RID: 2344 RVA: 0x00020A7C File Offset: 0x0001EC7C
		// (remove) Token: 0x06000929 RID: 2345 RVA: 0x00020AB4 File Offset: 0x0001ECB4
		public event TypedEventHandler<Layout, object> ArrangeInvalidated;

		// Token: 0x0600092A RID: 2346 RVA: 0x00020AE9 File Offset: 0x0001ECE9
		protected void InvalidateMeasure()
		{
			TypedEventHandler<Layout, object> measureInvalidated = this.MeasureInvalidated;
			if (measureInvalidated == null)
			{
				return;
			}
			measureInvalidated.Invoke(this, null);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00020AFD File Offset: 0x0001ECFD
		protected void InvalidateArrange()
		{
			TypedEventHandler<Layout, object> arrangeInvalidated = this.ArrangeInvalidated;
			if (arrangeInvalidated == null)
			{
				return;
			}
			arrangeInvalidated.Invoke(this, null);
		}
	}
}
