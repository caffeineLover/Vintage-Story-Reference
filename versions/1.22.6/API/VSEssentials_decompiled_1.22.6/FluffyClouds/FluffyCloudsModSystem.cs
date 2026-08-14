using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace FluffyClouds;

public class FluffyCloudsModSystem : ModSystem
{
	private ICoreClientAPI? capi;

	private CloudRendererMap? map;

	private CloudRendererVolumetric? volumetric;

	private string[] modes = new string[3] { "off", "volumetric", "simple" };

	public override void StartClientSide(ICoreClientAPI capi)
	{
		ArgumentNullException.ThrowIfNull(capi, "capi");
		this.capi = capi;
		capi.Settings.Int.AddWatcher("cloudRenderMode", delegate(int i)
		{
			registerCloudRenderer(i);
			capi.ShowChatMessage("cloud renderer: " + modes[i]);
		});
		capi.Event.LevelFinalize += delegate
		{
			map = new CloudRendererMap(this, capi);
			volumetric = new CloudRendererVolumetric(this, capi, map);
			capi.Event.RegisterRenderer(map, EnumRenderStage.OIT, "cloudsmap");
			int i = capi.Settings.Int.Get("cloudRenderMode", 0);
			registerCloudRenderer(i);
		};
		capi.Event.LeaveWorld += delegate
		{
			map?.Dispose();
			volumetric?.Dispose();
		};
	}

	private void registerCloudRenderer(int i)
	{
		ArgumentNullException.ThrowIfNull(capi, "capi");
		if (i != 1)
		{
			capi.Event.UnregisterRenderer(volumetric, EnumRenderStage.OIT);
		}
		else
		{
			capi.Event.RegisterRenderer(volumetric, EnumRenderStage.OIT, "cloudsvolumetric");
		}
	}
}
