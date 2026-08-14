using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200028D RID: 653
	public static class DataGridRowHelper
	{
		// Token: 0x0600133D RID: 4925 RVA: 0x000387FD File Offset: 0x000369FD
		public static bool GetAreRowDetailsFrozen(DataGridRow row)
		{
			return (bool)row.GetValue(DataGridRowHelper.AreRowDetailsFrozenProperty);
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0003880F File Offset: 0x00036A0F
		private static void SetAreRowDetailsFrozen(DataGridRow row, bool value)
		{
			row.SetValue(DataGridRowHelper.AreRowDetailsFrozenPropertyKey, value);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00038822 File Offset: 0x00036A22
		private static void OnAreRowDetailsFrozenInternalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridRowHelper.SetAreRowDetailsFrozen((DataGridRow)d, (bool)e.NewValue);
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0003883B File Offset: 0x00036A3B
		public static DataGridHeadersVisibility GetHeadersVisibility(DataGridRow row)
		{
			return (DataGridHeadersVisibility)row.GetValue(DataGridRowHelper.HeadersVisibilityProperty);
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0003884D File Offset: 0x00036A4D
		private static void SetHeadersVisibility(DataGridRow row, DataGridHeadersVisibility value)
		{
			row.SetValue(DataGridRowHelper.HeadersVisibilityPropertyKey, value);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00038860 File Offset: 0x00036A60
		private static void OnHeadersVisibilityInternalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridRowHelper.SetHeadersVisibility((DataGridRow)d, (DataGridHeadersVisibility)e.NewValue);
		}

		// Token: 0x0400066B RID: 1643
		private static readonly DependencyPropertyKey AreRowDetailsFrozenPropertyKey = DependencyProperty.RegisterAttachedReadOnly("AreRowDetailsFrozen", typeof(bool), typeof(DataGridRowHelper), new PropertyMetadata(false));

		// Token: 0x0400066C RID: 1644
		public static readonly DependencyProperty AreRowDetailsFrozenProperty = DataGridRowHelper.AreRowDetailsFrozenPropertyKey.DependencyProperty;

		// Token: 0x0400066D RID: 1645
		internal static readonly DependencyProperty AreRowDetailsFrozenInternalProperty = DependencyProperty.RegisterAttached("AreRowDetailsFrozenInternal", typeof(bool), typeof(DataGridRowHelper), new PropertyMetadata(false, new PropertyChangedCallback(DataGridRowHelper.OnAreRowDetailsFrozenInternalChanged)));

		// Token: 0x0400066E RID: 1646
		private static readonly DependencyPropertyKey HeadersVisibilityPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HeadersVisibility", typeof(DataGridHeadersVisibility), typeof(DataGridRowHelper), new PropertyMetadata(3));

		// Token: 0x0400066F RID: 1647
		public static readonly DependencyProperty HeadersVisibilityProperty = DataGridRowHelper.HeadersVisibilityPropertyKey.DependencyProperty;

		// Token: 0x04000670 RID: 1648
		internal static readonly DependencyProperty HeadersVisibilityInternalProperty = DependencyProperty.RegisterAttached("HeadersVisibilityInternal", typeof(DataGridHeadersVisibility), typeof(DataGridRowHelper), new PropertyMetadata(3, new PropertyChangedCallback(DataGridRowHelper.OnHeadersVisibilityInternalChanged)));
	}
}
