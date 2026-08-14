using System;

namespace Vintagestory.API.Common;

public interface IConcreteCloneable<out TSelf> : ICloneable
{
	new TSelf Clone();
}
