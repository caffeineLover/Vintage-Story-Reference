using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020000F2 RID: 242
	public class ColumnMajorUniformToLargestGridLayout : NonVirtualizingLayout
	{
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0002A6DA File Offset: 0x000288DA
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x0002A6EC File Offset: 0x000288EC
		public double ColumnSpacing
		{
			get
			{
				return (double)base.GetValue(ColumnMajorUniformToLargestGridLayout.ColumnSpacingProperty);
			}
			set
			{
				base.SetValue(ColumnMajorUniformToLargestGridLayout.ColumnSpacingProperty, value);
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0002A6FF File Offset: 0x000288FF
		private static void OnColumnSpacingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ColumnMajorUniformToLargestGridLayout)sender).OnColumnSpacingPropertyChanged(args);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0002A70D File Offset: 0x0002890D
		private void OnColumnSpacingPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			base.InvalidateMeasure();
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0002A715 File Offset: 0x00028915
		// (set) Token: 0x06000C70 RID: 3184 RVA: 0x0002A727 File Offset: 0x00028927
		public double RowSpacing
		{
			get
			{
				return (double)base.GetValue(ColumnMajorUniformToLargestGridLayout.RowSpacingProperty);
			}
			set
			{
				base.SetValue(ColumnMajorUniformToLargestGridLayout.RowSpacingProperty, value);
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0002A73A File Offset: 0x0002893A
		private static void OnRowSpacingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ColumnMajorUniformToLargestGridLayout)sender).OnRowSpacingPropertyChanged(args);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0002A748 File Offset: 0x00028948
		private void OnRowSpacingPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			base.InvalidateMeasure();
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0002A750 File Offset: 0x00028950
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x0002A762 File Offset: 0x00028962
		public int MaxColumns
		{
			get
			{
				return (int)base.GetValue(ColumnMajorUniformToLargestGridLayout.MaxColumnsProperty);
			}
			set
			{
				base.SetValue(ColumnMajorUniformToLargestGridLayout.MaxColumnsProperty, value);
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0002A775 File Offset: 0x00028975
		private static void OnMaxColumnsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((ColumnMajorUniformToLargestGridLayout)sender).OnMaxColumnsPropertyChanged(args);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0002A783 File Offset: 0x00028983
		private void OnMaxColumnsPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			base.InvalidateMeasure();
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0002A78B File Offset: 0x0002898B
		private static bool ValidateMaxColumns(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0002A798 File Offset: 0x00028998
		protected override Size MeasureOverride(NonVirtualizingLayoutContext context, Size availableSize)
		{
			ColumnMajorUniformToLargestGridLayout.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.availableSize = availableSize;
			CS$<>8__locals1.children = context.Children;
			if (CS$<>8__locals1.children != null && CS$<>8__locals1.children.Count > 0)
			{
				this.m_largestChildSize = ColumnMajorUniformToLargestGridLayout.<MeasureOverride>g__calculateLargestChildSize|20_0(ref CS$<>8__locals1);
				this.m_actualColumnCount = this.CalculateColumns(CS$<>8__locals1.children.Count, this.m_largestChildSize.Width, CS$<>8__locals1.availableSize.Width);
				int num = (int)Math.Ceiling((double)CS$<>8__locals1.children.Count / (double)this.m_actualColumnCount);
				return new Size(this.m_largestChildSize.Width * (double)this.m_actualColumnCount + this.ColumnSpacing * (double)(this.m_actualColumnCount - 1), this.m_largestChildSize.Height * (double)num + this.RowSpacing * (double)(num - 1));
			}
			return new Size(0.0, 0.0);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0002A888 File Offset: 0x00028A88
		protected override Size ArrangeOverride(NonVirtualizingLayoutContext context, Size finalSize)
		{
			IReadOnlyList<UIElement> children = context.Children;
			if (children != null)
			{
				int count = children.Count;
				int num = (int)Math.Floor((double)count / (double)this.m_actualColumnCount);
				int num2 = count % this.m_actualColumnCount;
				double columnSpacing = this.ColumnSpacing;
				double rowSpacing = this.RowSpacing;
				double num3 = 0.0;
				double num4 = 0.0;
				int num5 = 0;
				int num6 = 0;
				foreach (UIElement uielement in children)
				{
					Size desiredSize = uielement.DesiredSize;
					uielement.Arrange(new Rect(num3, num4, desiredSize.Width, desiredSize.Height));
					if (num6 < num2)
					{
						if (num5 % (num + 1) == num)
						{
							num3 += this.m_largestChildSize.Width + columnSpacing;
							num4 = 0.0;
							num6++;
						}
						else
						{
							num4 += this.m_largestChildSize.Height + rowSpacing;
						}
					}
					else if ((num5 - num2 * (num + 1)) % num == num - 1)
					{
						num3 += this.m_largestChildSize.Width + columnSpacing;
						num4 = 0.0;
						num6++;
					}
					else
					{
						num4 += this.m_largestChildSize.Height + rowSpacing;
					}
					num5++;
				}
				if (this.m_testHooksEnabled && (this.m_largerColumns != num2 || this.m_columns != num6 || this.m_rows != num))
				{
					this.m_largerColumns = num2;
					this.m_columns = num6;
					this.m_rows = num;
					TypedEventHandler<ColumnMajorUniformToLargestGridLayout, object> layoutChanged = this.LayoutChanged;
					if (layoutChanged != null)
					{
						layoutChanged.Invoke(this, null);
					}
				}
			}
			return finalSize;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0002AA34 File Offset: 0x00028C34
		private int CalculateColumns(int childCount, double maxItemWidth, double availableWidth)
		{
			double num = this.ColumnSpacing + maxItemWidth;
			int num2 = Math.Min(this.MaxColumns, childCount);
			if (num < 5E-324)
			{
				return num2;
			}
			double num3 = availableWidth - maxItemWidth;
			double num4 = Math.Max(0.0, Math.Floor(num3 / num));
			double num5 = Math.Min((double)num2, num4 + 1.0);
			return Math.Max(1, (int)num5);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0002AA9D File Offset: 0x00028C9D
		internal void SetTestHooksEnabled(bool enabled)
		{
			this.m_testHooksEnabled = enabled;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0002AAA6 File Offset: 0x00028CA6
		internal int GetRows()
		{
			return this.m_rows;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0002AAAE File Offset: 0x00028CAE
		internal int GetColumns()
		{
			return this.m_columns;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0002AAB6 File Offset: 0x00028CB6
		internal int GetLargerColumns()
		{
			return this.m_largerColumns;
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000C7F RID: 3199 RVA: 0x0002AAC0 File Offset: 0x00028CC0
		// (remove) Token: 0x06000C80 RID: 3200 RVA: 0x0002AAF8 File Offset: 0x00028CF8
		internal event TypedEventHandler<ColumnMajorUniformToLargestGridLayout, object> LayoutChanged;

		// Token: 0x06000C82 RID: 3202 RVA: 0x0002ABEC File Offset: 0x00028DEC
		[CompilerGenerated]
		internal static Size <MeasureOverride>g__calculateLargestChildSize|20_0(ref ColumnMajorUniformToLargestGridLayout.<>c__DisplayClass20_0 A_0)
		{
			double num = 0.0;
			double num2 = 0.0;
			foreach (UIElement uielement in A_0.children)
			{
				uielement.Measure(A_0.availableSize);
				Size desiredSize = uielement.DesiredSize;
				if (desiredSize.Width > num)
				{
					num = desiredSize.Width;
				}
				if (desiredSize.Height > num2)
				{
					num2 = desiredSize.Height;
				}
			}
			return new Size(num, num2);
		}

		// Token: 0x0400053C RID: 1340
		public static readonly DependencyProperty ColumnSpacingProperty = DependencyProperty.Register("ColumnSpacing", typeof(double), typeof(ColumnMajorUniformToLargestGridLayout), new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnMajorUniformToLargestGridLayout.OnColumnSpacingPropertyChanged)));

		// Token: 0x0400053D RID: 1341
		public static readonly DependencyProperty RowSpacingProperty = DependencyProperty.Register("RowSpacing", typeof(double), typeof(ColumnMajorUniformToLargestGridLayout), new FrameworkPropertyMetadata(new PropertyChangedCallback(ColumnMajorUniformToLargestGridLayout.OnRowSpacingPropertyChanged)));

		// Token: 0x0400053E RID: 1342
		public static readonly DependencyProperty MaxColumnsProperty = DependencyProperty.Register("MaxColumns", typeof(int), typeof(ColumnMajorUniformToLargestGridLayout), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(ColumnMajorUniformToLargestGridLayout.OnMaxColumnsPropertyChanged)), new ValidateValueCallback(ColumnMajorUniformToLargestGridLayout.ValidateMaxColumns));

		// Token: 0x04000540 RID: 1344
		private int m_actualColumnCount = 1;

		// Token: 0x04000541 RID: 1345
		private Size m_largestChildSize;

		// Token: 0x04000542 RID: 1346
		private bool m_testHooksEnabled;

		// Token: 0x04000543 RID: 1347
		private int m_rows = -1;

		// Token: 0x04000544 RID: 1348
		private int m_columns = -1;

		// Token: 0x04000545 RID: 1349
		private int m_largerColumns = -1;
	}
}
