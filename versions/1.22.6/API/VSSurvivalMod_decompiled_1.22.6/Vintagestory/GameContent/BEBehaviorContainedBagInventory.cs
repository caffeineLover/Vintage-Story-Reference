using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class BEBehaviorContainedBagInventory : BlockEntityBehavior
{
	public const int PacketIdBitShift = 16;

	private readonly BlockEntityContainer containerEntity;

	public BlockEntityContainedBagWorkspace[] BagInventories { get; private set; }

	public BEBehaviorContainedBagInventory(BlockEntity blockEntity)
		: base(blockEntity)
	{
		containerEntity = (BlockEntityContainer)blockEntity;
		BagInventories = new BlockEntityContainedBagWorkspace[containerEntity.Inventory.Count];
		containerEntity.Inventory.SlotModified += ContainerInv_SlotModified;
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		for (int i = 0; i < containerEntity.Inventory.Count; i++)
		{
			BlockEntityContainedBagWorkspace[] bagInventories = BagInventories;
			int num = i;
			if (bagInventories[num] == null)
			{
				bagInventories[num] = new BlockEntityContainedBagWorkspace(containerEntity, i);
			}
		}
	}

	private void ContainerInv_SlotModified(int slotid)
	{
		ItemSlot bagSlot = containerEntity.Inventory[slotid];
		if (bagSlot == null || !BagInventories[slotid].TryLoadBagInv(bagSlot, bagSlot.Itemstack?.Collectible.GetBehavior<CollectibleBehaviorGroundStoredHeldBag>()))
		{
			BagInventories[slotid].Dispose();
		}
	}

	public override void OnReceivedClientPacket(IPlayer player, int packetid, byte[] data)
	{
		int targetSlotId = packetid >> 16;
		int first15Bits = 65535;
		packetid &= first15Bits;
		BagInventories[targetSlotId].OnReceivedClientPacket(player, packetid, data);
	}

	public override void OnReceivedServerPacket(int packetid, byte[] data)
	{
		int targetSlotId = packetid >> 16;
		int first15Bits = 65535;
		packetid &= first15Bits;
		BagInventories[targetSlotId].OnReceivedServerPacket(packetid, data);
	}

	public virtual void Dispose()
	{
		for (int i = 0; i < BagInventories.Length; i++)
		{
			BagInventories[i]?.Dispose();
		}
		containerEntity.Inventory.SlotModified -= ContainerInv_SlotModified;
	}
}
