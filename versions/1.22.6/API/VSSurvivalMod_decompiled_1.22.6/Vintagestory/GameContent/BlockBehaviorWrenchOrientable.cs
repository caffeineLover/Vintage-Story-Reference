using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorWrenchOrientable : BlockBehavior
{
	public static Dictionary<string, SortedSet<AssetLocation>> VariantsByType = new Dictionary<string, SortedSet<AssetLocation>>();

	[DocumentAsJson("Required", "", false)]
	public string BaseCode;

	[DocumentAsJson("Optional", "False", false)]
	private bool hideInteractionHelpInSurvival;

	public static ItemStack[] wrenchItems;

	public static void loadWrenchItems(IWorldAccessor world)
	{
		Item[] wrenches = world.SearchItems(new AssetLocation("wrench-*"));
		wrenchItems = new ItemStack[wrenches.Length];
		for (int i = 0; i < wrenches.Length; i++)
		{
			wrenchItems[i] = new ItemStack(wrenches[i]);
		}
	}

	public BlockBehaviorWrenchOrientable(Block block)
		: base(block)
	{
	}

	public virtual bool ShowWrenchRotateHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		if (hideInteractionHelpInSurvival && forPlayer != null && forPlayer.WorldData.CurrentGameMode == EnumGameMode.Survival)
		{
			return false;
		}
		if (wrenchItems == null)
		{
			loadWrenchItems(world);
		}
		bool notProtected = true;
		if (world.Claims != null && world is IClientWorldAccessor clientWorld)
		{
			IClientPlayer player = clientWorld.Player;
			if (player != null && player.WorldData.CurrentGameMode == EnumGameMode.Survival && world.Claims.TestAccess(clientWorld.Player, selection.Position, EnumBlockAccessFlags.BuildOrBreak) != EnumWorldAccessResponse.Granted)
			{
				notProtected = false;
			}
		}
		if (wrenchItems.Length != 0 && notProtected)
		{
			return true;
		}
		return false;
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
	{
		if (!ShowWrenchRotateHelp(world, selection, forPlayer))
		{
			return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer, ref handling);
		}
		handling = EnumHandling.PassThrough;
		return new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "Rotate",
				Itemstacks = wrenchItems,
				MouseButton = EnumMouseButton.Right
			}
		};
	}

	public override int GetPlacedBlockInteractionHelpCount(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handled)
	{
		return ShowWrenchRotateHelp(world, selection, forPlayer) ? 1 : 0;
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		hideInteractionHelpInSurvival = properties["hideInteractionHelpInSurvival"].AsBool();
		BaseCode = properties["baseCode"].AsString();
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (BaseCode != null)
		{
			if (!VariantsByType.TryGetValue(BaseCode, out var vars))
			{
				vars = (VariantsByType[BaseCode] = new SortedSet<AssetLocation>());
			}
			vars.Add(block.Code);
		}
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		wrenchItems = null;
		VariantsByType.Clear();
	}
}
