using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using Newtonsoft.Json;

namespace Vintagestory.API.Datastructures;

/// <summary>
/// A set of tags. When using JSON, this is stored as a string array.
/// </summary>
/// <example> <code lang="json">
/// "tags": ["humanoid", "player", "seraph", "huntable", "habitat-land"],
/// </code>
/// </example>
[DocumentAsJson]
[JsonConverter(typeof(EntityTagSetConverter))]
public struct TagSetFast(Vector256<ulong> storage) : IEquatable<TagSetFast>
{
	internal Vector256<ulong> storage = storage;

	public static readonly TagSetFast Empty = default(TagSetFast);

	public static readonly TagSetFast All = new TagSetFast(Vector256<ulong>.AllBitsSet);

	public readonly bool IsEmpty
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return storage == Vector256<ulong>.Zero;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TagSetFast operator |(TagSetFast l, TagSetFast r)
	{
		return new TagSetFast(l.storage | r.storage);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TagSetFast operator &(TagSetFast l, TagSetFast r)
	{
		return new TagSetFast(l.storage & r.storage);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TagSetFast operator ^(TagSetFast l, TagSetFast r)
	{
		return new TagSetFast(l.storage ^ r.storage);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TagSetFast operator ~(TagSetFast l)
	{
		return new TagSetFast(~l.storage);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(TagSetFast l, TagSetFast r)
	{
		return l.storage == r.storage;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(TagSetFast l, TagSetFast r)
	{
		return l.storage != r.storage;
	}

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.TagSet.Overlaps(Vintagestory.API.Datastructures.TagSet@)" />
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly bool Overlaps(in TagSetFast r)
	{
		return (storage & r.storage) != Vector256<ulong>.Zero;
	}

	/// <inheritdoc cref="M:Vintagestory.API.Datastructures.TagSet.IsFullyContainedIn(Vintagestory.API.Datastructures.TagSet@)" />
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly bool IsFullyContainedIn(in TagSetFast r)
	{
		return (storage & r.storage) == storage;
	}

	public override readonly int GetHashCode()
	{
		return storage.GetHashCode();
	}

	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		return storage.Equals(obj);
	}

	public readonly bool Equals(TagSetFast other)
	{
		return storage.Equals(other);
	}

	public readonly void ToBytes(BinaryWriter writer)
	{
		Span<byte> buffer = stackalloc byte[32];
		storage.AsByte().StoreUnsafe(ref MemoryMarshal.AsRef<byte>(buffer));
		writer.Write(buffer);
	}

	public static TagSetFast FromBytes(BinaryReader reader)
	{
		Span<byte> buffer = stackalloc byte[32];
		reader.ReadExactly(buffer);
		return new TagSetFast(Vector256.LoadUnsafe(in MemoryMarshal.AsRef<byte>(buffer)).AsUInt64());
	}
}
