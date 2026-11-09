using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x0200021C RID: 540
	internal static class MergedDictionariesHelper
	{
		// Token: 0x06000FBB RID: 4027 RVA: 0x0002CE61 File Offset: 0x0002B061
		public static void AddIfNotNull(this Collection<ResourceDictionary> mergedDictionaries, ResourceDictionary item)
		{
			if (item != null)
			{
				mergedDictionaries.Add(item);
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0002CE6D File Offset: 0x0002B06D
		public static void RemoveIfNotNull(this Collection<ResourceDictionary> mergedDictionaries, ResourceDictionary item)
		{
			if (item != null)
			{
				mergedDictionaries.Remove(item);
			}
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0002CE7A File Offset: 0x0002B07A
		public static void InsertOrReplace(this Collection<ResourceDictionary> mergedDictionaries, int index, ResourceDictionary item)
		{
			if (mergedDictionaries.Count > index)
			{
				mergedDictionaries[index] = item;
				return;
			}
			mergedDictionaries.Insert(index, item);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0002CE98 File Offset: 0x0002B098
		public static void RemoveAll<T>(this Collection<ResourceDictionary> mergedDictionaries) where T : ResourceDictionary
		{
			for (int i = mergedDictionaries.Count - 1; i >= 0; i--)
			{
				if (mergedDictionaries[i] is T)
				{
					mergedDictionaries.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0002CECD File Offset: 0x0002B0CD
		public static void InsertIfNotExists(this Collection<ResourceDictionary> mergedDictionaries, int index, ResourceDictionary item)
		{
			if (!mergedDictionaries.Contains(item))
			{
				mergedDictionaries.Insert(index, item);
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
		public static void Swap(this Collection<ResourceDictionary> mergedDictionaries, int index1, int index2)
		{
			if (index1 == index2)
			{
				return;
			}
			int num = Math.Min(index1, index2);
			int num2 = Math.Max(index1, index2);
			ResourceDictionary resourceDictionary = mergedDictionaries[num];
			mergedDictionaries.RemoveAt(num);
			mergedDictionaries.Insert(num2, resourceDictionary);
		}
	}
}
