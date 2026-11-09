using System;
using OpenTK.Graphics.OpenGL;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace FluffyClouds;

public class CloudRendererVolumetric : IRenderer, IDisposable
{
	private ICoreClientAPI capi;

	private ModSystem mod;

	private CloudRendererMap map;

	private IShaderProgram program;

	private MeshRef quad;

	private Matrixf matrix = new Matrixf();

	private int frame;

	private float time;

	public double RenderOrder => 0.31;

	public int RenderRange => 1;

	public CloudRendererVolumetric(ModSystem mod, ICoreClientAPI capi, CloudRendererMap map)
	{
		this.mod = mod;
		this.capi = capi;
		this.map = map;
		capi.Event.ReloadShader += LoadShader;
		LoadShader();
		quad = capi.Render.UploadMesh(QuadMeshUtil.GetQuad());
	}

	public bool LoadShader()
	{
		program = capi.Shader.NewShaderProgram();
		program.VertexShader = capi.Shader.NewShader(EnumShaderType.VertexShader);
		program.FragmentShader = capi.Shader.NewShader(EnumShaderType.FragmentShader);
		program.AssetDomain = mod.Mod.Info.ModID;
		capi.Shader.RegisterFileShaderProgram("cloudvolumetric", program);
		return program.Compile();
	}

	public void Dispose()
	{
		capi.Render.DeleteMesh(quad);
		program?.Dispose();
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		program.Use();
		matrix.Set(capi.Render.PerspectiveProjectionMat).Mul(capi.Render.CameraMatrixOriginf).Invert();
		frame++;
		if (!capi.IsGamePaused)
		{
			time += deltaTime;
		}
		program.UniformMatrix("iMvpMatrix", matrix.Values);
		program.Uniform("cloudOffset", map.offset);
		program.Uniform("cloudMapWidth", (float)map.CloudTileLength);
		program.Uniform("FrameWidth", capi.Render.FrameBuffers[0].Width);
		program.Uniform("frame", frame);
		program.Uniform("time", time);
		program.Uniform("PerceptionEffectIntensity", capi.Render.ShaderUniforms.PerceptionEffectIntensity);
		program.BindTexture2D("depthTex", capi.Render.FrameBuffers[0].DepthTextureId, 0);
		program.BindTexture2D("cloudMap", map.TextureMap, 8);
		program.BindTexture2D("cloudCol", map.TextureCol, 9);
		GL.Disable((EnableCap)2929);
		GL.Enable((EnableCap)3042);
		capi.Render.RenderMesh(quad);
		GL.Enable((EnableCap)2929);
		program.Stop();
	}
}
