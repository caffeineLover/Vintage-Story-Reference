using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200019C RID: 412
	internal static class MarshalInspectable<T>
	{
		// Token: 0x06000D2E RID: 3374 RVA: 0x00026964 File Offset: 0x00024B64
		public static IObjectReference CreateMarshaler<V>(T o, Guid iid, bool unwrapObject = true)
		{
			if (o == null)
			{
				return null;
			}
			IObjectReference objectReference;
			if (unwrapObject && ComWrappersSupport.TryUnwrapObject(o, out objectReference))
			{
				return objectReference.As<V>(iid);
			}
			Type type = Projections.FindCustomHelperTypeMapping(o.GetType(), true);
			if (type != null)
			{
				return (IObjectReference)type.GetMethod("CreateMarshaler", 56).Invoke(null, new object[]
				{
					o
				});
			}
			return ComWrappersSupport.CreateCCWForObject<V>(o, iid);
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x000269E6 File Offset: 0x00024BE6
		public static IObjectReference CreateMarshaler(T o, bool unwrapObject = true)
		{
			return MarshalInspectable<T>.CreateMarshaler<IInspectable.Vftbl>(o, InterfaceIIDs.IInspectable_IID, unwrapObject);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000269F4 File Offset: 0x00024BF4
		public static ObjectReferenceValue CreateMarshaler2(T o, Guid iid, bool unwrapObject = true)
		{
			if (o == null)
			{
				return default(ObjectReferenceValue);
			}
			IObjectReference objectReference;
			if (unwrapObject && ComWrappersSupport.TryUnwrapObject(o, out objectReference))
			{
				return objectReference.AsValue(iid);
			}
			Type type = Projections.FindCustomHelperTypeMapping(o.GetType(), true);
			if (type != null)
			{
				return (ObjectReferenceValue)type.GetMethod("CreateMarshaler2", 56).Invoke(null, new object[]
				{
					o
				});
			}
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(o, iid);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00026A7E File Offset: 0x00024C7E
		public static ObjectReferenceValue CreateMarshaler2(T o, bool unwrapObject = true)
		{
			return MarshalInspectable<T>.CreateMarshaler2(o, InterfaceIIDs.IInspectable_IID, unwrapObject);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00026A8C File Offset: 0x00024C8C
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef != null)
			{
				return MarshalInterfaceHelper<T>.GetAbi(objRef);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00026A9D File Offset: 0x00024C9D
		public static IntPtr GetAbi(ObjectReferenceValue value)
		{
			return value.GetAbi();
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00026AA8 File Offset: 0x00024CA8
		public static T FromAbi(IntPtr ptr)
		{
			T result;
			if (ptr == IntPtr.Zero)
			{
				result = default(T);
				return result;
			}
			IntPtr zero = IntPtr.Zero;
			try
			{
				Guid iid = IUnknownVftbl.IID;
				Marshal.QueryInterface(ptr, ref iid, ref zero);
				if (IUnknownVftbl.IsReferenceToManagedObject(zero))
				{
					result = (T)((object)ComWrappersSupport.FindObject<object>(zero));
				}
				else
				{
					result = ComWrappersSupport.CreateRcwForComObject<T>(ptr);
				}
			}
			finally
			{
				MarshalInspectable<T>.DisposeAbi(zero);
			}
			return result;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00026B1C File Offset: 0x00024D1C
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			MarshalInterfaceHelper<T>.DisposeMarshaler(objRef);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00026B24 File Offset: 0x00024D24
		public static void DisposeMarshaler(ObjectReferenceValue value)
		{
			value.Dispose();
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00026B30 File Offset: 0x00024D30
		internal static void DisposeMarshaler(object value)
		{
			if (value is ObjectReferenceValue)
			{
				ObjectReferenceValue objectReferenceValue = (ObjectReferenceValue)value;
				MarshalInspectable<T>.DisposeMarshaler(objectReferenceValue);
				return;
			}
			MarshalInspectable<T>.DisposeMarshaler((IObjectReference)value);
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00026B5E File Offset: 0x00024D5E
		public static void DisposeAbi(IntPtr ptr)
		{
			MarshalInterfaceHelper<T>.DisposeAbi(ptr);
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00026B68 File Offset: 0x00024D68
		public static IntPtr FromManaged(T o, bool unwrapObject = true)
		{
			return MarshalInspectable<T>.CreateMarshaler2(o, unwrapObject).Detach();
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00026B84 File Offset: 0x00024D84
		public unsafe static void CopyManaged(T o, IntPtr dest, bool unwrapObject = true)
		{
			*(IntPtr*)dest.ToPointer() = MarshalInspectable<T>.CreateMarshaler2(o, unwrapObject).Detach();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00026BA8 File Offset: 0x00024DA8
		public static MarshalInterfaceHelper<T>.MarshalerArray CreateMarshalerArray(T[] array)
		{
			Func<T, ObjectReferenceValue> func;
			if ((func = MarshalInspectable<T>.<>c.<>9__13_0) == null)
			{
				func = (MarshalInspectable<T>.<>c.<>9__13_0 = new Func<T, ObjectReferenceValue>(MarshalInspectable<T>.<>c.<>9.<CreateMarshalerArray>b__13_0));
			}
			return MarshalInterfaceHelper<T>.CreateMarshalerArray2(array, func);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00026BCF File Offset: 0x00024DCF
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(T box)
		{
			return MarshalInterfaceHelper<T>.GetAbiArray(box);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00026BDC File Offset: 0x00024DDC
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		internal static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<T>.GetAbiArray(box);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00026BE4 File Offset: 0x00024DE4
		public static T[] FromAbiArray(T box)
		{
			return MarshalInterfaceHelper<T>.FromAbiArray(box, new Func<IntPtr, T>(MarshalInspectable<T>.FromAbi));
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00026BFD File Offset: 0x00024DFD
		internal static T[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<T>.FromAbiArray(box, new Func<IntPtr, T>(MarshalInspectable<T>.FromAbi));
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00026C11 File Offset: 0x00024E11
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(T[] array)
		{
			Func<T, IntPtr> func;
			if ((func = MarshalInspectable<T>.<>c.<>9__18_0) == null)
			{
				func = (MarshalInspectable<T>.<>c.<>9__18_0 = new Func<T, IntPtr>(MarshalInspectable<T>.<>c.<>9.<FromManagedArray>b__18_0));
			}
			return MarshalInterfaceHelper<T>.FromManagedArray(array, func);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00026C38 File Offset: 0x00024E38
		public static void CopyManagedArray(T[] array, IntPtr data)
		{
			Action<T, IntPtr> action;
			if ((action = MarshalInspectable<T>.<>c.<>9__19_0) == null)
			{
				action = (MarshalInspectable<T>.<>c.<>9__19_0 = new Action<T, IntPtr>(MarshalInspectable<T>.<>c.<>9.<CopyManagedArray>b__19_0));
			}
			MarshalInterfaceHelper<T>.CopyManagedArray(array, data, action);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00026C60 File Offset: 0x00024E60
		public static void DisposeMarshalerArray(T box)
		{
			MarshalInterfaceHelper<T>.DisposeMarshalerArray(box);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00026C6D File Offset: 0x00024E6D
		public static void DisposeAbiArray(T box)
		{
			MarshalInterfaceHelper<T>.DisposeAbiArray(box);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00026C7A File Offset: 0x00024E7A
		internal static void DisposeMarshalerArray(object box)
		{
			MarshalInterfaceHelper<T>.DisposeMarshalerArray(box);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00026C82 File Offset: 0x00024E82
		internal static void DisposeAbiArray(object box)
		{
			MarshalInterfaceHelper<T>.DisposeAbiArray(box);
		}
	}
}
