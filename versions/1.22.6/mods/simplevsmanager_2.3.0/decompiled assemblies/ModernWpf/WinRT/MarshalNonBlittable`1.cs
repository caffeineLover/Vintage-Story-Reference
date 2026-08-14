using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x02000199 RID: 409
	internal class MarshalNonBlittable<T> : MarshalGeneric<T>
	{
		// Token: 0x06000CFC RID: 3324 RVA: 0x00025D80 File Offset: 0x00023F80
		public new unsafe static MarshalNonBlittable<T>.MarshalerArray CreateMarshalerArray(T[] array)
		{
			MarshalNonBlittable<T>.MarshalerArray marshalerArray = default(MarshalNonBlittable<T>.MarshalerArray);
			if (array == null)
			{
				return marshalerArray;
			}
			bool flag = false;
			MarshalNonBlittable<T>.MarshalerArray result;
			try
			{
				int num = array.Length;
				int num2 = Marshal.SizeOf(MarshalNonBlittable<T>.AbiType);
				int num3 = num * num2;
				marshalerArray._array = Marshal.AllocCoTaskMem(num3);
				marshalerArray._marshalers = new object[num];
				byte* ptr = (byte*)marshalerArray._array.ToPointer();
				for (int i = 0; i < num; i++)
				{
					marshalerArray._marshalers[i] = Marshaler<T>.CreateMarshaler.Invoke(array[i]);
					Marshaler<T>.CopyAbi.Invoke(marshalerArray._marshalers[i], (IntPtr)((void*)ptr));
					ptr += num2;
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

		// Token: 0x06000CFD RID: 3325 RVA: 0x00025E48 File Offset: 0x00024048
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public new static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalNonBlittable<T>.MarshalerArray marshalerArray = (MarshalNonBlittable<T>.MarshalerArray)box;
			object[] marshalers = marshalerArray._marshalers;
			return new ValueTuple<int, IntPtr>((marshalers != null) ? marshalers.Length : 0, marshalerArray._array);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00025E78 File Offset: 0x00024078
		public new unsafe static T[] FromAbiArray(object box)
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
			byte* ptr = (byte*)valueTuple.Item2.ToPointer();
			int num = Marshal.SizeOf(MarshalNonBlittable<T>.AbiType);
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				object obj = Marshal.PtrToStructure((IntPtr)((void*)ptr), MarshalNonBlittable<T>.AbiType);
				array[i] = Marshaler<T>.FromAbi.Invoke(obj);
				ptr += num;
			}
			return array;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00025F0C File Offset: 0x0002410C
		public unsafe static void CopyAbiArray(T[] array, object box)
		{
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			if (valueTuple.Item2 == IntPtr.Zero)
			{
				return;
			}
			byte* ptr = (byte*)valueTuple.Item2.ToPointer();
			int num = Marshal.SizeOf(MarshalNonBlittable<T>.AbiType);
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				object obj = Marshal.PtrToStructure((IntPtr)((void*)ptr), MarshalNonBlittable<T>.AbiType);
				array[i] = Marshaler<T>.FromAbi.Invoke(obj);
				ptr += num;
			}
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00025F88 File Offset: 0x00024188
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public new unsafe static ValueTuple<int, IntPtr> FromManagedArray(T[] array)
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
				int num2 = Marshal.SizeOf(MarshalNonBlittable<T>.AbiType);
				intPtr = Marshal.AllocCoTaskMem(num * num2);
				byte* ptr = (byte*)intPtr.ToPointer();
				for (i = 0; i < num; i++)
				{
					Marshaler<T>.CopyManaged.Invoke(array[i], (IntPtr)((void*)ptr));
					ptr += num2;
				}
				flag = true;
				result = new ValueTuple<int, IntPtr>(i, intPtr);
			}
			finally
			{
				if (!flag)
				{
					MarshalNonBlittable<T>.DisposeAbiArray(new ValueTuple<int, IntPtr>(i, intPtr));
				}
			}
			return result;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00026030 File Offset: 0x00024230
		public new unsafe static void CopyManagedArray(T[] array, IntPtr data)
		{
			if (array == null)
			{
				return;
			}
			MarshalNonBlittable<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(array.Length, data));
			int i = 0;
			bool flag = false;
			try
			{
				int num = array.Length;
				int num2 = Marshal.SizeOf(MarshalNonBlittable<T>.AbiType);
				byte* ptr = (byte*)data.ToPointer();
				for (i = 0; i < num; i++)
				{
					Marshaler<T>.CopyManaged.Invoke(array[i], (IntPtr)((void*)ptr));
					ptr += num2;
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					MarshalNonBlittable<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(i, data));
				}
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000260BC File Offset: 0x000242BC
		public new static void DisposeMarshalerArray(object box)
		{
			((MarshalNonBlittable<T>.MarshalerArray)box).Dispose();
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000260D8 File Offset: 0x000242D8
		public unsafe static void DisposeAbiArrayElements([TupleElementNames(new string[]
		{
			"length",
			"data"
		})] ValueTuple<int, IntPtr> abi)
		{
			byte* ptr = (byte*)abi.Item2.ToPointer();
			int num = Marshal.SizeOf(MarshalNonBlittable<T>.AbiType);
			for (int i = 0; i < abi.Item1; i++)
			{
				object obj = Marshal.PtrToStructure((IntPtr)((void*)ptr), MarshalNonBlittable<T>.AbiType);
				Marshaler<T>.DisposeAbi.Invoke(obj);
				ptr += num;
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00026130 File Offset: 0x00024330
		public new static void DisposeAbiArray(object box)
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
			MarshalNonBlittable<T>.DisposeAbiArrayElements(valueTuple);
			Marshal.FreeCoTaskMem(valueTuple.Item2);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00026174 File Offset: 0x00024374
		// Note: this type is marked as 'beforefieldinit'.
		static MarshalNonBlittable()
		{
			MarshalNonBlittable<T>.AbiType = (typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : MarshalGeneric<T>.AbiType);
		}

		// Token: 0x0400025F RID: 607
		private new static readonly Type AbiType;

		// Token: 0x020003E6 RID: 998
		public struct MarshalerArray
		{
			// Token: 0x06001AFE RID: 6910 RVA: 0x00049640 File Offset: 0x00047840
			public void Dispose()
			{
				if (this._marshalers != null)
				{
					foreach (object obj in this._marshalers)
					{
						Marshaler<T>.DisposeMarshaler.Invoke(obj);
					}
				}
				if (this._array != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(this._array);
				}
			}

			// Token: 0x040009B1 RID: 2481
			public IntPtr _array;

			// Token: 0x040009B2 RID: 2482
			public object[] _marshalers;
		}
	}
}
