using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000104 RID: 260
	[NullableContext(1)]
	[Nullable(0)]
	public static class TypeConverter
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x000146CA File Offset: 0x000128CA
		[NullableContext(2)]
		[return: Nullable(1)]
		public static T ChangeType<T>(object value)
		{
			return (T)((object)TypeConverter.ChangeType(value, typeof(T)));
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000146E1 File Offset: 0x000128E1
		public static T ChangeType<[Nullable(2)] T>([Nullable(2)] object value, IFormatProvider provider)
		{
			return (T)((object)TypeConverter.ChangeType(value, typeof(T), provider));
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000146F9 File Offset: 0x000128F9
		public static T ChangeType<[Nullable(2)] T>([Nullable(2)] object value, CultureInfo culture)
		{
			return (T)((object)TypeConverter.ChangeType(value, typeof(T), culture));
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00014711 File Offset: 0x00012911
		[NullableContext(2)]
		public static object ChangeType(object value, [Nullable(1)] Type destinationType)
		{
			return TypeConverter.ChangeType(value, destinationType, CultureInfo.InvariantCulture);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001471F File Offset: 0x0001291F
		[return: Nullable(2)]
		public static object ChangeType([Nullable(2)] object value, Type destinationType, IFormatProvider provider)
		{
			return TypeConverter.ChangeType(value, destinationType, new CultureInfoAdapter(CultureInfo.CurrentCulture, provider));
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00014734 File Offset: 0x00012934
		[return: Nullable(2)]
		public static object ChangeType([Nullable(2)] object value, Type destinationType, CultureInfo culture)
		{
			if (value == null || value.IsDbNull())
			{
				if (!destinationType.IsValueType())
				{
					return null;
				}
				return Activator.CreateInstance(destinationType);
			}
			else
			{
				Type type = value.GetType();
				if (destinationType == type || destinationType.IsAssignableFrom(type))
				{
					return value;
				}
				if (destinationType.IsGenericType() && destinationType.GetGenericTypeDefinition() == typeof(Nullable))
				{
					Type destinationType2 = destinationType.GetGenericArguments()[0];
					object obj = TypeConverter.ChangeType(value, destinationType2, culture);
					return Activator.CreateInstance(destinationType, new object[]
					{
						obj
					});
				}
				if (destinationType.IsEnum())
				{
					string text = value as string;
					if (text == null)
					{
						return value;
					}
					return Enum.Parse(destinationType, text, true);
				}
				else
				{
					if (destinationType == typeof(bool))
					{
						if ("0".Equals(value))
						{
							return false;
						}
						if ("1".Equals(value))
						{
							return true;
						}
					}
					TypeConverter converter = TypeDescriptor.GetConverter(type);
					if (converter != null && converter.CanConvertTo(destinationType))
					{
						return converter.ConvertTo(null, culture, value, destinationType);
					}
					TypeConverter converter2 = TypeDescriptor.GetConverter(destinationType);
					if (converter2 != null && converter2.CanConvertFrom(type))
					{
						return converter2.ConvertFrom(null, culture, value);
					}
					Type[] array = new Type[]
					{
						type,
						destinationType
					};
					for (int i = 0; i < array.Length; i++)
					{
						foreach (MethodInfo methodInfo in array[i].GetPublicStaticMethods())
						{
							if (methodInfo.IsSpecialName && (methodInfo.Name == "op_Implicit" || methodInfo.Name == "op_Explicit") && destinationType.IsAssignableFrom(methodInfo.ReturnParameter.ParameterType))
							{
								ParameterInfo[] parameters = methodInfo.GetParameters();
								if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(type))
								{
									try
									{
										return methodInfo.Invoke(null, new object[]
										{
											value
										});
									}
									catch (TargetInvocationException ex)
									{
										throw ex.Unwrap();
									}
								}
							}
						}
					}
					if (type == typeof(string))
					{
						try
						{
							MethodInfo publicStaticMethod = destinationType.GetPublicStaticMethod("Parse", new Type[]
							{
								typeof(string),
								typeof(IFormatProvider)
							});
							if (publicStaticMethod != null)
							{
								return publicStaticMethod.Invoke(null, new object[]
								{
									value,
									culture
								});
							}
							publicStaticMethod = destinationType.GetPublicStaticMethod("Parse", new Type[]
							{
								typeof(string)
							});
							if (publicStaticMethod != null)
							{
								return publicStaticMethod.Invoke(null, new object[]
								{
									value
								});
							}
						}
						catch (TargetInvocationException ex2)
						{
							throw ex2.Unwrap();
						}
					}
					if (destinationType == typeof(TimeSpan))
					{
						return TimeSpan.Parse((string)TypeConverter.ChangeType(value, typeof(string), CultureInfo.InvariantCulture));
					}
					return Convert.ChangeType(value, destinationType, CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00014A5C File Offset: 0x00012C5C
		[NullableContext(0)]
		public static void RegisterTypeConverter<[Nullable(2)] TConvertible, TConverter>() where TConverter : TypeConverter
		{
			IEnumerable<TypeConverterAttribute> enumerable = Enumerable.OfType<TypeConverterAttribute>(TypeDescriptor.GetAttributes(typeof(TConvertible)));
			Func<TypeConverterAttribute, bool> func;
			if ((func = TypeConverter.<>c__6<TConvertible, TConverter>.<>9__6_0) == null)
			{
				func = (TypeConverter.<>c__6<TConvertible, TConverter>.<>9__6_0 = new Func<TypeConverterAttribute, bool>(TypeConverter.<>c__6<TConvertible, TConverter>.<>9.<RegisterTypeConverter>b__6_0));
			}
			if (!Enumerable.Any<TypeConverterAttribute>(enumerable, func))
			{
				TypeDescriptor.AddAttributes(typeof(TConvertible), new Attribute[]
				{
					new TypeConverterAttribute(typeof(TConverter))
				});
			}
		}
	}
}
