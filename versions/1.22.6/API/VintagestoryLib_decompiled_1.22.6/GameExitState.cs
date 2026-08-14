public class GameExitState
{
	public EnumExitMode Mode { get; set; }

	public bool Exit => Mode != EnumExitMode.None;
}
