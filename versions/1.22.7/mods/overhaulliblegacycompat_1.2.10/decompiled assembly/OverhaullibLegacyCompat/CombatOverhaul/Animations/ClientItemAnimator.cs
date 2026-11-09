using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A5 RID: 421
	[NullableContext(1)]
	[Nullable(0)]
	public class ClientItemAnimator : ClientAnimator
	{
		// Token: 0x06001438 RID: 5176 RVA: 0x00063B6B File Offset: 0x00061D6B
		public ClientItemAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, Animation[] animations, Action<string> onAnimationStoppedListener = null, Action<AnimationSound> onShouldPlaySoundListener = null) : base(walkSpeedSupplier, animations, onAnimationStoppedListener, ClientItemAnimator.AdaptSoundListener(onShouldPlaySoundListener))
		{
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00063B7D File Offset: 0x00061D7D
		public ClientItemAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById, Action<string> onAnimationStoppedListener = null, Action<AnimationSound> onShouldPlaySoundListener = null) : base(walkSpeedSupplier, animations, rootElements, jointsById, onAnimationStoppedListener, ClientItemAnimator.AdaptSoundListener(onShouldPlaySoundListener))
		{
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00063B93 File Offset: 0x00061D93
		public ClientItemAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, List<ElementPose> rootPoses, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById, Action<string> onAnimationStoppedListener = null, Action<AnimationSound> onShouldPlaySoundListener = null) : base(walkSpeedSupplier, rootPoses, animations, rootElements, jointsById, onAnimationStoppedListener, ClientItemAnimator.AdaptSoundListener(onShouldPlaySoundListener))
		{
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00063BAC File Offset: 0x00061DAC
		[return: Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		private static Action<string, AnimationSound> AdaptSoundListener([Nullable(new byte[]
		{
			2,
			1
		})] Action<AnimationSound> listener)
		{
			if (listener != null)
			{
				return delegate(string _, AnimationSound sound)
				{
					listener.Invoke(sound);
				};
			}
			return null;
		}
	}
}
