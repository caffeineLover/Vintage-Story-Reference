using ProtoBuf;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[ProtoContract]
public class PacketPlayerPosition
{
	[ProtoMember(1)]
	public string PlayerUid = "";

	[ProtoMember(2)]
	public double PosX;

	[ProtoMember(3)]
	public double PosZ;

	[ProtoMember(4)]
	public double Yaw;

	[ProtoMember(5)]
	public bool Despawn;

	public IPlayer? AssociatedPlayer;

	public void SetFrom(PacketPlayerPosition packet)
	{
		PosX = packet.PosX;
		PosZ = packet.PosZ;
		Yaw = packet.Yaw;
	}
}
