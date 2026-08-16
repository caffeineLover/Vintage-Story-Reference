using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000197 RID: 407
	[NullableContext(1)]
	[Nullable(0)]
	public class ClientItemAnimator : ClientAnimator
	{
		// Token: 0x060013C0 RID: 5056 RVA: 0x000620EB File Offset: 0x000602EB
		public ClientItemAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, Animation[] animations, Action<string> onAnimationStoppedListener = null, Action<AnimationSound> onShouldPlaySoundListener = null) : base(walkSpeedSupplier, animations, onAnimationStoppedListener, ClientItemAnimator.AdaptSoundListener(onShouldPlaySoundListener))
		{
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000620FD File Offset: 0x000602FD
		public ClientItemAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById, Action<string> onAnimationStoppedListener = null, Action<AnimationSound> onShouldPlaySoundListener = null) : base(walkSpeedSupplier, animations, rootElements, jointsById, onAnimationStoppedListener, ClientItemAnimator.AdaptSoundListener(onShouldPlaySoundListener))
		{
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00062113 File Offset: 0x00060313
		public ClientItemAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, List<ElementPose> rootPoses, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById, Action<string> onAnimationStoppedListener = null, Action<AnimationSound> onShouldPlaySoundListener = null) : base(walkSpeedSupplier, rootPoses, animations, rootElements, jointsById, onAnimationStoppedListener, ClientItemAnimator.AdaptSoundListener(onShouldPlaySoundListener))
		{
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0006212C File Offset: 0x0006032C
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
