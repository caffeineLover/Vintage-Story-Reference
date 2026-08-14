using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200029F RID: 671
	public static class RowDefinitionHelper
	{
		// Token: 0x06001421 RID: 5153 RVA: 0x0003BCC8 File Offset: 0x00039EC8
		public static double GetPixelHeight(RowDefinition rowDefinition)
		{
			return (double)rowDefinition.GetValue(RowDefinitionHelper.PixelHeightProperty);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0003BCDA File Offset: 0x00039EDA
		public static void SetPixelHeight(RowDefinition rowDefinition, double value)
		{
			rowDefinition.SetValue(RowDefinitionHelper.PixelHeightProperty, value);
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0003BCF0 File Offset: 0x00039EF0
		private static void OnPixelHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RowDefinition rowDefinition = (RowDefinition)d;
			double num = (double)e.NewValue;
			if (double.IsNaN(num) || double.IsInfinity(num))
			{
				rowDefinition.ClearValue(RowDefinition.HeightProperty);
				return;
			}
			rowDefinition.Height = new GridLength(num);
		}

		// Token: 0x040006BC RID: 1724
		public static readonly DependencyProperty PixelHeightProperty = DependencyProperty.RegisterAttached("PixelHeight", typeof(double), typeof(RowDefinitionHelper), new PropertyMetadata(double.NaN, new PropertyChangedCallback(RowDefinitionHelper.OnPixelHeightChanged)));
	}
}
