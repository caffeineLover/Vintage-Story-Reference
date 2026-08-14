using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200004F RID: 79
	public struct ImGuiPlatformImeDataPtr
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00014E1F File Offset: 0x0001301F
		public unsafe readonly ImGuiPlatformImeData* NativePtr { get; }

		// Token: 0x060007CC RID: 1996 RVA: 0x00014E27 File Offset: 0x00013027
		public unsafe ImGuiPlatformImeDataPtr(ImGuiPlatformImeData* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00014E30 File Offset: 0x00013030
		public unsafe ImGuiPlatformImeDataPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00014E3E File Offset: 0x0001303E
		public unsafe static implicit operator ImGuiPlatformImeDataPtr(ImGuiPlatformImeData* nativePtr)
		{
			return new ImGuiPlatformImeDataPtr(nativePtr);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00014E46 File Offset: 0x00013046
		public unsafe static implicit operator ImGuiPlatformImeData*(ImGuiPlatformImeDataPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00014E4F File Offset: 0x0001304F
		public static implicit operator ImGuiPlatformImeDataPtr(IntPtr nativePtr)
		{
			return new ImGuiPlatformImeDataPtr(nativePtr);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00014E57 File Offset: 0x00013057
		public unsafe ref bool WantVisible
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->WantVisible));
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00014E6A File Offset: 0x0001306A
		public unsafe ref Vector2 InputPos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->InputPos));
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00014E7D File Offset: 0x0001307D
		public unsafe ref float InputLineHeight
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->InputLineHeight));
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00014E90 File Offset: 0x00013090
		public void Destroy()
		{
			ImGuiNative.ImGuiPlatformImeData_destroy(this.NativePtr);
		}
	}
}
