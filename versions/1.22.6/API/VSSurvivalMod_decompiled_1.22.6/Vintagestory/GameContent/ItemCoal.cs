using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class ItemCoal : ItemPileable
{
	protected override AssetLocation PileBlockCode => new AssetLocation("coalpile");

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		JsonObject inforgeprops = Attributes["inForge"];
		int tpd = inforgeprops["tempGainDeg"].AsInt();
		if (tpd != 0)
		{
			dsc.AppendLine(Lang.Get("coal-forgetemp-mod", tpd));
		}
		float dm = inforgeprops["durationMul"].AsFloat();
		if (dm != 1f)
		{
			dsc.AppendLine(Lang.Get("coal-forgedur-mod", dm));
		}
	}
}
