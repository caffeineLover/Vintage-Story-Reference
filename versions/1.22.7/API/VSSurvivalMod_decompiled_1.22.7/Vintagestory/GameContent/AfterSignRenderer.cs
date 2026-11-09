using System;
using Vintagestory.API.Client;

namespace Vintagestory.GameContent;

public class AfterSignRenderer : IRenderer, IDisposable
{
	public static bool Registered;

	public double RenderOrder => BlockEntitySignRenderer.AfterSignRendererOrder;

	public int RenderRange => 24;

	public AfterSignRenderer()
	{
		Registered = true;
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		BlockEntitySignRenderer.AfterRenderFrameOnce();
	}

	public void Dispose()
	{
	}

	public static void OnGameDisposed()
	{
		Registered = false;
		BlockEntitySignRenderer.AfterRenderFrameOnce();
	}
}
