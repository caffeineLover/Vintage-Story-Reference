using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000284 RID: 644
	public static class ColumnDefinitionHelper
	{
		// Token: 0x060012CF RID: 4815 RVA: 0x00037203 File Offset: 0x00035403
		public static double GetPixelWidth(ColumnDefinition columnDefinition)
		{
			return (double)columnDefinition.GetValue(ColumnDefinitionHelper.PixelWidthProperty);
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00037215 File Offset: 0x00035415
		public static void SetPixelWidth(ColumnDefinition columnDefinition, double value)
		{
			columnDefinition.SetValue(ColumnDefinitionHelper.PixelWidthProperty, value);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00037228 File Offset: 0x00035428
		private static void OnPixelWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColumnDefinition columnDefinition = (ColumnDefinition)d;
			double num = (double)e.NewValue;
			if (double.IsNaN(num) || double.IsInfinity(num))
			{
				columnDefinition.ClearValue(ColumnDefinition.WidthProperty);
				return;
			}
			columnDefinition.Width = new GridLength(num);
		}

		// Token: 0x04000626 RID: 1574
		public static readonly DependencyProperty PixelWidthProperty = DependencyProperty.RegisterAttached("PixelWidth", typeof(double), typeof(ColumnDefinitionHelper), new PropertyMetadata(double.NaN, new PropertyChangedCallback(ColumnDefinitionHelper.OnPixelWidthChanged)));
	}
}
