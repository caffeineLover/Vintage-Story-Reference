using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000177 RID: 375
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct AnimationRequestByCode
	{
		// Token: 0x060012C7 RID: 4807 RVA: 0x0005B3E4 File Offset: 0x000595E4
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

		// Token: 0x060012C8 RID: 4808 RVA: 0x0005B438 File Offset: 0x00059638
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

		// Token: 0x040008D7 RID: 2263
		public readonly string Animation;

		// Token: 0x040008D8 RID: 2264
		public readonly float AnimationSpeed;

		// Token: 0x040008D9 RID: 2265
		public readonly float Weight;

		// Token: 0x040008DA RID: 2266
		public readonly string Category;

		// Token: 0x040008DB RID: 2267
		public readonly TimeSpan EaseOutDuration;

		// Token: 0x040008DC RID: 2268
		public readonly TimeSpan EaseInDuration;

		// Token: 0x040008DD RID: 2269
		public readonly bool EaseOut;

		// Token: 0x040008DE RID: 2270
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public readonly Action<string> CallbackHandler;

		// Token: 0x040008DF RID: 2271
		[Nullable(2)]
		public readonly Func<bool> FinishCallback;
	}
}
