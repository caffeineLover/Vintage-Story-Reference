using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Encryption;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Tokenization.Scanner
{
	// Token: 0x02000085 RID: 133
	internal class PdfTokenScanner : IPdfTokenScanner, ISeekableTokenScanner, ITokenScanner, IDisposable
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000E769 File Offset: 0x0000C969
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000E771 File Offset: 0x0000C971
		public IToken CurrentToken { get; private set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000E77A File Offset: 0x0000C97A
		public long CurrentPosition
		{
			get
			{
				return this.coreTokenScanner.CurrentPosition;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000E787 File Offset: 0x0000C987
		public long Length
		{
			get
			{
				return this.coreTokenScanner.Length;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000E794 File Offset: 0x0000C994
		public PdfTokenScanner(IInputBytes inputBytes, IObjectLocationProvider objectLocationProvider, ILookupFilterProvider filterProvider, IEncryptionHandler encryptionHandler)
		{
			this.inputBytes = inputBytes;
			this.objectLocationProvider = objectLocationProvider;
			this.filterProvider = filterProvider;
			this.encryptionHandler = encryptionHandler;
			this.coreTokenScanner = new CoreTokenScanner(inputBytes, 0, null);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000E800 File Offset: 0x0000CA00
		public void UpdateEncryptionHandler(IEncryptionHandler newHandler)
		{
			if (newHandler == null)
			{
				throw new ArgumentNullException("newHandler");
			}
			this.encryptionHandler = newHandler;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000E818 File Offset: 0x0000CA18
		public bool MoveNext()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("PdfTokenScanner");
			}
			int num = 0;
			while (this.coreTokenScanner.MoveNext() && !object.Equals(this.coreTokenScanner.CurrentToken, OperatorToken.StartObject))
			{
				if (!(this.coreTokenScanner.CurrentToken is CommentToken))
				{
					num++;
					this.previousTokens[0] = this.previousTokens[1];
					this.previousTokenPositions[0] = this.previousTokenPositions[1];
					this.previousTokens[1] = this.previousTokens[2];
					this.previousTokenPositions[1] = this.previousTokenPositions[2];
					this.previousTokens[2] = this.coreTokenScanner.CurrentToken;
					this.previousTokenPositions[2] = this.coreTokenScanner.CurrentTokenStart;
				}
			}
			if (num < 2)
			{
				return false;
			}
			long num2 = this.previousTokenPositions[1];
			NumericToken numericToken = this.previousTokens[1] as NumericToken;
			NumericToken numericToken2 = this.previousTokens[2] as NumericToken;
			if (numericToken == null || numericToken2 == null)
			{
				if (numericToken2 != null)
				{
					OperatorToken operatorToken = this.previousTokens[1] as OperatorToken;
					if (operatorToken != null)
					{
						Match match = PdfTokenScanner.EndsWithNumberRegex.Match(operatorToken.Data);
						int num3;
						if (match.Success && int.TryParse(match.Value, 511, CultureInfo.InvariantCulture, ref num3))
						{
							num2 = this.previousTokenPositions[1] + (long)match.Index;
							numericToken = new NumericToken(num3);
							goto IL_15C;
						}
						return false;
					}
				}
				return false;
			}
			IL_15C:
			bool flag = false;
			while (this.coreTokenScanner.MoveNext() && !object.Equals(this.coreTokenScanner.CurrentToken, OperatorToken.EndObject))
			{
				if (!(this.coreTokenScanner.CurrentToken is CommentToken))
				{
					if (this.coreTokenScanner.CurrentToken != OperatorToken.StartObject)
					{
						if (this.coreTokenScanner.CurrentToken == OperatorToken.StartStream)
						{
							IndirectReference indirectReference;
							indirectReference..ctor(numericToken.Long, numericToken2.Int);
							bool getLength = !this.isBruteForcing && (this.callingObject == null || !this.callingObject.Value.Equals(indirectReference));
							IndirectReference? indirectReference2 = this.callingObject;
							try
							{
								this.callingObject = new IndirectReference?(indirectReference);
								StreamToken streamToken;
								if (this.TryReadStream(this.coreTokenScanner.CurrentTokenStart, getLength, out streamToken))
								{
									this.readTokens.Clear();
									this.readTokens.Add(streamToken);
									flag = true;
								}
								goto IL_342;
							}
							finally
							{
								this.callingObject = indirectReference2;
							}
							goto IL_32C;
						}
						goto IL_32C;
						IL_342:
						this.previousTokens[0] = this.previousTokens[1];
						this.previousTokenPositions[0] = this.previousTokenPositions[1];
						this.previousTokens[1] = this.previousTokens[2];
						this.previousTokenPositions[1] = this.previousTokenPositions[2];
						this.previousTokens[2] = this.coreTokenScanner.CurrentToken;
						this.previousTokenPositions[2] = this.coreTokenScanner.CurrentTokenStart;
						continue;
						IL_32C:
						this.readTokens.Add(this.coreTokenScanner.CurrentToken);
						goto IL_342;
					}
					if (flag)
					{
						StreamToken streamToken2 = this.readTokens[0] as StreamToken;
						if (streamToken2 != null)
						{
							this.readTokens.Clear();
							this.readTokens.Add(streamToken2);
							this.coreTokenScanner.Seek(this.previousTokenPositions[0]);
							break;
						}
					}
					if (this.readTokens.Count == 3 && this.readTokens[1] is NumericToken && this.readTokens[2] is NumericToken)
					{
						IndirectReference indirectReference3;
						indirectReference3..ctor((long)numericToken.Int, numericToken2.Int);
						IToken token = this.encryptionHandler.Decrypt(indirectReference3, this.readTokens[0]);
						this.CurrentToken = new ObjectToken(num2, indirectReference3, token);
						this.readTokens.Clear();
						this.coreTokenScanner.Seek(this.previousTokenPositions[0]);
						return true;
					}
					return false;
				}
			}
			if (!flag && this.coreTokenScanner.CurrentToken != OperatorToken.EndObject)
			{
				this.readTokens.Clear();
				return false;
			}
			IndirectReference indirectReference4;
			indirectReference4..ctor(numericToken.Long, numericToken2.Int);
			IToken token2;
			if (this.readTokens.Count == 3)
			{
				NumericToken numericToken3 = this.readTokens[0] as NumericToken;
				if (numericToken3 != null)
				{
					NumericToken numericToken4 = this.readTokens[1] as NumericToken;
					if (numericToken4 != null && this.readTokens[2] == OperatorToken.R)
					{
						token2 = new IndirectReferenceToken(new IndirectReference(numericToken3.Long, numericToken4.Int));
						goto IL_48A;
					}
				}
			}
			token2 = this.readTokens[this.readTokens.Count - 1];
			IL_48A:
			token2 = this.encryptionHandler.Decrypt(indirectReference4, token2);
			this.CurrentToken = new ObjectToken(num2, indirectReference4, token2);
			this.objectLocationProvider.UpdateOffset(indirectReference4, num2);
			this.readTokens.Clear();
			return true;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000ECFC File Offset: 0x0000CEFC
		private bool TryReadStream(long startStreamTokenOffset, bool getLength, out StreamToken stream)
		{
			stream = null;
			DictionaryToken streamDictionary = this.GetStreamDictionary();
			long? num = getLength ? this.GetStreamLength(streamDictionary) : default(long?);
			NumericToken numericToken;
			if (!getLength && streamDictionary.TryGet<NumericToken>(NameToken.Length, ref numericToken))
			{
				num = new long?(numericToken.Long);
			}
			if (!PdfTokenScanner.ReadStreamTokenStart(this.inputBytes, startStreamTokenOffset))
			{
				return false;
			}
			while (this.inputBytes.MoveNext())
			{
				if (this.inputBytes.CurrentByte == 13)
				{
					if (!this.inputBytes.MoveNext())
					{
						return false;
					}
					if (this.inputBytes.CurrentByte != 10)
					{
						this.inputBytes.Seek(this.inputBytes.CurrentOffset - 1L);
					}
				}
				else if (this.inputBytes.CurrentByte != 10)
				{
					continue;
				}
				long currentOffset = this.inputBytes.CurrentOffset;
				long num2 = 0L;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				byte[] array;
				if (PdfTokenScanner.TryReadUsingLength(this.inputBytes, num, currentOffset, out array))
				{
					stream = new StreamToken(streamDictionary, array);
					return true;
				}
				List<PossibleStreamEndLocation> list = new List<PossibleStreamEndLocation>();
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						while (this.inputBytes.MoveNext())
						{
							if (num != null)
							{
								long num6 = num2;
								long? num7 = num;
								bool flag = num6 == num7.GetValueOrDefault() & num7 != null;
							}
							if (num5 < "end".Length && (char)this.inputBytes.CurrentByte == "end".get_Chars(num5))
							{
								num5++;
							}
							else if (num5 == "end".Length)
							{
								if ((char)this.inputBytes.CurrentByte == "stream".get_Chars(num4))
								{
									num3 = 0;
									num4++;
									if (num4 == "stream".Length && (!this.inputBytes.MoveNext() || ReadHelper.IsWhitespace(this.inputBytes.CurrentByte)))
									{
										PossibleStreamEndLocation possibleStreamEndLocation = new PossibleStreamEndLocation(this.inputBytes.CurrentOffset - (long)OperatorToken.EndStream.Data.Length, OperatorToken.EndStream);
										list.Add(possibleStreamEndLocation);
										if (num != null)
										{
											long num8 = num2;
											long? num7 = num;
											if (num8 > num7.GetValueOrDefault() & num7 != null)
											{
												break;
											}
										}
										num4 = 0;
									}
								}
								else if ((char)this.inputBytes.CurrentByte == "obj".get_Chars(num3))
								{
									num4 = 0;
									num3++;
									if (num3 == "obj".Length)
									{
										if (list.Count > 0)
										{
											PossibleStreamEndLocation possibleStreamEndLocation2 = list[list.Count - 1];
											this.inputBytes.Seek(possibleStreamEndLocation2.Offset + (long)possibleStreamEndLocation2.Type.Data.Length + 1L);
											break;
										}
										PossibleStreamEndLocation possibleStreamEndLocation3 = new PossibleStreamEndLocation(this.inputBytes.CurrentOffset - (long)OperatorToken.EndObject.Data.Length, OperatorToken.EndObject);
										list.Add(possibleStreamEndLocation3);
										long num9 = num2;
										long? num7 = num;
										if (num9 > num7.GetValueOrDefault() & num7 != null)
										{
											break;
										}
									}
								}
								else
								{
									num4 = 0;
									num3 = 0;
									num5 = 0;
								}
							}
							else
							{
								num4 = 0;
								num3 = 0;
								num5 = (((char)this.inputBytes.CurrentByte == "end".get_Chars(0)) ? 1 : 0);
							}
							binaryWriter.Write(this.inputBytes.CurrentByte);
							num2 += 1L;
						}
						binaryWriter.Flush();
						if (list.Count == 0)
						{
							return false;
						}
						memoryStream.Seek(0L, 0);
						if (num != null)
						{
							long length = memoryStream.Length;
							long? num7 = num;
							if (length >= num7.GetValueOrDefault() & num7 != null)
							{
								byte[] array2 = new byte[num.Value];
								memoryStream.Read(array2, 0, (int)num.Value);
								stream = new StreamToken(streamDictionary, array2);
								return true;
							}
						}
						PossibleStreamEndLocation possibleStreamEndLocation4 = list[list.Count - 1];
						long num10 = possibleStreamEndLocation4.Offset - currentOffset;
						long currentOffset2 = this.inputBytes.CurrentOffset;
						this.inputBytes.Seek(possibleStreamEndLocation4.Offset - 3L);
						this.inputBytes.MoveNext();
						if (this.inputBytes.CurrentByte == 13)
						{
							num10 -= 3L;
						}
						else
						{
							num10 -= 2L;
						}
						this.inputBytes.Seek(currentOffset2);
						byte[] array3 = new byte[num10];
						memoryStream.Read(array3, 0, (int)num10);
						stream = new StreamToken(streamDictionary, array3);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000F1B4 File Offset: 0x0000D3B4
		private static bool TryReadUsingLength(IInputBytes inputBytes, long? length, long startDataOffset, out byte[] data)
		{
			data = null;
			if (length == null || length.Value + startDataOffset >= inputBytes.Length)
			{
				return false;
			}
			byte[] array = new byte[PdfTokenScanner.EndstreamBytes.Length];
			int num = 0;
			inputBytes.Seek(length.Value + startDataOffset);
			byte? b = inputBytes.Peek();
			if (b != null && ReadHelper.IsEndOfLine(b.Value))
			{
				num++;
				inputBytes.MoveNext();
				b = inputBytes.Peek();
				if (b != null && ReadHelper.IsEndOfLine(b.Value))
				{
					num++;
					inputBytes.MoveNext();
				}
			}
			if (inputBytes.Read(array, default(int?)) != array.Length)
			{
				return false;
			}
			for (int i = 0; i < PdfTokenScanner.EndstreamBytes.Length; i++)
			{
				if (array[i] != PdfTokenScanner.EndstreamBytes[i])
				{
					inputBytes.Seek(startDataOffset);
					return false;
				}
			}
			inputBytes.Seek(startDataOffset);
			data = new byte[(int)length.Value];
			int num2 = inputBytes.Read(data, default(int?));
			if (num2 != data.Length)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(104, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Reading using the stream length failed to read as many bytes as the stream specified. Wanted ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(length.Value);
				defaultInterpolatedStringHandler.AppendLiteral(", got ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(startDataOffset + 1L);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			inputBytes.Read(array, default(int?));
			for (int j = 0; j < num; j++)
			{
				if (!inputBytes.MoveNext())
				{
					inputBytes.Seek(startDataOffset);
					return false;
				}
			}
			inputBytes.MoveNext();
			return true;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000F368 File Offset: 0x0000D568
		private DictionaryToken GetStreamDictionary()
		{
			DictionaryToken dictionaryToken = this.previousTokens[2] as DictionaryToken;
			DictionaryToken result;
			if (dictionaryToken != null)
			{
				result = dictionaryToken;
			}
			else
			{
				DictionaryToken dictionaryToken2 = this.previousTokens[1] as DictionaryToken;
				if (dictionaryToken2 == null)
				{
					string text = "No dictionary token was found prior to the 'stream' operator. Previous tokens were:";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(7, 2);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(this.previousTokens[2]);
					defaultInterpolatedStringHandler.AppendLiteral(" and ");
					defaultInterpolatedStringHandler.AppendFormatted<IToken>(this.previousTokens[1]);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentFormatException(text + defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = dictionaryToken2;
			}
			return result;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000F404 File Offset: 0x0000D604
		private long? GetStreamLength(DictionaryToken dictionary)
		{
			IToken token;
			if (!dictionary.Data.TryGetValue("Length", ref token))
			{
				return default(long?);
			}
			long? result = default(long?);
			NumericToken numericToken = token as NumericToken;
			if (numericToken != null)
			{
				return new long?(numericToken.Long);
			}
			long currentOffset = this.inputBytes.CurrentOffset;
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			long num;
			if (indirectReferenceToken != null && this.objectLocationProvider.TryGetOffset(indirectReferenceToken.Data, out num))
			{
				if (num < 0L)
				{
					ObjectToken objectFromStream = this.GetObjectFromStream(indirectReferenceToken.Data, num);
					NumericToken numericToken2 = objectFromStream.Data as NumericToken;
					if (numericToken2 == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(92, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Could not locate the length object with offset ");
						defaultInterpolatedStringHandler.AppendFormatted<long>(num);
						defaultInterpolatedStringHandler.AppendLiteral(" which should have been in a stream.");
						defaultInterpolatedStringHandler.AppendLiteral(" Found: ");
						defaultInterpolatedStringHandler.AppendFormatted<IToken>(objectFromStream.Data);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					return new long?(numericToken2.Long);
				}
				else
				{
					this.Seek(num);
					List<IToken> list = new List<IToken>(this.readTokens);
					this.readTokens.Clear();
					if (this.MoveNext())
					{
						NumericToken numericToken3 = ((ObjectToken)this.CurrentToken).Data as NumericToken;
						if (numericToken3 != null)
						{
							result = new long?(numericToken3.Long);
						}
					}
					this.readTokens.AddRange(list);
					this.Seek(currentOffset);
				}
			}
			return result;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000F57C File Offset: 0x0000D77C
		private static bool ReadStreamTokenStart(IInputBytes input, long tokenStart)
		{
			input.Seek(tokenStart);
			for (int i = 0; i < OperatorToken.StartStream.Data.Length; i++)
			{
				if (!input.MoveNext() || (char)input.CurrentByte != OperatorToken.StartStream.Data.get_Chars(i))
				{
					input.Seek(tokenStart);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000F5D4 File Offset: 0x0000D7D4
		public bool TryReadToken<T>(out T token) where T : class, IToken
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("PdfTokenScanner");
			}
			return this.coreTokenScanner.TryReadToken<T>(ref token);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000F5F5 File Offset: 0x0000D7F5
		public void Seek(long position)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("PdfTokenScanner");
			}
			this.coreTokenScanner.Seek(position);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000F616 File Offset: 0x0000D816
		public void RegisterCustomTokenizer(byte firstByte, ITokenizer tokenizer)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("PdfTokenScanner");
			}
			this.coreTokenScanner.RegisterCustomTokenizer(firstByte, tokenizer);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000F638 File Offset: 0x0000D838
		public void DeregisterCustomTokenizer(ITokenizer tokenizer)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("PdfTokenScanner");
			}
			this.coreTokenScanner.DeregisterCustomTokenizer(tokenizer);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000F65C File Offset: 0x0000D85C
		public ObjectToken Get(IndirectReference reference)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("PdfTokenScanner");
			}
			ObjectToken result;
			if (this.overwrittenTokens.TryGetValue(reference, ref result))
			{
				return result;
			}
			ObjectToken result2;
			if (this.objectLocationProvider.TryGetCached(reference, out result2))
			{
				return result2;
			}
			long num;
			if (!this.objectLocationProvider.TryGetOffset(reference, out num))
			{
				return null;
			}
			if (num < 0L)
			{
				return this.GetObjectFromStream(reference, num);
			}
			if (num == 0L && reference.Generation > 65535)
			{
				return new ObjectToken(num, reference, NullToken.Instance);
			}
			this.Seek(num);
			if (!this.MoveNext())
			{
				return this.BruteForceFileToFindReference(reference);
			}
			ObjectToken objectToken = (ObjectToken)this.CurrentToken;
			if (objectToken.Number.Equals(reference))
			{
				return objectToken;
			}
			return this.BruteForceFileToFindReference(reference);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000F727 File Offset: 0x0000D927
		public void ReplaceToken(IndirectReference reference, IToken token)
		{
			this.overwrittenTokens[reference] = new ObjectToken(0L, reference, token);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000F740 File Offset: 0x0000D940
		private ObjectToken BruteForceFileToFindReference(IndirectReference reference)
		{
			ObjectToken result;
			try
			{
				this.isBruteForcing = true;
				this.Seek(0L);
				while (this.MoveNext())
				{
					this.objectLocationProvider.Cache((ObjectToken)this.CurrentToken, true);
				}
				ObjectToken objectToken;
				if (!this.objectLocationProvider.TryGetCached(reference, out objectToken))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Could not locate object with reference: ");
					defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(reference);
					defaultInterpolatedStringHandler.AppendLiteral(" despite a full document search.");
					throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = objectToken;
			}
			finally
			{
				this.isBruteForcing = false;
			}
			return result;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000F7E0 File Offset: 0x0000D9E0
		private ObjectToken GetObjectFromStream(IndirectReference reference, long offset)
		{
			long num = offset * -1L;
			ObjectToken objectToken = this.Get(new IndirectReference(num, 0));
			StreamToken streamToken = objectToken.Data as StreamToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (streamToken == null)
			{
				string text = "Requested a stream object by reference but the requested stream object ";
				defaultInterpolatedStringHandler..ctor(21, 2);
				defaultInterpolatedStringHandler.AppendLiteral("was not a stream: ");
				defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(reference);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(objectToken.Data);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(text + defaultInterpolatedStringHandler.ToStringAndClear());
			}
			foreach (ObjectToken objectToken2 in this.ParseObjectStream(streamToken, offset))
			{
				this.objectLocationProvider.Cache(objectToken2, false);
			}
			ObjectToken result;
			if (!this.objectLocationProvider.TryGetCached(reference, out result))
			{
				defaultInterpolatedStringHandler..ctor(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Could not find the object ");
				defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(reference);
				defaultInterpolatedStringHandler.AppendLiteral(" in the stream ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(num);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return result;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000F914 File Offset: 0x0000DB14
		private IReadOnlyList<ObjectToken> ParseObjectStream(StreamToken stream, long offset)
		{
			IToken token;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (stream.StreamDictionary.TryGet(NameToken.N, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					IToken token2;
					if (!stream.StreamDictionary.TryGet(NameToken.First, ref token2) || !(token2 is NumericToken))
					{
						defaultInterpolatedStringHandler..ctor(62, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Object stream dictionary did not provide first object offset ");
						defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(stream.StreamDictionary);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					CoreTokenScanner coreTokenScanner = new CoreTokenScanner(new ByteArrayInputBytes(stream.Decode(this.filterProvider, this)), 0, null);
					List<Tuple<long, long>> list = new List<Tuple<long, long>>();
					for (int i = 0; i < numericToken.Int; i++)
					{
						coreTokenScanner.MoveNext();
						NumericToken numericToken2 = (NumericToken)coreTokenScanner.CurrentToken;
						coreTokenScanner.MoveNext();
						NumericToken numericToken3 = (NumericToken)coreTokenScanner.CurrentToken;
						list.Add(Tuple.Create<long, long>(numericToken2.Long, numericToken3.Long));
					}
					List<ObjectToken> list2 = new List<ObjectToken>();
					for (int j = 0; j < list.Count; j++)
					{
						Tuple<long, long> tuple = list[j];
						coreTokenScanner.MoveNext();
						IToken currentToken = coreTokenScanner.CurrentToken;
						if (currentToken.Equals(OperatorToken.EndObject))
						{
							coreTokenScanner.MoveNext();
							currentToken = coreTokenScanner.CurrentToken;
						}
						list2.Add(new ObjectToken(offset, new IndirectReference(tuple.Item1, 0), currentToken));
					}
					return list2;
				}
			}
			defaultInterpolatedStringHandler..ctor(60, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Object stream dictionary did not provide number of objects ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(stream.StreamDictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000FABD File Offset: 0x0000DCBD
		public void Dispose()
		{
			IInputBytes inputBytes = this.inputBytes;
			if (inputBytes != null)
			{
				inputBytes.Dispose();
			}
			this.isDisposed = true;
		}

		// Token: 0x040000F9 RID: 249
		private static readonly byte[] EndstreamBytes = new byte[]
		{
			101,
			110,
			100,
			115,
			116,
			114,
			101,
			97,
			109
		};

		// Token: 0x040000FA RID: 250
		private static readonly Regex EndsWithNumberRegex = new Regex("(?<=^[^\\s\\d]+)\\d+$");

		// Token: 0x040000FB RID: 251
		private readonly IInputBytes inputBytes;

		// Token: 0x040000FC RID: 252
		private readonly IObjectLocationProvider objectLocationProvider;

		// Token: 0x040000FD RID: 253
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x040000FE RID: 254
		private readonly CoreTokenScanner coreTokenScanner;

		// Token: 0x040000FF RID: 255
		private IEncryptionHandler encryptionHandler;

		// Token: 0x04000100 RID: 256
		private bool isDisposed;

		// Token: 0x04000101 RID: 257
		private bool isBruteForcing;

		// Token: 0x04000102 RID: 258
		private readonly Dictionary<IndirectReference, ObjectToken> overwrittenTokens = new Dictionary<IndirectReference, ObjectToken>();

		// Token: 0x04000103 RID: 259
		private readonly List<IToken> readTokens = new List<IToken>();

		// Token: 0x04000104 RID: 260
		private readonly long[] previousTokenPositions = new long[3];

		// Token: 0x04000105 RID: 261
		private readonly IToken[] previousTokens = new IToken[3];

		// Token: 0x04000107 RID: 263
		private IndirectReference? callingObject;
	}
}
