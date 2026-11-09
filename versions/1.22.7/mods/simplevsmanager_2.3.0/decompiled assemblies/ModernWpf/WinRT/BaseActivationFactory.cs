using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001AF RID: 431
	internal class BaseActivationFactory
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0002A484 File Offset: 0x00028684
		public ObjectReference<IActivationFactoryVftbl> Value
		{
			get
			{
				return this._IActivationFactory;
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0002A48C File Offset: 0x0002868C
		public I AsInterface<I>()
		{
			return this._IActivationFactory.AsInterface<I>();
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0002A49C File Offset: 0x0002869C
		public BaseActivationFactory(string typeNamespace, string typeFullName)
		{
			ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> activationFactory = WinrtModule.GetActivationFactory(typeFullName);
			this._IActivationFactory = activationFactory.Item1;
			int item = activationFactory.Item2;
			if (this._IActivationFactory != null)
			{
				return;
			}
			string text = typeNamespace;
			for (;;)
			{
				DllModule dllModule = null;
				if (DllModule.TryLoad(text + ".dll", out dllModule))
				{
					activationFactory = dllModule.GetActivationFactory(typeFullName);
					this._IActivationFactory = activationFactory.Item1;
					if (this._IActivationFactory != null)
					{
						break;
					}
				}
				int num = text.LastIndexOf(".", 4);
				if (num <= 0)
				{
					Marshal.ThrowExceptionForHR(item);
				}
				text = text.Remove(num);
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0002A52C File Offset: 0x0002872C
		public unsafe ObjectReference<I> _ActivateInstance<I>()
		{
			method activateInstance = this._IActivationFactory.Vftbl.ActivateInstance;
			IntPtr intPtr;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this._IActivationFactory.ThisPtr, &intPtr, activateInstance));
			ObjectReference<I> objectReferenceForInterface;
			try
			{
				objectReferenceForInterface = ComWrappersSupport.GetObjectReferenceForInterface<I>(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return objectReferenceForInterface;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0002A588 File Offset: 0x00028788
		public ObjectReference<I> _As<I>()
		{
			return this._IActivationFactory.As<I>();
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0002A595 File Offset: 0x00028795
		public IObjectReference _As(Guid iid)
		{
			return this._IActivationFactory.As<IUnknownVftbl>(iid);
		}

		// Token: 0x040002AD RID: 685
		private readonly ObjectReference<IActivationFactoryVftbl> _IActivationFactory;
	}
}
