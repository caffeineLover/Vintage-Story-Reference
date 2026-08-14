using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200001C RID: 28
	public struct ImDrawListSplitterPtr
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000302C File Offset: 0x0000122C
		public unsafe readonly ImDrawListSplitter* NativePtr { get; }

		// Token: 0x060000E3 RID: 227 RVA: 0x00003034 File Offset: 0x00001234
		public unsafe ImDrawListSplitterPtr(ImDrawListSplitter* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000303D File Offset: 0x0000123D
		public unsafe ImDrawListSplitterPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000304B File Offset: 0x0000124B
		public unsafe static implicit operator ImDrawListSplitterPtr(ImDrawListSplitter* nativePtr)
		{
			return new ImDrawListSplitterPtr(nativePtr);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00003053 File Offset: 0x00001253
		public unsafe static implicit operator ImDrawListSplitter*(ImDrawListSplitterPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000305C File Offset: 0x0000125C
		public static implicit operator ImDrawListSplitterPtr(IntPtr nativePtr)
		{
			return new ImDrawListSplitterPtr(nativePtr);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00003064 File Offset: 0x00001264
		public unsafe ref int _Current
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->_Current));
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00003077 File Offset: 0x00001277
		public unsafe ref int _Count
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->_Count));
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000308A File Offset: 0x0000128A
		public unsafe ImPtrVector<ImDrawChannelPtr> _Channels
		{
			get
			{
				return new ImPtrVector<ImDrawChannelPtr>(this.NativePtr->_Channels, Unsafe.SizeOf<ImDrawChannel>());
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000030A1 File Offset: 0x000012A1
		public void Clear()
		{
			ImGuiNative.ImDrawListSplitter_Clear(this.NativePtr);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000030AE File Offset: 0x000012AE
		public void ClearFreeMemory()
		{
			ImGuiNative.ImDrawListSplitter_ClearFreeMemory(this.NativePtr);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000030BB File Offset: 0x000012BB
		public void Destroy()
		{
			ImGuiNative.ImDrawListSplitter_destroy(this.NativePtr);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000030C8 File Offset: 0x000012C8
		public unsafe void Merge(ImDrawListPtr draw_list)
		{
			ImDrawList* nativePtr = draw_list.NativePtr;
			ImGuiNative.ImDrawListSplitter_Merge(this.NativePtr, nativePtr);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000030EC File Offset: 0x000012EC
		public unsafe void SetCurrentChannel(ImDrawListPtr draw_list, int channel_idx)
		{
			ImDrawList* nativePtr = draw_list.NativePtr;
			ImGuiNative.ImDrawListSplitter_SetCurrentChannel(this.NativePtr, nativePtr, channel_idx);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00003110 File Offset: 0x00001310
		public unsafe void Split(ImDrawListPtr draw_list, int count)
		{
			ImDrawList* nativePtr = draw_list.NativePtr;
			ImGuiNative.ImDrawListSplitter_Split(this.NativePtr, nativePtr, count);
		}
	}
}
