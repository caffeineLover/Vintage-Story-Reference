using System;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

namespace ImGuiController_OpenTK
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	internal struct SaveGLState : IDisposable
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00003784 File Offset: 0x00001984
		public SaveGLState(int GLVersion, bool CompatibilityProfile)
		{
			this.disposedValue = false;
			this.prevVAO = GL.GetInteger(34229);
			this.prevArrayBuffer = GL.GetInteger(34964);
			this.prevProgram = GL.GetInteger(35725);
			this.prevBlendEnabled = GL.GetBoolean(3042);
			this.prevScissorTestEnabled = GL.GetBoolean(3089);
			this.prevBlendEquationRgb = GL.GetInteger(32777);
			this.prevBlendEquationAlpha = GL.GetInteger(34877);
			this.prevBlendFuncSrcRgb = GL.GetInteger(32969);
			this.prevBlendFuncSrcAlpha = GL.GetInteger(32971);
			this.prevBlendFuncDstRgb = GL.GetInteger(32968);
			this.prevBlendFuncDstAlpha = GL.GetInteger(32970);
			this.prevCullFaceEnabled = GL.GetBoolean(2884);
			this.prevDepthTestEnabled = GL.GetBoolean(2929);
			this.prevActiveTexture = GL.GetInteger(34016);
			this.prevTexture2D = GL.GetInteger(32873);
			this.prevScissorBox = SaveGLState.GetScissorBox();
			this.prevPolygonMode = SaveGLState.GetPolygonMode();
			this.GLVersion = GLVersion;
			this.CompatibilityProfile = CompatibilityProfile;
			GL.ActiveTexture(33984);
			if (GLVersion <= 310 || CompatibilityProfile)
			{
				GL.PolygonMode(1028, 6914);
				GL.PolygonMode(1029, 6914);
				return;
			}
			GL.PolygonMode(1032, 6914);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000038F4 File Offset: 0x00001AF4
		private unsafe static int[] GetScissorBox()
		{
			Span<int> prevScissorBoxPtr = new Span<int>(stackalloc byte[(UIntPtr)16], 4);
			fixed (int* ptr = prevScissorBoxPtr[0])
			{
				int* iptr = ptr;
				GL.GetInteger(3088, iptr);
			}
			return prevScissorBoxPtr.ToArray();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003930 File Offset: 0x00001B30
		private unsafe static int[] GetPolygonMode()
		{
			Span<int> prevPolygonModePtr = new Span<int>(stackalloc byte[(UIntPtr)8], 2);
			fixed (int* ptr = prevPolygonModePtr[0])
			{
				int* iptr = ptr;
				GL.GetInteger(2880, iptr);
			}
			return prevPolygonModePtr.ToArray();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000396C File Offset: 0x00001B6C
		public void Dispose()
		{
			if (this.disposedValue)
			{
				return;
			}
			this.disposedValue = true;
			GC.SuppressFinalize(this);
			GL.BindTexture(3553, this.prevTexture2D);
			GL.ActiveTexture(this.prevActiveTexture);
			GL.UseProgram(this.prevProgram);
			GL.BindVertexArray(this.prevVAO);
			GL.Scissor(this.prevScissorBox[0], this.prevScissorBox[1], this.prevScissorBox[2], this.prevScissorBox[3]);
			GL.BindBuffer(34962, this.prevArrayBuffer);
			GL.BlendEquationSeparate(this.prevBlendEquationRgb, this.prevBlendEquationAlpha);
			GL.BlendFuncSeparate(this.prevBlendFuncSrcRgb, this.prevBlendFuncDstRgb, this.prevBlendFuncSrcAlpha, this.prevBlendFuncDstAlpha);
			if (this.prevBlendEnabled)
			{
				GL.Enable(3042);
			}
			else
			{
				GL.Disable(3042);
			}
			if (this.prevDepthTestEnabled)
			{
				GL.Enable(2929);
			}
			else
			{
				GL.Disable(2929);
			}
			if (this.prevCullFaceEnabled)
			{
				GL.Enable(2884);
			}
			else
			{
				GL.Disable(2884);
			}
			if (this.prevScissorTestEnabled)
			{
				GL.Enable(3089);
			}
			else
			{
				GL.Disable(3089);
			}
			if (this.GLVersion <= 310 || this.CompatibilityProfile)
			{
				GL.PolygonMode(1028, this.prevPolygonMode[0]);
				GL.PolygonMode(1029, this.prevPolygonMode[1]);
				return;
			}
			GL.PolygonMode(1032, this.prevPolygonMode[0]);
		}

		// Token: 0x0400001C RID: 28
		private bool disposedValue;

		// Token: 0x0400001D RID: 29
		private readonly int prevVAO;

		// Token: 0x0400001E RID: 30
		private readonly int prevArrayBuffer;

		// Token: 0x0400001F RID: 31
		private readonly int prevProgram;

		// Token: 0x04000020 RID: 32
		private readonly bool prevBlendEnabled;

		// Token: 0x04000021 RID: 33
		private readonly bool prevScissorTestEnabled;

		// Token: 0x04000022 RID: 34
		private readonly int prevBlendEquationRgb;

		// Token: 0x04000023 RID: 35
		private readonly int prevBlendEquationAlpha;

		// Token: 0x04000024 RID: 36
		private readonly int prevBlendFuncSrcRgb;

		// Token: 0x04000025 RID: 37
		private readonly int prevBlendFuncSrcAlpha;

		// Token: 0x04000026 RID: 38
		private readonly int prevBlendFuncDstRgb;

		// Token: 0x04000027 RID: 39
		private readonly int prevBlendFuncDstAlpha;

		// Token: 0x04000028 RID: 40
		private readonly bool prevCullFaceEnabled;

		// Token: 0x04000029 RID: 41
		private readonly bool prevDepthTestEnabled;

		// Token: 0x0400002A RID: 42
		private readonly int prevActiveTexture;

		// Token: 0x0400002B RID: 43
		private readonly int prevTexture2D;

		// Token: 0x0400002C RID: 44
		private readonly int[] prevScissorBox;

		// Token: 0x0400002D RID: 45
		private readonly int[] prevPolygonMode;

		// Token: 0x0400002E RID: 46
		private readonly int GLVersion;

		// Token: 0x0400002F RID: 47
		private readonly bool CompatibilityProfile;
	}
}
