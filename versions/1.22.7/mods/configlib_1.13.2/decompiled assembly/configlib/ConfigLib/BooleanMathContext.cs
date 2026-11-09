using System;
using System.IO;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x02000032 RID: 50
	internal sealed class BooleanMathContext : IContext<float, float>
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00008DAC File Offset: 0x00006FAC
		[NullableContext(1)]
		public bool Resolvable(string name)
		{
			return name == "if" || name == "not" || name == "and" || name == "or" || name == "true" || name == "false";
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008E24 File Offset: 0x00007024
		[NullableContext(1)]
		public float Resolve(string name, params float[] arguments)
		{
			float result;
			if (!(name == "if"))
			{
				if (!(name == "not"))
				{
					if (!(name == "and"))
					{
						if (!(name == "or"))
						{
							if (!(name == "true"))
							{
								if (!(name == "false"))
								{
									throw new InvalidDataException("Unknown function: '" + name + "'");
								}
								result = 0f;
							}
							else
							{
								result = 1f;
							}
						}
						else
						{
							result = BooleanMathContext.AsFloat(BooleanMathContext.AsBool(arguments[0]) || BooleanMathContext.AsBool(arguments[1]));
						}
					}
					else
					{
						result = BooleanMathContext.AsFloat(BooleanMathContext.AsBool(arguments[0]) && BooleanMathContext.AsBool(arguments[1]));
					}
				}
				else
				{
					result = BooleanMathContext.AsFloat(!BooleanMathContext.AsBool(arguments[0]));
				}
			}
			else
			{
				result = (BooleanMathContext.AsBool(arguments[0]) ? arguments[1] : arguments[2]);
			}
			return result;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008F0A File Offset: 0x0000710A
		private static bool AsBool(float value)
		{
			return MathF.Abs(value) > 1E-10f;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008F19 File Offset: 0x00007119
		private static float AsFloat(bool value)
		{
			if (!value)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x040000AB RID: 171
		private const float _epsilon = 1E-10f;

		// Token: 0x040000AC RID: 172
		private const float _true = 1f;

		// Token: 0x040000AD RID: 173
		private const float _false = 0f;
	}
}
