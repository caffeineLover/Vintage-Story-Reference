using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.ObjectPool;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000081 RID: 129
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ScalarNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002ED RID: 749 RVA: 0x0000A547 File Offset: 0x00008747
		public ScalarNodeDeserializer(bool attemptUnknownTypeDeserialization, ITypeConverter typeConverter, ITypeInspector typeInspector, YamlFormatter formatter, INamingConvention enumNamingConvention)
		{
			this.attemptUnknownTypeDeserialization = attemptUnknownTypeDeserialization;
			if (typeConverter == null)
			{
				throw new ArgumentNullException("typeConverter");
			}
			this.typeConverter = typeConverter;
			this.typeInspector = typeInspector;
			this.formatter = formatter;
			this.enumNamingConvention = enumNamingConvention;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000A584 File Offset: 0x00008784
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			Scalar scalar;
			if (!parser.TryConsume(out scalar))
			{
				value = null;
				return false;
			}
			Type type;
			if ((type = Nullable.GetUnderlyingType(expectedType)) == null)
			{
				type = (FsharpHelper.GetOptionUnderlyingType(expectedType) ?? expectedType);
			}
			Type type2 = type;
			if (type2.IsEnum())
			{
				string text = this.enumNamingConvention.Reverse(scalar.Value);
				text = this.typeInspector.GetEnumName(type2, text);
				value = Enum.Parse(type2, text, true);
				return true;
			}
			TypeCode typeCode = type2.GetTypeCode();
			switch (typeCode)
			{
			case 3:
				value = ScalarNodeDeserializer.DeserializeBooleanHelper(scalar.Value);
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
				value = this.typeConverter.ChangeType(scalar.Value, expectedType, this.enumNamingConvention, this.typeInspector);
			}
			return true;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000A774 File Offset: 0x00008974
		private static bool DeserializeBooleanHelper(string value)
		{
			bool result;
			if (Regex.IsMatch(value, "^(true|y|yes|on)$", 1))
			{
				result = true;
			}
			else
			{
				if (!Regex.IsMatch(value, "^(false|n|no|off)$", 1))
				{
					throw new FormatException("The value \"" + value + "\" is not a valid YAML Boolean");
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000A7BC File Offset: 0x000089BC
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
						num += ulong.Parse(array[j].Replace("_", ""), CultureInfo.InvariantCulture);
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

		// Token: 0x060002F1 RID: 753 RVA: 0x0000A990 File Offset: 0x00008B90
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

		// Token: 0x060002F2 RID: 754 RVA: 0x0000AA20 File Offset: 0x00008C20
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

		// Token: 0x060002F3 RID: 755 RVA: 0x0000AAB0 File Offset: 0x00008CB0
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
						goto IL_165;
					}
					break;
				case 2:
				case 3:
					goto IL_165;
				case 4:
				{
					char c = v2.get_Chars(0);
					if (c <= 'T')
					{
						if (c != 'N')
						{
							if (c != 'T')
							{
								goto IL_165;
							}
							if (!(v2 == "True") && !(v2 == "TRUE"))
							{
								goto IL_165;
							}
						}
						else
						{
							if (!(v2 == "Null") && !(v2 == "NULL"))
							{
								goto IL_165;
							}
							break;
						}
					}
					else if (c != 'n')
					{
						if (c != 't')
						{
							goto IL_165;
						}
						if (!(v2 == "true"))
						{
							goto IL_165;
						}
					}
					else
					{
						if (!(v2 == "null"))
						{
							goto IL_165;
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
							goto IL_165;
						}
						if (!(v2 == "false"))
						{
							goto IL_165;
						}
					}
					else if (!(v2 == "False") && !(v2 == "FALSE"))
					{
						goto IL_165;
					}
					return false;
				}
				default:
					goto IL_165;
				}
				return null;
			}
			IL_165:
			object result;
			if (Regex.IsMatch(v, "^0x[0-9a-fA-F]+$"))
			{
				v = v.Substring(2);
				byte b;
				short num;
				int num2;
				long num3;
				ulong num4;
				if (byte.TryParse(v, 512, this.formatter.NumberFormat, ref b))
				{
					result = b;
				}
				else if (short.TryParse(v, 512, this.formatter.NumberFormat, ref num))
				{
					result = num;
				}
				else if (int.TryParse(v, 512, this.formatter.NumberFormat, ref num2))
				{
					result = num2;
				}
				else if (long.TryParse(v, 512, this.formatter.NumberFormat, ref num3))
				{
					result = num3;
				}
				else if (ulong.TryParse(v, 512, this.formatter.NumberFormat, ref num4))
				{
					result = num4;
				}
				else
				{
					result = v;
				}
			}
			else if (Regex.IsMatch(v, "^0o[0-9a-fA-F]+$"))
			{
				if (!ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToByte(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt16(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt32(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToInt64(v, 8), out result) && !ScalarNodeDeserializer.TryAndSwallow(() => Convert.ToUInt64(v, 8), out result))
				{
					result = v;
				}
			}
			else if (Regex.IsMatch(v, "^[-+]?(\\.[0-9]+|[0-9]+(\\.[0-9]*)?)([eE][-+]?[0-9]+)?$"))
			{
				byte b2;
				short num5;
				int num6;
				long num7;
				ulong num8;
				double num9;
				if (byte.TryParse(v, 7, this.formatter.NumberFormat, ref b2))
				{
					result = b2;
				}
				else if (short.TryParse(v, 7, this.formatter.NumberFormat, ref num5))
				{
					result = num5;
				}
				else if (int.TryParse(v, 7, this.formatter.NumberFormat, ref num6))
				{
					result = num6;
				}
				else if (long.TryParse(v, 7, this.formatter.NumberFormat, ref num7))
				{
					result = num7;
				}
				else if (ulong.TryParse(v, 7, this.formatter.NumberFormat, ref num8))
				{
					result = num8;
				}
				else if (double.TryParse(v, 167, this.formatter.NumberFormat, ref num9))
				{
					float num10 = (float)num9;
					if (!float.IsNaN(num10) && !float.IsInfinity(num10))
					{
						result = num10;
					}
					else
					{
						result = num9;
					}
				}
				else
				{
					result = v;
				}
			}
			else if (Regex.IsMatch(v, "^[-+]?(\\.inf|\\.Inf|\\.INF)$"))
			{
				if (v.StartsWith('-'))
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

		// Token: 0x060002F4 RID: 756 RVA: 0x0000AF5C File Offset: 0x0000915C
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

		// Token: 0x040000C3 RID: 195
		private const string BooleanTruePattern = "^(true|y|yes|on)$";

		// Token: 0x040000C4 RID: 196
		private const string BooleanFalsePattern = "^(false|n|no|off)$";

		// Token: 0x040000C5 RID: 197
		private readonly bool attemptUnknownTypeDeserialization;

		// Token: 0x040000C6 RID: 198
		private readonly ITypeConverter typeConverter;

		// Token: 0x040000C7 RID: 199
		private readonly ITypeInspector typeInspector;

		// Token: 0x040000C8 RID: 200
		private readonly YamlFormatter formatter;

		// Token: 0x040000C9 RID: 201
		private readonly INamingConvention enumNamingConvention;
	}
}
