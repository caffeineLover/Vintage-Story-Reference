using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration.Transpilers;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000181 RID: 385
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ItemFrame
	{
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0005C4F1 File Offset: 0x0005A6F1
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		public IEnumerable<KeyValuePair<string, AnimationElement>> Elements
		{
			[return: Nullable(new byte[]
			{
				1,
				0,
				1
			})]
			get
			{
				return this.EnumerateElements();
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0005C4F9 File Offset: 0x0005A6F9
		public int Count
		{
			get
			{
				return this._elements.Length;
			}
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0005C504 File Offset: 0x0005A704
		public ItemFrame(Dictionary<string, AnimationElement> elements)
		{
			this.ElementsHash = 0;
			this._elementNames = Array.Empty<string>();
			this._elementNameHashes = Array.Empty<int>();
			this._elements = Array.Empty<AnimationElement>();
			this._elementIndexes = null;
			this._elementNames = new string[elements.Count];
			this._elementNameHashes = new int[elements.Count];
			this._elements = new AnimationElement[elements.Count];
			this._elementIndexes = new Dictionary<string, int>(elements.Count, StringComparer.Ordinal);
			int index = 0;
			foreach (KeyValuePair<string, AnimationElement> keyValuePair in elements)
			{
				string text;
				AnimationElement animationElement;
				keyValuePair.Deconstruct(ref text, ref animationElement);
				string code = text;
				AnimationElement value = animationElement;
				int hash = code.GetHashCode();
				this._elementNames[index] = code;
				this._elementNameHashes[index] = hash;
				this._elements[index] = value;
				this._elementIndexes.Add(code, index);
				this.ElementsHash = ((index == 0) ? hash : HashCode.Combine<int, int>(this.ElementsHash, hash));
				index++;
			}
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0005C62C File Offset: 0x0005A82C
		private ItemFrame(string[] elementNames, int[] elementNameHashes, AnimationElement[] elements, int elementsHash)
		{
			this.ElementsHash = 0;
			this._elementNames = Array.Empty<string>();
			this._elementNameHashes = Array.Empty<int>();
			this._elements = Array.Empty<AnimationElement>();
			this._elementIndexes = null;
			this._elementNames = elementNames;
			this._elementNameHashes = elementNameHashes;
			this._elements = elements;
			this._elementIndexes = null;
			this.ElementsHash = elementsHash;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0005C68C File Offset: 0x0005A88C
		public void Apply(ElementPose pose)
		{
			AnimationElement element;
			if (this.TryGetElement(pose.ForElement.Name, out element))
			{
				element.Apply(pose);
			}
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0005C6B8 File Offset: 0x0005A8B8
		public void Apply(ExtendedElementPose pose)
		{
			AnimationElement element;
			if (this.TryGetElement(pose.ElementNameHash, out element))
			{
				element.Apply(pose);
			}
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0005C6E0 File Offset: 0x0005A8E0
		public static ItemFrame Interpolate(ItemFrame from, ItemFrame to, float progress)
		{
			if (from.Count == 0)
			{
				return to;
			}
			if (to.Count == 0)
			{
				return ItemFrame.Empty;
			}
			AnimationElement[] elements = new AnimationElement[to._elements.Length];
			for (int index = 0; index < to._elements.Length; index++)
			{
				AnimationElement fromElement;
				if (from.TryGetElement(to._elementNames[index], out fromElement))
				{
					elements[index] = AnimationElement.Interpolate(fromElement, to._elements[index], progress);
				}
				else
				{
					elements[index] = to._elements[index];
				}
			}
			return new ItemFrame(to._elementNames, to._elementNameHashes, elements, to.ElementsHash);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0005C784 File Offset: 0x0005A984
		public static ItemFrame Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<ValueTuple<ItemFrame, float>> frames)
		{
			IReadOnlyList<ValueTuple<ItemFrame, float>> materializedFrames = frames as IReadOnlyList<ValueTuple<ItemFrame, float>>;
			if (materializedFrames == null)
			{
				materializedFrames = Enumerable.ToArray<ValueTuple<ItemFrame, float>>(frames);
			}
			if (materializedFrames.Count == 0)
			{
				return ItemFrame.Empty;
			}
			List<string> names = new List<string>();
			List<int> hashes = new List<int>();
			foreach (ValueTuple<ItemFrame, float> valueTuple in materializedFrames)
			{
				ItemFrame frame = valueTuple.Item1;
				for (int elementIndex = 0; elementIndex < frame._elementNames.Length; elementIndex++)
				{
					string name = frame._elementNames[elementIndex];
					if (!ItemFrame.ContainsName(names, name))
					{
						names.Add(name);
						hashes.Add(frame._elementNameHashes[elementIndex]);
					}
				}
			}
			if (names.Count == 0)
			{
				return ItemFrame.Empty;
			}
			AnimationElement[] elements = new AnimationElement[names.Count];
			List<ValueTuple<AnimationElement, float>> weightedElements = new List<ValueTuple<AnimationElement, float>>(materializedFrames.Count);
			for (int nameIndex = 0; nameIndex < names.Count; nameIndex++)
			{
				weightedElements.Clear();
				string name2 = names[nameIndex];
				foreach (ValueTuple<ItemFrame, float> valueTuple2 in materializedFrames)
				{
					ItemFrame frame2 = valueTuple2.Item1;
					float weight = valueTuple2.Item2;
					AnimationElement element;
					if (frame2.TryGetElement(name2, out element))
					{
						weightedElements.Add(new ValueTuple<AnimationElement, float>(element, weight));
					}
				}
				elements[nameIndex] = AnimationElement.Compose(weightedElements);
			}
			string[] elementNames = names.ToArray();
			int[] elementHashes = hashes.ToArray();
			int elementsHash = 0;
			for (int index = 0; index < elementHashes.Length; index++)
			{
				elementsHash = ((index == 0) ? elementHashes[index] : HashCode.Combine<int, int>(elementsHash, elementHashes[index]));
			}
			return new ItemFrame(elementNames, elementHashes, elements, elementsHash);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0005C94C File Offset: 0x0005AB4C
		private bool TryGetElement(string name, out AnimationElement element)
		{
			int mappedIndex;
			if (this._elementIndexes != null && this._elementIndexes.TryGetValue(name, ref mappedIndex))
			{
				element = this._elements[mappedIndex];
				return true;
			}
			for (int index = 0; index < this._elementNames.Length; index++)
			{
				if (string.Equals(this._elementNames[index], name, 4))
				{
					element = this._elements[index];
					return true;
				}
			}
			element = default(AnimationElement);
			return false;
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0005C9C8 File Offset: 0x0005ABC8
		private bool TryGetElement(int nameHash, out AnimationElement element)
		{
			for (int index = 0; index < this._elementNameHashes.Length; index++)
			{
				if (this._elementNameHashes[index] == nameHash)
				{
					element = this._elements[index];
					return true;
				}
			}
			element = default(AnimationElement);
			return false;
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0005CA0F File Offset: 0x0005AC0F
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private IEnumerable<KeyValuePair<string, AnimationElement>> EnumerateElements()
		{
			ItemFrame.<EnumerateElements>d__18 <EnumerateElements>d__ = new ItemFrame.<EnumerateElements>d__18(-2);
			<EnumerateElements>d__.<>3__<>4__this = this;
			return <EnumerateElements>d__;
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0005CA24 File Offset: 0x0005AC24
		private static bool ContainsName(List<string> names, string name)
		{
			using (List<string>.Enumerator enumerator = names.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (string.Equals(enumerator.Current, name, 4))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000940 RID: 2368
		public readonly int ElementsHash;

		// Token: 0x04000941 RID: 2369
		private readonly string[] _elementNames;

		// Token: 0x04000942 RID: 2370
		private readonly int[] _elementNameHashes;

		// Token: 0x04000943 RID: 2371
		private readonly AnimationElement[] _elements;

		// Token: 0x04000944 RID: 2372
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private readonly Dictionary<string, int> _elementIndexes;

		// Token: 0x04000945 RID: 2373
		public static readonly ItemFrame Empty = new ItemFrame(new Dictionary<string, AnimationElement>());
	}
}
