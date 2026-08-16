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
	// Token: 0x0200014B RID: 331
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolBagSystemServer
	{
		// Token: 0x0600105D RID: 4189 RVA: 0x000506DC File Offset: 0x0004E8DC
		public ToolBagSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<ToolBagPacket>().SetMessageHandler<ToolBagPacket>(new NetworkClientMessageHandler<ToolBagPacket>(this.HandlePacket));
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x0005073E File Offset: 0x0004E93E
		private IWorldAccessor _world
		{
			get
			{
				return this._api.World;
			}
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0005074C File Offset: 0x0004E94C
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

		// Token: 0x06001060 RID: 4192 RVA: 0x0005087C File Offset: 0x0004EA7C
		[return: Nullable(2)]
		private ItemSlotBagContentWithWildcardMatch GetToolSlot(IInventory inventory, string bagId, int bagIndex, bool mainHand, int slotIndex)
		{
			return Enumerable.FirstOrDefault<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.HandleHotkey || slot.Config.DisplayInToolDialog), (ItemSlotBagContentWithWildcardMatch slot) => slot.ToolBagId == bagId && slot.MainHand == mainHand && slot.ToolBagIndex == bagIndex && slot.SlotIndex == slotIndex);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x000508E8 File Offset: 0x0004EAE8
		[return: Nullable(2)]
		private ItemSlotTakeOutOnly GetTakeOutSlot(IInventory inventory, string bagId, int bagIndex, bool mainHand)
		{
			return Enumerable.FirstOrDefault<ItemSlotTakeOutOnly>(Enumerable.OfType<ItemSlotTakeOutOnly>(inventory), (ItemSlotTakeOutOnly slot) => slot.ToolBagId == bagId && slot.MainHand == mainHand && slot.ToolBagIndex == bagIndex);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00050928 File Offset: 0x0004EB28
		private ItemSlot GetActiveSlot(IServerPlayer player, bool mainHand)
		{
			if (!mainHand)
			{
				return player.Entity.LeftHandItemSlot;
			}
			return player.Entity.ActiveHandItemSlot;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00050944 File Offset: 0x0004EB44
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

		// Token: 0x06001064 RID: 4196 RVA: 0x000509D0 File Offset: 0x0004EBD0
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

		// Token: 0x06001065 RID: 4197 RVA: 0x00050AB4 File Offset: 0x0004ECB4
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

		// Token: 0x06001066 RID: 4198 RVA: 0x00050BCC File Offset: 0x0004EDCC
		private IEnumerable<ItemSlot> GetRegularTargetSlots(IServerPlayer player, ItemSlot activeSlot, ItemSlot excludedSlot)
		{
			ToolBagSystemServer.<GetRegularTargetSlots>d__16 <GetRegularTargetSlots>d__ = new ToolBagSystemServer.<GetRegularTargetSlots>d__16(-2);
			<GetRegularTargetSlots>d__.<>3__player = player;
			<GetRegularTargetSlots>d__.<>3__activeSlot = activeSlot;
			<GetRegularTargetSlots>d__.<>3__excludedSlot = excludedSlot;
			return <GetRegularTargetSlots>d__;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00050BEA File Offset: 0x0004EDEA
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

		// Token: 0x06001068 RID: 4200 RVA: 0x00050BFC File Offset: 0x0004EDFC
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

		// Token: 0x06001069 RID: 4201 RVA: 0x00050D04 File Offset: 0x0004EF04
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

		// Token: 0x0600106A RID: 4202 RVA: 0x00050D98 File Offset: 0x0004EF98
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

		// Token: 0x0600106B RID: 4203 RVA: 0x00050E14 File Offset: 0x0004F014
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

		// Token: 0x0600106C RID: 4204 RVA: 0x00050E3F File Offset: 0x0004F03F
		[return: Nullable(2)]
		private static IInventory GetBackpackInventory(IPlayer player)
		{
			return player.InventoryManager.GetOwnInventory("backpack");
		}

		// Token: 0x0400079E RID: 1950
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x0400079F RID: 1951
		private readonly ICoreServerAPI _api;

		// Token: 0x040007A0 RID: 1952
		private const long _toolSwapCooldown = 500L;

		// Token: 0x040007A1 RID: 1953
		private readonly Dictionary<long, long> _mainHandCooldownUntilMs = new Dictionary<long, long>();

		// Token: 0x040007A2 RID: 1954
		private readonly Dictionary<long, long> _offHandCooldownUntilMs = new Dictionary<long, long>();

		// Token: 0x040007A3 RID: 1955
		private readonly Dictionary<ToolBagSystemServer.DisplacementKey, ToolBagSystemServer.DisplacedStackLocation> _displacedStacks = new Dictionary<ToolBagSystemServer.DisplacementKey, ToolBagSystemServer.DisplacedStackLocation>();

		// Token: 0x0200029A RID: 666
		[Nullable(0)]
		private readonly struct DisplacementKey : IEquatable<ToolBagSystemServer.DisplacementKey>
		{
			// Token: 0x060017B8 RID: 6072 RVA: 0x0006BE02 File Offset: 0x0006A002
			public DisplacementKey(long EntityId, string BagId, int BagIndex, bool MainHand, int SlotIndex)
			{
				this.EntityId = EntityId;
				this.BagId = BagId;
				this.BagIndex = BagIndex;
				this.MainHand = MainHand;
				this.SlotIndex = SlotIndex;
			}

			// Token: 0x17000537 RID: 1335
			// (get) Token: 0x060017B9 RID: 6073 RVA: 0x0006BE29 File Offset: 0x0006A029
			// (set) Token: 0x060017BA RID: 6074 RVA: 0x0006BE31 File Offset: 0x0006A031
			public long EntityId { get; set; }

			// Token: 0x17000538 RID: 1336
			// (get) Token: 0x060017BB RID: 6075 RVA: 0x0006BE3A File Offset: 0x0006A03A
			// (set) Token: 0x060017BC RID: 6076 RVA: 0x0006BE42 File Offset: 0x0006A042
			public string BagId { get; set; }

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x060017BD RID: 6077 RVA: 0x0006BE4B File Offset: 0x0006A04B
			// (set) Token: 0x060017BE RID: 6078 RVA: 0x0006BE53 File Offset: 0x0006A053
			public int BagIndex { get; set; }

			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x060017BF RID: 6079 RVA: 0x0006BE5C File Offset: 0x0006A05C
			// (set) Token: 0x060017C0 RID: 6080 RVA: 0x0006BE64 File Offset: 0x0006A064
			public bool MainHand { get; set; }

			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0006BE6D File Offset: 0x0006A06D
			// (set) Token: 0x060017C2 RID: 6082 RVA: 0x0006BE75 File Offset: 0x0006A075
			public int SlotIndex { get; set; }

			// Token: 0x060017C3 RID: 6083 RVA: 0x0006BE80 File Offset: 0x0006A080
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

			// Token: 0x060017C4 RID: 6084 RVA: 0x0006BECC File Offset: 0x0006A0CC
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

			// Token: 0x060017C5 RID: 6085 RVA: 0x0006BF8F File Offset: 0x0006A18F
			[CompilerGenerated]
			public static bool operator !=(ToolBagSystemServer.DisplacementKey left, ToolBagSystemServer.DisplacementKey right)
			{
				return !(left == right);
			}

			// Token: 0x060017C6 RID: 6086 RVA: 0x0006BF9B File Offset: 0x0006A19B
			[CompilerGenerated]
			public static bool operator ==(ToolBagSystemServer.DisplacementKey left, ToolBagSystemServer.DisplacementKey right)
			{
				return left.Equals(right);
			}

			// Token: 0x060017C7 RID: 6087 RVA: 0x0006BFA8 File Offset: 0x0006A1A8
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((EqualityComparer<long>.Default.GetHashCode(this.<EntityId>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<BagId>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<BagIndex>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<MainHand>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<SlotIndex>k__BackingField);
			}

			// Token: 0x060017C8 RID: 6088 RVA: 0x0006C021 File Offset: 0x0006A221
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is ToolBagSystemServer.DisplacementKey && this.Equals((ToolBagSystemServer.DisplacementKey)obj);
			}

			// Token: 0x060017C9 RID: 6089 RVA: 0x0006C03C File Offset: 0x0006A23C
			[CompilerGenerated]
			public bool Equals(ToolBagSystemServer.DisplacementKey other)
			{
				return EqualityComparer<long>.Default.Equals(this.<EntityId>k__BackingField, other.<EntityId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<BagId>k__BackingField, other.<BagId>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<BagIndex>k__BackingField, other.<BagIndex>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<MainHand>k__BackingField, other.<MainHand>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<SlotIndex>k__BackingField, other.<SlotIndex>k__BackingField);
			}

			// Token: 0x060017CA RID: 6090 RVA: 0x0006C0C1 File Offset: 0x0006A2C1
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

		// Token: 0x0200029B RID: 667
		[Nullable(0)]
		private sealed class DisplacedStackLocation : IEquatable<ToolBagSystemServer.DisplacedStackLocation>
		{
			// Token: 0x060017CB RID: 6091 RVA: 0x0006C0ED File Offset: 0x0006A2ED
			public DisplacedStackLocation(ItemSlot Slot, ItemStack Stack)
			{
				this.Slot = Slot;
				this.Stack = Stack;
				base..ctor();
			}

			// Token: 0x1700053C RID: 1340
			// (get) Token: 0x060017CC RID: 6092 RVA: 0x0006C103 File Offset: 0x0006A303
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ToolBagSystemServer.DisplacedStackLocation);
				}
			}

			// Token: 0x1700053D RID: 1341
			// (get) Token: 0x060017CD RID: 6093 RVA: 0x0006C10F File Offset: 0x0006A30F
			// (set) Token: 0x060017CE RID: 6094 RVA: 0x0006C117 File Offset: 0x0006A317
			public ItemSlot Slot { get; set; }

			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x060017CF RID: 6095 RVA: 0x0006C120 File Offset: 0x0006A320
			// (set) Token: 0x060017D0 RID: 6096 RVA: 0x0006C128 File Offset: 0x0006A328
			public ItemStack Stack { get; set; }

			// Token: 0x060017D1 RID: 6097 RVA: 0x0006C134 File Offset: 0x0006A334
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

			// Token: 0x060017D2 RID: 6098 RVA: 0x0006C180 File Offset: 0x0006A380
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

			// Token: 0x060017D3 RID: 6099 RVA: 0x0006C1BA File Offset: 0x0006A3BA
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ToolBagSystemServer.DisplacedStackLocation left, ToolBagSystemServer.DisplacedStackLocation right)
			{
				return !(left == right);
			}

			// Token: 0x060017D4 RID: 6100 RVA: 0x0006C1C6 File Offset: 0x0006A3C6
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ToolBagSystemServer.DisplacedStackLocation left, ToolBagSystemServer.DisplacedStackLocation right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060017D5 RID: 6101 RVA: 0x0006C1DA File Offset: 0x0006A3DA
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ItemSlot>.Default.GetHashCode(this.<Slot>k__BackingField)) * -1521134295 + EqualityComparer<ItemStack>.Default.GetHashCode(this.<Stack>k__BackingField);
			}

			// Token: 0x060017D6 RID: 6102 RVA: 0x0006C21A File Offset: 0x0006A41A
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ToolBagSystemServer.DisplacedStackLocation);
			}

			// Token: 0x060017D7 RID: 6103 RVA: 0x0006C228 File Offset: 0x0006A428
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ToolBagSystemServer.DisplacedStackLocation other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ItemSlot>.Default.Equals(this.<Slot>k__BackingField, other.<Slot>k__BackingField) && EqualityComparer<ItemStack>.Default.Equals(this.<Stack>k__BackingField, other.<Stack>k__BackingField));
			}

			// Token: 0x060017D9 RID: 6105 RVA: 0x0006C289 File Offset: 0x0006A489
			[CompilerGenerated]
			private DisplacedStackLocation(ToolBagSystemServer.DisplacedStackLocation original)
			{
				this.Slot = original.<Slot>k__BackingField;
				this.Stack = original.<Stack>k__BackingField;
			}

			// Token: 0x060017DA RID: 6106 RVA: 0x0006C2A9 File Offset: 0x0006A4A9
			[CompilerGenerated]
			public void Deconstruct(out ItemSlot Slot, out ItemStack Stack)
			{
				Slot = this.Slot;
				Stack = this.Stack;
			}
		}
	}
}
