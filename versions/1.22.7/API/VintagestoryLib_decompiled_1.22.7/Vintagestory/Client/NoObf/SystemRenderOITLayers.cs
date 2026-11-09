using System;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL;
using Vintagestory.API.Client;

namespace Vintagestory.Client.NoObf;

public class SystemRenderOITLayers : ClientSystem
{
	public class BeforeOIT : IRenderer, IDisposable
	{
		private ICoreClientAPI capi;

		private FrameBufferRef transparentfb;

		public double RenderOrder => 0.0;

		public int RenderRange => 1;

		public BeforeOIT(ICoreClientAPI capi)
		{
			this.capi = capi;
			transparentfb = capi.Render.FrameBuffers[1];
		}

		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			IShaderProgram currentActiveShader = capi.Render.CurrentActiveShader;
			currentActiveShader?.Stop();
			IShaderProgram program = capi.Shader.GetProgram(9);
			program.Use();
			program.Uniform("OITreveal", 6);
			GL.Uniform1(GL.GetUniformLocation(program.ProgramId, "OITaccumulation"), 7);
			program.Stop();
			IShaderProgram programByName = capi.Shader.GetProgramByName("cloudvolumetric");
			programByName.Use();
			programByName.Uniform("liquidDepth", 4);
			programByName.Stop();
			currentActiveShader?.Use();
			if (revealTextureId == 0 || transparentfb.Disposed)
			{
				rebuild();
			}
			DrawBuffersEnum[] array = new DrawBuffersEnum[6];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			DrawBuffersEnum[] buffers = (DrawBuffersEnum[])(object)array;
			GL.DrawBuffers(buffers.Length, buffers);
			GL.BlendFunc(0, (BlendingFactorSrc)774, (BlendingFactorDest)0);
			GL.BlendFunc(1, (BlendingFactorSrc)774, (BlendingFactorDest)0);
			GL.BlendFunc(3, (BlendingFactorSrc)1, (BlendingFactorDest)1);
			GL.BlendFunc(4, (BlendingFactorSrc)1, (BlendingFactorDest)1);
			GL.BlendFunc(5, (BlendingFactorSrc)1, (BlendingFactorDest)1);
			float[] zeros = new float[4];
			float[] ones = new float[4] { 1f, 1f, 1f, 1f };
			GL.ClearBuffer((ClearBuffer)6144, 0, ones);
			GL.ClearBuffer((ClearBuffer)6144, 1, ones);
			GL.ClearBuffer((ClearBuffer)6144, 3, zeros);
			GL.ClearBuffer((ClearBuffer)6144, 4, zeros);
			GL.ClearBuffer((ClearBuffer)6144, 5, zeros);
		}

		private void freeResources()
		{
			GL.DeleteTexture(accumTextureId);
			GL.DeleteTexture(revealTextureId);
			accumTextureId = 0;
			revealTextureId = 0;
		}

		public void Dispose()
		{
			freeResources();
		}

		private void rebuild()
		{
			freeResources();
			transparentfb = capi.Render.FrameBuffers[1];
			int width = transparentfb.Width;
			int height = transparentfb.Height;
			GL.BindFramebuffer((FramebufferTarget)36160, transparentfb.FboId);
			revealTextureId = GL.GenTexture();
			GL.BindTexture((TextureTarget)3553, revealTextureId);
			GL.TexImage2D((TextureTarget)3553, 0, (PixelInternalFormat)32849, width, height, 0, (PixelFormat)6407, (PixelType)5121, (IntPtr)0);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9728);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9728);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33071);
			GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33071);
			accumTextureId = GL.GenTexture();
			GL.BindTexture((TextureTarget)35866, accumTextureId);
			GL.TexImage3D((TextureTarget)35866, 0, (PixelInternalFormat)34842, width, height, 3, 0, (PixelFormat)6408, (PixelType)5126, (IntPtr)0);
			GL.TexParameter((TextureTarget)35866, (TextureParameterName)10241, 9728);
			GL.TexParameter((TextureTarget)35866, (TextureParameterName)10240, 9728);
			GL.TexParameter((TextureTarget)35866, (TextureParameterName)10242, 33071);
			GL.TexParameter((TextureTarget)35866, (TextureParameterName)10243, 33071);
			GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36064, (TextureTarget)3553, revealTextureId, 0);
			GL.FramebufferTextureLayer((FramebufferTarget)36160, (FramebufferAttachment)36067, accumTextureId, 0, 0);
			GL.FramebufferTextureLayer((FramebufferTarget)36160, (FramebufferAttachment)36068, accumTextureId, 0, 1);
			GL.FramebufferTextureLayer((FramebufferTarget)36160, (FramebufferAttachment)36069, accumTextureId, 0, 2);
		}
	}

	public class AfterOIT : IRenderer, IDisposable
	{
		public double RenderOrder => 1.0;

		public int RenderRange => 1;

		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			GL.ActiveTexture((TextureUnit)33990);
			GL.BindTexture((TextureTarget)3553, revealTextureId);
			GL.ActiveTexture((TextureUnit)33991);
			GL.BindTexture((TextureTarget)35866, accumTextureId);
			GL.ActiveTexture((TextureUnit)33984);
		}

		public void Dispose()
		{
		}
	}

	private const int layers = 3;

	private static int accumTextureId;

	private static int revealTextureId;

	public override string Name => "oitlayers";

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Render;
	}

	public SystemRenderOITLayers(ClientMain game)
		: base(game)
	{
		game.api.Event.RegisterRenderer(new BeforeOIT(game.api), EnumRenderStage.OIT, "beforeoit");
		game.api.Event.RegisterRenderer(new AfterOIT(), EnumRenderStage.OIT, "afteroit");
	}
}
