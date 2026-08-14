using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace Vintagestory.API.Datastructures;

public interface ITagRegistry<TTagSet>
{
	ILogger logger { get; }

	string debugName { get; }

	/// <exception cref="T:Vintagestory.API.Datastructures.TagRegistryException"> If the registry is full or locked. </exception>
	/// <remarks> Will still register tags until it is at capacity. </remarks>
	void Register(params ReadOnlySpan<string> tags)
	{
		TagRegistryError err = TryRegister(tags);
		if (err != TagRegistryError.None)
		{
			throw new TagRegistryException(debugName, err, tags);
		}
	}

	/// <remarks> Will return an error if at capacity or locked. Will still register tags until it is at capacity. </remarks>
	TagRegistryError TryRegister(params ReadOnlySpan<string> tags);

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.ITagRegistry`1.TryRegister(System.ReadOnlySpan{System.String})" />
	TagRegistryError TryRegisterAndLogIssues(params ReadOnlySpan<string> tags)
	{
		TagRegistryError err = TryRegister(tags);
		if (err != TagRegistryError.None)
		{
			LogIssue(debugName, err, tags);
		}
		return err;
	}

	/// <remarks> Will still register tags and consume the iterator until it is at capacity. </remarks>
	/// <exception cref="T:Vintagestory.API.Datastructures.TagRegistryException"> If the registry is full or locked. </exception>
	void Register(IEnumerable<string> tags)
	{
		TagRegistryError err = TryRegister(tags);
		if (err != TagRegistryError.None)
		{
			throw new TagRegistryException(debugName, err, tags.ToArray());
		}
	}

	/// <remarks> Will return an error if at capacity or locked. Will still consume the iterator and register tags until it is at capacity. </remarks>
	TagRegistryError TryRegister(IEnumerable<string> tags);

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.ITagRegistry`1.TryRegister(System.Collections.Generic.IEnumerable{System.String})" />
	TagRegistryError TryRegisterAndLogIssues(IEnumerable<string> tags)
	{
		TagRegistryError err = TryRegister(tags);
		if (err != TagRegistryError.None)
		{
			LogIssue(debugName, err, tags.ToArray());
		}
		return err;
	}

	/// <summary> Create the corresponding <typeparamref name="TTagSet" /> in its native representation that has the provided tag marked as active. </summary>
	/// <exception cref="T:Vintagestory.API.Datastructures.TagRegistryException"> If any of the requested tags cannot be found. </exception>
	TTagSet CreateTagSet(params ReadOnlySpan<string> tags)
	{
		TTagSet set;
		TagRegistryError err = TryCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			throw new TagRegistryException(debugName, err, tags);
		}
		return set;
	}

	/// <summary> Create the corresponding <typeparamref name="TTagSet" /> in its native representation that has the provided span of tags marked as active. </summary>
	/// <remarks> Will return an error if at least one of the tags does not exist in the registry. The set will still be populated with the remaining found tags. </remarks>
	TagRegistryError TryCreateTagSet(out TTagSet set, params ReadOnlySpan<string> tags);

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.ITagRegistry`1.TryCreateTagSet(`0@,System.ReadOnlySpan{System.String})" />
	TagRegistryError TryCreateTagSetAndLogIssues(out TTagSet set, params ReadOnlySpan<string> tags)
	{
		TagRegistryError err = TryCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			LogIssue(debugName, err, tags);
		}
		return err;
	}

	/// <summary> Create the corresponding <typeparamref name="TTagSet" /> in its native representation that has the provided tag marked as active. </summary>
	/// <remarks> Will fully consume the iterator even if an exception is thrown. </remarks>
	/// <exception cref="T:Vintagestory.API.Datastructures.TagRegistryException"> If any of the requested tags cannot be found. </exception>
	TTagSet CreateTagSet(IEnumerable<string> tags)
	{
		TTagSet set;
		TagRegistryError err = TryCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			throw new TagRegistryException(debugName, err, tags.ToArray());
		}
		return set;
	}

	/// <summary> Create the corresponding <typeparamref name="TTagSet" /> in its native representation that has the provided enumerable of tags marked as active. </summary>
	/// <remarks>
	/// Will return an error if at least one of the tags does not exist in the registry.
	/// Will still consume the iterator and the set will still be populated with the remaining found tags.
	/// </remarks>
	TagRegistryError TryCreateTagSet(out TTagSet set, IEnumerable<string> tags);

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.ITagRegistry`1.TryCreateTagSet(`0@,System.Collections.Generic.IEnumerable{System.String})" />
	TagRegistryError TryCreateTagSetAndLogIssues(out TTagSet set, IEnumerable<string> tags)
	{
		TagRegistryError err = TryCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			LogIssue(debugName, err, tags.ToArray());
		}
		return err;
	}

	/// <summary> Create the corresponding <typeparamref name="TTagSet" /> in its native representation that has the provided tag marked as active. </summary>
	/// <exception cref="T:Vintagestory.API.Datastructures.TagRegistryException"> If any of the the requested tags cannot be found and the registry is full or locked. </exception>
	TTagSet RegisterAndCreateTagSet(params ReadOnlySpan<string> tags)
	{
		TTagSet set;
		TagRegistryError err = TryRegisterAndCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			throw new TagRegistryException(debugName, err, tags);
		}
		return set;
	}

	/// <remarks>
	/// Will return an error if at capacity or locked.
	/// Will still register tags until it is at capacity.
	/// The returned set will be missing the tags that could not be registered in that case.
	/// </remarks>
	TagRegistryError TryRegisterAndCreateTagSet(out TTagSet set, params ReadOnlySpan<string> tags);

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.ITagRegistry`1.TryRegisterAndCreateTagSet(`0@,System.ReadOnlySpan{System.String})" />
	TagRegistryError TryRegisterAndCreateTagSetAndLogIssues(out TTagSet set, params ReadOnlySpan<string> tags)
	{
		TagRegistryError err = TryRegisterAndCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			LogIssue(debugName, err, tags);
		}
		return err;
	}

	/// <summary> Create the corresponding <typeparamref name="TTagSet" /> in its native representation that has the provided tag marked as active. </summary>
	/// <remarks> Will fully consume the iterator even if an exception is thrown. </remarks>
	/// <exception cref="T:Vintagestory.API.Datastructures.TagRegistryException"> If any of the the requested tags cannot be found and the registry is full or locked. </exception>
	TTagSet RegisterAndCreateTagSet(IEnumerable<string> tags)
	{
		TTagSet set;
		TagRegistryError err = TryRegisterAndCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			throw new TagRegistryException(debugName, err, tags.ToArray());
		}
		return set;
	}

	/// <remarks>
	/// Will return an error if at capacity or locked.
	/// Will still consume the iterator completely and register tags until it is at capacity.
	/// The returned set will be missing the tags that could not be registered in that case.
	/// </remarks>
	TagRegistryError TryRegisterAndCreateTagSet(out TTagSet set, IEnumerable<string> tags);

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.ITagRegistry`1.TryRegisterAndCreateTagSet(`0@,System.Collections.Generic.IEnumerable{System.String})" />
	TagRegistryError TryRegisterAndCreateTagSetAndLogIssues(out TTagSet set, IEnumerable<string> tags)
	{
		TagRegistryError err = TryRegisterAndCreateTagSet(out set, tags);
		if (err != TagRegistryError.None)
		{
			LogIssue(debugName, err, tags.ToArray());
		}
		return err;
	}

	/// <summary>Create a new <typeparamref name="TTagSet" /> where all of the tags from both of the input sets are set.</summary>
	/// <remarks>
	/// This method does not check whether the tags in the given sets are actually registered with this registry.
	/// It has to assume you are using it with the registry the set actually came from.
	/// </remarks>
	TTagSet CreateMergedTagSet(TTagSet set1, TTagSet set2);

	IEnumerable<string> SlowEnumerateTagNames(TTagSet set);

	[MethodImpl(MethodImplOptions.NoInlining)]
	void LogIssue(string debugLocation, TagRegistryError error, params ReadOnlySpan<string> tags)
	{
		if (error != TagRegistryError.None)
		{
			logger.Debug(TagRegistry.FormatIssueMessage(debugLocation, error, tags));
		}
	}
}
