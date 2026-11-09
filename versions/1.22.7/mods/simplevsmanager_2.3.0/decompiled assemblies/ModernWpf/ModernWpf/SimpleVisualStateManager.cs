using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace ModernWpf
{
	// Token: 0x0200022E RID: 558
	public class SimpleVisualStateManager : VisualStateManager
	{
		// Token: 0x0600106A RID: 4202 RVA: 0x0002EF78 File Offset: 0x0002D178
		protected override bool GoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			if (state == null)
			{
				return false;
			}
			useTransitions &= Helper.IsAnimationsEnabled;
			if (group.Transitions.Count > 0 && VisualStateGroupHelper.IsSupported)
			{
				return this.GoToStateInternal(control, stateGroupsRoot, group, state, useTransitions);
			}
			return base.GoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions);
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0002EFCC File Offset: 0x0002D1CC
		internal static Collection<VisualStateGroup> GetVisualStateGroupsInternal(FrameworkElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (DependencyPropertyHelper.GetValueSource(obj, VisualStateManager.VisualStateGroupsProperty).BaseValueSource != 1)
			{
				return obj.GetValue(VisualStateManager.VisualStateGroupsProperty) as Collection<VisualStateGroup>;
			}
			return null;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0002F010 File Offset: 0x0002D210
		internal static bool TryGetState(IList<VisualStateGroup> groups, string stateName, out VisualStateGroup group, out VisualState state)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				VisualStateGroup visualStateGroup = groups[i];
				VisualState state2 = visualStateGroup.GetState(stateName);
				if (state2 != null)
				{
					group = visualStateGroup;
					state = state2;
					return true;
				}
			}
			group = null;
			state = null;
			return false;
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0002F050 File Offset: 0x0002D250
		private bool GoToStateInternal(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			if (stateGroupsRoot == null)
			{
				throw new ArgumentNullException("stateGroupsRoot");
			}
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}
			if (group == null)
			{
				throw new InvalidOperationException();
			}
			VisualState lastState = group.CurrentState;
			if (lastState == state)
			{
				return true;
			}
			VisualTransition transition = useTransitions ? SimpleVisualStateManager.GetTransition(stateGroupsRoot, group, lastState, state) : null;
			if (transition == null || (transition.GeneratedDuration == SimpleVisualStateManager.DurationZero && (transition.Storyboard == null || transition.Storyboard.Duration == SimpleVisualStateManager.DurationZero)))
			{
				if (transition != null && transition.Storyboard != null)
				{
					group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
					{
						transition.Storyboard,
						state.Storyboard
					});
				}
				else
				{
					group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
					{
						state.Storyboard
					});
				}
				base.RaiseCurrentStateChanging(group, lastState, state, control, stateGroupsRoot);
				base.RaiseCurrentStateChanged(group, lastState, state, control, stateGroupsRoot);
			}
			else
			{
				if (transition.Storyboard != null)
				{
					EventHandler transitionCompleted = null;
					transitionCompleted = delegate(object sender, EventArgs e)
					{
						if (SimpleVisualStateManager.ShouldRunStateStoryboard(control, stateGroupsRoot, state, group))
						{
							group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
							{
								state.Storyboard
							});
						}
						this.RaiseCurrentStateChanged(group, lastState, state, control, stateGroupsRoot);
						transition.Storyboard.Completed -= transitionCompleted;
					};
					transition.Storyboard.Completed += transitionCompleted;
				}
				group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
				{
					transition.Storyboard
				});
				base.RaiseCurrentStateChanging(group, lastState, state, control, stateGroupsRoot);
			}
			group.SetCurrentState(state);
			return true;
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0002F2A8 File Offset: 0x0002D4A8
		private static bool ShouldRunStateStoryboard(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualState state, VisualStateGroup group)
		{
			bool flag = true;
			bool flag2 = true;
			if (control != null && !control.IsVisible)
			{
				flag = (PresentationSource.FromVisual(control) != null);
			}
			if (stateGroupsRoot != null && !stateGroupsRoot.IsVisible)
			{
				flag2 = (PresentationSource.FromVisual(stateGroupsRoot) != null);
			}
			return flag && flag2 && state == group.CurrentState;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0002F2F4 File Offset: 0x0002D4F4
		internal static VisualTransition GetTransition(FrameworkElement element, VisualStateGroup group, VisualState from, VisualState to)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			VisualTransition visualTransition = null;
			VisualTransition visualTransition2 = null;
			int num = -1;
			IList<VisualTransition> list = (IList<VisualTransition>)group.Transitions;
			if (list != null)
			{
				foreach (VisualTransition visualTransition3 in list)
				{
					if (visualTransition2 == null && SimpleVisualStateManager.IsDefault(visualTransition3))
					{
						visualTransition2 = visualTransition3;
					}
					else
					{
						int num2 = -1;
						VisualState state = group.GetState(visualTransition3.From);
						VisualState state2 = group.GetState(visualTransition3.To);
						if (from == state)
						{
							num2++;
						}
						else if (state != null)
						{
							continue;
						}
						if (to == state2)
						{
							num2 += 2;
						}
						else if (state2 != null)
						{
							continue;
						}
						if (num2 > num)
						{
							num = num2;
							visualTransition = visualTransition3;
						}
					}
				}
			}
			return visualTransition ?? visualTransition2;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0002F3E4 File Offset: 0x0002D5E4
		internal static bool IsDefault(VisualTransition transition)
		{
			return transition.From == null && transition.To == null;
		}

		// Token: 0x0400044C RID: 1100
		private static readonly Duration DurationZero = new Duration(TimeSpan.Zero);
	}
}
