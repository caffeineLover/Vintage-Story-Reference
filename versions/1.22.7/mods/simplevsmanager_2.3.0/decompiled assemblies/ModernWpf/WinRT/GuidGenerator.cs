using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace WinRT
{
	// Token: 0x02000191 RID: 401
	internal static class GuidGenerator
	{
		// Token: 0x06000CA9 RID: 3241 RVA: 0x00024277 File Offset: 0x00022477
		public static Guid GetGUID(Type type)
		{
			return type.GetGuidType().GUID;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00024284 File Offset: 0x00022484
		public static Guid GetIID(Type type)
		{
			type = type.GetGuidType();
			if (!type.IsGenericType)
			{
				return type.GUID;
			}
			return (Guid)type.GetField("PIID").GetValue(null);
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x000242B4 File Offset: 0x000224B4
		public static string GetSignature(Type type)
		{
			if (type == typeof(object))
			{
				return "cinterface(IInspectable)";
			}
			if (type == typeof(string))
			{
				return "string";
			}
			Type type2 = type.FindHelperType();
			if (type2 != null)
			{
				MethodInfo method = type2.GetMethod("GetGuidSignature", 24);
				if (method != null)
				{
					return (string)method.Invoke(null, null);
				}
			}
			if (type.IsValueType)
			{
				string name = type.Name;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
				if (num <= 1324880019U)
				{
					if (num <= 697196164U)
					{
						if (num != 423635464U)
						{
							if (num != 679076413U)
							{
								if (num == 697196164U)
								{
									if (name == "Int64")
									{
										return "i8";
									}
								}
							}
							else if (name == "Char")
							{
								return "c2";
							}
						}
						else if (name == "SByte")
						{
							return "i1";
						}
					}
					else if (num != 765439473U)
					{
						if (num != 1323747186U)
						{
							if (num == 1324880019U)
							{
								if (name == "UInt64")
								{
									return "u8";
								}
							}
						}
						else if (name == "UInt16")
						{
							return "u2";
						}
					}
					else if (name == "Int16")
					{
						return "i2";
					}
				}
				else if (num <= 2898774828U)
				{
					if (num != 2386971688U)
					{
						if (num != 2711245919U)
						{
							if (num == 2898774828U)
							{
								if (name == "Guid")
								{
									return "g16";
								}
							}
						}
						else if (name == "Int32")
						{
							return "i4";
						}
					}
					else if (name == "Double")
					{
						return "f8";
					}
				}
				else if (num <= 3538687084U)
				{
					if (num != 3409549631U)
					{
						if (num == 3538687084U)
						{
							if (name == "UInt32")
							{
								return "u4";
							}
						}
					}
					else if (name == "Byte")
					{
						return "u1";
					}
				}
				else if (num != 3969205087U)
				{
					if (num == 4051133705U)
					{
						if (name == "Single")
						{
							return "f4";
						}
					}
				}
				else if (name == "Boolean")
				{
					return "b1";
				}
				if (type.IsEnum)
				{
					bool flag = CustomAttributeExtensions.IsDefined(type, typeof(FlagsAttribute));
					return string.Concat(new string[]
					{
						"enum(",
						type.FullName,
						";",
						flag ? "u4" : "i4",
						")"
					});
				}
				if (!type.IsPrimitive)
				{
					IEnumerable<string> enumerable = Enumerable.Select<FieldInfo, string>(type.GetFields(20), (FieldInfo fi) => GuidGenerator.GetSignature(fi.FieldType));
					return string.Concat(new string[]
					{
						"struct(",
						type.FullName,
						";",
						string.Join(";", enumerable),
						")"
					});
				}
				throw new InvalidOperationException("unsupported value type");
			}
			else
			{
				type = (type.IsInterface ? (type.GetAuthoringMetadataType() ?? type) : type);
				if (type.IsGenericType)
				{
					IEnumerable<string> enumerable2 = Enumerable.Select<Type, string>(type.GetGenericArguments(), (Type t) => GuidGenerator.GetSignature(t));
					return string.Concat(new string[]
					{
						"pinterface({",
						GuidGenerator.GetGUID(type).ToString(),
						"};",
						string.Join(";", enumerable2),
						")"
					});
				}
				if (type.IsDelegate())
				{
					return "delegate({" + GuidGenerator.GetGUID(type).ToString() + "})";
				}
				Type type3;
				if (type.IsClass && Projections.TryGetDefaultInterfaceTypeForRuntimeClassType(type, out type3))
				{
					return string.Concat(new string[]
					{
						"rc(",
						type.FullName,
						";",
						GuidGenerator.GetSignature(type3),
						")"
					});
				}
				return "{" + type.GUID.ToString() + "}";
			}
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00024760 File Offset: 0x00022960
		private unsafe static Guid encode_guid(Span<byte> data)
		{
			if (BitConverter.IsLittleEndian)
			{
				byte b = *data[0];
				*data[0] = *data[3];
				*data[3] = b;
				b = *data[1];
				*data[1] = *data[2];
				*data[2] = b;
				b = *data[4];
				*data[4] = *data[5];
				*data[5] = b;
				b = *data[6];
				*data[6] = *data[7];
				*data[7] = ((b & 15) | 80);
				*data[8] = ((*data[8] & 63) | 128);
			}
			Span<byte> span = data;
			return new Guid(span.Slice(0, 16));
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00024848 File Offset: 0x00022A48
		public static Guid CreateIID(Type type)
		{
			string signature = GuidGenerator.GetSignature(type);
			if (!type.IsGenericType)
			{
				return new Guid(signature);
			}
			int maxByteCount = Encoding.UTF8.GetMaxByteCount(signature.Length);
			byte[] array = new byte[16 + maxByteCount];
			Span<byte> span = array;
			Guid result = GuidGenerator.wrt_pinterface_namespace;
			result.TryWriteBytes(span);
			Encoding utf = Encoding.UTF8;
			ReadOnlySpan<char> readOnlySpan = signature;
			Span<byte> span2 = span;
			int bytes = utf.GetBytes(readOnlySpan, span2.Slice(16, span2.Length - 16));
			array = RuntimeHelpers.GetSubArray<byte>(array, Range.EndAt(16 + bytes));
			using (SHA1 sha = new SHA1CryptoServiceProvider())
			{
				result = GuidGenerator.encode_guid(sha.ComputeHash(array));
			}
			return result;
		}

		// Token: 0x04000234 RID: 564
		private static readonly Guid wrt_pinterface_namespace = new Guid(3581604881U, 29563, 49218, 171, 174, 135, 139, 30, 22, 173, 238);
	}
}
