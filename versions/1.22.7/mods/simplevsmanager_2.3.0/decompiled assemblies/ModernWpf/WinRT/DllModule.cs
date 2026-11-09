using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001AD RID: 429
	internal sealed class DllModule
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x0002A0D4 File Offset: 0x000282D4
		public static bool TryLoad(string fileName, out DllModule module)
		{
			Dictionary<string, DllModule> cache = DllModule._cache;
			bool result;
			lock (cache)
			{
				if (DllModule._cache.TryGetValue(fileName, ref module))
				{
					result = true;
				}
				else if (DllModule.TryCreate(fileName, out module))
				{
					DllModule._cache[fileName] = module;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0002A13C File Offset: 0x0002833C
		private unsafe static bool TryCreate(string fileName, out DllModule module)
		{
			IntPtr intPtr = Platform.LoadLibraryExW(Path.Combine(DllModule._currentModuleDirectory, fileName), IntPtr.Zero, 8U);
			if (intPtr == IntPtr.Zero)
			{
				NativeLibrary.TryLoad(fileName, Assembly.GetExecutingAssembly(), default(DllImportSearchPath?), ref intPtr);
			}
			if (intPtr == IntPtr.Zero)
			{
				module = null;
				return false;
			}
			void* ptr = Platform.TryGetProcAddress(intPtr, "DllGetActivationFactory");
			if (ptr == null)
			{
				module = null;
				return false;
			}
			module = new DllModule(fileName, intPtr, ptr);
			return true;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0002A1B8 File Offset: 0x000283B8
		private unsafe DllModule(string fileName, IntPtr moduleHandle, void* getActivationFactory)
		{
			this._fileName = fileName;
			this._moduleHandle = moduleHandle;
			this._GetActivationFactory = getActivationFactory;
			void* ptr = Platform.TryGetProcAddress(this._moduleHandle, "DllCanUnloadNow");
			if (ptr != null)
			{
				this._CanUnloadNow = ptr;
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002A200 File Offset: 0x00028400
		[return: TupleElementNames(new string[]
		{
			"obj",
			"hr"
		})]
		public unsafe ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> GetActivationFactory(string runtimeClassId)
		{
			IntPtr zero = IntPtr.Zero;
			ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(runtimeClassId);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method getActivationFactory = this._GetActivationFactory;
						int num = calli(System.Int32(System.IntPtr,System.IntPtr*), MarshalString.GetAbi(ref pinnable), &zero, getActivationFactory);
						if (num == 0)
						{
							result = new ValueTuple<ObjectReference<IActivationFactoryVftbl>, int>(ComWrappersSupport.GetObjectReferenceForInterface<IActivationFactoryVftbl>(zero), num);
						}
						else
						{
							result = new ValueTuple<ObjectReference<IActivationFactoryVftbl>, int>(null, num);
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
				MarshalInspectable<object>.DisposeAbi(zero);
			}
			return result;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0002A288 File Offset: 0x00028488
		protected override void Finalize()
		{
			try
			{
				Dictionary<string, DllModule> cache = DllModule._cache;
				lock (cache)
				{
					DllModule._cache.Remove(this._fileName);
				}
				if (this._moduleHandle != IntPtr.Zero && !Platform.FreeLibrary(this._moduleHandle))
				{
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x040002A5 RID: 677
		private readonly string _fileName;

		// Token: 0x040002A6 RID: 678
		private readonly IntPtr _moduleHandle;

		// Token: 0x040002A7 RID: 679
		private readonly method _GetActivationFactory;

		// Token: 0x040002A8 RID: 680
		private readonly method _CanUnloadNow;

		// Token: 0x040002A9 RID: 681
		private static readonly string _currentModuleDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		// Token: 0x040002AA RID: 682
		private static Dictionary<string, DllModule> _cache = new Dictionary<string, DllModule>(StringComparer.Ordinal);
	}
}
