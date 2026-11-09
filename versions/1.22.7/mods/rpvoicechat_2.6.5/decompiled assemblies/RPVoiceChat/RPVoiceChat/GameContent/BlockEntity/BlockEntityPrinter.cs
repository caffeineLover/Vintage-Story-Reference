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
	// Token: 0x020000E4 RID: 228
	public class BlockEntityPrinter : BlockEntityOpenableContainer
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x00024FAC File Offset: 0x000231AC
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00025050 File Offset: 0x00023250
		public virtual string DialogTitle
		{
			get
			{
				return Lang.Get(RPVoiceChatMod.modID + ":Printer.Contents", Array.Empty<object>());
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0002506B File Offset: 0x0002326B
		public bool IsDrawerOpen
		{
			get
			{
				return this.isDrawerOpen;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00025073 File Offset: 0x00023273
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002507B File Offset: 0x0002327B
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

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0002509E File Offset: 0x0002329E
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000250A6 File Offset: 0x000232A6
		public override string InventoryClassName
		{
			get
			{
				return "printer";
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000250AD File Offset: 0x000232AD
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

		// Token: 0x060006D8 RID: 1752 RVA: 0x000250E0 File Offset: 0x000232E0
		protected virtual void InitInventory(ICoreAPI api)
		{
			this.inventory = new InventoryPrinter(api, this.Pos);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000250F4 File Offset: 0x000232F4
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

		// Token: 0x060006DA RID: 1754 RVA: 0x00025162 File Offset: 0x00023362
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
			this.isDrawerOpen = false;
			this.CheckForTelegraphAbove();
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00025178 File Offset: 0x00023378
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			this.DisconnectFromTelegraph();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00025186 File Offset: 0x00023386
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.DisconnectFromTelegraph();
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00025194 File Offset: 0x00023394
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

		// Token: 0x060006DE RID: 1758 RVA: 0x000251E0 File Offset: 0x000233E0
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

		// Token: 0x060006DF RID: 1759 RVA: 0x00025268 File Offset: 0x00023468
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

		// Token: 0x060006E0 RID: 1760 RVA: 0x000252BB File Offset: 0x000234BB
		public void NotifyClientInventoryOpenIntent()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				this.lastClientInventoryOpenRequestMs = this.Api.World.ElapsedMilliseconds;
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000252EC File Offset: 0x000234EC
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

		// Token: 0x060006E2 RID: 1762 RVA: 0x000253E0 File Offset: 0x000235E0
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

		// Token: 0x060006E3 RID: 1763 RVA: 0x00025514 File Offset: 0x00023714
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

		// Token: 0x060006E4 RID: 1764 RVA: 0x00025670 File Offset: 0x00023870
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

		// Token: 0x060006E5 RID: 1765 RVA: 0x000256D0 File Offset: 0x000238D0
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

		// Token: 0x060006E6 RID: 1766 RVA: 0x00025731 File Offset: 0x00023931
		private void ConnectToTelegraph(BlockEntityTelegraph telegraph)
		{
			telegraph.SetPrinter(this);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002573A File Offset: 0x0002393A
		private void DisconnectFromTelegraph()
		{
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002573C File Offset: 0x0002393C
		public bool ConsumePaperSlip()
		{
			bool flag = this.inventory.PaperSlot.TryConsumePaperSlip();
			if (flag)
			{
				this.MarkDirty(false, null);
			}
			return flag;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002575C File Offset: 0x0002395C
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

		// Token: 0x060006EA RID: 1770 RVA: 0x000257AC File Offset: 0x000239AC
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

		// Token: 0x060006EB RID: 1771 RVA: 0x0002581C File Offset: 0x00023A1C
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002590E File Offset: 0x00023B0E
		public SlotPrinterPaper PaperSlot
		{
			get
			{
				return this.inventory.PaperSlot;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0002591B File Offset: 0x00023B1B
		public SlotPrinterTelegram[] TelegramSlots
		{
			get
			{
				return this.inventory.TelegramSlots;
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00025928 File Offset: 0x00023B28
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

		// Token: 0x060006EF RID: 1775 RVA: 0x0002595C File Offset: 0x00023B5C
		public void TriggerAnimation(string animationName)
		{
			this.StartAnimationIfNotRunning(animationName);
			this.playDrawerSound();
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002596B File Offset: 0x00023B6B
		private void TriggerDrawerAnimation(bool open)
		{
			if (!open)
			{
				this.StopAnimation("open-drawer");
				return;
			}
			this.StartAnimationIfNotRunning("open-drawer");
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00025987 File Offset: 0x00023B87
		private void playDrawerSound()
		{
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/printer/drawer.ogg"), this.Pos, 0.0, null, false, 6f, 0.7f);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000259C3 File Offset: 0x00023BC3
		private void playPrintSound()
		{
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/printer/print.ogg"), this.Pos, 0.0, null, false, 12f, 0.65f);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000259FF File Offset: 0x00023BFF
		public void CloseDrawer()
		{
			if (this.isDrawerOpen)
			{
				this.isDrawerOpen = false;
				this.TriggerDrawerAnimation(false);
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00025A20 File Offset: 0x00023C20
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

		// Token: 0x060006F5 RID: 1781 RVA: 0x00025AF4 File Offset: 0x00023CF4
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

		// Token: 0x060006F6 RID: 1782 RVA: 0x00025BA4 File Offset: 0x00023DA4
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

		// Token: 0x060006F7 RID: 1783 RVA: 0x00025BE4 File Offset: 0x00023DE4
		private void StopAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			animUtil.StopAnimation(animationCode);
		}

		// Token: 0x0400033B RID: 827
		private InventoryPrinter inventory;

		// Token: 0x0400033C RID: 828
		public static readonly int PacketIdPrinterInventory = BlockEntityPrinter.GetPrinterPacketIdBase();

		// Token: 0x0400033D RID: 829
		public static readonly int PacketIdPrinterDrawerClose = BlockEntityPrinter.GetPrinterPacketIdBase() + 1;

		// Token: 0x0400033E RID: 830
		private const string TreeKeyDrawerOpen = "rpvcDrawerOpen";

		// Token: 0x0400033F RID: 831
		private bool isDrawerOpen;

		// Token: 0x04000340 RID: 832
		private bool isPrinterDialogOpenClient;

		// Token: 0x04000341 RID: 833
		private long lastClientInventoryOpenRequestMs = -1L;
	}
}
