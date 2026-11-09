using System;
using System.Collections.Generic;
using ABI.System;

namespace WinRT
{
	// Token: 0x0200019F RID: 415
	internal class Marshaler<T>
	{
		// Token: 0x06000D4A RID: 3402 RVA: 0x00026DBC File Offset: 0x00024FBC
		static Marshaler()
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle.IsArray)
			{
				throw new InvalidOperationException("Arrays may not be marshaled generically.");
			}
			if (typeFromHandle == typeof(string))
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_0);
				Marshaler<T>.CreateMarshaler2 = Marshaler<T>.CreateMarshaler;
				Marshaler<T>.GetAbi = new Func<object, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_1);
				Marshaler<T>.FromAbi = new Func<object, T>(Marshaler<T>.<>c.<>9.<.cctor>b__0_2);
				Marshaler<T>.FromManaged = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_3);
				Marshaler<T>.DisposeMarshaler = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_4);
				Marshaler<T>.DisposeAbi = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_5);
				Marshaler<T>.CreateMarshalerArray = new Func<T[], object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_6);
				Marshaler<T>.GetAbiArray = new Func<object, ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_7);
				Marshaler<T>.FromAbiArray = new Func<object, T[]>(Marshaler<T>.<>c.<>9.<.cctor>b__0_8);
				Marshaler<T>.FromManagedArray = new Func<T[], ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_9);
				Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_10);
				Marshaler<T>.DisposeMarshalerArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_11);
				Marshaler<T>.DisposeAbiArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_12);
			}
			else if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(KeyValuePair))
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = MarshalGeneric<T>.CreateMarshaler2;
				Marshaler<T>.CreateMarshaler2 = MarshalGeneric<T>.CreateMarshaler2;
				Marshaler<T>.GetAbi = MarshalGeneric<T>.GetAbi;
				Marshaler<T>.CopyAbi = MarshalGeneric<T>.CopyAbi;
				Marshaler<T>.FromAbi = MarshalGeneric<T>.FromAbi;
				Marshaler<T>.FromManaged = MarshalGeneric<T>.FromManaged;
				Marshaler<T>.CopyManaged = MarshalGeneric<T>.CopyManaged;
				Marshaler<T>.DisposeMarshaler = MarshalGeneric<T>.DisposeMarshaler;
				Marshaler<T>.DisposeAbi = MarshalGeneric<T>.DisposeAbi;
				Marshaler<T>.CreateMarshalerArray = MarshalGeneric<T>.CreateMarshalerArray;
				Marshaler<T>.GetAbiArray = MarshalGeneric<T>.GetAbiArray;
				Marshaler<T>.FromAbiArray = MarshalGeneric<T>.FromAbiArray;
				Marshaler<T>.FromManagedArray = MarshalGeneric<T>.FromManagedArray;
				Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_13);
				Marshaler<T>.DisposeMarshalerArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_14);
				Marshaler<T>.DisposeAbiArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_15);
			}
			else if (typeFromHandle == typeof(Type))
			{
				Marshaler<T>.AbiType = typeof(Type);
				Marshaler<T>.CreateMarshaler = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_16);
				Marshaler<T>.CreateMarshaler2 = Marshaler<T>.CreateMarshaler;
				Marshaler<T>.GetAbi = new Func<object, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_17);
				Marshaler<T>.FromAbi = new Func<object, T>(Marshaler<T>.<>c.<>9.<.cctor>b__0_18);
				Marshaler<T>.CopyAbi = new Action<object, IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_19);
				Marshaler<T>.CopyManaged = new Action<T, IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_20);
				Marshaler<T>.FromManaged = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_21);
				Marshaler<T>.DisposeMarshaler = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_22);
				Marshaler<T>.DisposeAbi = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_23);
				Marshaler<T>.CreateMarshalerArray = new Func<T[], object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_24);
				Marshaler<T>.GetAbiArray = new Func<object, ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_25);
				Marshaler<T>.FromAbiArray = new Func<object, T[]>(Marshaler<T>.<>c.<>9.<.cctor>b__0_26);
				Marshaler<T>.FromManagedArray = new Func<T[], ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_27);
				Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_28);
				Marshaler<T>.DisposeMarshalerArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_29);
				Marshaler<T>.DisposeAbiArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_30);
			}
			else if (typeFromHandle.IsValueType)
			{
				Marshaler<T>.AbiType = typeFromHandle.FindHelperType();
				if (Marshaler<T>.AbiType != null && Marshaler<T>.AbiType.GetMethod("FromAbi", 56) == null)
				{
					Marshaler<T>.AbiType = null;
				}
				if (Marshaler<T>.AbiType == null)
				{
					Func<T, object> func = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_64);
					Marshaler<T>.AbiType = typeFromHandle;
					Marshaler<T>.CreateMarshaler = func;
					Marshaler<T>.CreateMarshaler2 = Marshaler<T>.CreateMarshaler;
					Marshaler<T>.GetAbi = Marshaler.ReturnParameterFunc;
					Marshaler<T>.FromAbi = new Func<object, T>(Marshaler<T>.<>c.<>9.<.cctor>b__0_65);
					Marshaler<T>.FromManaged = func;
					Marshaler<T>.DisposeMarshaler = Marshaler.EmptyFunc;
					Marshaler<T>.DisposeAbi = Marshaler.EmptyFunc;
					if (typeFromHandle.IsEnum)
					{
						if (typeFromHandle.GetEnumUnderlyingType() == typeof(int))
						{
							Marshaler<T>.CopyAbi = Marshaler.CopyIntEnumFunc;
							Marshaler<T>.CopyManaged = new Action<T, IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_66);
						}
						else
						{
							Marshaler<T>.CopyAbi = Marshaler.CopyUIntEnumFunc;
							Marshaler<T>.CopyManaged = new Action<T, IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_67);
						}
					}
					Marshaler<T>.CreateMarshalerArray = new Func<T[], object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_68);
					Marshaler<T>.GetAbiArray = new Func<object, ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_69);
					Marshaler<T>.FromAbiArray = new Func<object, T[]>(Marshaler<T>.<>c.<>9.<.cctor>b__0_70);
					Marshaler<T>.FromManagedArray = new Func<T[], ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_71);
					Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_72);
					Marshaler<T>.DisposeMarshalerArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_73);
					Marshaler<T>.DisposeAbiArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_74);
				}
				else
				{
					Marshaler<T>.CreateMarshaler = MarshalGeneric<T>.CreateMarshaler;
					Marshaler<T>.CreateMarshaler2 = Marshaler<T>.CreateMarshaler;
					Marshaler<T>.GetAbi = MarshalGeneric<T>.GetAbi;
					Marshaler<T>.CopyAbi = MarshalGeneric<T>.CopyAbi;
					Marshaler<T>.FromAbi = MarshalGeneric<T>.FromAbi;
					Marshaler<T>.FromManaged = MarshalGeneric<T>.FromManaged;
					Marshaler<T>.CopyManaged = MarshalGeneric<T>.CopyManaged;
					Marshaler<T>.DisposeMarshaler = MarshalGeneric<T>.DisposeMarshaler;
					Marshaler<T>.DisposeAbi = MarshalGeneric<T>.DisposeAbi;
					Marshaler<T>.CreateMarshalerArray = new Func<T[], object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_31);
					Marshaler<T>.GetAbiArray = new Func<object, ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_32);
					Marshaler<T>.FromAbiArray = new Func<object, T[]>(Marshaler<T>.<>c.<>9.<.cctor>b__0_33);
					Marshaler<T>.FromManagedArray = new Func<T[], ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_34);
					Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_35);
					Marshaler<T>.DisposeMarshalerArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_36);
					Marshaler<T>.DisposeAbiArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_37);
				}
			}
			else if (typeFromHandle.IsInterface)
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_38);
				Marshaler<T>.CreateMarshaler2 = Marshaler<T>.CreateMarshaler;
				Marshaler<T>.GetAbi = new Func<object, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_39);
				Marshaler<T>.FromAbi = new Func<object, T>(Marshaler<T>.<>c.<>9.<.cctor>b__0_40);
				Marshaler<T>.FromManaged = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_41);
				Marshaler<T>.DisposeMarshaler = new Action<object>(MarshalInterface<T>.DisposeMarshaler);
				Marshaler<T>.DisposeAbi = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_42);
				Marshaler<T>.CreateMarshalerArray = new Func<T[], object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_43);
				Marshaler<T>.GetAbiArray = new Func<object, ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_44);
				Marshaler<T>.FromAbiArray = new Func<object, T[]>(Marshaler<T>.<>c.<>9.<.cctor>b__0_45);
				Marshaler<T>.FromManagedArray = new Func<T[], ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_46);
				Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_47);
				Marshaler<T>.DisposeMarshalerArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_48);
				Marshaler<T>.DisposeAbiArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_49);
			}
			else if (typeof(T) == typeof(object))
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_50);
				Marshaler<T>.CreateMarshaler2 = Marshaler<T>.CreateMarshaler;
				Marshaler<T>.GetAbi = new Func<object, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_51);
				Marshaler<T>.FromAbi = new Func<object, T>(Marshaler<T>.<>c.<>9.<.cctor>b__0_52);
				Marshaler<T>.FromManaged = new Func<T, object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_53);
				Marshaler<T>.CopyManaged = new Action<T, IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_54);
				Marshaler<T>.DisposeMarshaler = new Action<object>(MarshalInspectable<T>.DisposeMarshaler);
				Marshaler<T>.DisposeAbi = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_55);
				Marshaler<T>.CreateMarshalerArray = new Func<T[], object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_56);
				Marshaler<T>.GetAbiArray = new Func<object, ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_57);
				Marshaler<T>.FromAbiArray = new Func<object, T[]>(Marshaler<T>.<>c.<>9.<.cctor>b__0_58);
				Marshaler<T>.FromManagedArray = new Func<T[], ValueTuple<int, IntPtr>>(Marshaler<T>.<>c.<>9.<.cctor>b__0_59);
				Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_60);
				Marshaler<T>.DisposeMarshalerArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_61);
				Marshaler<T>.DisposeAbiArray = new Action<object>(Marshaler<T>.<>c.<>9.<.cctor>b__0_62);
			}
			else
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = (typeof(T).IsDelegate() ? MarshalGeneric<T>.CreateMarshaler : MarshalGeneric<T>.CreateMarshaler2);
				Marshaler<T>.CreateMarshaler2 = MarshalGeneric<T>.CreateMarshaler2;
				Marshaler<T>.GetAbi = MarshalGeneric<T>.GetAbi;
				Marshaler<T>.FromAbi = MarshalGeneric<T>.FromAbi;
				Marshaler<T>.FromManaged = MarshalGeneric<T>.FromManaged;
				Marshaler<T>.CopyManaged = MarshalGeneric<T>.CopyManaged;
				Marshaler<T>.DisposeMarshaler = MarshalGeneric<T>.DisposeMarshaler;
				Marshaler<T>.DisposeAbi = MarshalGeneric<T>.DisposeAbi;
				Marshaler<T>.CreateMarshalerArray = MarshalGeneric<T>.CreateMarshalerArray;
				Marshaler<T>.GetAbiArray = MarshalGeneric<T>.GetAbiArray;
				Marshaler<T>.FromAbiArray = MarshalGeneric<T>.FromAbiArray;
				Marshaler<T>.FromManagedArray = MarshalGeneric<T>.FromManagedArray;
				Marshaler<T>.CopyManagedArray = new Action<T[], IntPtr>(Marshaler<T>.<>c.<>9.<.cctor>b__0_63);
				Marshaler<T>.DisposeMarshalerArray = MarshalGeneric<T>.DisposeMarshalerArray;
				Marshaler<T>.DisposeAbiArray = MarshalGeneric<T>.DisposeAbiArray;
			}
			Marshaler<T>.RefAbiType = Marshaler<T>.AbiType.MakeByRefType();
		}

		// Token: 0x04000269 RID: 617
		public static readonly Type AbiType;

		// Token: 0x0400026A RID: 618
		public static readonly Type RefAbiType;

		// Token: 0x0400026B RID: 619
		public static readonly Func<T, object> CreateMarshaler;

		// Token: 0x0400026C RID: 620
		internal static readonly Func<T, object> CreateMarshaler2;

		// Token: 0x0400026D RID: 621
		public static readonly Func<object, object> GetAbi;

		// Token: 0x0400026E RID: 622
		public static readonly Action<object, IntPtr> CopyAbi;

		// Token: 0x0400026F RID: 623
		public static readonly Func<object, T> FromAbi;

		// Token: 0x04000270 RID: 624
		public static readonly Func<T, object> FromManaged;

		// Token: 0x04000271 RID: 625
		public static readonly Action<T, IntPtr> CopyManaged;

		// Token: 0x04000272 RID: 626
		public static readonly Action<object> DisposeMarshaler;

		// Token: 0x04000273 RID: 627
		public static readonly Action<object> DisposeAbi;

		// Token: 0x04000274 RID: 628
		public static readonly Func<T[], object> CreateMarshalerArray;

		// Token: 0x04000275 RID: 629
		public static readonly Func<object, ValueTuple<int, IntPtr>> GetAbiArray;

		// Token: 0x04000276 RID: 630
		public static readonly Func<object, T[]> FromAbiArray;

		// Token: 0x04000277 RID: 631
		public static readonly Func<T[], ValueTuple<int, IntPtr>> FromManagedArray;

		// Token: 0x04000278 RID: 632
		public static readonly Action<T[], IntPtr> CopyManagedArray;

		// Token: 0x04000279 RID: 633
		public static readonly Action<object> DisposeMarshalerArray;

		// Token: 0x0400027A RID: 634
		public static readonly Action<object> DisposeAbiArray;
	}
}
