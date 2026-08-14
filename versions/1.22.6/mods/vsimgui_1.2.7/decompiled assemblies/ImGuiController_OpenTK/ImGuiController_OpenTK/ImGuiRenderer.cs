using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace ImGuiController_OpenTK
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImGuiRenderer : IImGuiRenderer, IDisposable
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002D90 File Offset: 0x00000F90
		public ImGuiRenderer(ImGuiViewportPtr viewport, NativeWindow mainWindow)
		{
			int major = GL.GetInteger(33307);
			int minor = GL.GetInteger(33308);
			this.GLVersion = major * 100 + minor * 10;
			this.CompatibilityProfile = ((GL.GetInteger(37158) & 2) != 0);
			this.mMain = true;
			this.mViewport = viewport;
			this.mWindow = mainWindow;
			this.CreateDeviceNotSharedResources(mainWindow.Context);
			this.CreateDeviceSharedResources(mainWindow.Context);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002E0C File Offset: 0x0000100C
		public ImGuiRenderer(ImGuiRenderer mainRenderer, ImGuiViewportPtr viewport, NativeWindow secondaryWindow)
		{
			int major = GL.GetInteger(33307);
			int minor = GL.GetInteger(33308);
			this.GLVersion = major * 100 + minor * 10;
			this.CompatibilityProfile = ((GL.GetInteger(37158) & 2) != 0);
			this.mViewport = viewport;
			this.mWindow = secondaryWindow;
			this.mFontTexture = mainRenderer.mFontTexture;
			this.mShader = mainRenderer.mShader;
			this.mShaderFontTextureLocation = mainRenderer.mShaderFontTextureLocation;
			this.mShaderProjectionMatrixLocation = mainRenderer.mShaderProjectionMatrixLocation;
			this.CreateDeviceNotSharedResources(secondaryWindow.Context);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002EA4 File Offset: 0x000010A4
		public void Render()
		{
			using (new SaveGLState(this.GLVersion, this.CompatibilityProfile))
			{
				this.RenderImDrawData(this.mViewport.DrawData, this.mWindow);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002F00 File Offset: 0x00001100
		public void Dispose()
		{
			if (this.disposedValue)
			{
				return;
			}
			this.disposedValue = true;
			GL.DeleteVertexArray(this.mVertexArray);
			GL.DeleteBuffer(this.mVertexBuffer);
			GL.DeleteBuffer(this.mIndexBuffer);
			if (!this.mMain)
			{
				return;
			}
			GL.DeleteTexture(this.mFontTexture);
			GL.DeleteProgram(this.mShader);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002F64 File Offset: 0x00001164
		private void CreateDeviceNotSharedResources(IGLFWGraphicsContext context)
		{
			context.MakeCurrent();
			this.mVertexBufferSize = 10000;
			this.mIndexBufferSize = 2000;
			int integer = GL.GetInteger(34229);
			int prevArrayBuffer = GL.GetInteger(34964);
			this.mVertexArray = GL.GenVertexArray();
			GL.BindVertexArray(this.mVertexArray);
			this.mVertexBuffer = GL.GenBuffer();
			GL.BindBuffer(34962, this.mVertexBuffer);
			GL.BufferData(34962, this.mVertexBufferSize, IntPtr.Zero, 35048);
			this.mIndexBuffer = GL.GenBuffer();
			GL.BindBuffer(34963, this.mIndexBuffer);
			GL.BufferData(34963, this.mIndexBufferSize, IntPtr.Zero, 35048);
			int stride = Unsafe.SizeOf<ImDrawVert>();
			GL.VertexAttribPointer(0, 2, 5126, false, stride, 0);
			GL.VertexAttribPointer(1, 2, 5126, false, stride, 8);
			GL.VertexAttribPointer(2, 4, 5121, true, stride, 16);
			GL.EnableVertexAttribArray(0);
			GL.EnableVertexAttribArray(1);
			GL.EnableVertexAttribArray(2);
			GL.BindVertexArray(integer);
			GL.BindBuffer(34962, prevArrayBuffer);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003078 File Offset: 0x00001278
		private void CreateDeviceSharedResources(IGLFWGraphicsContext context)
		{
			context.MakeCurrent();
			this.RecreateFontDeviceTexture();
			ValueTuple<int, int, int> valueTuple = ImGuiShader.Create();
			this.mShader = valueTuple.Item1;
			this.mShaderProjectionMatrixLocation = valueTuple.Item2;
			this.mShaderFontTextureLocation = valueTuple.Item3;
			ImGuiRenderer.CheckGLError("End of ImGui setup");
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000030C8 File Offset: 0x000012C8
		private void RecreateFontDeviceTexture()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			IntPtr pixels;
			int width;
			int height;
			int bytesPerPixel;
			io.Fonts.GetTexDataAsRGBA32(ref pixels, ref width, ref height, ref bytesPerPixel);
			int mips = (int)Math.Floor(Math.Log((double)Math.Max(width, height), 2.0));
			TextureUnit integer = GL.GetInteger(34016);
			GL.ActiveTexture(33984);
			int prevTexture2D = GL.GetInteger(32873);
			this.mFontTexture = GL.GenTexture();
			GL.BindTexture(3553, this.mFontTexture);
			GL.TexStorage2D(3553, mips, 32856, width, height);
			GL.TexSubImage2D(3553, 0, 0, 0, width, height, 32993, 5121, pixels);
			GL.GenerateMipmap(3553);
			GL.TexParameter(3553, 10242, 10497);
			GL.TexParameter(3553, 10243, 10497);
			GL.TexParameter(3553, 33085, mips - 1);
			GL.TexParameter(3553, 10240, 9729);
			GL.TexParameter(3553, 10241, 9729);
			GL.BindTexture(3553, prevTexture2D);
			GL.ActiveTexture(integer);
			io.Fonts.SetTexID((IntPtr)this.mFontTexture);
			io.Fonts.ClearTexData();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003220 File Offset: 0x00001420
		private unsafe void RenderImDrawData(ImDrawDataPtr drawData, NativeWindow window)
		{
			if (*drawData.CmdListsCount == 0)
			{
				return;
			}
			this.ResizeBuffers(drawData);
			this.SetupOrthographicProjection(window, drawData);
			ImGuiIOPtr io = ImGui.GetIO();
			drawData.ScaleClipRects(*io.DisplayFramebufferScale);
			GL.Enable(3042);
			GL.Enable(3089);
			GL.BlendEquation(32774);
			GL.BlendFunc(770, 771);
			GL.Disable(2884);
			GL.Disable(2929);
			for (int commandListIndex = 0; commandListIndex < *drawData.CmdListsCount; commandListIndex++)
			{
				this.RenderCommandList(drawData, window, commandListIndex, io);
			}
			GL.Disable(3042);
			GL.Disable(3089);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000032D4 File Offset: 0x000014D4
		private unsafe void ResizeBuffers(ImDrawDataPtr drawData)
		{
			GL.BindVertexArray(this.mVertexArray);
			GL.BindBuffer(34962, this.mVertexBuffer);
			for (int i = 0; i < *drawData.CmdListsCount; i++)
			{
				ImDrawListPtr cmd_list = *drawData.CmdLists[i];
				int vertexSize = cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>();
				if (vertexSize > this.mVertexBufferSize)
				{
					int newSize = (int)Math.Max((float)this.mVertexBufferSize * 1.5f, (float)vertexSize);
					GL.BufferData(34962, newSize, IntPtr.Zero, 35048);
					this.mVertexBufferSize = newSize;
				}
				int indexSize = cmd_list.IdxBuffer.Size * 2;
				if (indexSize > this.mIndexBufferSize)
				{
					int newSize2 = (int)Math.Max((float)this.mIndexBufferSize * 1.5f, (float)indexSize);
					GL.BufferData(34963, newSize2, IntPtr.Zero, 35048);
					this.mIndexBufferSize = newSize2;
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000033CC File Offset: 0x000015CC
		private unsafe void RenderCommandList(ImDrawDataPtr drawData, NativeWindow window, int index, ImGuiIOPtr io)
		{
			ImDrawListPtr commandList = *drawData.CmdLists[index];
			GL.BufferSubData(34962, IntPtr.Zero, commandList.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), commandList.VtxBuffer.Data);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(10, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Data Vert ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			ImGuiRenderer.CheckGLError(defaultInterpolatedStringHandler.ToStringAndClear());
			GL.BufferSubData(34963, IntPtr.Zero, commandList.IdxBuffer.Size * 2, commandList.IdxBuffer.Data);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(9, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Data Idx ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(index);
			ImGuiRenderer.CheckGLError(defaultInterpolatedStringHandler2.ToStringAndClear());
			for (int cmd_i = 0; cmd_i < commandList.CmdBuffer.Size; cmd_i++)
			{
				ImDrawCmdPtr command = commandList.CmdBuffer[cmd_i];
				if (*command.UserCallback != IntPtr.Zero)
				{
					throw new NotImplementedException();
				}
				GL.ActiveTexture(33984);
				GL.BindTexture(3553, (int)(*command.TextureId));
				ImGuiRenderer.CheckGLError("Texture");
				Vector4 clip = *command.ClipRect;
				ImGuiRenderer.SetupScissor(window, clip);
				if ((*io.BackendFlags & 8) != 0)
				{
					GL.DrawElementsBaseVertex(4, (int)(*command.ElemCount), 5123, (IntPtr)((UIntPtr)(*command.IdxOffset * 2U)), (int)(*command.VtxOffset));
				}
				else
				{
					GL.DrawElements(4, (int)(*command.ElemCount), 5123, (int)(*command.IdxOffset * 2U));
				}
				ImGuiRenderer.CheckGLError("Draw");
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000357C File Offset: 0x0000177C
		private static void SetupScissor(NativeWindow window, Vector4 clip)
		{
			int posX = window.ClientLocation.X;
			int num = window.ClientLocation.Y + window.ClientSize.Y;
			int x = (int)clip.X - posX;
			int y = num - (int)clip.W;
			int width = (int)(clip.Z - clip.X);
			int height = (int)(clip.W - clip.Y);
			GL.Scissor(x, y, width, height);
			ImGuiRenderer.CheckGLError("Scissor");
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000035F4 File Offset: 0x000017F4
		private void SetupOrthographicProjection(NativeWindow window, ImDrawDataPtr drawData)
		{
			Matrix4 mvp = Matrix4.CreateOrthographicOffCenter((float)window.ClientLocation.X, (float)window.ClientLocation.X + drawData.DisplaySize.X, (float)window.ClientLocation.Y + drawData.DisplaySize.Y, (float)window.ClientLocation.Y, -1f, 1f);
			GL.UseProgram(this.mShader);
			GL.UniformMatrix4(this.mShaderProjectionMatrixLocation, false, ref mvp);
			GL.Uniform1(this.mShaderFontTextureLocation, 0);
			ImGuiRenderer.CheckGLError("Projection");
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000368A File Offset: 0x0000188A
		private static void CheckGLError(string title)
		{
			while (GL.GetError() != null)
			{
			}
		}

		// Token: 0x0400000B RID: 11
		public readonly int GLVersion;

		// Token: 0x0400000C RID: 12
		public readonly bool CompatibilityProfile;

		// Token: 0x0400000D RID: 13
		private bool disposedValue;

		// Token: 0x0400000E RID: 14
		private readonly ImGuiViewportPtr mViewport;

		// Token: 0x0400000F RID: 15
		private readonly NativeWindow mWindow;

		// Token: 0x04000010 RID: 16
		private readonly bool mMain;

		// Token: 0x04000011 RID: 17
		private int mVertexArray;

		// Token: 0x04000012 RID: 18
		private int mVertexBuffer;

		// Token: 0x04000013 RID: 19
		private int mVertexBufferSize;

		// Token: 0x04000014 RID: 20
		private int mIndexBuffer;

		// Token: 0x04000015 RID: 21
		private int mIndexBufferSize;

		// Token: 0x04000016 RID: 22
		private int mFontTexture;

		// Token: 0x04000017 RID: 23
		private int mShader;

		// Token: 0x04000018 RID: 24
		private int mShaderFontTextureLocation;

		// Token: 0x04000019 RID: 25
		private int mShaderProjectionMatrixLocation;
	}
}
