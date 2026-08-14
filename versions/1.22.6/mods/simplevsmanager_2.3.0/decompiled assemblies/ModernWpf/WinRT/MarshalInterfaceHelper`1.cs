using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x0200019A RID: 410
	internal class MarshalInterfaceHelper<T>
	{
		// Token: 0x06000D07 RID: 3335 RVA: 0x000261A4 File Offset: 0x000243A4
		private unsafe static MarshalInterfaceHelper<T>.MarshalerArray CreateMarshalerArray(T[] array, Func<T, IObjectReference> createMarshaler, Func<T, ObjectReferenceValue> createMarshaler2)
		{
			MarshalInterfaceHelper<T>.MarshalerArray marshalerArray = default(MarshalInterfaceHelper<T>.MarshalerArray);
			if (array == null)
			{
				return marshalerArray;
			}
			bool flag = false;
			MarshalInterfaceHelper<T>.MarshalerArray result;
			try
			{
				int num = array.Length;
				int num2 = num * IntPtr.Size;
				marshalerArray._array = Marshal.AllocCoTaskMem(num2);
				IntPtr* ptr = (IntPtr*)marshalerArray._array.ToPointer();
				if (createMarshaler2 != null)
				{
					marshalerArray._objectReferenceValues = new ObjectReferenceValue[num];
					for (int i = 0; i < num; i++)
					{
						marshalerArray._objectReferenceValues[i] = createMarshaler2.Invoke(array[i]);
						ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalInterfaceHelper<T>.GetAbi(marshalerArray._objectReferenceValues[i]);
					}
				}
				else
				{
					marshalerArray._marshalers = new IObjectReference[num];
					for (int j = 0; j < num; j++)
					{
						marshalerArray._marshalers[j] = createMarshaler.Invoke(array[j]);
						ptr[(IntPtr)j * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalInterfaceHelper<T>.GetAbi(marshalerArray._marshalers[j]);
					}
				}
				flag = true;
				result = marshalerArray;
			}
			finally
			{
				if (!flag)
				{
					marshalerArray.Dispose();
				}
			}
			return result;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000262B8 File Offset: 0x000244B8
		public static MarshalInterfaceHelper<T>.MarshalerArray CreateMarshalerArray(T[] array, Func<T, IObjectReference> createMarshaler)
		{
			return MarshalInterfaceHelper<T>.CreateMarshalerArray(array, createMarshaler, null);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x000262C2 File Offset: 0x000244C2
		public static MarshalInterfaceHelper<T>.MarshalerArray CreateMarshalerArray2(T[] array, Func<T, ObjectReferenceValue> createMarshaler)
		{
			return MarshalInterfaceHelper<T>.CreateMarshalerArray(array, null, createMarshaler);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x000262CC File Offset: 0x000244CC
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalInterfaceHelper<T>.MarshalerArray marshalerArray = (MarshalInterfaceHelper<T>.MarshalerArray)box;
			ObjectReferenceValue[] objectReferenceValues = marshalerArray._objectReferenceValues;
			int num;
			if (objectReferenceValues == null)
			{
				IObjectReference[] marshalers = marshalerArray._marshalers;
				num = ((marshalers != null) ? marshalers.Length : 0);
			}
			else
			{
				num = objectReferenceValues.Length;
			}
			return new ValueTuple<int, IntPtr>(num, marshalerArray._array);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00026308 File Offset: 0x00024508
		public unsafe static T[] FromAbiArray(object box, Func<IntPtr, T> fromAbi)
		{
			if (box == null)
			{
				return null;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			if (valueTuple.Item2 == IntPtr.Zero)
			{
				return null;
			}
			T[] array = new T[valueTuple.Item1];
			IntPtr* ptr = (IntPtr*)valueTuple.Item2.ToPointer();
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				array[i] = fromAbi.Invoke(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
			return array;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0002637C File Offset: 0x0002457C
		public unsafe static void CopyAbiArray(T[] array, object box, Func<IntPtr, T> fromAbi)
		{
			if (box == null)
			{
				return;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			IntPtr* ptr = (IntPtr*)valueTuple.Item2.ToPointer();
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				array[i] = fromAbi.Invoke(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000263CC File Offset: 0x000245CC
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public unsafe static ValueTuple<int, IntPtr> FromManagedArray(T[] array, Func<T, IntPtr> fromManaged)
		{
			if (array == null)
			{
				return new ValueTuple<int, IntPtr>(0, IntPtr.Zero);
			}
			IntPtr intPtr = IntPtr.Zero;
			int i = 0;
			bool flag = false;
			ValueTuple<int, IntPtr> result;
			try
			{
				int num = array.Length;
				intPtr = Marshal.AllocCoTaskMem(num * IntPtr.Size);
				IntPtr* ptr = (IntPtr*)intPtr.ToPointer();
				for (i = 0; i < num; i++)
				{
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = fromManaged.Invoke(array[i]);
				}
				flag = true;
				result = new ValueTuple<int, IntPtr>(i, intPtr);
			}
			finally
			{
				if (!flag)
				{
					MarshalInterfaceHelper<T>.DisposeAbiArray(new ValueTuple<int, IntPtr>(i, intPtr));
				}
			}
			return result;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00026468 File Offset: 0x00024668
		public unsafe static void CopyManagedArray(T[] array, IntPtr data, Action<T, IntPtr> copyManaged)
		{
			if (array == null)
			{
				return;
			}
			MarshalInterfaceHelper<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(array.Length, data));
			int i = 0;
			bool flag = false;
			try
			{
				int num = array.Length;
				int size = IntPtr.Size;
				byte* ptr = (byte*)data.ToPointer();
				for (i = 0; i < num; i++)
				{
					copyManaged.Invoke(array[i], (IntPtr)((void*)ptr));
					ptr += IntPtr.Size;
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					MarshalInterfaceHelper<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(i, data));
				}
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000264E8 File Offset: 0x000246E8
		public static void DisposeMarshalerArray(object box)
		{
			((MarshalInterfaceHelper<T>.MarshalerArray)box).Dispose();
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00026504 File Offset: 0x00024704
		public unsafe static void DisposeAbiArrayElements([TupleElementNames(new string[]
		{
			"length",
			"data"
		})] ValueTuple<int, IntPtr> abi)
		{
			IntPtr* ptr = (IntPtr*)abi.Item2.ToPointer();
			for (int i = 0; i < abi.Item1; i++)
			{
				MarshalInterfaceHelper<T>.DisposeAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00026540 File Offset: 0x00024740
		public static void DisposeAbiArray(object box)
		{
			if (box == null)
			{
				return;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			if (valueTuple.Item2 == IntPtr.Zero)
			{
				return;
			}
			MarshalInterfaceHelper<T>.DisposeAbiArrayElements(valueTuple);
			Marshal.FreeCoTaskMem(valueTuple.Item2);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0002657C File Offset: 0x0002477C
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0002658D File Offset: 0x0002478D
		public static IntPtr GetAbi(ObjectReferenceValue value)
		{
			return value.GetAbi();
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00026596 File Offset: 0x00024796
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x000265A1 File Offset: 0x000247A1
		public static void DisposeMarshaler(ObjectReferenceValue value)
		{
			value.Dispose();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000265AC File Offset: 0x000247AC
		public unsafe static void DisposeAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return;
			}
			method release = ((IntPtr*)((void*)ptr))->Release;
			object obj = calli(System.UInt32(System.IntPtr), ptr, release);
		}

		// Token: 0x020003E7 RID: 999
		public struct MarshalerArray
		{
			// Token: 0x06001AFF RID: 6911 RVA: 0x00049698 File Offset: 0x00047898
			public void Dispose()
			{
				if (this._marshalers != null)
				{
					IObjectReference[] marshalers = this._marshalers;
					for (int i = 0; i < marshalers.Length; i++)
					{
						MarshalInterfaceHelper<T>.DisposeMarshaler(marshalers[i]);
					}
				}
				if (this._objectReferenceValues != null)
				{
					foreach (ObjectReferenceValue objectReferenceValue in this._objectReferenceValues)
					{
						objectReferenceValue.Dispose();
					}
				}
				if (this._array != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(this._array);
				}
			}

			// Token: 0x040009B3 RID: 2483
			public IntPtr _array;

			// Token: 0x040009B4 RID: 2484
			public IObjectReference[] _marshalers;

			// Token: 0x040009B5 RID: 2485
			internal ObjectReferenceValue[] _objectReferenceValues;
		}
	}
}
