using ProtoBuf;

namespace Vintagestory.GameContent;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class RiftsStatus
{
	public bool Enabled;
}
