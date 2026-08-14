using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityParticleGrasshopper : EntityParticleInsect
{
	public override string Type => "grassHopper";

	public EntityParticleGrasshopper(ICoreClientAPI capi, double x, double y, double z)
		: base(capi, x, y, z)
	{
		Block block = capi.World.BlockAccessor.GetBlockRaw((int)x, (int)y, (int)z);
		if (block.BlockMaterial == EnumBlockMaterial.Plant)
		{
			int col = block.GetColor(capi, new BlockPos((int)x, (int)y, (int)z));
			ColorRed = (byte)((col >> 16) & 0xFF);
			ColorGreen = (byte)((col >> 8) & 0xFF);
			ColorBlue = (byte)(col & 0xFF);
		}
		else
		{
			ColorRed = 31;
			ColorGreen = 178;
			ColorBlue = 144;
		}
		sound = new AssetLocation("sounds/creature/grasshopper");
	}

	protected override bool shouldPlaySound()
	{
		if (EntityParticleInsect.rand.NextDouble() < 0.01 && capi.World.BlockAccessor.GetLightLevel(Position.AsBlockPos, EnumLightLevelType.TimeOfDaySunLight) > 7)
		{
			if (capi.World.Calendar.GetSeason(Position.AsBlockPos) != EnumSeason.Summer)
			{
				return EntityParticleInsect.rand.NextDouble() < 0.33;
			}
			return true;
		}
		return false;
	}
}
