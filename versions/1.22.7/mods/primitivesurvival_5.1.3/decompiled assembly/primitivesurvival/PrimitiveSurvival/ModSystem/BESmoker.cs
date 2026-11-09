using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000011 RID: 17
	public class BESmoker : BlockEntityDisplayCase, ITexPositionSource
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000CACD File Offset: 0x0000ACCD
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x0000CAD5 File Offset: 0x0000ACD5
		public string State { get; protected set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000EA RID: 234 RVA: 0x0000CADE File Offset: 0x0000ACDE
		public override string InventoryClassName
		{
			get
			{
				return "smoker";
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000EB RID: 235 RVA: 0x0000CAE5 File Offset: 0x0000ACE5
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000CAF0 File Offset: 0x0000ACF0
		static BESmoker()
		{
			BESmoker.smallMetalSparks = new SimpleParticleProperties(2f, 5f, ColorUtil.ToRgba(255, 255, 233, 83), new Vec3d(), new Vec3d(), new Vec3f(-3f, 8f, -3f), new Vec3f(3f, 12f, 3f), 0.1f, 1f, 0.25f, 0.25f, 0)
			{
				WithTerrainCollision = false,
				VertexFlags = 128
			};
			BESmoker.smallMetalSparks.AddPos.Set(0.0625, 0.0, 0.0625);
			BESmoker.smallMetalSparks.SizeEvolve = new EvolvingNatFloat(5, -0.5f);
			BESmoker.smallMetalSparks.AddPos.Set(0.25, 0.1875, 0.25);
			BESmoker.smallMetalSparks.ParticleModel = 1;
			BESmoker.smallMetalSparks.LifeLength = 0.04f;
			BESmoker.smallMetalSparks.MinQuantity = 1f;
			BESmoker.smallMetalSparks.AddQuantity = 1f;
			BESmoker.smallMetalSparks.MinSize = 0.2f;
			BESmoker.smallMetalSparks.MaxSize = 0.2f;
			BESmoker.smallMetalSparks.GravityEffect = 0f;
			BESmoker.breakSparks = new SimpleParticleProperties(40f, 80f, ColorUtil.ToRgba(255, 255, 233, 83), new Vec3d(), new Vec3d(), new Vec3f(-1f, 0.5f, -1f), new Vec3f(2f, 1.5f, 2f), 0.5f, 1f, 0.25f, 0.25f, 1)
			{
				VertexFlags = 128
			};
			BESmoker.breakSparks.AddPos.Set(0.25, 0.25, 0.25);
			BESmoker.breakSparks.SizeEvolve = new EvolvingNatFloat(1, -0.25f);
			BESmoker.smoke = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(128, 110, 110, 110), new Vec3d(), new Vec3d(), new Vec3f(-0.2f, 0.3f, -0.2f), new Vec3f(0.2f, 0.3f, 0.2f), 2f, 0f, 0.5f, 1f, 0)
			{
				SelfPropelled = true,
				OpacityEvolve = new EvolvingNatFloat(1, -255f),
				SizeEvolve = new EvolvingNatFloat(1, 2f)
			};
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000CDAA File Offset: 0x0000AFAA
		public BESmoker()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000EE RID: 238 RVA: 0x0000CDD9 File Offset: 0x0000AFD9
		// (set) Token: 0x060000EF RID: 239 RVA: 0x0000CDE1 File Offset: 0x0000AFE1
		public bool IsBurning { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x0000CDEA File Offset: 0x0000AFEA
		public ItemSlot WoodSlot
		{
			get
			{
				return this.inventory[4];
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x0000CE0B File Offset: 0x0000B00B
		public ItemStack WoodStack
		{
			get
			{
				return this.inventory[4].Itemstack;
			}
			set
			{
				this.inventory[4].Itemstack = value;
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000CE20 File Offset: 0x0000B020
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.inventory.LateInitialize(string.Concat(new string[]
			{
				"smoker-",
				this.Pos.X.ToString(),
				"/",
				this.Pos.Y.ToString(),
				"/",
				this.Pos.Z.ToString()
			}), api);
			this.RegisterGameTickListener(new Action<float>(this.OnGameTick), 100, 0);
			this.doorOpenSound = new AssetLocation("game", "sounds/block/chestopen");
			this.doorCloseSound = new AssetLocation("game", "sounds/block/chestclose");
			this.meatSound = new AssetLocation("game", "sounds/block/stickplace");
			this.logSound = new AssetLocation("game", "sounds/block/planks");
			this.ownFacing = BlockFacing.FromCode(api.World.BlockAccessor.GetBlock(this.Pos).LastCodePart(0));
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000CF2D File Offset: 0x0000B12D
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.UnregisterGameTickListener(this.particleTick);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000CF44 File Offset: 0x0000B144
		private void EmitParticles()
		{
			if (this.Api.World.Rand.Next(5) > 0)
			{
				BESmoker.smoke.MinPos.Set((double)this.Pos.X + 0.5 - 0.125, (double)((float)this.Pos.Y + 0.625f), (double)this.Pos.Z + 0.5 - 0.125);
				BESmoker.smoke.AddPos.Set(0.25, 0.0, 0.25);
				this.Api.World.SpawnParticles(BESmoker.smoke, null);
			}
			if (this.Api.World.Rand.Next(3) == 0)
			{
				Vec3f dir = this.ownFacing.Normalf;
				Vec3d particlePos = BESmoker.smallMetalSparks.MinPos;
				particlePos.Set((double)this.Pos.X + 0.5, (double)this.Pos.Y, (double)this.Pos.Z + 0.5);
				particlePos.Sub((double)dir.X * 0.375 + 0.125, 0.0, (double)dir.Z * 0.375 + 0.125);
				BESmoker.smallMetalSparks.MinPos = particlePos;
				BESmoker.smallMetalSparks.MinVelocity = new Vec3f(-0.5f - dir.X, -0.3f, -0.5f - dir.Z);
				BESmoker.smallMetalSparks.AddVelocity = new Vec3f(1f - dir.X, 0.6f, 1f - dir.Z);
				this.Api.World.SpawnParticles(BESmoker.smallMetalSparks, null);
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000D144 File Offset: 0x0000B344
		private void OnGameTick(float dt)
		{
			if (this.IsBurning)
			{
				this.EmitParticles();
			}
			if (!this.IsBurning)
			{
				return;
			}
			if (this.Api.Side == 1 && this.burningUntilTotalDays < this.Api.World.Calendar.TotalDays)
			{
				this.DoSmoke();
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000D19C File Offset: 0x0000B39C
		private void DoSmoke()
		{
			this.WoodStack = null;
			for (int cnt = 0; cnt < this.FirstFreeSlot(); cnt++)
			{
				string thisStack = this.inventory[cnt].Itemstack.Collectible.Code.Path;
				if (thisStack.Contains("trussed"))
				{
					thisStack = "primitivesurvival:" + thisStack.Replace("raw", "smoked");
					Item smokedItem = this.Api.World.GetItem(new AssetLocation(thisStack));
					if (smokedItem != null)
					{
						this.inventory[cnt].Itemstack = new ItemStack(smokedItem, 1);
					}
				}
			}
			this.IsBurning = false;
			this.burningUntilTotalDays = 0.0;
			this.State = "closed";
			this.MarkDirty(false, null);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000D268 File Offset: 0x0000B468
		public bool TryIgnite()
		{
			if (!this.CanIgnite() || this.IsBurning)
			{
				return false;
			}
			this.IsBurning = true;
			this.State = "lit";
			this.burningUntilTotalDays = this.Api.World.Calendar.TotalDays + 0.4166666666666667;
			this.burningStartTotalDays = this.Api.World.Calendar.TotalDays;
			this.MarkDirty(false, null);
			return true;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000D2E2 File Offset: 0x0000B4E2
		public bool CanIgnite()
		{
			return !this.IsBurning && this.WoodSlot.StackSize == 4 && this.inventory[0].StackSize > 0;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000D310 File Offset: 0x0000B510
		private int FirstFreeSlot()
		{
			int slot = 0;
			bool found = false;
			do
			{
				if (this.inventory[slot].Empty)
				{
					found = true;
				}
				else
				{
					slot++;
				}
			}
			while (slot < this.maxSlots && !found);
			if (!found)
			{
				slot = -1;
			}
			return slot;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000D350 File Offset: 0x0000B550
		internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
		{
			bool result = false;
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			string playerPath = "";
			ItemStack itemstack = playerSlot.Itemstack;
			if (((itemstack != null) ? itemstack.Collectible : null) != null)
			{
				playerPath = playerSlot.Itemstack.Collectible.Code.Path;
			}
			string smokerPath = "";
			ItemStack itemstack2 = this.inventory[0].Itemstack;
			if (((itemstack2 != null) ? itemstack2.Collectible : null) != null)
			{
				smokerPath = this.inventory[0].Itemstack.Collectible.Code.Path;
			}
			if (this.State == "open")
			{
				if (smokerPath.Contains("smoked") || smokerPath.Contains("rot"))
				{
					int cnt = this.FirstFreeSlot() - 1;
					if (cnt == -2)
					{
						cnt = 3;
					}
					if (0 <= cnt && cnt <= 3)
					{
						result = this.TryTake(byPlayer, this.inventory[cnt]);
					}
					if (result)
					{
						this.Api.World.PlaySoundAt(this.meatSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
						return true;
					}
				}
				else if (playerPath.Contains("firewood"))
				{
					if (!this.WoodSlot.Empty)
					{
						ItemStack woodStack = this.WoodStack;
						if (woodStack == null || woodStack.StackSize >= 4)
						{
							goto IL_178;
						}
					}
					result = this.TryPut(playerSlot, this.WoodSlot);
					IL_178:
					if (result)
					{
						this.Api.World.PlaySoundAt(this.logSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
						return true;
					}
				}
				else if (playerPath.Contains("trussedmeat") && playerPath.Contains("raw"))
				{
					int cnt2 = this.FirstFreeSlot();
					if (0 <= cnt2 && cnt2 <= 3)
					{
						result = this.TryPut(playerSlot, this.inventory[cnt2]);
					}
					if (result)
					{
						this.Api.World.PlaySoundAt(this.meatSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
						return true;
					}
				}
			}
			else if (this.State == "closed")
			{
				this.Api.World.PlaySoundAt(this.doorOpenSound, (double)blockSel.Position.X, (double)(blockSel.Position.Y - 1), (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
				this.State = "open";
				this.MarkDirty(true, null);
				return true;
			}
			if (this.State == "open")
			{
				this.Api.World.PlaySoundAt(this.doorCloseSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
				this.State = "closed";
				this.MarkDirty(true, null);
				return true;
			}
			return false;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000D698 File Offset: 0x0000B898
		internal void OnBreak()
		{
			for (int index = this.maxSlots - 2; index >= 0; index--)
			{
				if (!this.inventory[index].Empty)
				{
					if (this.inventory[index].StackSize > 0)
					{
						ItemStack stack = this.inventory[index].TakeOut(1);
						string stateCode = stack.Collectible.LastCodePart(0);
						if (stateCode == "raw")
						{
							double d = (double)(index / 10);
							this.Api.World.SpawnItemEntity(stack, this.Pos.ToVec3d().Add(0.5, 0.5 + d, 0.5), null);
						}
						else if (stateCode == "rot")
						{
							AssetLocation newAsset = new AssetLocation("game:rot");
							Item rotItem = this.Api.World.GetItem(newAsset);
							if (rotItem != null)
							{
								ItemStack tempStack = new ItemStack(rotItem, 4);
								double d2 = (double)(index / 10);
								this.Api.World.SpawnItemEntity(tempStack, this.Pos.ToVec3d().Add(0.5, 1.0 + d2, 0.5), null);
							}
						}
						else
						{
							string meatCode = stack.Collectible.LastCodePart(1);
							AssetLocation newAsset2 = new AssetLocation("primitivesurvival:smokedmeat-" + meatCode + "-raw");
							Item smokedItem = this.Api.World.GetItem(newAsset2);
							if (smokedItem != null)
							{
								ItemStack tempStack2 = new ItemStack(smokedItem, 4);
								double d3 = (double)(index / 10);
								this.Api.World.SpawnItemEntity(tempStack2, this.Pos.ToVec3d().Add(0.5, 1.0 + d3, 0.5), null);
							}
						}
					}
					this.MarkDirty(true, null);
				}
			}
			if (!this.IsBurning)
			{
				for (int cnt = 1; cnt <= this.WoodSlot.StackSize; cnt++)
				{
					ItemStack stack2 = this.WoodSlot.TakeOut(1);
					double d4 = (double)(cnt / 10);
					this.Api.World.SpawnItemEntity(stack2, this.Pos.ToVec3d().Add(0.5, 0.5 + d4, 0.5), null);
				}
				this.MarkDirty(true, null);
				return;
			}
			this.WoodStack = null;
			this.MarkDirty(true, null);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000D920 File Offset: 0x0000BB20
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			this.State = "closed";
			this.MarkDirty(true, null);
			base.OnBlockPlaced(byItemStack);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000D93C File Offset: 0x0000BB3C
		private bool TryPut(ItemSlot playerSlot, ItemSlot targetSlot)
		{
			int moved = playerSlot.TryPutInto(this.Api.World, targetSlot, 1);
			if (moved > 0)
			{
				this.MarkDirty(true, null);
				return moved > 0;
			}
			return false;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000D970 File Offset: 0x0000BB70
		private bool TryTake(IPlayer byPlayer, ItemSlot sourceSlot)
		{
			if (sourceSlot.Empty)
			{
				return false;
			}
			string stackCode = sourceSlot.Itemstack.Collectible.Code.Path;
			if (!stackCode.Contains("rot"))
			{
				string meatCode = sourceSlot.Itemstack.Collectible.LastCodePart(1);
				stackCode = "primitivesurvival:smokedmeat-" + meatCode + "-raw";
			}
			AssetLocation newAsset = new AssetLocation(stackCode);
			Item meatItem = this.Api.World.GetItem(newAsset);
			if (meatItem == null)
			{
				return false;
			}
			ItemStack tempStack = new ItemStack(meatItem, 4);
			if (!byPlayer.InventoryManager.TryGiveItemstack(tempStack, false))
			{
				this.Api.World.SpawnItemEntity(tempStack, byPlayer.Entity.Pos.XYZ.Add(0.0, 0.5, 0.0), null);
			}
			sourceSlot.Itemstack = null;
			this.MarkDirty(true, null);
			return true;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolve)
		{
			base.FromTreeAttributes(tree, worldForResolve);
			this.State = tree.GetString("state", null);
			this.IsBurning = (tree.GetInt("burning", 0) > 0);
			this.burningUntilTotalDays = tree.GetDouble("burningUntilTotalDays", 0.0);
			this.burningStartTotalDays = tree.GetDouble("burningStartTotalDays", 0.0);
			if (this.Api != null && this.Api.Side == 2)
			{
				this.Api.World.BlockAccessor.MarkBlockDirty(this.Pos, null);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000DB00 File Offset: 0x0000BD00
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("state", this.State);
			tree.SetInt("burning", (this.IsBurning > false) ? 1 : 0);
			tree.SetDouble("burningUntilTotalDays", this.burningUntilTotalDays);
			tree.SetDouble("burningStartTotalDays", this.burningStartTotalDays);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000DB5C File Offset: 0x0000BD5C
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			int cnt = 0;
			if (!this.inventory[0].Empty)
			{
				string path = this.inventory[0].Itemstack.Item.Code.Path;
				if (path.Contains("rot"))
				{
					sb.Append(Lang.Get("primitivesurvival:item-trussedrot", Array.Empty<object>()));
				}
				else
				{
					sb.Append(Lang.Get("primitivesurvival:item-" + path, Array.Empty<object>()));
				}
				cnt++;
			}
			if (!this.inventory[1].Empty)
			{
				string path2 = this.inventory[1].Itemstack.Item.Code.Path;
				if (path2.Contains("rot"))
				{
					sb.AppendLine(", " + Lang.Get("primitivesurvival:item-trussedrot", Array.Empty<object>()));
				}
				else
				{
					sb.AppendLine(", " + Lang.Get("primitivesurvival:item-" + path2, Array.Empty<object>()));
				}
				cnt++;
			}
			if (!this.inventory[2].Empty)
			{
				string path3 = this.inventory[2].Itemstack.Item.Code.Path;
				if (path3.Contains("rot"))
				{
					sb.Append(Lang.Get("primitivesurvival:item-trussedrot", Array.Empty<object>()));
				}
				else
				{
					sb.Append(Lang.Get("primitivesurvival:item-" + path3, Array.Empty<object>()));
				}
				cnt++;
			}
			if (!this.inventory[3].Empty)
			{
				string path4 = this.inventory[3].Itemstack.Item.Code.Path;
				if (path4.Contains("rot"))
				{
					sb.AppendLine(", " + Lang.Get("primitivesurvival:item-trussedrot", Array.Empty<object>()));
				}
				else
				{
					sb.AppendLine(", " + Lang.Get("primitivesurvival:item-" + path4, Array.Empty<object>()));
				}
				cnt++;
			}
			if (cnt == 1 || cnt == 3)
			{
				sb.AppendLine();
			}
			int WoodCount = 0;
			if (!this.WoodSlot.Empty)
			{
				WoodCount = this.WoodStack.StackSize;
				Item item = this.WoodStack.Item;
				string WoodType = (item != null) ? item.Code.Path : null;
				Item item2 = this.WoodStack.Item;
				string Domain = (item2 != null) ? item2.Code.Domain : null;
				if (WoodType == null)
				{
					WoodType = "firewood";
					Domain = "game";
				}
				sb.AppendLine(WoodCount.ToString() + "/4 " + Lang.Get(Domain + ":item-" + WoodType, Array.Empty<object>()));
				sb.AppendLine();
			}
			else
			{
				sb.AppendLine(WoodCount.ToString() + "/4 " + Lang.Get("item-firewood", Array.Empty<object>()));
				sb.AppendLine();
			}
			double percentComplete = Math.Round((this.Api.World.Calendar.TotalDays - this.burningStartTotalDays) / (this.burningUntilTotalDays - this.burningStartTotalDays) * 100.0, 0);
			if (0.0 <= percentComplete && percentComplete < 100.0 && this.IsBurning)
			{
				sb.AppendLine(percentComplete.ToString() + "% " + Lang.Get("primitivesurvival:blockhelp-smoker-complete", Array.Empty<object>()));
			}
			sb.AppendLine();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000DEEC File Offset: 0x0000C0EC
		private MeshData GenItemMesh(ItemStack trussedStack, int count)
		{
			this.nowTesselatingObj = trussedStack.Item;
			MeshData mesh = null;
			bool flag;
			if (trussedStack == null)
			{
				flag = (null != null);
			}
			else
			{
				Item item = trussedStack.Item;
				flag = (((item != null) ? item.Shape : null) != null);
			}
			if (flag)
			{
				try
				{
					this.capi.Tesselator.TesselateItem(trussedStack.Item, ref mesh, this);
				}
				catch
				{
					return mesh;
				}
				if (mesh != null)
				{
					ArrayExtensions.Fill<short>(mesh.RenderPassesAndExtraBits, 2);
				}
				if (count == 0)
				{
					mesh.Translate(0f, 1.2f, -0.2f);
					mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 0.5235988f, 0f);
				}
				else if (count == 1)
				{
					mesh.Translate(0f, 1.2f, 0.3f);
					mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, -0.34906584f, 0f);
				}
				else if (count == 2)
				{
					mesh.Translate(-0.5f, 1.2f, 0f);
					mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, -0.43633232f, 0f);
				}
				else
				{
					mesh.Translate(-0.5f, 1.2f, 0.15f);
					mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 0.2617994f, 0f);
				}
				if (trussedStack.Item.Code.Path.Contains("fish"))
				{
					mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 3.1415927f, 3.1415927f, 3.1415927f);
					mesh.Scale(new Vec3f(0.5f, 0f, 0.5f), 0.5f, 0.5f, 0.5f);
					mesh.Translate(0f, 0.4f, 0f);
				}
				else
				{
					mesh.Scale(new Vec3f(0.5f, 0f, 0.5f), 0.75f, 0.75f, 0.75f);
				}
				float rotate = base.Block.Shape.rotateY;
				mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, rotate * 0.017453292f, 0f);
				return mesh;
			}
			return mesh;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000E170 File Offset: 0x0000C370
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			string shapeBase = "primitivesurvival:shapes/block/smoker/smokerblock-placed";
			int count = 0;
			BlockSmoker block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockSmoker;
			if (block != null)
			{
				ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
				MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase, texture, this.State, count);
				mesher.AddMeshData(mesh, 1);
				shapeBase = shapeBase.Replace("placed", "door");
				mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase, texture, this.State, count);
				mesher.AddMeshData(mesh, 1);
				if (this.inventory != null)
				{
					if (!this.WoodSlot.Empty)
					{
						if (this.State == "lit")
						{
							shapeBase = shapeBase.Replace("door", "lit");
						}
						else
						{
							shapeBase = shapeBase.Replace("door", "log");
						}
						for (count = 1; count <= this.WoodStack.StackSize; count++)
						{
							mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase, texture, this.State, count);
							mesher.AddMeshData(mesh, 1);
						}
					}
					if (this.State != "lit")
					{
						for (count = 0; count < 4; count++)
						{
							if (!this.inventory[count].Empty)
							{
								ItemStack tmpStack = this.inventory[count].Itemstack;
								if (tmpStack.Collectible.Code.Path == "rot")
								{
									AssetLocation newAsset = new AssetLocation("primitivesurvival:trussedrot");
									Item trussedItem = this.Api.World.GetItem(newAsset);
									if (trussedItem != null)
									{
										tmpStack = new ItemStack(trussedItem, 1);
									}
								}
								mesh = this.GenItemMesh(tmpStack, count);
								if (mesh != null)
								{
									mesher.AddMeshData(mesh, 1);
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04000089 RID: 137
		private static SimpleParticleProperties breakSparks;

		// Token: 0x0400008A RID: 138
		private static SimpleParticleProperties smallMetalSparks;

		// Token: 0x0400008B RID: 139
		private static SimpleParticleProperties smoke;

		// Token: 0x0400008C RID: 140
		private readonly int maxSlots = 5;

		// Token: 0x0400008D RID: 141
		protected static readonly Random Rnd = new Random();

		// Token: 0x0400008F RID: 143
		private readonly long particleTick;

		// Token: 0x04000090 RID: 144
		private BlockFacing ownFacing;

		// Token: 0x04000091 RID: 145
		private double burningUntilTotalDays;

		// Token: 0x04000092 RID: 146
		private double burningStartTotalDays;

		// Token: 0x04000093 RID: 147
		protected InventoryGeneric inventory;

		// Token: 0x04000094 RID: 148
		private AssetLocation doorOpenSound;

		// Token: 0x04000095 RID: 149
		private AssetLocation doorCloseSound;

		// Token: 0x04000096 RID: 150
		private AssetLocation meatSound;

		// Token: 0x04000097 RID: 151
		private AssetLocation logSound;
	}
}
