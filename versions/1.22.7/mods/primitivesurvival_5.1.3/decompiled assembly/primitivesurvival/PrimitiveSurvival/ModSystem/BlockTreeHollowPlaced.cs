using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200002C RID: 44
	public class BlockTreeHollowPlaced : Block, ITexPositionSource
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00018CA3 File Offset: 0x00016EA3
		public Size2i AtlasSize
		{
			get
			{
				return this.tmpTextureSource.AtlasSize;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00018CB0 File Offset: 0x00016EB0
		public string Subtype
		{
			get
			{
				if (this.variantByGroup != null)
				{
					return this.Variant[this.variantByGroup];
				}
				return "";
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00018CD1 File Offset: 0x00016ED1
		public string SubtypeInventory
		{
			get
			{
				if (this.variantByGroupInventory != null)
				{
					return this.Variant[this.variantByGroupInventory];
				}
				return "";
			}
		}

		// Token: 0x17000051 RID: 81
		public TextureAtlasPosition this[string textureCode]
		{
			get
			{
				return this.tmpTextureSource[this.curType + "-" + textureCode];
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00018D10 File Offset: 0x00016F10
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.defaultType = this.Attributes["defaultType"].AsString("normal-generic");
			this.variantByGroup = this.Attributes["variantByGroup"].AsString(null);
			this.variantByGroupInventory = this.Attributes["variantByGroupInventory"].AsString(null);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00018D7C File Offset: 0x00016F7C
		public string GetType(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BETreeHollowPlaced be = blockAccessor.GetBlockEntity(pos) as BETreeHollowPlaced;
			if (be != null)
			{
				return be.type;
			}
			return this.defaultType;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00018DA6 File Offset: 0x00016FA6
		public override List<ItemStack> GetHandBookStacks(ICoreClientAPI capi)
		{
			return base.GetHandBookStacks(capi);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00018DB0 File Offset: 0x00016FB0
		private Cuboidf[] BuildCollisionSelectionBoxes(BETreeHollowPlaced be)
		{
			int angle = 0;
			string text = be.Block.LastCodePart(0);
			if (!(text == "south"))
			{
				if (!(text == "east"))
				{
					if (text == "west")
					{
						angle = 90;
					}
				}
				else
				{
					angle = 270;
				}
			}
			else
			{
				angle = 180;
			}
			Cuboidf[] boxes;
			if (be.type.Contains("up"))
			{
				boxes = new Cuboidf[]
				{
					new Cuboidf(0f, 0.7799f, 0f, 1f, 1f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0.25f, 0.23f, 0f, 0f, 0.7799f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(1f, 0.23f, 0f, 0.75f, 0.7799f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0f, 0f, 0f, 1f, 0.23f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0.25f, 0.23f, 0f, 0.75f, 0.78f, 0.25f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5))
				};
			}
			else
			{
				boxes = new Cuboidf[]
				{
					new Cuboidf(0f, 0.8114f, 0f, 1f, 1f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0.7185f, 0.03f, 0f, 1f, 0.8114f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0f, 0.03f, 0f, 0.2873f, 0.8114f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0f, 0f, 0f, 1f, 0.03f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0.2873f, 0.03f, 0f, 0.7185f, 0.8114f, 0.25f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0.5935f, 0.625f, 0.75f, 0.7185f, 0.8125f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5)),
					new Cuboidf(0.2873f, 0.5625f, 0.75f, 0.406f, 0.8125f, 1f).RotatedCopy(0f, (float)angle, 0f, new Vec3d(0.5, 0.5, 0.5))
				};
			}
			return boxes;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00019248 File Offset: 0x00017448
		public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BETreeHollowPlaced be = blockAccessor.GetBlockEntity(pos) as BETreeHollowPlaced;
			if (be != null)
			{
				return this.BuildCollisionSelectionBoxes(be);
			}
			return base.GetCollisionBoxes(blockAccessor, pos);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00019278 File Offset: 0x00017478
		public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BETreeHollowPlaced be = blockAccessor.GetBlockEntity(pos) as BETreeHollowPlaced;
			if (be != null)
			{
				return this.BuildCollisionSelectionBoxes(be);
			}
			return base.GetSelectionBoxes(blockAccessor, pos);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000192A8 File Offset: 0x000174A8
		public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
		{
			bool flag = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
			if (flag)
			{
				BETreeHollowPlaced bect = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BETreeHollowPlaced;
				if (bect != null)
				{
					BlockPos targetPos = blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position;
					double num = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
					double dz = (double)((float)byPlayer.Entity.Pos.Z) - ((double)targetPos.Z + blockSel.HitPosition.Z);
					double num2 = (double)((float)Math.Atan2(num, dz));
					string type = bect.type;
					float deg22dot5rad = 1.5707964f;
					float roundRad = (float)((int)Math.Round(num2 / (double)deg22dot5rad)) * deg22dot5rad;
					bect.MeshAngle = roundRad;
				}
			}
			return flag;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00019380 File Offset: 0x00017580
		public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
		{
			Dictionary<string, MultiTextureMeshRef> meshrefs = new Dictionary<string, MultiTextureMeshRef>();
			string key = "genericTypedContainerMeshRefs" + base.FirstCodePart(0) + this.SubtypeInventory;
			meshrefs = ObjectCacheUtil.GetOrCreate<Dictionary<string, MultiTextureMeshRef>>(capi, key, delegate()
			{
				foreach (KeyValuePair<string, MeshData> val in this.GenGuiMeshes(capi))
				{
					meshrefs[val.Key] = capi.Render.UploadMultiTextureMesh(val.Value);
				}
				return meshrefs;
			});
			string type = itemstack.Attributes.GetString("type", this.defaultType);
			if (!meshrefs.TryGetValue(type, ref renderinfo.ModelRef))
			{
				MeshData mesh = this.GenGuiMesh(capi, type);
				meshrefs[type] = (renderinfo.ModelRef = capi.Render.UploadMultiTextureMesh(mesh));
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00019447 File Offset: 0x00017647
		public override void OnDecalTesselation(IWorldAccessor world, MeshData decalMesh, BlockPos pos)
		{
			base.OnDecalTesselation(world, decalMesh, pos);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00019454 File Offset: 0x00017654
		public override void OnUnloaded(ICoreAPI api)
		{
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi == null)
			{
				return;
			}
			string key = "genericTypedContainerMeshRefs" + base.FirstCodePart(0) + this.SubtypeInventory;
			Dictionary<string, MultiTextureMeshRef> meshrefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, key);
			if (meshrefs != null)
			{
				foreach (KeyValuePair<string, MultiTextureMeshRef> val in meshrefs)
				{
					val.Value.Dispose();
				}
				capi.ObjectCache.Remove(key);
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000194E4 File Offset: 0x000176E4
		private MeshData GenGuiMesh(ICoreClientAPI capi, string type)
		{
			string shapename = this.Attributes["shape"][type].AsString(null);
			return this.GenMesh(capi, type, shapename, null, null, 0);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0001951C File Offset: 0x0001771C
		public Dictionary<string, MeshData> GenGuiMeshes(ICoreClientAPI capi)
		{
			string[] types = this.Attributes["types"].AsArray<string>(null, "game");
			Dictionary<string, MeshData> meshes = new Dictionary<string, MeshData>();
			if (types == null)
			{
				return meshes;
			}
			foreach (string type in types)
			{
				string shapename = this.Attributes["shape"][type].AsString(null);
				meshes[type] = this.GenMesh(capi, type, shapename, null, (this.ShapeInventory == null) ? null : new Vec3f(this.ShapeInventory.rotateX, this.ShapeInventory.rotateY, this.ShapeInventory.rotateZ), 0);
			}
			return meshes;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000195CC File Offset: 0x000177CC
		public Shape GetShape(ICoreClientAPI capi, string type, string shapename, ITesselatorAPI tesselator = null, int altTexNumber = 0)
		{
			if (shapename == null)
			{
				return null;
			}
			if (tesselator == null)
			{
				tesselator = capi.Tesselator;
			}
			this.tmpTextureSource = tesselator.GetTextureSource(this, altTexNumber, false);
			AssetLocation shapeloc = AssetLocation.Create(shapename, this.Code.Domain).WithPathPrefix("shapes/");
			IAsset asset = capi.Assets.TryGet(shapeloc + ".json", true);
			Shape shape = (asset != null) ? asset.ToObject<Shape>(null) : null;
			if (shape == null)
			{
				IAsset asset2 = capi.Assets.TryGet(shapeloc + "1.json", true);
				shape = ((asset2 != null) ? asset2.ToObject<Shape>(null) : null);
			}
			this.curType = type;
			return shape;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00019680 File Offset: 0x00017880
		public MeshData GenMesh(ICoreClientAPI capi, string type, string shapename, ITesselatorAPI tesselator = null, Vec3f rotation = null, int altTexNumber = 0)
		{
			Shape shape = this.GetShape(capi, type, shapename, tesselator, altTexNumber);
			if (tesselator == null)
			{
				tesselator = capi.Tesselator;
			}
			if (shape == null)
			{
				return new MeshData(true);
			}
			this.curType = type;
			MeshData mesh;
			tesselator.TesselateShape("typedcontainer", shape, ref mesh, this, rotation ?? new Vec3f(this.Shape.rotateX, this.Shape.rotateY, this.Shape.rotateZ), 0, 0, 0, default(int?), null);
			return mesh;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00019704 File Offset: 0x00017904
		public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
		{
			BETreeHollowPlaced be = world.BlockAccessor.GetBlockEntity(pos) as BETreeHollowPlaced;
			if (be == null)
			{
				base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
				return;
			}
			ICoreClientAPI capi = this.api as ICoreClientAPI;
			string shapename = this.Attributes["shape"][be.type].AsString(null);
			if (shapename == null)
			{
				base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
				return;
			}
			blockModelData = this.GenMesh(capi, be.type, shapename, null, null, 0);
			AssetLocation shapeloc = new AssetLocation(shapename).WithPathPrefix("shapes/");
			IAsset asset = capi.Assets.TryGet(shapeloc + ".json", true);
			Shape shape = (asset != null) ? asset.ToObject<Shape>(null) : null;
			if (shape == null)
			{
				shape = capi.Assets.TryGet(shapeloc + "1.json", true).ToObject<Shape>(null);
			}
			MeshData md;
			capi.Tesselator.TesselateShape("typedcontainer-decal", shape, ref md, decalTexSource, null, 0, 0, 0, default(int?), null);
			decalModelData = md;
			decalModelData.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, be.MeshAngle, 0f);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00019850 File Offset: 0x00017A50
		public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
		{
			Block pickBlock = world.GetBlock(base.CodeWithVariant("side", "north"));
			if (pickBlock == null)
			{
				return base.OnPickBlock(world, pos);
			}
			ItemStack stack = new ItemStack(pickBlock, 1);
			BETreeHollowPlaced be = world.BlockAccessor.GetBlockEntity(pos) as BETreeHollowPlaced;
			if (be != null)
			{
				stack.Attributes.SetString("type", be.type);
			}
			else
			{
				stack.Attributes.SetString("type", this.defaultType);
			}
			return stack;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000198CC File Offset: 0x00017ACC
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			bool preventDefault = false;
			foreach (BlockBehavior blockBehavior in this.BlockBehaviors)
			{
				EnumHandling handled = 0;
				blockBehavior.OnBlockBroken(world, pos, byPlayer, 1f, ref handled);
				if (handled == 2)
				{
					preventDefault = true;
				}
				if (handled == 3)
				{
					return;
				}
			}
			if (preventDefault)
			{
				return;
			}
			if (world.Side == 1 && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != 2))
			{
				ItemStack[] drops = new ItemStack[]
				{
					this.OnPickBlock(world, pos)
				};
				JsonObject jsonObject = this.Attributes["drop"];
				bool flag;
				if (jsonObject == null)
				{
					flag = false;
				}
				else
				{
					JsonObject jsonObject2 = jsonObject[this.GetType(world.BlockAccessor, pos)];
					flag = ((jsonObject2 != null) ? new bool?(jsonObject2.AsBool(false)) : default(bool?)).GetValueOrDefault();
				}
				if (flag && drops != null)
				{
					for (int i = 0; i < drops.Length; i++)
					{
						world.SpawnItemEntity(drops[i], new Vec3d((double)pos.X + 0.5, (double)pos.Y + 0.5, (double)pos.Z + 0.5), null);
					}
				}
				world.PlaySoundAt(this.Sounds.GetBreakSound(byPlayer), (double)pos.X, (double)pos.Y, (double)pos.Z, 0, byPlayer, 1f);
			}
			if (this.EntityClass != null)
			{
				BlockEntity entity = world.BlockAccessor.GetBlockEntity(pos);
				if (entity != null)
				{
					entity.OnBlockBroken(null);
				}
			}
			world.BlockAccessor.SetBlock(0, pos);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00019A50 File Offset: 0x00017C50
		public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
		{
			ITreeAttribute attributes = handbookStack.Attributes;
			string type = (attributes != null) ? attributes.GetString("type", null) : null;
			if (type == null)
			{
				ILogger logger = this.api.World.Logger;
				string text = "BlockGenericTypedContainer.GetDropsForHandbook(): type not set for block ";
				CollectibleObject collectible = handbookStack.Collectible;
				logger.Warning(text + ((collectible != null) ? collectible.Code : null));
				return new BlockDropItemStack[0];
			}
			JsonObject attributes2 = this.Attributes;
			bool flag;
			if (attributes2 == null)
			{
				flag = false;
			}
			else
			{
				JsonObject jsonObject = attributes2["drop"];
				bool? flag2;
				if (jsonObject == null)
				{
					flag2 = default(bool?);
				}
				else
				{
					JsonObject jsonObject2 = jsonObject[type];
					flag2 = ((jsonObject2 != null) ? new bool?(jsonObject2.AsBool(false)) : default(bool?));
				}
				bool? flag3 = flag2;
				bool flag4 = false;
				flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
			}
			if (flag)
			{
				return new BlockDropItemStack[0];
			}
			return new BlockDropItemStack[]
			{
				new BlockDropItemStack(handbookStack, 1f)
			};
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00019B30 File Offset: 0x00017D30
		public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			Block dropBlock = world.GetBlock(base.CodeWithVariant("side", "north"));
			if (dropBlock == null)
			{
				return new ItemStack[0];
			}
			return new ItemStack[]
			{
				new ItemStack(dropBlock, 1)
			};
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00019B6E File Offset: 0x00017D6E
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00019B7C File Offset: 0x00017D7C
		public override string GetHeldItemName(ItemStack itemStack)
		{
			string type = itemStack.Attributes.GetString("type", null);
			string[] array = new string[5];
			int num = 0;
			AssetLocation code = this.Code;
			array[num] = ((code != null) ? code.Domain : null);
			array[1] = ":block-";
			array[2] = type;
			array[3] = "-";
			int num2 = 4;
			AssetLocation code2 = this.Code;
			array[num2] = ((code2 != null) ? code2.Path : null);
			return Lang.GetMatching(string.Concat(array), Array.Empty<object>());
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00019BF0 File Offset: 0x00017DF0
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			string type = inSlot.Itemstack.Attributes.GetString("type", null);
			if (type != null)
			{
				JsonObject itemAttributes = inSlot.Itemstack.ItemAttributes;
				int? num;
				if (itemAttributes == null)
				{
					num = default(int?);
				}
				else
				{
					JsonObject jsonObject = itemAttributes["quantitySlots"];
					if (jsonObject == null)
					{
						num = default(int?);
					}
					else
					{
						JsonObject jsonObject2 = jsonObject[type];
						num = ((jsonObject2 != null) ? new int?(jsonObject2.AsInt(0)) : default(int?));
					}
				}
				int? qslots = num;
				dsc.AppendLine("\n" + Lang.Get("Quantity Slots: {0}", new object[]
				{
					qslots
				}));
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00019CA0 File Offset: 0x00017EA0
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-chest-open",
					MouseButton = 2
				}
			}, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
		}

		// Token: 0x040000E3 RID: 227
		private string curType;

		// Token: 0x040000E4 RID: 228
		private string defaultType;

		// Token: 0x040000E5 RID: 229
		private string variantByGroup;

		// Token: 0x040000E6 RID: 230
		private string variantByGroupInventory;

		// Token: 0x040000E7 RID: 231
		private ITexPositionSource tmpTextureSource;
	}
}
