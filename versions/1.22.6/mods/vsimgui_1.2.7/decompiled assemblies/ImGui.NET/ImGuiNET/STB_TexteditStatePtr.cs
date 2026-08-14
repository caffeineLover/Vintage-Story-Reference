using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200007D RID: 125
	public struct STB_TexteditStatePtr
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00016369 File Offset: 0x00014569
		public unsafe readonly STB_TexteditState* NativePtr { get; }

		// Token: 0x060008F9 RID: 2297 RVA: 0x00016371 File Offset: 0x00014571
		public unsafe STB_TexteditStatePtr(STB_TexteditState* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001637A File Offset: 0x0001457A
		public unsafe STB_TexteditStatePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00016388 File Offset: 0x00014588
		public unsafe static implicit operator STB_TexteditStatePtr(STB_TexteditState* nativePtr)
		{
			return new STB_TexteditStatePtr(nativePtr);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00016390 File Offset: 0x00014590
		public unsafe static implicit operator STB_TexteditState*(STB_TexteditStatePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00016399 File Offset: 0x00014599
		public static implicit operator STB_TexteditStatePtr(IntPtr nativePtr)
		{
			return new STB_TexteditStatePtr(nativePtr);
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x000163A1 File Offset: 0x000145A1
		public unsafe ref int cursor
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->cursor));
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x000163B4 File Offset: 0x000145B4
		public unsafe ref int select_start
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->select_start));
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x000163C7 File Offset: 0x000145C7
		public unsafe ref int select_end
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->select_end));
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x000163DA File Offset: 0x000145DA
		public unsafe ref byte insert_mode
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->insert_mode));
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x000163ED File Offset: 0x000145ED
		public unsafe ref int row_count_per_page
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->row_count_per_page));
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00016400 File Offset: 0x00014600
		public unsafe ref byte cursor_at_end_of_line
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->cursor_at_end_of_line));
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00016413 File Offset: 0x00014613
		public unsafe ref byte initialized
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->initialized));
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00016426 File Offset: 0x00014626
		public unsafe ref byte has_preferred_x
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->has_preferred_x));
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x00016439 File Offset: 0x00014639
		public unsafe ref byte single_line
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->single_line));
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x0001644C File Offset: 0x0001464C
		public unsafe ref byte padding1
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->padding1));
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0001645F File Offset: 0x0001465F
		public unsafe ref byte padding2
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->padding2));
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00016472 File Offset: 0x00014672
		public unsafe ref byte padding3
		{
			get
			{
				return Unsafe.AsRef<byte>((void*)(&this.NativePtr->padding3));
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x00016485 File Offset: 0x00014685
		public unsafe ref float preferred_x
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->preferred_x));
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00016498 File Offset: 0x00014698
		public unsafe ref StbUndoState undostate
		{
			get
			{
				return Unsafe.AsRef<StbUndoState>((void*)(&this.NativePtr->undostate));
			}
		}
	}
}
