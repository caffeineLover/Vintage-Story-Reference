using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000014 RID: 20
	public class BETreeHollowPlaced : BlockEntityOpenableContainer
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000F675 File Offset: 0x0000D875
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000F67D File Offset: 0x0000D87D
		public virtual float MeshAngle
		{
			get
			{
				return this.meshangle;
			}
			set
			{
				this.meshangle = value;
				this.rendererRot.Y = value * 57.295776f;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000F698 File Offset: 0x0000D898
		public virtual string DialogTitle
		{
			get
			{
				return Lang.Get(this.dialogTitleLangCode, Array.Empty<object>());
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000F6AA File Offset: 0x0000D8AA
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000F6B2 File Offset: 0x0000D8B2
		public override string InventoryClassName
		{
			get
			{
				return this.inventoryClassName;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000F6BA File Offset: 0x0000D8BA
		private BlockEntityAnimationUtil AnimUtil
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000F710 File Offset: 0x0000D910
		public override void Initialize(ICoreAPI api)
		{
			JsonObject attributes = base.Block.Attributes;
			string text;
			if (attributes == null)
			{
				text = null;
			}
			else
			{
				JsonObject jsonObject = attributes["defaultType"];
				text = ((jsonObject != null) ? jsonObject.AsString("normal-generic") : null);
			}
			this.defaultType = text;
			if (this.defaultType == null)
			{
				this.defaultType = "normal-generic";
			}
			if (this.inventory == null)
			{
				this.InitInventory(base.Block);
			}
			base.Initialize(api);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000F780 File Offset: 0x0000D980
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			if (((byItemStack != null) ? byItemStack.Attributes : null) != null)
			{
				string nowType = byItemStack.Attributes.GetString("type", this.defaultType);
				if (nowType != this.type)
				{
					this.type = nowType;
					this.InitInventory(base.Block);
					this.LateInitInventory();
				}
			}
			base.OnBlockPlaced(null);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000F7E0 File Offset: 0x0000D9E0
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			string prevType = this.type;
			this.type = tree.GetString("type", this.defaultType);
			this.MeshAngle = tree.GetFloat("meshAngle", this.MeshAngle);
			if (this.inventory == null)
			{
				if (tree.HasAttribute("forBlockId"))
				{
					this.InitInventory(worldForResolving.GetBlock((int)((ushort)tree.GetInt("forBlockId", 0))));
				}
				else if (tree.HasAttribute("forBlockCode"))
				{
					this.InitInventory(worldForResolving.GetBlock(new AssetLocation(tree.GetString("forBlockCode", null))));
				}
				else
				{
					tree.GetTreeAttribute("inventory").GetInt("qslots", 0);
					this.InitInventory(null);
				}
			}
			else if (this.type != prevType)
			{
				this.InitInventory(base.Block);
				if (this.Api == null)
				{
					this.Api = worldForResolving.Api;
				}
				this.LateInitInventory();
			}
			if (this.Api != null && this.Api.Side == 2)
			{
				this.ownMesh = null;
				this.MarkDirty(true, null);
			}
			base.FromTreeAttributes(tree, worldForResolving);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000F904 File Offset: 0x0000DB04
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			if (base.Block != null)
			{
				tree.SetString("forBlockCode", base.Block.Code.ToShortString());
			}
			if (this.type == null)
			{
				this.type = this.defaultType;
			}
			tree.SetString("type", this.type);
			tree.SetFloat("meshAngle", this.MeshAngle);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000F974 File Offset: 0x0000DB74
		protected virtual void InitInventory(Block block)
		{
			block = base.Block;
			if (((block != null) ? block.Attributes : null) != null)
			{
				JsonObject jsonObject = block.Attributes["collisionSelectionBoxes"];
				Cuboidf[] array;
				if (jsonObject == null)
				{
					array = null;
				}
				else
				{
					JsonObject jsonObject2 = jsonObject[this.type];
					array = ((jsonObject2 != null) ? jsonObject2.AsObject<Cuboidf[]>(null) : null);
				}
				this.collisionSelectionBoxes = array;
				this.inventoryClassName = block.Attributes["inventoryClassName"].AsString(this.inventoryClassName);
				this.dialogTitleLangCode = block.Attributes["dialogTitleLangCode"][this.type].AsString(this.dialogTitleLangCode);
				this.quantitySlots = block.Attributes["quantitySlots"][this.type].AsInt(this.quantitySlots);
				this.quantityColumns = block.Attributes["quantityColumns"][this.type].AsInt(4);
				this.retrieveOnly = block.Attributes["retrieveOnly"][this.type].AsBool(false);
				if (block.Attributes["typedOpenSound"][this.type].Exists)
				{
					this.OpenSound = (block.Attributes["typedOpenSound"][this.type].AsObject<SoundAttributes?>(default(SoundAttributes?), base.Block.Code.Domain, true) ?? this.OpenSound);
				}
				if (block.Attributes["typedCloseSound"][this.type].Exists)
				{
					this.CloseSound = (block.Attributes["typedCloseSound"][this.type].AsObject<SoundAttributes?>(default(SoundAttributes?), base.Block.Code.Domain, true) ?? this.CloseSound);
				}
			}
			this.inventory = new InventoryGeneric(this.quantitySlots, null, null, null)
			{
				BaseWeight = 1f
			};
			this.inventory.OnGetSuitability = ((ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge) => (isMerge ? (this.inventory.BaseWeight + 3f) : (this.inventory.BaseWeight + 1f)) + (sourceSlot.Inventory is InventoryBasePlayer));
			this.inventory.OnGetAutoPullFromSlot = new GetAutoPullFromSlotDelegate(this.GetAutoPullFromSlot);
			this.container.Reset();
			if (((block != null) ? block.Attributes : null) != null)
			{
				if (block.Attributes["spoilSpeedMulByFoodCat"][this.type].Exists)
				{
					this.inventory.PerishableFactorByFoodCategory = block.Attributes["spoilSpeedMulByFoodCat"][this.type].AsObject<Dictionary<EnumFoodCategory, float>>(null);
				}
				if (block.Attributes["transitionSpeedMulByType"][this.type].Exists)
				{
					this.inventory.TransitionableSpeedMulByType = block.Attributes["transitionSpeedMulByType"][this.type].AsObject<Dictionary<EnumTransitionType, float>>(null);
				}
			}
			this.inventory.PutLocked = this.retrieveOnly;
			this.inventory.OnInventoryClosed += new OnInventoryClosedDelegate(this.OnInvClosed);
			this.inventory.OnInventoryOpened += new OnInventoryOpenedDelegate(this.OnInvOpened);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000FCD4 File Offset: 0x0000DED4
		public virtual void LateInitInventory()
		{
			InventoryBase inventoryBase = this.Inventory;
			string text = this.InventoryClassName;
			string text2 = "-";
			BlockPos pos = this.Pos;
			inventoryBase.LateInitialize(text + text2 + ((pos != null) ? pos.ToString() : null), this.Api);
			this.Inventory.ResolveBlocksOrItems();
			this.container.LateInit();
			this.MarkDirty(false, null);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000FD32 File Offset: 0x0000DF32
		private ItemSlot GetAutoPullFromSlot(BlockFacing atBlockFace)
		{
			if (atBlockFace == BlockFacing.DOWN)
			{
				return Enumerable.FirstOrDefault<ItemSlot>(this.inventory, (ItemSlot slot) => !slot.Empty);
			}
			return null;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000FD68 File Offset: 0x0000DF68
		protected virtual void OnInvOpened(IPlayer player)
		{
			this.inventory.PutLocked = (this.retrieveOnly && player.WorldData.CurrentGameMode != 2);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000FD94 File Offset: 0x0000DF94
		protected virtual void OnInvClosed(IPlayer player)
		{
			this.inventory.PutLocked = this.retrieveOnly;
			GuiDialogBlockEntity inv = this.invDialog;
			this.invDialog = null;
			if (inv != null && inv.IsOpened() && inv != null)
			{
				inv.TryClose();
			}
			if (inv != null)
			{
				inv.Dispose();
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000FDE0 File Offset: 0x0000DFE0
		public override bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel)
		{
			if (byPlayer.WorldData.CurrentGameMode == 2)
			{
				this.inventory.PutLocked = false;
			}
			if (this.inventory.PutLocked && this.inventory.Empty)
			{
				return false;
			}
			if (this.Api.World is IServerWorldAccessor)
			{
				byte[] data;
				using (MemoryStream ms = new MemoryStream())
				{
					BinaryWriter writer = new BinaryWriter(ms);
					writer.Write("BlockEntityInventory");
					writer.Write(this.DialogTitle);
					writer.Write((byte)this.quantityColumns);
					TreeAttribute tree = new TreeAttribute();
					this.inventory.ToTreeAttributes(tree);
					tree.ToBytes(writer);
					data = ms.ToArray();
				}
				BlockPos tempPos = new BlockPos(this.Pos.X, this.Pos.Y, this.Pos.Z, 0);
				((ICoreServerAPI)this.Api).Network.SendBlockEntityPacket((IServerPlayer)byPlayer, tempPos, 5000, data);
				byPlayer.InventoryManager.OpenInventory(this.inventory);
			}
			return true;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000FF08 File Offset: 0x0000E108
		private MeshData GenMesh(ITesselatorAPI tesselator)
		{
			BlockTreeHollowPlaced block = base.Block as BlockTreeHollowPlaced;
			if (base.Block == null)
			{
				block = (this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockTreeHollowPlaced);
				base.Block = block;
			}
			if (block == null)
			{
				return null;
			}
			JsonObject attributes = base.Block.Attributes;
			int? num;
			if (attributes == null)
			{
				num = default(int?);
			}
			else
			{
				JsonObject jsonObject = attributes["rndTexNum"][this.type];
				num = ((jsonObject != null) ? new int?(jsonObject.AsInt(0)) : default(int?));
			}
			int? num2 = num;
			int rndTexNum = num2.GetValueOrDefault();
			string key = "typedContainerMeshes" + base.Block.Code.ToShortString();
			Dictionary<string, MeshData> meshes = ObjectCacheUtil.GetOrCreate<Dictionary<string, MeshData>>(this.Api, key, () => new Dictionary<string, MeshData>());
			JsonObject attributes2 = base.Block.Attributes;
			string shapename = (attributes2 != null) ? attributes2["shape"][this.type].AsString(null) : null;
			if (shapename == null)
			{
				return null;
			}
			string meshKey = this.type + block.Subtype + "-" + rndTexNum.ToString();
			MeshData mesh;
			if (meshes.TryGetValue(meshKey, ref mesh))
			{
				return mesh;
			}
			if (rndTexNum > 0)
			{
				rndTexNum = GameMath.MurmurHash3Mod(this.Pos.X, this.Pos.Y, this.Pos.Z, rndTexNum);
			}
			return meshes[meshKey] = block.GenMesh(this.Api as ICoreClientAPI, this.type, shapename, tesselator, new Vec3f(), rndTexNum);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000100B0 File Offset: 0x0000E2B0
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			if (!base.OnTesselation(mesher, tesselator))
			{
				if (this.ownMesh == null)
				{
					this.ownMesh = this.GenMesh(tesselator);
					if (this.ownMesh == null)
					{
						return false;
					}
				}
				mesher.AddMeshData(this.ownMesh.Clone().Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, this.MeshAngle, 0f), 1);
			}
			return true;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00010122 File Offset: 0x0000E322
		public void OnTransformed(ITreeAttribute tree, int degreeRotation, EnumAxis? flipAxis)
		{
			this.MeshAngle = tree.GetFloat("meshAngle", 0f);
			this.MeshAngle -= (float)degreeRotation * 0.017453292f;
			tree.SetFloat("meshAngle", this.MeshAngle);
		}

		// Token: 0x040000A4 RID: 164
		internal InventoryGeneric inventory;

		// Token: 0x040000A5 RID: 165
		public string type = "normal-generic";

		// Token: 0x040000A6 RID: 166
		public string defaultType;

		// Token: 0x040000A7 RID: 167
		public int quantitySlots = 10;

		// Token: 0x040000A8 RID: 168
		public int quantityColumns = 4;

		// Token: 0x040000A9 RID: 169
		public string inventoryClassName = "chest";

		// Token: 0x040000AA RID: 170
		public string dialogTitleLangCode = "chestcontents";

		// Token: 0x040000AB RID: 171
		public bool retrieveOnly;

		// Token: 0x040000AC RID: 172
		private float meshangle;

		// Token: 0x040000AD RID: 173
		private MeshData ownMesh;

		// Token: 0x040000AE RID: 174
		public Cuboidf[] collisionSelectionBoxes;

		// Token: 0x040000AF RID: 175
		private readonly Vec3f rendererRot = new Vec3f();
	}
}
