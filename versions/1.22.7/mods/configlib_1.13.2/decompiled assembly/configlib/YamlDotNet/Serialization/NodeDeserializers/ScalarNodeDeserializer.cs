using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200014E RID: 334
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScalarNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x0001778C File Offset: 0x0001598C
		public ScalarNodeDeserializer(bool attemptUnknownTypeDeserialization, ITypeConverter typeConverter, YamlFormatter formatter)
		{
			this.attemptUnknownTypeDeserialization = attemptUnknownTypeDeserialization;
			if (typeConverter == null)
			{
				throw new ArgumentNullException("typeConverter");
			}
			this.typeConverter = typeConverter;
			this.formatter = formatter;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000177B8 File Offset: 0x000159B8
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			Scalar scalar;
			if (!parser.TryConsume(out scalar))
			{
				value = null;
				return false;
			}
			Type type = Nullable.GetUnderlyingType(expectedType) ?? expectedType;
			if (type.IsEnum())
			{
				value = Enum.Parse(type, scalar.Value, true);
				return true;
			}
			TypeCode typeCode = type.GetTypeCode();
			switch (typeCode)
			{
			case 3:
				value = this.DeserializeBooleanHelper(scalar.Value);
				return true;
			case 4:
				value = scalar.Value.get_Chars(0);
				return true;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				value = this.DeserializeIntegerHelper(typeCode, scalar.Value);
				return true;
			case 13:
				value = float.Parse(scalar.Value, this.formatter.NumberFormat);
				return true;
			case 14:
				value = double.Parse(scalar.Value, this.formatter.NumberFormat);
				return true;
			case 15:
				value = decimal.Parse(scalar.Value, this.formatter.NumberFormat);
				return true;
			case 16:
				value = DateTime.Parse(scalar.Value, CultureInfo.InvariantCulture, 128);
				return true;
			case 18:
				value = scalar.Value;
				return true;
			}
			if (expectedType == typeof(object))
			{
				if (!scalar.IsKey && this.attemptUnknownTypeDeserialization)
				{
					value = this.AttemptUnknownTypeDeserialization(scalar);
				}
				else
				{
					value = scalar.Value;
				}
			}
			else
			{
				value = this.typeConverter.ChangeType(scalar.Value, expectedType);
			}
			return true;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00017974 File Offset: 0x00015B74
		private object DeserializeBooleanHelper(string value)
		{
			bool flag;
			if (Regex.IsMatch(value, "^(true|y|yes|on)$", 1))
			{
				flag = true;
			}
			else
			{
				if (!Regex.IsMatch(value, "^(false|n|no|off)$", 1))
				{
					throw new FormatException("The value \"" + value + "\" is not a valid YAML Boolean");
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000179C4 File Offset: 0x00015BC4
		private object DeserializeIntegerHelper(TypeCode typeCode, string value)
		{
			object result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				int i = 0;
				bool flag = false;
				ulong num = 0UL;
				if (value.get_Chars(0) == '-')
				{
					i++;
					flag = true;
				}
				else if (value.get_Chars(0) == '+')
				{
					i++;
				}
				if (value.get_Chars(i) == '0')
				{
					int num2;
					if (i == value.Length - 1)
					{
						num2 = 10;
						num = 0UL;
					}
					else
					{
						i++;
						if (value.get_Chars(i) == 'b')
						{
							num2 = 2;
							i++;
						}
						else if (value.get_Chars(i) == 'x')
						{
							num2 = 16;
							i++;
						}
						else
						{
							num2 = 8;
						}
					}
					while (i < value.Length)
					{
						if (value.get_Chars(i) != '_')
						{
							builder.Append(value.get_Chars(i));
						}
						i++;
					}
					if (num2 <= 8)
					{
						if (num2 == 2 || num2 == 8)
						{
							num = Convert.ToUInt64(builder.ToString(), num2);
						}
					}
					else if (num2 != 10)
					{
						if (num2 == 16)
						{
							num = ulong.Parse(builder.ToString(), 515, this.formatter.NumberFormat);
						}
					}
				}
				else
				{
					string[] array = value.Substring(i).Split(':', 0);
					num = 0UL;
					for (int j = 0; j < array.Length; j++)
					{
						num *= 60UL;
						num += ulong.Parse(array[j].Replace("_", ""));
					}
				}
				if (flag)
				{
					long number;
					if (num == 9223372036854775808UL)
					{
						number = long.MinValue;
					}
					else
					{
						number = checked(0L - (long)num);
					}
					result = ScalarNodeDeserializer.CastInteger(number, typeCode);
				}
				else
				{
					result = ScalarNodeDeserializer.CastInteger(num, typeCode);
				}
			}
			return result;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00017B90 File Offset: 0x00015D90
		private static object CastInteger(long number, TypeCode typeCode)
		{
			checked
			{
				object result;
				switch (typeCode)
				{
				case 5:
					result = (sbyte)number;
					break;
				case 6:
					result = (byte)number;
					break;
				case 7:
					result = (short)number;
					break;
				case 8:
					result = (ushort)number;
					break;
				case 9:
					result = (int)number;
					break;
				case 10:
					result = (uint)number;
					break;
				case 11:
					result = number;
					break;
				case 12:
					result = (ulong)number;
					break;
				default:
					result = number;
					break;
				}
				return result;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00017C20 File Offset: 0x00015E20
		private static object CastInteger(ulong number, TypeCode typeCode)
		{
			checked
			{
				object result;
				switch (typeCode)
				{
				case 5:
					result = (sbyte)number;
					break;
				case 6:
					result = (byte)number;
					break;
				case 7:
					result = (short)number;
					break;
				case 8:
					result = (ushort)number;
					break;
				case 9:
					result = (int)number;
					break;
				case 10:
					result = (uint)number;
					break;
				case 11:
					result = (long)number;
					break;
				case 12:
					result = number;
					break;
				default:
					result = number;
					break;
				}
				return result;
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00017CB0 File Offset: 0x00015EB0
		[return: Nullable(2)]
		private object AttemptUnknownTypeDeserialization(Scalar value)
		{
			if (value.Style == ScalarStyle.SingleQuoted || value.Style == ScalarStyle.DoubleQuoted || value.Style == ScalarStyle.Folded)
			{
				return value.Value;
			}
			string v = value.Value;
			string v2 = v;
			if (v2 != null)
			{
				switch (v2.Length)
				{
				case 0:
					break;
				case 1:
					if (!(v2 == "~"))
					{
						goto IL_16C;
					}
					break;
				case 2:
				case 3:
					goto IL_16C;
				case 4:
				{
					char c = v2.get_Chars(0);
					if (c <= 'T')
					{
						if (c != 'N')
						{
							if (c != 'T')
							{
								goto IL_16C;
							}
							if (!(v2 == "True") && !(v2 == "TRUE"))
							{
								goto IL_16C;
							}
						}
						else
						{
							if (!(v2 == "Null") && !(v2 == "NULL"))
							{
								goto IL_16C;
							}
							break;
						}
					}
					else if (c != 'n')
					{
						if (c != 't')
						{
							goto IL_16C;
						}
						if (!(v2 == "true"))
						{
							goto IL_16C;
						}
					}
					else
					{
						if (!(v2 == "null"))
						{
							goto IL_16C;
						}
						break;
					}
					return true;
				}
				case 5:
				{
					char c = v2.get_Chars(0);
					if (c != 'F')
					{
						if (c != 'f')
						{
							goto IL_16C;
						}
						if (!(v2 == "false"))
						{
							goto IL_16C;
						}
					}
					else if (!(v2 == "False") && !(v2 == "FALSE"))
					{
						goto IL_16C;
					}
					return false;
				}
				default:
					goto IL_16C;
				}
				return null;
			}
			IL_16C:
			object result;
			if (Regex.IsMatch(v, "0x[0-9a-fA-F]+"))
			{
				if (!ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToByte(v, 16), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt16(v, 16), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt32(v, 16), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt64(v, 16), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToUInt64(v, 16), out result))
				{
					result = v;
				}
			}
			else if (Regex.IsMatch(v, "0o[0-9a-fA-F]+"))
			{
				if (!ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToByte(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt16(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt32(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt64(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToUInt64(v, 8), out result))
				{
					result = v;
				}
			}
			else if (Regex.IsMatch(v, "[-+]?(\\.[0-9]+|[0-9]+(\\.[0-9]*)?)([eE][-+]?[0-9]+)?"))
			{
				if (!ScalarNodeDeserializer.TryAndSwallow(() => byte.Parse(v, this.formatter.NumberFormat), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => short.Parse(v, this.formatter.NumberFormat), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => int.Parse(v, this.formatter.NumberFormat), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => long.Parse(v, this.formatter.NumberFormat), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => ulong.Parse(v, this.formatter.NumberFormat), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => float.Parse(v, this.formatter.NumberFormat), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => double.Parse(v, this.formatter.NumberFormat), out result))
				{
					result = v;
				}
			}
			else if (Regex.IsMatch(v, "^[-+]?(\\.inf|\\.Inf|\\.INF)$"))
			{
				if (v.StartsWith("-"))
				{
					result = float.NegativeInfinity;
				}
				else
				{
					result = float.PositiveInfinity;
				}
			}
			else if (Regex.IsMatch(v, "^(\\.nan|\\.NaN|\\.NAN)$"))
			{
				result = float.NaN;
			}
			else
			{
				result = v;
			}
			return result;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00018084 File Offset: 0x00016284
		private static bool TryAndSwallow(Func<object> attempt, [Nullable(2)] out object value)
		{
			bool result;
			try
			{
				value = attempt.Invoke();
				result = true;
			}
			catch
			{
				value = null;
				result = false;
			}
			return result;
		}

		// Token: 0x040002EC RID: 748
		private const string BooleanTruePattern = "^(true|y|yes|on)$";

		// Token: 0x040002ED RID: 749
		private const string BooleanFalsePattern = "^(false|n|no|off)$";

		// Token: 0x040002EE RID: 750
		private readonly bool attemptUnknownTypeDeserialization;

		// Token: 0x040002EF RID: 751
		private readonly ITypeConverter typeConverter;

		// Token: 0x040002F0 RID: 752
		private readonly YamlFormatter formatter;
	}
}
