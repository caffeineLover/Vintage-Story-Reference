using System;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x02000220 RID: 544
	internal static class ResourceDictionaryHelper
	{
		// Token: 0x06000FCB RID: 4043 RVA: 0x0002D064 File Offset: 0x0002B264
		public static void SealValues(this ResourceDictionary dictionary)
		{
			foreach (ResourceDictionary dictionary2 in dictionary.MergedDictionaries)
			{
				dictionary2.SealValues();
			}
			foreach (object obj in dictionary.Values)
			{
				Freezable freezable = obj as Freezable;
				if (freezable != null)
				{
					if (!freezable.CanFreeze)
					{
						LocalValueEnumerator localValueEnumerator = freezable.GetLocalValueEnumerator();
						while (localValueEnumerator.MoveNext())
						{
							LocalValueEntry localValueEntry = localValueEnumerator.Current;
							DependencyProperty property = localValueEntry.Property;
							if (DependencyPropertyHelper.GetValueSource(freezable, property).IsExpression)
							{
								freezable.SetValue(property, freezable.GetValue(property));
							}
						}
					}
					if (!freezable.IsFrozen)
					{
						freezable.Freeze();
					}
				}
				else
				{
					Style style = obj as Style;
					if (style != null && !style.IsSealed)
					{
						style.Seal();
					}
				}
			}
			ResourceDictionaryEx resourceDictionaryEx = dictionary as ResourceDictionaryEx;
			if (resourceDictionaryEx != null)
			{
				foreach (ResourceDictionary dictionary3 in resourceDictionaryEx.ThemeDictionaries.Values)
				{
					dictionary3.SealValues();
				}
			}
		}
	}
}
