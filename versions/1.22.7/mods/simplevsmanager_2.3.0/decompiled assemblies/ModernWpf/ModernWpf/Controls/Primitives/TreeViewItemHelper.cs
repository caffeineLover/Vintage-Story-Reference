using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002AB RID: 683
	public static class TreeViewItemHelper
	{
		// Token: 0x0600149E RID: 5278 RVA: 0x0003D8C7 File Offset: 0x0003BAC7
		public static bool GetIsEnabled(TreeViewItem treeViewItem)
		{
			return (bool)treeViewItem.GetValue(TreeViewItemHelper.IsEnabledProperty);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0003D8D9 File Offset: 0x0003BAD9
		public static void SetIsEnabled(TreeViewItem treeViewItem, bool value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.IsEnabledProperty, value);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0003D8EC File Offset: 0x0003BAEC
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem treeViewItem = (TreeViewItem)d;
			if ((bool)e.NewValue)
			{
				treeViewItem.IsVisibleChanged += new DependencyPropertyChangedEventHandler(TreeViewItemHelper.OnTreeViewItemIsVisibleChanged);
				if (treeViewItem.IsVisible)
				{
					TreeViewItemHelper.UpdateIndentation(treeViewItem);
					return;
				}
			}
			else
			{
				treeViewItem.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(TreeViewItemHelper.OnTreeViewItemIsVisibleChanged);
				treeViewItem.ClearValue(TreeViewItemHelper.IndentationPropertyKey);
			}
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0003D94C File Offset: 0x0003BB4C
		private static void OnTreeViewItemIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				TreeViewItemHelper.UpdateIndentation((TreeViewItem)sender);
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0003D967 File Offset: 0x0003BB67
		[Obsolete]
		[EditorBrowsable(1)]
		public static string GetCollapsedGlyph(TreeViewItem treeViewItem)
		{
			return (string)treeViewItem.GetValue(TreeViewItemHelper.CollapsedGlyphProperty);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0003D979 File Offset: 0x0003BB79
		[Obsolete]
		[EditorBrowsable(1)]
		public static void SetCollapsedGlyph(TreeViewItem treeViewItem, string value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.CollapsedGlyphProperty, value);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0003D987 File Offset: 0x0003BB87
		[Obsolete]
		[EditorBrowsable(1)]
		public static string GetExpandedGlyph(TreeViewItem treeViewItem)
		{
			return (string)treeViewItem.GetValue(TreeViewItemHelper.ExpandedGlyphProperty);
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0003D999 File Offset: 0x0003BB99
		[Obsolete]
		[EditorBrowsable(1)]
		public static void SetExpandedGlyph(TreeViewItem treeViewItem, string value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.ExpandedGlyphProperty, value);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0003D9A7 File Offset: 0x0003BBA7
		public static Geometry GetCollapsedPath(TreeViewItem treeViewItem)
		{
			return (Geometry)treeViewItem.GetValue(TreeViewItemHelper.CollapsedPathProperty);
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0003D9B9 File Offset: 0x0003BBB9
		public static void SetCollapsedPath(TreeViewItem treeViewItem, Geometry value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.CollapsedPathProperty, value);
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0003D9C7 File Offset: 0x0003BBC7
		public static Geometry GetExpandedPath(TreeViewItem treeViewItem)
		{
			return (Geometry)treeViewItem.GetValue(TreeViewItemHelper.ExpandedPathProperty);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0003D9D9 File Offset: 0x0003BBD9
		public static void SetExpandedPath(TreeViewItem treeViewItem, Geometry value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.ExpandedPathProperty, value);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0003D9E7 File Offset: 0x0003BBE7
		public static Brush GetGlyphBrush(TreeViewItem treeViewItem)
		{
			return (Brush)treeViewItem.GetValue(TreeViewItemHelper.GlyphBrushProperty);
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0003D9F9 File Offset: 0x0003BBF9
		public static void SetGlyphBrush(TreeViewItem treeViewItem, Brush value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.GlyphBrushProperty, value);
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0003DA07 File Offset: 0x0003BC07
		public static double GetGlyphOpacity(TreeViewItem treeViewItem)
		{
			return (double)treeViewItem.GetValue(TreeViewItemHelper.GlyphOpacityProperty);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0003DA19 File Offset: 0x0003BC19
		public static void SetGlyphOpacity(TreeViewItem treeViewItem, double value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.GlyphOpacityProperty, value);
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0003DA2C File Offset: 0x0003BC2C
		public static double GetGlyphSize(TreeViewItem treeViewItem)
		{
			return (double)treeViewItem.GetValue(TreeViewItemHelper.GlyphSizeProperty);
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0003DA3E File Offset: 0x0003BC3E
		public static void SetGlyphSize(TreeViewItem treeViewItem, double value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.GlyphSizeProperty, value);
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0003DA51 File Offset: 0x0003BC51
		public static Thickness GetIndentation(TreeViewItem treeViewItem)
		{
			return (Thickness)treeViewItem.GetValue(TreeViewItemHelper.IndentationProperty);
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0003DA63 File Offset: 0x0003BC63
		private static void SetIndentation(TreeViewItem treeViewItem, Thickness value)
		{
			treeViewItem.SetValue(TreeViewItemHelper.IndentationPropertyKey, value);
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0003DA76 File Offset: 0x0003BC76
		private static void UpdateIndentation(TreeViewItem item)
		{
			TreeViewItemHelper.SetIndentation(item, new Thickness((double)(TreeViewItemHelper.GetDepth(item) * 16), 0.0, 0.0, 0.0));
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0003DAA8 File Offset: 0x0003BCA8
		private static int GetDepth(TreeViewItem item)
		{
			int num = 0;
			for (;;)
			{
				TreeViewItem treeViewItem = ItemsControl.ItemsControlFromItemContainer(item) as TreeViewItem;
				if (treeViewItem == null)
				{
					break;
				}
				num++;
				item = treeViewItem;
			}
			return num;
		}

		// Token: 0x040006EA RID: 1770
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TreeViewItemHelper), new PropertyMetadata(new PropertyChangedCallback(TreeViewItemHelper.OnIsEnabledChanged)));

		// Token: 0x040006EB RID: 1771
		[Obsolete]
		[EditorBrowsable(1)]
		public static readonly DependencyProperty CollapsedGlyphProperty = DependencyProperty.RegisterAttached("CollapsedGlyph", typeof(string), typeof(TreeViewItemHelper), new PropertyMetadata(""));

		// Token: 0x040006EC RID: 1772
		[Obsolete]
		[EditorBrowsable(1)]
		public static readonly DependencyProperty ExpandedGlyphProperty = DependencyProperty.RegisterAttached("ExpandedGlyph", typeof(string), typeof(TreeViewItemHelper), new PropertyMetadata(""));

		// Token: 0x040006ED RID: 1773
		public static readonly DependencyProperty CollapsedPathProperty = DependencyProperty.RegisterAttached("CollapsedPath", typeof(Geometry), typeof(TreeViewItemHelper));

		// Token: 0x040006EE RID: 1774
		public static readonly DependencyProperty ExpandedPathProperty = DependencyProperty.RegisterAttached("ExpandedPath", typeof(Geometry), typeof(TreeViewItemHelper));

		// Token: 0x040006EF RID: 1775
		public static readonly DependencyProperty GlyphBrushProperty = DependencyProperty.RegisterAttached("GlyphBrush", typeof(Brush), typeof(TreeViewItemHelper), null);

		// Token: 0x040006F0 RID: 1776
		public static readonly DependencyProperty GlyphOpacityProperty = DependencyProperty.RegisterAttached("GlyphOpacity", typeof(double), typeof(TreeViewItemHelper), new PropertyMetadata(1.0));

		// Token: 0x040006F1 RID: 1777
		public static readonly DependencyProperty GlyphSizeProperty = DependencyProperty.RegisterAttached("GlyphSize", typeof(double), typeof(TreeViewItemHelper), new PropertyMetadata(12.0));

		// Token: 0x040006F2 RID: 1778
		private static readonly DependencyPropertyKey IndentationPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Indentation", typeof(Thickness), typeof(TreeViewItemHelper), null);

		// Token: 0x040006F3 RID: 1779
		public static readonly DependencyProperty IndentationProperty = TreeViewItemHelper.IndentationPropertyKey.DependencyProperty;
	}
}
