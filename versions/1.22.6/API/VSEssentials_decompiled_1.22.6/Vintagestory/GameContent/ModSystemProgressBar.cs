using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ModSystemProgressBar : ModSystem
{
	private List<ProgressBarRenderer> pbrenderer = new List<ProgressBarRenderer>();

	private ICoreClientAPI capi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Client;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
	}

	public IProgressBar AddProgressbar()
	{
		ProgressBarRenderer pbr = new ProgressBarRenderer(capi, pbrenderer.Count * 30);
		capi.Event.RegisterRenderer(pbr, EnumRenderStage.Ortho);
		pbrenderer.Add(pbr);
		return pbr;
	}

	public void RemoveProgressbar(IProgressBar pbr)
	{
		if (pbr != null)
		{
			ProgressBarRenderer prend = pbr as ProgressBarRenderer;
			pbrenderer.Remove(prend);
			capi.Event.UnregisterRenderer(prend, EnumRenderStage.Ortho);
			prend.Dispose();
		}
	}
}
