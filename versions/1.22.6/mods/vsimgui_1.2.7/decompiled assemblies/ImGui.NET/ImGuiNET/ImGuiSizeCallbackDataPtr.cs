using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000057 RID: 87
	public struct ImGuiSizeCallbackDataPtr
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x00015189 File Offset: 0x00013389
		public unsafe readonly ImGuiSizeCallbackData* NativePtr { get; }

		// Token: 0x06000804 RID: 2052 RVA: 0x00015191 File Offset: 0x00013391
		public unsafe ImGuiSizeCallbackDataPtr(ImGuiSizeCallbackData* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001519A File Offset: 0x0001339A
		public unsafe ImGuiSizeCallbackDataPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x000151A8 File Offset: 0x000133A8
		public unsafe static implicit operator ImGuiSizeCallbackDataPtr(ImGuiSizeCallbackData* nativePtr)
		{
			return new ImGuiSizeCallbackDataPtr(nativePtr);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000151B0 File Offset: 0x000133B0
		public unsafe static implicit operator ImGuiSizeCallbackData*(ImGuiSizeCallbackDataPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000151B9 File Offset: 0x000133B9
		public static implicit operator ImGuiSizeCallbackDataPtr(IntPtr nativePtr)
		{
			return new ImGuiSizeCallbackDataPtr(nativePtr);
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x000151C1 File Offset: 0x000133C1
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x000151D3 File Offset: 0x000133D3
		public unsafe IntPtr UserData
		{
			get
			{
				return (IntPtr)this.NativePtr->UserData;
			}
			set
			{
				this.NativePtr->UserData = (void*)value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x000151E6 File Offset: 0x000133E6
		public unsafe ref Vector2 Pos
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->Pos));
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x000151F9 File Offset: 0x000133F9
		public unsafe ref Vector2 CurrentSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->CurrentSize));
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0001520C File Offset: 0x0001340C
		public unsafe ref Vector2 DesiredSize
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->DesiredSize));
			}
		}
	}
}
