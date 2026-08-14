using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.Common.Datastructures;

public sealed class ConcurrentTagRegistry(ILogger logger_, string debugName_) : ITagRegistry<TagSet>
{
	private class ListAccess<T>
	{
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_items")]
		public static extern ref T[] Items(List<T> list);
	}

	private class MemoryAccess<T>
	{
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_object")]
		public static extern ref object Object(ref ReadOnlyMemory<T> memory);
	}

	internal readonly Dictionary<string, ushort> nameToTagHandle = new Dictionary<string, ushort>(256);

	private readonly ReaderWriterLockSlim dictLock = new ReaderWriterLockSlim();

	private const ushort HANDLE_AT_CAPACITY = 0;

	private const ushort HANDLE_LOCKED = 1;

	private ushort nextHandle = 2;

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
				if (nextHandle == 0)
				{
					return TagRegistryError.RegistryAtCapacity;
				}
				if (nextHandle == 1)
				{
					return TagRegistryError.RegistryLocked;
				}
				if (nameToTagHandle.TryAdd(tag, nextHandle))
				{
					nextHandle++;
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
				if (nextHandle == 0)
				{
					return TagRegistryError.RegistryAtCapacity;
				}
				if (nextHandle == 1)
				{
					return TagRegistryError.RegistryLocked;
				}
				if (nameToTagHandle.TryAdd(tag, nextHandle))
				{
					nextHandle++;
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
		nextHandle = 1;
	}

	public TagRegistryError TryCreateTagSet(out TagSet set, params ReadOnlySpan<string> tags)
	{
		if (tags.Length == 0)
		{
			set = TagSet.Empty;
			return TagRegistryError.None;
		}
		TagRegistryError error = TagRegistryError.None;
		List<ushort> list = new List<ushort>(tags.Length);
		bool needsSynchronization = nextHandle != 1;
		if (needsSynchronization)
		{
			dictLock.EnterReadLock();
		}
		try
		{
			ReadOnlySpan<string> readOnlySpan = tags;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				string tag = readOnlySpan[i];
				if (nameToTagHandle.TryGetValue(tag, out var handle))
				{
					list.Add(handle);
				}
				else
				{
					error = TagRegistryError.SomeTagsNotFound;
				}
			}
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitReadLock();
			}
		}
		set = CreateFromList(list);
		return error;
	}

	public TagRegistryError TryCreateTagSet(out TagSet set, IEnumerable<string> tags)
	{
		int len;
		int initialLength = (tags.TryGetNonEnumeratedCount(out len) ? len : 4);
		if (initialLength == 0)
		{
			set = TagSet.Empty;
			return TagRegistryError.None;
		}
		TagRegistryError error = TagRegistryError.None;
		List<ushort> list = new List<ushort>(initialLength);
		bool needsSynchronization = nextHandle != 1;
		if (needsSynchronization)
		{
			dictLock.EnterReadLock();
		}
		try
		{
			foreach (string tag in tags)
			{
				if (nameToTagHandle.TryGetValue(tag, out var handle))
				{
					list.Add(handle);
				}
				else
				{
					error = TagRegistryError.SomeTagsNotFound;
				}
			}
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitReadLock();
			}
		}
		set = CreateFromList(list);
		return error;
	}

	public TagRegistryError TryRegisterAndCreateTagSet(out TagSet set, params ReadOnlySpan<string> tags)
	{
		if (tags.Length == 0)
		{
			set = TagSet.Empty;
			return TagRegistryError.None;
		}
		List<ushort> list = new List<ushort>(tags.Length);
		bool needsSynchronization = nextHandle != 1;
		if (needsSynchronization)
		{
			dictLock.EnterUpgradeableReadLock();
		}
		try
		{
			ReadOnlySpan<string> readOnlySpan = tags;
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				string tag = readOnlySpan[i];
				if (!nameToTagHandle.TryGetValue(tag, out var handle))
				{
					if (needsSynchronization)
					{
						dictLock.EnterWriteLock();
					}
					try
					{
						if (nextHandle == 0)
						{
							set = CreateFromList(list);
							return TagRegistryError.RegistryAtCapacity;
						}
						if (nextHandle == 1)
						{
							set = CreateFromList(list);
							return TagRegistryError.RegistryLocked;
						}
						handle = nextHandle;
						if (nameToTagHandle.TryAdd(tag, handle))
						{
							nextHandle++;
						}
						else
						{
							handle = nameToTagHandle[tag];
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
				list.Add(handle);
			}
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitUpgradeableReadLock();
			}
		}
		set = CreateFromList(list);
		return TagRegistryError.None;
	}

	public TagRegistryError TryRegisterAndCreateTagSet(out TagSet set, IEnumerable<string> tags)
	{
		int len;
		int initialLength = (tags.TryGetNonEnumeratedCount(out len) ? len : 4);
		if (initialLength == 0)
		{
			set = TagSet.Empty;
			return TagRegistryError.None;
		}
		List<ushort> list = new List<ushort>(initialLength);
		bool needsSynchronization = nextHandle != 1;
		if (needsSynchronization)
		{
			dictLock.EnterUpgradeableReadLock();
		}
		try
		{
			foreach (string tag in tags)
			{
				if (!nameToTagHandle.TryGetValue(tag, out var handle))
				{
					if (needsSynchronization)
					{
						dictLock.EnterWriteLock();
					}
					try
					{
						if (nextHandle == 0)
						{
							set = CreateFromList(list);
							return TagRegistryError.RegistryAtCapacity;
						}
						if (nextHandle == 1)
						{
							set = CreateFromList(list);
							return TagRegistryError.RegistryLocked;
						}
						handle = nextHandle;
						if (nameToTagHandle.TryAdd(tag, handle))
						{
							nextHandle++;
						}
						else
						{
							handle = nameToTagHandle[tag];
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
				list.Add(handle);
			}
		}
		finally
		{
			if (needsSynchronization)
			{
				dictLock.ExitUpgradeableReadLock();
			}
		}
		set = CreateFromList(list);
		return TagRegistryError.None;
	}

	public TagSet CreateMergedTagSet(TagSet set1, TagSet set2)
	{
		(ushort[]?, int length) tuple = UnsafeAccessHandles(in set1);
		ushort[] handles1 = tuple.Item1;
		int len1 = tuple.length;
		(ushort[]?, int length) tuple2 = UnsafeAccessHandles(in set2);
		ushort[] handles2 = tuple2.Item1;
		int len2 = tuple2.length;
		List<ushort> list = new List<ushort>(len1 + len2);
		list.AddRange(handles1.AsSpan(0, len1));
		list.AddRange(handles2.AsSpan(0, len2));
		return CreateFromList(list);
	}

	private static TagSet CreateFromList(List<ushort> handles)
	{
		handles.Sort();
		int deduplicatedCount = DeduplicateSortedListStable(handles);
		return new TagSet(new ReadOnlyMemory<ushort>(ListAccess<ushort>.Items(handles), 0, deduplicatedCount));
	}

	public static TagSet UnsafeCreateUnchecked(ushort[] activeTags)
	{
		return new TagSet(activeTags);
	}

	public static (ushort[]?, int length) UnsafeAccessHandles(in TagSet set)
	{
		return ((ushort[])MemoryAccess<ushort>.Object(ref Unsafe.AsRef(in set.storage)), length: set.storage.Length);
	}

	public IEnumerable<string> SlowEnumerateTagNames(TagSet set)
	{
		if (set.storage.Length == 0)
		{
			yield break;
		}
		var (array, length) = UnsafeAccessHandles(in set);
		foreach (var (name, handle) in nameToTagHandle)
		{
			if (Array.BinarySearch(array, 0, length, handle) >= 0)
			{
				yield return name;
			}
		}
	}

	private static int DeduplicateSortedListStable(List<ushort> list)
	{
		if (list.Count < 2)
		{
			return list.Count;
		}
		int dst = 0;
		for (int src = 1; src < list.Count; src++)
		{
			if (list[dst] != list[src])
			{
				dst++;
				list[dst] = list[src];
			}
		}
		return dst + 1;
	}
}
