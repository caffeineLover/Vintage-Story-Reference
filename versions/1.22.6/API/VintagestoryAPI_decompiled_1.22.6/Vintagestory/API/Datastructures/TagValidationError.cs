namespace Vintagestory.API.Datastructures;

public enum TagValidationError
{
	None,
	TagEmpty,
	TooShort,
	InvalidStart,
	DoubleDash,
	/// <remarks> Don't test for this directly. If the tag contains an invalid character, InvalidCharacter + index of that character will be returned from <see cref="M:Vintagestory.API.Datastructures.TagRegistry.ValidateTag(System.ReadOnlySpan{System.Char})" />. </remarks>
	InvalidCharacter
}
