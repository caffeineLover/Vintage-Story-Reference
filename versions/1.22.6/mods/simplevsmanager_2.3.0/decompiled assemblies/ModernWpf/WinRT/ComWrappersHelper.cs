using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200018A RID: 394
	internal class ComWrappersHelper
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x00023650 File Offset: 0x00021850
		public static void Init(bool isAggregation, object thisInstance, IntPtr newInstance, IntPtr inner, out IObjectReference objRef)
		{
			objRef = ComWrappersSupport.GetObjectReferenceForInterface(isAggregation ? inner : newInstance);
			Guid iid = IReferenceTrackerVftbl.IID;
			IntPtr intPtr;
			if (Marshal.QueryInterface(objRef.ThisPtr, ref iid, ref intPtr) != 0)
			{
				intPtr = default(IntPtr);
			}
			CreateObjectFlags createObjectFlags = 0;
			IntPtr thisPtr = newInstance;
			if (intPtr != (IntPtr)0)
			{
				createObjectFlags |= 1;
			}
			if (isAggregation)
			{
				createObjectFlags |= 4;
				if (intPtr != (IntPtr)0)
				{
					Marshal.Release(intPtr);
					thisPtr = inner;
				}
			}
			ComWrappersSupport.RegisterObjectForInterface(thisInstance, thisPtr, createObjectFlags);
			if (isAggregation)
			{
				objRef.IsAggregated = true;
				objRef.PreventReleaseOnDispose = (intPtr != (IntPtr)0);
				return;
			}
			if (intPtr != (IntPtr)0)
			{
				objRef.ReferenceTrackerPtr = intPtr;
				objRef.PreventReleaseFromTrackerSourceOnDispose = true;
				Marshal.Release(intPtr);
			}
			Marshal.Release(newInstance);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00023708 File Offset: 0x00021908
		public static void Init(IObjectReference objRef, bool addRefFromTrackerSource = true)
		{
			if (objRef.ReferenceTrackerPtr == IntPtr.Zero)
			{
				Guid iid = IReferenceTrackerVftbl.IID;
				IntPtr intPtr;
				if (Marshal.QueryInterface(objRef.ThisPtr, ref iid, ref intPtr) == 0)
				{
					objRef.ReferenceTrackerPtr = intPtr;
					if (addRefFromTrackerSource)
					{
						objRef.AddRefFromTrackerSource();
					}
					else
					{
						objRef.PreventReleaseFromTrackerSourceOnDispose = true;
					}
					Marshal.Release(intPtr);
				}
			}
		}
	}
}
