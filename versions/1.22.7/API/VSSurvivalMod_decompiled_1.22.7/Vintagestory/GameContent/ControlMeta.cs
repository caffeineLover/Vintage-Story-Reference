using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class ControlMeta : AnimationMetaData
{
	public float MoveSpeedMultiplier;

	public float MoveSpeed;

	public AnimationMetaData RiderAnim;

	public AnimationMetaData PassengerAnim;

	public AnimationMetaData? GetSeatAnimation(IMountableSeat seat)
	{
		if (seat.CanControl)
		{
			return RiderAnim;
		}
		return PassengerAnim;
	}
}
