using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;
using WinRT.Interop;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000167 RID: 359
	[DynamicInterfaceCastableImplementation]
	[Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
	internal interface IReadOnlyList<T> : IReadOnlyList<T>, IEnumerable<!0>, IEnumerable, IReadOnlyCollection<!0>, IVectorView<T>, IIterable<!0>
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x00020658 File Offset: 0x0001E858
		public static IObjectReference CreateMarshaler(IReadOnlyList<T> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IReadOnlyList<T>.Vftbl>(obj, IReadOnlyList<T>.PIID);
			}
			return null;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002066A File Offset: 0x0001E86A
		public static ObjectReferenceValue CreateMarshaler2(IReadOnlyList<T> obj)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(obj, IReadOnlyList<T>.PIID);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00020677 File Offset: 0x0001E877
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00020688 File Offset: 0x0001E888
		public static IntPtr FromManaged(IReadOnlyList<T> value)
		{
			if (value != null)
			{
				return IReadOnlyList<T>.CreateMarshaler2(value).Detach();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000206AC File Offset: 0x0001E8AC
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x000206B7 File Offset: 0x0001E8B7
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IVectorView<T>>.DisposeAbi(abi);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x000206BF File Offset: 0x0001E8BF
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IReadOnlyList<T>));
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x000206D0 File Offset: 0x0001E8D0
		public static ObjectReference<IReadOnlyList<T>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IReadOnlyList<T>.Vftbl vftbl = new IReadOnlyList<T>.Vftbl(thisPtr);
			return ObjectReference<IReadOnlyList<T>.Vftbl>.FromAbi(thisPtr, vftbl);
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x000206FB File Offset: 0x0001E8FB
		int IReadOnlyCollection<!0>.Count
		{
			get
			{
				return IReadOnlyListMethods<T>.get_Count((ObjectReference<IReadOnlyList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyList<!0>).TypeHandle));
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00020721 File Offset: 0x0001E921
		T IReadOnlyList<!0>.Item
		{
			get
			{
				return IReadOnlyListMethods<T>.Indexer_Get((ObjectReference<IReadOnlyList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IReadOnlyList<!0>).TypeHandle), index);
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00020748 File Offset: 0x0001E948
		IEnumerator<T> GetEnumerator()
		{
			((IWinRTObject)this).IsInterfaceImplemented(typeof(IEnumerable<!0>).TypeHandle, true);
			return IEnumerableMethods<T>.GetEnumerator((ObjectReference<IReadOnlyList<T>.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(IEnumerable<!0>).TypeHandle));
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00020795 File Offset: 0x0001E995
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002079D File Offset: 0x0001E99D
		static IReadOnlyList()
		{
			IReadOnlyList<T>.PIID = IReadOnlyList<T>.Vftbl.PIID;
		}

		// Token: 0x040001F4 RID: 500
		public static Guid PIID;

		// Token: 0x0200039D RID: 925
		public sealed class ToAbiHelper : IVectorView<T>, IIterable<!0>
		{
			// Token: 0x060019EA RID: 6634 RVA: 0x00047ACC File Offset: 0x00045CCC
			internal ToAbiHelper(IReadOnlyList<T> list)
			{
				this._list = list;
			}

			// Token: 0x060019EB RID: 6635 RVA: 0x00047ADB File Offset: 0x00045CDB
			IEnumerator<T> IIterable<!0>.First()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060019EC RID: 6636 RVA: 0x00047AE8 File Offset: 0x00045CE8
			private static void EnsureIndexInt32(uint index, int limit = 2147483647)
			{
				if (2147483647U <= index || index >= (uint)limit)
				{
					ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("index", WinRTRuntimeErrorStrings.ArgumentOutOfRange_IndexLargerThanMaxValue);
					ex.SetHResult(-2147483637);
					throw ex;
				}
			}

			// Token: 0x060019ED RID: 6637 RVA: 0x00047B14 File Offset: 0x00045D14
			public T GetAt(uint index)
			{
				IReadOnlyList<T>.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
				T result;
				try
				{
					result = this._list[(int)index];
				}
				catch (ArgumentOutOfRangeException ex)
				{
					ex.SetHResult(-2147483637);
					throw;
				}
				return result;
			}

			// Token: 0x170004DF RID: 1247
			// (get) Token: 0x060019EE RID: 6638 RVA: 0x00047B60 File Offset: 0x00045D60
			public uint Size
			{
				get
				{
					return (uint)this._list.Count;
				}
			}

			// Token: 0x060019EF RID: 6639 RVA: 0x00047B70 File Offset: 0x00045D70
			public bool IndexOf(T value, out uint index)
			{
				int num = -1;
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<T>.Default.Equals(value, this._list[i]))
					{
						num = i;
						break;
					}
				}
				if (-1 == num)
				{
					index = 0U;
					return false;
				}
				index = (uint)num;
				return true;
			}

			// Token: 0x060019F0 RID: 6640 RVA: 0x00047BC4 File Offset: 0x00045DC4
			public uint GetMany(uint startIndex, ref T[] items)
			{
				if ((ulong)startIndex == (ulong)((long)this._list.Count))
				{
					return 0U;
				}
				IReadOnlyList<T>.ToAbiHelper.EnsureIndexInt32(startIndex, this._list.Count);
				if (items == null)
				{
					return 0U;
				}
				uint num = Math.Min((uint)items.Length, (uint)(this._list.Count - (int)startIndex));
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					items[(int)num2] = this._list[(int)(num2 + startIndex)];
				}
				if (typeof(T) == typeof(string))
				{
					string[] array = items as string[];
					uint num3 = num;
					while ((ulong)num3 < (ulong)((long)items.Length))
					{
						array[(int)num3] = string.Empty;
						num3 += 1U;
					}
				}
				return num;
			}

			// Token: 0x0400092C RID: 2348
			private readonly IReadOnlyList<T> _list;
		}

		// Token: 0x0200039E RID: 926
		[Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
		public struct Vftbl
		{
			// Token: 0x170004E0 RID: 1248
			// (get) Token: 0x060019F1 RID: 6641 RVA: 0x00047C70 File Offset: 0x00045E70
			// (set) Token: 0x060019F2 RID: 6642 RVA: 0x00047C78 File Offset: 0x00045E78
			internal method GetSize_1
			{
				get
				{
					return this._get_Size_1;
				}
				set
				{
					this._get_Size_1 = value;
				}
			}

			// Token: 0x170004E1 RID: 1249
			// (get) Token: 0x060019F3 RID: 6643 RVA: 0x00047C81 File Offset: 0x00045E81
			// (set) Token: 0x060019F4 RID: 6644 RVA: 0x00047C89 File Offset: 0x00045E89
			public method GetMany_3
			{
				get
				{
					return this._getMany_3;
				}
				set
				{
					this._getMany_3 = value;
				}
			}

			// Token: 0x060019F5 RID: 6645 RVA: 0x00047C94 File Offset: 0x00045E94
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IReadOnlyList<T>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.GetAt_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IReadOnlyList<T>.Vftbl.GetAt_0_Type);
				this.GetSize_1 = (void*)ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
				this.IndexOf_2 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IReadOnlyList<T>.Vftbl.IndexOf_2_Type);
				this.GetMany_3 = (void*)ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x060019F6 RID: 6646 RVA: 0x00047D34 File Offset: 0x00045F34
			unsafe static Vftbl()
			{
				IReadOnlyList<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IReadOnlyList<T>));
				IReadOnlyList<T>.Vftbl.GetAt_0_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					typeof(uint),
					Marshaler<T>.AbiType.MakeByRefType(),
					typeof(int)
				});
				IReadOnlyList<T>.Vftbl.IndexOf_2_Type = Expression.GetDelegateType(new Type[]
				{
					typeof(void*),
					Marshaler<T>.AbiType,
					typeof(uint).MakeByRefType(),
					typeof(byte).MakeByRefType(),
					typeof(int)
				});
				IReadOnlyList<T>.Vftbl.DelegateCache = new Delegate[2];
				IReadOnlyList<T>.Vftbl._adapterTable = new ConditionalWeakTable<IReadOnlyList<T>, IReadOnlyList<T>.ToAbiHelper>();
				IReadOnlyList<T>.Vftbl.AbiToProjectionVftable = new IReadOnlyList<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					GetAt_0 = Delegate.CreateDelegate(IReadOnlyList<T>.Vftbl.GetAt_0_Type, typeof(IReadOnlyList<T>.Vftbl).GetMethod("Do_Abi_GetAt_0", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					_get_Size_1 = (void*)Marshal.GetFunctionPointerForDelegate(IReadOnlyList<T>.Vftbl.DelegateCache[0] = new _get_PropertyAsUInt32(IReadOnlyList<T>.Vftbl.Do_Abi_get_Size_1)),
					IndexOf_2 = Delegate.CreateDelegate(IReadOnlyList<T>.Vftbl.IndexOf_2_Type, typeof(IReadOnlyList<T>.Vftbl).GetMethod("Do_Abi_IndexOf_2", 40).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					_getMany_3 = (void*)Marshal.GetFunctionPointerForDelegate(IReadOnlyList<T>.Vftbl.DelegateCache[1] = new IReadOnlyList_Delegates.GetMany_3(IReadOnlyList<T>.Vftbl.Do_Abi_GetMany_3))
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.GetAt_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.GetSize_1;
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.IndexOf_2);
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.GetMany_3;
				IReadOnlyList<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060019F7 RID: 6647 RVA: 0x00047F88 File Offset: 0x00046188
			private static IReadOnlyList<T>.ToAbiHelper FindAdapter(IntPtr thisPtr)
			{
				IReadOnlyList<T> readOnlyList = ComWrappersSupport.FindObject<IReadOnlyList<T>>(thisPtr);
				ConditionalWeakTable<IReadOnlyList<T>, IReadOnlyList<T>.ToAbiHelper> adapterTable = IReadOnlyList<T>.Vftbl._adapterTable;
				IReadOnlyList<T> readOnlyList2 = readOnlyList;
				ConditionalWeakTable<IReadOnlyList<T>, IReadOnlyList<T>.ToAbiHelper>.CreateValueCallback createValueCallback;
				if ((createValueCallback = IReadOnlyList<T>.Vftbl.<>c.<>9__20_0) == null)
				{
					createValueCallback = (IReadOnlyList<T>.Vftbl.<>c.<>9__20_0 = new ConditionalWeakTable<IReadOnlyList<T>, IReadOnlyList<T>.ToAbiHelper>.CreateValueCallback(IReadOnlyList<T>.Vftbl.<>c.<>9.<FindAdapter>b__20_0));
				}
				return adapterTable.GetValue(readOnlyList2, createValueCallback);
			}

			// Token: 0x060019F8 RID: 6648 RVA: 0x00047FC8 File Offset: 0x000461C8
			private unsafe static int Do_Abi_GetAt_0<TAbi>(void* thisPtr, uint index, out TAbi __return_value__)
			{
				T t = default(T);
				__return_value__ = default(TAbi);
				try
				{
					t = IReadOnlyList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).GetAt(index);
					__return_value__ = (TAbi)((object)Marshaler<T>.FromManaged.Invoke(t));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019F9 RID: 6649 RVA: 0x00048030 File Offset: 0x00046230
			private unsafe static int Do_Abi_IndexOf_2<TAbi>(void* thisPtr, TAbi value, out uint index, out byte __return_value__)
			{
				index = 0U;
				__return_value__ = 0;
				uint num = 0U;
				try
				{
					bool flag = IReadOnlyList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).IndexOf(Marshaler<T>.FromAbi.Invoke(value), ref num);
					index = num;
					__return_value__ = (flag ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019FA RID: 6650 RVA: 0x0004809C File Offset: 0x0004629C
			private static int Do_Abi_GetMany_3(IntPtr thisPtr, uint startIndex, int __itemsSize, IntPtr items, out uint __return_value__)
			{
				__return_value__ = 0U;
				T[] array = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(__itemsSize, items));
				try
				{
					uint many = IReadOnlyList<T>.Vftbl.FindAdapter(thisPtr).GetMany(startIndex, ref array);
					Marshaler<T>.CopyManagedArray.Invoke(array, items);
					__return_value__ = many;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x060019FB RID: 6651 RVA: 0x00048108 File Offset: 0x00046308
			private static int Do_Abi_get_Size_1(IntPtr thisPtr, out uint __return_value__)
			{
				__return_value__ = 0U;
				try
				{
					uint size = IReadOnlyList<T>.Vftbl.FindAdapter(thisPtr).Size;
					__return_value__ = size;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400092D RID: 2349
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400092E RID: 2350
			public Delegate GetAt_0;

			// Token: 0x0400092F RID: 2351
			private unsafe void* _get_Size_1;

			// Token: 0x04000930 RID: 2352
			public Delegate IndexOf_2;

			// Token: 0x04000931 RID: 2353
			private unsafe void* _getMany_3;

			// Token: 0x04000932 RID: 2354
			public static Guid PIID;

			// Token: 0x04000933 RID: 2355
			private static readonly Type GetAt_0_Type;

			// Token: 0x04000934 RID: 2356
			private static readonly Type IndexOf_2_Type;

			// Token: 0x04000935 RID: 2357
			private static readonly IReadOnlyList<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x04000936 RID: 2358
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x04000937 RID: 2359
			private static readonly Delegate[] DelegateCache;

			// Token: 0x04000938 RID: 2360
			private static ConditionalWeakTable<IReadOnlyList<T>, IReadOnlyList<T>.ToAbiHelper> _adapterTable;
		}
	}
}
