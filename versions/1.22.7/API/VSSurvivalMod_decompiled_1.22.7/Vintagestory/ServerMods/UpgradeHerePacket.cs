using ProtoBuf;
using Vintagestory.API.MathTools;

namespace Vintagestory.ServerMods;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
internal class UpgradeHerePacket
{
	public required BlockPos Pos;
}
