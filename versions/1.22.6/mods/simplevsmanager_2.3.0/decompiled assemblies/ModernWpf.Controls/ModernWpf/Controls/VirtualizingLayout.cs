using System;
using System.Collections.Specialized;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000C6 RID: 198
	public class VirtualizingLayout : Layout, IVirtualizingLayoutOverrides
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x00022422 File Offset: 0x00020622
		protected virtual void InitializeForContextCore(VirtualizingLayoutContext context)
		{
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00022424 File Offset: 0x00020624
		protected virtual void UninitializeForContextCore(VirtualizingLayoutContext context)
		{
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00022426 File Offset: 0x00020626
		protected virtual Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002242D File Offset: 0x0002062D
		protected virtual Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
		{
			return finalSize;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00022430 File Offset: 0x00020630
		protected virtual void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
		{
			base.InvalidateMeasure();
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00022438 File Offset: 0x00020638
		void IVirtualizingLayoutOverrides.InitializeForContextCore(VirtualizingLayoutContext context)
		{
			this.InitializeForContextCore(context);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00022441 File Offset: 0x00020641
		void IVirtualizingLayoutOverrides.UninitializeForContextCore(VirtualizingLayoutContext context)
		{
			this.UninitializeForContextCore(context);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002244A File Offset: 0x0002064A
		Size IVirtualizingLayoutOverrides.MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
		{
			return this.MeasureOverride(context, availableSize);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00022454 File Offset: 0x00020654
		Size IVirtualizingLayoutOverrides.ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
		{
			return this.ArrangeOverride(context, finalSize);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0002245E File Offset: 0x0002065E
		void IVirtualizingLayoutOverrides.OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
		{
			this.OnItemsChangedCore(context, source, args);
		}
	}
}
