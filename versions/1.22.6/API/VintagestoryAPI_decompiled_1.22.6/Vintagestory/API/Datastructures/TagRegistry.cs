using System;
using System.Text;

namespace Vintagestory.API.Datastructures;

public static class TagRegistry
{
	public static string? FormatIssueMessage(string debugLocation, TagRegistryError error, ReadOnlySpan<string> tags)
	{
		if (error == TagRegistryError.None)
		{
			return null;
		}
		StringBuilder sb = new StringBuilder(1024);
		sb.Append('[').Append(debugLocation).Append("] ");
		sb.Append((tags.Length > 1) ? "Some tags in " : "Tag ");
		for (int i = 0; i < tags.Length; i++)
		{
			if (i > 0)
			{
				sb.Append(", ");
			}
			sb.Append('\'').Append(tags[i]).Append('\'');
		}
		switch (error)
		{
		case TagRegistryError.RegistryAtCapacity:
			sb.Append(" could not be registered because the registry is full.");
			break;
		case TagRegistryError.RegistryLocked:
			sb.Append(" could not be registered because the registry is locked. You are too late or tying to register a tag clientside. Maybe pre-register the tags you are trying to use?");
			break;
		case TagRegistryError.SomeTagsNotFound:
			sb.Append(" were not found.");
			break;
		}
		return sb.ToString();
	}

	/// <summary>
	/// Validates the format of a tag.<br />
	/// Tags must only contain lowercase alphanumerics (0-9, a-z) and dashes, must not contain two consecutive dashes, and must start with a lowercase letter.<br />
	/// It must also not be empty and at least three characters long.
	/// </summary>
	public static TagValidationError ValidateTag(ReadOnlySpan<char> tag)
	{
		if (tag.Length == 0)
		{
			return TagValidationError.TagEmpty;
		}
		if (tag.Length < 3)
		{
			return TagValidationError.TooShort;
		}
		char firstChar = tag[0];
		if (firstChar < 'a' || 'z' < firstChar)
		{
			return TagValidationError.InvalidStart;
		}
		for (int i = 1; i < tag.Length; i++)
		{
			char c = tag[i];
			if (('a' > c || c > 'z') && ('0' > c || c > '9'))
			{
				if (c != '-')
				{
					return (TagValidationError)(5 + i);
				}
				if (tag[i - 1] == '-')
				{
					return TagValidationError.DoubleDash;
				}
			}
		}
		return TagValidationError.None;
	}

	public static string? FormatIssueMessage(TagValidationError error, ReadOnlySpan<char> tag)
	{
		switch (error)
		{
		case TagValidationError.None:
			return null;
		case TagValidationError.TagEmpty:
			return "A tag must not be empty.";
		case TagValidationError.TooShort:
			return "A tag must be at least three cahracters long.";
		case TagValidationError.InvalidStart:
			return $"Malformed tag '{tag}', tags must start with a lowercase letter (a-z).";
		case TagValidationError.DoubleDash:
			return $"Malformed tag '{tag}', tags must not contain consecutive dashes.";
		default:
		{
			int index = (int)(error - 5);
			if (index >= 0 && index < tag.Length)
			{
				return $"Malformed tag '{tag}', invalid character '{tag[index]}' at index {index}. Tags may only contain lowercase alphanumerics (0-9, a-z) and dashes";
			}
			return $"Malformed tag '{tag}'.";
		}
		}
	}
}
