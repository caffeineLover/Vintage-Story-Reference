using System;

namespace Vintagestory.API.Datastructures;

public class TagRegistryException(string debugLocation, TagRegistryError error, ReadOnlySpan<string> tags) : Exception
{
	public readonly TagRegistryError Error = error;

	public override string Message { get; } = TagRegistry.FormatIssueMessage(debugLocation, error, tags);
}
