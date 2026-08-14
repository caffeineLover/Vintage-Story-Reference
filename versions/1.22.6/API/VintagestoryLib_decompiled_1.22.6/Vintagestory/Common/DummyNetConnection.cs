using System.Net;
using System.Threading;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace Vintagestory.Common;

public class DummyNetConnection : NetConnection
{
	internal DummyNetwork network;

	private IPEndPoint dummyEndPoint = new IPEndPoint(new IPAddress(new byte[4] { 127, 0, 0, 1 }), 0);

	public override EnumSendResult Send(byte[] data, bool compressed = false)
	{
		Monitor.Enter(network.ClientReceiveBufferLock);
		DummyNetworkPacket packet = new DummyNetworkPacket();
		packet.Data = data;
		packet.Length = data.Length;
		network.ClientReceiveBuffer.Enqueue(packet);
		Monitor.Exit(network.ClientReceiveBufferLock);
		return EnumSendResult.Ok;
	}

	public override EnumSendResult HiPerformanceSend(BoxedPacket box, ILogger Logger, bool compressionAllowed)
	{
		bool compressed;
		byte[] packetBytes = PreparePacketForSending(box, compressionAllowed, out compressed);
		return SendPreparedPacket(packetBytes, compressed, Logger);
	}

	public override byte[] PreparePacketForSending(BoxedPacket box, bool compressionAllowed, out bool compressed)
	{
		int len = box.Length;
		box.LengthSent = len;
		compressed = false;
		return box.Clone(0);
	}

	public override EnumSendResult SendPreparedPacket(byte[] dataCopy, bool compressed, ILogger Logger)
	{
		Monitor.Enter(network.ClientReceiveBufferLock);
		DummyNetworkPacket packet = new DummyNetworkPacket();
		packet.Data = dataCopy;
		packet.Length = dataCopy.Length;
		network.ClientReceiveBuffer.Enqueue(packet);
		Monitor.Exit(network.ClientReceiveBufferLock);
		return EnumSendResult.Ok;
	}

	public override IPEndPoint RemoteEndPoint()
	{
		return dummyEndPoint;
	}

	public override bool EqualsConnection(NetConnection connection)
	{
		return true;
	}

	public override void Close()
	{
	}

	public override void Shutdown()
	{
	}

	internal static bool SendServerAssetsPacketDirectly(Packet_Server packet)
	{
		return ClientSystemStartup.ReceiveAssetsPacketDirectly(packet);
	}

	internal static bool SendServerPacketDirectly(Packet_Server packet)
	{
		return ClientSystemStartup.ReceiveServerPacketDirectly(packet);
	}
}
