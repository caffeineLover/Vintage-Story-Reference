using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000092 RID: 146
	public class ElementAnimator
	{
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000726 RID: 1830 RVA: 0x00019D58 File Offset: 0x00017F58
		// (remove) Token: 0x06000727 RID: 1831 RVA: 0x00019D90 File Offset: 0x00017F90
		public event ElementAnimationCompleted ShowAnimationCompleted;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06000728 RID: 1832 RVA: 0x00019DC8 File Offset: 0x00017FC8
		// (remove) Token: 0x06000729 RID: 1833 RVA: 0x00019E00 File Offset: 0x00018000
		public event ElementAnimationCompleted HideAnimationCompleted;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600072A RID: 1834 RVA: 0x00019E38 File Offset: 0x00018038
		// (remove) Token: 0x0600072B RID: 1835 RVA: 0x00019E70 File Offset: 0x00018070
		public event ElementAnimationCompleted BoundsChangeAnimationCompleted;

		// Token: 0x0600072C RID: 1836 RVA: 0x00019EA5 File Offset: 0x000180A5
		public void OnElementShown(UIElement element, AnimationContext context)
		{
			if (this.HasShowAnimation(element, context))
			{
				this.HasShowAnimationsPending = true;
				this.SharedContext |= context;
				this.QueueElementForAnimation(new ElementAnimator.ElementInfo(element, ElementAnimator.AnimationTrigger.Show, context));
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00019ED4 File Offset: 0x000180D4
		public void OnElementHidden(UIElement element, AnimationContext context)
		{
			if (this.HasHideAnimation(element, context))
			{
				this.HasHideAnimationsPending = true;
				this.SharedContext |= context;
				this.QueueElementForAnimation(new ElementAnimator.ElementInfo(element, ElementAnimator.AnimationTrigger.Hide, context));
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00019F03 File Offset: 0x00018103
		public void OnElementBoundsChanged(UIElement element, AnimationContext context, Rect oldBounds, Rect newBounds)
		{
			if (this.HasBoundsChangeAnimation(element, context, oldBounds, newBounds))
			{
				this.HasBoundsChangeAnimationsPending = true;
				this.SharedContext |= context;
				this.QueueElementForAnimation(new ElementAnimator.ElementInfo(element, ElementAnimator.AnimationTrigger.BoundsChange, context, oldBounds, newBounds));
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00019F38 File Offset: 0x00018138
		public bool HasShowAnimation(UIElement element, AnimationContext context)
		{
			return this.HasShowAnimationCore(element, context);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00019F42 File Offset: 0x00018142
		public bool HasHideAnimation(UIElement element, AnimationContext context)
		{
			return this.HasHideAnimationCore(element, context);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00019F4C File Offset: 0x0001814C
		public bool HasBoundsChangeAnimation(UIElement element, AnimationContext context, Rect oldBounds, Rect newBounds)
		{
			return this.HasBoundsChangeAnimationCore(element, context, oldBounds, newBounds);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00019F59 File Offset: 0x00018159
		protected virtual bool HasShowAnimationCore(UIElement element, AnimationContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00019F60 File Offset: 0x00018160
		protected virtual bool HasHideAnimationCore(UIElement element, AnimationContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00019F67 File Offset: 0x00018167
		protected virtual bool HasBoundsChangeAnimationCore(UIElement element, AnimationContext context, Rect oldBounds, Rect newBounds)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00019F6E File Offset: 0x0001816E
		protected virtual void StartShowAnimation(UIElement element, AnimationContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00019F75 File Offset: 0x00018175
		protected virtual void StartHideAnimation(UIElement element, AnimationContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00019F7C File Offset: 0x0001817C
		protected virtual void StartBoundsChangeAnimation(UIElement element, AnimationContext context, Rect oldBounds, Rect newBounds)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x00019F83 File Offset: 0x00018183
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x00019F8B File Offset: 0x0001818B
		private protected bool HasShowAnimationsPending { protected get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00019F94 File Offset: 0x00018194
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x00019F9C File Offset: 0x0001819C
		private protected bool HasHideAnimationsPending { protected get; private set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x00019FA5 File Offset: 0x000181A5
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x00019FAD File Offset: 0x000181AD
		private protected bool HasBoundsChangeAnimationsPending { protected get; private set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x00019FB6 File Offset: 0x000181B6
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x00019FBE File Offset: 0x000181BE
		private protected AnimationContext SharedContext { protected get; private set; }

		// Token: 0x06000740 RID: 1856 RVA: 0x00019FC7 File Offset: 0x000181C7
		protected void OnShowAnimationCompleted(UIElement element)
		{
			ElementAnimationCompleted showAnimationCompleted = this.ShowAnimationCompleted;
			if (showAnimationCompleted == null)
			{
				return;
			}
			showAnimationCompleted(this, element);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00019FDB File Offset: 0x000181DB
		protected void OnHideAnimationCompleted(UIElement element)
		{
			ElementAnimationCompleted hideAnimationCompleted = this.HideAnimationCompleted;
			if (hideAnimationCompleted == null)
			{
				return;
			}
			hideAnimationCompleted(this, element);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00019FEF File Offset: 0x000181EF
		protected void OnBoundsChangeAnimationCompleted(UIElement element)
		{
			ElementAnimationCompleted boundsChangeAnimationCompleted = this.BoundsChangeAnimationCompleted;
			if (boundsChangeAnimationCompleted == null)
			{
				return;
			}
			boundsChangeAnimationCompleted(this, element);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001A003 File Offset: 0x00018203
		private void QueueElementForAnimation(ElementAnimator.ElementInfo elementInfo)
		{
			this.m_animatingElements.Add(elementInfo);
			if (this.m_animatingElements.Count == 1)
			{
				CompositionTarget.Rendering += new EventHandler(this.OnRendering);
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001A030 File Offset: 0x00018230
		private void OnRendering(object sender, EventArgs args)
		{
			CompositionTarget.Rendering -= new EventHandler(this.OnRendering);
			try
			{
				for (int i = 0; i < this.m_animatingElements.Count; i++)
				{
					ElementAnimator.ElementInfo elementInfo = this.m_animatingElements[i];
					switch (elementInfo.Trigger)
					{
					case ElementAnimator.AnimationTrigger.Show:
						this.StartShowAnimation(elementInfo.Element, elementInfo.Context);
						break;
					case ElementAnimator.AnimationTrigger.Hide:
						this.StartHideAnimation(elementInfo.Element, elementInfo.Context);
						break;
					case ElementAnimator.AnimationTrigger.BoundsChange:
						this.StartBoundsChangeAnimation(elementInfo.Element, elementInfo.Context, elementInfo.OldBounds, elementInfo.NewBounds);
						break;
					}
				}
			}
			finally
			{
				this.ResetState();
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001A0F8 File Offset: 0x000182F8
		private void ResetState()
		{
			this.m_animatingElements.Clear();
			this.HasShowAnimationsPending = (this.HasHideAnimationsPending = (this.HasBoundsChangeAnimationsPending = false));
			this.SharedContext = AnimationContext.None;
		}

		// Token: 0x0400034C RID: 844
		private readonly List<ElementAnimator.ElementInfo> m_animatingElements = new List<ElementAnimator.ElementInfo>();

		// Token: 0x02000114 RID: 276
		private enum AnimationTrigger
		{
			// Token: 0x04000598 RID: 1432
			Show,
			// Token: 0x04000599 RID: 1433
			Hide,
			// Token: 0x0400059A RID: 1434
			BoundsChange
		}

		// Token: 0x02000115 RID: 277
		private struct ElementInfo
		{
			// Token: 0x06000CB8 RID: 3256 RVA: 0x0002B406 File Offset: 0x00029606
			public ElementInfo(UIElement element, ElementAnimator.AnimationTrigger trigger, AnimationContext context)
			{
				this.Element = element;
				this.Trigger = trigger;
				this.Context = context;
				this.OldBounds = default(Rect);
				this.NewBounds = default(Rect);
			}

			// Token: 0x06000CB9 RID: 3257 RVA: 0x0002B435 File Offset: 0x00029635
			public ElementInfo(UIElement element, ElementAnimator.AnimationTrigger trigger, AnimationContext context, Rect oldBounds, Rect newBounds)
			{
				this.Element = element;
				this.Trigger = trigger;
				this.Context = context;
				this.OldBounds = oldBounds;
				this.NewBounds = newBounds;
			}

			// Token: 0x17000234 RID: 564
			// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0002B45C File Offset: 0x0002965C
			public readonly UIElement Element { get; }

			// Token: 0x17000235 RID: 565
			// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0002B464 File Offset: 0x00029664
			public readonly ElementAnimator.AnimationTrigger Trigger { get; }

			// Token: 0x17000236 RID: 566
			// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0002B46C File Offset: 0x0002966C
			public readonly AnimationContext Context { get; }

			// Token: 0x17000237 RID: 567
			// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0002B474 File Offset: 0x00029674
			public readonly Rect OldBounds { get; }

			// Token: 0x17000238 RID: 568
			// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0002B47C File Offset: 0x0002967C
			public readonly Rect NewBounds { get; }
		}
	}
}
