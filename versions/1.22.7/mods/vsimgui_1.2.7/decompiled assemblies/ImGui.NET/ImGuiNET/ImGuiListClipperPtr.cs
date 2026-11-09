using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000043 RID: 67
	public struct ImGuiListClipperPtr
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00014AF8 File Offset: 0x00012CF8
		public unsafe readonly ImGuiListClipper* NativePtr { get; }

		// Token: 0x0600051A RID: 1306 RVA: 0x00014B00 File Offset: 0x00012D00
		public unsafe ImGuiListClipperPtr(ImGuiListClipper* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00014B09 File Offset: 0x00012D09
		public unsafe ImGuiListClipperPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00014B17 File Offset: 0x00012D17
		public unsafe static implicit operator ImGuiListClipperPtr(ImGuiListClipper* nativePtr)
		{
			return new ImGuiListClipperPtr(nativePtr);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00014B1F File Offset: 0x00012D1F
		public unsafe static implicit operator ImGuiListClipper*(ImGuiListClipperPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00014B28 File Offset: 0x00012D28
		public static implicit operator ImGuiListClipperPtr(IntPtr nativePtr)
		{
			return new ImGuiListClipperPtr(nativePtr);
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00014B30 File Offset: 0x00012D30
		public unsafe ref IntPtr Ctx
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->Ctx));
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00014B43 File Offset: 0x00012D43
		public unsafe ref int DisplayStart
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->DisplayStart));
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00014B56 File Offset: 0x00012D56
		public unsafe ref int DisplayEnd
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->DisplayEnd));
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00014B69 File Offset: 0x00012D69
		public unsafe ref int ItemsCount
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->ItemsCount));
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00014B7C File Offset: 0x00012D7C
		public unsafe ref float ItemsHeight
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ItemsHeight));
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00014B8F File Offset: 0x00012D8F
		public unsafe ref float StartPosY
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->StartPosY));
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00014BA2 File Offset: 0x00012DA2
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x00014BB4 File Offset: 0x00012DB4
		public unsafe IntPtr TempData
		{
			get
			{
				return (IntPtr)this.NativePtr->TempData;
			}
			set
			{
				this.NativePtr->TempData = (void*)value;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00014BC8 File Offset: 0x00012DC8
		public void Begin(int items_count)
		{
			float items_height = -1f;
			ImGuiNative.ImGuiListClipper_Begin(this.NativePtr, items_count, items_height);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00014BE8 File Offset: 0x00012DE8
		public void Begin(int items_count, float items_height)
		{
			ImGuiNative.ImGuiListClipper_Begin(this.NativePtr, items_count, items_height);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00014BF7 File Offset: 0x00012DF7
		public void Destroy()
		{
			ImGuiNative.ImGuiListClipper_destroy(this.NativePtr);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00014C04 File Offset: 0x00012E04
		public void End()
		{
			ImGuiNative.ImGuiListClipper_End(this.NativePtr);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00014C11 File Offset: 0x00012E11
		public void IncludeItemByIndex(int item_index)
		{
			ImGuiNative.ImGuiListClipper_IncludeItemByIndex(this.NativePtr, item_index);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00014C1F File Offset: 0x00012E1F
		public bool Step()
		{
			return ImGuiNative.ImGuiListClipper_Step(this.NativePtr) > 0;
		}
	}
}
