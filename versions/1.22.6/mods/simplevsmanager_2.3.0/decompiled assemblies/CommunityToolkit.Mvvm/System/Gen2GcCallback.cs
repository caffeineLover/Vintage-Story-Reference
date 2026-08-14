using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Gen2GcCallback : CriticalFinalizerObject
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002298 File Offset: 0x00000498
		private Gen2GcCallback(Action<object> callback, object target)
		{
			this.callback = callback;
			this.handle = GCHandle.Alloc(target, 0);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022B4 File Offset: 0x000004B4
		public static void Register(Action<object> callback, object target)
		{
			new Gen2GcCallback(callback, target);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022C0 File Offset: 0x000004C0
		protected override void Finalize()
		{
			try
			{
				object target = this.handle.Target;
				if (target != null)
				{
					try
					{
						this.callback.Invoke(target);
					}
					catch
					{
					}
					GC.ReRegisterForFinalize(this);
				}
				else
				{
					this.handle.Free();
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x04000012 RID: 18
		private readonly Action<object> callback;

		// Token: 0x04000013 RID: 19
		private GCHandle handle;
	}
}
