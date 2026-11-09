using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x02000197 RID: 407
	internal struct MarshalBlittable<T>
	{
		// Token: 0x06000CE1 RID: 3297 RVA: 0x000254B3 File Offset: 0x000236B3
		public static MarshalBlittable<T>.MarshalerArray CreateMarshalerArray(Array array)
		{
			return new MarshalBlittable<T>.MarshalerArray(array);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000254BC File Offset: 0x000236BC
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalBlittable<T>.MarshalerArray marshalerArray = (MarshalBlittable<T>.MarshalerArray)box;
			if (!marshalerArray._gchandle.IsAllocated)
			{
				return new ValueTuple<int, IntPtr>(0, IntPtr.Zero);
			}
			return new ValueTuple<int, IntPtr>(((Array)marshalerArray._gchandle.Target).Length, marshalerArray._gchandle.AddrOfPinnedObject());
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00025514 File Offset: 0x00023714
		public static T[] FromAbiArray(object box)
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
			if (valueTuple.Item1 == 0)
			{
				return new T[0];
			}
			ReadOnlySpan<T> readOnlySpan = new ReadOnlySpan<T>(valueTuple.Item2.ToPointer(), valueTuple.Item1);
			return readOnlySpan.ToArray();
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00025570 File Offset: 0x00023770
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Array array)
		{
			if (array == null)
			{
				return new ValueTuple<int, IntPtr>(0, IntPtr.Zero);
			}
			int length = array.Length;
			IntPtr intPtr = Marshal.AllocCoTaskMem(length * Marshal.SizeOf<T>());
			MarshalBlittable<T>.CopyManagedArray(array, intPtr);
			return new ValueTuple<int, IntPtr>(length, intPtr);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x000255AC File Offset: 0x000237AC
		public static void CopyManagedArray(Array array, IntPtr data)
		{
			if (array == null)
			{
				return;
			}
			int num = array.Length * Marshal.SizeOf<T>();
			GCHandle gchandle = GCHandle.Alloc(array, 3);
			Buffer.MemoryCopy(gchandle.AddrOfPinnedObject().ToPointer(), data.ToPointer(), (long)num, (long)num);
			gchandle.Free();
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000255F8 File Offset: 0x000237F8
		public static void DisposeMarshalerArray(object box)
		{
			if (box != null)
			{
				((MarshalBlittable<T>.MarshalerArray)box).Dispose();
			}
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00025616 File Offset: 0x00023816
		public static void DisposeAbiArray(object box)
		{
			if (box == null)
			{
				return;
			}
			Marshal.FreeCoTaskMem(((ValueTuple<int, IntPtr>)box).Item2);
		}

		// Token: 0x020003D5 RID: 981
		public struct MarshalerArray
		{
			// Token: 0x06001ACC RID: 6860 RVA: 0x000492BC File Offset: 0x000474BC
			public MarshalerArray(Array array)
			{
				this._gchandle = ((array == null) ? default(GCHandle) : GCHandle.Alloc(array, 3));
			}

			// Token: 0x06001ACD RID: 6861 RVA: 0x000492E4 File Offset: 0x000474E4
			public void Dispose()
			{
				this._gchandle.Dispose();
			}

			// Token: 0x040009A0 RID: 2464
			public GCHandle _gchandle;
		}
	}
}
