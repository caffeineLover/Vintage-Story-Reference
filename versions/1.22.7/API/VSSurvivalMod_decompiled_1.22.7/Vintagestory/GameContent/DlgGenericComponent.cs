namespace Vintagestory.GameContent;

public class DlgGenericComponent : DialogueComponent
{
	public override string Execute()
	{
		setVars();
		if (Code == "opentrade")
		{
			return "welcomeback";
		}
		if (JumpTo == null)
		{
			return "next";
		}
		return JumpTo;
	}
}
