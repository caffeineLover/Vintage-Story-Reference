using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200000E RID: 14
	public struct ImColorPtr
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002050 File Offset: 0x00000250
		public unsafe readonly ImColor* NativePtr { get; }

		// Token: 0x0600002E RID: 46 RVA: 0x00002058 File Offset: 0x00000258
		public unsafe ImColorPtr(ImColor* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002061 File Offset: 0x00000261
		public unsafe ImColorPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000206F File Offset: 0x0000026F
		public unsafe static implicit operator ImColorPtr(ImColor* nativePtr)
		{
			return new ImColorPtr(nativePtr);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002077 File Offset: 0x00000277
		public unsafe static implicit operator ImColor*(ImColorPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002080 File Offset: 0x00000280
		public static implicit operator ImColorPtr(IntPtr nativePtr)
		{
			return new ImColorPtr(nativePtr);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002088 File Offset: 0x00000288
		public unsafe ref Vector4 Value
		{
			get
			{
				return Unsafe.AsRef<Vector4>((void*)(&this.NativePtr->Value));
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000209B File Offset: 0x0000029B
		public void Destroy()
		{
			ImGuiNative.ImColor_destroy(this.NativePtr);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000020A8 File Offset: 0x000002A8
		public unsafe ImColor HSV(float h, float s, float v)
		{
			float a = 1f;
			ImColor result;
			ImGuiNative.ImColor_HSV(&result, h, s, v, a);
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000020C8 File Offset: 0x000002C8
		public unsafe ImColor HSV(float h, float s, float v, float a)
		{
			ImColor result;
			ImGuiNative.ImColor_HSV(&result, h, s, v, a);
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000020E4 File Offset: 0x000002E4
		public void SetHSV(float h, float s, float v)
		{
			float a = 1f;
			ImGuiNative.ImColor_SetHSV(this.NativePtr, h, s, v, a);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002106 File Offset: 0x00000306
		public void SetHSV(float h, float s, float v, float a)
		{
			ImGuiNative.ImColor_SetHSV(this.NativePtr, h, s, v, a);
		}
	}
}
