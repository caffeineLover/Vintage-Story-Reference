using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockBehaviorDisplay(Block block) : StrongBlockBehavior(block), ICustomSelectionBoxRender
{
	public PlacementSurface[] PlacementSurfaces;

	public CuboidfWithId[] SelectionBoxes;

	protected int maxXDivisions = 32;

	protected int maxZDivisions = 32;

	private ICoreClientAPI capi;

	public static Size3f DefaultItemSize = new Size3f(6f, 4f, 6f);

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		maxXDivisions = properties["maxXDivisions"].AsInt(32);
		maxZDivisions = properties["maxZDivisions"].AsInt(32);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		capi = api as ICoreClientAPI;
		capi?.Event.RegisterEventBusListener(OnGetTransform, 0.5, "ongettransform");
		base.OnLoaded(api);
		Shape shape = api.Assets.Get<Shape>(block.Shape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json"));
		List<PlacementSurface> psurfaces = new List<PlacementSurface>();
		List<CuboidfWithId> selBoxes = new List<CuboidfWithId>();
		int slotCount = 0;
		int i = 0;
		ShapeElement[] elements = shape.Elements;
		foreach (ShapeElement element in elements)
		{
			if (!element.Name.StartsWith("psurface"))
			{
				continue;
			}
			string name = element.Name;
			int length = "psurface".Length;
			string[] parts = name.Substring(length, name.Length - length).Split('-');
			name = parts[1];
			int width = name.Substring(1, name.Length - 1).ToInt();
			name = parts[2];
			int height = name.Substring(1, name.Length - 1).ToInt();
			name = parts[3];
			Size3i size = new Size3i(width, height, name.Substring(1, name.Length - 1).ToInt());
			Vec3f baseBos = new Vec3f((float)element.From[0], (float)element.From[1], (float)element.From[2]);
			psurfaces.Add(new PlacementSurface
			{
				ElementName = element.Name,
				Index = parts[0].ToInt(),
				Size = size,
				VoxelPosition = baseBos,
				DisplayCategory = ((parts.Length > 4) ? parts[4] : null)
			});
			int xDivisons = Math.Min(maxXDivisions, size.Width);
			int zDivisons = Math.Min(maxZDivisions, size.Length);
			double xstep = (element.To[0] - (double)baseBos.X) / (double)xDivisons;
			double zstep = (element.To[2] - (double)baseBos.Z) / (double)zDivisons;
			for (int x = 0; x < xDivisons; x++)
			{
				double x2 = (double)baseBos.X + (double)x * xstep;
				double x3 = (double)baseBos.X + (double)(x + 1) * xstep;
				for (int z = 0; z < zDivisons; z++)
				{
					double z2 = (double)baseBos.Z + (double)z * zstep;
					double z3 = (double)baseBos.Z + (double)(z + 1) * zstep;
					CuboidfWithId cubf = new CuboidfWithId(x2 / 16.0, (double)baseBos.Y / 16.0, z2 / 16.0, x3 / 16.0, (double)baseBos.Y / 16.0, z3 / 16.0);
					cubf.Id = i + "-" + (double)x * xstep + "-" + (double)z * zstep;
					selBoxes.Add(cubf);
					slotCount++;
				}
			}
			i++;
		}
		SelectionBoxes = selBoxes.ToArray();
		PlacementSurfaces = psurfaces.ToArray();
	}

	private void OnGetTransform(string eventName, ref EnumHandling handling, IAttribute data)
	{
		TreeAttribute tree = data as TreeAttribute;
		if (!(tree.GetString("target") != "onshelfTransform"))
		{
			DisplayableAttributes attr = GetDisplayableAttributes(capi.World.Player.InventoryManager.ActiveHotbarSlot, "shelf");
			if (attr?.Transform != null)
			{
				handling = EnumHandling.PreventDefault;
				tree.SetBool("preventDefault", value: true);
				attr.Transform.ToTreeAttribute(tree);
			}
		}
	}

	public void RenderSelectionBoxes(BlockSelection blockSel, RenderBoxDelegate renderBoxHandler)
	{
		float linewidthMul = 1.6f * capi.Settings.Float["wireframethickness"];
		BEBehaviorDisplay be = block.GetBEBehavior<BEBehaviorDisplay>(blockSel.Position);
		if (be == null)
		{
			renderBoxHandler(Cuboidf.Default(), linewidthMul, block.GetSelectionColor(capi, blockSel.Position));
			return;
		}
		Cuboidf[] selBoxes = block.GetSelectionBoxes(capi.World.BlockAccessor, blockSel.Position);
		ItemSlot heldSlot = capi.World.Player.Entity.RightHandItemSlot;
		string selectedSlotId = blockSel.SelectionBoxId;
		SlotLocation slotLoc = decodeSlotid(selectedSlotId);
		if (blockSel.SelectionBoxIndex >= selBoxes.Length)
		{
			return;
		}
		Cuboidf selBox = selBoxes[blockSel.SelectionBoxIndex];
		if (placingItemsPreview(be))
		{
			renderBoxHandler(selBox, linewidthMul, block.GetSelectionColor(capi, blockSel.Position));
			return;
		}
		if (selectedSlotId.StartsWith("p-"))
		{
			selBox = selBoxes.FirstOrDefault((Cuboidf box) => (box as CuboidfWithId)?.Id == selectedSlotId.Substring(2));
		}
		PlacementSurface psurface = PlacementSurfaces[slotLoc.PlacementSurfaceIndex];
		string displayType = psurface.DisplayCategory ?? "shelf";
		Size3f size = getItemSize(heldSlot, displayType);
		if (size != null)
		{
			bool placeable = true;
			_ = blockSel.SelectionBoxId;
			float wm = size.Width / 16f;
			float hm = size.Height / 16f;
			float lm = size.Length / 16f;
			Vec3f offset = getOffsetFromPreviewCuboid(size, be.MeshAngleRad);
			if (slotLoc.X + offset.X < 0f || slotLoc.Z + offset.Z < 0f || slotLoc.X + offset.X > (float)psurface.Size.Width - size.Width || slotLoc.Z + offset.Z > (float)psurface.Size.Length - size.Length)
			{
				placeable = false;
			}
			if ((float)psurface.Size.Width < size.Width || (float)psurface.Size.Height < size.Height || (float)psurface.Size.Length < size.Length)
			{
				placeable = false;
			}
			if (placeable && be.getCollidingSlotId(blockSel, new Cuboidf(size)) != null)
			{
				placeable = false;
			}
			selBox = selBox.RotatedCopyRad(0f, 0f - be.MeshAngleRad, 0f, new Vec3d(0.5, 0.0, 0.5));
			Cuboidf previewCuboid = new Cuboidf(0f, 0.0625f, 0f, wm, hm + 0.0625f, lm).Translate(selBox.Start).Translate(offset.X / 16f, 0f, offset.Z / 16f);
			previewCuboid = previewCuboid.RotatedCopyRad(0f, be.MeshAngleRad, 0f, new Vec3d(0.5, 0.0, 0.5));
			renderBoxHandler(previewCuboid, linewidthMul, placeable ? new Vec4f(0f, 0f, 0f, 0.5f) : new Vec4f(0.5f, 0f, 0f, 0.5f));
		}
	}

	public bool placingItemsPreview(BEBehaviorDisplay be)
	{
		ItemSlot heldSlot = capi.World.Player.InventoryManager.ActiveHotbarSlot;
		BlockSelection blockSel = capi?.World.Player.CurrentBlockSelection;
		string selectedSlotId = blockSel?.SelectionBoxId;
		if (heldSlot == null || heldSlot.Empty || selectedSlotId == null || heldSlot.Itemstack.Collectible.GetTool(heldSlot) == EnumTool.Wrench)
		{
			return true;
		}
		SlotLocation slotLoc = decodeSlotid(selectedSlotId);
		if (slotLoc == null)
		{
			return true;
		}
		string displayType = PlacementSurfaces[slotLoc.PlacementSurfaceIndex].DisplayCategory ?? "shelf";
		DisplayableAttributes dattr = GetDisplayableAttributes(heldSlot, displayType);
		if (dattr != null && dattr.Behavior == EnumDisplayableBehavior.Stacking && be.getCollidingSlotId(blockSel, new Cuboidf(dattr.Size)) != null)
		{
			return true;
		}
		return false;
	}

	public static Vec3f getOffsetFromPreviewCuboid(Size3f size, float rad)
	{
		return new Vec3f((0f - size.Width) / 2f, 0f, (0f - size.Length) / 2f);
	}

	public static Size3f getItemSize(ItemSlot slot, string displayType)
	{
		CollectibleObject collObj = slot.Itemstack?.Collectible;
		DisplayableAttributes interfaceSize = collObj?.GetCollectibleInterface<IDisplayableProps>()?.GetDisplayableProps(slot, displayType);
		if (interfaceSize != null)
		{
			return interfaceSize.Size;
		}
		return collObj?.Attributes?["displayable"][displayType]["size"].AsObject(DefaultItemSize);
	}

	public static DisplayableAttributes GetDisplayableAttributes(ItemSlot slot, string displayType)
	{
		CollectibleObject collObj = slot.Itemstack?.Collectible;
		DisplayableAttributes interfaceProps = collObj?.GetCollectibleInterface<IDisplayableProps>()?.GetDisplayableProps(slot, displayType);
		if (interfaceProps != null)
		{
			return interfaceProps;
		}
		DisplayableAttributes heldDAttr = collObj?.Attributes?["displayable"][displayType].AsObject<DisplayableAttributes>();
		if (heldDAttr != null)
		{
			return heldDAttr;
		}
		if (collObj != null && collObj.Attributes?.IsTrue("shelvable") == true)
		{
			return new DisplayableAttributes
			{
				Size = DefaultItemSize,
				Transform = (collObj?.Attributes["onshelfTransform"].AsObject<ModelTransform>() ?? collObj?.Attributes["onDisplayTransform"].AsObject<ModelTransform>())
			};
		}
		return null;
	}

	public static SlotLocation decodeSlotid(string slotid)
	{
		if (slotid == null)
		{
			return null;
		}
		bool placedItem = slotid.StartsWith("p-");
		string[] parts = (placedItem ? slotid.Substring(2) : slotid).Split('-');
		if (parts.Length < 3)
		{
			return null;
		}
		return new SlotLocation(parts[1].ToFloat(), (parts.Length > 3) ? parts[3].ToFloat() : 0f, parts[2].ToFloat())
		{
			PlacementSurfaceIndex = parts[0].ToInt(),
			IsPlacedItem = placedItem
		};
	}

	public override int GetColorWithoutTint(ICoreClientAPI capi, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		int texSubId = block.GetBEBehavior<BEBehaviorDisplay>(pos)?.GetParticleColorTextureSubId() ?? 0;
		return capi.BlockTextureAtlas.GetAverageColor(texSubId) | -16777216;
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		int texSubId = block.GetBEBehavior<BEBehaviorDisplay>(pos)?.GetParticleColorTextureSubId() ?? 0;
		return capi.BlockTextureAtlas.GetRandomColor(texSubId, rndIndex) | -16777216;
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
	{
		ItemStack displayStack = block.GetBEBehavior<BEBehaviorDisplay>(pos)?.DisplayStack;
		if (displayStack != null)
		{
			handling = EnumHandling.PreventDefault;
			return displayStack.Clone();
		}
		return base.OnPickBlock(world, pos, ref handling);
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		ItemStack displayStack = block.GetBEBehavior<BEBehaviorDisplay>(pos)?.DisplayStack;
		if (displayStack != null)
		{
			handling = EnumHandling.PreventDefault;
			return new ItemStack[1] { displayStack };
		}
		return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
	}

	public override void OnDecalTesselation(IWorldAccessor world, MeshData decalMesh, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		float? num = block.GetBEBehavior<BEBehaviorDisplay>(pos)?.MeshAngleRad;
		if (num.HasValue)
		{
			float meshAngleRad = num.GetValueOrDefault();
			decalMesh.Rotate(0f, meshAngleRad, 0f);
		}
	}
}
