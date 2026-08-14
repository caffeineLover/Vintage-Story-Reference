using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x02000019 RID: 25
	public struct ImDrawListPtr
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000240A File Offset: 0x0000060A
		public unsafe readonly ImDrawList* NativePtr { get; }

		// Token: 0x0600006F RID: 111 RVA: 0x00002412 File Offset: 0x00000612
		public unsafe ImDrawListPtr(ImDrawList* nativePtr)
		{
			this.NativePtr = nativePtr;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000241B File Offset: 0x0000061B
		public unsafe ImDrawListPtr(IntPtr nativePtr)
		{
			this.NativePtr = (void*)nativePtr;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002429 File Offset: 0x00000629
		public unsafe static implicit operator ImDrawListPtr(ImDrawList* nativePtr)
		{
			return new ImDrawListPtr(nativePtr);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002431 File Offset: 0x00000631
		public unsafe static implicit operator ImDrawList*(ImDrawListPtr wrappedPtr)
		{
			return wrappedPtr.NativePtr;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000243A File Offset: 0x0000063A
		public static implicit operator ImDrawListPtr(IntPtr nativePtr)
		{
			return new ImDrawListPtr(nativePtr);
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002442 File Offset: 0x00000642
		public unsafe ImPtrVector<ImDrawCmdPtr> CmdBuffer
		{
			get
			{
				return new ImPtrVector<ImDrawCmdPtr>(this.NativePtr->CmdBuffer, Unsafe.SizeOf<ImDrawCmd>());
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002459 File Offset: 0x00000659
		public unsafe ImVector<ushort> IdxBuffer
		{
			get
			{
				return new ImVector<ushort>(this.NativePtr->IdxBuffer);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000246B File Offset: 0x0000066B
		public unsafe ImPtrVector<ImDrawVertPtr> VtxBuffer
		{
			get
			{
				return new ImPtrVector<ImDrawVertPtr>(this.NativePtr->VtxBuffer, Unsafe.SizeOf<ImDrawVert>());
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002482 File Offset: 0x00000682
		public unsafe ref ImDrawListFlags Flags
		{
			get
			{
				return Unsafe.AsRef<ImDrawListFlags>((void*)(&this.NativePtr->Flags));
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002495 File Offset: 0x00000695
		public unsafe ref uint _VtxCurrentIdx
		{
			get
			{
				return Unsafe.AsRef<uint>((void*)(&this.NativePtr->_VtxCurrentIdx));
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000024A8 File Offset: 0x000006A8
		public unsafe ref IntPtr _Data
		{
			get
			{
				return Unsafe.AsRef<IntPtr>((void*)(&this.NativePtr->_Data));
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000024BB File Offset: 0x000006BB
		public unsafe NullTerminatedString _OwnerName
		{
			get
			{
				return new NullTerminatedString(this.NativePtr->_OwnerName);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000024CD File Offset: 0x000006CD
		public unsafe ImDrawVertPtr _VtxWritePtr
		{
			get
			{
				return new ImDrawVertPtr(this.NativePtr->_VtxWritePtr);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000024DF File Offset: 0x000006DF
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000024F1 File Offset: 0x000006F1
		public unsafe IntPtr _IdxWritePtr
		{
			get
			{
				return (IntPtr)((void*)this.NativePtr->_IdxWritePtr);
			}
			set
			{
				this.NativePtr->_IdxWritePtr = (ushort*)((void*)value);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002504 File Offset: 0x00000704
		public unsafe ImVector<Vector4> _ClipRectStack
		{
			get
			{
				return new ImVector<Vector4>(this.NativePtr->_ClipRectStack);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002516 File Offset: 0x00000716
		public unsafe ImVector<IntPtr> _TextureIdStack
		{
			get
			{
				return new ImVector<IntPtr>(this.NativePtr->_TextureIdStack);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002528 File Offset: 0x00000728
		public unsafe ImVector<Vector2> _Path
		{
			get
			{
				return new ImVector<Vector2>(this.NativePtr->_Path);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000253A File Offset: 0x0000073A
		public unsafe ref ImDrawCmdHeader _CmdHeader
		{
			get
			{
				return Unsafe.AsRef<ImDrawCmdHeader>((void*)(&this.NativePtr->_CmdHeader));
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000082 RID: 130 RVA: 0x0000254D File Offset: 0x0000074D
		public unsafe ref ImDrawListSplitter _Splitter
		{
			get
			{
				return Unsafe.AsRef<ImDrawListSplitter>((void*)(&this.NativePtr->_Splitter));
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002560 File Offset: 0x00000760
		public unsafe ref float _FringeScale
		{
			get
			{
				return Unsafe.AsRef<float>((void*)(&this.NativePtr->_FringeScale));
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002573 File Offset: 0x00000773
		public int _CalcCircleAutoSegmentCount(float radius)
		{
			return ImGuiNative.ImDrawList__CalcCircleAutoSegmentCount(this.NativePtr, radius);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00002581 File Offset: 0x00000781
		public void _ClearFreeMemory()
		{
			ImGuiNative.ImDrawList__ClearFreeMemory(this.NativePtr);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000258E File Offset: 0x0000078E
		public void _OnChangedClipRect()
		{
			ImGuiNative.ImDrawList__OnChangedClipRect(this.NativePtr);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000259B File Offset: 0x0000079B
		public void _OnChangedTextureID()
		{
			ImGuiNative.ImDrawList__OnChangedTextureID(this.NativePtr);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000025A8 File Offset: 0x000007A8
		public void _OnChangedVtxOffset()
		{
			ImGuiNative.ImDrawList__OnChangedVtxOffset(this.NativePtr);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000025B5 File Offset: 0x000007B5
		public void _PathArcToFastEx(Vector2 center, float radius, int a_min_sample, int a_max_sample, int a_step)
		{
			ImGuiNative.ImDrawList__PathArcToFastEx(this.NativePtr, center, radius, a_min_sample, a_max_sample, a_step);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000025C9 File Offset: 0x000007C9
		public void _PathArcToN(Vector2 center, float radius, float a_min, float a_max, int num_segments)
		{
			ImGuiNative.ImDrawList__PathArcToN(this.NativePtr, center, radius, a_min, a_max, num_segments);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000025DD File Offset: 0x000007DD
		public void _PopUnusedDrawCmd()
		{
			ImGuiNative.ImDrawList__PopUnusedDrawCmd(this.NativePtr);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000025EA File Offset: 0x000007EA
		public void _ResetForNewFrame()
		{
			ImGuiNative.ImDrawList__ResetForNewFrame(this.NativePtr);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000025F7 File Offset: 0x000007F7
		public void _TryMergeDrawCmds()
		{
			ImGuiNative.ImDrawList__TryMergeDrawCmds(this.NativePtr);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002604 File Offset: 0x00000804
		public void AddBezierCubic(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness)
		{
			int num_segments = 0;
			ImGuiNative.ImDrawList_AddBezierCubic(this.NativePtr, p1, p2, p3, p4, col, thickness, num_segments);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002628 File Offset: 0x00000828
		public void AddBezierCubic(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int num_segments)
		{
			ImGuiNative.ImDrawList_AddBezierCubic(this.NativePtr, p1, p2, p3, p4, col, thickness, num_segments);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002640 File Offset: 0x00000840
		public void AddBezierQuadratic(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness)
		{
			int num_segments = 0;
			ImGuiNative.ImDrawList_AddBezierQuadratic(this.NativePtr, p1, p2, p3, col, thickness, num_segments);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00002662 File Offset: 0x00000862
		public void AddBezierQuadratic(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int num_segments)
		{
			ImGuiNative.ImDrawList_AddBezierQuadratic(this.NativePtr, p1, p2, p3, col, thickness, num_segments);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002678 File Offset: 0x00000878
		public unsafe void AddCallback(IntPtr callback, IntPtr callback_data)
		{
			void* callback_data2 = callback_data.ToPointer();
			ImGuiNative.ImDrawList_AddCallback(this.NativePtr, callback, callback_data2);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000269C File Offset: 0x0000089C
		public void AddCircle(Vector2 center, float radius, uint col)
		{
			int num_segments = 0;
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddCircle(this.NativePtr, center, radius, col, num_segments, thickness);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000026C4 File Offset: 0x000008C4
		public void AddCircle(Vector2 center, float radius, uint col, int num_segments)
		{
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddCircle(this.NativePtr, center, radius, col, num_segments, thickness);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000026E8 File Offset: 0x000008E8
		public void AddCircle(Vector2 center, float radius, uint col, int num_segments, float thickness)
		{
			ImGuiNative.ImDrawList_AddCircle(this.NativePtr, center, radius, col, num_segments, thickness);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000026FC File Offset: 0x000008FC
		public void AddCircleFilled(Vector2 center, float radius, uint col)
		{
			int num_segments = 0;
			ImGuiNative.ImDrawList_AddCircleFilled(this.NativePtr, center, radius, col, num_segments);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000271A File Offset: 0x0000091A
		public void AddCircleFilled(Vector2 center, float radius, uint col, int num_segments)
		{
			ImGuiNative.ImDrawList_AddCircleFilled(this.NativePtr, center, radius, col, num_segments);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000272C File Offset: 0x0000092C
		public unsafe void AddConvexPolyFilled(ref Vector2 points, int num_points, uint col)
		{
			fixed (Vector2* ptr = &points)
			{
				Vector2* points2 = ptr;
				ImGuiNative.ImDrawList_AddConvexPolyFilled(this.NativePtr, points2, num_points, col);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000274F File Offset: 0x0000094F
		public void AddDrawCmd()
		{
			ImGuiNative.ImDrawList_AddDrawCmd(this.NativePtr);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000275C File Offset: 0x0000095C
		public void AddImage(IntPtr user_texture_id, Vector2 p_min, Vector2 p_max)
		{
			Vector2 uv_min = default(Vector2);
			Vector2 uv_max;
			uv_max..ctor(1f, 1f);
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImage(this.NativePtr, user_texture_id, p_min, p_max, uv_min, uv_max, maxValue);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002798 File Offset: 0x00000998
		public void AddImage(IntPtr user_texture_id, Vector2 p_min, Vector2 p_max, Vector2 uv_min)
		{
			Vector2 uv_max;
			uv_max..ctor(1f, 1f);
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImage(this.NativePtr, user_texture_id, p_min, p_max, uv_min, uv_max, maxValue);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000027CC File Offset: 0x000009CC
		public void AddImage(IntPtr user_texture_id, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max)
		{
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImage(this.NativePtr, user_texture_id, p_min, p_max, uv_min, uv_max, maxValue);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000027EE File Offset: 0x000009EE
		public void AddImage(IntPtr user_texture_id, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col)
		{
			ImGuiNative.ImDrawList_AddImage(this.NativePtr, user_texture_id, p_min, p_max, uv_min, uv_max, col);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002804 File Offset: 0x00000A04
		public void AddImageQuad(IntPtr user_texture_id, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
		{
			Vector2 uv = default(Vector2);
			Vector2 uv2;
			uv2..ctor(1f, 0f);
			Vector2 uv3;
			uv3..ctor(1f, 1f);
			Vector2 uv4;
			uv4..ctor(0f, 1f);
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImageQuad(this.NativePtr, user_texture_id, p1, p2, p3, p4, uv, uv2, uv3, uv4, maxValue);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002868 File Offset: 0x00000A68
		public void AddImageQuad(IntPtr user_texture_id, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1)
		{
			Vector2 uv2;
			uv2..ctor(1f, 0f);
			Vector2 uv3;
			uv3..ctor(1f, 1f);
			Vector2 uv4;
			uv4..ctor(0f, 1f);
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImageQuad(this.NativePtr, user_texture_id, p1, p2, p3, p4, uv1, uv2, uv3, uv4, maxValue);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000028C4 File Offset: 0x00000AC4
		public void AddImageQuad(IntPtr user_texture_id, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2)
		{
			Vector2 uv3;
			uv3..ctor(1f, 1f);
			Vector2 uv4;
			uv4..ctor(0f, 1f);
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImageQuad(this.NativePtr, user_texture_id, p1, p2, p3, p4, uv1, uv2, uv3, uv4, maxValue);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002910 File Offset: 0x00000B10
		public void AddImageQuad(IntPtr user_texture_id, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3)
		{
			Vector2 uv4;
			uv4..ctor(0f, 1f);
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImageQuad(this.NativePtr, user_texture_id, p1, p2, p3, p4, uv1, uv2, uv3, uv4, maxValue);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000294C File Offset: 0x00000B4C
		public void AddImageQuad(IntPtr user_texture_id, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
		{
			uint maxValue = uint.MaxValue;
			ImGuiNative.ImDrawList_AddImageQuad(this.NativePtr, user_texture_id, p1, p2, p3, p4, uv1, uv2, uv3, uv4, maxValue);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00002978 File Offset: 0x00000B78
		public void AddImageQuad(IntPtr user_texture_id, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col)
		{
			ImGuiNative.ImDrawList_AddImageQuad(this.NativePtr, user_texture_id, p1, p2, p3, p4, uv1, uv2, uv3, uv4, col);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000029A4 File Offset: 0x00000BA4
		public void AddImageRounded(IntPtr user_texture_id, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col, float rounding)
		{
			ImDrawFlags flags = ImDrawFlags.None;
			ImGuiNative.ImDrawList_AddImageRounded(this.NativePtr, user_texture_id, p_min, p_max, uv_min, uv_max, col, rounding, flags);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000029CC File Offset: 0x00000BCC
		public void AddImageRounded(IntPtr user_texture_id, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col, float rounding, ImDrawFlags flags)
		{
			ImGuiNative.ImDrawList_AddImageRounded(this.NativePtr, user_texture_id, p_min, p_max, uv_min, uv_max, col, rounding, flags);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000029F4 File Offset: 0x00000BF4
		public void AddLine(Vector2 p1, Vector2 p2, uint col)
		{
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddLine(this.NativePtr, p1, p2, col, thickness);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00002A16 File Offset: 0x00000C16
		public void AddLine(Vector2 p1, Vector2 p2, uint col, float thickness)
		{
			ImGuiNative.ImDrawList_AddLine(this.NativePtr, p1, p2, col, thickness);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002A28 File Offset: 0x00000C28
		public void AddNgon(Vector2 center, float radius, uint col, int num_segments)
		{
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddNgon(this.NativePtr, center, radius, col, num_segments, thickness);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00002A4C File Offset: 0x00000C4C
		public void AddNgon(Vector2 center, float radius, uint col, int num_segments, float thickness)
		{
			ImGuiNative.ImDrawList_AddNgon(this.NativePtr, center, radius, col, num_segments, thickness);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00002A60 File Offset: 0x00000C60
		public void AddNgonFilled(Vector2 center, float radius, uint col, int num_segments)
		{
			ImGuiNative.ImDrawList_AddNgonFilled(this.NativePtr, center, radius, col, num_segments);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002A74 File Offset: 0x00000C74
		public unsafe void AddPolyline(ref Vector2 points, int num_points, uint col, ImDrawFlags flags, float thickness)
		{
			fixed (Vector2* ptr = &points)
			{
				Vector2* points2 = ptr;
				ImGuiNative.ImDrawList_AddPolyline(this.NativePtr, points2, num_points, col, flags, thickness);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00002A9C File Offset: 0x00000C9C
		public void AddQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col)
		{
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddQuad(this.NativePtr, p1, p2, p3, p4, col, thickness);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002AC2 File Offset: 0x00000CC2
		public void AddQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness)
		{
			ImGuiNative.ImDrawList_AddQuad(this.NativePtr, p1, p2, p3, p4, col, thickness);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public void AddQuadFilled(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col)
		{
			ImGuiNative.ImDrawList_AddQuadFilled(this.NativePtr, p1, p2, p3, p4, col);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00002AEC File Offset: 0x00000CEC
		public void AddRect(Vector2 p_min, Vector2 p_max, uint col)
		{
			float rounding = 0f;
			ImDrawFlags flags = ImDrawFlags.None;
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddRect(this.NativePtr, p_min, p_max, col, rounding, flags, thickness);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00002B18 File Offset: 0x00000D18
		public void AddRect(Vector2 p_min, Vector2 p_max, uint col, float rounding)
		{
			ImDrawFlags flags = ImDrawFlags.None;
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddRect(this.NativePtr, p_min, p_max, col, rounding, flags, thickness);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002B40 File Offset: 0x00000D40
		public void AddRect(Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags)
		{
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddRect(this.NativePtr, p_min, p_max, col, rounding, flags, thickness);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00002B66 File Offset: 0x00000D66
		public void AddRect(Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags, float thickness)
		{
			ImGuiNative.ImDrawList_AddRect(this.NativePtr, p_min, p_max, col, rounding, flags, thickness);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00002B7C File Offset: 0x00000D7C
		public void AddRectFilled(Vector2 p_min, Vector2 p_max, uint col)
		{
			float rounding = 0f;
			ImDrawFlags flags = ImDrawFlags.None;
			ImGuiNative.ImDrawList_AddRectFilled(this.NativePtr, p_min, p_max, col, rounding, flags);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002BA4 File Offset: 0x00000DA4
		public void AddRectFilled(Vector2 p_min, Vector2 p_max, uint col, float rounding)
		{
			ImDrawFlags flags = ImDrawFlags.None;
			ImGuiNative.ImDrawList_AddRectFilled(this.NativePtr, p_min, p_max, col, rounding, flags);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public void AddRectFilled(Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags)
		{
			ImGuiNative.ImDrawList_AddRectFilled(this.NativePtr, p_min, p_max, col, rounding, flags);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public void AddRectFilledMultiColor(Vector2 p_min, Vector2 p_max, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left)
		{
			ImGuiNative.ImDrawList_AddRectFilledMultiColor(this.NativePtr, p_min, p_max, col_upr_left, col_upr_right, col_bot_right, col_bot_left);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public void AddTriangle(Vector2 p1, Vector2 p2, Vector2 p3, uint col)
		{
			float thickness = 1f;
			ImGuiNative.ImDrawList_AddTriangle(this.NativePtr, p1, p2, p3, col, thickness);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00002C14 File Offset: 0x00000E14
		public void AddTriangle(Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness)
		{
			ImGuiNative.ImDrawList_AddTriangle(this.NativePtr, p1, p2, p3, col, thickness);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00002C28 File Offset: 0x00000E28
		public void AddTriangleFilled(Vector2 p1, Vector2 p2, Vector2 p3, uint col)
		{
			ImGuiNative.ImDrawList_AddTriangleFilled(this.NativePtr, p1, p2, p3, col);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00002C3A File Offset: 0x00000E3A
		public void ChannelsMerge()
		{
			ImGuiNative.ImDrawList_ChannelsMerge(this.NativePtr);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00002C47 File Offset: 0x00000E47
		public void ChannelsSetCurrent(int n)
		{
			ImGuiNative.ImDrawList_ChannelsSetCurrent(this.NativePtr, n);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00002C55 File Offset: 0x00000E55
		public void ChannelsSplit(int count)
		{
			ImGuiNative.ImDrawList_ChannelsSplit(this.NativePtr, count);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002C63 File Offset: 0x00000E63
		public ImDrawListPtr CloneOutput()
		{
			return new ImDrawListPtr(ImGuiNative.ImDrawList_CloneOutput(this.NativePtr));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00002C75 File Offset: 0x00000E75
		public void Destroy()
		{
			ImGuiNative.ImDrawList_destroy(this.NativePtr);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00002C84 File Offset: 0x00000E84
		public unsafe Vector2 GetClipRectMax()
		{
			Vector2 result;
			ImGuiNative.ImDrawList_GetClipRectMax(&result, this.NativePtr);
			return result;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public unsafe Vector2 GetClipRectMin()
		{
			Vector2 result;
			ImGuiNative.ImDrawList_GetClipRectMin(&result, this.NativePtr);
			return result;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00002CBC File Offset: 0x00000EBC
		public void PathArcTo(Vector2 center, float radius, float a_min, float a_max)
		{
			int num_segments = 0;
			ImGuiNative.ImDrawList_PathArcTo(this.NativePtr, center, radius, a_min, a_max, num_segments);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00002CDC File Offset: 0x00000EDC
		public void PathArcTo(Vector2 center, float radius, float a_min, float a_max, int num_segments)
		{
			ImGuiNative.ImDrawList_PathArcTo(this.NativePtr, center, radius, a_min, a_max, num_segments);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public void PathArcToFast(Vector2 center, float radius, int a_min_of_12, int a_max_of_12)
		{
			ImGuiNative.ImDrawList_PathArcToFast(this.NativePtr, center, radius, a_min_of_12, a_max_of_12);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00002D04 File Offset: 0x00000F04
		public void PathBezierCubicCurveTo(Vector2 p2, Vector2 p3, Vector2 p4)
		{
			int num_segments = 0;
			ImGuiNative.ImDrawList_PathBezierCubicCurveTo(this.NativePtr, p2, p3, p4, num_segments);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00002D22 File Offset: 0x00000F22
		public void PathBezierCubicCurveTo(Vector2 p2, Vector2 p3, Vector2 p4, int num_segments)
		{
			ImGuiNative.ImDrawList_PathBezierCubicCurveTo(this.NativePtr, p2, p3, p4, num_segments);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00002D34 File Offset: 0x00000F34
		public void PathBezierQuadraticCurveTo(Vector2 p2, Vector2 p3)
		{
			int num_segments = 0;
			ImGuiNative.ImDrawList_PathBezierQuadraticCurveTo(this.NativePtr, p2, p3, num_segments);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00002D51 File Offset: 0x00000F51
		public void PathBezierQuadraticCurveTo(Vector2 p2, Vector2 p3, int num_segments)
		{
			ImGuiNative.ImDrawList_PathBezierQuadraticCurveTo(this.NativePtr, p2, p3, num_segments);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00002D61 File Offset: 0x00000F61
		public void PathClear()
		{
			ImGuiNative.ImDrawList_PathClear(this.NativePtr);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00002D6E File Offset: 0x00000F6E
		public void PathFillConvex(uint col)
		{
			ImGuiNative.ImDrawList_PathFillConvex(this.NativePtr, col);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002D7C File Offset: 0x00000F7C
		public void PathLineTo(Vector2 pos)
		{
			ImGuiNative.ImDrawList_PathLineTo(this.NativePtr, pos);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002D8A File Offset: 0x00000F8A
		public void PathLineToMergeDuplicate(Vector2 pos)
		{
			ImGuiNative.ImDrawList_PathLineToMergeDuplicate(this.NativePtr, pos);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00002D98 File Offset: 0x00000F98
		public void PathRect(Vector2 rect_min, Vector2 rect_max)
		{
			float rounding = 0f;
			ImDrawFlags flags = ImDrawFlags.None;
			ImGuiNative.ImDrawList_PathRect(this.NativePtr, rect_min, rect_max, rounding, flags);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00002DBC File Offset: 0x00000FBC
		public void PathRect(Vector2 rect_min, Vector2 rect_max, float rounding)
		{
			ImDrawFlags flags = ImDrawFlags.None;
			ImGuiNative.ImDrawList_PathRect(this.NativePtr, rect_min, rect_max, rounding, flags);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002DDA File Offset: 0x00000FDA
		public void PathRect(Vector2 rect_min, Vector2 rect_max, float rounding, ImDrawFlags flags)
		{
			ImGuiNative.ImDrawList_PathRect(this.NativePtr, rect_min, rect_max, rounding, flags);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002DEC File Offset: 0x00000FEC
		public void PathStroke(uint col)
		{
			ImDrawFlags flags = ImDrawFlags.None;
			float thickness = 1f;
			ImGuiNative.ImDrawList_PathStroke(this.NativePtr, col, flags, thickness);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00002E10 File Offset: 0x00001010
		public void PathStroke(uint col, ImDrawFlags flags)
		{
			float thickness = 1f;
			ImGuiNative.ImDrawList_PathStroke(this.NativePtr, col, flags, thickness);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00002E31 File Offset: 0x00001031
		public void PathStroke(uint col, ImDrawFlags flags, float thickness)
		{
			ImGuiNative.ImDrawList_PathStroke(this.NativePtr, col, flags, thickness);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00002E41 File Offset: 0x00001041
		public void PopClipRect()
		{
			ImGuiNative.ImDrawList_PopClipRect(this.NativePtr);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00002E4E File Offset: 0x0000104E
		public void PopTextureID()
		{
			ImGuiNative.ImDrawList_PopTextureID(this.NativePtr);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00002E5C File Offset: 0x0000105C
		public void PrimQuadUV(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col)
		{
			ImGuiNative.ImDrawList_PrimQuadUV(this.NativePtr, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00002E83 File Offset: 0x00001083
		public void PrimRect(Vector2 a, Vector2 b, uint col)
		{
			ImGuiNative.ImDrawList_PrimRect(this.NativePtr, a, b, col);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00002E93 File Offset: 0x00001093
		public void PrimRectUV(Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col)
		{
			ImGuiNative.ImDrawList_PrimRectUV(this.NativePtr, a, b, uv_a, uv_b, col);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00002EA7 File Offset: 0x000010A7
		public void PrimReserve(int idx_count, int vtx_count)
		{
			ImGuiNative.ImDrawList_PrimReserve(this.NativePtr, idx_count, vtx_count);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00002EB6 File Offset: 0x000010B6
		public void PrimUnreserve(int idx_count, int vtx_count)
		{
			ImGuiNative.ImDrawList_PrimUnreserve(this.NativePtr, idx_count, vtx_count);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00002EC5 File Offset: 0x000010C5
		public void PrimVtx(Vector2 pos, Vector2 uv, uint col)
		{
			ImGuiNative.ImDrawList_PrimVtx(this.NativePtr, pos, uv, col);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00002ED5 File Offset: 0x000010D5
		public void PrimWriteIdx(ushort idx)
		{
			ImGuiNative.ImDrawList_PrimWriteIdx(this.NativePtr, idx);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00002EE3 File Offset: 0x000010E3
		public void PrimWriteVtx(Vector2 pos, Vector2 uv, uint col)
		{
			ImGuiNative.ImDrawList_PrimWriteVtx(this.NativePtr, pos, uv, col);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00002EF4 File Offset: 0x000010F4
		public void PushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max)
		{
			byte intersect_with_current_clip_rect = 0;
			ImGuiNative.ImDrawList_PushClipRect(this.NativePtr, clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00002F14 File Offset: 0x00001114
		public void PushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, bool intersect_with_current_clip_rect)
		{
			byte intersect_with_current_clip_rect2 = (intersect_with_current_clip_rect > false) ? 1 : 0;
			ImGuiNative.ImDrawList_PushClipRect(this.NativePtr, clip_rect_min, clip_rect_max, intersect_with_current_clip_rect2);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00002F35 File Offset: 0x00001135
		public void PushClipRectFullScreen()
		{
			ImGuiNative.ImDrawList_PushClipRectFullScreen(this.NativePtr);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00002F42 File Offset: 0x00001142
		public void PushTextureID(IntPtr texture_id)
		{
			ImGuiNative.ImDrawList_PushTextureID(this.NativePtr, texture_id);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00002F50 File Offset: 0x00001150
		public unsafe void AddText(Vector2 pos, uint col, ReadOnlySpan<char> text_begin)
		{
			int byteCount = Encoding.UTF8.GetByteCount(text_begin);
			byte* ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			fixed (char* pinnableReference = text_begin.GetPinnableReference())
			{
				char* ptr2 = pinnableReference;
				int bytes = Encoding.UTF8.GetBytes(ptr2, text_begin.Length, ptr, byteCount);
				ptr[bytes] = 0;
			}
			byte* text_end = null;
			ImGuiNative.ImDrawList_AddText_Vec2(this.NativePtr, pos, col, ptr, text_end);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00002FB0 File Offset: 0x000011B0
		public unsafe void AddText(ImFontPtr font, float font_size, Vector2 pos, uint col, ReadOnlySpan<char> text_begin)
		{
			ImFont* nativePtr = font.NativePtr;
			int byteCount = Encoding.UTF8.GetByteCount(text_begin);
			byte* ptr = stackalloc byte[(UIntPtr)(byteCount + 1)];
			fixed (char* pinnableReference = text_begin.GetPinnableReference())
			{
				char* ptr2 = pinnableReference;
				int bytes = Encoding.UTF8.GetBytes(ptr2, text_begin.Length, ptr, byteCount);
				ptr[bytes] = 0;
			}
			byte* text_end = null;
			float wrap_width = 0f;
			Vector4* cpu_fine_clip_rect = null;
			ImGuiNative.ImDrawList_AddText_FontPtr(this.NativePtr, nativePtr, font_size, pos, col, ptr, text_end, wrap_width, cpu_fine_clip_rect);
		}
	}
}
