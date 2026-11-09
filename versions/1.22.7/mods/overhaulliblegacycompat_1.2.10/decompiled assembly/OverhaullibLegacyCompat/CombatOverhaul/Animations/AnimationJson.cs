using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000179 RID: 377
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationJson
	{
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060012C4 RID: 4804 RVA: 0x0005B239 File Offset: 0x00059439
		// (set) Token: 0x060012C5 RID: 4805 RVA: 0x0005B241 File Offset: 0x00059441
		public bool Hold { get; set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0005B24A File Offset: 0x0005944A
		// (set) Token: 0x060012C7 RID: 4807 RVA: 0x0005B252 File Offset: 0x00059452
		public PLayerKeyFrameJson[] PlayerKeyFrames { get; set; } = Array.Empty<PLayerKeyFrameJson>();

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0005B25B File Offset: 0x0005945B
		// (set) Token: 0x060012C9 RID: 4809 RVA: 0x0005B263 File Offset: 0x00059463
		public ItemKeyFrameJson[] ItemKeyFrames { get; set; } = Array.Empty<ItemKeyFrameJson>();

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x0005B26C File Offset: 0x0005946C
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x0005B274 File Offset: 0x00059474
		public SoundFrameJson[] SoundFrames { get; set; } = Array.Empty<SoundFrameJson>();

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x0005B27D File Offset: 0x0005947D
		// (set) Token: 0x060012CD RID: 4813 RVA: 0x0005B285 File Offset: 0x00059485
		public ParticlesFrameJson[] ParticlesFrames { get; set; } = Array.Empty<ParticlesFrameJson>();

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x0005B28E File Offset: 0x0005948E
		// (set) Token: 0x060012CF RID: 4815 RVA: 0x0005B296 File Offset: 0x00059496
		public CallbackFrameJson[] CallbackFrames { get; set; } = Array.Empty<CallbackFrameJson>();

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0005B29F File Offset: 0x0005949F
		// (set) Token: 0x060012D1 RID: 4817 RVA: 0x0005B2A7 File Offset: 0x000594A7
		public int ItemAnimationStart { get; set; }

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x0005B2B0 File Offset: 0x000594B0
		// (set) Token: 0x060012D3 RID: 4819 RVA: 0x0005B2B8 File Offset: 0x000594B8
		public int ItemAnimationEnd { get; set; }

		// Token: 0x060012D4 RID: 4820 RVA: 0x0005B2C4 File Offset: 0x000594C4
		public Animation ToAnimation()
		{
			return new Animation(Enumerable.Select<PLayerKeyFrameJson, PLayerKeyFrame>(this.PlayerKeyFrames, (PLayerKeyFrameJson element) => element.ToKeyFrame()), Enumerable.Select<ItemKeyFrameJson, ItemKeyFrame>(this.ItemKeyFrames, (ItemKeyFrameJson element) => element.ToKeyFrame()), Enumerable.Select<SoundFrameJson, SoundFrame>(this.SoundFrames, (SoundFrameJson element) => element.ToSoundFrame()), Enumerable.Select<ParticlesFrameJson, ParticlesFrame>(this.ParticlesFrames, (ParticlesFrameJson element) => element.ToParticlesFrame()), Enumerable.Select<CallbackFrameJson, CallbackFrame>(this.CallbackFrames, (CallbackFrameJson element) => element.ToCallbackFrame()))
			{
				Hold = this.Hold,
				ItemAnimationStart = TimeSpan.FromMilliseconds((long)this.ItemAnimationStart),
				ItemAnimationEnd = TimeSpan.FromMilliseconds((long)this.ItemAnimationEnd)
			};
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0005B3D8 File Offset: 0x000595D8
		public static AnimationJson FromAnimation(Animation animation)
		{
			AnimationJson animationJson = new AnimationJson();
			animationJson.Hold = animation.Hold;
			IEnumerable<PLayerKeyFrame> playerKeyFrames = animation.PlayerKeyFrames;
			Func<PLayerKeyFrame, PLayerKeyFrameJson> func;
			if ((func = AnimationJson.<>O.<0>__FromKeyFrame) == null)
			{
				func = (AnimationJson.<>O.<0>__FromKeyFrame = new Func<PLayerKeyFrame, PLayerKeyFrameJson>(PLayerKeyFrameJson.FromKeyFrame));
			}
			animationJson.PlayerKeyFrames = Enumerable.ToArray<PLayerKeyFrameJson>(Enumerable.Select<PLayerKeyFrame, PLayerKeyFrameJson>(playerKeyFrames, func));
			IEnumerable<ItemKeyFrame> itemKeyFrames = animation.ItemKeyFrames;
			Func<ItemKeyFrame, ItemKeyFrameJson> func2;
			if ((func2 = AnimationJson.<>O.<1>__FromKeyFrame) == null)
			{
				func2 = (AnimationJson.<>O.<1>__FromKeyFrame = new Func<ItemKeyFrame, ItemKeyFrameJson>(ItemKeyFrameJson.FromKeyFrame));
			}
			animationJson.ItemKeyFrames = Enumerable.ToArray<ItemKeyFrameJson>(Enumerable.Select<ItemKeyFrame, ItemKeyFrameJson>(itemKeyFrames, func2));
			IEnumerable<SoundFrame> soundFrames = animation.SoundFrames;
			Func<SoundFrame, SoundFrameJson> func3;
			if ((func3 = AnimationJson.<>O.<2>__FromSoundFrame) == null)
			{
				func3 = (AnimationJson.<>O.<2>__FromSoundFrame = new Func<SoundFrame, SoundFrameJson>(SoundFrameJson.FromSoundFrame));
			}
			animationJson.SoundFrames = Enumerable.ToArray<SoundFrameJson>(Enumerable.Select<SoundFrame, SoundFrameJson>(soundFrames, func3));
			IEnumerable<ParticlesFrame> particlesFrames = animation.ParticlesFrames;
			Func<ParticlesFrame, ParticlesFrameJson> func4;
			if ((func4 = AnimationJson.<>O.<3>__FromParticlesFrame) == null)
			{
				func4 = (AnimationJson.<>O.<3>__FromParticlesFrame = new Func<ParticlesFrame, ParticlesFrameJson>(ParticlesFrameJson.FromParticlesFrame));
			}
			animationJson.ParticlesFrames = Enumerable.ToArray<ParticlesFrameJson>(Enumerable.Select<ParticlesFrame, ParticlesFrameJson>(particlesFrames, func4));
			IEnumerable<CallbackFrame> callbackFrames = animation.CallbackFrames;
			Func<CallbackFrame, CallbackFrameJson> func5;
			if ((func5 = AnimationJson.<>O.<4>__FromCallbackFrame) == null)
			{
				func5 = (AnimationJson.<>O.<4>__FromCallbackFrame = new Func<CallbackFrame, CallbackFrameJson>(CallbackFrameJson.FromCallbackFrame));
			}
			animationJson.CallbackFrames = Enumerable.ToArray<CallbackFrameJson>(Enumerable.Select<CallbackFrame, CallbackFrameJson>(callbackFrames, func5));
			animationJson.ItemAnimationStart = (int)animation.ItemAnimationStart.TotalMilliseconds;
			animationJson.ItemAnimationEnd = (int)animation.ItemAnimationEnd.TotalMilliseconds;
			return animationJson;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0005B515 File Offset: 0x00059715
		public override string ToString()
		{
			return JsonUtil.ToPrettyString<AnimationJson>(this);
		}

		// Token: 0x020002C5 RID: 709
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000E06 RID: 3590
			[Nullable(0)]
			public static Func<PLayerKeyFrame, PLayerKeyFrameJson> <0>__FromKeyFrame;

			// Token: 0x04000E07 RID: 3591
			[Nullable(0)]
			public static Func<ItemKeyFrame, ItemKeyFrameJson> <1>__FromKeyFrame;

			// Token: 0x04000E08 RID: 3592
			[Nullable(0)]
			public static Func<SoundFrame, SoundFrameJson> <2>__FromSoundFrame;

			// Token: 0x04000E09 RID: 3593
			[Nullable(0)]
			public static Func<ParticlesFrame, ParticlesFrameJson> <3>__FromParticlesFrame;

			// Token: 0x04000E0A RID: 3594
			[Nullable(0)]
			public static Func<CallbackFrame, CallbackFrameJson> <4>__FromCallbackFrame;
		}
	}
}
