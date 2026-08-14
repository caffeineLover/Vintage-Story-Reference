using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001AE RID: 430
	internal sealed class WinrtModule
	{
		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002A335 File Offset: 0x00028535
		private static WinrtModule MakeWinRTModule()
		{
			Interlocked.CompareExchange<WinrtModule>(ref WinrtModule._instance, new WinrtModule(), null);
			return WinrtModule._instance;
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x0002A34F File Offset: 0x0002854F
		public static WinrtModule Instance
		{
			get
			{
				return WinrtModule._instance ?? WinrtModule.MakeWinRTModule();
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0002A364 File Offset: 0x00028564
		public unsafe WinrtModule()
		{
			IntPtr mtaCookie;
			Marshal.ThrowExceptionForHR(Platform.CoIncrementMTAUsage(&mtaCookie));
			this._mtaCookie = mtaCookie;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0002A38C File Offset: 0x0002858C
		[return: TupleElementNames(new string[]
		{
			"instancePtr",
			"hr"
		})]
		public unsafe static ValueTuple<IntPtr, int> GetActivationFactory(IntPtr hstrRuntimeClassId)
		{
			WinrtModule instance = WinrtModule.Instance;
			Guid iid = IActivationFactoryVftbl.IID;
			IntPtr intPtr;
			int num = Platform.RoGetActivationFactory(hstrRuntimeClassId, ref iid, &intPtr);
			return new ValueTuple<IntPtr, int>((num == 0) ? intPtr : IntPtr.Zero, num);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0002A3C4 File Offset: 0x000285C4
		[return: TupleElementNames(new string[]
		{
			"obj",
			"hr"
		})]
		public unsafe static ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> GetActivationFactory(string runtimeClassId)
		{
			IntPtr intPtr = IntPtr.Zero;
			ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(runtimeClassId);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						ValueTuple<IntPtr, int> activationFactory = WinrtModule.GetActivationFactory(MarshalString.GetAbi(ref pinnable));
						intPtr = activationFactory.Item1;
						int item = activationFactory.Item2;
						if (item == 0)
						{
							result = new ValueTuple<ObjectReference<IActivationFactoryVftbl>, int>(ComWrappersSupport.GetObjectReferenceForInterface<IActivationFactoryVftbl>(intPtr), item);
						}
						else
						{
							result = new ValueTuple<ObjectReference<IActivationFactoryVftbl>, int>(null, item);
						}
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0002A44C File Offset: 0x0002864C
		~WinrtModule()
		{
			Marshal.ThrowExceptionForHR(Platform.CoDecrementMTAUsage(this._mtaCookie));
		}

		// Token: 0x040002AB RID: 683
		private readonly IntPtr _mtaCookie;

		// Token: 0x040002AC RID: 684
		private static volatile WinrtModule _instance;
	}
}
