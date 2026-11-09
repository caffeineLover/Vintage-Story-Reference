using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000183 RID: 387
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct AnimationRequest
	{
		// Token: 0x0600133A RID: 4922 RVA: 0x0005CC94 File Offset: 0x0005AE94
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

		// Token: 0x0600133B RID: 4923 RVA: 0x0005CCE8 File Offset: 0x0005AEE8
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

		// Token: 0x0600133C RID: 4924 RVA: 0x0005CD5C File Offset: 0x0005AF5C
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

		// Token: 0x0600133D RID: 4925 RVA: 0x0005CDD0 File Offset: 0x0005AFD0
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

		// Token: 0x040008F6 RID: 2294
		public readonly Animation Animation;

		// Token: 0x040008F7 RID: 2295
		public readonly float AnimationSpeed;

		// Token: 0x040008F8 RID: 2296
		public readonly float Weight;

		// Token: 0x040008F9 RID: 2297
		public readonly string Category;

		// Token: 0x040008FA RID: 2298
		public readonly TimeSpan EaseOutDuration;

		// Token: 0x040008FB RID: 2299
		public readonly TimeSpan EaseInDuration;

		// Token: 0x040008FC RID: 2300
		public readonly bool EaseOut;

		// Token: 0x040008FD RID: 2301
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public readonly Action<string> CallbackHandler;

		// Token: 0x040008FE RID: 2302
		[Nullable(2)]
		public readonly Func<bool> FinishCallback;
	}
}
