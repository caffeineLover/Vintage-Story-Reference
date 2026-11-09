using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x0200007B RID: 123
	public struct StbUndoStatePtr
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x000162B3 File Offset: 0x000144B3
		public unsafe readonly StbUndoState* NativePtr { get; }

		// Token: 0x060008ED RID: 2285 RVA: 0x000162BB File Offset: 0x000144BB
		public unsafe StbUndoStatePtr(StbUndoState* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x000162C4 File Offset: 0x000144C4
		public unsafe StbUndoStatePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x000162D2 File Offset: 0x000144D2
		public unsafe static implicit operator StbUndoStatePtr(StbUndoState* nativePtr)
		{
			return new StbUndoStatePtr(nativePtr);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x000162DA File Offset: 0x000144DA
		public unsafe static implicit operator StbUndoState*(StbUndoStatePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000162E3 File Offset: 0x000144E3
		public static implicit operator StbUndoStatePtr(IntPtr nativePtr)
		{
			return new StbUndoStatePtr(nativePtr);
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x000162EB File Offset: 0x000144EB
		public unsafe RangeAccessor<StbUndoRecord> undo_rec
		{
			get
			{
				return new RangeAccessor<StbUndoRecord>((void*)(&this.NativePtr->undo_rec_0), 99);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00016300 File Offset: 0x00014500
		public unsafe RangeAccessor<ushort> undo_char
		{
			get
			{
				return new RangeAccessor<ushort>((void*)(&this.NativePtr->undo_char.FixedElementField), 999);
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0001631D File Offset: 0x0001451D
		public unsafe ref short undo_point
		{
			get
			{
				return Unsafe.AsRef<short>((void*)(&this.NativePtr->undo_point));
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00016330 File Offset: 0x00014530
		public unsafe ref short redo_point
		{
			get
			{
				return Unsafe.AsRef<short>((void*)(&this.NativePtr->redo_point));
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00016343 File Offset: 0x00014543
		public unsafe ref int undo_char_point
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->undo_char_point));
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00016356 File Offset: 0x00014556
		public unsafe ref int redo_char_point
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->redo_char_point));
			}
		}
	}
}
