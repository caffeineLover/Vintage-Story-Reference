using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000068 RID: 104
	public struct ImGuiTableSortSpecsPtr
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00015922 File Offset: 0x00013B22
		public unsafe readonly ImGuiTableSortSpecs* NativePtr { get; }

		// Token: 0x06000871 RID: 2161 RVA: 0x0001592A File Offset: 0x00013B2A
		public unsafe ImGuiTableSortSpecsPtr(ImGuiTableSortSpecs* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00015933 File Offset: 0x00013B33
		public unsafe ImGuiTableSortSpecsPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00015941 File Offset: 0x00013B41
		public unsafe static implicit operator ImGuiTableSortSpecsPtr(ImGuiTableSortSpecs* nativePtr)
		{
			return new ImGuiTableSortSpecsPtr(nativePtr);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00015949 File Offset: 0x00013B49
		public unsafe static implicit operator ImGuiTableSortSpecs*(ImGuiTableSortSpecsPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00015952 File Offset: 0x00013B52
		public static implicit operator ImGuiTableSortSpecsPtr(IntPtr nativePtr)
		{
			return new ImGuiTableSortSpecsPtr(nativePtr);
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0001595A File Offset: 0x00013B5A
		public unsafe ImGuiTableColumnSortSpecsPtr Specs
		{
			get
			{
				return new ImGuiTableColumnSortSpecsPtr(this.NativePtr->Specs);
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0001596C File Offset: 0x00013B6C
		public unsafe ref int SpecsCount
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->SpecsCount));
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x0001597F File Offset: 0x00013B7F
		public unsafe ref bool SpecsDirty
		{
			get
			{
				return Unsafe.AsRef<bool>((void*)(&this.NativePtr->SpecsDirty));
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00015992 File Offset: 0x00013B92
		public void Destroy()
		{
			ImGuiNative.ImGuiTableSortSpecs_destroy(this.NativePtr);
		}
	}
}
