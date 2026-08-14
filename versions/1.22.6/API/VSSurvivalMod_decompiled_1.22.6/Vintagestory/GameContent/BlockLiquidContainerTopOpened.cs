using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockLiquidContainerTopOpened : BlockLiquidContainerBase, IContainedMeshSource, IContainedCustomName, IContainedInteractable
{
	private LiquidTopOpenContainerProps Props;

	private MeshData origcontainermesh;

	private Shape contentShape;

	private Shape liquidContentShape;

	protected virtual string meshRefsCacheKey => Code.ToShortString() + "meshRefs";

	protected virtual AssetLocation emptyShapeLoc => Props.EmptyShapeLoc;

	protected virtual AssetLocation contentShapeLoc => Props.OpaqueContentShapeLoc;

	protected virtual AssetLocation liquidContentShapeLoc => Props.LiquidContentShapeLoc;

	public override float TransferSizeLitres => Props.TransferSizeLitres;

	public override float CapacityLitres => Props.CapacityLitres;

	public override bool CanDrinkFrom => true;

	public override bool IsTopOpened => true;

	public override bool AllowHeldLiquidTransfer => true;

	protected virtual float liquidMaxYTranslate => Props.LiquidMaxYTranslate;

	protected virtual float liquidYTranslatePerLitre => liquidMaxYTranslate / CapacityLitres;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		Props = new LiquidTopOpenContainerProps();
		JsonObject attributes = Attributes;
		if (attributes != null && attributes["liquidContainerProps"].Exists)
		{
			Props = Attributes["liquidContainerProps"].AsObject<LiquidTopOpenContainerProps>(null, Code.Domain);
		}
	}

	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		object obj;
		Dictionary<int, MultiTextureMeshRef> meshrefs = (Dictionary<int, MultiTextureMeshRef>)(capi.ObjectCache.TryGetValue(meshRefsCacheKey, out obj) ? (obj as Dictionary<int, MultiTextureMeshRef>) : (capi.ObjectCache[meshRefsCacheKey] = new Dictionary<int, MultiTextureMeshRef>()));
		ItemStack contentStack = GetContent(itemstack);
		if (contentStack != null)
		{
			int hashcode = GetStackCacheHashCode(contentStack);
			if (!meshrefs.TryGetValue(hashcode, out var meshRef))
			{
				MeshData meshdata = GenMesh(capi, contentStack);
				meshRef = (meshrefs[hashcode] = capi.Render.UploadMultiTextureMesh(meshdata));
			}
			renderinfo.ModelRef = meshRef;
		}
	}

	protected int GetStackCacheHashCode(ItemStack contentStack)
	{
		return (contentStack.StackSize + "x" + contentStack.Collectible.Code.ToShortString()).GetHashCode();
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		if (!(api is ICoreClientAPI capi) || !capi.ObjectCache.TryGetValue(meshRefsCacheKey, out var obj))
		{
			return;
		}
		foreach (KeyValuePair<int, MultiTextureMeshRef> item in obj as Dictionary<int, MultiTextureMeshRef>)
		{
			item.Value.Dispose();
		}
		capi.ObjectCache.Remove(meshRefsCacheKey);
	}

	public MeshData GenMesh(ICoreClientAPI capi, ItemStack contentStack, BlockPos forBlockPos = null)
	{
		if (origcontainermesh == null)
		{
			Shape shape = Vintagestory.API.Common.Shape.TryGet(capi, emptyShapeLoc.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/"));
			if (shape == null)
			{
				capi.World.Logger.Error("Empty shape {0} not found. Liquid container {1} will be invisible.", emptyShapeLoc, Code);
				return new MeshData();
			}
			capi.Tesselator.TesselateShape(this, shape, out origcontainermesh, new Vec3f(Shape.rotateX, Shape.rotateY, Shape.rotateZ));
		}
		MeshData containerMesh = origcontainermesh.Clone();
		if (contentStack != null)
		{
			WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(contentStack);
			if (props == null)
			{
				capi.World.Logger.Error("Contents ('{0}') has no liquid properties, contents of liquid container {1} will be invisible.", contentStack.GetName(), Code);
				return containerMesh;
			}
			ContainerTextureSource contentSource = new ContainerTextureSource(capi, contentStack, props.Texture);
			Shape shape2 = (props.IsOpaque ? contentShape : liquidContentShape);
			AssetLocation loc = (props.IsOpaque ? contentShapeLoc : liquidContentShapeLoc);
			if (shape2 == null)
			{
				shape2 = Vintagestory.API.Common.Shape.TryGet(capi, loc.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/"));
				if (props.IsOpaque)
				{
					contentShape = shape2;
				}
				else
				{
					liquidContentShape = shape2;
				}
			}
			if (shape2 == null)
			{
				capi.World.Logger.Error("Content shape {0} not found. Contents of liquid container {1} will be invisible.", loc, Code);
				return containerMesh;
			}
			capi.Tesselator.TesselateShape(GetType().Name, shape2, out var contentMesh, contentSource, new Vec3f(Shape.rotateX, Shape.rotateY, Shape.rotateZ), props.GlowLevel, 0, 0);
			contentMesh.Translate(0f, GameMath.Min(liquidMaxYTranslate, (float)contentStack.StackSize / props.ItemsPerLitre * liquidYTranslatePerLitre), 0f);
			if (props.ClimateColorMap != null)
			{
				int col = ((!(forBlockPos != null)) ? capi.World.ApplyColorMapOnRgba(props.ClimateColorMap, null, -1, 196, 128, flipRb: false) : capi.World.ApplyColorMapOnRgba(props.ClimateColorMap, null, -1, forBlockPos.X, forBlockPos.Y, forBlockPos.Z, flipRb: false));
				byte[] rgba = ColorUtil.ToBGRABytes(col);
				for (int i = 0; i < contentMesh.Rgba.Length; i++)
				{
					contentMesh.Rgba[i] = (byte)(contentMesh.Rgba[i] * rgba[i % 4] / 255);
				}
			}
			for (int j = 0; j < contentMesh.FlagsCount; j++)
			{
				contentMesh.Flags[j] = contentMesh.Flags[j] & -4097;
			}
			containerMesh.AddMeshData(contentMesh);
			if (forBlockPos != null)
			{
				containerMesh.CustomInts = new CustomMeshDataPartInt(containerMesh.FlagsCount);
				containerMesh.CustomInts.Count = containerMesh.FlagsCount;
				containerMesh.CustomInts.Values.Fill(268435456);
				containerMesh.CustomFloats = new CustomMeshDataPartFloat(containerMesh.FlagsCount * 2);
				containerMesh.CustomFloats.Count = containerMesh.FlagsCount * 2;
			}
		}
		return containerMesh;
	}

	public MeshData GenMesh(ItemSlot slot, ITextureAtlasAPI targetAtlas, BlockPos forBlockPos)
	{
		ItemStack contentStack = GetContent(slot.Itemstack);
		return GenMesh(api as ICoreClientAPI, contentStack, forBlockPos);
	}

	public string GetMeshCacheKey(ItemSlot slot)
	{
		ItemStack contentStack = GetContent(slot.Itemstack);
		return slot.Itemstack.Collectible.Code.ToShortString() + "-" + contentStack?.StackSize + "x" + contentStack?.Collectible.Code.ToShortString();
	}

	public string GetContainedInfo(ItemSlot inSlot)
	{
		float litres = GetCurrentLitres(inSlot.Itemstack);
		ItemStack contentStack = GetContent(inSlot.Itemstack);
		if (litres <= 0f)
		{
			return Lang.GetWithFallback("contained-empty-container", "{0} (Empty)", inSlot.Itemstack.GetName());
		}
		string incontainername = Lang.Get(contentStack.Collectible.Code.Domain + ":incontainer-" + contentStack.Class.ToString().ToLowerInvariant() + "-" + contentStack.Collectible.Code.Path);
		return Lang.Get("contained-liquidcontainer-compact", inSlot.Itemstack.GetName(), litres, incontainername, PerishableInfoCompactContainer(api, inSlot));
	}

	public string GetContainedName(ItemSlot inSlot, int quantity)
	{
		return inSlot.Itemstack.GetName();
	}

	public bool OnContainedInteractStart(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (hotbarSlot.Empty || !(hotbarSlot.Itemstack.Collectible is ILiquidInterface heldObj))
		{
			return false;
		}
		bool singleTake = byPlayer.WorldData.EntityControls.ShiftKey;
		bool singlePut = byPlayer.WorldData.EntityControls.CtrlKey;
		if (!singleTake)
		{
			ILiquidSource liquidSource = heldObj as ILiquidSource;
			if (liquidSource != null && liquidSource.AllowHeldLiquidTransfer)
			{
				ItemStack contentStackToMove = liquidSource.GetContent(hotbarSlot.Itemstack);
				int moved = TryPutLiquid(slot.Itemstack, contentStackToMove, singlePut ? liquidSource.TransferSizeLitres : liquidSource.CapacityLitres);
				if (moved > 0)
				{
					SplitStackAndPerformAction(byPlayer.Entity, hotbarSlot, delegate(ItemStack stack)
					{
						liquidSource.TryTakeContent(stack, moved);
						return moved;
					});
					DoLiquidMovedEffects(byPlayer, contentStackToMove, moved, EnumLiquidDirection.Pour);
					be.MarkDirty();
					return true;
				}
			}
		}
		if (!singlePut)
		{
			ILiquidSink liquidSink = heldObj as ILiquidSink;
			if (liquidSink != null && liquidSink.AllowHeldLiquidTransfer)
			{
				ItemStack owncontentStack = GetContent(slot.Itemstack);
				if (owncontentStack != null)
				{
					BlockLiquidContainerBase heldLiquidContainer = liquidSink as BlockLiquidContainerBase;
					float litres = (singleTake ? liquidSink.TransferSizeLitres : liquidSink.CapacityLitres);
					int moved2 = heldLiquidContainer?.SplitStackAndPerformAction(byPlayer.Entity, hotbarSlot, (ItemStack stack) => liquidSink.TryPutLiquid(stack, owncontentStack, litres)) ?? liquidSink.TryPutLiquid(hotbarSlot.Itemstack, owncontentStack, litres);
					if (moved2 > 0)
					{
						TryTakeContent(slot.Itemstack, moved2);
						heldLiquidContainer?.DoLiquidMovedEffects(byPlayer, owncontentStack, moved2, EnumLiquidDirection.Fill);
						be.MarkDirty();
						return true;
					}
				}
			}
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

	public WorldInteraction[] GetContainedInteractionHelp(BlockEntityContainer be, ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
	{
		bool notProtected = true;
		if (be.Api.World.Claims != null && be.Api.World is IClientWorldAccessor clientWorld)
		{
			IClientPlayer player = clientWorld.Player;
			if (player != null && player.WorldData.CurrentGameMode == EnumGameMode.Survival && clientWorld.Claims.TestAccess(clientWorld.Player, blockSel.Position, EnumBlockAccessFlags.Use) != EnumWorldAccessResponse.Granted)
			{
				notProtected = false;
			}
		}
		if (notProtected)
		{
			return base.interactions;
		}
		return Array.Empty<WorldInteraction>();
	}
}
