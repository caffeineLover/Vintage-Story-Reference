using System;
using System.Collections.Specialized;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000091 RID: 145
	internal class AnimationManager
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x00019B66 File Offset: 0x00017D66
		public AnimationManager(ItemsRepeater owner)
		{
			this.m_owner = owner;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00019B75 File Offset: 0x00017D75
		public void OnAnimatorChanged(ElementAnimator newAnimator)
		{
			if (this.m_animator != null)
			{
				this.m_animator.HideAnimationCompleted -= this.OnHideAnimationCompleted;
			}
			this.m_animator = newAnimator;
			if (newAnimator != null)
			{
				newAnimator.HideAnimationCompleted += this.OnHideAnimationCompleted;
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00019BB2 File Offset: 0x00017DB2
		public void OnLayoutChanging()
		{
			this.m_hasRecordedLayoutTransitions = true;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00019BBC File Offset: 0x00017DBC
		public void OnItemsSourceChanged(object source, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
			case 0:
				this.m_hasRecordedAdds = true;
				return;
			case 1:
				this.m_hasRecordedRemoves = true;
				return;
			case 2:
				this.m_hasRecordedAdds = true;
				this.m_hasRecordedRemoves = true;
				return;
			case 3:
				break;
			case 4:
				this.m_hasRecordedResets = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00019C14 File Offset: 0x00017E14
		public void OnElementPrepared(UIElement element)
		{
			if (this.m_animator != null)
			{
				AnimationContext animationContext = AnimationContext.None;
				if (this.m_hasRecordedAdds)
				{
					animationContext |= AnimationContext.CollectionChangeAdd;
				}
				if (this.m_hasRecordedResets)
				{
					animationContext |= AnimationContext.CollectionChangeReset;
				}
				if (this.m_hasRecordedLayoutTransitions)
				{
					animationContext |= AnimationContext.LayoutTransition;
				}
				if (animationContext != AnimationContext.None)
				{
					this.m_animator.OnElementShown(element, animationContext);
				}
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00019C60 File Offset: 0x00017E60
		public bool ClearElement(UIElement element)
		{
			bool flag = false;
			if (this.m_animator != null)
			{
				AnimationContext animationContext = AnimationContext.None;
				if (this.m_hasRecordedRemoves)
				{
					animationContext |= AnimationContext.CollectionChangeRemove;
				}
				if (this.m_hasRecordedResets)
				{
					animationContext |= AnimationContext.CollectionChangeReset;
				}
				flag = (animationContext != AnimationContext.None && this.m_animator.HasHideAnimation(element, animationContext));
				if (flag)
				{
					this.m_animator.OnElementHidden(element, animationContext);
				}
			}
			return flag;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00019CB8 File Offset: 0x00017EB8
		public void OnElementBoundsChanged(UIElement element, Rect oldBounds, Rect newBounds)
		{
			if (this.m_animator != null)
			{
				AnimationContext animationContext = AnimationContext.None;
				if (this.m_hasRecordedAdds)
				{
					animationContext |= AnimationContext.CollectionChangeAdd;
				}
				if (this.m_hasRecordedRemoves)
				{
					animationContext |= AnimationContext.CollectionChangeRemove;
				}
				if (this.m_hasRecordedResets)
				{
					animationContext |= AnimationContext.CollectionChangeReset;
				}
				if (this.m_hasRecordedLayoutTransitions)
				{
					animationContext |= AnimationContext.LayoutTransition;
				}
				this.m_animator.OnElementBoundsChanged(element, animationContext, oldBounds, newBounds);
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00019D0E File Offset: 0x00017F0E
		public void OnOwnerArranged()
		{
			this.m_hasRecordedAdds = false;
			this.m_hasRecordedRemoves = false;
			this.m_hasRecordedResets = false;
			this.m_hasRecordedLayoutTransitions = false;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00019D2C File Offset: 0x00017F2C
		private void OnHideAnimationCompleted(ElementAnimator sender, UIElement element)
		{
			if (CachedVisualTreeHelpers.GetParent(element) == this.m_owner)
			{
				this.m_owner.ViewManager.ClearElementToElementFactory(element);
				this.m_owner.InvalidateArrange();
			}
		}

		// Token: 0x0400033F RID: 831
		private readonly ItemsRepeater m_owner;

		// Token: 0x04000340 RID: 832
		private ElementAnimator m_animator;

		// Token: 0x04000341 RID: 833
		private bool m_hasRecordedAdds;

		// Token: 0x04000342 RID: 834
		private bool m_hasRecordedRemoves;

		// Token: 0x04000343 RID: 835
		private bool m_hasRecordedResets;

		// Token: 0x04000344 RID: 836
		private bool m_hasRecordedLayoutTransitions;
	}
}
