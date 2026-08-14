using System.Collections.Generic;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.ServerMods;

[ProtoContract]
public class DungeonLocation : IStructureLocation, IWorldGenArea
{
	[ProtoMember(3)]
	public BlockPos Position;

	[ProtoMember(1)]
	public string Code { get; }

	[ProtoMember(2)]
	public Cuboidi Location { get; }

	public BlockPos CenterPos => Location.Center.AsBlockPos;

	public int LandformRadius { get; }

	public int GenerationRadius { get; }

	public Dictionary<int, int> SkipGenerationFlags { get; }

	public int MaxSkipGenerationRadiusSq { get; }

	public DungeonLocation()
	{
	}

	public DungeonLocation(BlockPos position, string code, Cuboidi location)
	{
		Position = position;
		Code = code;
		Location = location;
	}
}
