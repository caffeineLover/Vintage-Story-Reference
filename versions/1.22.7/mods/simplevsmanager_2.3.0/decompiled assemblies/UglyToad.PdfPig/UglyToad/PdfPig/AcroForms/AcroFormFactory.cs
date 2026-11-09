using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.AcroForms.Fields;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.AcroForms
{
	// Token: 0x0200020D RID: 525
	internal class AcroFormFactory
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
		public AcroFormFactory(IPdfTokenScanner tokenScanner, ILookupFilterProvider filterProvider, CrossReferenceTable crossReferenceTable)
		{
			if (tokenScanner == null)
			{
				throw new ArgumentNullException("tokenScanner");
			}
			this.tokenScanner = tokenScanner;
			if (filterProvider == null)
			{
				throw new ArgumentNullException("filterProvider");
			}
			this.filterProvider = filterProvider;
			if (crossReferenceTable == null)
			{
				throw new ArgumentNullException("crossReferenceTable");
			}
			this.crossReferenceTable = crossReferenceTable;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0003E1F8 File Offset: 0x0003C3F8
		[CanBeNull]
		public AcroForm GetAcroForm(Catalog catalog)
		{
			IToken token;
			if (!catalog.CatalogDictionary.TryGet(NameToken.AcroForm, ref token))
			{
				return null;
			}
			DictionaryToken dictionaryToken;
			if (!DirectObjectFinder.TryGet<DictionaryToken>(token, this.tokenScanner, out dictionaryToken))
			{
				List<IndirectReferenceToken> list = new List<IndirectReferenceToken>();
				foreach (IndirectReference indirectReference in this.crossReferenceTable.ObjectOffsets.Keys)
				{
					IndirectReferenceToken indirectReferenceToken = new IndirectReferenceToken(indirectReference);
					DictionaryToken dictionary;
					ArrayToken arrayToken;
					StringToken stringToken;
					if (DirectObjectFinder.TryGet<DictionaryToken>(indirectReferenceToken, this.tokenScanner, out dictionary) && dictionary.TryGet(NameToken.Kids, this.tokenScanner, out arrayToken) && dictionary.TryGet(NameToken.T, this.tokenScanner, out stringToken))
					{
						list.Add(indirectReferenceToken);
					}
				}
				if (list.Count == 0)
				{
					return null;
				}
				Dictionary<NameToken, IToken> dictionary2 = new Dictionary<NameToken, IToken>();
				dictionary2.Add(NameToken.Fields, new ArrayToken(list));
				dictionaryToken = new DictionaryToken(dictionary2);
			}
			SignatureFlags signatureFlags = (SignatureFlags)0;
			NumericToken numericToken;
			if (dictionaryToken.TryGetOptionalTokenDirect(NameToken.SigFlags, this.tokenScanner, out numericToken))
			{
				signatureFlags = (SignatureFlags)numericToken.Int;
			}
			bool needAppearances = false;
			BooleanToken booleanToken;
			if (dictionaryToken.TryGetOptionalTokenDirect(NameToken.NeedAppearances, this.tokenScanner, out booleanToken))
			{
				needAppearances = booleanToken.Data;
			}
			ArrayToken arrayToken2 = null;
			dictionaryToken.TryGetOptionalTokenDirect(NameToken.Co, this.tokenScanner, out arrayToken2);
			DictionaryToken dictionaryToken2 = null;
			dictionaryToken.TryGetOptionalTokenDirect(NameToken.Dr, this.tokenScanner, out dictionaryToken2);
			StringToken stringToken2;
			HexToken hexToken;
			if (dictionaryToken.TryGetOptionalTokenDirect(NameToken.Da, this.tokenScanner, out stringToken2))
			{
				string data = stringToken2.Data;
			}
			else if (dictionaryToken.TryGetOptionalTokenDirect(NameToken.Da, this.tokenScanner, out hexToken))
			{
				string data2 = hexToken.Data;
			}
			NumericToken numericToken2;
			if (dictionaryToken.TryGetOptionalTokenDirect(NameToken.Q, this.tokenScanner, out numericToken2))
			{
				int @int = numericToken2.Int;
			}
			ArrayToken arrayToken3;
			if (!dictionaryToken.TryGet(NameToken.Fields, this.tokenScanner, out arrayToken3))
			{
				return null;
			}
			Dictionary<IndirectReference, AcroFieldBase> dictionary3 = new Dictionary<IndirectReference, AcroFieldBase>(arrayToken3.Length);
			foreach (IToken token2 in arrayToken3.Data)
			{
				IndirectReferenceToken indirectReferenceToken2 = token2 as IndirectReferenceToken;
				if (indirectReferenceToken2 == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The fields array should only contain indirect references, instead got: ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(token2);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				DictionaryToken fieldDictionary = DirectObjectFinder.Get<DictionaryToken>(token2, this.tokenScanner);
				AcroFieldBase acroField = this.GetAcroField(fieldDictionary, catalog, new List<DictionaryToken>(0));
				dictionary3[indirectReferenceToken2.Data] = acroField;
			}
			return new AcroForm(dictionaryToken, signatureFlags, needAppearances, dictionary3);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0003E49C File Offset: 0x0003C69C
		private AcroFieldBase GetAcroField(DictionaryToken fieldDictionary, Catalog catalog, IReadOnlyList<DictionaryToken> parentDictionaries)
		{
			ValueTuple<DictionaryToken, bool> valueTuple = AcroFormFactory.CreateInheritedDictionary(fieldDictionary, parentDictionaries);
			DictionaryToken item = valueTuple.Item1;
			bool item2 = valueTuple.Item2;
			fieldDictionary = item;
			NameToken nameToken;
			fieldDictionary.TryGet(NameToken.Ft, this.tokenScanner, out nameToken);
			NumericToken numericToken;
			fieldDictionary.TryGet(NameToken.Ff, this.tokenScanner, out numericToken);
			List<ValueTuple<bool, DictionaryToken>> list = new List<ValueTuple<bool, DictionaryToken>>();
			ArrayToken arrayToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (fieldDictionary.TryGetOptionalTokenDirect(NameToken.Kids, this.tokenScanner, out arrayToken))
			{
				foreach (IToken token in arrayToken.Data)
				{
					IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
					if (indirectReferenceToken == null)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
						defaultInterpolatedStringHandler.AppendLiteral("AcroForm kids should only contain indirect reference, instead got: ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					ObjectToken objectToken = this.tokenScanner.Get(indirectReferenceToken.Data);
					if (objectToken == null)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Could not find the object with reference: ");
						defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(indirectReferenceToken.Data);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					DictionaryToken dictionaryToken = objectToken.Data as DictionaryToken;
					if (dictionaryToken == null)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unexpected type of kid in AcroForm field. Expected dictionary but got: ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(objectToken.Data);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					IndirectReferenceToken indirectReferenceToken2;
					bool flag = dictionaryToken.TryGet<IndirectReferenceToken>(NameToken.Parent, ref indirectReferenceToken2);
					list.Add(new ValueTuple<bool, DictionaryToken>(flag, dictionaryToken));
				}
			}
			string partialName;
			fieldDictionary.TryGetOptionalStringDirect(NameToken.T, this.tokenScanner, out partialName);
			string alternateName;
			fieldDictionary.TryGetOptionalStringDirect(NameToken.Tu, this.tokenScanner, out alternateName);
			string mappingName;
			fieldDictionary.TryGetOptionalStringDirect(NameToken.Tm, this.tokenScanner, out mappingName);
			IndirectReferenceToken indirectReferenceToken3;
			fieldDictionary.TryGet<IndirectReferenceToken>(NameToken.Parent, ref indirectReferenceToken3);
			AcroFieldCommonInformation information = new AcroFieldCommonInformation((indirectReferenceToken3 != null) ? new IndirectReference?(indirectReferenceToken3.Data) : default(IndirectReference?), partialName, alternateName, mappingName);
			int? pageNumber = default(int?);
			IndirectReferenceToken indirectReferenceToken4;
			if (fieldDictionary.TryGet(NameToken.P, this.tokenScanner, out indirectReferenceToken4))
			{
				PageTreeNode pageByReference = catalog.Pages.GetPageByReference(indirectReferenceToken4.Data);
				pageNumber = ((pageByReference != null) ? pageByReference.PageNumber : default(int?));
			}
			PdfRectangle? bounds = default(PdfRectangle?);
			ArrayToken arrayToken2;
			if (fieldDictionary.TryGet(NameToken.Rect, this.tokenScanner, out arrayToken2) && arrayToken2.Length == 4)
			{
				bounds = new PdfRectangle?(arrayToken2.ToRectangle(this.tokenScanner));
			}
			List<DictionaryToken> list2 = new List<DictionaryToken>(parentDictionaries);
			list2.Add(fieldDictionary);
			List<DictionaryToken> parentDictionaries2 = list2;
			List<AcroFieldBase> list3 = new List<AcroFieldBase>(list.Count);
			foreach (ValueTuple<bool, DictionaryToken> valueTuple2 in list)
			{
				if (valueTuple2.Item1)
				{
					list3.Add(this.GetAcroField(valueTuple2.Item2, catalog, parentDictionaries2));
				}
			}
			uint num = (uint)((numericToken != null) ? numericToken.Long : 0L);
			AcroFieldBase result;
			if (nameToken == null)
			{
				result = new AcroNonTerminalField(fieldDictionary, "Non-Terminal Field", num, information, AcroFieldType.Unknown, list3);
			}
			else if (nameToken == NameToken.Btn)
			{
				AcroButtonFieldFlags acroButtonFieldFlags = (AcroButtonFieldFlags)num;
				if (acroButtonFieldFlags.HasFlag(AcroButtonFieldFlags.Radio))
				{
					if (list3.Count > 0)
					{
						result = new AcroRadioButtonsField(fieldDictionary, nameToken, acroButtonFieldFlags, information, list3);
					}
					else
					{
						ValueTuple<bool, NameToken> checkedState = this.GetCheckedState(fieldDictionary, item2);
						bool item3 = checkedState.Item1;
						NameToken item4 = checkedState.Item2;
						result = new AcroRadioButtonField(fieldDictionary, nameToken, acroButtonFieldFlags, information, pageNumber, bounds, item4, item3);
					}
				}
				else if (acroButtonFieldFlags.HasFlag(AcroButtonFieldFlags.PushButton))
				{
					result = new AcroPushButtonField(fieldDictionary, nameToken, acroButtonFieldFlags, information, pageNumber, bounds);
				}
				else if (list3.Count > 0)
				{
					result = new AcroCheckboxesField(fieldDictionary, nameToken, acroButtonFieldFlags, information, list3);
				}
				else
				{
					ValueTuple<bool, NameToken> checkedState2 = this.GetCheckedState(fieldDictionary, item2);
					bool item5 = checkedState2.Item1;
					NameToken item6 = checkedState2.Item2;
					result = new AcroCheckboxField(fieldDictionary, nameToken, acroButtonFieldFlags, information, item6, item5, pageNumber, bounds);
				}
			}
			else if (nameToken == NameToken.Tx)
			{
				result = this.GetTextField(fieldDictionary, nameToken, num, information, pageNumber, bounds);
			}
			else if (nameToken == NameToken.Ch)
			{
				result = this.GetChoiceField(fieldDictionary, nameToken, num, information, pageNumber, bounds);
			}
			else
			{
				if (!(nameToken == NameToken.Sig))
				{
					defaultInterpolatedStringHandler..ctor(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unexpected type for field in AcroForm: ");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = new AcroSignatureField(fieldDictionary, nameToken, num, information, pageNumber, bounds);
			}
			return result;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0003E9C0 File Offset: 0x0003CBC0
		private AcroFieldBase GetTextField(DictionaryToken fieldDictionary, NameToken fieldType, uint fieldFlags, AcroFieldCommonInformation information, int? pageNumber, PdfRectangle? bounds)
		{
			string value = null;
			IToken token;
			if (fieldDictionary.TryGet(NameToken.V, ref token))
			{
				StringToken stringToken;
				HexToken hexToken;
				StreamToken stream;
				if (DirectObjectFinder.TryGet<StringToken>(token, this.tokenScanner, out stringToken))
				{
					value = stringToken.Data;
				}
				else if (DirectObjectFinder.TryGet<HexToken>(token, this.tokenScanner, out hexToken))
				{
					value = hexToken.Data;
				}
				else if (DirectObjectFinder.TryGet<StreamToken>(token, this.tokenScanner, out stream))
				{
					value = OtherEncodings.BytesAsLatin1String(Enumerable.ToArray<byte>(stream.Decode(this.filterProvider, this.tokenScanner)));
				}
			}
			int? maxLength = default(int?);
			NumericToken numericToken;
			if (fieldDictionary.TryGetOptionalTokenDirect(NameToken.MaxLen, this.tokenScanner, out numericToken))
			{
				maxLength = new int?(numericToken.Int);
			}
			return new AcroTextField(fieldDictionary, fieldType, (AcroTextFieldFlags)fieldFlags, information, value, maxLength, pageNumber, bounds);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0003EA84 File Offset: 0x0003CC84
		private AcroFieldBase GetChoiceField(DictionaryToken fieldDictionary, NameToken fieldType, uint fieldFlags, AcroFieldCommonInformation information, int? pageNumber, PdfRectangle? bounds)
		{
			string[] array = EmptyArray<string>.Instance;
			IToken token;
			if (fieldDictionary.TryGet(NameToken.V, ref token))
			{
				StringToken stringToken;
				HexToken hexToken;
				ArrayToken arrayToken;
				if (DirectObjectFinder.TryGet<StringToken>(token, this.tokenScanner, out stringToken))
				{
					array = new string[]
					{
						stringToken.Data
					};
				}
				else if (DirectObjectFinder.TryGet<HexToken>(token, this.tokenScanner, out hexToken))
				{
					array = new string[]
					{
						hexToken.Data
					};
				}
				else if (DirectObjectFinder.TryGet<ArrayToken>(token, this.tokenScanner, out arrayToken))
				{
					array = new string[arrayToken.Length];
					for (int i = 0; i < arrayToken.Length; i++)
					{
						IToken token2 = arrayToken.Data[i];
						StringToken stringToken2;
						HexToken hexToken2;
						if (DirectObjectFinder.TryGet<StringToken>(token2, this.tokenScanner, out stringToken2))
						{
							array[i] = stringToken2.Data;
						}
						else if (DirectObjectFinder.TryGet<HexToken>(token2, this.tokenScanner, out hexToken2))
						{
							array[i] = hexToken2.Data;
						}
					}
				}
			}
			int[] array2 = null;
			ArrayToken arrayToken2;
			if (fieldDictionary.TryGetOptionalTokenDirect(NameToken.I, this.tokenScanner, out arrayToken2))
			{
				array2 = new int[arrayToken2.Length];
				for (int j = 0; j < arrayToken2.Data.Count; j++)
				{
					NumericToken numericToken = DirectObjectFinder.Get<NumericToken>(arrayToken2.Data[j], this.tokenScanner);
					array2[j] = numericToken.Int;
				}
			}
			List<AcroChoiceOption> list = new List<AcroChoiceOption>();
			ArrayToken arrayToken3;
			if (fieldDictionary.TryGetOptionalTokenDirect(NameToken.Opt, this.tokenScanner, out arrayToken3))
			{
				for (int k = 0; k < arrayToken3.Data.Count; k++)
				{
					IToken token3 = arrayToken3.Data[k];
					StringToken stringToken3;
					HexToken hexToken3;
					if (DirectObjectFinder.TryGet<StringToken>(token3, this.tokenScanner, out stringToken3))
					{
						string data = stringToken3.Data;
						bool isSelected = AcroFormFactory.IsChoiceSelected(array, array2, k, data);
						list.Add(new AcroChoiceOption(k, isSelected, stringToken3.Data, null));
					}
					else if (DirectObjectFinder.TryGet<HexToken>(token3, this.tokenScanner, out hexToken3))
					{
						string data2 = hexToken3.Data;
						bool isSelected2 = AcroFormFactory.IsChoiceSelected(array, array2, k, data2);
						list.Add(new AcroChoiceOption(k, isSelected2, hexToken3.Data, null));
					}
					else
					{
						ArrayToken arrayToken4;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						if (!DirectObjectFinder.TryGet<ArrayToken>(token3, this.tokenScanner, out arrayToken4))
						{
							defaultInterpolatedStringHandler..ctor(81, 1);
							defaultInterpolatedStringHandler.AppendLiteral("An option array should contain either strings or 2 element arrays, instead got: ");
							defaultInterpolatedStringHandler.AppendFormatted<IToken>(token3);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						if (arrayToken4.Length != 2)
						{
							defaultInterpolatedStringHandler..ctor(82, 1);
							defaultInterpolatedStringHandler.AppendLiteral("An option array containing array elements should contain 2 strings, instead got: ");
							defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(arrayToken4);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						StringToken stringToken4;
						string data3;
						if (DirectObjectFinder.TryGet<StringToken>(arrayToken4.Data[0], this.tokenScanner, out stringToken4))
						{
							data3 = stringToken4.Data;
						}
						else
						{
							HexToken hexToken4;
							if (!DirectObjectFinder.TryGet<HexToken>(arrayToken4.Data[0], this.tokenScanner, out hexToken4))
							{
								defaultInterpolatedStringHandler..ctor(93, 1);
								defaultInterpolatedStringHandler.AppendLiteral("An option array array element's first value should be the export value string, instead got: ");
								defaultInterpolatedStringHandler.AppendFormatted<IToken>(arrayToken4.Data[0]);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							data3 = hexToken4.Data;
						}
						StringToken stringToken5;
						string data4;
						if (DirectObjectFinder.TryGet<StringToken>(arrayToken4.Data[1], this.tokenScanner, out stringToken5))
						{
							data4 = stringToken5.Data;
						}
						else
						{
							HexToken hexToken5;
							if (!DirectObjectFinder.TryGet<HexToken>(arrayToken4.Data[1], this.tokenScanner, out hexToken5))
							{
								defaultInterpolatedStringHandler..ctor(93, 1);
								defaultInterpolatedStringHandler.AppendLiteral("An option array array element's second value should be the option name string, instead got: ");
								defaultInterpolatedStringHandler.AppendFormatted<IToken>(arrayToken4.Data[1]);
								defaultInterpolatedStringHandler.AppendLiteral(".");
								throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							data4 = hexToken5.Data;
						}
						bool isSelected3 = AcroFormFactory.IsChoiceSelected(array, array2, k, data4);
						list.Add(new AcroChoiceOption(k, isSelected3, data4, data3));
					}
				}
			}
			if (((AcroChoiceFieldFlags)fieldFlags).HasFlag(AcroChoiceFieldFlags.Combo))
			{
				return new AcroComboBoxField(fieldDictionary, fieldType, (AcroChoiceFieldFlags)fieldFlags, information, list, array, array2, pageNumber, bounds);
			}
			int? topIndex = default(int?);
			NumericToken numericToken2;
			if (fieldDictionary.TryGetOptionalTokenDirect(NameToken.Ti, this.tokenScanner, out numericToken2))
			{
				topIndex = new int?(numericToken2.Int);
			}
			return new AcroListBoxField(fieldDictionary, fieldType, (AcroChoiceFieldFlags)fieldFlags, information, list, array, array2, topIndex, pageNumber, bounds);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0003EEF8 File Offset: 0x0003D0F8
		[return: TupleElementNames(new string[]
		{
			"isChecked",
			"stateName"
		})]
		private ValueTuple<bool, NameToken> GetCheckedState(DictionaryToken fieldDictionary, bool inheritsValue)
		{
			bool flag = false;
			NameToken nameToken;
			NameToken nameToken3;
			if (!fieldDictionary.TryGetOptionalTokenDirect(NameToken.V, this.tokenScanner, out nameToken))
			{
				NameToken nameToken2;
				DictionaryToken dictionaryToken;
				if (fieldDictionary.TryGetOptionalTokenDirect(NameToken.As, this.tokenScanner, out nameToken2) && fieldDictionary.TryGetOptionalTokenDirect(NameToken.Ap, this.tokenScanner, out dictionaryToken))
				{
					flag = !string.Equals(nameToken2.Data, NameToken.Off, 5);
					nameToken = nameToken2;
					return new ValueTuple<bool, NameToken>(flag, nameToken);
				}
				nameToken = NameToken.Off;
			}
			else if (inheritsValue && fieldDictionary.TryGet(NameToken.As, this.tokenScanner, out nameToken3))
			{
				flag = nameToken3.Equals(nameToken);
				nameToken = nameToken3;
			}
			else
			{
				flag = !string.Equals(nameToken.Data, NameToken.Off, 5);
			}
			return new ValueTuple<bool, NameToken>(flag, nameToken);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0003EFBC File Offset: 0x0003D1BC
		[return: TupleElementNames(new string[]
		{
			"dictionary",
			"inheritsValue"
		})]
		private static ValueTuple<DictionaryToken, bool> CreateInheritedDictionary(DictionaryToken fieldDictionary, IReadOnlyList<DictionaryToken> parents)
		{
			if (parents.Count == 0)
			{
				return new ValueTuple<DictionaryToken, bool>(fieldDictionary, false);
			}
			bool flag = false;
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			foreach (DictionaryToken dictionaryToken in parents)
			{
				foreach (KeyValuePair<string, IToken> keyValuePair in dictionaryToken.Data)
				{
					NameToken nameToken = NameToken.Create(keyValuePair.Key);
					if (AcroFormFactory.InheritableFields.Contains(nameToken))
					{
						dictionary[nameToken] = keyValuePair.Value;
						if (NameToken.V.Equals(nameToken))
						{
							flag = true;
						}
					}
				}
			}
			foreach (KeyValuePair<string, IToken> keyValuePair2 in fieldDictionary.Data)
			{
				NameToken nameToken2 = NameToken.Create(keyValuePair2.Key);
				dictionary[nameToken2] = keyValuePair2.Value;
				if (NameToken.V.Equals(nameToken2))
				{
					flag = false;
				}
			}
			return new ValueTuple<DictionaryToken, bool>(new DictionaryToken(dictionary), flag);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0003F0F4 File Offset: 0x0003D2F4
		private static bool IsChoiceSelected(IReadOnlyList<string> selectedOptionNames, IReadOnlyList<int> selectedOptionIndices, int index, string name)
		{
			if (selectedOptionNames.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < selectedOptionNames.Count; i++)
			{
				if (!(selectedOptionNames[i] != name))
				{
					return selectedOptionIndices == null || Enumerable.Contains<int>(selectedOptionIndices, index);
				}
			}
			return false;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0003F140 File Offset: 0x0003D340
		// Note: this type is marked as 'beforefieldinit'.
		static AcroFormFactory()
		{
			HashSet<NameToken> hashSet = new HashSet<NameToken>();
			hashSet.Add(NameToken.Ft);
			hashSet.Add(NameToken.Ff);
			hashSet.Add(NameToken.V);
			hashSet.Add(NameToken.Dv);
			hashSet.Add(NameToken.Aa);
			AcroFormFactory.InheritableFields = hashSet;
		}

		// Token: 0x040006DE RID: 1758
		private static readonly HashSet<NameToken> InheritableFields;

		// Token: 0x040006DF RID: 1759
		private readonly IPdfTokenScanner tokenScanner;

		// Token: 0x040006E0 RID: 1760
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x040006E1 RID: 1761
		private readonly CrossReferenceTable crossReferenceTable;
	}
}
