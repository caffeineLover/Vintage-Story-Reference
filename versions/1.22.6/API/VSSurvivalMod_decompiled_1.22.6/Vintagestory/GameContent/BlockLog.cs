using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent;

public class BlockLog : Block, IBlockLog, ICustomChiselMaterialName
{
	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		return Drops[0].ResolvedItemstack.Clone();
	}

	public override void AddMiningTierInfo(StringBuilder sb, IWorldAccessor world, BlockPos pos)
	{
		if (Code.PathStartsWith("log-grown"))
		{
			int woodTier = Attributes?["treeFellingGroupSpreadIndex"].AsInt() ?? 0;
			int requiredMiningTier = GetRequiredMiningTier(world, pos);
			woodTier += requiredMiningTier - 4;
			if (woodTier < requiredMiningTier)
			{
				woodTier = requiredMiningTier;
			}
			string tierName = "?";
			if (woodTier < Block.miningTierNames.Length)
			{
				tierName = Block.miningTierNames[woodTier];
			}
			sb.AppendLine(Lang.Get("Requires tool tier {0} ({1}) to break", woodTier, (tierName == "?") ? tierName : Lang.Get(tierName)));
		}
		else
		{
			base.AddMiningTierInfo(sb, world, pos);
		}
	}

	string ICustomChiselMaterialName.GetName(ItemStack itemStack)
	{
		return Lang.Get("chiselmaterialnamewithorientation-" + Variant["rotation"], itemStack.GetName());
	}
}
