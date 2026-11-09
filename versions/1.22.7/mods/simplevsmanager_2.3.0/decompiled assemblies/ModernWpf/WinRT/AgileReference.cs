using System;
using System.Runtime.InteropServices;
using ABI.WinRT.Interop;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x02000183 RID: 387
	internal class AgileReference : IDisposable
	{
		// Token: 0x06000C31 RID: 3121 RVA: 0x000218C0 File Offset: 0x0001FAC0
		public AgileReference(IObjectReference instance) : this((instance != null) ? instance.ThisPtr : IntPtr.Zero)
		{
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000218D8 File Offset: 0x0001FAD8
		internal AgileReference(in ObjectReferenceValue instance) : this(instance.GetAbi())
		{
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000218E8 File Offset: 0x0001FAE8
		internal unsafe AgileReference(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return;
			}
			IntPtr intPtr = default(IntPtr);
			Guid iid = IUnknownVftbl.IID;
			try
			{
				Marshal.ThrowExceptionForHR(Platform.RoGetAgileReference(0U, ref iid, thisPtr, &intPtr));
				this._agileReference = ObjectReference<IUnknownVftbl>.Attach(ref intPtr);
			}
			catch (TypeLoadException)
			{
				this._cookie = AgileReference.Git.Value.RegisterInterfaceInGlobal(thisPtr, iid);
			}
			finally
			{
				MarshalInterface<WinRT.Interop.IAgileReference>.DisposeAbi(intPtr);
			}
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00021974 File Offset: 0x0001FB74
		public IObjectReference Get()
		{
			if (this._cookie == IntPtr.Zero)
			{
				return IAgileReferenceMethods.Resolve(this._agileReference, IUnknownVftbl.IID);
			}
			WinRT.Interop.IGlobalInterfaceTable value = AgileReference.Git.Value;
			if (value == null)
			{
				return null;
			}
			return value.GetInterfaceFromGlobal(this._cookie, IUnknownVftbl.IID);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000219C4 File Offset: 0x0001FBC4
		internal ObjectReference<T> Get<T>(Guid iid)
		{
			if (this._cookie == IntPtr.Zero)
			{
				return IAgileReferenceMethods.Resolve<T>(this._agileReference, iid);
			}
			WinRT.Interop.IGlobalInterfaceTable value = AgileReference.Git.Value;
			if (value == null)
			{
				return null;
			}
			IObjectReference interfaceFromGlobal = value.GetInterfaceFromGlobal(this._cookie, IUnknownVftbl.IID);
			if (interfaceFromGlobal == null)
			{
				return null;
			}
			return interfaceFromGlobal.As<T>(iid);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00021A1C File Offset: 0x0001FC1C
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (this._cookie != IntPtr.Zero)
				{
					try
					{
						AgileReference.Git.Value.RevokeInterfaceFromGlobal(this._cookie);
					}
					catch (ArgumentException)
					{
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00021A74 File Offset: 0x0001FC74
		private unsafe static WinRT.Interop.IGlobalInterfaceTable GetGitTable()
		{
			Guid clsid_StdGlobalInterfaceTable = AgileReference.CLSID_StdGlobalInterfaceTable;
			Guid iid = ABI.WinRT.Interop.IGlobalInterfaceTable.IID;
			IntPtr intPtr = default(IntPtr);
			WinRT.Interop.IGlobalInterfaceTable result;
			try
			{
				Marshal.ThrowExceptionForHR(Platform.CoCreateInstance(ref clsid_StdGlobalInterfaceTable, IntPtr.Zero, 1U, ref iid, &intPtr));
				result = ABI.WinRT.Interop.IGlobalInterfaceTable.FromAbi(intPtr).AsType<ABI.WinRT.Interop.IGlobalInterfaceTable>();
			}
			finally
			{
				MarshalInterface<WinRT.Interop.IGlobalInterfaceTable>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00021AD4 File Offset: 0x0001FCD4
		~AgileReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00021B04 File Offset: 0x0001FD04
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400020A RID: 522
		private static readonly Guid CLSID_StdGlobalInterfaceTable = new Guid(803, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70);

		// Token: 0x0400020B RID: 523
		private static readonly Lazy<WinRT.Interop.IGlobalInterfaceTable> Git = new Lazy<WinRT.Interop.IGlobalInterfaceTable>(() => AgileReference.GetGitTable());

		// Token: 0x0400020C RID: 524
		private readonly IObjectReference _agileReference;

		// Token: 0x0400020D RID: 525
		private readonly IntPtr _cookie;

		// Token: 0x0400020E RID: 526
		private bool disposed;
	}
}
