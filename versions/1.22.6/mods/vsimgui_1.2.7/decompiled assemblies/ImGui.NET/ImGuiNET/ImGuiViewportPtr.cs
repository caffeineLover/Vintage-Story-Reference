using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000071 RID: 113
	public struct ImGuiViewportPtr
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x00015E9E File Offset: 0x0001409E
		public unsafe readonly ImGuiViewport* NativePtr { get; }

		// Token: 0x060008AA RID: 2218 RVA: 0x00015EA6 File Offset: 0x000140A6
		public unsafe ImGuiViewportPtr(ImGuiViewport* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00015EAF File Offset: 0x000140AF
		public unsafe ImGuiViewportPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00015EBD File Offset: 0x000140BD
		public unsafe static implicit operator ImGuiViewportPtr(ImGuiViewport* nativePtr)
		{
			return new ImGuiViewportPtr(nativePtr);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00015EC5 File Offset: 0x000140C5
		public unsafe static implicit operator ImGuiViewport*(ImGuiViewportPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00015ECE File Offset: 0x000140CE
		public static implicit operator ImGuiViewportPtr(IntPtr nativePtr)
		{
			return new ImGuiViewportPtr(nativePtr);
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00015ED6 File Offset: 0x000140D6
		public unsafe ref uint ID
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->ID));
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00015EE9 File Offset: 0x000140E9
		public unsafe ref ImGuiViewportFlags Flags
		{
			get
			{
				return Unsafe.AsRef<ImGuiViewportFlags>((void*)(&this.NativePtr->Flags));
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00015EFC File Offset: 0x000140FC
		public unsafe ref Vector2 Pos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->Pos));
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00015F0F File Offset: 0x0001410F
		public unsafe ref Vector2 Size
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->Size));
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00015F22 File Offset: 0x00014122
		public unsafe ref Vector2 WorkPos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->WorkPos));
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00015F35 File Offset: 0x00014135
		public unsafe ref Vector2 WorkSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->WorkSize));
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x00015F48 File Offset: 0x00014148
		public unsafe ref float DpiScale
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->DpiScale));
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00015F5B File Offset: 0x0001415B
		public unsafe ref uint ParentViewportId
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->ParentViewportId));
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00015F6E File Offset: 0x0001416E
		public unsafe ImDrawDataPtr DrawData
		{
			get
			{
				return new ImDrawDataPtr(this.NativePtr->DrawData);
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00015F80 File Offset: 0x00014180
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x00015F92 File Offset: 0x00014192
		public unsafe IntPtr RendererUserData
		{
			get
			{
				return (IntPtr)this.NativePtr->RendererUserData;
			}
			set
			{
				this.NativePtr->RendererUserData = (void*)value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00015FA5 File Offset: 0x000141A5
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00015FB7 File Offset: 0x000141B7
		public unsafe IntPtr PlatformUserData
		{
			get
			{
				return (IntPtr)this.NativePtr->PlatformUserData;
			}
			set
			{
				this.NativePtr->PlatformUserData = (void*)value;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00015FCA File Offset: 0x000141CA
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x00015FDC File Offset: 0x000141DC
		public unsafe IntPtr PlatformHandle
		{
			get
			{
				return (IntPtr)this.NativePtr->PlatformHandle;
			}
			set
			{
				this.NativePtr->PlatformHandle = (void*)value;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00015FEF File Offset: 0x000141EF
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x00016001 File Offset: 0x00014201
		public unsafe IntPtr PlatformHandleRaw
		{
			get
			{
				return (IntPtr)this.NativePtr->PlatformHandleRaw;
			}
			set
			{
				this.NativePtr->PlatformHandleRaw = (void*)value;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00016014 File Offset: 0x00014214
		public unsafe ref bool PlatformWindowCreated
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->PlatformWindowCreated));
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00016027 File Offset: 0x00014227
		public unsafe ref bool PlatformRequestMove
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->PlatformRequestMove));
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0001603A File Offset: 0x0001423A
		public unsafe ref bool PlatformRequestResize
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->PlatformRequestResize));
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0001604D File Offset: 0x0001424D
		public unsafe ref bool PlatformRequestClose
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->PlatformRequestClose));
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00016060 File Offset: 0x00014260
		public void Destroy()
		{
			ImGuiNative.ImGuiViewport_destroy(this.NativePtr);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00016070 File Offset: 0x00014270
		public unsafe Vector2 GetCenter()
		{
			Vector2 result;
			ImGuiNative.ImGuiViewport_GetCenter(&result, this.NativePtr);
			return result;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001608C File Offset: 0x0001428C
		public unsafe Vector2 GetWorkCenter()
		{
			Vector2 result;
			ImGuiNative.ImGuiViewport_GetWorkCenter(&result, this.NativePtr);
			return result;
		}
	}
}
