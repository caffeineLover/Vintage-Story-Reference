using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000BA RID: 186
	public class NonVirtualizingLayout : Layout, INonVirtualizingLayoutOverrides
	{
		// Token: 0x06000943 RID: 2371 RVA: 0x00020CF5 File Offset: 0x0001EEF5
		protected virtual void InitializeForContextCore(NonVirtualizingLayoutContext context)
		{
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00020CF7 File Offset: 0x0001EEF7
		protected virtual void UninitializeForContextCore(NonVirtualizingLayoutContext context)
		{
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00020CF9 File Offset: 0x0001EEF9
		protected virtual Size MeasureOverride(NonVirtualizingLayoutContext context, Size availableSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00020D00 File Offset: 0x0001EF00
		protected virtual Size ArrangeOverride(NonVirtualizingLayoutContext context, Size finalSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00020D07 File Offset: 0x0001EF07
		void INonVirtualizingLayoutOverrides.InitializeForContextCore(NonVirtualizingLayoutContext context)
		{
			this.InitializeForContextCore(context);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00020D10 File Offset: 0x0001EF10
		void INonVirtualizingLayoutOverrides.UninitializeForContextCore(NonVirtualizingLayoutContext context)
		{
			this.UninitializeForContextCore(context);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00020D19 File Offset: 0x0001EF19
		Size INonVirtualizingLayoutOverrides.MeasureOverride(NonVirtualizingLayoutContext context, Size availableSize)
		{
			return this.MeasureOverride(context, availableSize);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00020D23 File Offset: 0x0001EF23
		Size INonVirtualizingLayoutOverrides.ArrangeOverride(NonVirtualizingLayoutContext context, Size finalSize)
		{
			return this.ArrangeOverride(context, finalSize);
		}
	}
}
