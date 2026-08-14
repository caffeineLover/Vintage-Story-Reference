using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class FruitingBushState
{
	public static string[][] AllTraits = new string[4][]
	{
		new string[2] { "shybearer", "heavybearer" },
		new string[2] { "thinskinnedfruit", "thickskinnedfruit" },
		new string[2] { "weakrooted", "strongrooted" },
		new string[2] { "weakclusteredberries", "strongclusteredberries" }
	};

	public double PlantedTotalDays;

	public double MatureTotalDays;

	public double TransitionHoursLeft;

	private EnumFruitingBushGrowthState growthstate;

	public string[] Traits = Array.Empty<string>();

	public EnumFruitingBushHealthState PrevHealthState;

	private EnumFruitingBushHealthState? wildBushState;

	public bool MeshDirty;

	public double LastCuttingTakenTotalDays = -99999.0;

	public EnumFruitingBushGrowthState Growthstate
	{
		get
		{
			return growthstate;
		}
		set
		{
			if (value != growthstate)
			{
				MeshDirty = true;
			}
			growthstate = value;
		}
	}

	public EnumFruitingBushHealthState? WildBushState
	{
		get
		{
			return wildBushState;
		}
		set
		{
			if (value != wildBushState)
			{
				MeshDirty = true;
			}
			wildBushState = value;
		}
	}

	public void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		PlantedTotalDays = tree.GetDouble("plantedTotalDays");
		MatureTotalDays = tree.GetDouble("matureTotalDays");
		LastCuttingTakenTotalDays = tree.GetDouble("lastCuttingTakenTotalDays", -99999.0);
		TransitionHoursLeft = tree.GetDouble("transitionHoursLeft");
		Growthstate = (EnumFruitingBushGrowthState)tree.GetInt("growthState");
		if (tree.HasAttribute("traits"))
		{
			if (tree.GetString("traits").Length == 0)
			{
				Traits = Array.Empty<string>();
			}
			Traits = tree.GetString("traits").Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}
		else
		{
			Traits = Array.Empty<string>();
		}
		WildBushState = null;
		if (tree.HasAttribute("wildBushState"))
		{
			WildBushState = (EnumFruitingBushHealthState)tree.GetInt("wildBushState");
		}
	}

	public void ToTreeAttributes(ITreeAttribute tree)
	{
		tree.SetDouble("plantedTotalDays", PlantedTotalDays);
		tree.SetDouble("matureTotalDays", MatureTotalDays);
		tree.SetDouble("lastCuttingTakenTotalDays", LastCuttingTakenTotalDays);
		tree.SetDouble("transitionHoursLeft", TransitionHoursLeft);
		tree.SetInt("growthState", (int)Growthstate);
		string[] traits = Traits;
		if (traits != null && traits.Length != 0)
		{
			tree.SetString("traits", string.Join(",", Traits));
		}
		if (WildBushState.HasValue)
		{
			tree.SetInt("wildBushState", (int)WildBushState.Value);
		}
	}
}
