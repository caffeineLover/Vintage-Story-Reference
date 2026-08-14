using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class EntityLibraryResonator : EntityAgent
{
	private SimpleParticleProperties lhp;

	public override void OnEntityLoaded()
	{
		base.OnEntityLoaded();
		lhp = GuiStyle.LoreHintParticles.Clone(Api.World);
	}

	public override void OnGameTick(float dt)
	{
		base.OnGameTick(dt);
		if (Api.Side == EnumAppSide.Client && Api.World.Rand.NextDouble() < 0.05 && lhp != null)
		{
			lhp.MinPos = base.Pos.XYZ.AddCopy(-0.5, 0.10000000149011612, -0.5);
			Api.World.SpawnParticles(lhp);
		}
	}
}
