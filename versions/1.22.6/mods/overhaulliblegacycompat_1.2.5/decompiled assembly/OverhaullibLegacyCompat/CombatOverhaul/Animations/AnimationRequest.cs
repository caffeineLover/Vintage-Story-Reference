using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000176 RID: 374
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct AnimationRequest
	{
		// Token: 0x060012C3 RID: 4803 RVA: 0x0005B240 File Offset: 0x00059440
		public AnimationRequest(Animation animation, float animationSpeed, float weight, string category, TimeSpan easeOutDuration, TimeSpan easeInDuration, bool easeOut, [Nullable(2)] Func<bool> finishCallback = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> callbackHandler = null)
		{
			this.Animation = animation;
			this.AnimationSpeed = animationSpeed;
			this.Weight = weight;
			this.Category = category;
			this.EaseOutDuration = easeOutDuration;
			this.EaseInDuration = easeInDuration;
			this.EaseOut = easeOut;
			this.FinishCallback = finishCallback;
			this.CallbackHandler = callbackHandler;
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0005B294 File Offset: 0x00059494
		public AnimationRequest(Animation animation, AnimationRequestByCode request)
		{
			this.Animation = animation;
			this.AnimationSpeed = request.AnimationSpeed;
			this.Weight = request.Weight;
			this.Category = request.Category;
			this.EaseOutDuration = request.EaseOutDuration;
			this.EaseInDuration = request.EaseInDuration;
			this.EaseOut = request.EaseOut;
			this.FinishCallback = request.FinishCallback;
			this.CallbackHandler = request.CallbackHandler;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0005B308 File Offset: 0x00059508
		public AnimationRequest(Func<bool> callback, AnimationRequest request)
		{
			this.Animation = request.Animation;
			this.AnimationSpeed = request.AnimationSpeed;
			this.Weight = request.Weight;
			this.Category = request.Category;
			this.EaseOutDuration = request.EaseOutDuration;
			this.EaseInDuration = request.EaseInDuration;
			this.EaseOut = request.EaseOut;
			this.FinishCallback = callback;
			this.CallbackHandler = request.CallbackHandler;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0005B37C File Offset: 0x0005957C
		public AnimationRequest()
		{
			this.AnimationSpeed = 0f;
			this.Weight = 0f;
			this.EaseOutDuration = default(TimeSpan);
			this.EaseInDuration = default(TimeSpan);
			this.EaseOut = false;
			this.CallbackHandler = null;
			this.FinishCallback = null;
			this.Animation = Animation.Zero;
			this.Category = "";
		}

		// Token: 0x040008CE RID: 2254
		public readonly Animation Animation;

		// Token: 0x040008CF RID: 2255
		public readonly float AnimationSpeed;

		// Token: 0x040008D0 RID: 2256
		public readonly float Weight;

		// Token: 0x040008D1 RID: 2257
		public readonly string Category;

		// Token: 0x040008D2 RID: 2258
		public readonly TimeSpan EaseOutDuration;

		// Token: 0x040008D3 RID: 2259
		public readonly TimeSpan EaseInDuration;

		// Token: 0x040008D4 RID: 2260
		public readonly bool EaseOut;

		// Token: 0x040008D5 RID: 2261
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public readonly Action<string> CallbackHandler;

		// Token: 0x040008D6 RID: 2262
		[Nullable(2)]
		public readonly Func<bool> FinishCallback;
	}
}
