using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200005B RID: 91
	public struct ImGuiStoragePtr
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x0001521F File Offset: 0x0001341F
		public unsafe readonly ImGuiStorage* NativePtr { get; }

		// Token: 0x0600080F RID: 2063 RVA: 0x00015227 File Offset: 0x00013427
		public unsafe ImGuiStoragePtr(ImGuiStorage* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00015230 File Offset: 0x00013430
		public unsafe ImGuiStoragePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001523E File Offset: 0x0001343E
		public unsafe static implicit operator ImGuiStoragePtr(ImGuiStorage* nativePtr)
		{
			return new ImGuiStoragePtr(nativePtr);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00015246 File Offset: 0x00013446
		public unsafe static implicit operator ImGuiStorage*(ImGuiStoragePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001524F File Offset: 0x0001344F
		public static implicit operator ImGuiStoragePtr(IntPtr nativePtr)
		{
			return new ImGuiStoragePtr(nativePtr);
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00015257 File Offset: 0x00013457
		public unsafe ImPtrVector<ImGuiStoragePairPtr> Data
		{
			get
			{
				return new ImPtrVector<ImGuiStoragePairPtr>(this.NativePtr->Data, Unsafe.SizeOf<ImGuiStoragePair>());
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001526E File Offset: 0x0001346E
		public void BuildSortByKey()
		{
			ImGuiNative.ImGuiStorage_BuildSortByKey(this.NativePtr);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001527B File Offset: 0x0001347B
		public void Clear()
		{
			ImGuiNative.ImGuiStorage_Clear(this.NativePtr);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00015288 File Offset: 0x00013488
		public bool GetBool(uint key)
		{
			byte default_val = 0;
			return ImGuiNative.ImGuiStorage_GetBool(this.NativePtr, key, default_val) > 0;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x000152A8 File Offset: 0x000134A8
		public bool GetBool(uint key, bool default_val)
		{
			byte default_val2 = (default_val > false) ? 1 : 0;
			return ImGuiNative.ImGuiStorage_GetBool(this.NativePtr, key, default_val2) > 0;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000152CC File Offset: 0x000134CC
		public unsafe byte* GetBoolRef(uint key)
		{
			byte default_val = 0;
			return ImGuiNative.ImGuiStorage_GetBoolRef(this.NativePtr, key, default_val);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000152E8 File Offset: 0x000134E8
		public unsafe byte* GetBoolRef(uint key, bool default_val)
		{
			byte default_val2 = (default_val > false) ? 1 : 0;
			return ImGuiNative.ImGuiStorage_GetBoolRef(this.NativePtr, key, default_val2);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00015308 File Offset: 0x00013508
		public float GetFloat(uint key)
		{
			float default_val = 0f;
			return ImGuiNative.ImGuiStorage_GetFloat(this.NativePtr, key, default_val);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00015328 File Offset: 0x00013528
		public float GetFloat(uint key, float default_val)
		{
			return ImGuiNative.ImGuiStorage_GetFloat(this.NativePtr, key, default_val);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00015338 File Offset: 0x00013538
		public unsafe float* GetFloatRef(uint key)
		{
			float default_val = 0f;
			return ImGuiNative.ImGuiStorage_GetFloatRef(this.NativePtr, key, default_val);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00015358 File Offset: 0x00013558
		public unsafe float* GetFloatRef(uint key, float default_val)
		{
			return ImGuiNative.ImGuiStorage_GetFloatRef(this.NativePtr, key, default_val);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00015368 File Offset: 0x00013568
		public int GetInt(uint key)
		{
			int default_val = 0;
			return ImGuiNative.ImGuiStorage_GetInt(this.NativePtr, key, default_val);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00015384 File Offset: 0x00013584
		public int GetInt(uint key, int default_val)
		{
			return ImGuiNative.ImGuiStorage_GetInt(this.NativePtr, key, default_val);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00015394 File Offset: 0x00013594
		public unsafe int* GetIntRef(uint key)
		{
			int default_val = 0;
			return ImGuiNative.ImGuiStorage_GetIntRef(this.NativePtr, key, default_val);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x000153B0 File Offset: 0x000135B0
		public unsafe int* GetIntRef(uint key, int default_val)
		{
			return ImGuiNative.ImGuiStorage_GetIntRef(this.NativePtr, key, default_val);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x000153BF File Offset: 0x000135BF
		public IntPtr GetVoidPtr(uint key)
		{
			return (IntPtr)ImGuiNative.ImGuiStorage_GetVoidPtr(this.NativePtr, key);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000153D4 File Offset: 0x000135D4
		public unsafe void** GetVoidPtrRef(uint key)
		{
			void* default_val = null;
			return ImGuiNative.ImGuiStorage_GetVoidPtrRef(this.NativePtr, key, default_val);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x000153F4 File Offset: 0x000135F4
		public unsafe void** GetVoidPtrRef(uint key, IntPtr default_val)
		{
			void* default_val2 = default_val.ToPointer();
			return ImGuiNative.ImGuiStorage_GetVoidPtrRef(this.NativePtr, key, default_val2);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00015416 File Offset: 0x00013616
		public void SetAllInt(int val)
		{
			ImGuiNative.ImGuiStorage_SetAllInt(this.NativePtr, val);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00015424 File Offset: 0x00013624
		public void SetBool(uint key, bool val)
		{
			byte val2 = (val > false) ? 1 : 0;
			ImGuiNative.ImGuiStorage_SetBool(this.NativePtr, key, val2);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00015444 File Offset: 0x00013644
		public void SetFloat(uint key, float val)
		{
			ImGuiNative.ImGuiStorage_SetFloat(this.NativePtr, key, val);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00015453 File Offset: 0x00013653
		public void SetInt(uint key, int val)
		{
			ImGuiNative.ImGuiStorage_SetInt(this.NativePtr, key, val);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00015464 File Offset: 0x00013664
		public unsafe void SetVoidPtr(uint key, IntPtr val)
		{
			void* val2 = val.ToPointer();
			ImGuiNative.ImGuiStorage_SetVoidPtr(this.NativePtr, key, val2);
		}
	}
}
