using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf
{
	// Token: 0x0200022F RID: 559
	internal static class VisualStateGroupHelper
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0002F412 File Offset: 0x0002D612
		internal static bool IsSupported
		{
			get
			{
				return VisualStateGroupHelper._setCurrentState.Value != null;
			}
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0002F421 File Offset: 0x0002D621
		internal static void SetCurrentState(this VisualStateGroup group, VisualState value)
		{
			if (!VisualStateGroupHelper.IsSupported)
			{
				throw new InvalidOperationException();
			}
			VisualStateGroupHelper._setCurrentState.Value.Invoke(group, value);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0002F444 File Offset: 0x0002D644
		internal static VisualState GetState(this VisualStateGroup group, string stateName)
		{
			for (int i = 0; i < group.States.Count; i++)
			{
				VisualState visualState = (VisualState)group.States[i];
				if (visualState.Name == stateName)
				{
					return visualState;
				}
			}
			return null;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0002F48C File Offset: 0x0002D68C
		internal static Collection<Storyboard> GetCurrentStoryboards(VisualStateGroup group)
		{
			Collection<Storyboard> collection = (Collection<Storyboard>)group.GetValue(VisualStateGroupHelper.CurrentStoryboardsProperty);
			if (collection == null)
			{
				collection = new Collection<Storyboard>();
				group.SetValue(VisualStateGroupHelper.CurrentStoryboardsProperty, collection);
			}
			return collection;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0002F4C0 File Offset: 0x0002D6C0
		internal static void StartNewThenStopOld(this VisualStateGroup group, FrameworkElement element, params Storyboard[] newStoryboards)
		{
			Collection<Storyboard> currentStoryboards = VisualStateGroupHelper.GetCurrentStoryboards(group);
			for (int i = 0; i < currentStoryboards.Count; i++)
			{
				if (currentStoryboards[i] != null)
				{
					currentStoryboards[i].Remove(element);
				}
			}
			currentStoryboards.Clear();
			for (int j = 0; j < newStoryboards.Length; j++)
			{
				if (newStoryboards[j] != null)
				{
					newStoryboards[j].Begin(element, 0, true);
					currentStoryboards.Add(newStoryboards[j]);
				}
			}
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0002F528 File Offset: 0x0002D728
		private static Action<VisualStateGroup, VisualState> CreateSetCurrentStateDelegate()
		{
			Action<VisualStateGroup, VisualState> result;
			try
			{
				result = DelegateHelper.CreatePropertySetter<VisualStateGroup, VisualState>("CurrentState", 28, true);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400044D RID: 1101
		private static readonly DependencyProperty CurrentStoryboardsProperty = DependencyProperty.RegisterAttached("CurrentStoryboards", typeof(Collection<Storyboard>), typeof(VisualStateGroupHelper));

		// Token: 0x0400044E RID: 1102
		private static readonly Lazy<Action<VisualStateGroup, VisualState>> _setCurrentState = new Lazy<Action<VisualStateGroup, VisualState>>(new Func<Action<VisualStateGroup, VisualState>>(VisualStateGroupHelper.CreateSetCurrentStateDelegate));
	}
}
