using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x02000196 RID: 406
	internal class MarshalString
	{
		// Token: 0x06000CCA RID: 3274 RVA: 0x0002500A File Offset: 0x0002320A
		public static MarshalString.Pinnable CreatePinnable(string value)
		{
			return new MarshalString.Pinnable(value);
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00025012 File Offset: 0x00023212
		public static IntPtr GetAbi(ref MarshalString.Pinnable p)
		{
			return p.GetAbi();
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002501A File Offset: 0x0002321A
		public MarshalString(string value)
		{
			this._gchandle = GCHandle.Alloc(value, 3);
			this._header = IntPtr.Zero;
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0002503A File Offset: 0x0002323A
		public void Dispose()
		{
			this._gchandle.Dispose();
			this._gchandle = default(GCHandle);
			Marshal.FreeHGlobal(this._header);
			this._header = IntPtr.Zero;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00025069 File Offset: 0x00023269
		public static MarshalString CreateMarshaler(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return new MarshalString(value);
			}
			return null;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0002507C File Offset: 0x0002327C
		public unsafe IntPtr GetAbi()
		{
			string text = (string)this._gchandle.Target;
			char* sourceString;
			if (text == null)
			{
				sourceString = null;
			}
			else
			{
				fixed (char* pinnableReference = text.GetPinnableReference())
				{
					sourceString = pinnableReference;
				}
			}
			this._header = Marshal.AllocHGlobal(Unsafe.SizeOf<MarshalString.HSTRING_HEADER>());
			IntPtr result;
			Marshal.ThrowExceptionForHR(Platform.WindowsCreateStringReference(sourceString, text.Length, (IntPtr*)((void*)this._header), &result));
			return result;
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x000250D9 File Offset: 0x000232D9
		public static IntPtr GetAbi(MarshalString m)
		{
			if (m != null)
			{
				return m.GetAbi();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000250EA File Offset: 0x000232EA
		public static IntPtr GetAbi(object box)
		{
			if (box != null)
			{
				return MarshalString.GetAbi((MarshalString)box);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00025100 File Offset: 0x00023300
		public static void DisposeMarshaler(MarshalString m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002510B File Offset: 0x0002330B
		public static void DisposeMarshaler(object box)
		{
			if (box != null)
			{
				MarshalString.DisposeMarshaler((MarshalString)box);
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002511B File Offset: 0x0002331B
		public static void DisposeAbi(IntPtr hstring)
		{
			if (hstring != IntPtr.Zero)
			{
				Platform.WindowsDeleteString(hstring);
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00025131 File Offset: 0x00023331
		public static void DisposeAbi(object abi)
		{
			if (abi != null)
			{
				MarshalString.DisposeAbi((IntPtr)abi);
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00025144 File Offset: 0x00023344
		public unsafe static string FromAbi(IntPtr value)
		{
			if (value == IntPtr.Zero)
			{
				return "";
			}
			uint num;
			return new string(Platform.WindowsGetStringRawBuffer(value, &num), 0, (int)num);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00025174 File Offset: 0x00023374
		public unsafe static IntPtr FromManaged(string value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			IntPtr result;
			Marshal.ThrowExceptionForHR(Platform.WindowsCreateString(value, value.Length, &result));
			return result;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x000251A0 File Offset: 0x000233A0
		public unsafe static MarshalString.MarshalerArray CreateMarshalerArray(string[] array)
		{
			MarshalString.MarshalerArray marshalerArray = default(MarshalString.MarshalerArray);
			if (array == null)
			{
				return marshalerArray;
			}
			bool flag = false;
			MarshalString.MarshalerArray result;
			try
			{
				int num = array.Length;
				marshalerArray._array = Marshal.AllocCoTaskMem(num * Marshal.SizeOf<IntPtr>());
				marshalerArray._marshalers = new MarshalString[num];
				IntPtr* ptr = (IntPtr*)marshalerArray._array.ToPointer();
				for (int i = 0; i < num; i++)
				{
					marshalerArray._marshalers[i] = MarshalString.CreateMarshaler(array[i]);
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalString.GetAbi(marshalerArray._marshalers[i]);
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

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00025250 File Offset: 0x00023450
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalString.MarshalerArray marshalerArray = (MarshalString.MarshalerArray)box;
			MarshalString[] marshalers = marshalerArray._marshalers;
			return new ValueTuple<int, IntPtr>((marshalers != null) ? marshalers.Length : 0, marshalerArray._array);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00025280 File Offset: 0x00023480
		public unsafe static string[] FromAbiArray(object box)
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
			string[] array = new string[valueTuple.Item1];
			IntPtr* ptr = (IntPtr*)valueTuple.Item2.ToPointer();
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				array[i] = MarshalString.FromAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
			return array;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x000252EC File Offset: 0x000234EC
		public unsafe static void CopyAbiArray(string[] array, object box)
		{
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			IntPtr* ptr = (IntPtr*)valueTuple.Item2.ToPointer();
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				array[i] = MarshalString.FromAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00025334 File Offset: 0x00023534
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public unsafe static ValueTuple<int, IntPtr> FromManagedArray(string[] array)
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
				intPtr = Marshal.AllocCoTaskMem(num * Marshal.SizeOf<IntPtr>());
				IntPtr* ptr = (IntPtr*)((void*)intPtr);
				for (i = 0; i < num; i++)
				{
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalString.FromManaged(array[i]);
				}
				flag = true;
				result = new ValueTuple<int, IntPtr>(i, intPtr);
			}
			finally
			{
				if (!flag)
				{
					MarshalString.DisposeAbiArray(new ValueTuple<int, IntPtr>(i, intPtr));
				}
			}
			return result;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x000253C8 File Offset: 0x000235C8
		public unsafe static void CopyManagedArray(string[] array, IntPtr data)
		{
			if (array == null)
			{
				return;
			}
			MarshalString.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(array.Length, data));
			int i = 0;
			bool flag = false;
			try
			{
				int num = array.Length;
				IntPtr* ptr = (IntPtr*)((void*)data);
				for (i = 0; i < num; i++)
				{
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalString.FromManaged(array[i]);
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					MarshalString.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(i, data));
				}
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0002543C File Offset: 0x0002363C
		public static void DisposeMarshalerArray(object box)
		{
			if (box != null)
			{
				((MarshalString.MarshalerArray)box).Dispose();
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0002545C File Offset: 0x0002365C
		public unsafe static void DisposeAbiArrayElements([TupleElementNames(new string[]
		{
			"length",
			"data"
		})] ValueTuple<int, IntPtr> abi)
		{
			IntPtr* ptr = (IntPtr*)((void*)abi.Item2);
			for (int i = 0; i < abi.Item1; i++)
			{
				MarshalString.DisposeAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00025497 File Offset: 0x00023697
		public static void DisposeAbiArray(object box)
		{
			if (box == null)
			{
				return;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			MarshalString.DisposeAbiArrayElements(valueTuple);
			Marshal.FreeCoTaskMem(valueTuple.Item2);
		}

		// Token: 0x0400023C RID: 572
		private IntPtr _header;

		// Token: 0x0400023D RID: 573
		private GCHandle _gchandle;

		// Token: 0x020003D2 RID: 978
		internal struct HSTRING_HEADER
		{
			// Token: 0x04000997 RID: 2455
			private IntPtr reserved1;

			// Token: 0x04000998 RID: 2456
			private int reserved2;

			// Token: 0x04000999 RID: 2457
			private int reserved3;

			// Token: 0x0400099A RID: 2458
			private int reserved4;

			// Token: 0x0400099B RID: 2459
			private int reserved5;
		}

		// Token: 0x020003D3 RID: 979
		public ref struct Pinnable
		{
			// Token: 0x06001AC8 RID: 6856 RVA: 0x000491DA File Offset: 0x000473DA
			public Pinnable(string value)
			{
				this._value = (value ?? "");
				this._header = default(MarshalString.HSTRING_HEADER);
			}

			// Token: 0x06001AC9 RID: 6857 RVA: 0x000491F8 File Offset: 0x000473F8
			public ref readonly char GetPinnableReference()
			{
				return this._value.GetPinnableReference();
			}

			// Token: 0x06001ACA RID: 6858 RVA: 0x00049208 File Offset: 0x00047408
			public unsafe IntPtr GetAbi()
			{
				if (this._value == "")
				{
					return IntPtr.Zero;
				}
				IntPtr result;
				Marshal.ThrowExceptionForHR(Platform.WindowsCreateStringReference((char*)Unsafe.AsPointer<char>(Unsafe.AsRef<char>(this.GetPinnableReference())), this._value.Length, (IntPtr*)Unsafe.AsPointer<MarshalString.HSTRING_HEADER>(ref this._header), &result));
				return result;
			}

			// Token: 0x0400099C RID: 2460
			private MarshalString.HSTRING_HEADER _header;

			// Token: 0x0400099D RID: 2461
			private string _value;
		}

		// Token: 0x020003D4 RID: 980
		public struct MarshalerArray
		{
			// Token: 0x06001ACB RID: 6859 RVA: 0x00049264 File Offset: 0x00047464
			public void Dispose()
			{
				if (this._marshalers != null)
				{
					foreach (MarshalString marshalString in this._marshalers)
					{
						if (marshalString != null)
						{
							marshalString.Dispose();
						}
					}
				}
				if (this._array != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(this._array);
				}
			}

			// Token: 0x0400099E RID: 2462
			public IntPtr _array;

			// Token: 0x0400099F RID: 2463
			public MarshalString[] _marshalers;
		}
	}
}
