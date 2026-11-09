using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace imnodesNET
{
	// Token: 0x0200000D RID: 13
	public struct ImNodesStylePtr
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00002960 File Offset: 0x00000B60
		public unsafe readonly ImNodesStyle* NativePtr { get; }

		// Token: 0x060000C7 RID: 199 RVA: 0x00002968 File Offset: 0x00000B68
		public unsafe ImNodesStylePtr(ImNodesStyle* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00002971 File Offset: 0x00000B71
		public unsafe ImNodesStylePtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000297F File Offset: 0x00000B7F
		public unsafe static implicit operator ImNodesStylePtr(ImNodesStyle* nativePtr)
		{
			return new ImNodesStylePtr(nativePtr);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002987 File Offset: 0x00000B87
		public unsafe static implicit operator ImNodesStyle*(ImNodesStylePtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002990 File Offset: 0x00000B90
		public static implicit operator ImNodesStylePtr(IntPtr nativePtr)
		{
			return new ImNodesStylePtr(nativePtr);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00002998 File Offset: 0x00000B98
		public unsafe ref float GridSpacing
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->GridSpacing));
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000029AB File Offset: 0x00000BAB
		public unsafe ref float NodeCornerRounding
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->NodeCornerRounding));
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000029BE File Offset: 0x00000BBE
		public unsafe ref Vector2 NodePadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->NodePadding));
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000029D1 File Offset: 0x00000BD1
		public unsafe ref float NodeBorderThickness
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->NodeBorderThickness));
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000029E4 File Offset: 0x00000BE4
		public unsafe ref float LinkThickness
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->LinkThickness));
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000029F7 File Offset: 0x00000BF7
		public unsafe ref float LinkLineSegmentsPerLength
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->LinkLineSegmentsPerLength));
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00002A0A File Offset: 0x00000C0A
		public unsafe ref float LinkHoverDistance
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->LinkHoverDistance));
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00002A1D File Offset: 0x00000C1D
		public unsafe ref float PinCircleRadius
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PinCircleRadius));
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00002A30 File Offset: 0x00000C30
		public unsafe ref float PinQuadSideLength
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PinQuadSideLength));
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00002A43 File Offset: 0x00000C43
		public unsafe ref float PinTriangleSideLength
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PinTriangleSideLength));
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00002A56 File Offset: 0x00000C56
		public unsafe ref float PinLineThickness
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PinLineThickness));
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00002A69 File Offset: 0x00000C69
		public unsafe ref float PinHoverRadius
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PinHoverRadius));
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00002A7C File Offset: 0x00000C7C
		public unsafe ref float PinOffset
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->PinOffset));
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00002A8F File Offset: 0x00000C8F
		public unsafe ref Vector2 MiniMapPadding
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MiniMapPadding));
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00002AA2 File Offset: 0x00000CA2
		public unsafe ref Vector2 MiniMapOffset
		{
			get
			{
				return Unsafe.AsRef<Vector2>((void*)(&this.NativePtr->MiniMapOffset));
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00002AB5 File Offset: 0x00000CB5
		public unsafe ref ImNodesStyleFlags Flags
		{
			get
			{
				return Unsafe.AsRef<ImNodesStyleFlags>((void*)(&this.NativePtr->Flags));
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public unsafe RangeAccessor<uint> Colors
		{
			get
			{
				return new RangeAccessor<uint>((void*)(&this.NativePtr->Colors.FixedElementField), 29);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00002AE2 File Offset: 0x00000CE2
		public void Destroy()
		{
			imnodesNative.ImNodesStyle_destroy(this.NativePtr);
		}
	}
}
