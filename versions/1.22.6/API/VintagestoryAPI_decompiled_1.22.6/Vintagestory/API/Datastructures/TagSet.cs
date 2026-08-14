using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
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
[JsonConverter(typeof(CollectibleTagSetConverter))]
public readonly struct TagSet : IEquatable<TagSet>
{
	/// <remarks> This is internal storage and should not be manipulated in any way unless you know exactly what you are doing. </remarks>
	internal readonly ReadOnlyMemory<ushort> storage;

	public static readonly TagSet Empty;

	public bool IsEmpty => storage.IsEmpty;

	public TagSet()
	{
		storage = Empty.storage;
	}

	/// <param name="storage">Must be <b>backed by an array</b> that <b>conforms to the invariants</b>.</param>
	/// <remarks>If this is used incorrectly none of the operations on the resulting <see cref="T:Vintagestory.API.Datastructures.TagSet" /> will work.</remarks>
	internal TagSet(ReadOnlyMemory<ushort> storage)
	{
		this.storage = storage;
	}

	/// <returns> True if all any of the tags that are active in this instance are also active in the <paramref name="other" /> instance, false otherwise. </returns>
	/// <remarks> This is symmetrical, meaning a.Overlaps(b) == b.Overlaps(a) .</remarks>
	public bool Overlaps(in TagSet other)
	{
		if (storage.IsEmpty || other.storage.IsEmpty)
		{
			return false;
		}
		ReadOnlySpan<ushort> thisSpan = storage.Span;
		ReadOnlySpan<ushort> otherSpan = other.storage.Span;
		if (otherSpan[0] > thisSpan[thisSpan.Length - 1])
		{
			return false;
		}
		ushort lastOtherHandle = otherSpan[otherSpan.Length - 1];
		ReadOnlySpan<ushort> readOnlySpan = thisSpan;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ushort thisHandle = readOnlySpan[i];
			if (thisHandle > lastOtherHandle)
			{
				break;
			}
			ReadOnlySpan<ushort> readOnlySpan2 = otherSpan;
			for (int j = 0; j < readOnlySpan2.Length; j++)
			{
				ushort otherHandle = readOnlySpan2[j];
				if (thisHandle == otherHandle)
				{
					return true;
				}
				if (otherHandle > thisHandle)
				{
					break;
				}
			}
		}
		return false;
	}

	/// <returns> True if all tags that are active in this instance are also active in the <paramref name="other" /> instance, false otherwise. </returns>
	/// <remarks> This is NOT symmetrical, meaning a.IsFullyContainedIn(b) != b.IsFullyContainedIn(a) !</remarks>
	public bool IsFullyContainedIn(in TagSet other)
	{
		if (storage.IsEmpty)
		{
			return true;
		}
		if (other.storage.IsEmpty)
		{
			return false;
		}
		ReadOnlySpan<ushort> thisSpan = storage.Span;
		ReadOnlySpan<ushort> otherSpan = other.storage.Span;
		ushort firstThis = thisSpan[0];
		if (firstThis > otherSpan[otherSpan.Length - 1])
		{
			return false;
		}
		int indexOther;
		for (indexOther = 0; indexOther < otherSpan.Length && otherSpan[indexOther] < firstThis; indexOther++)
		{
		}
		ReadOnlySpan<ushort> readOnlySpan = thisSpan;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ushort thisHandle = readOnlySpan[i];
			while (true)
			{
				if (indexOther < otherSpan.Length)
				{
					ushort otherHandle = otherSpan[indexOther];
					if (otherHandle == thisHandle)
					{
						break;
					}
					if (otherHandle > thisHandle)
					{
						return false;
					}
					indexOther++;
					continue;
				}
				return false;
			}
			indexOther++;
		}
		return true;
	}

	public void ToBytes(BinaryWriter writer)
	{
		ReadOnlySpan<ushort> span = storage.Span;
		writer.Write((ushort)span.Length);
		ReadOnlySpan<ushort> readOnlySpan = span;
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ushort handle = readOnlySpan[i];
			writer.Write(handle);
		}
	}

	public static TagSet FromBytes(BinaryReader reader)
	{
		ushort[] storage = new ushort[reader.ReadUInt16()];
		for (int i = 0; i < storage.Length; i++)
		{
			storage[i] = reader.ReadUInt16();
		}
		return new TagSet(storage);
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder(256);
		ReadOnlySpan<ushort> span = storage.Span;
		for (int i = 0; i < span.Length; i++)
		{
			if (i > 0)
			{
				sb.Append(", ");
			}
			sb.Append(span[i]);
		}
		return sb.ToString();
	}

	/// <remark>
	/// Do not try to compare these against each other.<br />
	/// This is implemented for the rare case where we want to generate distinct keys based on hashes.
	/// </remark>
	public override int GetHashCode()
	{
		int acc = 0;
		ReadOnlySpan<ushort> span = storage.Span;
		for (int i = 0; i < span.Length; i++)
		{
			ushort handle = span[i];
			acc = acc * 17 + handle;
		}
		return acc;
	}

	public static bool operator ==(in TagSet self, in TagSet other)
	{
		return self.Equals(other);
	}

	public static bool operator !=(in TagSet self, in TagSet other)
	{
		return !self.Equals(other);
	}

	/// <summary>
	/// A true equality test: are these exactly the same TagSet (e.g. when searching for duplicate grid recipe ingredients using the same tags)
	/// </summary>
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if (!(obj is TagSet other))
		{
			return false;
		}
		return Equals(other);
	}

	/// <summary>
	/// A true equality test: are these exactly the same TagSet (e.g. when searching for duplicate grid recipe ingredients using the same tags)
	/// </summary>
	public bool Equals(TagSet other)
	{
		if (storage.IsEmpty && other.storage.IsEmpty)
		{
			return true;
		}
		if (storage.IsEmpty || other.storage.IsEmpty)
		{
			return false;
		}
		ReadOnlySpan<ushort> thisSpan = storage.Span;
		ReadOnlySpan<ushort> otherSpan = other.storage.Span;
		if (thisSpan.Length != otherSpan.Length)
		{
			return false;
		}
		for (int i = 0; i < thisSpan.Length; i++)
		{
			if (thisSpan[i] != otherSpan[i])
			{
				return false;
			}
		}
		return true;
	}

	static TagSet()
	{
		Empty = new TagSet(ReadOnlyMemory<ushort>.Empty);
	}
}
