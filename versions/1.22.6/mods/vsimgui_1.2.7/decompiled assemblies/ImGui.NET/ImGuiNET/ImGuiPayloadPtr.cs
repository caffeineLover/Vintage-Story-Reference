using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x0200004D RID: 77
	public struct ImGuiPayloadPtr
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00014C87 File Offset: 0x00012E87
		public unsafe readonly ImGuiPayload* NativePtr { get; }

		// Token: 0x060007B8 RID: 1976 RVA: 0x00014C8F File Offset: 0x00012E8F
		public unsafe ImGuiPayloadPtr(ImGuiPayload* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00014C98 File Offset: 0x00012E98
		public unsafe ImGuiPayloadPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00014CA6 File Offset: 0x00012EA6
		public unsafe static implicit operator ImGuiPayloadPtr(ImGuiPayload* nativePtr)
		{
			return new ImGuiPayloadPtr(nativePtr);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00014CAE File Offset: 0x00012EAE
		public unsafe static implicit operator ImGuiPayload*(ImGuiPayloadPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00014CB7 File Offset: 0x00012EB7
		public static implicit operator ImGuiPayloadPtr(IntPtr nativePtr)
		{
			return new ImGuiPayloadPtr(nativePtr);
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00014CBF File Offset: 0x00012EBF
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00014CD1 File Offset: 0x00012ED1
		public unsafe IntPtr Data
		{
			get
			{
				return (IntPtr)this.NativePtr->Data;
			}
			set
			{
				this.NativePtr->Data = (void*)value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00014CE4 File Offset: 0x00012EE4
		public unsafe ref int DataSize
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->DataSize));
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00014CF7 File Offset: 0x00012EF7
		public unsafe ref uint SourceId
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->SourceId));
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00014D0A File Offset: 0x00012F0A
		public unsafe ref uint SourceParentId
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->SourceParentId));
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00014D1D File Offset: 0x00012F1D
		public unsafe ref int DataFrameCount
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->DataFrameCount));
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00014D30 File Offset: 0x00012F30
		public unsafe RangeAccessor<byte> DataType
		{
			get
			{
				return new RangeAccessor<byte>((void*)(&this.NativePtr->DataType.FixedElementField), 33);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00014D4A File Offset: 0x00012F4A
		public unsafe ref bool Preview
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->Preview));
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00014D5D File Offset: 0x00012F5D
		public unsafe ref bool Delivery
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->Delivery));
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00014D70 File Offset: 0x00012F70
		public void Clear()
		{
			ImGuiNative.ImGuiPayload_Clear(this.NativePtr);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00014D7D File Offset: 0x00012F7D
		public void Destroy()
		{
			ImGuiNative.ImGuiPayload_destroy(this.NativePtr);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00014D8C File Offset: 0x00012F8C
		public unsafe bool IsDataType(ReadOnlySpan<char> type)
		{
			int num = 0;
			byte* ptr;
			if (type != null)
			{
				num = Encoding.UTF8.GetByteCount(type);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(type, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			int num2 = (int)ImGuiNative.ImGuiPayload_IsDataType(this.NativePtr, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
			return num2 != 0;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00014DFF File Offset: 0x00012FFF
		public bool IsDelivery()
		{
			return ImGuiNative.ImGuiPayload_IsDelivery(this.NativePtr) > 0;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00014E0F File Offset: 0x0001300F
		public bool IsPreview()
		{
			return ImGuiNative.ImGuiPayload_IsPreview(this.NativePtr) > 0;
		}
	}
}
