using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class BlockEntityPie : BlockEntityContainer
{
	private InventoryGeneric inv;

	private MealMeshCache? ms;

	private MeshData? mesh;

	public override InventoryBase Inventory => inv;

	public override string InventoryClassName => "pie";

	public bool HasAnyFilling
	{
		get
		{
			ItemStack[] cStacks = (inv[0].Itemstack?.Block as BlockPie)?.GetContents(Api.World, inv[0].Itemstack);
			if (((cStacks != null) ? cStacks[1] : null) == null && ((cStacks != null) ? cStacks[2] : null) == null && ((cStacks != null) ? cStacks[3] : null) == null)
			{
				return ((cStacks != null) ? cStacks[4] : null) != null;
			}
			return true;
		}
	}

	public bool HasAllFilling
	{
		get
		{
			ItemStack[] cStacks = (inv[0].Itemstack?.Block as BlockPie)?.GetContents(Api.World, inv[0].Itemstack);
			if (((cStacks != null) ? cStacks[1] : null) != null && ((cStacks != null) ? cStacks[2] : null) != null && ((cStacks != null) ? cStacks[3] : null) != null)
			{
				return ((cStacks != null) ? cStacks[4] : null) != null;
			}
			return false;
		}
	}

	public bool HasCrust
	{
		get
		{
			BlockPie obj = inv[0].Itemstack?.Block as BlockPie;
			return ((obj != null) ? obj.GetContents(Api.World, inv[0].Itemstack)[5] : null) != null;
		}
	}

	public string? State => (inv[0].Itemstack?.Block as BlockPie)?.State;

	public int SlicesLeft => inv[0].Itemstack?.Attributes.GetAsInt("pieSize") ?? 0;

	public BlockEntityPie()
	{
		inv = new InventoryGeneric(1, null, null);
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		ms = api.ModLoader.GetModSystem<MealMeshCache>();
		loadMesh();
	}

	protected override void OnTick(float dt)
	{
		base.OnTick(dt);
		if (inv[0].Itemstack?.Collectible.Code.Path == "rot")
		{
			Api.World.BlockAccessor.SetBlock(0, Pos);
			Api.World.SpawnItemEntity(inv[0].Itemstack, Pos.ToVec3d().Add(0.5, 0.1, 0.5));
		}
	}

	public override void OnBlockPlaced(ItemStack? byItemStack = null)
	{
		if (byItemStack != null)
		{
			inv[0].Itemstack = byItemStack.Clone();
			inv[0].Itemstack.StackSize = 1;
		}
	}

	public ItemStack? TakeSlice()
	{
		ItemStack stack = inv[0].Itemstack?.Clone();
		if (stack == null)
		{
			return null;
		}
		int size = SlicesLeft;
		float servings = inv[0].Itemstack?.Attributes.GetFloat("quantityServings") ?? 0f;
		MarkDirty(redrawOnClient: true);
		if (size <= 1)
		{
			if (!stack.Attributes.HasAttribute("quantityServings"))
			{
				stack.Attributes.SetFloat("quantityServings", 0.25f);
			}
			inv[0].Itemstack = null;
			Api.World.BlockAccessor.SetBlock(0, Pos);
		}
		else
		{
			inv[0].Itemstack?.Attributes.SetInt("pieSize", size - 1);
			ItemStack? itemstack = inv[0].Itemstack;
			if (itemstack != null && itemstack.Attributes.HasAttribute("quantityServings"))
			{
				inv[0].Itemstack?.Attributes.SetFloat("quantityServings", servings - 0.25f);
			}
			stack.Attributes.SetInt("pieSize", 1);
			stack.Attributes.SetFloat("quantityServings", 0.25f);
		}
		stack.Attributes.SetBool("bakeable", value: false);
		loadMesh();
		MarkDirty(redrawOnClient: true);
		return stack;
	}

	public void OnPlaced(IPlayer? byPlayer)
	{
		ItemStack doughStack = byPlayer?.InventoryManager.ActiveHotbarSlot.TakeOut(2);
		if (doughStack != null)
		{
			ItemStack pie = new ItemStack(base.Block);
			(pie.Block as BlockPie)?.SetContents(pie, new ItemStack[6] { doughStack, null, null, null, null, null });
			pie.Attributes.SetInt("pieSize", 4);
			pie.Attributes.SetBool("bakeable", value: false);
			if (State != "raw" && !pie.Attributes.HasAttribute("quantityServings"))
			{
				pie.Attributes.SetFloat("quantityServings", (float)pie.Attributes.GetAsInt("pieSize") * 0.25f);
			}
			inv[0].Itemstack = pie;
			loadMesh();
		}
	}

	public bool OnInteract(IPlayer byPlayer)
	{
		if (!(inv[0].Itemstack?.Block is BlockPie pieBlock))
		{
			return false;
		}
		ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		bool flag;
		switch (hotbarSlot?.Itemstack?.Collectible.GetTool(hotbarSlot))
		{
		case EnumTool.Knife:
		case EnumTool.Sword:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			if (pieBlock.State != "raw")
			{
				if (Api.Side == EnumAppSide.Server)
				{
					ItemStack slicestack = TakeSlice();
					if (slicestack != null)
					{
						if (!byPlayer.InventoryManager.TryGiveItemstack(slicestack))
						{
							Api.World.SpawnItemEntity(slicestack, Pos);
						}
						Api.World.Logger.Audit("{0} Took 1x{1} slice from Pie at {2}.", byPlayer.PlayerName, slicestack.Collectible.Code, Pos);
					}
				}
			}
			else
			{
				ItemStack[] cStacks = pieBlock.GetContents(Api.World, inv[0].Itemstack);
				if (!HasAnyFilling || cStacks[5] == null)
				{
					return true;
				}
				inv[0].Itemstack = BlockPie.CycleTopCrustType(inv[0].Itemstack);
				MarkDirty(redrawOnClient: true);
			}
			return true;
		}
		if (hotbarSlot != null && !hotbarSlot.Empty && pieBlock.State == "raw")
		{
			bool num = TryAddIngredientFrom(hotbarSlot, byPlayer);
			if (num)
			{
				loadMesh();
				MarkDirty(redrawOnClient: true);
			}
			ItemStack? itemstack = inv[0].Itemstack;
			if (itemstack != null)
			{
				itemstack.Attributes.SetBool("bakeable", HasAllFilling);
				return num;
			}
			return num;
		}
		if (SlicesLeft == 1)
		{
			ItemStack? itemstack2 = inv[0].Itemstack;
			if (itemstack2 == null || !itemstack2.Attributes.HasAttribute("quantityServings"))
			{
				inv[0].Itemstack?.Attributes.SetBool("bakeable", value: false);
				inv[0].Itemstack?.Attributes.SetFloat("quantityServings", 0.25f);
			}
		}
		if (Api.Side == EnumAppSide.Server)
		{
			if (!byPlayer.InventoryManager.TryGiveItemstack(inv[0].Itemstack))
			{
				Api.World.SpawnItemEntity(inv[0].Itemstack, Pos.ToVec3d().Add(0.5, 0.25, 0.5));
			}
			Api.World.Logger.Audit("{0} Took 1x{1} at {2}.", byPlayer.PlayerName, inv[0].Itemstack?.Collectible.Code, Pos);
			inv[0].Itemstack = null;
		}
		Api.World.BlockAccessor.SetBlock(0, Pos);
		return true;
	}

	private bool TryAddIngredientFrom(ItemSlot slot, IPlayer? byPlayer = null)
	{
		ICoreClientAPI capi = Api as ICoreClientAPI;
		InPieProperties pieProps = slot.Itemstack?.ItemAttributes?["inPieProperties"]?.AsObject<InPieProperties>(null, slot.Itemstack.Collectible.Code.Domain);
		if (pieProps == null)
		{
			if (byPlayer != null)
			{
				capi?.TriggerIngameError(this, "notpieable", Lang.Get("This item can not be added to pies"));
			}
			return false;
		}
		if (slot.StackSize < 2)
		{
			if (byPlayer != null)
			{
				capi?.TriggerIngameError(this, "notpieable", Lang.Get("Need at least 2 items each"));
			}
			return false;
		}
		if (!(inv[0].Itemstack?.Block is BlockPie pieBlock))
		{
			return false;
		}
		ItemStack[] cStacks = pieBlock.GetContents(Api.World, inv[0].Itemstack);
		bool num = cStacks[1] != null && cStacks[2] != null && cStacks[3] != null && cStacks[4] != null;
		bool hasFilling = cStacks[1] != null || cStacks[2] != null || cStacks[3] != null || cStacks[4] != null;
		if (num)
		{
			if (pieProps.PartType == EnumPiePartType.Crust)
			{
				if (cStacks[5] == null)
				{
					cStacks[5] = slot.TakeOut(2);
					pieBlock.SetContents(inv[0].Itemstack, cStacks);
					inv[0].Itemstack.Attributes.SetString("topCrustType", "full");
				}
				else
				{
					inv[0].Itemstack = BlockPie.CycleTopCrustType(inv[0].Itemstack);
				}
				return true;
			}
			if (byPlayer != null)
			{
				capi?.TriggerIngameError(this, "piefullfilling", Lang.Get("Can't add more filling - already completely filled pie"));
			}
			return false;
		}
		if (pieProps.PartType != EnumPiePartType.Filling)
		{
			if (byPlayer != null)
			{
				capi?.TriggerIngameError(this, "pieneedsfilling", Lang.Get("Need to add a filling next"));
			}
			return false;
		}
		if (!hasFilling)
		{
			cStacks[1] = slot.TakeOut(2);
			pieBlock.SetContents(inv[0].Itemstack, cStacks);
			return true;
		}
		EnumFoodCategory[] foodCats = cStacks.Select(BlockPie.FillingFoodCategory).ToArray();
		InPieProperties[] stackprops = cStacks.Select((ItemStack stack) => stack?.ItemAttributes?["inPieProperties"]?.AsObject<InPieProperties>(null, stack.Collectible.Code.Domain)).ToArray();
		ItemStack cstack = slot.Itemstack;
		EnumFoodCategory foodCat = BlockPie.FillingFoodCategory(cstack);
		bool equal = true;
		bool foodCatEquals = true;
		bool allowMixing = true;
		IEnumerable<string> mixCodes = pieProps.MixingCodes;
		int i = 1;
		while ((equal || foodCatEquals || mixCodes.Any()) && i < cStacks.Length - 1)
		{
			if (cstack != null)
			{
				equal &= cStacks[i] == null || cstack.Equals(Api.World, cStacks[i], GlobalConstants.IgnoredStackAttributes);
				foodCatEquals &= cStacks[i] == null || foodCats[i] == foodCat;
				allowMixing &= stackprops[i]?.AllowMixing ?? true;
				mixCodes = stackprops[i]?.MixingCodes.Intersect(mixCodes) ?? mixCodes;
				cstack = cStacks[i];
				foodCat = foodCats[i];
			}
			i++;
		}
		int emptySlotIndex = 2 + ((cStacks[2] != null) ? (1 + ((cStacks[3] != null) ? 1 : 0)) : 0);
		if (equal)
		{
			cStacks[emptySlotIndex] = slot.TakeOut(2);
			pieBlock.SetContents(inv[0].Itemstack, cStacks);
			return true;
		}
		if (!foodCatEquals && !mixCodes.Any())
		{
			if (byPlayer != null)
			{
				capi?.TriggerIngameError(this, "piefullfilling", Lang.Get("piemaking-unabletomixingredient"));
			}
			return false;
		}
		if (!allowMixing)
		{
			if (byPlayer != null)
			{
				capi?.TriggerIngameError(this, "piefullfilling", Lang.Get("piemaking-mixingnotallowed"));
			}
			return false;
		}
		cStacks[emptySlotIndex] = slot.TakeOut(2);
		pieBlock.SetContents(inv[0].Itemstack, cStacks);
		return true;
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (inv[0].Empty)
		{
			return true;
		}
		mesher.AddMeshData(mesh);
		return true;
	}

	private void loadMesh()
	{
		if (Api != null && Api.Side != EnumAppSide.Server && !inv[0].Empty)
		{
			mesh = ms.GetPieMesh(inv[0].Itemstack);
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (MealMeshCache.ContentsRotten(inv))
		{
			dsc.Append(Lang.Get("Rotten"));
		}
		else
		{
			dsc.Append(BlockEntityShelf.PerishableInfoCompact(Api, inv[0], 0f, withStackName: false));
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		if (worldForResolving.Side == EnumAppSide.Client)
		{
			MarkDirty(redrawOnClient: true);
			loadMesh();
		}
	}

	public override void OnBlockBroken(IPlayer? byPlayer = null)
	{
	}
}
