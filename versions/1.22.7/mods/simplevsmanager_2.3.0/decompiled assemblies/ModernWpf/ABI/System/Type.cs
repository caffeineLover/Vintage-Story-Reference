using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000139 RID: 313
	internal struct Type
	{
		// Token: 0x06000A46 RID: 2630 RVA: 0x0001CCC0 File Offset: 0x0001AEC0
		[return: TupleElementNames(new string[]
		{
			"Name",
			"Kind"
		})]
		private static ValueTuple<string, TypeKind> ToAbi(Type value)
		{
			TypeKind typeKind = TypeKind.Custom;
			if (value != null)
			{
				if (value.IsPrimitive)
				{
					typeKind = TypeKind.Primitive;
				}
				else if (value == typeof(object) || value == typeof(string) || value == typeof(Guid) || value == typeof(Type))
				{
					typeKind = TypeKind.Metadata;
				}
				else if (Projections.IsTypeWindowsRuntimeType(value))
				{
					typeKind = TypeKind.Metadata;
				}
			}
			return new ValueTuple<string, TypeKind>(Type.GetNameForTypeCached(value, typeKind == TypeKind.Custom), typeKind);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001CD44 File Offset: 0x0001AF44
		private static string GetNameForTypeCached(Type value, bool customKind)
		{
			if (customKind)
			{
				return Type.typeNameCache.GetOrAdd(value, (Type type) => type.AssemblyQualifiedName);
			}
			return Type.typeNameCache.GetOrAdd(value, (Type type) => TypeNameSupport.GetNameForType(type, TypeNameGenerationFlags.None));
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001CDAC File Offset: 0x0001AFAC
		public static Type.Marshaler CreateMarshaler(Type value)
		{
			ValueTuple<string, TypeKind> valueTuple = Type.ToAbi(value);
			return new Type.Marshaler
			{
				Name = MarshalString.CreateMarshaler(valueTuple.Item1),
				Kind = valueTuple.Item2
			};
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001CDE8 File Offset: 0x0001AFE8
		public static Type GetAbi(ref Type.Pinnable p)
		{
			return new Type
			{
				Name = MarshalString.GetAbi(ref p.Name),
				Kind = p.Kind
			};
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0001CE20 File Offset: 0x0001B020
		public static Type GetAbi(Type.Marshaler m)
		{
			return new Type
			{
				Name = MarshalString.GetAbi(m.Name),
				Kind = m.Kind
			};
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0001CE58 File Offset: 0x0001B058
		public static Type FromAbi(Type value)
		{
			string text = MarshalString.FromAbi(value.Name);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (value.Kind == TypeKind.Custom)
			{
				return Type.GetType(text);
			}
			return TypeNameSupport.FindTypeByNameCached(text);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001CE91 File Offset: 0x0001B091
		public unsafe static void CopyAbi(Type.Marshaler arg, IntPtr dest)
		{
			*(Type*)dest.ToPointer() = Type.GetAbi(arg);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0001CEA8 File Offset: 0x0001B0A8
		public static Type FromManaged(Type value)
		{
			ValueTuple<string, TypeKind> valueTuple = Type.ToAbi(value);
			return new Type
			{
				Name = MarshalString.FromManaged(valueTuple.Item1),
				Kind = valueTuple.Item2
			};
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
		public unsafe static void CopyManaged(Type arg, IntPtr dest)
		{
			*(Type*)dest.ToPointer() = Type.FromManaged(arg);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0001CEF8 File Offset: 0x0001B0F8
		public static void DisposeMarshaler(Type.Marshaler m)
		{
			m.Dispose();
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0001CF01 File Offset: 0x0001B101
		public static void DisposeAbi(Type abi)
		{
			MarshalString.DisposeAbi(abi.Name);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0001CF0E File Offset: 0x0001B10E
		public static string GetGuidSignature()
		{
			return "struct(Windows.UI.Xaml.Interop.TypeName;string;enum(Windows.UI.Xaml.Interop.TypeKind;i4))";
		}

		// Token: 0x040001E1 RID: 481
		private IntPtr Name;

		// Token: 0x040001E2 RID: 482
		private TypeKind Kind;

		// Token: 0x040001E3 RID: 483
		private static readonly ConcurrentDictionary<Type, string> typeNameCache = new ConcurrentDictionary<Type, string>();

		// Token: 0x0200034E RID: 846
		public struct Marshaler
		{
			// Token: 0x06001821 RID: 6177 RVA: 0x00043938 File Offset: 0x00041B38
			internal void Dispose()
			{
				MarshalString.DisposeMarshaler(this.Name);
			}

			// Token: 0x04000881 RID: 2177
			internal MarshalString Name;

			// Token: 0x04000882 RID: 2178
			internal TypeKind Kind;
		}

		// Token: 0x0200034F RID: 847
		public ref struct Pinnable
		{
			// Token: 0x06001822 RID: 6178 RVA: 0x00043948 File Offset: 0x00041B48
			public Pinnable(Type type)
			{
				ValueTuple<string, TypeKind> valueTuple = Type.ToAbi(type);
				this.Name = new MarshalString.Pinnable(valueTuple.Item1);
				this.Kind = valueTuple.Item2;
			}

			// Token: 0x06001823 RID: 6179 RVA: 0x00043979 File Offset: 0x00041B79
			public ref readonly char GetPinnableReference()
			{
				return this.Name.GetPinnableReference();
			}

			// Token: 0x04000883 RID: 2179
			internal MarshalString.Pinnable Name;

			// Token: 0x04000884 RID: 2180
			internal TypeKind Kind;
		}
	}
}
