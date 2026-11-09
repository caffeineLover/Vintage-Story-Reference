namespace Vintagestory.API.Common;

/// <summary>
/// Internal state number when sending an interaction from client to server
/// </summary>
public enum EnumHandInteractNw
{
	StartHeldItemUse,
	CancelHeldItemUse,
	StopHeldItemUse,
	StepHeldItemUse,
	StartBlockUse,
	CancelBlockUse,
	StopBlockUse,
	StepBlockUse
}
