using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	[Nullable(0)]
	public static class TypeConverter
	{
		// Token: 0x06000225 RID: 549 RVA: 0x00007279 File Offset: 0x00005479
		public static T ChangeType<[Nullable(2)] T>([Nullable(2)] object value, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
		{
			return (T)((object)TypeConverter.ChangeType(value, typeof(T), enumNamingConvention, typeInspector));
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00007292 File Offset: 0x00005492
		[return: Nullable(2)]
		public static object ChangeType([Nullable(2)] object value, Type destinationType, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
		{
			return TypeConverter.ChangeType(value, destinationType, CultureInfo.InvariantCulture, enumNamingConvention, typeInspector);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000072A2 File Offset: 0x000054A2
		[return: Nullable(2)]
		public static object ChangeType([Nullable(2)] object value, Type destinationType, IFormatProvider provider, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
		{
			return TypeConverter.ChangeType(value, destinationType, new CultureInfoAdapter(CultureInfo.CurrentCulture, provider), enumNamingConvention, typeInspector);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000072BC File Offset: 0x000054BC
		[return: Nullable(2)]
		public static object ChangeType([Nullable(2)] object value, Type destinationType, CultureInfo culture, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
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
				if (destinationType.IsGenericType())
				{
					Type genericTypeDefinition = destinationType.GetGenericTypeDefinition();
					if (genericTypeDefinition == typeof(Nullable) || FsharpHelper.IsOptionType(genericTypeDefinition))
					{
						Type destinationType2 = destinationType.GetGenericArguments()[0];
						object obj = TypeConverter.ChangeType(value, destinationType2, culture, enumNamingConvention, typeInspector);
						return Activator.CreateInstance(destinationType, new object[]
						{
							obj
						});
					}
				}
				if (destinationType.IsEnum())
				{
					object result = value;
					string text = value as string;
					if (text != null)
					{
						text = enumNamingConvention.Reverse(text);
						text = typeInspector.GetEnumName(destinationType, text);
						result = Enum.Parse(destinationType, text, true);
					}
					return result;
				}
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
				foreach (Type type2 in new Type[]
				{
					type,
					destinationType
				})
				{
					foreach (MethodInfo methodInfo in type2.GetPublicStaticMethods())
					{
						bool flag = methodInfo.IsSpecialName && (methodInfo.Name == "op_Implicit" || methodInfo.Name == "op_Explicit") && destinationType.IsAssignableFrom(methodInfo.ReturnParameter.ParameterType);
						if (flag)
						{
							ParameterInfo[] parameters = methodInfo.GetParameters();
							bool flag2 = parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(type);
							if (flag2)
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
									throw ex.InnerException;
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
						throw ex2.InnerException;
					}
				}
				if (destinationType == typeof(TimeSpan))
				{
					return TimeSpan.Parse((string)TypeConverter.ChangeType(value, typeof(string), CultureInfo.InvariantCulture, enumNamingConvention, typeInspector), CultureInfo.InvariantCulture);
				}
				return Convert.ChangeType(value, destinationType, CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000762C File Offset: 0x0000582C
		[NullableContext(0)]
		public static void RegisterTypeConverter<[Nullable(2)] TConvertible, TConverter>() where TConverter : TypeConverter
		{
			IEnumerable<TypeConverterAttribute> enumerable = Enumerable.OfType<TypeConverterAttribute>(TypeDescriptor.GetAttributes(typeof(TConvertible)));
			Func<TypeConverterAttribute, bool> func;
			if ((func = TypeConverter.<>c__4<TConvertible, TConverter>.<>9__4_0) == null)
			{
				func = (TypeConverter.<>c__4<TConvertible, TConverter>.<>9__4_0 = new Func<TypeConverterAttribute, bool>(TypeConverter.<>c__4<TConvertible, TConverter>.<>9.<RegisterTypeConverter>b__4_0));
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
