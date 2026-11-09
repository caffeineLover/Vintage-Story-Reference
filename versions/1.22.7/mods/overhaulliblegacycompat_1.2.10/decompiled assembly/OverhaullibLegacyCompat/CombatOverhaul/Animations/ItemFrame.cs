using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration.Transpilers;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018E RID: 398
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ItemFrame
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0005DF45 File Offset: 0x0005C145
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

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x0005DF4D File Offset: 0x0005C14D
		public int Count
		{
			get
			{
				return this._elements.Length;
			}
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0005DF58 File Offset: 0x0005C158
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

		// Token: 0x06001360 RID: 4960 RVA: 0x0005E080 File Offset: 0x0005C280
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

		// Token: 0x06001361 RID: 4961 RVA: 0x0005E0E0 File Offset: 0x0005C2E0
		public void Apply(ElementPose pose)
		{
			AnimationElement element;
			if (this.TryGetElement(pose.ForElement.Name, out element))
			{
				element.Apply(pose);
			}
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0005E10C File Offset: 0x0005C30C
		public void Apply(ExtendedElementPose pose)
		{
			AnimationElement element;
			if (this.TryGetElement(pose.ElementNameHash, out element))
			{
				element.Apply(pose);
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0005E134 File Offset: 0x0005C334
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

		// Token: 0x06001364 RID: 4964 RVA: 0x0005E1D8 File Offset: 0x0005C3D8
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

		// Token: 0x06001365 RID: 4965 RVA: 0x0005E3A0 File Offset: 0x0005C5A0
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

		// Token: 0x06001366 RID: 4966 RVA: 0x0005E41C File Offset: 0x0005C61C
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

		// Token: 0x06001367 RID: 4967 RVA: 0x0005E463 File Offset: 0x0005C663
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

		// Token: 0x06001368 RID: 4968 RVA: 0x0005E478 File Offset: 0x0005C678
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

		// Token: 0x04000968 RID: 2408
		public readonly int ElementsHash;

		// Token: 0x04000969 RID: 2409
		private readonly string[] _elementNames;

		// Token: 0x0400096A RID: 2410
		private readonly int[] _elementNameHashes;

		// Token: 0x0400096B RID: 2411
		private readonly AnimationElement[] _elements;

		// Token: 0x0400096C RID: 2412
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private readonly Dictionary<string, int> _elementIndexes;

		// Token: 0x0400096D RID: 2413
		public static readonly ItemFrame Empty = new ItemFrame(new Dictionary<string, AnimationElement>());
	}
}
