using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public sealed class HeldItemMouseInputHandler : ModSystem
{
	private ICoreClientAPI? clientApi;

	private const string networkChannelId = "HeldItemMouseInputHandler";

	private IClientNetworkChannel? clientChannel;

	public override void StartClientSide(ICoreClientAPI api)
	{
		clientApi = api;
		clientChannel = api.Network.RegisterChannel("HeldItemMouseInputHandler").RegisterMessageType<HeldItemMouseWheelEventPacket>();
		clientApi.Event.MouseWheelMove += OnMouseWheel;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		api.Network.RegisterChannel("HeldItemMouseInputHandler").RegisterMessageType<HeldItemMouseWheelEventPacket>().SetMessageHandler<HeldItemMouseWheelEventPacket>(PacketHandler);
	}

	public void OnMouseWheel(MouseWheelEventArgs args)
	{
		EntityPlayer playerEntity = clientApi?.World.Player.Entity;
		if (playerEntity == null)
		{
			return;
		}
		ItemSlot activeSlot = playerEntity.ActiveHandItemSlot;
		if (activeSlot == null)
		{
			return;
		}
		IHeldItemOnMouseWheel objInterface = activeSlot.Itemstack?.Collectible.GetCollectibleInterface<IHeldItemOnMouseWheel>();
		if (objInterface != null)
		{
			objInterface.OnMouseWheel(playerEntity, activeSlot, args);
			if (args.IsHandled)
			{
				HeldItemMouseWheelEventPacket packet = new HeldItemMouseWheelEventPacket
				{
					Delta = args.delta,
					DeltaPrecise = args.deltaPrecise,
					Value = args.value,
					ValuePrecise = args.valuePrecise
				};
				clientChannel?.SendPacket(packet);
			}
		}
	}

	private void PacketHandler(IServerPlayer player, HeldItemMouseWheelEventPacket packet)
	{
		EntityPlayer playerEntity = player.Entity;
		if (playerEntity == null)
		{
			return;
		}
		ItemSlot activeSlot = playerEntity.ActiveHandItemSlot;
		if (activeSlot != null)
		{
			IHeldItemOnMouseWheel objInterface = activeSlot.Itemstack?.Collectible.GetCollectibleInterface<IHeldItemOnMouseWheel>();
			if (objInterface != null)
			{
				MouseWheelEventArgs args = new MouseWheelEventArgs
				{
					delta = packet.Delta,
					deltaPrecise = packet.DeltaPrecise,
					value = packet.Value,
					valuePrecise = packet.ValuePrecise
				};
				objInterface.OnMouseWheel(playerEntity, activeSlot, args);
			}
		}
	}
}
