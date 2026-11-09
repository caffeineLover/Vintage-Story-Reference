namespace Vintagestory.API.Common;

public interface IHeadController
{
	float YawOffset { get; set; }

	float PitchOffset { get; set; }

	void OnFrame(float dt);
}
