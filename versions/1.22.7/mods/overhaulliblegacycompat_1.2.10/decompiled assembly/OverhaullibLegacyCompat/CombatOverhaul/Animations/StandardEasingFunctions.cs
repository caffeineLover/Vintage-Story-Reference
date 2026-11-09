using System;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000187 RID: 391
	public static class StandardEasingFunctions
	{
		// Token: 0x06001347 RID: 4935 RVA: 0x0005D0AC File Offset: 0x0005B2AC
		public static float Calculate(EasingFunctionType function, float x)
		{
			switch (function)
			{
			case EasingFunctionType.Linear:
				return x;
			case EasingFunctionType.EaseInSine:
				return 1f - MathF.Cos(x * 3.1415927f / 2f);
			case EasingFunctionType.EaseOutSine:
				return MathF.Sin(x * 3.1415927f / 2f);
			case EasingFunctionType.EaseInOutSine:
				return -(MathF.Cos(3.1415927f * x) - 1f) / 2f;
			case EasingFunctionType.EaseInQuad:
				return x * x;
			case EasingFunctionType.EaseOutQuad:
				return 1f - (1f - x) * (1f - x);
			case EasingFunctionType.EaseInOutQuad:
				return ((double)x < 0.5) ? (2f * x * x) : (1f - MathF.Pow(-2f * x + 2f, 2f) / 2f);
			case EasingFunctionType.EaseInCubic:
				return x * x * x;
			case EasingFunctionType.EaseOutCubic:
				return 1f - MathF.Pow(1f - x, 3f);
			case EasingFunctionType.EaseInOutCubic:
				return ((double)x < 0.5) ? (4f * x * x * x) : (1f - MathF.Pow(-2f * x + 2f, 3f) / 2f);
			case EasingFunctionType.EaseInQuart:
				return x * x * x * x;
			case EasingFunctionType.EaseOutQuart:
				return 1f - MathF.Pow(1f - x, 4f);
			case EasingFunctionType.EaseInOutQuart:
				return ((double)x < 0.5) ? (8f * x * x * x * x) : (1f - MathF.Pow(-2f * x + 2f, 4f) / 2f);
			case EasingFunctionType.EaseInQuint:
				return x * x * x * x * x;
			case EasingFunctionType.EaseOutQuint:
				return 1f - MathF.Pow(1f - x, 5f);
			case EasingFunctionType.EaseInOutQuint:
				return ((double)x < 0.5) ? (16f * x * x * x * x * x) : (1f - MathF.Pow(-2f * x + 2f, 5f) / 2f);
			case EasingFunctionType.EaseInExpo:
				return (x == 0f) ? 0f : MathF.Pow(2f, 10f * x - 10f);
			case EasingFunctionType.EaseOutExpo:
				return (x == 1f) ? 1f : (1f - MathF.Pow(2f, -10f * x));
			case EasingFunctionType.EaseInOutExpo:
				return (x == 0f) ? 0f : ((x == 1f) ? 1f : (((double)x < 0.5) ? (MathF.Pow(2f, 20f * x - 10f) / 2f) : ((2f - MathF.Pow(2f, -20f * x + 10f)) / 2f)));
			case EasingFunctionType.EaseInCirc:
				return 1f - MathF.Sqrt(1f - MathF.Pow(x, 2f));
			case EasingFunctionType.EaseOutCirc:
				return MathF.Sqrt(1f - MathF.Pow(x - 1f, 2f));
			case EasingFunctionType.EaseInOutCirc:
				return ((double)x < 0.5) ? ((1f - MathF.Sqrt(1f - MathF.Pow(2f * x, 2f))) / 2f) : ((MathF.Sqrt(1f - MathF.Pow(-2f * x + 2f, 2f)) + 1f) / 2f);
			case EasingFunctionType.EaseInBack:
				return 2.70158f * x * x * x - 1.70158f * x * x;
			case EasingFunctionType.EaseOutBack:
				return 1f + 2.70158f * MathF.Pow(x - 1f, 3f) + 1.70158f * MathF.Pow(x - 1f, 2f);
			case EasingFunctionType.EaseInOutBack:
				return ((double)x < 0.5) ? (MathF.Pow(2f * x, 2f) * (7.189819f * x - 2.5949094f) / 2f) : ((MathF.Pow(2f * x - 2f, 2f) * (3.5949094f * (x * 2f - 2f) + 2.5949094f) + 2f) / 2f);
			case EasingFunctionType.EaseInElastic:
				return (x == 0f) ? 0f : ((x == 1f) ? 1f : (-MathF.Pow(2f, 10f * x - 10f) * MathF.Sin((x * 10f - 10.75f) * 2.0943952f)));
			case EasingFunctionType.EaseOutElastic:
				return (x == 0f) ? 0f : ((x == 1f) ? 1f : (MathF.Pow(2f, -10f * x) * MathF.Sin((x * 10f - 0.75f) * 2.0943952f) + 1f));
			case EasingFunctionType.EaseInOutElastic:
				return (x == 0f) ? 0f : ((x == 1f) ? 1f : (((double)x < 0.5) ? (-(MathF.Pow(2f, 20f * x - 10f) * MathF.Sin((20f * x - 11.125f) * 1.3962635f)) / 2f) : (MathF.Pow(2f, -20f * x + 10f) * MathF.Sin((20f * x - 11.125f) * 1.3962635f) / 2f + 1f)));
			case EasingFunctionType.EaseInBounce:
				return 1f - StandardEasingFunctions.Bounce(1f - x);
			case EasingFunctionType.EaseOutBounce:
				return StandardEasingFunctions.Bounce(x);
			case EasingFunctionType.EaseInOutBounce:
				return ((double)x < 0.5) ? ((1f - StandardEasingFunctions.Bounce(1f - 2f * x)) / 2f) : ((1f + StandardEasingFunctions.Bounce(2f * x - 1f)) / 2f);
			}
			return x;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0005D780 File Offset: 0x0005B980
		public static float Bounce(float x)
		{
			if (x < 0.36363637f)
			{
				return 7.5625f * x * x;
			}
			if (x < 0.72727275f)
			{
				return 7.5625f * ((x - 1.5f) / 2.75f) * (x - 1.5f) + 0.75f;
			}
			if ((double)x < 0.9090909090909091)
			{
				return 7.5625f * ((x - 2.25f) / 2.75f) * (x - 2.25f) + 0.9375f;
			}
			return 7.5625f * ((x - 2.625f) / 2.75f) * (x - 2.625f) + 0.984375f;
		}

		// Token: 0x04000934 RID: 2356
		public const float BackC1 = 1.70158f;

		// Token: 0x04000935 RID: 2357
		public const float BackC2 = 2.5949094f;

		// Token: 0x04000936 RID: 2358
		public const float BackC3 = 2.70158f;

		// Token: 0x04000937 RID: 2359
		public const float ElasticC1 = 2.0943952f;

		// Token: 0x04000938 RID: 2360
		public const float ElasticC2 = 1.3962635f;

		// Token: 0x04000939 RID: 2361
		public const float BounceN1 = 7.5625f;

		// Token: 0x0400093A RID: 2362
		public const float BounceD1 = 2.75f;
	}
}
