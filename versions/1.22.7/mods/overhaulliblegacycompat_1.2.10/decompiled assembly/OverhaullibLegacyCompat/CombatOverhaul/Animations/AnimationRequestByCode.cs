using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000184 RID: 388
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct AnimationRequestByCode
	{
		// Token: 0x0600133E RID: 4926 RVA: 0x0005CE38 File Offset: 0x0005B038
		public AnimationRequestByCode(string animation, float animationSpeed, float weight, string category, TimeSpan easeOutDuration, TimeSpan easeInDuration, bool easeOut, [Nullable(2)] Func<bool> finishCallback = null, [Nullable(new byte[]
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

		// Token: 0x0600133F RID: 4927 RVA: 0x0005CE8C File Offset: 0x0005B08C
		[NullableContext(2)]
		public AnimationRequestByCode(AnimationRequestByCode request, float animationSpeed, Func<bool> finishCallback = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> callbackHandler = null)
		{
			this.Animation = request.Animation;
			this.AnimationSpeed = animationSpeed;
			this.Weight = request.Weight;
			this.Category = request.Category;
			this.EaseOutDuration = request.EaseOutDuration;
			this.EaseInDuration = request.EaseInDuration;
			this.EaseOut = request.EaseOut;
			this.FinishCallback = finishCallback;
			this.CallbackHandler = callbackHandler;
		}

		// Token: 0x040008FF RID: 2303
		public readonly string Animation;

		// Token: 0x04000900 RID: 2304
		public readonly float AnimationSpeed;

		// Token: 0x04000901 RID: 2305
		public readonly float Weight;

		// Token: 0x04000902 RID: 2306
		public readonly string Category;

		// Token: 0x04000903 RID: 2307
		public readonly TimeSpan EaseOutDuration;

		// Token: 0x04000904 RID: 2308
		public readonly TimeSpan EaseInDuration;

		// Token: 0x04000905 RID: 2309
		public readonly bool EaseOut;

		// Token: 0x04000906 RID: 2310
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public readonly Action<string> CallbackHandler;

		// Token: 0x04000907 RID: 2311
		[Nullable(2)]
		public readonly Func<bool> FinishCallback;
	}
}
