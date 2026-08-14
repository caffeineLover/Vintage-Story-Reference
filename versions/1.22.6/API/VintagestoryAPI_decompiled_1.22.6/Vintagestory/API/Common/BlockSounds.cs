using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Vintagestory.API.Common;

/// <summary>
/// A set of sounds that are defined for a block. All fields use default or empty sounds if not set.
/// </summary>
/// <example>
/// <code language="json">
///             "sounds": {
///             	"place": "block/dirt",
///             	"break": "block/dirt",
///             	"hit": "block/dirt",
///             	"walk": "walk/grass"
///             },
/// </code>
/// </example>
[DocumentAsJson]
public class BlockSounds
{
	/// <summary>
	/// Played in ambience for this block.
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation? Ambient;

	/// <summary>
	/// The type of sound for this block's ambient sound.
	/// </summary>
	[DocumentAsJson("Optional", "Ambient", false)]
	public EnumSoundType AmbientSoundType = EnumSoundType.Ambient;

	/// <summary>
	/// Adjacent ambient sound sources are merged to avoid playing too many sounds too loudly. This is the maximum distance a sound source can be from another to allow a merge.
	/// </summary>
	[DocumentAsJson("Optional", "3", false)]
	public float AmbientMaxDistanceMerge = 3f;

	/// <summary>
	/// Amount of nearby ambient sound blocks in order to reach full ambient sound volume
	/// </summary>
	[DocumentAsJson("Optional", "10", false)]
	public float AmbientBlockCount = 10f;

	/// <summary>
	/// Played when an entity walks on this block.
	/// </summary>
	[DocumentAsJson("Optional", "See game/soundconfig.json. Range defaults to 12.", false)]
	[JsonConverter(typeof(SoundAttributeConverter), new object[] { true, 12 })]
	public virtual SoundAttributes Walk { get; set; } = new SoundAttributes();

	/// <summary>
	/// Played when an entity moves inside this block. Primarily used for liquids and foliage.
	/// </summary>
	[DocumentAsJson("Optional", "None. Range defaults to 12.", false)]
	[JsonConverter(typeof(SoundAttributeConverter), new object[] { true, 12 })]
	public virtual SoundAttributes Inside { get; set; } = new SoundAttributes();

	/// <summary>
	/// Played when this block is broken.
	/// </summary>
	[DocumentAsJson("Optional", "See game/soundconfig.json. Range defaults to 16.", false)]
	[JsonConverter(typeof(SoundAttributeConverter), new object[] { true, 16 })]
	public virtual SoundAttributes Break { get; set; } = new SoundAttributes();

	/// <summary>
	/// Played when this block is placed.
	/// </summary>
	[DocumentAsJson("Optional", "See game/soundconfig.json. Range defaults to 16.", false)]
	[JsonConverter(typeof(SoundAttributeConverter), new object[] { true, 16 })]
	public virtual SoundAttributes Place { get; set; } = new SoundAttributes();

	/// <summary>
	/// Played when this block is hit. Will be overridden by <see cref="P:Vintagestory.API.Common.BlockSounds.ByTool" /> if an appropriate tool is set.
	/// </summary>
	[DocumentAsJson("Optional", "See game/soundconfig.json. Range defaults to 16.", false)]
	[JsonConverter(typeof(SoundAttributeConverter), new object[] { true, 16 })]
	public virtual SoundAttributes Hit { get; set; } = new SoundAttributes();

	/// <summary>
	/// Gets the sound that occurs when a specific tool hits a block.  (Note for coders: if none specified in the JSON, this will be null from version 1.20.4 onwards)
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public virtual Dictionary<EnumTool, BlockSounds>? ByTool { get; set; }

	/// <summary>
	/// Clones the block sounds.
	/// </summary>
	/// <returns></returns>
	public BlockSounds Clone()
	{
		BlockSounds sounds = new BlockSounds
		{
			Walk = Walk.Clone(),
			Inside = Inside.Clone(),
			Break = Break.Clone(),
			Place = Place.Clone(),
			Hit = Hit.Clone(),
			Ambient = Ambient?.PermanentClone(),
			AmbientBlockCount = AmbientBlockCount,
			AmbientSoundType = AmbientSoundType,
			AmbientMaxDistanceMerge = AmbientMaxDistanceMerge
		};
		if (ByTool != null)
		{
			sounds.ByTool = new Dictionary<EnumTool, BlockSounds>(ByTool.Count);
			foreach (KeyValuePair<EnumTool, BlockSounds> val in ByTool)
			{
				sounds.ByTool[val.Key] = val.Value.Clone();
			}
		}
		return sounds;
	}

	/// <summary>
	/// Gets the breaking sound, either provided by the tool or by this sound.
	/// </summary>
	/// <param name="byPlayer"></param>
	/// <returns></returns>
	public SoundAttributes GetBreakSound(IPlayer? byPlayer)
	{
		EnumTool? tool = byPlayer?.InventoryManager?.ActiveHotbarSlot?.Itemstack?.Collectible?.GetTool(byPlayer.InventoryManager.ActiveHotbarSlot);
		if (tool.HasValue)
		{
			return GetBreakSound(tool.Value);
		}
		return Break;
	}

	/// <summary>
	/// Gets the hit sound either provided by the tool or by the block.
	/// </summary>
	/// <param name="byPlayer"></param>
	/// <returns></returns>
	public SoundAttributes GetHitSound(IPlayer? byPlayer)
	{
		EnumTool? tool = byPlayer?.InventoryManager?.ActiveHotbarSlot?.Itemstack?.Collectible?.GetTool(byPlayer.InventoryManager.ActiveHotbarSlot);
		if (tool.HasValue)
		{
			return GetHitSound(tool.Value);
		}
		return Hit;
	}

	/// <summary>
	/// Gets the break sound either by the tool or by the block if the tool does not have a break sound.
	/// </summary>
	/// <param name="tool">The Tool used.</param>
	/// <returns>The resulting sound</returns>
	public SoundAttributes GetBreakSound(EnumTool tool)
	{
		if (ByTool != null)
		{
			ByTool.TryGetValue(tool, out BlockSounds toolSounds);
			if (toolSounds?.Break != null)
			{
				return toolSounds.Break;
			}
		}
		return Break;
	}

	/// <summary>
	/// Gets the hit sound either by the tool or by the block if the tool does not have a hit sound.
	/// </summary>
	/// <param name="tool"></param>
	/// <returns></returns>
	public SoundAttributes GetHitSound(EnumTool tool)
	{
		if (ByTool != null)
		{
			ByTool.TryGetValue(tool, out BlockSounds toolSounds);
			if (toolSounds?.Hit != null)
			{
				return toolSounds.Hit;
			}
		}
		return Hit;
	}

	[OnDeserialized]
	public void OnDeserializedMethod(StreamingContext context)
	{
		Ambient?.WithPathPrefixOnce("sounds/");
	}
}
