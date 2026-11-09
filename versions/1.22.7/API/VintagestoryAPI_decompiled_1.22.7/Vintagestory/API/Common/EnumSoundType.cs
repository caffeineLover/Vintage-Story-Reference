namespace Vintagestory.API.Common;

/// <summary>
/// Sound types, often used to determine specific volume controls.
/// </summary>
[DocumentAsJson]
public enum EnumSoundType
{
	Sound,
	Music,
	Ambient,
	Weather,
	Entity,
	MusicGlitchunaffected,
	AmbientGlitchunaffected,
	SoundGlitchunaffected
}
