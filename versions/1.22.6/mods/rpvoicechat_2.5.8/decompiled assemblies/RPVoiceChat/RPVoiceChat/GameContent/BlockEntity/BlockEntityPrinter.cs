using System;
using System.IO;
using System.Text;
using RPVoiceChat.GameContent.Inventory;
using RPVoiceChat.Gui;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B2 RID: 178
	public class BlockEntityPrinter : BlockEntityOpenableContainer
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x0001D980 File Offset: 0x0001BB80
		private static int GetPrinterPacketIdBase()
		{
			int result;
			try
			{
				Type enumType = typeof(EnumBlockContainerPacketId);
				if (enumType == null || !enumType.IsEnum)
				{
					result = 10000;
				}
				else
				{
					Array values = Enum.GetValues(enumType);
					int max = 0;
					foreach (object obj in values)
					{
						int i = (int)obj;
						if (i > max)
						{
							max = i;
						}
					}
					result = max + 10;
				}
			}
			catch
			{
				result = 10010;
			}
			return result;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0001DA24 File Offset: 0x0001BC24
		public virtual string DialogTitle
		{
			get
			{
				return Lang.Get(RPVoiceChatMod.modID + ":Printer.Contents", Array.Empty<object>());
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001DA3F File Offset: 0x0001BC3F
		public bool IsDrawerOpen
		{
			get
			{
				return this.isDrawerOpen;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001DA47 File Offset: 0x0001BC47
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0001DA4F File Offset: 0x0001BC4F
		private BlockEntityAnimationUtil AnimUtil
		{
			get
			{
				BEBehaviorAnimatable animatable = this.Animatable;
				if (animatable == null)
				{
					return null;
				}
				return animatable.animUtil;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001DA72 File Offset: 0x0001BC72
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001DA7A File Offset: 0x0001BC7A
		public override string InventoryClassName
		{
			get
			{
				return "printer";
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001DA81 File Offset: 0x0001BC81
		public override void Initialize(ICoreAPI api)
		{
			if (this.inventory == null)
			{
				this.InitInventory(api);
			}
			base.Initialize(api);
			this.LateInitInventory();
			if (api.Side == 2)
			{
				this.InitializeClientAnimator();
			}
			this.CheckForTelegraphAbove();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001DAB4 File Offset: 0x0001BCB4
		protected virtual void InitInventory(ICoreAPI api)
		{
			this.inventory = new InventoryPrinter(api, this.Pos);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001DAC8 File Offset: 0x0001BCC8
		public virtual void LateInitInventory()
		{
			InventoryBase inventoryBase = this.Inventory;
			string inventoryClassName = this.InventoryClassName;
			string text = "-";
			BlockPos pos = this.Pos;
			inventoryBase.LateInitialize(inventoryClassName + text + ((pos != null) ? pos.ToString() : null), this.Api);
			this.Inventory.ResolveBlocksOrItems();
			InventoryBase inventoryBase2 = this.Inventory;
			if (inventoryBase2.Pos == null)
			{
				inventoryBase2.Pos = this.Pos;
			}
			this.MarkDirty(false, null);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001DB36 File Offset: 0x0001BD36
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
			this.isDrawerOpen = false;
			this.CheckForTelegraphAbove();
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001DB4C File Offset: 0x0001BD4C
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			this.DisconnectFromTelegraph();
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001DB5A File Offset: 0x0001BD5A
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.DisconnectFromTelegraph();
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001DB68 File Offset: 0x0001BD68
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			if (this.inventory != null)
			{
				TreeAttribute invTree = new TreeAttribute();
				this.inventory.ToTreeAttributes(invTree);
				tree["inventory"] = invTree;
			}
			tree.SetBool("rpvcDrawerOpen", this.isDrawerOpen);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001DBB4 File Offset: 0x0001BDB4
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			if (this.Api == null)
			{
				this.Api = worldForResolving.Api;
			}
			if (this.inventory == null)
			{
				this.InitInventory(worldForResolving.Api);
			}
			base.FromTreeAttributes(tree, worldForResolving);
			if (tree["inventory"] != null)
			{
				this.inventory.FromTreeAttributes(tree["inventory"] as TreeAttribute);
				this.inventory.ResolveBlocksOrItems();
			}
			this.ApplyDrawerStateFromTree(tree);
			if (this.inventory != null)
			{
				this.LateInitInventory();
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001DC3C File Offset: 0x0001BE3C
		private void ApplyDrawerStateFromTree(ITreeAttribute tree)
		{
			bool incoming = tree.GetBool("rpvcDrawerOpen", false);
			bool prev = this.isDrawerOpen;
			this.isDrawerOpen = incoming;
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2 || prev == incoming)
			{
				return;
			}
			this.TriggerDrawerAnimation(this.isDrawerOpen);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001DC8F File Offset: 0x0001BE8F
		public void NotifyClientInventoryOpenIntent()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				this.lastClientInventoryOpenRequestMs = this.Api.World.ElapsedMilliseconds;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001DCC0 File Offset: 0x0001BEC0
		public override bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel)
		{
			if (this.Api.World is IClientWorldAccessor)
			{
				this.NotifyClientInventoryOpenIntent();
			}
			if (!this.isDrawerOpen)
			{
				this.ToggleDrawerState(byPlayer, !this.isDrawerOpen);
			}
			if (this.Api.World is IServerWorldAccessor)
			{
				byte[] data;
				using (MemoryStream ms = new MemoryStream())
				{
					BinaryWriter writer = new BinaryWriter(ms);
					writer.Write("BlockEntityInventory");
					writer.Write(this.DialogTitle);
					writer.Write(5);
					TreeAttribute tree = new TreeAttribute();
					this.inventory.ToTreeAttributes(tree);
					tree.ToBytes(writer);
					data = ms.ToArray();
				}
				((ICoreServerAPI)this.Api).Network.SendBlockEntityPacket((IServerPlayer)byPlayer, this.Pos, 5000, data);
				byPlayer.InventoryManager.OpenInventory(this.inventory);
			}
			return true;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001DDB4 File Offset: 0x0001BFB4
		public override void OnReceivedClientPacket(IPlayer fromPlayer, int packetid, byte[] data)
		{
			if (packetid == BlockEntityPrinter.PacketIdPrinterDrawerClose)
			{
				bool flag;
				if (fromPlayer == null)
				{
					flag = (null != null);
				}
				else
				{
					EntityPlayer entity = fromPlayer.Entity;
					flag = (((entity != null) ? entity.Pos : null) != null);
				}
				if (!flag)
				{
					return;
				}
				if (fromPlayer.Entity.Pos.SquareDistanceTo((double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5) > 36f)
				{
					return;
				}
				if (!this.isDrawerOpen)
				{
					return;
				}
				this.isDrawerOpen = false;
				this.playDrawerSound();
				this.MarkDirty(true, null);
				return;
			}
			else
			{
				if (packetid == BlockEntityPrinter.PacketIdPrinterInventory && data != null && data.Length != 0 && this.inventory != null)
				{
					try
					{
						TreeAttribute tree = new TreeAttribute();
						using (MemoryStream ms = new MemoryStream(data))
						{
							tree.FromBytes(new BinaryReader(ms));
						}
						this.inventory.FromTreeAttributes(tree);
						this.inventory.ResolveBlocksOrItems();
						this.MarkDirty(false, null);
					}
					catch
					{
					}
					return;
				}
				base.OnReceivedClientPacket(fromPlayer, packetid, data);
				return;
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001DEE8 File Offset: 0x0001C0E8
		public override void OnReceivedServerPacket(int packetid, byte[] data)
		{
			if (packetid == 5000)
			{
				ICoreClientAPI capi = this.Api as ICoreClientAPI;
				if (capi == null)
				{
					return;
				}
				TreeAttribute tree = new TreeAttribute();
				string dialogTitle;
				int cols;
				using (MemoryStream ms = new MemoryStream(data))
				{
					BinaryReader reader = new BinaryReader(ms);
					if (!string.Equals(reader.ReadString(), "BlockEntityInventory", 4))
					{
						return;
					}
					dialogTitle = reader.ReadString();
					cols = (int)reader.ReadByte();
					tree.FromBytes(reader);
				}
				if (this.inventory == null)
				{
					this.InitInventory(this.Api);
					this.LateInitInventory();
				}
				if (this.inventory != null)
				{
					this.inventory.FromTreeAttributes(tree);
					this.inventory.ResolveBlocksOrItems();
					long nowMs = this.Api.World.ElapsedMilliseconds;
					if (this.lastClientInventoryOpenRequestMs < 0L || nowMs - this.lastClientInventoryOpenRequestMs > 1500L || this.isPrinterDialogOpenClient)
					{
						return;
					}
					this.lastClientInventoryOpenRequestMs = -1L;
					this.isPrinterDialogOpenClient = true;
					new PrinterInventoryDialog(dialogTitle, this.inventory, this.Pos, cols, capi, delegate
					{
						this.isPrinterDialogOpenClient = false;
						this.isDrawerOpen = false;
						this.TriggerDrawerAnimation(false);
						capi.Network.SendBlockEntityPacket(this.Pos, BlockEntityPrinter.PacketIdPrinterDrawerClose, Array.Empty<byte>());
					}).TryOpen();
				}
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001E044 File Offset: 0x0001C244
		private void ToggleDrawerState(IPlayer byPlayer, bool newState)
		{
			this.isDrawerOpen = newState;
			this.TriggerDrawerAnimation(this.isDrawerOpen);
			this.playDrawerSound();
			this.MarkDirty(true, null);
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(this.Pos);
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001E0A4 File Offset: 0x0001C2A4
		private void CheckForTelegraphAbove()
		{
			ICoreAPI api = this.Api;
			bool flag;
			if (api == null)
			{
				flag = (null != null);
			}
			else
			{
				IWorldAccessor world = api.World;
				flag = (((world != null) ? world.BlockAccessor : null) != null);
			}
			if (!flag)
			{
				return;
			}
			BlockPos abovePos = this.Pos.UpCopy(1);
			BlockEntityTelegraph telegraph = this.Api.World.BlockAccessor.GetBlockEntity(abovePos) as BlockEntityTelegraph;
			if (telegraph != null)
			{
				this.ConnectToTelegraph(telegraph);
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001E105 File Offset: 0x0001C305
		private void ConnectToTelegraph(BlockEntityTelegraph telegraph)
		{
			telegraph.SetPrinter(this);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001E10E File Offset: 0x0001C30E
		private void DisconnectFromTelegraph()
		{
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001E110 File Offset: 0x0001C310
		public bool ConsumePaperSlip()
		{
			bool flag = this.inventory.PaperSlot.TryConsumePaperSlip();
			if (flag)
			{
				this.MarkDirty(false, null);
			}
			return flag;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001E130 File Offset: 0x0001C330
		public bool StoreTelegram(string message, string networkUID = "", string networkName = "", string sourceEndpointName = "", string targetEndpointName = "")
		{
			foreach (SlotPrinterTelegram slot in this.inventory.TelegramSlots)
			{
				if (slot.Empty && slot.TryStoreTelegram(message, networkUID, networkName, sourceEndpointName, targetEndpointName))
				{
					this.MarkDirty(false, null);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001E180 File Offset: 0x0001C380
		public void CreateTelegram(string message, string networkUID = "", string networkName = "", string sourceEndpointName = "", string targetEndpointName = "")
		{
			if (this.Api.Side == 1 && this.StoreTelegram(message, networkUID, networkName, sourceEndpointName, targetEndpointName))
			{
				if (this.ConsumePaperSlip())
				{
					this.playPrintSound();
					return;
				}
				foreach (SlotPrinterTelegram slot in this.inventory.TelegramSlots)
				{
					if (!slot.Empty)
					{
						slot.Itemstack = null;
						slot.MarkDirty();
						return;
					}
				}
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			if (this.inventory != null)
			{
				bool empty = this.PaperSlot.Empty;
				bool isFull = !this.HasEmptyTelegramSlot();
				if (empty)
				{
					dsc.AppendLine(UIUtils.I18n("Printer.Warning.NoPaper"));
				}
				if (isFull)
				{
					dsc.AppendLine(UIUtils.I18n("Printer.Warning.Full"));
				}
				if (!this.inventory.PaperSlot.Empty)
				{
					dsc.AppendLine(UIUtils.I18n("Printer.Paper", new object[]
					{
						this.inventory.PaperSlot.Itemstack.StackSize
					}));
				}
				int telegramCount = 0;
				SlotPrinterTelegram[] telegramSlots = this.inventory.TelegramSlots;
				for (int i = 0; i < telegramSlots.Length; i++)
				{
					if (!telegramSlots[i].Empty)
					{
						telegramCount++;
					}
				}
				if (telegramCount > 0)
				{
					dsc.AppendLine(UIUtils.I18n("Printer.Telegrams", new object[]
					{
						telegramCount,
						9
					}));
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0001E2E2 File Offset: 0x0001C4E2
		public SlotPrinterPaper PaperSlot
		{
			get
			{
				return this.inventory.PaperSlot;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0001E2EF File Offset: 0x0001C4EF
		public SlotPrinterTelegram[] TelegramSlots
		{
			get
			{
				return this.inventory.TelegramSlots;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		private bool HasEmptyTelegramSlot()
		{
			SlotPrinterTelegram[] telegramSlots = this.inventory.TelegramSlots;
			for (int i = 0; i < telegramSlots.Length; i++)
			{
				if (telegramSlots[i].Empty)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001E330 File Offset: 0x0001C530
		public void TriggerAnimation(string animationName)
		{
			this.StartAnimationIfNotRunning(animationName);
			this.playDrawerSound();
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001E33F File Offset: 0x0001C53F
		private void TriggerDrawerAnimation(bool open)
		{
			if (!open)
			{
				this.StopAnimation("open-drawer");
				return;
			}
			this.StartAnimationIfNotRunning("open-drawer");
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001E35B File Offset: 0x0001C55B
		private void playDrawerSound()
		{
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/printer/drawer.ogg"), this.Pos, 0.0, null, false, 6f, 0.7f);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001E397 File Offset: 0x0001C597
		private void playPrintSound()
		{
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/printer/print.ogg"), this.Pos, 0.0, null, false, 12f, 0.65f);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001E3D3 File Offset: 0x0001C5D3
		public void CloseDrawer()
		{
			if (this.isDrawerOpen)
			{
				this.isDrawerOpen = false;
				this.TriggerDrawerAnimation(false);
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001E3F4 File Offset: 0x0001C5F4
		private void InitializeClientAnimator()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null && animUtil.animator == null)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					Block block = base.Block;
					if (((block != null) ? block.Code : null) != null)
					{
						AssetLocation assetLoc = new AssetLocation(base.Block.Code.Domain, "shapes/block/printer.json");
						Shape shape = Shape.TryGet(this.Api, assetLoc);
						if (((shape != null) ? shape.Animations : null) != null && shape.Animations.Length != 0)
						{
							shape.InitForAnimations(this.Api.Logger, "block/printer", Array.Empty<string>());
						}
					}
					float rotYDeg = this.GetBlockSideRotY();
					animUtil.InitializeAnimator("block/printer", null, null, new Vec3f(0f, rotYDeg, 0f));
					return;
				}
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
		private float GetBlockSideRotY()
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = false;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = ((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			float result;
			if (flag)
			{
				float num;
				if (!(side == "north"))
				{
					if (!(side == "east"))
					{
						if (!(side == "west"))
						{
							if (!(side == "south"))
							{
								num = 0f;
							}
							else
							{
								num = 180f;
							}
						}
						else
						{
							num = 90f;
						}
					}
					else
					{
						num = 270f;
					}
				}
				else
				{
					num = 0f;
				}
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001E578 File Offset: 0x0001C778
		private void StartAnimationIfNotRunning(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null || animUtil.activeAnimationsByAnimCode.ContainsKey(animationCode))
			{
				return;
			}
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = animationCode,
				Code = animationCode
			});
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001E5B8 File Offset: 0x0001C7B8
		private void StopAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			animUtil.StopAnimation(animationCode);
		}

		// Token: 0x0400028E RID: 654
		private InventoryPrinter inventory;

		// Token: 0x0400028F RID: 655
		public static readonly int PacketIdPrinterInventory = BlockEntityPrinter.GetPrinterPacketIdBase();

		// Token: 0x04000290 RID: 656
		public static readonly int PacketIdPrinterDrawerClose = BlockEntityPrinter.GetPrinterPacketIdBase() + 1;

		// Token: 0x04000291 RID: 657
		private const string TreeKeyDrawerOpen = "rpvcDrawerOpen";

		// Token: 0x04000292 RID: 658
		private bool isDrawerOpen;

		// Token: 0x04000293 RID: 659
		private bool isPrinterDialogOpenClient;

		// Token: 0x04000294 RID: 660
		private long lastClientInventoryOpenRequestMs = -1L;
	}
}
