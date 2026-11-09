namespace Vintagestory.API.Common.Entities;

public interface IPhysicsTickable
{
	bool Ticking { get; set; }

	Entity Entity { get; }

	/// <summary>
	/// Called at a fixed interval, potentially 30 times per second (if server is running smoothly)
	/// </summary>
	void OnPhysicsTick(float dt);

	/// <summary>
	/// Called once per server tick, after all physics ticking has occurred; on main thread.
	/// </summary>
	void AfterPhysicsTick(float dt);
}
