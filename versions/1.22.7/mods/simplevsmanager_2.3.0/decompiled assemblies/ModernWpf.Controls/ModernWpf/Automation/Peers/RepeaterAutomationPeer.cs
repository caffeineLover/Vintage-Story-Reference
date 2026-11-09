using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x02000010 RID: 16
	public class RepeaterAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public RepeaterAutomationPeer(FrameworkElement owner) : base(owner)
		{
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002CEC File Offset: 0x00000EEC
		protected override List<AutomationPeer> GetChildrenCore()
		{
			ItemsRepeater repeater = (ItemsRepeater)base.Owner;
			List<AutomationPeer> childrenCore = base.GetChildrenCore();
			if (childrenCore == null)
			{
				return null;
			}
			int count = childrenCore.Count;
			List<Tuple<int, AutomationPeer>> list = new List<Tuple<int, AutomationPeer>>();
			list.Capacity = count;
			for (int i = 0; i < count; i++)
			{
				AutomationPeer automationPeer = childrenCore[i];
				UIElement element = this.GetElement(automationPeer, repeater);
				if (element != null)
				{
					VirtualizationInfo virtualizationInfo = ItemsRepeater.GetVirtualizationInfo(element);
					if (virtualizationInfo.IsRealized)
					{
						list.Add(Tuple.Create<int, AutomationPeer>(virtualizationInfo.Index, automationPeer));
					}
				}
			}
			List<AutomationPeer> list2 = new List<AutomationPeer>(list.Count);
			foreach (Tuple<int, AutomationPeer> tuple in Enumerable.OrderBy<Tuple<int, AutomationPeer>, int>(list, (Tuple<int, AutomationPeer> x) => x.Item1))
			{
				list2.Add(tuple.Item2);
			}
			return list2;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002DF0 File Offset: 0x00000FF0
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 26;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002DF4 File Offset: 0x00000FF4
		private UIElement GetElement(AutomationPeer childPeer, ItemsRepeater repeater)
		{
			DependencyObject dependencyObject = ((FrameworkElementAutomationPeer)childPeer).Owner;
			DependencyObject parent = CachedVisualTreeHelpers.GetParent(dependencyObject);
			while (parent != null && parent as ItemsRepeater != repeater)
			{
				dependencyObject = parent;
				parent = CachedVisualTreeHelpers.GetParent(dependencyObject);
			}
			return (UIElement)dependencyObject;
		}
	}
}
