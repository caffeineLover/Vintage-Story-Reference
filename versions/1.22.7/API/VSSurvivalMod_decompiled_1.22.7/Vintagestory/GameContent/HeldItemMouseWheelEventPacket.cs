using ProtoBuf;

namespace Vintagestory.GameContent;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class HeldItemMouseWheelEventPacket
{
	public int Delta { get; set; }

	public float DeltaPrecise { get; set; }

	public int Value { get; set; }

	public float ValuePrecise { get; set; }
}
