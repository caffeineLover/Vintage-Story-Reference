using System.Collections.Generic;
using Vintagestory.API.MathTools;

namespace Vintagestory.ServerMods;

public interface IWorldGenArea
{
	string Code { get; }

	Cuboidi Location { get; }

	BlockPos CenterPos { get; }

	int LandformRadius { get; }

	int GenerationRadius { get; }

	Dictionary<int, int> SkipGenerationFlags { get; }

	int MaxSkipGenerationRadiusSq { get; }
}
