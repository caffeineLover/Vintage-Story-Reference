using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000079 RID: 121
	public struct StbUndoRecordPtr
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0001622F File Offset: 0x0001442F
		public unsafe readonly StbUndoRecord* NativePtr { get; }

		// Token: 0x060008E3 RID: 2275 RVA: 0x00016237 File Offset: 0x00014437
		public unsafe StbUndoRecordPtr(StbUndoRecord* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00016240 File Offset: 0x00014440
		public unsafe StbUndoRecordPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001624E File Offset: 0x0001444E
		public unsafe static implicit operator StbUndoRecordPtr(StbUndoRecord* nativePtr)
		{
			return new StbUndoRecordPtr(nativePtr);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00016256 File Offset: 0x00014456
		public unsafe static implicit operator StbUndoRecord*(StbUndoRecordPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001625F File Offset: 0x0001445F
		public static implicit operator StbUndoRecordPtr(IntPtr nativePtr)
		{
			return new StbUndoRecordPtr(nativePtr);
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00016267 File Offset: 0x00014467
		public unsafe ref int where
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->where));
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0001627A File Offset: 0x0001447A
		public unsafe ref int insert_length
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->insert_length));
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0001628D File Offset: 0x0001448D
		public unsafe ref int delete_length
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->delete_length));
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x000162A0 File Offset: 0x000144A0
		public unsafe ref int char_storage
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->char_storage));
			}
		}
	}
}
