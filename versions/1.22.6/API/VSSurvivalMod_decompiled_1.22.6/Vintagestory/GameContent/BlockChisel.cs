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

public class BlockChisel : BlockMicroBlock, IWrenchOrientable, IAttachableToEntity, IWearableShapeSupplier
{
	private WorldInteraction[] interactions;

	public int RequiresBehindSlots { get; set; }

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		interactions = new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "blockhelp-chisel-removedeco",
				MouseButton = EnumMouseButton.Right,
				Itemstacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Knife),
				GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
				{
					BlockEntityChisel blockEntity = GetBlockEntity<BlockEntityChisel>(bs.Position);
					return (blockEntity?.DecorIds != null && blockEntity.DecorIds[bs.Face.Index] != 0) ? wi.Itemstacks : null;
				}
			}
		};
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		StringArrayAttribute obj = inSlot.Itemstack.Attributes["materials"] as StringArrayAttribute;
		if (obj == null || obj.value.Length <= 1)
		{
			IntArrayAttribute obj2 = inSlot.Itemstack.Attributes["materials"] as IntArrayAttribute;
			if (obj2 == null || obj2.value.Length <= 1)
			{
				return;
			}
		}
		dsc.AppendLine(Lang.Get("<font color=\"lightblue\">Multimaterial chiseled block</font>"));
	}

	public void Rotate(EntityAgent byEntity, BlockSelection blockSel, int dir)
	{
		BlockEntityChisel bechisel = api.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityChisel;
		if (byEntity.Controls.CtrlKey)
		{
			int rot = bechisel.DecorRotations;
			int bitshift = blockSel.Face.Index * 3;
			int facerot = (rot >> bitshift) & 7;
			rot &= ~(7 << bitshift);
			rot += ((facerot + 1) & 7) << bitshift;
			bechisel.DecorRotations = rot;
		}
		else
		{
			bechisel.RotateModel((dir > 0) ? 90 : (-90), null);
		}
		bechisel.MarkDirty(redrawOnClient: true);
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		BlockEntityChisel obj = api.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityChisel;
		if (obj != null && obj.Interact(byPlayer, blockSel))
		{
			return true;
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityChisel)
		{
			ICoreClientAPI obj = api as ICoreClientAPI;
			if (obj != null && obj.World.Player.InventoryManager.ActiveTool == EnumTool.Chisel)
			{
				((ICoreClientAPI)api).Network.SendBlockEntityPacket(pos, 1011);
				return null;
			}
		}
		return base.OnPickBlock(world, pos);
	}

	public override bool TryToRemoveSoilFirst(IWorldAccessor world, BlockPos pos, IPlayer byPlayer)
	{
		return false;
	}

	public override bool IsSoilNonSoilMix(BlockEntityMicroBlock be)
	{
		return false;
	}

	public override bool IsSoilNonSoilMix(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return false;
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return interactions.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}

	public virtual void CollectTextures(ItemStack stack, Shape shape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
	{
		int[] blockIds = BlockEntityMicroBlock.MaterialIdsFromAttributes(stack.Attributes, api.World);
		uint[] voxelCuboids = (stack.Attributes["cuboids"] as IntArrayAttribute)?.AsUint;
		if (voxelCuboids == null)
		{
			return;
		}
		CuboidWithMaterial cwm = new CuboidWithMaterial();
		for (int i = 0; i < voxelCuboids.Length; i++)
		{
			BlockEntityMicroBlock.FromUint(voxelCuboids[i], cwm);
			Block block = api.World.Blocks[blockIds[cwm.Material]];
			BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
			foreach (BlockFacing facing in aLLFACES)
			{
				string texCode = texturePrefixCode + "-" + i + "-" + block.Code.ToShortString() + "-" + facing.Code;
				if (intoDict.ContainsKey(texCode))
				{
					continue;
				}
				if (!block.Textures.TryGetValue(facing.Code, out var ctex))
				{
					if (facing.IsVertical)
					{
						block.Textures.TryGetValue("vertical", out ctex);
					}
					else
					{
						block.Textures.TryGetValue("horizontal", out ctex);
					}
					if (ctex == null)
					{
						block.Textures.TryGetValue("all", out ctex);
					}
					if (ctex == null)
					{
						block.Textures.TryGetValue("material", out ctex);
					}
					if (ctex == null && block.Textures.Count > 0)
					{
						ctex = block.Textures.First().Value;
					}
				}
				if (ctex != null)
				{
					intoDict[texCode] = ctex.Clone();
					intoDict[texCode].Base.Path = intoDict[texCode].Base.Path.Replace("*", "1");
				}
			}
		}
	}

	public virtual Shape GetShape(ItemStack stack, Entity forEntity, string texturePrefixCode)
	{
		float scale = 1f;
		Vec3f offset = new Vec3f(-4f, 0f, -6f);
		int[] blockIds = BlockEntityMicroBlock.MaterialIdsFromAttributes(stack.Attributes, forEntity.World);
		uint[] voxelCuboids = (stack.Attributes["cuboids"] as IntArrayAttribute)?.AsUint;
		if (voxelCuboids == null)
		{
			return null;
		}
		Shape shape = new Shape
		{
			Elements = new ShapeElement[voxelCuboids.Length],
			Textures = new Dictionary<string, AssetLocation>(),
			TextureWidth = 16,
			TextureHeight = 16
		};
		CuboidWithMaterial cwm = new CuboidWithMaterial();
		for (int i = 0; i < voxelCuboids.Length; i++)
		{
			BlockEntityMicroBlock.FromUint(voxelCuboids[i], cwm);
			Block block = forEntity.World.Blocks[blockIds[cwm.Material]];
			ShapeElement shapeElement = new ShapeElement();
			shapeElement.Name = "Cuboid" + i;
			shapeElement.From = new double[3]
			{
				offset.X + (float)cwm.X1 * scale,
				offset.Y + (float)cwm.Y1 * scale,
				offset.Z + (float)cwm.Z1 * scale
			};
			shapeElement.To = new double[3]
			{
				offset.X + (float)cwm.X2 * scale,
				offset.Y + (float)cwm.Y2 * scale,
				offset.Z + (float)cwm.Z2 * scale
			};
			shapeElement.FacesResolved = new ShapeElementFace[6];
			ShapeElement elem = shapeElement;
			BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
			foreach (BlockFacing facing in aLLFACES)
			{
				float[] uv = facing.Index switch
				{
					0 => new float[4]
					{
						16 - cwm.X2,
						16 - cwm.Y2,
						16 - cwm.X1,
						16 - cwm.Y1
					}, 
					1 => new float[4]
					{
						16 - cwm.Z2,
						16 - cwm.Y2,
						16 - cwm.Z1,
						16 - cwm.Y1
					}, 
					2 => new float[4]
					{
						cwm.X1,
						16 - cwm.Y2,
						cwm.X2,
						16 - cwm.Y1
					}, 
					3 => new float[4]
					{
						cwm.Z1,
						16 - cwm.Y2,
						cwm.Z2,
						16 - cwm.Y1
					}, 
					4 => new float[4]
					{
						16 - cwm.X1,
						16 - cwm.Z1,
						16 - cwm.X2,
						16 - cwm.Z2
					}, 
					5 => new float[4] { cwm.X1, cwm.Z1, cwm.X2, cwm.Z2 }, 
					_ => new float[4] { 0f, 0f, 16f, 16f }, 
				};
				string texCode = texturePrefixCode + "-" + i + "-" + block.Code.ToShortString() + "-" + facing.Code;
				elem.FacesResolved[facing.Index] = new ShapeElementFace
				{
					Texture = texCode,
					Uv = uv
				};
			}
			shape.Elements[i] = elem;
		}
		return shape;
	}

	public CompositeShape GetAttachedShape(ItemStack stack, string slotCode)
	{
		return null;
	}

	public string GetCategoryCode(ItemStack stack)
	{
		return "chiseled";
	}

	public string[] GetDisableElements(ItemStack stack)
	{
		return Array.Empty<string>();
	}

	public string[] GetKeepElements(ItemStack stack)
	{
		return Array.Empty<string>();
	}

	public bool IsAttachable(Entity toEntity, ItemStack itemStack)
	{
		return true;
	}

	public string GetTexturePrefixCode(ItemStack stack)
	{
		int[] BlockIds = BlockEntityMicroBlock.MaterialIdsFromAttributes(stack.Attributes, api.World);
		uint[] VoxelCuboids = (stack.Attributes["cuboids"] as IntArrayAttribute)?.AsUint;
		return string.Concat("chiseled-" + "blockids:-" + string.Join("-", BlockIds), "cuboids:-", string.Join("-", VoxelCuboids));
	}
}
