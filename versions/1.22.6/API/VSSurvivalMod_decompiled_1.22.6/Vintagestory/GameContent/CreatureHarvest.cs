using ProtoBuf;

namespace Vintagestory.GameContent;

[ProtoContract]
public struct CreatureHarvest
{
	[ProtoMember(1)]
	public double TotalDays;

	[ProtoMember(2)]
	public int Quantity;
}
