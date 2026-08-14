using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000299 RID: 665
	public static class PivotHelper
	{
		// Token: 0x060013CD RID: 5069 RVA: 0x0003A638 File Offset: 0x00038838
		public static object GetTitle(TabControl tabControl)
		{
			return tabControl.GetValue(PivotHelper.TitleProperty);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0003A645 File Offset: 0x00038845
		public static void SetTitle(TabControl tabControl, object value)
		{
			tabControl.SetValue(PivotHelper.TitleProperty, value);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0003A653 File Offset: 0x00038853
		public static DataTemplate GetTitleTemplate(TabControl tabControl)
		{
			return (DataTemplate)tabControl.GetValue(PivotHelper.TitleTemplateProperty);
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0003A665 File Offset: 0x00038865
		public static void SetTitleTemplate(TabControl tabControl, DataTemplate value)
		{
			tabControl.SetValue(PivotHelper.TitleTemplateProperty, value);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0003A673 File Offset: 0x00038873
		public static object GetLeftHeader(TabControl tabControl)
		{
			return tabControl.GetValue(PivotHelper.LeftHeaderProperty);
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0003A680 File Offset: 0x00038880
		public static void SetLeftHeader(TabControl tabControl, object value)
		{
			tabControl.SetValue(PivotHelper.LeftHeaderProperty, value);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0003A68E File Offset: 0x0003888E
		public static DataTemplate GetLeftHeaderTemplate(TabControl tabControl)
		{
			return (DataTemplate)tabControl.GetValue(PivotHelper.LeftHeaderTemplateProperty);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0003A6A0 File Offset: 0x000388A0
		public static void SetLeftHeaderTemplate(TabControl tabControl, DataTemplate value)
		{
			tabControl.SetValue(PivotHelper.LeftHeaderTemplateProperty, value);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0003A6AE File Offset: 0x000388AE
		public static object GetRightHeader(TabControl tabControl)
		{
			return tabControl.GetValue(PivotHelper.RightHeaderProperty);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0003A6BB File Offset: 0x000388BB
		public static void SetRightHeader(TabControl tabControl, object value)
		{
			tabControl.SetValue(PivotHelper.RightHeaderProperty, value);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x0003A6C9 File Offset: 0x000388C9
		public static DataTemplate GetRightHeaderTemplate(TabControl tabControl)
		{
			return (DataTemplate)tabControl.GetValue(PivotHelper.RightHeaderTemplateProperty);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0003A6DB File Offset: 0x000388DB
		public static void SetRightHeaderTemplate(TabControl tabControl, DataTemplate value)
		{
			tabControl.SetValue(PivotHelper.RightHeaderTemplateProperty, value);
		}

		// Token: 0x040006A4 RID: 1700
		public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached("Title", typeof(object), typeof(PivotHelper));

		// Token: 0x040006A5 RID: 1701
		public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.RegisterAttached("TitleTemplate", typeof(DataTemplate), typeof(PivotHelper));

		// Token: 0x040006A6 RID: 1702
		public static readonly DependencyProperty LeftHeaderProperty = DependencyProperty.RegisterAttached("LeftHeader", typeof(object), typeof(PivotHelper));

		// Token: 0x040006A7 RID: 1703
		public static readonly DependencyProperty LeftHeaderTemplateProperty = DependencyProperty.RegisterAttached("LeftHeaderTemplate", typeof(DataTemplate), typeof(PivotHelper));

		// Token: 0x040006A8 RID: 1704
		public static readonly DependencyProperty RightHeaderProperty = DependencyProperty.RegisterAttached("RightHeader", typeof(object), typeof(PivotHelper));

		// Token: 0x040006A9 RID: 1705
		public static readonly DependencyProperty RightHeaderTemplateProperty = DependencyProperty.RegisterAttached("RightHeaderTemplate", typeof(DataTemplate), typeof(PivotHelper));
	}
}
