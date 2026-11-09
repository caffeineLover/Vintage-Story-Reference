using ProtoBuf;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[ProtoContract]
public class BeamPlacePacket
{
	[ProtoMember(1)]
	public bool begin;

	[ProtoMember(2)]
	public BlockSelection Blocksel;

	[ProtoMember(3)]
	public bool PartialEnds;
}
