using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000157 RID: 343
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolBagSystemServer
	{
		// Token: 0x060010D1 RID: 4305 RVA: 0x00051FE4 File Offset: 0x000501E4
		public ToolBagSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<ToolBagPacket>().SetMessageHandler<ToolBagPacket>(new NetworkClientMessageHandler<ToolBagPacket>(this.HandlePacket));
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x00052046 File Offset: 0x00050246
		private IWorldAccessor _world
		{
			get
			{
				return this._api.World;
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00052054 File Offset: 0x00050254
		private void HandlePacket(IServerPlayer player, ToolBagPacket packet)
		{
			IInventory inventory = ToolBagSystemServer.GetBackpackInventory(player);
			if (inventory == null)
			{
				return;
			}
			long currentTime = this._world.ElapsedMilliseconds;
			EntityPlayer entity = player.Entity;
			long entityId = (entity != null) ? entity.EntityId : 0L;
			long mainHandCooldownUntilMs;
			this._mainHandCooldownUntilMs.TryGetValue(entityId, ref mainHandCooldownUntilMs);
			long offHandCooldownUntilMs;
			this._offHandCooldownUntilMs.TryGetValue(entityId, ref offHandCooldownUntilMs);
			try
			{
				if (mainHandCooldownUntilMs < currentTime && this.ProcessSlots(player, inventory, packet.ToolBagId, packet.ToolBagIndex, packet.SlotIndex, true))
				{
					this._mainHandCooldownUntilMs[entityId] = currentTime + 500L;
				}
				if (offHandCooldownUntilMs < currentTime && this.ProcessSlots(player, inventory, packet.ToolBagId, packet.ToolBagIndex, packet.SlotIndex, false))
				{
					this._offHandCooldownUntilMs[entityId] = currentTime + 500L;
				}
			}
			catch (Exception exception)
			{
				EntityPlayer entity2 = player.Entity;
				ICoreAPI api = (entity2 != null) ? entity2.Api : null;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error when trying to use tool bag/sheath '");
				defaultInterpolatedStringHandler.AppendFormatted(packet.ToolBagId);
				defaultInterpolatedStringHandler.AppendLiteral("': ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00052184 File Offset: 0x00050384
		[return: Nullable(2)]
		private ItemSlotBagContentWithWildcardMatch GetToolSlot(IInventory inventory, string bagId, int bagIndex, bool mainHand, int slotIndex)
		{
			return Enumerable.FirstOrDefault<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.HandleHotkey || slot.Config.DisplayInToolDialog), (ItemSlotBagContentWithWildcardMatch slot) => slot.ToolBagId == bagId && slot.MainHand == mainHand && slot.ToolBagIndex == bagIndex && slot.SlotIndex == slotIndex);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x000521F0 File Offset: 0x000503F0
		[return: Nullable(2)]
		private ItemSlotTakeOutOnly GetTakeOutSlot(IInventory inventory, string bagId, int bagIndex, bool mainHand)
		{
			return Enumerable.FirstOrDefault<ItemSlotTakeOutOnly>(Enumerable.OfType<ItemSlotTakeOutOnly>(inventory), (ItemSlotTakeOutOnly slot) => slot.ToolBagId == bagId && slot.MainHand == mainHand && slot.ToolBagIndex == bagIndex);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00052230 File Offset: 0x00050430
		private ItemSlot GetActiveSlot(IServerPlayer player, bool mainHand)
		{
			if (!mainHand)
			{
				return player.Entity.LeftHandItemSlot;
			}
			return player.Entity.ActiveHandItemSlot;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0005224C File Offset: 0x0005044C
		private bool ProcessSlots(IServerPlayer player, IInventory inventory, string bagId, int bagIndex, int slotIndex, bool mainHand)
		{
			ItemSlotBagContentWithWildcardMatch toolSlot = this.GetToolSlot(inventory, bagId, bagIndex, mainHand, slotIndex);
			if (toolSlot == null)
			{
				return false;
			}
			ItemSlot activeSlot = this.GetActiveSlot(player, mainHand);
			EntityPlayer entity = player.Entity;
			ToolBagSystemServer.DisplacementKey displacementKey = new ToolBagSystemServer.DisplacementKey((entity != null) ? entity.EntityId : 0L, bagId, bagIndex, mainHand, slotIndex);
			if (activeSlot.Empty && toolSlot.Empty)
			{
				return false;
			}
			if (toolSlot.Empty)
			{
				return this.PutBack(player, inventory, bagId, bagIndex, mainHand, slotIndex, activeSlot, displacementKey);
			}
			return this.TakeOut(player, inventory, bagId, bagIndex, mainHand, slotIndex, activeSlot, displacementKey);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x000522D8 File Offset: 0x000504D8
		private bool TakeOut(IServerPlayer player, IInventory inventory, string bagId, int bagIndex, bool mainHand, int slotIndex, ItemSlot activeSlot, ToolBagSystemServer.DisplacementKey displacementKey)
		{
			ItemSlotTakeOutOnly takeOutSlot = this.GetTakeOutSlot(inventory, bagId, bagIndex, mainHand);
			ItemSlotBagContentWithWildcardMatch toolSlot = this.GetToolSlot(inventory, bagId, bagIndex, mainHand, slotIndex);
			if (toolSlot == null || toolSlot.Empty)
			{
				return false;
			}
			this._displacedStacks.Remove(displacementKey);
			ToolBagSystemServer.DisplacedStackLocation displacedStack = null;
			if (!activeSlot.Empty && !this.TryMoveActiveStackToRegularInventory(player, activeSlot, toolSlot, out displacedStack) && !this.TryMoveActiveStackToTakeOutSlot(takeOutSlot, activeSlot, out displacedStack))
			{
				return false;
			}
			if (!activeSlot.Empty)
			{
				return false;
			}
			ItemSlot itemSlot = toolSlot;
			IWorldAccessor world = this._world;
			ItemStack itemstack = toolSlot.Itemstack;
			int movedQuantity = itemSlot.TryPutInto(world, activeSlot, (itemstack != null) ? itemstack.StackSize : 1);
			ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
			{
				toolSlot,
				activeSlot,
				takeOutSlot
			});
			if (movedQuantity <= 0)
			{
				if (displacedStack != null)
				{
					this.TryRestoreDisplacedStack(displacedStack, activeSlot);
				}
				return false;
			}
			if (displacedStack != null)
			{
				this._displacedStacks[displacementKey] = displacedStack;
			}
			return true;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x000523BC File Offset: 0x000505BC
		private bool TryMoveActiveStackToRegularInventory(IServerPlayer player, ItemSlot activeSlot, ItemSlot excludedSlot, [Nullable(2)] out ToolBagSystemServer.DisplacedStackLocation displacedStack)
		{
			displacedStack = null;
			if (!activeSlot.Empty)
			{
				ItemStack itemstack = activeSlot.Itemstack;
				if (itemstack == null || itemstack.StackSize > 0)
				{
					foreach (ItemSlot targetSlot in this.GetRegularTargetSlots(player, activeSlot, excludedSlot))
					{
						ItemStack itemstack2 = activeSlot.Itemstack;
						int stackSize = (itemstack2 != null) ? itemstack2.StackSize : 0;
						if (stackSize <= 0)
						{
							return true;
						}
						if (stackSize <= targetSlot.MaxSlotStackSize)
						{
							int movedQuantity = activeSlot.TryPutInto(this._world, targetSlot, stackSize);
							if (movedQuantity > 0)
							{
								ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
								{
									activeSlot,
									targetSlot
								});
							}
							if (activeSlot.Empty && targetSlot.Itemstack != null)
							{
								displacedStack = new ToolBagSystemServer.DisplacedStackLocation(targetSlot, targetSlot.Itemstack);
								return true;
							}
							if (movedQuantity > 0)
							{
								targetSlot.TryPutInto(this._world, activeSlot, movedQuantity);
								ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
								{
									activeSlot,
									targetSlot
								});
							}
						}
					}
					return activeSlot.Empty;
				}
			}
			return true;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x000524D4 File Offset: 0x000506D4
		private IEnumerable<ItemSlot> GetRegularTargetSlots(IServerPlayer player, ItemSlot activeSlot, ItemSlot excludedSlot)
		{
			ToolBagSystemServer.<GetRegularTargetSlots>d__16 <GetRegularTargetSlots>d__ = new ToolBagSystemServer.<GetRegularTargetSlots>d__16(-2);
			<GetRegularTargetSlots>d__.<>3__player = player;
			<GetRegularTargetSlots>d__.<>3__activeSlot = activeSlot;
			<GetRegularTargetSlots>d__.<>3__excludedSlot = excludedSlot;
			return <GetRegularTargetSlots>d__;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x000524F2 File Offset: 0x000506F2
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private static IEnumerable<IInventory> GetRegularTargetInventories(IServerPlayer player)
		{
			ToolBagSystemServer.<GetRegularTargetInventories>d__17 <GetRegularTargetInventories>d__ = new ToolBagSystemServer.<GetRegularTargetInventories>d__17(-2);
			<GetRegularTargetInventories>d__.<>3__player = player;
			return <GetRegularTargetInventories>d__;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00052504 File Offset: 0x00050704
		[NullableContext(2)]
		private bool TryMoveActiveStackToTakeOutSlot(ItemSlotTakeOutOnly takeOutSlot, [Nullable(1)] ItemSlot activeSlot, out ToolBagSystemServer.DisplacedStackLocation displacedStack)
		{
			displacedStack = null;
			if (!activeSlot.Empty)
			{
				ItemStack itemstack = activeSlot.Itemstack;
				if (itemstack == null || itemstack.StackSize > 0)
				{
					if (takeOutSlot == null)
					{
						return false;
					}
					if (!takeOutSlot.Empty)
					{
						return false;
					}
					if (activeSlot.Itemstack.StackSize > takeOutSlot.MaxSlotStackSize)
					{
						return false;
					}
					bool result;
					try
					{
						takeOutSlot.CanHoldNow = true;
						int movedQuantity = activeSlot.TryPutInto(this._world, takeOutSlot, activeSlot.Itemstack.StackSize);
						if (movedQuantity > 0)
						{
							ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
							{
								activeSlot,
								takeOutSlot
							});
						}
						if (activeSlot.Empty && takeOutSlot.Itemstack != null)
						{
							displacedStack = new ToolBagSystemServer.DisplacedStackLocation(takeOutSlot, takeOutSlot.Itemstack);
							result = true;
						}
						else
						{
							if (movedQuantity > 0)
							{
								takeOutSlot.TryPutInto(this._world, activeSlot, movedQuantity);
								ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
								{
									activeSlot,
									takeOutSlot
								});
							}
							result = false;
						}
					}
					finally
					{
						takeOutSlot.CanHoldNow = false;
						ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
						{
							activeSlot,
							takeOutSlot
						});
					}
					return result;
				}
			}
			return true;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0005260C File Offset: 0x0005080C
		private bool PutBack(IServerPlayer player, IInventory inventory, string bagId, int bagIndex, bool mainHand, int slotIndex, ItemSlot activeSlot, ToolBagSystemServer.DisplacementKey displacementKey)
		{
			ItemSlotBagContentWithWildcardMatch toolSlot = this.GetToolSlot(inventory, bagId, bagIndex, mainHand, slotIndex);
			if (toolSlot == null || !toolSlot.Empty || activeSlot.Empty)
			{
				return false;
			}
			IWorldAccessor world = this._world;
			ItemSlot itemSlot = toolSlot;
			ItemStack itemstack = activeSlot.Itemstack;
			int movedQuantity = activeSlot.TryPutInto(world, itemSlot, (itemstack != null) ? itemstack.StackSize : 1);
			ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
			{
				activeSlot,
				toolSlot
			});
			if (movedQuantity <= 0)
			{
				return false;
			}
			ToolBagSystemServer.DisplacedStackLocation displacedStack;
			if (activeSlot.Empty && this._displacedStacks.Remove(displacementKey, ref displacedStack))
			{
				this.TryRestoreDisplacedStack(displacedStack, activeSlot);
			}
			return true;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x000526A0 File Offset: 0x000508A0
		private bool TryRestoreDisplacedStack(ToolBagSystemServer.DisplacedStackLocation displacedStack, ItemSlot activeSlot)
		{
			if (!activeSlot.Empty)
			{
				return false;
			}
			if (displacedStack.Slot.Itemstack == null)
			{
				return false;
			}
			if (displacedStack.Slot.Itemstack != displacedStack.Stack)
			{
				return false;
			}
			int movedQuantity = displacedStack.Slot.TryPutInto(this._world, activeSlot, displacedStack.Slot.Itemstack.StackSize);
			ToolBagSystemServer.MarkSlotsDirty(new ItemSlot[]
			{
				displacedStack.Slot,
				activeSlot
			});
			return movedQuantity > 0;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0005271C File Offset: 0x0005091C
		private static void MarkSlotsDirty([Nullable(new byte[]
		{
			1,
			2
		})] params ItemSlot[] slots)
		{
			foreach (ItemSlot itemSlot in slots)
			{
				if (itemSlot != null)
				{
					itemSlot.MarkDirty();
				}
			}
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00052747 File Offset: 0x00050947
		[return: Nullable(2)]
		private static IInventory GetBackpackInventory(IPlayer player)
		{
			return player.InventoryManager.GetOwnInventory("backpack");
		}

		// Token: 0x040007C4 RID: 1988
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x040007C5 RID: 1989
		private readonly ICoreServerAPI _api;

		// Token: 0x040007C6 RID: 1990
		private const long _toolSwapCooldown = 500L;

		// Token: 0x040007C7 RID: 1991
		private readonly Dictionary<long, long> _mainHandCooldownUntilMs = new Dictionary<long, long>();

		// Token: 0x040007C8 RID: 1992
		private readonly Dictionary<long, long> _offHandCooldownUntilMs = new Dictionary<long, long>();

		// Token: 0x040007C9 RID: 1993
		private readonly Dictionary<ToolBagSystemServer.DisplacementKey, ToolBagSystemServer.DisplacedStackLocation> _displacedStacks = new Dictionary<ToolBagSystemServer.DisplacementKey, ToolBagSystemServer.DisplacedStackLocation>();

		// Token: 0x020002AD RID: 685
		[Nullable(0)]
		private readonly struct DisplacementKey : IEquatable<ToolBagSystemServer.DisplacementKey>
		{
			// Token: 0x0600185D RID: 6237 RVA: 0x0006E39E File Offset: 0x0006C59E
			public DisplacementKey(long EntityId, string BagId, int BagIndex, bool MainHand, int SlotIndex)
			{
				this.EntityId = EntityId;
				this.BagId = BagId;
				this.BagIndex = BagIndex;
				this.MainHand = MainHand;
				this.SlotIndex = SlotIndex;
			}

			// Token: 0x17000561 RID: 1377
			// (get) Token: 0x0600185E RID: 6238 RVA: 0x0006E3C5 File Offset: 0x0006C5C5
			// (set) Token: 0x0600185F RID: 6239 RVA: 0x0006E3CD File Offset: 0x0006C5CD
			public long EntityId { get; set; }

			// Token: 0x17000562 RID: 1378
			// (get) Token: 0x06001860 RID: 6240 RVA: 0x0006E3D6 File Offset: 0x0006C5D6
			// (set) Token: 0x06001861 RID: 6241 RVA: 0x0006E3DE File Offset: 0x0006C5DE
			public string BagId { get; set; }

			// Token: 0x17000563 RID: 1379
			// (get) Token: 0x06001862 RID: 6242 RVA: 0x0006E3E7 File Offset: 0x0006C5E7
			// (set) Token: 0x06001863 RID: 6243 RVA: 0x0006E3EF File Offset: 0x0006C5EF
			public int BagIndex { get; set; }

			// Token: 0x17000564 RID: 1380
			// (get) Token: 0x06001864 RID: 6244 RVA: 0x0006E3F8 File Offset: 0x0006C5F8
			// (set) Token: 0x06001865 RID: 6245 RVA: 0x0006E400 File Offset: 0x0006C600
			public bool MainHand { get; set; }

			// Token: 0x17000565 RID: 1381
			// (get) Token: 0x06001866 RID: 6246 RVA: 0x0006E409 File Offset: 0x0006C609
			// (set) Token: 0x06001867 RID: 6247 RVA: 0x0006E411 File Offset: 0x0006C611
			public int SlotIndex { get; set; }

			// Token: 0x06001868 RID: 6248 RVA: 0x0006E41C File Offset: 0x0006C61C
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("DisplacementKey");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001869 RID: 6249 RVA: 0x0006E468 File Offset: 0x0006C668
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("EntityId = ");
				builder.Append(this.EntityId.ToString());
				builder.Append(", BagId = ");
				builder.Append(this.BagId);
				builder.Append(", BagIndex = ");
				builder.Append(this.BagIndex.ToString());
				builder.Append(", MainHand = ");
				builder.Append(this.MainHand.ToString());
				builder.Append(", SlotIndex = ");
				builder.Append(this.SlotIndex.ToString());
				return true;
			}

			// Token: 0x0600186A RID: 6250 RVA: 0x0006E52B File Offset: 0x0006C72B
			[CompilerGenerated]
			public static bool operator !=(ToolBagSystemServer.DisplacementKey left, ToolBagSystemServer.DisplacementKey right)
			{
				return !(left == right);
			}

			// Token: 0x0600186B RID: 6251 RVA: 0x0006E537 File Offset: 0x0006C737
			[CompilerGenerated]
			public static bool operator ==(ToolBagSystemServer.DisplacementKey left, ToolBagSystemServer.DisplacementKey right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600186C RID: 6252 RVA: 0x0006E544 File Offset: 0x0006C744
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((EqualityComparer<long>.Default.GetHashCode(this.<EntityId>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<BagId>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<BagIndex>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<MainHand>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<SlotIndex>k__BackingField);
			}

			// Token: 0x0600186D RID: 6253 RVA: 0x0006E5BD File Offset: 0x0006C7BD
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is ToolBagSystemServer.DisplacementKey && this.Equals((ToolBagSystemServer.DisplacementKey)obj);
			}

			// Token: 0x0600186E RID: 6254 RVA: 0x0006E5D8 File Offset: 0x0006C7D8
			[CompilerGenerated]
			public bool Equals(ToolBagSystemServer.DisplacementKey other)
			{
				return EqualityComparer<long>.Default.Equals(this.<EntityId>k__BackingField, other.<EntityId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<BagId>k__BackingField, other.<BagId>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<BagIndex>k__BackingField, other.<BagIndex>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<MainHand>k__BackingField, other.<MainHand>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<SlotIndex>k__BackingField, other.<SlotIndex>k__BackingField);
			}

			// Token: 0x0600186F RID: 6255 RVA: 0x0006E65D File Offset: 0x0006C85D
			[CompilerGenerated]
			public void Deconstruct(out long EntityId, out string BagId, out int BagIndex, out bool MainHand, out int SlotIndex)
			{
				EntityId = this.EntityId;
				BagId = this.BagId;
				BagIndex = this.BagIndex;
				MainHand = this.MainHand;
				SlotIndex = this.SlotIndex;
			}
		}

		// Token: 0x020002AE RID: 686
		[Nullable(0)]
		private sealed class DisplacedStackLocation : IEquatable<ToolBagSystemServer.DisplacedStackLocation>
		{
			// Token: 0x06001870 RID: 6256 RVA: 0x0006E689 File Offset: 0x0006C889
			public DisplacedStackLocation(ItemSlot Slot, ItemStack Stack)
			{
				this.Slot = Slot;
				this.Stack = Stack;
				base..ctor();
			}

			// Token: 0x17000566 RID: 1382
			// (get) Token: 0x06001871 RID: 6257 RVA: 0x0006E69F File Offset: 0x0006C89F
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ToolBagSystemServer.DisplacedStackLocation);
				}
			}

			// Token: 0x17000567 RID: 1383
			// (get) Token: 0x06001872 RID: 6258 RVA: 0x0006E6AB File Offset: 0x0006C8AB
			// (set) Token: 0x06001873 RID: 6259 RVA: 0x0006E6B3 File Offset: 0x0006C8B3
			public ItemSlot Slot { get; set; }

			// Token: 0x17000568 RID: 1384
			// (get) Token: 0x06001874 RID: 6260 RVA: 0x0006E6BC File Offset: 0x0006C8BC
			// (set) Token: 0x06001875 RID: 6261 RVA: 0x0006E6C4 File Offset: 0x0006C8C4
			public ItemStack Stack { get; set; }

			// Token: 0x06001876 RID: 6262 RVA: 0x0006E6D0 File Offset: 0x0006C8D0
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("DisplacedStackLocation");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001877 RID: 6263 RVA: 0x0006E71C File Offset: 0x0006C91C
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Slot = ");
				builder.Append(this.Slot);
				builder.Append(", Stack = ");
				builder.Append(this.Stack);
				return true;
			}

			// Token: 0x06001878 RID: 6264 RVA: 0x0006E756 File Offset: 0x0006C956
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ToolBagSystemServer.DisplacedStackLocation left, ToolBagSystemServer.DisplacedStackLocation right)
			{
				return !(left == right);
			}

			// Token: 0x06001879 RID: 6265 RVA: 0x0006E762 File Offset: 0x0006C962
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ToolBagSystemServer.DisplacedStackLocation left, ToolBagSystemServer.DisplacedStackLocation right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600187A RID: 6266 RVA: 0x0006E776 File Offset: 0x0006C976
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ItemSlot>.Default.GetHashCode(this.<Slot>k__BackingField)) * -1521134295 + EqualityComparer<ItemStack>.Default.GetHashCode(this.<Stack>k__BackingField);
			}

			// Token: 0x0600187B RID: 6267 RVA: 0x0006E7B6 File Offset: 0x0006C9B6
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ToolBagSystemServer.DisplacedStackLocation);
			}

			// Token: 0x0600187C RID: 6268 RVA: 0x0006E7C4 File Offset: 0x0006C9C4
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ToolBagSystemServer.DisplacedStackLocation other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ItemSlot>.Default.Equals(this.<Slot>k__BackingField, other.<Slot>k__BackingField) && EqualityComparer<ItemStack>.Default.Equals(this.<Stack>k__BackingField, other.<Stack>k__BackingField));
			}

			// Token: 0x0600187E RID: 6270 RVA: 0x0006E825 File Offset: 0x0006CA25
			[CompilerGenerated]
			private DisplacedStackLocation(ToolBagSystemServer.DisplacedStackLocation original)
			{
				this.Slot = original.<Slot>k__BackingField;
				this.Stack = original.<Stack>k__BackingField;
			}

			// Token: 0x0600187F RID: 6271 RVA: 0x0006E845 File Offset: 0x0006CA45
			[CompilerGenerated]
			public void Deconstruct(out ItemSlot Slot, out ItemStack Stack)
			{
				Slot = this.Slot;
				Stack = this.Stack;
			}
		}
	}
}
