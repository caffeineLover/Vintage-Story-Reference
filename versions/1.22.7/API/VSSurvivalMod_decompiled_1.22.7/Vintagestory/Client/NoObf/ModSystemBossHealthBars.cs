using System.Collections.Generic;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Vintagestory.Client.NoObf;

public class ModSystemBossHealthBars : ModSystem
{
	private ICoreClientAPI capi;

	private EntityPartitioning partUtil;

	private List<HudBosshealthBars> trackedBosses = new List<HudBosshealthBars>();

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Client;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		partUtil = api.ModLoader.GetModSystem<EntityPartitioning>();
		api.Event.RegisterGameTickListener(onTick, 200, 12);
	}

	private void onTick(float dt)
	{
		List<EntityAgent> foundBosses = new List<EntityAgent>();
		Vec3d plrpos = capi.World.Player.Entity.Pos.XYZ;
		partUtil.WalkEntities(plrpos, 60.0, delegate(Entity e)
		{
			EntityBehaviorBoss behavior;
			if (e.Alive && e.IsInteractable && (behavior = e.GetBehavior<EntityBehaviorBoss>()) != null)
			{
				double distance = getDistance(capi.World.Player.Entity, e);
				if (behavior.ShowHealthBar && distance <= (double)behavior.BossHpbarRange)
				{
					foundBosses.Add(e as EntityAgent);
				}
			}
			return true;
		}, EnumEntitySearchType.Creatures);
		int dimensionDiff = 0;
		dimensionDiff = ((capi.World.Player.Entity.Pos.Dimension != 0) ? (-capi.World.Player.Entity.Pos.Dimension) : 2);
		plrpos.Y += dimensionDiff * 32768;
		partUtil.WalkEntities(plrpos, 60.0, delegate(Entity e)
		{
			EntityBehaviorBoss behavior;
			if (e.Alive && e.IsInteractable && (behavior = e.GetBehavior<EntityBehaviorBoss>()) != null)
			{
				double distance = getDistance(capi.World.Player.Entity, e);
				if (behavior.ShowHealthBar && distance <= (double)behavior.BossHpbarRange)
				{
					foundBosses.Add(e as EntityAgent);
				}
			}
			return true;
		}, EnumEntitySearchType.Creatures);
		int reorganizePositionsAt = -1;
		for (int i = 0; i < trackedBosses.Count; i++)
		{
			HudBosshealthBars hud = trackedBosses[i];
			if (foundBosses.Contains(hud.TargetEntity))
			{
				foundBosses.Remove(hud.TargetEntity);
				continue;
			}
			trackedBosses[i].TryClose();
			trackedBosses[i].Dispose();
			trackedBosses.RemoveAt(i);
			reorganizePositionsAt = i;
			i--;
		}
		foreach (EntityAgent eagent in foundBosses)
		{
			trackedBosses.Add(new HudBosshealthBars(capi, eagent, trackedBosses.Count));
		}
		if (reorganizePositionsAt >= 0)
		{
			for (int i2 = reorganizePositionsAt; i2 < trackedBosses.Count; i2++)
			{
				trackedBosses[i2].barIndex = i2;
				trackedBosses[i2].ComposeGuis();
			}
		}
		foreach (HudBosshealthBars hudbar in trackedBosses)
		{
			int previousDimension = hudbar.Dimension;
			int currentDimesnsion = hudbar.TargetEntity.Pos.Dimension;
			if (currentDimesnsion != previousDimension)
			{
				hudbar.ComposeGuis();
				hudbar.Dimension = currentDimesnsion;
			}
		}
	}

	private double getDistance(Entity player, Entity entity)
	{
		Vector3d vec = new Vector3d(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		Vector3d playerPos = new Vector3d(player.Pos.X, player.Pos.Y, player.Pos.Z);
		return Vector3d.Distance(vec, playerPos);
	}
}
