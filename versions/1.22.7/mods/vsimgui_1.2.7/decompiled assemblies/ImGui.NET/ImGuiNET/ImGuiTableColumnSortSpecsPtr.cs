using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000064 RID: 100
	public struct ImGuiTableColumnSortSpecsPtr
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00015891 File Offset: 0x00013A91
		public unsafe readonly ImGuiTableColumnSortSpecs* NativePtr { get; }

		// Token: 0x06000866 RID: 2150 RVA: 0x00015899 File Offset: 0x00013A99
		public unsafe ImGuiTableColumnSortSpecsPtr(ImGuiTableColumnSortSpecs* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000158A2 File Offset: 0x00013AA2
		public unsafe ImGuiTableColumnSortSpecsPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000158B0 File Offset: 0x00013AB0
		public unsafe static implicit operator ImGuiTableColumnSortSpecsPtr(ImGuiTableColumnSortSpecs* nativePtr)
		{
			return new ImGuiTableColumnSortSpecsPtr(nativePtr);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x000158B8 File Offset: 0x00013AB8
		public unsafe static implicit operator ImGuiTableColumnSortSpecs*(ImGuiTableColumnSortSpecsPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000158C1 File Offset: 0x00013AC1
		public static implicit operator ImGuiTableColumnSortSpecsPtr(IntPtr nativePtr)
		{
			return new ImGuiTableColumnSortSpecsPtr(nativePtr);
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x000158C9 File Offset: 0x00013AC9
		public unsafe ref uint ColumnUserID
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->ColumnUserID));
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x000158DC File Offset: 0x00013ADC
		public unsafe ref short ColumnIndex
		{
			get
			{
				return Unsafe.AsRef<short>((void*)(&this.NativePtr->ColumnIndex));
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x000158EF File Offset: 0x00013AEF
		public unsafe ref short SortOrder
		{
			get
			{
				return Unsafe.AsRef<short>((void*)(&this.NativePtr->SortOrder));
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00015902 File Offset: 0x00013B02
		public unsafe ref ImGuiSortDirection SortDirection
		{
			get
			{
				return Unsafe.AsRef<ImGuiSortDirection>((void*)(&this.NativePtr->SortDirection));
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00015915 File Offset: 0x00013B15
		public void Destroy()
		{
			ImGuiNative.ImGuiTableColumnSortSpecs_destroy(this.NativePtr);
		}
	}
}
