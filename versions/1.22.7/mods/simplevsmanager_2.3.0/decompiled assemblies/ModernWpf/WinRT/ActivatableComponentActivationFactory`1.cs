using System;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001B2 RID: 434
	internal class ActivatableComponentActivationFactory<T> : ComponentActivationFactory, IActivationFactory where T : class, new()
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002A61A File Offset: 0x0002881A
		public new IntPtr ActivateInstance()
		{
			return MarshalInspectable<T>.FromManaged(Activator.CreateInstance<T>(), true);
		}
	}
}
