using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public sealed class ModSystemFishInstaFlee : ModSystem
{
	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Server;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		api.Event.HandInteract += event_HandInteract;
	}

	private void event_HandInteract(IServerPlayer player, EnumHandInteractNw enumHandInteract, float secondsPassed, ref EnumHandling handling)
	{
		if (player.CurrentEntitySelection?.Entity is EntityFish fishEntity && (enumHandInteract == EnumHandInteractNw.StartHeldItemUse || enumHandInteract == EnumHandInteractNw.StepHeldItemUse) && fishEntity.Swimming)
		{
			fishEntity.TryFleeAway();
		}
	}
}
