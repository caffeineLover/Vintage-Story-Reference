using System;
using System.Windows;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x020002AD RID: 685
	public class VisualStateGroupListener : FrameworkElement
	{
		// Token: 0x060014B9 RID: 5305 RVA: 0x0003DD20 File Offset: 0x0003BF20
		static VisualStateGroupListener()
		{
			UIElement.VisibilityProperty.OverrideMetadata(typeof(VisualStateGroupListener), new FrameworkPropertyMetadata(2));
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x0003DDEF File Offset: 0x0003BFEF
		// (set) Token: 0x060014BC RID: 5308 RVA: 0x0003DE01 File Offset: 0x0003C001
		public VisualStateGroup Group
		{
			get
			{
				return (VisualStateGroup)base.GetValue(VisualStateGroupListener.GroupProperty);
			}
			set
			{
				base.SetValue(VisualStateGroupListener.GroupProperty, value);
			}
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0003DE0F File Offset: 0x0003C00F
		private static void OnGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((VisualStateGroupListener)d).OnGroupChanged((VisualStateGroup)e.OldValue, (VisualStateGroup)e.NewValue);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0003DE34 File Offset: 0x0003C034
		private void OnGroupChanged(VisualStateGroup oldGroup, VisualStateGroup newGroup)
		{
			if (oldGroup != null)
			{
				oldGroup.CurrentStateChanged -= new EventHandler<VisualStateChangedEventArgs>(this.OnCurrentStateChanged);
			}
			if (newGroup != null)
			{
				newGroup.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(this.OnCurrentStateChanged);
			}
			this.UpdateCurrentStateName((newGroup != null) ? newGroup.CurrentState : null);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0003DE72 File Offset: 0x0003C072
		private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
		{
			this.UpdateCurrentStateName(e.NewState);
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0003DE80 File Offset: 0x0003C080
		// (set) Token: 0x060014C1 RID: 5313 RVA: 0x0003DE92 File Offset: 0x0003C092
		public string CurrentStateName
		{
			get
			{
				return (string)base.GetValue(VisualStateGroupListener.CurrentStateNameProperty);
			}
			private set
			{
				base.SetValue(VisualStateGroupListener.CurrentStateNamePropertyKey, value);
			}
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0003DEA0 File Offset: 0x0003C0A0
		private void UpdateCurrentStateName(VisualState currentState)
		{
			if (currentState != null)
			{
				this.CurrentStateName = currentState.Name;
				return;
			}
			base.ClearValue(VisualStateGroupListener.CurrentStateNamePropertyKey);
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0003DEBD File Offset: 0x0003C0BD
		public static VisualStateGroupListener GetListener(VisualStateGroup group)
		{
			return (VisualStateGroupListener)group.GetValue(VisualStateGroupListener.ListenerProperty);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0003DECF File Offset: 0x0003C0CF
		public static void SetListener(VisualStateGroup group, VisualStateGroupListener value)
		{
			group.SetValue(VisualStateGroupListener.ListenerProperty, value);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0003DEE0 File Offset: 0x0003C0E0
		private static void OnListenerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			VisualStateGroupListener visualStateGroupListener = e.OldValue as VisualStateGroupListener;
			if (visualStateGroupListener != null)
			{
				visualStateGroupListener.ClearValue(VisualStateGroupListener.GroupProperty);
			}
			VisualStateGroupListener visualStateGroupListener2 = e.NewValue as VisualStateGroupListener;
			if (visualStateGroupListener2 != null)
			{
				visualStateGroupListener2.Group = (VisualStateGroup)d;
			}
		}

		// Token: 0x040006F5 RID: 1781
		public static readonly DependencyProperty GroupProperty = DependencyProperty.Register("Group", typeof(VisualStateGroup), typeof(VisualStateGroupListener), new PropertyMetadata(new PropertyChangedCallback(VisualStateGroupListener.OnGroupChanged)));

		// Token: 0x040006F6 RID: 1782
		private static readonly DependencyPropertyKey CurrentStateNamePropertyKey = DependencyProperty.RegisterReadOnly("CurrentStateName", typeof(string), typeof(VisualStateGroupListener), null);

		// Token: 0x040006F7 RID: 1783
		public static readonly DependencyProperty CurrentStateNameProperty = VisualStateGroupListener.CurrentStateNamePropertyKey.DependencyProperty;

		// Token: 0x040006F8 RID: 1784
		public static readonly DependencyProperty ListenerProperty = DependencyProperty.RegisterAttached("Listener", typeof(VisualStateGroupListener), typeof(VisualStateGroupListener), new PropertyMetadata(new PropertyChangedCallback(VisualStateGroupListener.OnListenerChanged)));
	}
}
