using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class RightClickConstruction
{
	public delegate Vec3d PositionSupplierDelegate();

	public ConstructionStage[] Stages;

	public int CurrentCompletedStage;

	public Dictionary<string, string> StoredWildCards = new Dictionary<string, string>();

	protected WorldInteraction[] nextConstructWis;

	protected string codeForErrorLogging;

	protected PositionSupplierDelegate positionForSound;

	protected int dimension;

	protected ICoreAPI api;

	public void LateInit(ConstructionStage[] stages, ICoreAPI api, PositionSupplierDelegate positionForSound, string codeForErrorLogging)
	{
		Stages = stages;
		this.api = api;
		this.codeForErrorLogging = codeForErrorLogging;
		this.positionForSound = positionForSound;
	}

	public bool OnInteract(EntityAgent byEntity, ItemSlot handslot)
	{
		if (CurrentCompletedStage >= Stages.Length - 1)
		{
			return false;
		}
		if (!tryConsumeIngredients(byEntity, handslot))
		{
			return false;
		}
		if (CurrentCompletedStage < Stages.Length - 1)
		{
			CurrentCompletedStage++;
		}
		GenInteractionHelp();
		return true;
	}

	public string[] getShapeElements()
	{
		HashSet<string> addElements = new HashSet<string>();
		int cstage = CurrentCompletedStage;
		for (int i = 0; i <= cstage; i++)
		{
			ConstructionStage stage = Stages[i];
			if (stage.AddElements != null)
			{
				string[] addElements2 = stage.AddElements;
				foreach (string addele in addElements2)
				{
					addElements.Add(addele + "/*");
				}
			}
			if (stage.RemoveElements != null)
			{
				string[] addElements2 = stage.RemoveElements;
				foreach (string remele in addElements2)
				{
					addElements.Remove(remele + "/*");
				}
			}
		}
		return addElements.ToArray();
	}

	public MeshData Tesselate(TesselationMetaData meta, ITesselatorAPI tesselator, Shape shape)
	{
		meta.SelectiveElements = getShapeElements();
		tesselator.TesselateShape(meta, shape, out var meshdata);
		return meshdata;
	}

	protected void GenInteractionHelp()
	{
		if (CurrentCompletedStage + 1 >= Stages.Length)
		{
			nextConstructWis = null;
			return;
		}
		ConstructionStage stage = Stages[CurrentCompletedStage + 1];
		if (stage.RequireStacks == null)
		{
			nextConstructWis = null;
			return;
		}
		List<WorldInteraction> wis = new List<WorldInteraction>();
		int i = 0;
		ConstructionIngredient[] requireStacks = stage.RequireStacks;
		foreach (ConstructionIngredient ingred in requireStacks)
		{
			List<ItemStack> stacksl = new List<ItemStack>();
			foreach (KeyValuePair<string, string> val in StoredWildCards)
			{
				ingred.FillPlaceHolder(val.Key, val.Value);
			}
			if (!ingred.Resolve(api.World, "Require stack for construction stage " + (CurrentCompletedStage + 1) + " on " + codeForErrorLogging))
			{
				return;
			}
			i++;
			foreach (CollectibleObject collectible in api.World.Collectibles)
			{
				ItemStack stack = new ItemStack(collectible);
				if (ingred.SatisfiesAsIngredient(stack, checkStackSize: false))
				{
					stack.StackSize = ingred.Quantity;
					stacksl.Add(stack);
				}
			}
			ItemStack[] stacks = stacksl.ToArray();
			wis.Add(new WorldInteraction
			{
				ActionLangCode = stage.ActionLangCode,
				Itemstacks = stacks,
				GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => stacks,
				MouseButton = EnumMouseButton.Right
			});
		}
		if (stage.RequireStacks.Length == 0)
		{
			wis.Add(new WorldInteraction
			{
				ActionLangCode = stage.ActionLangCode,
				MouseButton = EnumMouseButton.Right
			});
		}
		nextConstructWis = wis.ToArray();
	}

	public ItemStack[] GetDrops(float dropRatio = 1f, Random rnd = null)
	{
		if (CurrentCompletedStage <= 1)
		{
			return Array.Empty<ItemStack>();
		}
		List<ItemStack> stacks = new List<ItemStack>();
		for (int i = 0; i < CurrentCompletedStage; i++)
		{
			ConstructionStage stage = Stages[i];
			if (stage.RequireStacks == null)
			{
				continue;
			}
			ConstructionIngredient[] requireStacks = stage.RequireStacks;
			foreach (ConstructionIngredient ingred in requireStacks)
			{
				new List<ItemStack>();
				foreach (KeyValuePair<string, string> val in StoredWildCards)
				{
					ingred.FillPlaceHolder(val.Key, val.Value);
				}
				ConstructionIngredient resolveIngred = ingred;
				if (ingred.StoreWildCard != null)
				{
					resolveIngred = ingred.Clone();
					resolveIngred.Code.Path = resolveIngred.Code.Path.Replace("*", StoredWildCards[ingred.StoreWildCard]);
				}
				if (resolveIngred.Resolve(api.World, "Drop stack for construction stage " + i + " on " + codeForErrorLogging))
				{
					ItemStack stack = resolveIngred.ResolvedItemStack.Clone();
					stack.StackSize = GameMath.RoundRandom(rnd, (float)stack.StackSize * dropRatio);
					stacks.Add(stack);
				}
			}
		}
		return stacks.ToArray();
	}

	private bool tryConsumeIngredients(EntityAgent byEntity, ItemSlot handslot)
	{
		IPlayer plr = (byEntity as EntityPlayer).Player;
		ConstructionStage stage = Stages[CurrentCompletedStage + 1];
		IInventory hotbarinv = plr.InventoryManager.GetHotbarInventory();
		List<KeyValuePair<ItemSlot, int>> takeFrom = new List<KeyValuePair<ItemSlot, int>>();
		List<ConstructionIngredient> requireIngreds = new List<ConstructionIngredient>();
		if (stage.RequireStacks == null)
		{
			return true;
		}
		for (int i = 0; i < stage.RequireStacks.Length; i++)
		{
			requireIngreds.Add(stage.RequireStacks[i].Clone());
		}
		Dictionary<string, string> storeWildCard = new Dictionary<string, string>();
		bool skipMatCost = plr != null && plr.WorldData.CurrentGameMode == EnumGameMode.Creative && byEntity.Controls.CtrlKey;
		for (int j = 0; j < requireIngreds.Count; j++)
		{
			ConstructionIngredient ingred = requireIngreds[j];
			foreach (KeyValuePair<string, string> val in StoredWildCards)
			{
				ingred.FillPlaceHolder(val.Key, val.Value);
			}
			if (!ingred.Resolve(api.World, "Require stack for construction stage " + j + " on " + codeForErrorLogging) && ingred.MatchingType == EnumRecipeMatchType.Exact)
			{
				return false;
			}
		}
		foreach (ItemSlot slot in hotbarinv)
		{
			if (slot.Empty)
			{
				continue;
			}
			if (requireIngreds.Count == 0)
			{
				break;
			}
			for (int k = 0; k < requireIngreds.Count; k++)
			{
				ConstructionIngredient ingred2 = requireIngreds[k];
				if (!skipMatCost && ingred2.SatisfiesAsIngredient(slot.Itemstack, checkStackSize: false))
				{
					int amountToTake = Math.Min(ingred2.Quantity, slot.Itemstack.StackSize);
					takeFrom.Add(new KeyValuePair<ItemSlot, int>(slot, amountToTake));
					ingred2.Quantity -= amountToTake;
					if (ingred2.Quantity <= 0)
					{
						requireIngreds.RemoveAt(k);
						k--;
						if (ingred2.StoreWildCard != null)
						{
							storeWildCard[ingred2.StoreWildCard] = slot.Itemstack.Collectible.Variant[ingred2.StoreWildCard];
						}
					}
				}
				else if (skipMatCost && ingred2.StoreWildCard != null)
				{
					storeWildCard["wood"] = "oak";
					storeWildCard["metal"] = "iron";
				}
			}
		}
		if (!skipMatCost && requireIngreds.Count > 0)
		{
			ConstructionIngredient ingred3 = requireIngreds[0];
			if (plr is IClientPlayer)
			{
				(plr.Entity.Api as ICoreClientAPI).TriggerIngameError(this, "missingstack", Lang.Get("ingameerror-missingstack", ingred3.Quantity, (ingred3.MatchingType != EnumRecipeMatchType.Exact) ? Lang.Get(ingred3.Name ?? "") : ingred3.ResolvedItemStack.GetName()));
			}
			return false;
		}
		foreach (KeyValuePair<string, string> val2 in storeWildCard)
		{
			StoredWildCards[val2.Key] = val2.Value;
		}
		if (!skipMatCost)
		{
			bool soundPlayed = false;
			foreach (KeyValuePair<ItemSlot, int> kvp in takeFrom)
			{
				if (!soundPlayed)
				{
					AssetLocation soundLoc = null;
					ItemStack stack = kvp.Key.Itemstack;
					if (stack.Block != null)
					{
						soundLoc = stack.Block.Sounds?.Place.Location;
					}
					if (soundLoc == null)
					{
						soundLoc = stack.Collectible.GetBehavior<CollectibleBehaviorGroundStorable>()?.StorageProps?.PlaceRemoveSound;
					}
					if (soundLoc != null)
					{
						soundPlayed = true;
						Vec3d pos = positionForSound?.Invoke();
						api.World.PlaySoundAt(new SoundAttributes
						{
							Location = soundLoc
						}, pos.X, pos.Y, pos.Z, dimension);
					}
				}
				kvp.Key.TakeOut(kvp.Value);
				kvp.Key.MarkDirty();
			}
		}
		return true;
	}

	public void ToTreeAttributes(ITreeAttribute tree)
	{
		TreeAttribute subtree = new TreeAttribute();
		foreach (KeyValuePair<string, string> val in StoredWildCards)
		{
			subtree[val.Key] = new StringAttribute(val.Value);
		}
		tree["wildcards"] = subtree;
		tree.SetInt("currentStage", CurrentCompletedStage);
	}

	public void FromTreeAttributes(ITreeAttribute tree)
	{
		StoredWildCards.Clear();
		if (tree["wildcards"] is TreeAttribute subtree)
		{
			foreach (KeyValuePair<string, IAttribute> val in subtree)
			{
				StoredWildCards[val.Key] = (val.Value as StringAttribute).value;
			}
		}
		CurrentCompletedStage = tree.GetInt("currentStage");
	}

	public WorldInteraction[] GetInteractionHelp(IWorldAccessor world, IPlayer player)
	{
		GenInteractionHelp();
		return nextConstructWis;
	}
}
