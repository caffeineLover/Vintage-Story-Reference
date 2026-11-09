using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005D RID: 93
	public sealed class MathContext : IContext<float, float>
	{
		// Token: 0x06000281 RID: 641 RVA: 0x0000DC44 File Offset: 0x0000BE44
		[NullableContext(1)]
		public bool Resolvable(string name)
		{
			if (name != null)
			{
				switch (name.Length)
				{
				case 1:
					if (name == "e")
					{
						return true;
					}
					break;
				case 2:
					if (name == "pi")
					{
						return true;
					}
					break;
				case 3:
				{
					char c = name.get_Chars(0);
					if (c <= 'l')
					{
						switch (c)
						{
						case 'a':
							if (name == "abs")
							{
								return true;
							}
							break;
						case 'b':
						case 'd':
							break;
						case 'c':
							if (name == "cos")
							{
								return true;
							}
							break;
						case 'e':
							if (name == "exp")
							{
								return true;
							}
							break;
						default:
							if (c == 'l')
							{
								if (name == "log")
								{
									return true;
								}
							}
							break;
						}
					}
					else if (c != 'm')
					{
						if (c == 's')
						{
							if (name == "sin")
							{
								return true;
							}
						}
					}
					else
					{
						if (name == "max")
						{
							return true;
						}
						if (name == "min")
						{
							return true;
						}
					}
					break;
				}
				case 4:
				{
					char c = name.get_Chars(1);
					if (c != 'i')
					{
						if (c == 'q')
						{
							if (name == "sqrt")
							{
								return true;
							}
						}
					}
					else if (name == "sign")
					{
						return true;
					}
					break;
				}
				case 5:
				{
					char c = name.get_Chars(0);
					switch (c)
					{
					case 'c':
						if (name == "clamp")
						{
							return true;
						}
						break;
					case 'd':
						break;
					case 'e':
						if (name == "equal")
						{
							return true;
						}
						break;
					case 'f':
						if (name == "floor")
						{
							return true;
						}
						break;
					default:
						if (c == 'r')
						{
							if (name == "round")
							{
								return true;
							}
						}
						break;
					}
					break;
				}
				case 6:
					if (name == "lesser")
					{
						return true;
					}
					break;
				case 7:
				{
					char c = name.get_Chars(0);
					if (c != 'c')
					{
						if (c == 'g')
						{
							if (name == "greater")
							{
								return true;
							}
						}
					}
					else if (name == "ceiling")
					{
						return true;
					}
					break;
				}
				}
			}
			return false;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		[NullableContext(1)]
		public float Resolve(string name, params float[] arguments)
		{
			if (name != null)
			{
				float result;
				switch (name.Length)
				{
				case 1:
					if (!(name == "e"))
					{
						goto IL_3E2;
					}
					result = 2.7182817f;
					break;
				case 2:
					if (!(name == "pi"))
					{
						goto IL_3E2;
					}
					result = 3.1415927f;
					break;
				case 3:
				{
					char c = name.get_Chars(0);
					if (c <= 'l')
					{
						switch (c)
						{
						case 'a':
							if (!(name == "abs"))
							{
								goto IL_3E2;
							}
							result = MathF.Abs(arguments[0]);
							break;
						case 'b':
						case 'd':
							goto IL_3E2;
						case 'c':
							if (!(name == "cos"))
							{
								goto IL_3E2;
							}
							result = MathF.Cos(arguments[0]);
							break;
						case 'e':
							if (!(name == "exp"))
							{
								goto IL_3E2;
							}
							result = MathF.Exp(arguments[0]);
							break;
						default:
							if (c != 'l')
							{
								goto IL_3E2;
							}
							if (!(name == "log"))
							{
								goto IL_3E2;
							}
							result = MathF.Log(arguments[0]);
							break;
						}
					}
					else if (c != 'm')
					{
						if (c != 's')
						{
							goto IL_3E2;
						}
						if (!(name == "sin"))
						{
							goto IL_3E2;
						}
						result = MathF.Sin(arguments[0]);
					}
					else if (!(name == "max"))
					{
						if (!(name == "min"))
						{
							goto IL_3E2;
						}
						result = MathF.Min(arguments[0], arguments[1]);
					}
					else
					{
						result = MathF.Max(arguments[0], arguments[1]);
					}
					break;
				}
				case 4:
				{
					char c = name.get_Chars(1);
					if (c != 'i')
					{
						if (c != 'q')
						{
							goto IL_3E2;
						}
						if (!(name == "sqrt"))
						{
							goto IL_3E2;
						}
						result = MathF.Sqrt(arguments[0]);
					}
					else
					{
						if (!(name == "sign"))
						{
							goto IL_3E2;
						}
						result = (float)MathF.Sign(arguments[0]);
					}
					break;
				}
				case 5:
				{
					char c = name.get_Chars(0);
					switch (c)
					{
					case 'c':
						if (!(name == "clamp"))
						{
							goto IL_3E2;
						}
						result = Math.Clamp(arguments[0], arguments[1], arguments[2]);
						break;
					case 'd':
						goto IL_3E2;
					case 'e':
						if (!(name == "equal"))
						{
							goto IL_3E2;
						}
						result = ((MathF.Abs(arguments[0] - arguments[1]) < MathF.Max(1E-15f, 1E-15f * MathF.Min(arguments[0], arguments[1]))) ? arguments[2] : arguments[3]);
						break;
					case 'f':
						if (!(name == "floor"))
						{
							goto IL_3E2;
						}
						result = MathF.Floor(arguments[0]);
						break;
					default:
						if (c != 'r')
						{
							goto IL_3E2;
						}
						if (!(name == "round"))
						{
							goto IL_3E2;
						}
						result = MathF.Round(arguments[0]);
						break;
					}
					break;
				}
				case 6:
					if (!(name == "lesser"))
					{
						goto IL_3E2;
					}
					result = ((arguments[0] < arguments[1]) ? arguments[2] : arguments[3]);
					break;
				case 7:
				{
					char c = name.get_Chars(0);
					if (c != 'c')
					{
						if (c != 'g')
						{
							goto IL_3E2;
						}
						if (!(name == "greater"))
						{
							goto IL_3E2;
						}
						result = ((arguments[0] > arguments[1]) ? arguments[2] : arguments[3]);
					}
					else
					{
						if (!(name == "ceiling"))
						{
							goto IL_3E2;
						}
						result = MathF.Ceiling(arguments[0]);
					}
					break;
				}
				case 8:
					if (!(name == "notequal"))
					{
						goto IL_3E2;
					}
					result = ((MathF.Abs(arguments[0] - arguments[1]) > MathF.Max(1E-15f, 1E-15f * MathF.Min(arguments[0], arguments[1]))) ? arguments[2] : arguments[3]);
					break;
				default:
					goto IL_3E2;
				}
				return result;
			}
			IL_3E2:
			throw new InvalidDataException("Unknown function: '" + name + "'");
		}

		// Token: 0x04000135 RID: 309
		private const float cEpsilon = 1E-15f;
	}
}
