using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.Common.Datastructures;

public sealed class ConcurrentTagRegistryFast(ILogger logger_, string debugName_) : ITagRegistry<TagSetFast>
{
	internal readonly Dictionary<string, ushort> nameToTagBitOffset = new Dictionary<string, ushort>(256);

	private readonly ReaderWriterLockSlim dictLock = new ReaderWriterLockSlim();

	private const ushort OFFSET_AT_CAPACITY = 256;

	private const ushort OFFSET_LOCKED = 257;

	private ushort nextOffset;

	public ILogger logger => logger_;

	public string debugName => debugName_;

	public TagRegistryError TryRegister(params ReadOnlySpan<string> tags)
	{
		dictLock.EnterWriteLock();
		try
		{
			ReadOnlySpan<string> readOnlySpan = tags;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				string tag = readOnlySpan[i];
				if (nextOffset == 256)
				{
					return TagRegistryError.RegistryAtCapacity;
				}
				if (nextOffset == 257)
				{
					return TagRegistryError.RegistryLocked;
				}
				if (nameToTagBitOffset.TryAdd(tag, nextOffset))
				{
					nextOffset++;
				}
			}
			return TagRegistryError.None;
		}
		finally
		{
			dictLock.ExitWriteLock();
		}
	}

	public TagRegistryError TryRegister(IEnumerable<string> tags)
	{
		dictLock.EnterWriteLock();
		try
		{
			foreach (string tag in tags)
			{
				if (nextOffset == 256)
				{
					return TagRegistryError.RegistryAtCapacity;
				}
				if (nextOffset == 257)
				{
					return TagRegistryError.RegistryLocked;
				}
				if (nameToTagBitOffset.TryAdd(tag, nextOffset))
				{
					nextOffset++;
				}
			}
			return TagRegistryError.None;
		}
		finally
		{
			dictLock.ExitWriteLock();
		}
	}

	public void Lock()
	{
		nextOffset = 257;
	}

	public TagRegistryError TryCreateTagSet(out TagSetFast set, params ReadOnlySpan<string> tags)
	{
		set = default(TagSetFast);
		TagRegistryError error = TagRegistryError.None;
		bool needsSynchronization = nextOffset != 257;
		if (needsSynchronization)
		{
			dictLock.EnterReadLock();
		}
		try
		{
			ref ulong address = ref Unsafe.As<TagSetFast, ulong>(ref set);
			ReadOnlySpan<string> readOnlySpan = tags;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				string tag = readOnlySpan[i];
				if (nameToTagBitOffset.TryGetValue(tag, out var offset))
				{
					Unsafe.Add(ref address, offset / 64) |= (ulong)(1L << (int)offset);
				}
				else
				{
					error = TagRegistryError.SomeTagsNotFound;
				}
			}
			return error;
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitReadLock();
			}
		}
	}

	public TagRegistryError TryCreateTagSet(out TagSetFast set, IEnumerable<string> tags)
	{
		set = default(TagSetFast);
		TagRegistryError error = TagRegistryError.None;
		bool needsSynchronization = nextOffset != 257;
		if (needsSynchronization)
		{
			dictLock.EnterReadLock();
		}
		try
		{
			ref ulong address = ref Unsafe.As<TagSetFast, ulong>(ref set);
			foreach (string tag in tags)
			{
				if (nameToTagBitOffset.TryGetValue(tag, out var offset))
				{
					Unsafe.Add(ref address, offset / 64) |= (ulong)(1L << (int)offset);
				}
				else
				{
					error = TagRegistryError.SomeTagsNotFound;
				}
			}
			return error;
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitReadLock();
			}
		}
	}

	public TagRegistryError TryRegisterAndCreateTagSet(out TagSetFast set, params ReadOnlySpan<string> tags)
	{
		set = default(TagSetFast);
		if (tags.Length == 0)
		{
			return TagRegistryError.None;
		}
		bool needsSynchronization = nextOffset != 257;
		if (needsSynchronization)
		{
			dictLock.EnterUpgradeableReadLock();
		}
		try
		{
			ref ulong address = ref Unsafe.As<TagSetFast, ulong>(ref set);
			ReadOnlySpan<string> readOnlySpan = tags;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				string tag = readOnlySpan[i];
				if (!nameToTagBitOffset.TryGetValue(tag, out var offset))
				{
					if (needsSynchronization)
					{
						dictLock.EnterWriteLock();
					}
					try
					{
						if (nextOffset == 256)
						{
							return TagRegistryError.RegistryAtCapacity;
						}
						if (nextOffset == 257)
						{
							return TagRegistryError.RegistryLocked;
						}
						offset = nextOffset;
						if (nameToTagBitOffset.TryAdd(tag, offset))
						{
							nextOffset++;
						}
						else
						{
							offset = nameToTagBitOffset[tag];
						}
					}
					finally
					{
						if (needsSynchronization)
						{
							dictLock.ExitWriteLock();
						}
					}
				}
				Unsafe.Add(ref address, offset / 64) |= (ulong)(1L << (int)offset);
			}
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitUpgradeableReadLock();
			}
		}
		return TagRegistryError.None;
	}

	public TagRegistryError TryRegisterAndCreateTagSet(out TagSetFast set, IEnumerable<string> tags)
	{
		set = default(TagSetFast);
		bool needsSynchronization = nextOffset != 257;
		if (needsSynchronization)
		{
			dictLock.EnterUpgradeableReadLock();
		}
		try
		{
			ref ulong address = ref Unsafe.As<TagSetFast, ulong>(ref set);
			foreach (string tag in tags)
			{
				if (!nameToTagBitOffset.TryGetValue(tag, out var offset))
				{
					if (needsSynchronization)
					{
						dictLock.EnterWriteLock();
					}
					try
					{
						if (nextOffset == 256)
						{
							return TagRegistryError.RegistryAtCapacity;
						}
						if (nextOffset == 257)
						{
							return TagRegistryError.RegistryLocked;
						}
						offset = nextOffset;
						if (nameToTagBitOffset.TryAdd(tag, offset))
						{
							nextOffset++;
						}
						else
						{
							offset = nameToTagBitOffset[tag];
						}
					}
					finally
					{
						if (needsSynchronization)
						{
							dictLock.ExitWriteLock();
						}
					}
				}
				Unsafe.Add(ref address, offset / 64) |= (ulong)(1L << (int)offset);
			}
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitUpgradeableReadLock();
			}
		}
		return TagRegistryError.None;
	}

	public TagSetFast CreateMergedTagSet(TagSetFast set1, TagSetFast set2)
	{
		return set1 | set2;
	}

	public IEnumerable<string> SlowEnumerateTagNames(TagSetFast set)
	{
		foreach (KeyValuePair<string, ushort> item in nameToTagBitOffset)
		{
			item.Deconstruct(out var key, out var value);
			string name = key;
			ushort num = value;
			int block = num / 64;
			int shift = num % 64;
			if (((set.storage[block] >> shift) & 1) == 1)
			{
				yield return name;
			}
		}
	}
}
