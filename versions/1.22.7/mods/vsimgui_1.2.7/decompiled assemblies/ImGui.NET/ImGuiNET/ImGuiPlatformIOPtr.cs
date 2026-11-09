using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000051 RID: 81
	public struct ImGuiPlatformIOPtr
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00014E9D File Offset: 0x0001309D
		public unsafe readonly ImGuiPlatformIO* NativePtr { get; }

		// Token: 0x060007D6 RID: 2006 RVA: 0x00014EA5 File Offset: 0x000130A5
		public unsafe ImGuiPlatformIOPtr(ImGuiPlatformIO* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00014EAE File Offset: 0x000130AE
		public unsafe ImGuiPlatformIOPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00014EBC File Offset: 0x000130BC
		public unsafe static implicit operator ImGuiPlatformIOPtr(ImGuiPlatformIO* nativePtr)
		{
			return new ImGuiPlatformIOPtr(nativePtr);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00014EC4 File Offset: 0x000130C4
		public unsafe static implicit operator ImGuiPlatformIO*(ImGuiPlatformIOPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00014ECD File Offset: 0x000130CD
		public static implicit operator ImGuiPlatformIOPtr(IntPtr nativePtr)
		{
			return new ImGuiPlatformIOPtr(nativePtr);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00014ED5 File Offset: 0x000130D5
		public unsafe ref IntPtr Platform_CreateWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_CreateWindow));
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00014EE8 File Offset: 0x000130E8
		public unsafe ref IntPtr Platform_DestroyWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_DestroyWindow));
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00014EFB File Offset: 0x000130FB
		public unsafe ref IntPtr Platform_ShowWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_ShowWindow));
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00014F0E File Offset: 0x0001310E
		public unsafe ref IntPtr Platform_SetWindowPos
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_SetWindowPos));
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00014F21 File Offset: 0x00013121
		public unsafe ref IntPtr Platform_GetWindowPos
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_GetWindowPos));
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00014F34 File Offset: 0x00013134
		public unsafe ref IntPtr Platform_SetWindowSize
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_SetWindowSize));
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00014F47 File Offset: 0x00013147
		public unsafe ref IntPtr Platform_GetWindowSize
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_GetWindowSize));
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00014F5A File Offset: 0x0001315A
		public unsafe ref IntPtr Platform_SetWindowFocus
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_SetWindowFocus));
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00014F6D File Offset: 0x0001316D
		public unsafe ref IntPtr Platform_GetWindowFocus
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_GetWindowFocus));
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00014F80 File Offset: 0x00013180
		public unsafe ref IntPtr Platform_GetWindowMinimized
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_GetWindowMinimized));
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00014F93 File Offset: 0x00013193
		public unsafe ref IntPtr Platform_SetWindowTitle
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_SetWindowTitle));
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x00014FA6 File Offset: 0x000131A6
		public unsafe ref IntPtr Platform_SetWindowAlpha
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_SetWindowAlpha));
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00014FB9 File Offset: 0x000131B9
		public unsafe ref IntPtr Platform_UpdateWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_UpdateWindow));
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00014FCC File Offset: 0x000131CC
		public unsafe ref IntPtr Platform_RenderWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_RenderWindow));
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00014FDF File Offset: 0x000131DF
		public unsafe ref IntPtr Platform_SwapBuffers
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_SwapBuffers));
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x00014FF2 File Offset: 0x000131F2
		public unsafe ref IntPtr Platform_GetWindowDpiScale
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_GetWindowDpiScale));
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00015005 File Offset: 0x00013205
		public unsafe ref IntPtr Platform_OnChangedViewport
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_OnChangedViewport));
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00015018 File Offset: 0x00013218
		public unsafe ref IntPtr Platform_CreateVkSurface
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Platform_CreateVkSurface));
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0001502B File Offset: 0x0001322B
		public unsafe ref IntPtr Renderer_CreateWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Renderer_CreateWindow));
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0001503E File Offset: 0x0001323E
		public unsafe ref IntPtr Renderer_DestroyWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Renderer_DestroyWindow));
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x00015051 File Offset: 0x00013251
		public unsafe ref IntPtr Renderer_SetWindowSize
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Renderer_SetWindowSize));
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00015064 File Offset: 0x00013264
		public unsafe ref IntPtr Renderer_RenderWindow
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Renderer_RenderWindow));
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00015077 File Offset: 0x00013277
		public unsafe ref IntPtr Renderer_SwapBuffers
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Renderer_SwapBuffers));
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0001508A File Offset: 0x0001328A
		public unsafe ImPtrVector<ImGuiPlatformMonitorPtr> Monitors
		{
			get
			{
				return new ImPtrVector<ImGuiPlatformMonitorPtr>(this.NativePtr->Monitors, Unsafe.SizeOf<ImGuiPlatformMonitor>());
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000150A1 File Offset: 0x000132A1
		public unsafe ImVector<ImGuiViewportPtr> Viewports
		{
			get
			{
				return new ImVector<ImGuiViewportPtr>(this.NativePtr->Viewports);
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000150B3 File Offset: 0x000132B3
		public void Destroy()
		{
			ImGuiNative.ImGuiPlatformIO_destroy(this.NativePtr);
		}
	}
}
