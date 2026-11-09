using System;
using Newtonsoft.Json;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
public class JsonPatch
{
	[DocumentAsJson("Required", "", false)]
	public EnumJsonPatchOp Op;

	[DocumentAsJson("Required", "", false)]
	public AssetLocation File;

	[DocumentAsJson("Optional", "None", false)]
	public string FromPath;

	[DocumentAsJson("Required", "", false)]
	public string Path;

	[DocumentAsJson("Optional", "None", false)]
	public PatchModDependence[] DependsOn;

	[DocumentAsJson("Optional", "True", false)]
	public bool Enabled = true;

	[DocumentAsJson("Optional", "Universal", false)]
	public EnumAppSide? Side = EnumAppSide.Universal;

	[DocumentAsJson("Optional", "None", false)]
	public PatchCondition Condition;

	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Recommended", "None", false)]
	public JsonObject Value;

	[Obsolete("Use Side instead")]
	[DocumentAsJson("Obsolete", "", false)]
	public EnumAppSide? SideType
	{
		get
		{
			return Side;
		}
		set
		{
			Side = value;
		}
	}
}
