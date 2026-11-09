using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000186 RID: 390
	[NullableContext(1)]
	[Nullable(0)]
	public static class EasingFunctions
	{
		// Token: 0x06001340 RID: 4928 RVA: 0x0005CEF8 File Offset: 0x0005B0F8
		public static EasingFunctions.EasingFunctionDelegate Get(EasingFunctionType id)
		{
			if (id > EasingFunctionType.Bounce)
			{
				return (float x) => StandardEasingFunctions.Calculate(id, x);
			}
			return EasingFunctions._modifiers[id];
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0005CF39 File Offset: 0x0005B139
		public static EasingFunctions.EasingFunctionDelegate Get(int id)
		{
			return EasingFunctions.Get((EasingFunctionType)id);
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0005CF41 File Offset: 0x0005B141
		public static EasingFunctions.EasingFunctionDelegate Get(string name)
		{
			return EasingFunctions.Get((EasingFunctionType)Enum.Parse(typeof(EasingFunctionType), name));
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0005CF5D File Offset: 0x0005B15D
		public static bool Register(int id, EasingFunctions.EasingFunctionDelegate modifier)
		{
			return EasingFunctions._modifiers.TryAdd((EasingFunctionType)id, modifier);
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0005CF6B File Offset: 0x0005B16B
		public static bool Register(string name, EasingFunctions.EasingFunctionDelegate modifier)
		{
			return EasingFunctions._modifiers.TryAdd((EasingFunctionType)EasingFunctions.ToCrc32(name), modifier);
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0005CF7E File Offset: 0x0005B17E
		internal static uint ToCrc32(string value)
		{
			return GameMath.Crc32(value.ToLowerInvariant()) & 2147483647U;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0005CF94 File Offset: 0x0005B194
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

		// Token: 0x04000933 RID: 2355
		private static readonly Dictionary<EasingFunctionType, EasingFunctions.EasingFunctionDelegate> _modifiers;

		// Token: 0x020002CB RID: 715
		// (Invoke) Token: 0x060018EA RID: 6378
		[NullableContext(0)]
		public delegate float EasingFunctionDelegate(float progress);
	}
}
