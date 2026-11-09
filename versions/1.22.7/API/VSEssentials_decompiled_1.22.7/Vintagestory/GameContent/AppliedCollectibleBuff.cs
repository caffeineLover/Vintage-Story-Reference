using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class AppliedCollectibleBuff
{
	public required string Code;

	public required string StatCode;

	public int RemainingDurability;

	public float Multiplier = 1f;

	public float FlatChange;

	public override string ToString()
	{
		if (FlatChange != 0f)
		{
			return Lang.Get("flatchangebuff-" + Code + "-" + StatCode, FlatChange, RemainingDurability);
		}
		return Lang.Get("mulbuff-" + Code + "-" + StatCode, Multiplier - 1f, RemainingDurability);
	}

	public TreeAttribute ToAttribute()
	{
		TreeAttribute treeAttribute = new TreeAttribute();
		treeAttribute.SetString("code", Code);
		treeAttribute.SetString("statcode", StatCode);
		treeAttribute.SetInt("durability", RemainingDurability);
		treeAttribute.SetFloat("multiplier", Multiplier);
		treeAttribute.SetFloat("flatchange", FlatChange);
		return treeAttribute;
	}

	public static AppliedCollectibleBuff FromAttribute(TreeAttribute bufftree)
	{
		return new AppliedCollectibleBuff
		{
			Code = bufftree.GetString("code"),
			StatCode = bufftree.GetString("statcode"),
			RemainingDurability = bufftree.GetInt("durability"),
			Multiplier = bufftree.GetFloat("multiplier"),
			FlatChange = bufftree.GetFloat("flatchange")
		};
	}
}
