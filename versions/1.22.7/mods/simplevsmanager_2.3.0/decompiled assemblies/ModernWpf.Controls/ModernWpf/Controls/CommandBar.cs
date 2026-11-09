using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using ModernWpf.Controls.Primitives;

namespace ModernWpf.Controls
{
	// Token: 0x02000022 RID: 34
	[ContentProperty("PrimaryCommands")]
	[TemplatePart(Name = "PART_ToolBar", Type = typeof(CommandBarToolBar))]
	public class CommandBar : Control
	{
		// Token: 0x06000177 RID: 375 RVA: 0x00005D90 File Offset: 0x00003F90
		static CommandBar()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandBar), new FrameworkPropertyMetadata(typeof(CommandBar)));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00005ED4 File Offset: 0x000040D4
		public CommandBar()
		{
			this.PrimaryCommands = new ObservableCollection<ICommandBarElement>();
			this.PrimaryCommands.CollectionChanged += new NotifyCollectionChangedEventHandler(this.PrimaryCommands_CollectionChanged);
			this.SecondaryCommands = new ObservableCollection<ICommandBarElement>();
			this.SecondaryCommands.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SecondaryCommands_CollectionChanged);
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00005F2B File Offset: 0x0000412B
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00005F38 File Offset: 0x00004138
		public object Content
		{
			get
			{
				return base.GetValue(CommandBar.ContentProperty);
			}
			set
			{
				base.SetValue(CommandBar.ContentProperty, value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00005F46 File Offset: 0x00004146
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00005F58 File Offset: 0x00004158
		public DataTemplate ContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(CommandBar.ContentTemplateProperty);
			}
			set
			{
				base.SetValue(CommandBar.ContentTemplateProperty, value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00005F66 File Offset: 0x00004166
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00005F78 File Offset: 0x00004178
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(CommandBar.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(CommandBar.CornerRadiusProperty, value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00005F8B File Offset: 0x0000418B
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00005F9D File Offset: 0x0000419D
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(CommandBar.IsOpenProperty);
			}
			set
			{
				base.SetValue(CommandBar.IsOpenProperty, value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00005FB0 File Offset: 0x000041B0
		public ObservableCollection<ICommandBarElement> PrimaryCommands { get; }

		// Token: 0x06000182 RID: 386 RVA: 0x00005FB8 File Offset: 0x000041B8
		private void PrimaryCommands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				this.UpdateOverflowModeForPrimaryCommands(Enumerable.OfType<DependencyObject>(e.NewItems));
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00005FD3 File Offset: 0x000041D3
		private bool HasPrimaryCommands
		{
			get
			{
				return this.m_toolBar != null && this.m_toolBar.HasPrimaryCommands;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00005FEA File Offset: 0x000041EA
		public ObservableCollection<ICommandBarElement> SecondaryCommands { get; }

		// Token: 0x06000185 RID: 389 RVA: 0x00005FF2 File Offset: 0x000041F2
		private void SecondaryCommands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				this.UpdateOverflowModeForSecondaryCommands(Enumerable.OfType<DependencyObject>(e.NewItems));
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000600D File Offset: 0x0000420D
		private bool HasSecondaryCommands
		{
			get
			{
				return this.m_toolBar != null && this.m_toolBar.HasOverflowItems;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006024 File Offset: 0x00004224
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00006036 File Offset: 0x00004236
		public Style CommandBarOverflowPresenterStyle
		{
			get
			{
				return (Style)base.GetValue(CommandBar.CommandBarOverflowPresenterStyleProperty);
			}
			set
			{
				base.SetValue(CommandBar.CommandBarOverflowPresenterStyleProperty, value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006044 File Offset: 0x00004244
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00006056 File Offset: 0x00004256
		public CommandBarDefaultLabelPosition DefaultLabelPosition
		{
			get
			{
				return (CommandBarDefaultLabelPosition)base.GetValue(CommandBar.DefaultLabelPositionProperty);
			}
			set
			{
				base.SetValue(CommandBar.DefaultLabelPositionProperty, value);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006069 File Offset: 0x00004269
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000607B File Offset: 0x0000427B
		public bool IsDynamicOverflowEnabled
		{
			get
			{
				return (bool)base.GetValue(CommandBar.IsDynamicOverflowEnabledProperty);
			}
			set
			{
				base.SetValue(CommandBar.IsDynamicOverflowEnabledProperty, value);
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000608E File Offset: 0x0000428E
		private static void OnIsDynamicOverflowEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CommandBar)d).OnIsDynamicOverflowEnabledChanged();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000609B File Offset: 0x0000429B
		private void OnIsDynamicOverflowEnabledChanged()
		{
			this.UpdateOverflowModeForPrimaryCommands(Enumerable.OfType<DependencyObject>(this.PrimaryCommands));
			this.UpdateOverflowModeForSecondaryCommands(Enumerable.OfType<DependencyObject>(this.SecondaryCommands));
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000060C0 File Offset: 0x000042C0
		private void UpdateOverflowModeForPrimaryCommands(IEnumerable<DependencyObject> items)
		{
			bool isDynamicOverflowEnabled = this.IsDynamicOverflowEnabled;
			foreach (DependencyObject dependencyObject in items)
			{
				ToolBar.SetOverflowMode(dependencyObject, isDynamicOverflowEnabled ? 0 : 2);
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006114 File Offset: 0x00004314
		private void UpdateOverflowModeForSecondaryCommands(IEnumerable<DependencyObject> items)
		{
			foreach (DependencyObject dependencyObject in items)
			{
				ToolBar.SetOverflowMode(dependencyObject, 1);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000615C File Offset: 0x0000435C
		// (set) Token: 0x06000192 RID: 402 RVA: 0x0000616E File Offset: 0x0000436E
		public CommandBarOverflowButtonVisibility OverflowButtonVisibility
		{
			get
			{
				return (CommandBarOverflowButtonVisibility)base.GetValue(CommandBar.OverflowButtonVisibilityProperty);
			}
			set
			{
				base.SetValue(CommandBar.OverflowButtonVisibilityProperty, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000193 RID: 403 RVA: 0x00006184 File Offset: 0x00004384
		// (remove) Token: 0x06000194 RID: 404 RVA: 0x000061BC File Offset: 0x000043BC
		public event EventHandler<object> Opened;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000195 RID: 405 RVA: 0x000061F4 File Offset: 0x000043F4
		// (remove) Token: 0x06000196 RID: 406 RVA: 0x0000622C File Offset: 0x0000442C
		public event EventHandler<object> Closed;

		// Token: 0x06000197 RID: 407 RVA: 0x00006264 File Offset: 0x00004464
		public override void OnApplyTemplate()
		{
			if (this.m_toolBar != null)
			{
				this.m_toolBar.ClearValue(ItemsControl.ItemsSourceProperty);
				this.m_toolBar.OverflowOpened -= new EventHandler(this.OnOverflowOpened);
				this.m_toolBar.OverflowClosed -= new EventHandler(this.OnOverflowClosed);
				this.m_toolBar.HasPrimaryCommandsChanged -= new EventHandler(this.OnHasPrimaryCommandsChanged);
				this.m_toolBar.HasOverflowItemsChanged -= new EventHandler(this.OnHasOverflowItemsChanged);
			}
			base.OnApplyTemplate();
			this.m_toolBar = (base.GetTemplateChild("PART_ToolBar") as CommandBarToolBar);
			if (this.m_toolBar != null)
			{
				ItemsControl toolBar = this.m_toolBar;
				CompositeCollection compositeCollection = new CompositeCollection();
				compositeCollection.Add(new CollectionContainer
				{
					Collection = this.PrimaryCommands
				});
				compositeCollection.Add(new CollectionContainer
				{
					Collection = this.SecondaryCommands
				});
				toolBar.ItemsSource = compositeCollection;
				this.m_toolBar.OverflowOpened += new EventHandler(this.OnOverflowOpened);
				this.m_toolBar.OverflowClosed += new EventHandler(this.OnOverflowClosed);
				this.m_toolBar.HasPrimaryCommandsChanged += new EventHandler(this.OnHasPrimaryCommandsChanged);
				this.m_toolBar.HasOverflowItemsChanged += new EventHandler(this.OnHasOverflowItemsChanged);
			}
			this.UpdateVisualState(false);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000063AF File Offset: 0x000045AF
		private void OnOverflowOpened(object sender, EventArgs e)
		{
			EventHandler<object> opened = this.Opened;
			if (opened == null)
			{
				return;
			}
			opened.Invoke(this, null);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000063C3 File Offset: 0x000045C3
		private void OnOverflowClosed(object sender, EventArgs e)
		{
			EventHandler<object> closed = this.Closed;
			if (closed == null)
			{
				return;
			}
			closed.Invoke(this, null);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000063D7 File Offset: 0x000045D7
		private void OnHasPrimaryCommandsChanged(object sender, EventArgs e)
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000063E0 File Offset: 0x000045E0
		private void OnHasOverflowItemsChanged(object sender, EventArgs e)
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000063EC File Offset: 0x000045EC
		internal void UpdateVisualState(bool useTransitions = true)
		{
			if (this.m_toolBar != null)
			{
				string text;
				if (this.HasPrimaryCommands && this.HasSecondaryCommands)
				{
					text = "BothCommands";
				}
				else if (this.HasSecondaryCommands)
				{
					text = "SecondaryCommandsOnly";
				}
				else
				{
					text = "PrimaryCommandsOnly";
				}
				VisualStateManager.GoToState(this.m_toolBar, text, useTransitions);
			}
		}

		// Token: 0x0400006A RID: 106
		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(CommandBar));

		// Token: 0x0400006B RID: 107
		public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(CommandBar));

		// Token: 0x0400006C RID: 108
		public static readonly DependencyProperty CornerRadiusProperty = ControlHelper.CornerRadiusProperty.AddOwner(typeof(CommandBar));

		// Token: 0x0400006D RID: 109
		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(CommandBar), new FrameworkPropertyMetadata(false, 256));

		// Token: 0x04000070 RID: 112
		public static readonly DependencyProperty CommandBarOverflowPresenterStyleProperty = DependencyProperty.Register("CommandBarOverflowPresenterStyle", typeof(Style), typeof(CommandBar), null);

		// Token: 0x04000071 RID: 113
		public static readonly DependencyProperty DefaultLabelPositionProperty = CommandBarToolBar.DefaultLabelPositionProperty.AddOwner(typeof(CommandBar));

		// Token: 0x04000072 RID: 114
		public static readonly DependencyProperty IsDynamicOverflowEnabledProperty = CommandBarToolBar.IsDynamicOverflowEnabledProperty.AddOwner(typeof(CommandBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(CommandBar.OnIsDynamicOverflowEnabledChanged)));

		// Token: 0x04000073 RID: 115
		public static readonly DependencyProperty OverflowButtonVisibilityProperty = CommandBarToolBar.OverflowButtonVisibilityProperty.AddOwner(typeof(CommandBar));

		// Token: 0x04000076 RID: 118
		private CommandBarToolBar m_toolBar;

		// Token: 0x04000077 RID: 119
		internal const string ToolBarName = "PART_ToolBar";
	}
}
