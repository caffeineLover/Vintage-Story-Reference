using System;
using System.IO;
using System.Runtime.CompilerServices;
using SimpleExpressionEngine;

namespace ConfigLib
{
	// Token: 0x02000030 RID: 48
	internal sealed class BooleanMathContext : IContext<float, float>
	{
		// Token: 0x06000167 RID: 359 RVA: 0x00008A90 File Offset: 0x00006C90
		[NullableContext(1)]
		public bool Resolvable(string name)
		{
			return name == "if" || name == "not" || name == "and" || name == "or" || name == "true" || name == "false";
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008B08 File Offset: 0x00006D08
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

		// Token: 0x06000169 RID: 361 RVA: 0x00008BEE File Offset: 0x00006DEE
		private static bool AsBool(float value)
		{
			return MathF.Abs(value) > 1E-10f;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008BFD File Offset: 0x00006DFD
		private static float AsFloat(bool value)
		{
			if (!value)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x040000A8 RID: 168
		private const float _epsilon = 1E-10f;

		// Token: 0x040000A9 RID: 169
		private const float _true = 1f;

		// Token: 0x040000AA RID: 170
		private const float _false = 0f;
	}
}
