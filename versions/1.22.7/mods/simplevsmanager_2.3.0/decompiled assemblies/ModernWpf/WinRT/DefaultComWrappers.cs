using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.WinRT.Interop;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200018B RID: 395
	internal class DefaultComWrappers : ComWrappers
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00023768 File Offset: 0x00021968
		public unsafe static IUnknownVftbl IUnknownVftbl
		{
			get
			{
				return *Unsafe.AsRef<IUnknownVftbl>(DefaultComWrappers.IUnknownVftblPtr.ToPointer());
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0002378C File Offset: 0x0002198C
		internal static IntPtr IUnknownVftblPtr { get; }

		// Token: 0x06000C84 RID: 3204 RVA: 0x00023794 File Offset: 0x00021994
		unsafe static DefaultComWrappers()
		{
			IntPtr intPtr;
			IntPtr intPtr2;
			IntPtr intPtr3;
			ComWrappers.GetIUnknownImpl(ref intPtr, ref intPtr2, ref intPtr3);
			DefaultComWrappers.IUnknownVftblPtr = RuntimeHelpers.AllocateTypeAssociatedMemory(typeof(IUnknownVftbl), sizeof(IUnknownVftbl));
			*(IUnknownVftbl*)((void*)DefaultComWrappers.IUnknownVftblPtr) = new IUnknownVftbl
			{
				QueryInterface = (void*)intPtr,
				AddRef = (void*)intPtr2,
				Release = (void*)intPtr3
			};
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00023810 File Offset: 0x00021A10
		protected unsafe override ComWrappers.ComInterfaceEntry* ComputeVtables(object obj, CreateComInterfaceFlags flags, out int count)
		{
			DefaultComWrappers.VtableEntries value = DefaultComWrappers.TypeVtableEntryTable.GetValue(obj.GetType(), delegate([Nullable(1)] Type type)
			{
				if (DefaultComWrappers.IsRuntimeImplementedRCW(type))
				{
					return new DefaultComWrappers.VtableEntries();
				}
				return new DefaultComWrappers.VtableEntries(ComWrappersSupport.GetInterfaceTableEntries(type), type);
			});
			count = value.Count;
			if (count != 0 && !flags.HasFlag(1))
			{
				count--;
			}
			return value.Data;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00023878 File Offset: 0x00021A78
		private static bool IsRuntimeImplementedRCW(Type objType)
		{
			bool result = objType.IsCOMObject;
			if (objType.IsGenericType)
			{
				Type[] genericArguments = objType.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (genericArguments[i].IsCOMObject)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000238B8 File Offset: 0x00021AB8
		private static object CreateObject(IntPtr externalComObject)
		{
			Guid iinspectable_IID = InterfaceIIDs.IInspectable_IID;
			Guid iid = ABI.WinRT.Interop.IWeakReference.IID;
			IntPtr zero = IntPtr.Zero;
			object result;
			try
			{
				if (ComWrappersSupport.CreateRCWType != null && ComWrappersSupport.CreateRCWType.IsDelegate())
				{
					result = ComWrappersSupport.CreateDelegateFactory(ComWrappersSupport.CreateRCWType).Invoke(externalComObject);
				}
				else if (Marshal.QueryInterface(externalComObject, ref iinspectable_IID, ref zero) == 0)
				{
					ObjectReference<IInspectable.Vftbl> objectReferenceForInterface = ComWrappersSupport.GetObjectReferenceForInterface<IInspectable.Vftbl>(zero);
					ComWrappersHelper.Init(objectReferenceForInterface, true);
					IInspectable inspectable = new IInspectable(objectReferenceForInterface);
					if (ComWrappersSupport.CreateRCWType != null && ComWrappersSupport.CreateRCWType.IsSealed)
					{
						result = ComWrappersSupport.GetTypedRcwFactory(ComWrappersSupport.CreateRCWType).Invoke(inspectable);
					}
					else
					{
						Type runtimeClassForTypeCreation = ComWrappersSupport.GetRuntimeClassForTypeCreation(inspectable, ComWrappersSupport.CreateRCWType);
						if (runtimeClassForTypeCreation == null)
						{
							result = inspectable;
						}
						else
						{
							result = ComWrappersSupport.GetTypedRcwFactory(runtimeClassForTypeCreation).Invoke(inspectable);
						}
					}
				}
				else if (Marshal.QueryInterface(externalComObject, ref iid, ref zero) == 0)
				{
					ObjectReference<IUnknownVftbl> objectReferenceForInterface2 = ComWrappersSupport.GetObjectReferenceForInterface<IUnknownVftbl>(zero);
					ComWrappersHelper.Init(objectReferenceForInterface2, true);
					result = new SingleInterfaceOptimizedObject(typeof(WinRT.Interop.IWeakReference), objectReferenceForInterface2, false);
				}
				else
				{
					result = null;
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.Release(zero);
				}
			}
			return result;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x000239DC File Offset: 0x00021BDC
		protected override object CreateObject(IntPtr externalComObject, CreateObjectFlags flags)
		{
			object obj = DefaultComWrappers.CreateObject(externalComObject);
			IWinRTObject winRTObject = obj as IWinRTObject;
			if (winRTObject != null && winRTObject.HasUnwrappableNativeObject && winRTObject.NativeObject != null)
			{
				winRTObject.NativeObject.ReleaseFromTrackerSource();
				winRTObject.NativeObject.PreventReleaseFromTrackerSourceOnDispose = true;
			}
			return obj;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00023A20 File Offset: 0x00021C20
		protected override void ReleaseObjects(IEnumerable objects)
		{
			using (IEnumerator enumerator = objects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IObjectReference objectReference;
					if (!ComWrappersSupport.TryUnwrapObject(enumerator.Current, out objectReference))
					{
						throw new InvalidOperationException("Cannot release objects that are not runtime wrappers of native WinRT objects.");
					}
					objectReference.Dispose();
				}
			}
		}

		// Token: 0x0400021D RID: 541
		private static readonly ConditionalWeakTable<Type, DefaultComWrappers.VtableEntries> TypeVtableEntryTable = new ConditionalWeakTable<Type, DefaultComWrappers.VtableEntries>();

		// Token: 0x020003CA RID: 970
		private sealed class VtableEntries
		{
			// Token: 0x170004F7 RID: 1271
			// (get) Token: 0x06001AAB RID: 6827 RVA: 0x00048F49 File Offset: 0x00047149
			public unsafe ComWrappers.ComInterfaceEntry* Data { get; }

			// Token: 0x170004F8 RID: 1272
			// (get) Token: 0x06001AAC RID: 6828 RVA: 0x00048F51 File Offset: 0x00047151
			public int Count { get; }

			// Token: 0x06001AAD RID: 6829 RVA: 0x00048F59 File Offset: 0x00047159
			public VtableEntries()
			{
				this.Data = (UIntPtr)0;
				this.Count = 0;
			}

			// Token: 0x06001AAE RID: 6830 RVA: 0x00048F70 File Offset: 0x00047170
			public unsafe VtableEntries(List<ComWrappers.ComInterfaceEntry> entries, Type type)
			{
				this.Data = (void*)RuntimeHelpers.AllocateTypeAssociatedMemory(type, sizeof(ComWrappers.ComInterfaceEntry) * entries.Count);
				for (int i = 0; i < entries.Count; i++)
				{
					this.Data[i] = entries[i];
				}
				this.Count = entries.Count;
			}
		}
	}
}
