using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x0200028C RID: 652
	public static class DataGridHelper
	{
		// Token: 0x06001322 RID: 4898 RVA: 0x00038346 File Offset: 0x00036546
		public static bool GetIsEnabled(DataGrid dataGrid)
		{
			return (bool)dataGrid.GetValue(DataGridHelper.IsEnabledProperty);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00038358 File Offset: 0x00036558
		public static void SetIsEnabled(DataGrid dataGrid, bool value)
		{
			dataGrid.SetValue(DataGridHelper.IsEnabledProperty, value);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0003836C File Offset: 0x0003656C
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			if ((bool)e.NewValue)
			{
				dataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGridHelper.OnLoadingRow);
				return;
			}
			dataGrid.LoadingRow -= new EventHandler<DataGridRowEventArgs>(DataGridHelper.OnLoadingRow);
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x000383B4 File Offset: 0x000365B4
		private static void OnLoadingRow(object sender, DataGridRowEventArgs e)
		{
			DataGridRow row = e.Row;
			if (row.ReadLocalValue(DataGridRowHelper.AreRowDetailsFrozenInternalProperty) == DependencyProperty.UnsetValue)
			{
				row.SetBinding(DataGridRowHelper.AreRowDetailsFrozenInternalProperty, new Binding
				{
					Path = new PropertyPath(DataGrid.AreRowDetailsFrozenProperty),
					Source = sender
				});
			}
			if (row.ReadLocalValue(DataGridRowHelper.HeadersVisibilityInternalProperty) == DependencyProperty.UnsetValue)
			{
				row.SetBinding(DataGridRowHelper.HeadersVisibilityInternalProperty, new Binding
				{
					Path = new PropertyPath(DataGrid.HeadersVisibilityProperty),
					Source = sender
				});
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0003843C File Offset: 0x0003663C
		public static Style GetTextColumnElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.TextColumnElementStyleProperty);
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0003844E File Offset: 0x0003664E
		public static void SetTextColumnElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.TextColumnElementStyleProperty, value);
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0003845C File Offset: 0x0003665C
		public static Style GetTextColumnEditingElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.TextColumnEditingElementStyleProperty);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0003846E File Offset: 0x0003666E
		public static void SetTextColumnEditingElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.TextColumnEditingElementStyleProperty, value);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0003847C File Offset: 0x0003667C
		public static double GetTextColumnFontSize(DataGrid dataGrid)
		{
			return (double)dataGrid.GetValue(DataGridHelper.TextColumnFontSizeProperty);
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0003848E File Offset: 0x0003668E
		public static void SetTextColumnFontSize(DataGrid dataGrid, double value)
		{
			dataGrid.SetValue(DataGridHelper.TextColumnFontSizeProperty, value);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x000384A1 File Offset: 0x000366A1
		public static Style GetCheckBoxColumnElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.CheckBoxColumnElementStyleProperty);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x000384B3 File Offset: 0x000366B3
		public static void SetCheckBoxColumnElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.CheckBoxColumnElementStyleProperty, value);
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000384C1 File Offset: 0x000366C1
		public static Style GetCheckBoxColumnEditingElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.CheckBoxColumnEditingElementStyleProperty);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x000384D3 File Offset: 0x000366D3
		public static void SetCheckBoxColumnEditingElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.CheckBoxColumnEditingElementStyleProperty, value);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000384E1 File Offset: 0x000366E1
		public static Style GetComboBoxColumnElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.ComboBoxColumnElementStyleProperty);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000384F3 File Offset: 0x000366F3
		public static void SetComboBoxColumnElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.ComboBoxColumnElementStyleProperty, value);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00038501 File Offset: 0x00036701
		public static Style GetComboBoxColumnEditingElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.ComboBoxColumnEditingElementStyleProperty);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00038513 File Offset: 0x00036713
		public static void SetComboBoxColumnEditingElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.ComboBoxColumnEditingElementStyleProperty, value);
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x00038521 File Offset: 0x00036721
		public static Style GetHyperlinkColumnElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.HyperlinkColumnElementStyleProperty);
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00038533 File Offset: 0x00036733
		public static void SetHyperlinkColumnElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.HyperlinkColumnElementStyleProperty, value);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00038541 File Offset: 0x00036741
		public static Style GetHyperlinkColumnEditingElementStyle(DataGrid dataGrid)
		{
			return (Style)dataGrid.GetValue(DataGridHelper.HyperlinkColumnEditingElementStyleProperty);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00038553 File Offset: 0x00036753
		public static void SetHyperlinkColumnEditingElementStyle(DataGrid dataGrid, Style value)
		{
			dataGrid.SetValue(DataGridHelper.HyperlinkColumnEditingElementStyleProperty, value);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00038561 File Offset: 0x00036761
		public static bool GetUseModernColumnStyles(DataGrid dataGrid)
		{
			return (bool)dataGrid.GetValue(DataGridHelper.UseModernColumnStylesProperty);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00038573 File Offset: 0x00036773
		public static void SetUseModernColumnStyles(DataGrid dataGrid, bool value)
		{
			dataGrid.SetValue(DataGridHelper.UseModernColumnStylesProperty, value);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x00038588 File Offset: 0x00036788
		private static void OnUseModernColumnStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			if ((bool)e.NewValue)
			{
				dataGrid.SetValue(DataGridHelper.ColumnStylesHelperProperty, new DataGridHelper.ColumnStylesHelper(dataGrid));
				return;
			}
			dataGrid.ClearValue(DataGridHelper.ColumnStylesHelperProperty);
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000385C8 File Offset: 0x000367C8
		private static void OnColumnStylesHelperChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridHelper.ColumnStylesHelper columnStylesHelper = e.OldValue as DataGridHelper.ColumnStylesHelper;
			if (columnStylesHelper != null)
			{
				columnStylesHelper.Detach();
			}
			DataGridHelper.ColumnStylesHelper columnStylesHelper2 = e.NewValue as DataGridHelper.ColumnStylesHelper;
			if (columnStylesHelper2 != null)
			{
				columnStylesHelper2.Attach();
			}
		}

		// Token: 0x0400065F RID: 1631
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(false, new PropertyChangedCallback(DataGridHelper.OnIsEnabledChanged)));

		// Token: 0x04000660 RID: 1632
		public static readonly DependencyProperty TextColumnElementStyleProperty = DependencyProperty.RegisterAttached("TextColumnElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000661 RID: 1633
		public static readonly DependencyProperty TextColumnEditingElementStyleProperty = DependencyProperty.RegisterAttached("TextColumnEditingElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000662 RID: 1634
		public static readonly DependencyProperty TextColumnFontSizeProperty = DependencyProperty.RegisterAttached("TextColumnFontSize", typeof(double), typeof(DataGridHelper), new PropertyMetadata(SystemFonts.MessageFontSize));

		// Token: 0x04000663 RID: 1635
		public static readonly DependencyProperty CheckBoxColumnElementStyleProperty = DependencyProperty.RegisterAttached("CheckBoxColumnElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000664 RID: 1636
		public static readonly DependencyProperty CheckBoxColumnEditingElementStyleProperty = DependencyProperty.RegisterAttached("CheckBoxColumnEditingElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000665 RID: 1637
		public static readonly DependencyProperty ComboBoxColumnElementStyleProperty = DependencyProperty.RegisterAttached("ComboBoxColumnElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000666 RID: 1638
		public static readonly DependencyProperty ComboBoxColumnEditingElementStyleProperty = DependencyProperty.RegisterAttached("ComboBoxColumnEditingElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000667 RID: 1639
		public static readonly DependencyProperty HyperlinkColumnElementStyleProperty = DependencyProperty.RegisterAttached("HyperlinkColumnElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000668 RID: 1640
		public static readonly DependencyProperty HyperlinkColumnEditingElementStyleProperty = DependencyProperty.RegisterAttached("HyperlinkColumnEditingElementStyle", typeof(Style), typeof(DataGridHelper));

		// Token: 0x04000669 RID: 1641
		public static readonly DependencyProperty UseModernColumnStylesProperty = DependencyProperty.RegisterAttached("UseModernColumnStyles", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(new PropertyChangedCallback(DataGridHelper.OnUseModernColumnStylesChanged)));

		// Token: 0x0400066A RID: 1642
		private static readonly DependencyProperty ColumnStylesHelperProperty = DependencyProperty.RegisterAttached("ColumnStylesHelper", typeof(DataGridHelper.ColumnStylesHelper), typeof(DataGridHelper), new PropertyMetadata(new PropertyChangedCallback(DataGridHelper.OnColumnStylesHelperChanged)));

		// Token: 0x02000415 RID: 1045
		private class ColumnStylesHelper
		{
			// Token: 0x06001BF5 RID: 7157 RVA: 0x0004AC0B File Offset: 0x00048E0B
			public ColumnStylesHelper(DataGrid dataGrid)
			{
				this._dataGrid = dataGrid;
			}

			// Token: 0x06001BF6 RID: 7158 RVA: 0x0004AC1C File Offset: 0x00048E1C
			public void Attach()
			{
				this._dataGrid.Columns.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnColumnsCollectionChanged);
				foreach (DataGridColumn column in this._dataGrid.Columns)
				{
					this.BindColumnStyleProperties(column);
				}
			}

			// Token: 0x06001BF7 RID: 7159 RVA: 0x0004AC8C File Offset: 0x00048E8C
			public void Detach()
			{
				this._dataGrid.Columns.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnColumnsCollectionChanged);
				foreach (DataGridColumn column in this._dataGrid.Columns)
				{
					this.ClearColumnStyleProperties(column);
				}
			}

			// Token: 0x06001BF8 RID: 7160 RVA: 0x0004ACFC File Offset: 0x00048EFC
			private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				if (e.NewItems != null)
				{
					foreach (object obj in e.NewItems)
					{
						this.BindColumnStyleProperties(obj as DataGridColumn);
					}
				}
			}

			// Token: 0x06001BF9 RID: 7161 RVA: 0x0004AD60 File Offset: 0x00048F60
			private void BindColumnStyleProperties(DataGridColumn column)
			{
				DataGridTextColumn dataGridTextColumn = column as DataGridTextColumn;
				if (dataGridTextColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Bind(dataGridTextColumn, DataGridBoundColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.TextColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Bind(dataGridTextColumn, DataGridBoundColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.TextColumnEditingElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Bind(dataGridTextColumn, DataGridTextColumn.FontSizeProperty, this._dataGrid, DataGridHelper.TextColumnFontSizeProperty);
					return;
				}
				DataGridCheckBoxColumn dataGridCheckBoxColumn = column as DataGridCheckBoxColumn;
				if (dataGridCheckBoxColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Bind(dataGridCheckBoxColumn, DataGridBoundColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.CheckBoxColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Bind(dataGridCheckBoxColumn, DataGridBoundColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.CheckBoxColumnEditingElementStyleProperty);
					return;
				}
				DataGridComboBoxColumn dataGridComboBoxColumn = column as DataGridComboBoxColumn;
				if (dataGridComboBoxColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Bind(dataGridComboBoxColumn, DataGridComboBoxColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.ComboBoxColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Bind(dataGridComboBoxColumn, DataGridComboBoxColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.ComboBoxColumnEditingElementStyleProperty);
					return;
				}
				DataGridHyperlinkColumn dataGridHyperlinkColumn = column as DataGridHyperlinkColumn;
				if (dataGridHyperlinkColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Bind(dataGridHyperlinkColumn, DataGridBoundColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.HyperlinkColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Bind(dataGridHyperlinkColumn, DataGridBoundColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.HyperlinkColumnEditingElementStyleProperty);
				}
			}

			// Token: 0x06001BFA RID: 7162 RVA: 0x0004AE60 File Offset: 0x00049060
			private void ClearColumnStyleProperties(DataGridColumn column)
			{
				DataGridTextColumn dataGridTextColumn = column as DataGridTextColumn;
				if (dataGridTextColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Clear(dataGridTextColumn, DataGridBoundColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.TextColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Clear(dataGridTextColumn, DataGridBoundColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.TextColumnEditingElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Clear(dataGridTextColumn, DataGridTextColumn.FontSizeProperty, this._dataGrid, DataGridHelper.TextColumnFontSizeProperty);
					return;
				}
				DataGridCheckBoxColumn dataGridCheckBoxColumn = column as DataGridCheckBoxColumn;
				if (dataGridCheckBoxColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Clear(dataGridCheckBoxColumn, DataGridBoundColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.CheckBoxColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Clear(dataGridCheckBoxColumn, DataGridBoundColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.CheckBoxColumnEditingElementStyleProperty);
					return;
				}
				DataGridComboBoxColumn dataGridComboBoxColumn = column as DataGridComboBoxColumn;
				if (dataGridComboBoxColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Clear(dataGridComboBoxColumn, DataGridComboBoxColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.ComboBoxColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Clear(dataGridComboBoxColumn, DataGridComboBoxColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.ComboBoxColumnEditingElementStyleProperty);
					return;
				}
				DataGridHyperlinkColumn dataGridHyperlinkColumn = column as DataGridHyperlinkColumn;
				if (dataGridHyperlinkColumn != null)
				{
					DataGridHelper.ColumnStylesHelper.Clear(dataGridHyperlinkColumn, DataGridBoundColumn.ElementStyleProperty, this._dataGrid, DataGridHelper.HyperlinkColumnElementStyleProperty);
					DataGridHelper.ColumnStylesHelper.Clear(dataGridHyperlinkColumn, DataGridBoundColumn.EditingElementStyleProperty, this._dataGrid, DataGridHelper.HyperlinkColumnEditingElementStyleProperty);
				}
			}

			// Token: 0x06001BFB RID: 7163 RVA: 0x0004AF5E File Offset: 0x0004915E
			private static void Bind(DependencyObject target, DependencyProperty targetDP, DependencyObject source, DependencyProperty sourceDP)
			{
				if (target.ReadLocalValue(targetDP) == DependencyProperty.UnsetValue)
				{
					BindingOperations.SetBinding(target, targetDP, new Binding
					{
						Path = new PropertyPath(sourceDP),
						Source = source
					});
				}
			}

			// Token: 0x06001BFC RID: 7164 RVA: 0x0004AF90 File Offset: 0x00049190
			private static void Clear(DependencyObject target, DependencyProperty targetDP, DependencyObject source, DependencyProperty sourceDP)
			{
				Binding binding = BindingOperations.GetBinding(target, targetDP);
				if (binding != null && binding.Source == source)
				{
					target.ClearValue(targetDP);
				}
			}

			// Token: 0x04000A15 RID: 2581
			private readonly DataGrid _dataGrid;
		}
	}
}
