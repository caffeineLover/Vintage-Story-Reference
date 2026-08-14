using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x020001A0 RID: 416
	internal static class Mono
	{
		// Token: 0x06000D4C RID: 3404
		[DllImport("mono-2.0-bdwgc.dll")]
		private static extern IntPtr mono_thread_current();

		// Token: 0x06000D4D RID: 3405
		[DllImport("mono-2.0-bdwgc.dll")]
		[return: MarshalAs(2)]
		private static extern bool mono_thread_is_foreign(IntPtr threadPtr);

		// Token: 0x06000D4E RID: 3406
		[DllImport("mono-2.0-bdwgc.dll")]
		private static extern void mono_unity_thread_fast_attach(IntPtr domainPtr);

		// Token: 0x06000D4F RID: 3407
		[DllImport("mono-2.0-bdwgc.dll")]
		private static extern void mono_unity_thread_fast_detach();

		// Token: 0x06000D50 RID: 3408
		[DllImport("mono-2.0-bdwgc.dll")]
		private static extern void mono_thread_pop_appdomain_ref();

		// Token: 0x06000D51 RID: 3409
		[DllImport("mono-2.0-bdwgc.dll")]
		private static extern IntPtr mono_domain_get();

		// Token: 0x0400027B RID: 635
		private static Lazy<bool> _usingMono = new Lazy<bool>(delegate()
		{
			IntPtr intPtr = Platform.LoadLibraryExW("mono-2.0-bdwgc.dll", IntPtr.Zero, 0U);
			if (intPtr == IntPtr.Zero)
			{
				return false;
			}
			if (!Platform.FreeLibrary(intPtr))
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
			return true;
		});

		// Token: 0x020003EF RID: 1007
		private struct MonoObject
		{
			// Token: 0x040009C4 RID: 2500
			private IntPtr vtable;

			// Token: 0x040009C5 RID: 2501
			private IntPtr synchronisation;
		}

		// Token: 0x020003F0 RID: 1008
		private struct MonoThread
		{
			// Token: 0x040009C6 RID: 2502
			private Mono.MonoObject obj;

			// Token: 0x040009C7 RID: 2503
			public unsafe Mono.MonoInternalThread_x64* internal_thread;

			// Token: 0x040009C8 RID: 2504
			private IntPtr start_obj;

			// Token: 0x040009C9 RID: 2505
			private IntPtr pending_exception;
		}

		// Token: 0x020003F1 RID: 1009
		[Flags]
		private enum MonoThreadFlag
		{
			// Token: 0x040009CB RID: 2507
			MONO_THREAD_FLAG_DONT_MANAGE = 1,
			// Token: 0x040009CC RID: 2508
			MONO_THREAD_FLAG_NAME_SET = 2,
			// Token: 0x040009CD RID: 2509
			MONO_THREAD_FLAG_APPDOMAIN_ABORT = 4
		}

		// Token: 0x020003F2 RID: 1010
		[StructLayout(2)]
		private struct MonoInternalThread_x64
		{
			// Token: 0x040009CE RID: 2510
			[FieldOffset(208)]
			public Mono.MonoThreadFlag flags;
		}

		// Token: 0x020003F3 RID: 1011
		public sealed class ThreadContext : IDisposable
		{
			// Token: 0x06001B65 RID: 7013 RVA: 0x00049C90 File Offset: 0x00047E90
			public unsafe ThreadContext()
			{
				if (Mono._usingMono.Value)
				{
					IntPtr intPtr = Mono.mono_thread_current();
					if (Mono.mono_thread_is_foreign(intPtr))
					{
						if (Mono.ThreadContext._foreignThreads.Value.Add(intPtr))
						{
							Mono.mono_thread_pop_appdomain_ref();
							((Mono.MonoThread*)((void*)intPtr))->internal_thread->flags |= Mono.MonoThreadFlag.MONO_THREAD_FLAG_DONT_MANAGE;
						}
						Mono.mono_unity_thread_fast_attach(Mono.mono_domain_get());
						this._threadPtr = intPtr;
					}
				}
			}

			// Token: 0x06001B66 RID: 7014 RVA: 0x00049D05 File Offset: 0x00047F05
			public void Dispose()
			{
				if (this._threadPtr != IntPtr.Zero)
				{
					Mono.mono_unity_thread_fast_detach();
				}
			}

			// Token: 0x040009CF RID: 2511
			private static Lazy<HashSet<IntPtr>> _foreignThreads = new Lazy<HashSet<IntPtr>>();

			// Token: 0x040009D0 RID: 2512
			private readonly IntPtr _threadPtr = IntPtr.Zero;
		}
	}
}
