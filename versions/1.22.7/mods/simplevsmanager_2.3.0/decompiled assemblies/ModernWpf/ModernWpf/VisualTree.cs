using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf
{
	// Token: 0x02000223 RID: 547
	public static class VisualTree
	{
		// Token: 0x06000FD1 RID: 4049 RVA: 0x0002D228 File Offset: 0x0002B428
		public static FrameworkElement FindDescendantByName(this DependencyObject element, string name)
		{
			if (element == null || string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			FrameworkElement frameworkElement = element as FrameworkElement;
			if (name.Equals((frameworkElement != null) ? frameworkElement.Name : null, 5))
			{
				return element as FrameworkElement;
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childrenCount; i++)
			{
				FrameworkElement frameworkElement2 = VisualTreeHelper.GetChild(element, i).FindDescendantByName(name);
				if (frameworkElement2 != null)
				{
					return frameworkElement2;
				}
			}
			return null;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0002D28C File Offset: 0x0002B48C
		public static T FindDescendant<T>(this DependencyObject element) where T : DependencyObject
		{
			T t = default(T);
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				T t2 = child as T;
				if (t2 != null)
				{
					t = t2;
					break;
				}
				t = child.FindDescendant<T>();
				if (t != null)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0002D2E8 File Offset: 0x0002B4E8
		public static object FindDescendant(this DependencyObject element, Type type)
		{
			object obj = null;
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				if (child.GetType() == type)
				{
					obj = child;
					break;
				}
				obj = child.FindDescendant(type);
				if (obj != null)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0002D330 File Offset: 0x0002B530
		public static IEnumerable<T> FindDescendants<T>(this DependencyObject element) where T : DependencyObject
		{
			VisualTree.<FindDescendants>d__3<T> <FindDescendants>d__ = new VisualTree.<FindDescendants>d__3<T>(-2);
			<FindDescendants>d__.<>3__element = element;
			return <FindDescendants>d__;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0002D340 File Offset: 0x0002B540
		public static FrameworkElement FindAscendantByName(this DependencyObject element, string name)
		{
			if (element == null || string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			DependencyObject parent = VisualTreeHelper.GetParent(element);
			if (parent == null)
			{
				return null;
			}
			FrameworkElement frameworkElement = parent as FrameworkElement;
			if (name.Equals((frameworkElement != null) ? frameworkElement.Name : null, 5))
			{
				return parent as FrameworkElement;
			}
			return parent.FindAscendantByName(name);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0002D390 File Offset: 0x0002B590
		public static T FindAscendant<T>(this DependencyObject element) where T : DependencyObject
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);
			if (parent == null)
			{
				return default(T);
			}
			if (parent is T)
			{
				return parent as T;
			}
			return parent.FindAscendant<T>();
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0002D3CC File Offset: 0x0002B5CC
		public static object FindAscendant(this DependencyObject element, Type type)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);
			if (parent == null)
			{
				return null;
			}
			if (parent.GetType() == type)
			{
				return parent;
			}
			return parent.FindAscendant(type);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0002D3FC File Offset: 0x0002B5FC
		public static IEnumerable<DependencyObject> FindAscendants(this DependencyObject element)
		{
			for (DependencyObject parent = VisualTreeHelper.GetParent(element); parent != null; parent = VisualTreeHelper.GetParent(parent))
			{
				yield return parent;
			}
			yield break;
		}
	}
}
