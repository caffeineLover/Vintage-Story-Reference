using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockLantern : Block, ITexPositionSource, IAttachableToEntity, IContainedMeshSource, IContainedInteractable
{
	protected IAttachableToEntity attrAtta;

	protected string curMat;

	protected string curLining;

	protected ITexPositionSource glassTextureSource;

	protected ITexPositionSource tmpTextureSource;

	public int RequiresBehindSlots { get; set; }

	public Size2i AtlasSize { get; set; }

	public TextureAtlasPosition this[string textureCode] => textureCode switch
	{
		"material" => tmpTextureSource[curMat], 
		"material-deco" => tmpTextureSource["deco-" + curMat], 
		"material-grid" => tmpTextureSource["grid-" + curMat], 
		"lining" => tmpTextureSource[(curLining == "plain") ? curMat : curLining], 
		"glass" => glassTextureSource["material"], 
		_ => tmpTextureSource[textureCode], 
	};

	public virtual string baseCacheKey => "blockLantern" + Variant["size"];

	string IAttachableToEntity.GetCategoryCode(ItemStack stack)
	{
		return attrAtta?.GetCategoryCode(stack);
	}

	CompositeShape IAttachableToEntity.GetAttachedShape(ItemStack stack, string slotCode)
	{
		return attrAtta?.GetAttachedShape(stack, slotCode);
	}

	string[] IAttachableToEntity.GetDisableElements(ItemStack stack)
	{
		return attrAtta?.GetDisableElements(stack);
	}

	string[] IAttachableToEntity.GetKeepElements(ItemStack stack)
	{
		return attrAtta?.GetKeepElements(stack);
	}

	string IAttachableToEntity.GetTexturePrefixCode(ItemStack stack)
	{
		return attrAtta?.GetTexturePrefixCode(stack);
	}

	void IAttachableToEntity.CollectTextures(ItemStack itemstack, Shape intoShape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
	{
		string material = itemstack.Attributes.GetString("material");
		string lining = itemstack.Attributes.GetString("lining");
		string glassMaterial = itemstack.Attributes.GetString("glass", "quartz");
		Block glassBlock = api.World.GetBlock(new AssetLocation("glass-" + glassMaterial));
		intoShape.Textures["glass"] = glassBlock.Textures["material"].Base;
		intoShape.Textures["material"] = Textures[material].Base;
		intoShape.Textures["lining"] = Textures[(lining == null || lining == "plain") ? material : lining].Base;
		intoShape.Textures["material-deco"] = Textures["deco-" + material].Base;
		intoShape.Textures["material-grid"] = Textures["grid-" + material].Base;
	}

	public MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos atBlockPos)
	{
		string material = slot.Itemstack.Attributes.GetString("material");
		string lining = slot.Itemstack.Attributes.GetString("lining");
		string glass = slot.Itemstack.Attributes.GetString("glass", "quartz");
		_ = material + "-" + lining + "-" + glass;
		AssetLocation shapeloc = Shape.Base.CopyWithPathPrefixAndAppendixOnce("shapes/", ".json");
		Shape shape = Vintagestory.API.Common.Shape.TryGet(api, shapeloc);
		return GenMesh(api as ICoreClientAPI, material, lining, glass, shape);
	}

	public string GetMeshCacheKey(ItemSlot slot)
	{
		string material = slot.Itemstack.Attributes.GetString("material");
		string lining = slot.Itemstack.Attributes.GetString("lining");
		string glass = slot.Itemstack.Attributes.GetString("glass", "quartz");
		return string.Concat(new string[7] { Code, "-", material, "-", lining, "-", glass });
	}

	public bool IsAttachable(Entity toEntity, ItemStack itemStack)
	{
		return true;
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		attrAtta = IAttachableToEntity.FromAttributes(this);
	}

	public override string GetHeldTpIdleAnimation(ItemSlot activeHotbarSlot, Entity forEntity, EnumHand hand)
	{
		IPlayer player = (forEntity as EntityPlayer)?.Player;
		if (forEntity.AnimManager.IsAnimationActive("sleep", "wave", "cheer", "shrug", "cry", "nod", "facepalm", "bow", "laugh", "rage", "scythe", "bowaim", "bowhit", "spearidle"))
		{
			return null;
		}
		if (player?.InventoryManager?.ActiveHotbarSlot != null && !player.InventoryManager.ActiveHotbarSlot.Empty && hand == EnumHand.Left)
		{
			ItemStack stack = player.InventoryManager.ActiveHotbarSlot.Itemstack;
			if (stack != null && stack.Collectible?.GetHeldTpIdleAnimation(player.InventoryManager.ActiveHotbarSlot, forEntity, EnumHand.Right) != null)
			{
				return null;
			}
			if (player != null && player.Entity?.Controls.LeftMouseDown == true && stack != null && stack.Collectible?.GetHeldTpHitAnimation(player.InventoryManager.ActiveHotbarSlot, forEntity) != null)
			{
				return null;
			}
		}
		if (hand != EnumHand.Left)
		{
			return "holdinglanternrighthand";
		}
		return "holdinglanternlefthand";
	}

	public override byte[] GetLightHsv(IBlockAccessor blockAccessor, BlockPos pos, ItemStack stack = null)
	{
		if (pos != null && blockAccessor.GetBlockEntity(pos) is BELantern be)
		{
			return be.GetLightHsv();
		}
		if (stack != null)
		{
			string lining = stack.Attributes.GetString("lining");
			stack.Attributes.GetString("material");
			int v = LightHsv[2] + ((lining != "plain") ? 2 : 0);
			byte[] lightHsv = new byte[3]
			{
				LightHsv[0],
				LightHsv[1],
				(byte)v
			};
			BELantern.setLightColor(LightHsv, lightHsv, stack.Attributes.GetString("glass"));
			return lightHsv;
		}
		if (pos != null)
		{
			int v2 = LightHsv[2] + 3;
			return new byte[3]
			{
				LightHsv[0],
				LightHsv[1],
				(byte)v2
			};
		}
		return base.GetLightHsv(blockAccessor, pos, stack);
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.GetOrCreate(capi, baseCacheKey + "GuiMeshRefs", () => new Dictionary<string, MultiTextureMeshRef>());
		string material = itemstack.Attributes.GetString("material");
		string lining = itemstack.Attributes.GetString("lining");
		string glass = itemstack.Attributes.GetString("glass", "quartz");
		string key = material + "-" + lining + "-" + glass;
		if (!meshrefs.TryGetValue(key, out var meshref))
		{
			AssetLocation shapeloc = Shape.Base.CopyWithPathPrefixAndAppendixOnce("shapes/", ".json");
			Shape shape = Vintagestory.API.Common.Shape.TryGet(capi, shapeloc);
			MeshData mesh = GenMesh(capi, material, lining, glass, shape);
			meshref = (meshrefs[key] = capi.Render.UploadMultiTextureMesh(mesh));
		}
		renderinfo.ModelRef = meshref;
		renderinfo.CullFaces = false;
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		if (!(api is ICoreClientAPI capi) || !capi.ObjectCache.TryGetValue("blockLanternGuiMeshRefs", out var obj))
		{
			return;
		}
		foreach (KeyValuePair<string, MultiTextureMeshRef> item in obj as Dictionary<string, MultiTextureMeshRef>)
		{
			item.Value.Dispose();
		}
		capi.ObjectCache.Remove("blockLanternGuiMeshRefs");
	}

	public MeshData GenMesh(ICoreClientAPI capi, string material, string lining, string glassMaterial, Shape shape = null, ITesselatorAPI tesselator = null)
	{
		if (tesselator == null)
		{
			tesselator = capi.Tesselator;
		}
		tmpTextureSource = tesselator.GetTextureSource(this);
		if (shape == null)
		{
			shape = Vintagestory.API.Common.Shape.TryGet(capi, "shapes/" + Shape.Base.Path + ".json");
		}
		if (shape == null)
		{
			return null;
		}
		AtlasSize = capi.BlockTextureAtlas.Size;
		curMat = material;
		curLining = lining;
		Block glassBlock = capi.World.GetBlock(new AssetLocation("glass-" + glassMaterial));
		glassTextureSource = tesselator.GetTextureSource(glassBlock);
		tesselator.TesselateShape("blocklantern", shape, out var mesh, this, new Vec3f(Shape.rotateX, Shape.rotateY, Shape.rotateZ), 0, 0, 0);
		return mesh;
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		if (!base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack))
		{
			return false;
		}
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BELantern be)
		{
			string material = byItemStack.Attributes.GetString("material");
			string lining = byItemStack.Attributes.GetString("lining");
			string glass = byItemStack.Attributes.GetString("glass");
			be.DidPlace(material, lining, glass);
			BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
			double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
			double dz = byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
			float num = (float)Math.Atan2(y, dz);
			float deg22dot5rad = (float)Math.PI / 8f;
			float roundRad = (float)(int)Math.Round(num / deg22dot5rad) * deg22dot5rad;
			be.MeshAngle = roundRad;
		}
		return true;
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		ItemStack stack = new ItemStack(world.GetBlock(CodeWithParts("up")));
		if (world.BlockAccessor.GetBlockEntity(pos) is BELantern be)
		{
			stack.Attributes.SetString("material", be.material);
			stack.Attributes.SetString("lining", be.lining);
			stack.Attributes.SetString("glass", be.glass);
		}
		else
		{
			stack.Attributes.SetString("material", "copper");
			stack.Attributes.SetString("lining", "plain");
			stack.Attributes.SetString("glass", "plain");
		}
		return stack;
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		return new BlockDropItemStack[1]
		{
			new BlockDropItemStack(handbookStack)
		};
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		foreach (BlockBehavior obj in blockBehaviors)
		{
			EnumHandling handled = EnumHandling.PassThrough;
			obj.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handled);
			if (handled == EnumHandling.PreventDefault)
			{
				preventDefault = true;
			}
			if (handled == EnumHandling.PreventSubsequent)
			{
				return;
			}
		}
		if (preventDefault)
		{
			return;
		}
		if (world.Side == EnumAppSide.Server && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative))
		{
			ItemStack[] drops = new ItemStack[1] { OnPickBlock(world, pos) };
			if (drops != null)
			{
				for (int j = 0; j < drops.Length; j++)
				{
					world.SpawnItemEntity(drops[j], pos);
				}
			}
			world.PlaySoundAt(Sounds.GetBreakSound(byPlayer), pos, -0.5, byPlayer);
		}
		if (EntityClass != null)
		{
			world.BlockAccessor.GetBlockEntity(pos)?.OnBlockBroken(byPlayer);
		}
		world.BlockAccessor.SetBlock(0, pos);
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (!byPlayer.Entity.Controls.ShiftKey)
		{
			BELantern bel = world.BlockAccessor.GetBlockEntity<BELantern>(blockSel.Position);
			if (bel != null && bel.Interact(byPlayer))
			{
				return true;
			}
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		string material = itemStack.Attributes.GetString("material");
		return Lang.GetMatching(Code?.Domain + ":block-" + Code?.Path + "-" + material);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		string material = inSlot.Itemstack.Attributes.GetString("material");
		string lining = inSlot.Itemstack.Attributes.GetString("lining");
		string glass = inSlot.Itemstack.Attributes.GetString("glass");
		dsc.AppendLine(Lang.Get("Material: {0}", Lang.Get("material-" + material)));
		dsc.AppendLine(Lang.Get("Lining: {0}", (lining == "plain") ? "-" : Lang.Get("material-" + lining)));
		if (glass != null)
		{
			dsc.AppendLine(Lang.Get("Glass: {0}", Lang.Get("glass-" + glass)));
		}
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
	{
		if (capi.World.BlockAccessor.GetBlockEntity(pos) is BELantern be && Textures.TryGetValue(be.material, out var tex))
		{
			return capi.BlockTextureAtlas.GetRandomColor(tex.Baked.TextureSubId, rndIndex);
		}
		return base.GetRandomColor(capi, pos, facing, rndIndex);
	}

	public override List<ItemStack> GetHandBookStacks(ICoreClientAPI capi)
	{
		if (Code == null)
		{
			return null;
		}
		bool inCreativeTab = CreativeInventoryTabs != null && CreativeInventoryTabs.Length != 0;
		bool inCreativeTabStack = CreativeInventoryStacks != null && CreativeInventoryStacks.Length != 0;
		JsonObject attributes = Attributes;
		bool explicitlyIncluded = attributes != null && attributes["handbook"]?["include"].AsBool() == true;
		JsonObject attributes2 = Attributes;
		if (attributes2 != null && attributes2["handbook"]?["exclude"].AsBool() == true)
		{
			return null;
		}
		if (!explicitlyIncluded && !inCreativeTab && !inCreativeTabStack)
		{
			return null;
		}
		List<ItemStack> stacks = new List<ItemStack>();
		if (inCreativeTabStack)
		{
			for (int i = 0; i < CreativeInventoryStacks.Length; i++)
			{
				for (int j = 0; j < CreativeInventoryStacks[i].Stacks.Length; j++)
				{
					ItemStack stack = CreativeInventoryStacks[i].Stacks[j].ResolvedItemstack;
					stack.ResolveBlockOrItem(capi.World);
					stack = stack.Clone();
					stack.StackSize = stack.Collectible.MaxStackSize;
					if (!stacks.Any((ItemStack itemStack) => itemStack.Equals(stack)))
					{
						stacks.Add(stack);
						ItemStack otherGlass = stack.Clone();
						otherGlass.Attributes.SetString("glass", "plain");
						stacks.Add(otherGlass);
						ItemStack otherLiningSilver = stack.Clone();
						ItemStack otherLiningGold = stack.Clone();
						ItemStack otherLiningElectrum = stack.Clone();
						otherLiningSilver.Attributes.SetString("lining", "silver");
						otherLiningGold.Attributes.SetString("lining", "gold");
						otherLiningElectrum.Attributes.SetString("lining", "electrum");
						stacks.Add(otherLiningSilver);
						stacks.Add(otherLiningGold);
						stacks.Add(otherLiningElectrum);
					}
				}
			}
		}
		else
		{
			stacks.Add(new ItemStack(this));
		}
		return stacks;
	}

	public WorldInteraction[] GetContainedInteractionHelp(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		return Array.Empty<WorldInteraction>();
	}

	public bool OnContainedInteractStart(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		string lining = slot.Itemstack.Attributes.GetString("lining");
		string glass = slot.Itemstack.Attributes.GetString("glass");
		ItemSlot handSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (handSlot.Empty)
		{
			return false;
		}
		CollectibleObject obj = handSlot.Itemstack.Collectible;
		Block oldGlassBlock = be.Api.World.GetBlock(AssetLocation.Create("glass-" + glass));
		if (oldGlassBlock != null && obj.FirstCodePart() == "glass" && obj.Variant.ContainsKey("color"))
		{
			if (glass != "quartz" && byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
			{
				ItemStack stack = new ItemStack(oldGlassBlock);
				if (!byPlayer.InventoryManager.TryGiveItemstack(stack, slotNotifyEffect: true))
				{
					be.Api.World.SpawnItemEntity(stack, be.Pos.ToVec3d().Add(0.5, 0.0, 0.5));
				}
				be.Api.World.Logger.Audit("{0} Replaced glass {1} with {2} for Lantern in Ground Storage at {3}.", byPlayer.PlayerName, stack.Collectible.Code, obj.Code, be.Pos);
			}
			slot.Itemstack.Attributes.SetString("glass", obj.Variant["color"]);
			glass = obj.Variant["color"];
			if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative && glass != "quartz")
			{
				handSlot.TakeOut(1);
			}
			(byPlayer as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			be.Api.World.PlaySoundAt(oldGlassBlock?.Sounds.Place ?? GlobalConstants.DefaultBuildSound, be.Pos, -0.4, byPlayer);
			(be as BlockEntityGroundStorage)?.LightUpdate(slot.Itemstack);
			be.MarkDirty(redrawOnClient: true);
			return true;
		}
		bool flag = lining == null;
		if (!flag)
		{
			bool flag2 = lining == "plain" && obj is ItemMetalPlate;
			if (flag2)
			{
				bool flag3;
				switch (obj.Variant["metal"])
				{
				case "gold":
				case "silver":
				case "electrum":
					flag3 = true;
					break;
				default:
					flag3 = false;
					break;
				}
				flag2 = flag3;
			}
			flag = flag2;
		}
		if (flag)
		{
			slot.Itemstack.Attributes.SetString("lining", obj.Variant["metal"]);
			(byPlayer as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			be.Api.World.PlaySoundAt(new AssetLocation("sounds/block/plate"), be.Pos, -0.4, byPlayer);
			handSlot.TakeOut(1);
			be.MarkDirty(redrawOnClient: true);
			return true;
		}
		return false;
	}

	public bool OnContainedInteractStep(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		return false;
	}

	public void OnContainedInteractStop(float secondsUsed, BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
	}
}
