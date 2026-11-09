using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ModernWpf.Controls
{
	// Token: 0x0200001E RID: 30
	internal static class AppBarElementProperties
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00004FD3 File Offset: 0x000031D3
		private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IAppBarElement appBarElement = d as IAppBarElement;
			if (appBarElement == null)
			{
				return;
			}
			appBarElement.UpdateApplicationViewState();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00004FE5 File Offset: 0x000031E5
		private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			frameworkElement.CoerceValue(FrameworkElement.ToolTipProperty);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004FFC File Offset: 0x000031FC
		private static object CoerceLabel(DependencyObject d, object value)
		{
			ButtonBase buttonBase = (ButtonBase)d;
			if (string.IsNullOrEmpty(value as string) && !Helper.HasNonDefaultValue(buttonBase, AppBarElementProperties.LabelProperty))
			{
				RoutedUICommand routedUICommand = buttonBase.Command as RoutedUICommand;
				if (routedUICommand != null)
				{
					value = routedUICommand.Text;
				}
				return value;
			}
			return value;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005044 File Offset: 0x00003244
		private static void OnLabelPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IAppBarElement appBarElement = d as IAppBarElement;
			if (appBarElement == null)
			{
				return;
			}
			appBarElement.UpdateApplicationViewState();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005056 File Offset: 0x00003256
		private static void OnIsCompactChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IAppBarElement appBarElement = d as IAppBarElement;
			if (appBarElement == null)
			{
				return;
			}
			appBarElement.UpdateApplicationViewState();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005068 File Offset: 0x00003268
		private static void OnIsInOverflowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IAppBarElement appBarElement = d as IAppBarElement;
			if (appBarElement != null)
			{
				appBarElement.UpdateApplicationViewState();
			}
			AppBarElementProperties.UpdateShowKeyboardAcceleratorText(d as FrameworkElement);
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			frameworkElement.CoerceValue(FrameworkElement.ToolTipProperty);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000509C File Offset: 0x0000329C
		internal static void UpdateIsInOverflow(DependencyObject element)
		{
			bool flag = ToolBar.GetIsOverflowItem(element) || ToolBar.GetOverflowMode(element) == 1;
			element.SetValue(AppBarElementProperties.IsInOverflowPropertyKey, flag);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000050CF File Offset: 0x000032CF
		private static void OnApplicationViewStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IAppBarElement appBarElement = d as IAppBarElement;
			if (appBarElement == null)
			{
				return;
			}
			appBarElement.ApplyApplicationViewState();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000050E1 File Offset: 0x000032E1
		private static void OnInputGestureTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarElementProperties.UpdateHasInputGestureText(d, (string)e.NewValue);
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			frameworkElement.CoerceValue(FrameworkElement.ToolTipProperty);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000510C File Offset: 0x0000330C
		private static object CoerceInputGestureText(DependencyObject d, object value)
		{
			ButtonBase buttonBase = (ButtonBase)d;
			RoutedCommand routedCommand;
			if (string.IsNullOrEmpty((string)value) && !Helper.HasNonDefaultValue(buttonBase, AppBarElementProperties.InputGestureTextProperty) && (routedCommand = (buttonBase.Command as RoutedCommand)) != null)
			{
				InputGestureCollection inputGestures = routedCommand.InputGestures;
				if (inputGestures != null && inputGestures.Count >= 1)
				{
					for (int i = 0; i < inputGestures.Count; i++)
					{
						KeyGesture keyGesture = inputGestures[i] as KeyGesture;
						if (keyGesture != null)
						{
							return keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
						}
					}
				}
			}
			return value;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000518D File Offset: 0x0000338D
		private static void OnHasInputGestureTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AppBarElementProperties.UpdateShowKeyboardAcceleratorText(d as FrameworkElement);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000519A File Offset: 0x0000339A
		private static void UpdateHasInputGestureText(DependencyObject element, string inputGestureText)
		{
			element.SetValue(AppBarElementProperties.HasInputGestureTextPropertyKey, !string.IsNullOrEmpty(inputGestureText));
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000051B5 File Offset: 0x000033B5
		internal static bool GetShowKeyboardAcceleratorText(DependencyObject element)
		{
			return (bool)element.GetValue(AppBarElementProperties.ShowKeyboardAcceleratorTextProperty);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000051C7 File Offset: 0x000033C7
		private static void SetShowKeyboardAcceleratorText(DependencyObject element, bool value)
		{
			element.SetValue(AppBarElementProperties.ShowKeyboardAcceleratorTextProperty, value);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000051DC File Offset: 0x000033DC
		private static void UpdateShowKeyboardAcceleratorText(FrameworkElement element)
		{
			if (element != null)
			{
				bool value = (bool)element.GetValue(AppBarElementProperties.HasInputGestureTextProperty) && (bool)element.GetValue(AppBarElementProperties.IsInOverflowProperty);
				AppBarElementProperties.SetShowKeyboardAcceleratorText(element, value);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000521C File Offset: 0x0000341C
		internal static object CoerceToolTip(DependencyObject d, object baseValue)
		{
			ButtonBase buttonBase = (ButtonBase)d;
			if (baseValue == null && Helper.HasDefaultValue(buttonBase, FrameworkElement.ToolTipProperty) && (bool)buttonBase.GetValue(AppBarElementProperties.HasInputGestureTextProperty) && !(bool)buttonBase.GetValue(AppBarElementProperties.IsInOverflowProperty))
			{
				string text = (string)buttonBase.GetValue(AppBarElementProperties.LabelProperty);
				string text2 = (string)buttonBase.GetValue(AppBarElementProperties.InputGestureTextProperty);
				return (text + " (" + text2 + ")").Trim();
			}
			return baseValue;
		}

		// Token: 0x0400004F RID: 79
		public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon", typeof(IconElement), typeof(AppBarElementProperties), new PropertyMetadata(new PropertyChangedCallback(AppBarElementProperties.OnIconChanged)));

		// Token: 0x04000050 RID: 80
		public static readonly DependencyProperty LabelProperty = DependencyProperty.RegisterAttached("Label", typeof(string), typeof(AppBarElementProperties), new PropertyMetadata(string.Empty, new PropertyChangedCallback(AppBarElementProperties.OnLabelChanged), new CoerceValueCallback(AppBarElementProperties.CoerceLabel)));

		// Token: 0x04000051 RID: 81
		public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.RegisterAttached("LabelPosition", typeof(CommandBarLabelPosition), typeof(AppBarElementProperties), new PropertyMetadata(CommandBarLabelPosition.Default, new PropertyChangedCallback(AppBarElementProperties.OnLabelPositionChanged)));

		// Token: 0x04000052 RID: 82
		public static readonly DependencyProperty IsCompactProperty = DependencyProperty.RegisterAttached("IsCompact", typeof(bool), typeof(AppBarElementProperties), new PropertyMetadata(false, new PropertyChangedCallback(AppBarElementProperties.OnIsCompactChanged)));

		// Token: 0x04000053 RID: 83
		internal static readonly DependencyPropertyKey IsInOverflowPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsInOverflow", typeof(bool), typeof(AppBarElementProperties), new PropertyMetadata(false, new PropertyChangedCallback(AppBarElementProperties.OnIsInOverflowChanged)));

		// Token: 0x04000054 RID: 84
		public static readonly DependencyProperty IsInOverflowProperty = AppBarElementProperties.IsInOverflowPropertyKey.DependencyProperty;

		// Token: 0x04000055 RID: 85
		internal static readonly DependencyPropertyKey ApplicationViewStatePropertyKey = DependencyProperty.RegisterAttachedReadOnly("ApplicationViewState", typeof(AppBarElementApplicationViewState), typeof(AppBarElementProperties), new PropertyMetadata(AppBarElementApplicationViewState.FullSize, new PropertyChangedCallback(AppBarElementProperties.OnApplicationViewStateChanged)));

		// Token: 0x04000056 RID: 86
		internal static readonly DependencyProperty ApplicationViewStateProperty = AppBarElementProperties.ApplicationViewStatePropertyKey.DependencyProperty;

		// Token: 0x04000057 RID: 87
		public static readonly DependencyProperty InputGestureTextProperty = DependencyProperty.RegisterAttached("InputGestureText", typeof(string), typeof(AppBarElementProperties), new PropertyMetadata(string.Empty, new PropertyChangedCallback(AppBarElementProperties.OnInputGestureTextChanged), new CoerceValueCallback(AppBarElementProperties.CoerceInputGestureText)));

		// Token: 0x04000058 RID: 88
		private static readonly DependencyPropertyKey HasInputGestureTextPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HasInputGestureText", typeof(bool), typeof(AppBarElementProperties), new PropertyMetadata(false, new PropertyChangedCallback(AppBarElementProperties.OnHasInputGestureTextChanged)));

		// Token: 0x04000059 RID: 89
		public static readonly DependencyProperty HasInputGestureTextProperty = AppBarElementProperties.HasInputGestureTextPropertyKey.DependencyProperty;

		// Token: 0x0400005A RID: 90
		internal static readonly DependencyProperty ShowKeyboardAcceleratorTextProperty = DependencyProperty.RegisterAttached("ShowKeyboardAcceleratorText", typeof(bool), typeof(AppBarElementProperties));
	}
}
