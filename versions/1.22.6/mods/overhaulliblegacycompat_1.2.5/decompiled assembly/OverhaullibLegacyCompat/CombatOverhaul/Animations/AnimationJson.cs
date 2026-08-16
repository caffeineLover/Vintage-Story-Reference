using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200016D RID: 365
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationJson
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001250 RID: 4688 RVA: 0x00059919 File Offset: 0x00057B19
		// (set) Token: 0x06001251 RID: 4689 RVA: 0x00059921 File Offset: 0x00057B21
		public bool Hold { get; set; }

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x0005992A File Offset: 0x00057B2A
		// (set) Token: 0x06001253 RID: 4691 RVA: 0x00059932 File Offset: 0x00057B32
		public PLayerKeyFrameJson[] PlayerKeyFrames { get; set; } = Array.Empty<PLayerKeyFrameJson>();

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x0005993B File Offset: 0x00057B3B
		// (set) Token: 0x06001255 RID: 4693 RVA: 0x00059943 File Offset: 0x00057B43
		public ItemKeyFrameJson[] ItemKeyFrames { get; set; } = Array.Empty<ItemKeyFrameJson>();

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001256 RID: 4694 RVA: 0x0005994C File Offset: 0x00057B4C
		// (set) Token: 0x06001257 RID: 4695 RVA: 0x00059954 File Offset: 0x00057B54
		public SoundFrameJson[] SoundFrames { get; set; } = Array.Empty<SoundFrameJson>();

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x0005995D File Offset: 0x00057B5D
		// (set) Token: 0x06001259 RID: 4697 RVA: 0x00059965 File Offset: 0x00057B65
		public ParticlesFrameJson[] ParticlesFrames { get; set; } = Array.Empty<ParticlesFrameJson>();

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x0005996E File Offset: 0x00057B6E
		// (set) Token: 0x0600125B RID: 4699 RVA: 0x00059976 File Offset: 0x00057B76
		public CallbackFrameJson[] CallbackFrames { get; set; } = Array.Empty<CallbackFrameJson>();

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x0005997F File Offset: 0x00057B7F
		// (set) Token: 0x0600125D RID: 4701 RVA: 0x00059987 File Offset: 0x00057B87
		public int ItemAnimationStart { get; set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x00059990 File Offset: 0x00057B90
		// (set) Token: 0x0600125F RID: 4703 RVA: 0x00059998 File Offset: 0x00057B98
		public int ItemAnimationEnd { get; set; }

		// Token: 0x06001260 RID: 4704 RVA: 0x000599A4 File Offset: 0x00057BA4
		public Animation ToAnimation()
		{
			return new Animation(Enumerable.Select<PLayerKeyFrameJson, PLayerKeyFrame>(this.PlayerKeyFrames, (PLayerKeyFrameJson element) => element.ToKeyFrame()), Enumerable.Select<ItemKeyFrameJson, ItemKeyFrame>(this.ItemKeyFrames, (ItemKeyFrameJson element) => element.ToKeyFrame()), Enumerable.Select<SoundFrameJson, SoundFrame>(this.SoundFrames, (SoundFrameJson element) => element.ToSoundFrame()), Enumerable.Select<ParticlesFrameJson, ParticlesFrame>(this.ParticlesFrames, (ParticlesFrameJson element) => element.ToParticlesFrame()), Enumerable.Select<CallbackFrameJson, CallbackFrame>(this.CallbackFrames, (CallbackFrameJson element) => element.ToCallbackFrame()))
			{
				Hold = this.Hold,
				ItemAnimationStart = TimeSpan.FromMilliseconds((long)this.ItemAnimationStart),
				ItemAnimationEnd = TimeSpan.FromMilliseconds((long)this.ItemAnimationEnd)
			};
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00059AB8 File Offset: 0x00057CB8
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

		// Token: 0x06001262 RID: 4706 RVA: 0x00059BF5 File Offset: 0x00057DF5
		public override string ToString()
		{
			return JsonUtil.ToPrettyString<AnimationJson>(this);
		}

		// Token: 0x020002B2 RID: 690
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000DBA RID: 3514
			[Nullable(0)]
			public static Func<PLayerKeyFrame, PLayerKeyFrameJson> <0>__FromKeyFrame;

			// Token: 0x04000DBB RID: 3515
			[Nullable(0)]
			public static Func<ItemKeyFrame, ItemKeyFrameJson> <1>__FromKeyFrame;

			// Token: 0x04000DBC RID: 3516
			[Nullable(0)]
			public static Func<SoundFrame, SoundFrameJson> <2>__FromSoundFrame;

			// Token: 0x04000DBD RID: 3517
			[Nullable(0)]
			public static Func<ParticlesFrame, ParticlesFrameJson> <3>__FromParticlesFrame;

			// Token: 0x04000DBE RID: 3518
			[Nullable(0)]
			public static Func<CallbackFrame, CallbackFrameJson> <4>__FromCallbackFrame;
		}
	}
}
