using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

internal class ItemLiquidPortion : Item, ICoolingMedium
{
	private float coolingMediumTemperature;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		coolingMediumTemperature = Attributes["coolingMediumTemperature"].AsInt(GlobalConstants.CollectibleDefaultTemperature);
	}

	public bool CanCool(ItemSlot slot, Vec3d pos)
	{
		return Attributes?.IsTrue("coolingMedium") ?? false;
	}

	public void CoolNow(ItemSlot slot, Vec3d pos, float dt, bool playSizzle = true)
	{
		CollectibleBehaviorQuenchable.CoolToTemperature(api.World, slot, pos, dt, coolingMediumTemperature, playSizzle);
	}

	public override void OnGroundIdle(EntityItem entityItem)
	{
		entityItem.Die(EnumDespawnReason.Removed);
		if (entityItem.World.Side == EnumAppSide.Server)
		{
			WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(entityItem.Itemstack);
			float litres = (float)entityItem.Itemstack.StackSize / (props?.ItemsPerLitre ?? 1f);
			entityItem.World.SpawnCubeParticles(entityItem.Pos.XYZ, entityItem.Itemstack, 0.75f, (int)(litres * 2f), 0.45f);
			entityItem.World.PlaySoundAt(new AssetLocation("sounds/environment/smallsplash"), (float)entityItem.Pos.X, (float)entityItem.Pos.InternalY, (float)entityItem.Pos.Z);
		}
		base.OnGroundIdle(entityItem);
	}
}
