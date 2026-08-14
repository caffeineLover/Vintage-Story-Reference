using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Win32;

namespace ModernWpf.Controls
{
	// Token: 0x02000031 RID: 49
	internal static class SharedHelpers
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00006838 File Offset: 0x00004A38
		public static bool IsAnimationsEnabled
		{
			get
			{
				return SystemParameters.ClientAreaAnimation && RenderCapability.Tier > 0;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000684B File Offset: 0x00004A4B
		public static bool IsRS1OrHigher()
		{
			return true;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000684E File Offset: 0x00004A4E
		public static bool IsRS2OrHigher()
		{
			return true;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00006851 File Offset: 0x00004A51
		public static bool IsRS3OrHigher()
		{
			return true;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00006854 File Offset: 0x00004A54
		public static bool IsRS4OrHigher()
		{
			return true;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006857 File Offset: 0x00004A57
		public static bool IsRS5OrHigher()
		{
			return true;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000685A File Offset: 0x00004A5A
		public static bool IsControlCornerRadiusAvailable()
		{
			return true;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000685D File Offset: 0x00004A5D
		public static bool IsThemeShadowAvailable()
		{
			return false;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006860 File Offset: 0x00004A60
		public static bool IsOnXbox()
		{
			return false;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00006863 File Offset: 0x00004A63
		public static void QueueCallbackForCompositionRendering(Action callback)
		{
			SharedHelpers.<>c__DisplayClass10_0 CS$<>8__locals1 = new SharedHelpers.<>c__DisplayClass10_0();
			CS$<>8__locals1.callback = callback;
			CompositionTarget.Rendering += new EventHandler(CS$<>8__locals1.<QueueCallbackForCompositionRendering>g__onRendering|0);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006884 File Offset: 0x00004A84
		public static bool DoRectsIntersect(Rect rect1, Rect rect2)
		{
			return rect1.Width > 0.0 && rect1.Height > 0.0 && rect2.Width > 0.0 && rect2.Height > 0.0 && rect2.X <= rect1.X + rect1.Width && rect2.X + rect2.Width >= rect1.X && rect2.Y <= rect1.Y + rect1.Height && rect2.Y + rect2.Height >= rect1.Y;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006944 File Offset: 0x00004B44
		public static object FindResource(string resource, ResourceDictionary resources, object defaultValue)
		{
			if (!resources.Contains(resource))
			{
				return defaultValue;
			}
			return resources[resource];
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00006965 File Offset: 0x00004B65
		public static object FindResource(string resource, FrameworkElement element, object defaultValue)
		{
			return element.TryFindResource(resource) ?? defaultValue;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00006973 File Offset: 0x00004B73
		public static object FindInApplicationResources(string resource, object defaultValue)
		{
			return SharedHelpers.FindResource(resource, Application.Current.Resources, defaultValue);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00006988 File Offset: 0x00004B88
		public static void SetBinding(string pathString, DependencyObject target, DependencyProperty targetProperty)
		{
			Binding binding = new Binding(pathString)
			{
				RelativeSource = RelativeSource.TemplatedParent
			};
			BindingOperations.SetBinding(target, targetProperty, binding);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000069B0 File Offset: 0x00004BB0
		public static bool IsFrameworkElementLoaded(FrameworkElement frameworkElement)
		{
			return frameworkElement.IsLoaded;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000069B8 File Offset: 0x00004BB8
		public static AncestorType GetAncestorOfType<AncestorType>(DependencyObject firstGuess) where AncestorType : DependencyObject
		{
			DependencyObject dependencyObject = firstGuess;
			AncestorType ancestorType = default(AncestorType);
			while (dependencyObject != null && ancestorType == null)
			{
				ancestorType = (dependencyObject as AncestorType);
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
			if (ancestorType != null)
			{
				return ancestorType;
			}
			return default(AncestorType);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00006A04 File Offset: 0x00004C04
		internal static void ForwardCollectionChange<T>(ObservableCollection<T> source, ObservableCollection<T> destination, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
			case 0:
				destination.Insert(args.NewStartingIndex, (T)((object)args.NewItems[0]));
				return;
			case 1:
				destination.RemoveAt(args.OldStartingIndex);
				return;
			case 2:
				destination[args.NewStartingIndex] = (T)((object)args.NewItems[0]);
				return;
			case 3:
				destination.Move(args.OldStartingIndex, args.NewStartingIndex);
				return;
			case 4:
				SharedHelpers.CopyList<T>(source, destination);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00006A98 File Offset: 0x00004C98
		public static void RaiseAutomationPropertyChangedEvent(UIElement element, object oldValue, object newValue)
		{
			AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(element);
			if (automationPeer != null)
			{
				automationPeer.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue, newValue);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00006ABC File Offset: 0x00004CBC
		public static IconElement MakeIconElementFrom(IconSource iconSource)
		{
			FontIconSource fontIconSource = iconSource as FontIconSource;
			if (fontIconSource != null)
			{
				FontIcon fontIcon = new FontIcon();
				fontIcon.Glyph = fontIconSource.Glyph;
				fontIcon.FontSize = fontIconSource.FontSize;
				Brush foreground = fontIconSource.Foreground;
				if (foreground != null)
				{
					fontIcon.Foreground = foreground;
				}
				if (fontIconSource.FontFamily != null)
				{
					fontIcon.FontFamily = fontIconSource.FontFamily;
				}
				fontIcon.FontWeight = fontIconSource.FontWeight;
				fontIcon.FontStyle = fontIconSource.FontStyle;
				return fontIcon;
			}
			SymbolIconSource symbolIconSource = iconSource as SymbolIconSource;
			if (symbolIconSource != null)
			{
				SymbolIcon symbolIcon = new SymbolIcon();
				symbolIcon.Symbol = symbolIconSource.Symbol;
				Brush foreground2 = symbolIconSource.Foreground;
				if (foreground2 != null)
				{
					symbolIcon.Foreground = foreground2;
				}
				return symbolIcon;
			}
			BitmapIconSource bitmapIconSource = iconSource as BitmapIconSource;
			if (bitmapIconSource != null)
			{
				BitmapIcon bitmapIcon = new BitmapIcon();
				if (bitmapIconSource.UriSource != null)
				{
					bitmapIcon.UriSource = bitmapIconSource.UriSource;
				}
				bitmapIcon.ShowAsMonochrome = bitmapIconSource.ShowAsMonochrome;
				Brush foreground3 = bitmapIconSource.Foreground;
				if (foreground3 != null)
				{
					bitmapIcon.Foreground = foreground3;
				}
				return bitmapIcon;
			}
			PathIconSource pathIconSource = iconSource as PathIconSource;
			if (pathIconSource != null)
			{
				PathIcon pathIcon = new PathIcon();
				if (pathIconSource.Data != null)
				{
					pathIcon.Data = pathIconSource.Data;
				}
				Brush foreground4 = pathIconSource.Foreground;
				if (foreground4 != null)
				{
					pathIcon.Foreground = foreground4;
				}
				return pathIcon;
			}
			return null;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00006C01 File Offset: 0x00004E01
		public static BindingExpressionBase SetBinding(this FrameworkElement element, DependencyProperty dp, DependencyProperty sourceDP, DependencyObject source)
		{
			return element.SetBinding(dp, new Binding
			{
				Path = new PropertyPath(sourceDP),
				Source = source
			});
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00006C24 File Offset: 0x00004E24
		public static void CopyList<T>(IList<T> source, IList<T> destination)
		{
			destination.Clear();
			foreach (T t in source)
			{
				destination.Add(t);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00006C74 File Offset: 0x00004E74
		public static Window GetActiveWindow()
		{
			IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
			if (activeWindow != IntPtr.Zero)
			{
				HwndSource hwndSource = HwndSource.FromHwnd(activeWindow);
				return ((hwndSource != null) ? hwndSource.RootVisual : null) as Window;
			}
			return null;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00006CAD File Offset: 0x00004EAD
		public static string SafeSubstring(this string s, int startIndex)
		{
			return s.SafeSubstring(startIndex, s.Length - startIndex);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006CBE File Offset: 0x00004EBE
		public static string SafeSubstring(this string s, int startIndex, int length)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (startIndex > s.Length)
			{
				return string.Empty;
			}
			if (length > s.Length - startIndex)
			{
				length = s.Length - startIndex;
			}
			return s.Substring(startIndex, length);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006CFC File Offset: 0x00004EFC
		public static bool IndexOf(this UIElementCollection collection, UIElement element, out int index)
		{
			int num = collection.IndexOf(element);
			if (num >= 0)
			{
				index = num;
				return true;
			}
			index = 0;
			return false;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006D1E File Offset: 0x00004F1E
		public static string TryGetStringRepresentationFromObject(object obj)
		{
			return ((obj != null) ? obj.ToString() : null) ?? string.Empty;
		}
	}
}
