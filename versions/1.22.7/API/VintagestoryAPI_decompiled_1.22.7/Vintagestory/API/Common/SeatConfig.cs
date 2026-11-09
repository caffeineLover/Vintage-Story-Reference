using System;
using Newtonsoft.Json;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

/// <summary>
/// Seat properties
/// </summary>
[DocumentAsJson]
public class SeatConfig
{
	/// <summary>
	/// Attachment point name
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public string APName;

	/// <summary>
	/// The name of attachment point of shape element that should be used as selection box for interactions with this seat
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public string SelectionBox;

	public string SeatId;

	/// <summary>
	/// Can player in this seat control the mountable entity?
	/// </summary>
	[DocumentAsJson("Optional", "False", false)]
	public bool Controllable;

	/// <summary>
	/// Obsolete. Offset of the mount point. Setting this triggers use of slightly incorrect math for backwards compatibility. 
	/// </summary>
	[Obsolete("Setting this triggers use of slightly incorrect math for backwards compatibility")]
	[DocumentAsJson("Obsolete", "None", false)]
	public Vec3f MountOffset;

	/// <summary>
	/// Offset of the mount point
	/// </summary>
	[DocumentAsJson("Optional", "0, 0, 0,", false)]
	public Vec3f RiderOffset;

	public Vec3f MountRotation = new Vec3f();

	/// <summary>
	/// Optional maximum yaw rotation (in radians) that a player can turn to look to left or right (relative to their body) while seated.
	/// <br />If null, no yaw limit is applied.  
	/// <br />The limit is applied relative to the seat�s mount rotation
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public float? BodyYawLimit;

	/// <summary>
	/// Overrides player's eye height while seated
	/// </summary>
	[DocumentAsJson("Optional", "1.5", false)]
	public float EyeHeight = 1.5f;

	/// <summary>
	/// Offset player's eye position while seated
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float EyeOffsetX;

	/// <summary>
	/// How the mounted entity should rotate relative to the mount
	/// </summary>
	[DocumentAsJson("Optional", "FixateYaw", false)]
	public EnumMountAngleMode AngleMode = EnumMountAngleMode.FixateYaw;

	/// <summary>
	/// Miscallaneous attributes 
	/// </summary>
	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject Attributes;

	/// <summary>
	/// Player's animation code that is used while seated
	/// </summary>
	[DocumentAsJson("Recommended", "None", false)]
	public string Animation { get; set; }
}
