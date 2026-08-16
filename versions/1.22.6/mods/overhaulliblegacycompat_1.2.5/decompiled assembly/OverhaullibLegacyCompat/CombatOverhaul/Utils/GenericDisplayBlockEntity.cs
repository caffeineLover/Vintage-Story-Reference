using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	public class GenericDisplayBlockEntity : GenericDisplayProto
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000AE5C File Offset: 0x0000905C
		public override string InventoryClassName
		{
			get
			{
				return "display-generic";
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000AE63 File Offset: 0x00009063
		public override InventoryBase Inventory
		{
			get
			{
				return this._inventory;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000AE6B File Offset: 0x0000906B
		public override string AttributeTransformCode
		{
			get
			{
				return this._inventoryTranformName;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000AE73 File Offset: 0x00009073
		public GenericDisplayBlockEntity()
		{
			this._inventory = new InventoryDisplayed(this, 16, "display-generic", null, null);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AE9C File Offset: 0x0000909C
		public override void CreateBehaviors(Block block, IWorldAccessor worldForResolve)
		{
			base.CreateBehaviors(block, worldForResolve);
			if (block.Attributes == null)
			{
				return;
			}
			block.Attributes["inventorySize"].AsInt(1);
			string transform = block.Attributes["inventoryTransformAttribute"].AsString("inGenericDisplayTransform");
			JsonObject jsonObject = block.Attributes["rotate"];
			float? num;
			if (jsonObject == null)
			{
				num = default(float?);
			}
			else
			{
				JsonObject jsonObject2 = jsonObject["x"];
				num = ((jsonObject2 != null) ? new float?(jsonObject2.AsFloat(0f)) : default(float?));
			}
			float? num2 = num;
			float rotateX = num2.GetValueOrDefault();
			JsonObject jsonObject3 = block.Attributes["rotate"];
			float? num3;
			if (jsonObject3 == null)
			{
				num3 = default(float?);
			}
			else
			{
				JsonObject jsonObject4 = jsonObject3["y"];
				num3 = ((jsonObject4 != null) ? new float?(jsonObject4.AsFloat(0f)) : default(float?));
			}
			num2 = num3;
			float rotateY = num2.GetValueOrDefault();
			JsonObject jsonObject5 = block.Attributes["rotate"];
			float? num4;
			if (jsonObject5 == null)
			{
				num4 = default(float?);
			}
			else
			{
				JsonObject jsonObject6 = jsonObject5["z"];
				num4 = ((jsonObject6 != null) ? new float?(jsonObject6.AsFloat(0f)) : default(float?));
			}
			num2 = num4;
			float rotateZ = num2.GetValueOrDefault();
			JsonObject[] rotations = Array.Empty<JsonObject>();
			if (block.Attributes.KeyExists("rotations"))
			{
				rotations = block.Attributes["rotations"].AsArray();
			}
			this._inventoryTranformName = transform;
			IEnumerable<Cuboidf> selectionBoxes = block.SelectionBoxes;
			Func<Cuboidf, Vec3f> func;
			if ((func = GenericDisplayBlockEntity.<>O.<0>__GetCentre) == null)
			{
				func = (GenericDisplayBlockEntity.<>O.<0>__GetCentre = new Func<Cuboidf, Vec3f>(GenericDisplayBlockEntity.GetCentre));
			}
			Vec3f[] selectionBoxesTranslations = Enumerable.ToArray<Vec3f>(Enumerable.Select<Cuboidf, Vec3f>(selectionBoxes, func));
			this._transforms = new float[selectionBoxesTranslations.Length][];
			for (int index = 0; index < selectionBoxesTranslations.Length; index++)
			{
				float x = selectionBoxesTranslations[index].X;
				float y = selectionBoxesTranslations[index].Y;
				float z = selectionBoxesTranslations[index].Z;
				float rotateXByIndex = 0f;
				float rotateYByIndex = 0f;
				float rotateZByIndex = 0f;
				if (rotations.Length > index)
				{
					rotateXByIndex = rotations[index]["x"].AsFloat(0f);
					rotateYByIndex = rotations[index]["y"].AsFloat(0f);
					rotateZByIndex = rotations[index]["z"].AsFloat(0f);
				}
				this._transforms[index] = new Matrixf().Translate(x, y, z).RotateX(rotateXByIndex * 0.017453292f).RotateY(rotateYByIndex * 0.017453292f).RotateZ(rotateZByIndex * 0.017453292f).RotateX(rotateX * 0.017453292f).RotateY(rotateY * 0.017453292f).RotateZ(rotateZ * 0.017453292f).Values;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B16C File Offset: 0x0000936C
		protected override string getMeshCacheKey(ItemStack stack)
		{
			IContainedMeshSource containedMeshSource = stack.Collectible as IContainedMeshSource;
			if (containedMeshSource != null)
			{
				return containedMeshSource.GetMeshCacheKey(new DummySlot(stack));
			}
			ITreeAttribute attributes = stack.Attributes;
			int renderVariant = (attributes != null) ? attributes.GetInt("renderVariant", 0) : 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(stack.Collectible.Code);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<int>(renderVariant);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(this._inventoryTranformName);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B200 File Offset: 0x00009400
		protected static Vec3f GetCentre(Cuboidf box)
		{
			return new Vec3f(box.X1 + (box.X2 - box.X1) / 2f, box.Y1 + (box.Y2 - box.Y1) / 2f, box.Z1 + (box.Z2 - box.Z1) / 2f);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000B260 File Offset: 0x00009460
		public virtual bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (slot.Empty)
			{
				return this.TryTake(byPlayer, blockSel);
			}
			CollectibleObject colObj = slot.Itemstack.Collectible;
			if (!this.CanDisplayCollectible(colObj))
			{
				ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
				if (coreClientAPI != null)
				{
					coreClientAPI.TriggerIngameError(this, "doesnotfit", Lang.Get("This item does not fit.", Array.Empty<object>()));
				}
				return true;
			}
			if (this.TryPut(slot, blockSel, byPlayer))
			{
				this.Api.World.PlaySoundAt(new AssetLocation("sounds/player/build"), byPlayer.Entity, byPlayer, true, 16f, 1f);
				return true;
			}
			return false;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B30C File Offset: 0x0000950C
		protected virtual bool TryPut(ItemSlot slot, BlockSelection blockSel, IPlayer player)
		{
			int index = blockSel.SelectionBoxIndex;
			if (this._inventory[index].Empty)
			{
				int num = slot.TryPutInto(this.Api.World, this._inventory[index], 1);
				if (num > 0)
				{
					this.updateMesh(index, false);
					this.MarkDirty(true, null);
				}
				return num > 0;
			}
			return false;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B36C File Offset: 0x0000956C
		protected virtual bool TryTake(IPlayer byPlayer, BlockSelection blockSel)
		{
			int index = blockSel.SelectionBoxIndex;
			if (!this._inventory[index].Empty)
			{
				ItemStack stack = this._inventory[index].TakeOut(1);
				if (byPlayer.InventoryManager.TryGiveItemstack(stack, false))
				{
					this.Api.World.PlaySoundAt(new AssetLocation("sounds/player/build"), byPlayer.Entity, byPlayer, true, 16f, 1f);
				}
				if (stack.StackSize > 0)
				{
					this.Api.World.SpawnItemEntity(stack, this.Pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
				}
				this.updateMesh(index, false);
				this.MarkDirty(true, null);
				return true;
			}
			return false;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000B43E File Offset: 0x0000963E
		protected override float[][] genTransformationMatrices()
		{
			return this._transforms;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000B448 File Offset: 0x00009648
		protected virtual bool MatchesAcceptedWildcards(CollectibleObject collectible)
		{
			Block block = base.Block;
			string[] array;
			if (block == null)
			{
				array = null;
			}
			else
			{
				JsonObject attributes = block.Attributes;
				if (attributes == null)
				{
					array = null;
				}
				else
				{
					JsonObject jsonObject = attributes["acceptedWildcards"];
					array = ((jsonObject != null) ? jsonObject.AsArray<string>(null, "game") : null);
				}
			}
			string[] wildcards = array ?? Array.Empty<string>();
			return wildcards.Length != 0 && !(((collectible != null) ? collectible.Code : null) == null) && (WildcardUtil.Match(wildcards, collectible.Code.Path) || WildcardUtil.Match(wildcards, collectible.Code.ToString()));
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000B4D4 File Offset: 0x000096D4
		protected virtual bool MatchesAcceptedItemTags(CollectibleObject collectible)
		{
			GenericDisplayBlockEntity.<>c__DisplayClass19_0 CS$<>8__locals1 = new GenericDisplayBlockEntity.<>c__DisplayClass19_0();
			Block block = base.Block;
			string[] array;
			if (block == null)
			{
				array = null;
			}
			else
			{
				JsonObject attributes = block.Attributes;
				if (attributes == null)
				{
					array = null;
				}
				else
				{
					JsonObject jsonObject = attributes["acceptedItemTags"];
					array = ((jsonObject != null) ? jsonObject.AsArray<string>(null, "game") : null);
				}
			}
			string[] acceptedTags = array ?? Array.Empty<string>();
			if (acceptedTags.Length == 0 || ((collectible != null) ? collectible.Attributes : null) == null)
			{
				return false;
			}
			GenericDisplayBlockEntity.<>c__DisplayClass19_0 CS$<>8__locals2 = CS$<>8__locals1;
			JsonObject jsonObject2 = collectible.Attributes["tags"];
			CS$<>8__locals2.collectibleTags = (((jsonObject2 != null) ? jsonObject2.AsArray<string>(null, "game") : null) ?? Array.Empty<string>());
			return Enumerable.Any<string>(acceptedTags, (string tag) => ArrayExtensions.Contains<string>(CS$<>8__locals1.collectibleTags, tag));
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000B580 File Offset: 0x00009780
		protected virtual bool MatchesWallmountFallback(CollectibleObject collectible)
		{
			Block block = base.Block;
			string text;
			if (block == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = block.Code;
				text = ((code != null) ? code.Path : null);
			}
			string blockCode = text ?? string.Empty;
			if (!blockCode.Contains("mount", 5))
			{
				return false;
			}
			if (((collectible != null) ? collectible.Attributes : null) == null)
			{
				return false;
			}
			JsonObject jsonObject = collectible.Attributes["tags"];
			string[] collectibleTags = ((jsonObject != null) ? jsonObject.AsArray<string>(null, "game") : null) ?? Array.Empty<string>();
			JsonObject jsonObject2 = collectible.Attributes["tool"];
			string tool = ((jsonObject2 != null) ? jsonObject2.AsString(null) : null) ?? string.Empty;
			if (blockCode.Contains("shield", 5))
			{
				return ArrayExtensions.Contains<string>(collectibleTags, "shield") || string.Equals(tool, "shield", 5);
			}
			if (blockCode.Contains("crossbow", 5))
			{
				return ArrayExtensions.Contains<string>(collectibleTags, "crossbow");
			}
			return ArrayExtensions.Contains<string>(collectibleTags, "firearm") || ArrayExtensions.Contains<string>(collectibleTags, "crossbow") || ArrayExtensions.Contains<string>(collectibleTags, "shield");
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000B694 File Offset: 0x00009894
		protected virtual bool CanDisplayCollectible(CollectibleObject collectible)
		{
			return ((collectible != null) ? collectible.Attributes : null) != null && (collectible.Attributes.KeyExists(this.AttributeTransformCode) || this.MatchesAcceptedWildcards(collectible) || this.MatchesAcceptedItemTags(collectible) || this.MatchesWallmountFallback(collectible));
		}

		// Token: 0x04000104 RID: 260
		private readonly InventoryGeneric _inventory;

		// Token: 0x04000105 RID: 261
		private string _inventoryTranformName = "inGenericDisplayTransform";

		// Token: 0x04000106 RID: 262
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private float[][] _transforms;

		// Token: 0x04000107 RID: 263
		public const int MaxSlots = 16;

		// Token: 0x020001B4 RID: 436
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000A8E RID: 2702
			[Nullable(0)]
			public static Func<Cuboidf, Vec3f> <0>__GetCentre;
		}
	}
}
