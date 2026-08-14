using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D6 RID: 214
	internal static class IMemoryBufferReferenceMethods
	{
		// Token: 0x060007C7 RID: 1991 RVA: 0x00013E4C File Offset: 0x0001204C
		public unsafe static uint get_Capacity(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00013E84 File Offset: 0x00012084
		private static ConditionalWeakTable<object, EventSource<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>> MakeClosedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<object, EventSource<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>>>(ref IMemoryBufferReferenceMethods._Closed_, new ConditionalWeakTable<object, EventSource<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>>(), null);
			return IMemoryBufferReferenceMethods._Closed_;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x00013E9E File Offset: 0x0001209E
		private static ConditionalWeakTable<object, EventSource<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>> _Closed
		{
			get
			{
				return IMemoryBufferReferenceMethods._Closed_ ?? IMemoryBufferReferenceMethods.MakeClosedTable();
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00013EB0 File Offset: 0x000120B0
		public unsafe static ValueTuple<Action<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>, Action<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>> Get_Closed(IObjectReference _obj, object _thisObj)
		{
			return IMemoryBufferReferenceMethods._Closed.GetValue(_thisObj, delegate([Nullable(1)] object key)
			{
				IntPtr thisPtr = _obj.ThisPtr;
				return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_Foundation_IMemoryBufferReference__object_(_obj, *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method)), *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method)), 0);
			}).EventActions;
		}

		// Token: 0x04000182 RID: 386
		private static volatile ConditionalWeakTable<object, EventSource<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>> _Closed_;
	}
}
