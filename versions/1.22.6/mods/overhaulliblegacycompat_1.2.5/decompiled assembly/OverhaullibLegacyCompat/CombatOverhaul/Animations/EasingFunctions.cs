using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000179 RID: 377
	[NullableContext(1)]
	[Nullable(0)]
	public static class EasingFunctions
	{
		// Token: 0x060012C9 RID: 4809 RVA: 0x0005B4A4 File Offset: 0x000596A4
		public static EasingFunctions.EasingFunctionDelegate Get(EasingFunctionType id)
		{
			if (id > EasingFunctionType.Bounce)
			{
				return (float x) => StandardEasingFunctions.Calculate(id, x);
			}
			return EasingFunctions._modifiers[id];
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0005B4E5 File Offset: 0x000596E5
		public static EasingFunctions.EasingFunctionDelegate Get(int id)
		{
			return EasingFunctions.Get((EasingFunctionType)id);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0005B4ED File Offset: 0x000596ED
		public static EasingFunctions.EasingFunctionDelegate Get(string name)
		{
			return EasingFunctions.Get((EasingFunctionType)Enum.Parse(typeof(EasingFunctionType), name));
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0005B509 File Offset: 0x00059709
		public static bool Register(int id, EasingFunctions.EasingFunctionDelegate modifier)
		{
			return EasingFunctions._modifiers.TryAdd((EasingFunctionType)id, modifier);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0005B517 File Offset: 0x00059717
		public static bool Register(string name, EasingFunctions.EasingFunctionDelegate modifier)
		{
			return EasingFunctions._modifiers.TryAdd((EasingFunctionType)EasingFunctions.ToCrc32(name), modifier);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0005B52A File Offset: 0x0005972A
		internal static uint ToCrc32(string value)
		{
			return GameMath.Crc32(value.ToLowerInvariant()) & 2147483647U;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0005B540 File Offset: 0x00059740
		// Note: this type is marked as 'beforefieldinit'.
		static EasingFunctions()
		{
			Dictionary<EasingFunctionType, EasingFunctions.EasingFunctionDelegate> dictionary = new Dictionary<EasingFunctionType, EasingFunctions.EasingFunctionDelegate>();
			dictionary.Add(EasingFunctionType.Linear, (float progress) => progress);
			dictionary.Add(EasingFunctionType.Quadratic, (float progress) => progress * progress);
			dictionary.Add(EasingFunctionType.Cubic, (float progress) => progress * progress * progress);
			dictionary.Add(EasingFunctionType.Quintic, (float progress) => progress * progress * progress * progress * progress);
			dictionary.Add(EasingFunctionType.Sqrt, (float progress) => GameMath.Sqrt(progress));
			dictionary.Add(EasingFunctionType.Sin, (float progress) => GameMath.Sin(progress / 2f * 3.1415927f));
			dictionary.Add(EasingFunctionType.SinQuadratic, (float progress) => GameMath.Sin(progress * progress / 2f * 3.1415927f));
			dictionary.Add(EasingFunctionType.SinQuartic, (float progress) => GameMath.Sin(progress * progress * progress * progress / 2f * 3.1415927f));
			dictionary.Add(EasingFunctionType.CosShifted, (float progress) => 0.5f - GameMath.Cos(progress * 3.1415927f) / 2f);
			dictionary.Add(EasingFunctionType.SqrtSqrt, (float progress) => GameMath.Sqrt(GameMath.Sqrt(progress)));
			dictionary.Add(EasingFunctionType.Bounce, (float progress) => 0.5f - GameMath.Cos(progress * 3.1415927f) / 2f + MathF.Pow(GameMath.Sin(progress * 3.1415927f), 2f) * 0.35f);
			EasingFunctions._modifiers = dictionary;
		}

		// Token: 0x0400090B RID: 2315
		private static readonly Dictionary<EasingFunctionType, EasingFunctions.EasingFunctionDelegate> _modifiers;

		// Token: 0x020002B8 RID: 696
		// (Invoke) Token: 0x06001845 RID: 6213
		[NullableContext(0)]
		public delegate float EasingFunctionDelegate(float progress);
	}
}
