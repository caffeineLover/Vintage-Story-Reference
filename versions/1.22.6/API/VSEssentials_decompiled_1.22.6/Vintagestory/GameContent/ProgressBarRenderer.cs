using System;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class ProgressBarRenderer : IRenderer, IDisposable, IProgressBar
{
	private MeshRef whiteRectangleRef;

	private MeshRef progressQuadRef;

	private ICoreClientAPI capi;

	private Matrixf mvMatrix = new Matrixf();

	private float offsety;

	public float Progress { get; set; }

	public double RenderOrder => 0.0;

	public int RenderRange => 10;

	public ProgressBarRenderer(ICoreClientAPI api, float offsety)
	{
		capi = api;
		this.offsety = offsety;
		MeshData rectangle = LineMeshUtil.GetRectangle(-1);
		whiteRectangleRef = api.Render.UploadMesh(rectangle);
		progressQuadRef = api.Render.UploadMesh(QuadMeshUtil.GetQuad());
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		IShaderProgram currentActiveShader = capi.Render.CurrentActiveShader;
		Vec4f color = new Vec4f(1f, 1f, 1f, 1f);
		currentActiveShader.Uniform("rgbaIn", color);
		currentActiveShader.Uniform("extraGlow", 0);
		currentActiveShader.Uniform("applyColor", 0);
		currentActiveShader.Uniform("tex2d", 0);
		currentActiveShader.Uniform("noTexture", 1f);
		int fwdt = capi.Render.FrameWidth;
		int fhgt = capi.Render.FrameHeight;
		float sc = RuntimeEnv.GUIScale / 2f;
		mvMatrix.Set(capi.Render.CurrentModelviewMatrix).Translate((float)(fwdt / 2) - 50f * sc, (float)(fhgt / 2) + 20f * sc + offsety * sc, 50f).Scale(sc * 100f, sc * 20f, 0f)
			.Translate(0.5f, 0.5f, 0f)
			.Scale(0.5f, 0.5f, 0f);
		currentActiveShader.UniformMatrix("projectionMatrix", capi.Render.CurrentProjectionMatrix);
		currentActiveShader.UniformMatrix("modelViewMatrix", mvMatrix.Values);
		capi.Render.RenderMesh(whiteRectangleRef);
		float width = Progress * 100f;
		mvMatrix.Set(capi.Render.CurrentModelviewMatrix).Translate((float)(fwdt / 2) - 50f * sc, (float)(fhgt / 2) + 20f * sc + offsety * sc, 50f).Scale(sc * width, sc * 20f, 0f)
			.Translate(0.5f, 0.5f, 0f)
			.Scale(0.5f, 0.5f, 0f);
		currentActiveShader.UniformMatrix("projectionMatrix", capi.Render.CurrentProjectionMatrix);
		currentActiveShader.UniformMatrix("modelViewMatrix", mvMatrix.Values);
		capi.Render.RenderMesh(progressQuadRef);
	}

	public void Dispose()
	{
		capi.Render.DeleteMesh(whiteRectangleRef);
		capi.Render.DeleteMesh(progressQuadRef);
	}
}
