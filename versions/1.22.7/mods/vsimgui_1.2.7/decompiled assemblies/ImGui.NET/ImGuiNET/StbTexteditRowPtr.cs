using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000077 RID: 119
	public struct StbTexteditRowPtr
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00016185 File Offset: 0x00014385
		public unsafe readonly StbTexteditRow* NativePtr { get; }

		// Token: 0x060008D7 RID: 2263 RVA: 0x0001618D File Offset: 0x0001438D
		public unsafe StbTexteditRowPtr(StbTexteditRow* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00016196 File Offset: 0x00014396
		public unsafe StbTexteditRowPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x000161A4 File Offset: 0x000143A4
		public unsafe static implicit operator StbTexteditRowPtr(StbTexteditRow* nativePtr)
		{
			return new StbTexteditRowPtr(nativePtr);
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x000161AC File Offset: 0x000143AC
		public unsafe static implicit operator StbTexteditRow*(StbTexteditRowPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000161B5 File Offset: 0x000143B5
		public static implicit operator StbTexteditRowPtr(IntPtr nativePtr)
		{
			return new StbTexteditRowPtr(nativePtr);
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x000161BD File Offset: 0x000143BD
		public unsafe ref float x0
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->x0));
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000161D0 File Offset: 0x000143D0
		public unsafe ref float x1
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->x1));
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x000161E3 File Offset: 0x000143E3
		public unsafe ref float baseline_y_delta
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->baseline_y_delta));
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x000161F6 File Offset: 0x000143F6
		public unsafe ref float ymin
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ymin));
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00016209 File Offset: 0x00014409
		public unsafe ref float ymax
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->ymax));
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0001621C File Offset: 0x0001441C
		public unsafe ref int num_chars
		{
			get
			{
				return Unsafe.AsRef<int>((void*)(&this.NativePtr->num_chars));
			}
		}
	}
}
