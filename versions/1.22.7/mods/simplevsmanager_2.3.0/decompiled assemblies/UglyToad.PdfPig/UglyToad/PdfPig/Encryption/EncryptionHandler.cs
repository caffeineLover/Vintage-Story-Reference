using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Exceptions;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001D2 RID: 466
	internal class EncryptionHandler : IEncryptionHandler
	{
		// Token: 0x06000B06 RID: 2822 RVA: 0x00037A98 File Offset: 0x00035C98
		public EncryptionHandler(EncryptionDictionary encryptionDictionary, TrailerDictionary trailerDictionary, IReadOnlyList<string> passwords)
		{
			this.encryptionDictionary = encryptionDictionary;
			IReadOnlyList<string> readOnlyList;
			if ((readOnlyList = passwords) == null)
			{
				(readOnlyList = new string[1])[0] = string.Empty;
			}
			passwords = readOnlyList;
			if (!Enumerable.Contains<string>(passwords, string.Empty))
			{
				List<string> list = new List<string>(passwords);
				list.Add(string.Empty);
				passwords = list;
			}
			byte[] array;
			if (trailerDictionary.Identifier != null && trailerDictionary.Identifier.Count == 2)
			{
				IDataToken<string> dataToken = trailerDictionary.Identifier[0];
				HexToken hexToken = dataToken as HexToken;
				if (hexToken != null)
				{
					array = Enumerable.ToArray<byte>(hexToken.Bytes);
				}
				else
				{
					array = OtherEncodings.StringAsLatin1Bytes(dataToken.Data);
				}
			}
			else
			{
				array = EmptyArray<byte>.Instance;
			}
			if (encryptionDictionary == null)
			{
				return;
			}
			this.useAes = false;
			if (encryptionDictionary.EncryptionAlgorithmCode == EncryptionAlgorithmCode.SecurityHandlerInDocument || encryptionDictionary.EncryptionAlgorithmCode == EncryptionAlgorithmCode.SecurityHandlerInDocument256)
			{
				CryptHandler cryptHandler;
				if (!encryptionDictionary.TryGetCryptHandler(out cryptHandler))
				{
					throw new PdfDocumentEncryptedException("Document encrypted with security handler in document but no crypt dictionary found.", encryptionDictionary);
				}
				this.cryptHandler = cryptHandler;
				bool flag;
				if (cryptHandler != null)
				{
					CryptDictionary streamDictionary = cryptHandler.StreamDictionary;
					CryptDictionary.Method? method = (streamDictionary != null) ? new CryptDictionary.Method?(streamDictionary.Name) : default(CryptDictionary.Method?);
					CryptDictionary.Method method2 = CryptDictionary.Method.AesV2;
					if (method.GetValueOrDefault() == method2 & method != null)
					{
						flag = true;
						goto IL_163;
					}
				}
				if (cryptHandler == null)
				{
					flag = false;
				}
				else
				{
					CryptDictionary streamDictionary2 = cryptHandler.StreamDictionary;
					CryptDictionary.Method? method = (streamDictionary2 != null) ? new CryptDictionary.Method?(streamDictionary2.Name) : default(CryptDictionary.Method?);
					CryptDictionary.Method method2 = CryptDictionary.Method.AesV3;
					flag = (method.GetValueOrDefault() == method2 & method != null);
				}
				IL_163:
				this.useAes = flag;
			}
			Encoding encoding = OtherEncodings.Iso88591;
			if (encryptionDictionary.Revision == 5 || encryptionDictionary.Revision == 6)
			{
				encoding = Encoding.UTF8;
			}
			int length = (encryptionDictionary.EncryptionAlgorithmCode == EncryptionAlgorithmCode.Rc4OrAes40BitKey) ? 5 : (encryptionDictionary.KeyLength.GetValueOrDefault() / 8);
			bool flag2 = false;
			foreach (string text in passwords)
			{
				byte[] bytes = encoding.GetBytes(text);
				bool isUserPassword = false;
				byte[] array2;
				byte[] password;
				if (EncryptionHandler.IsOwnerPassword(bytes, encryptionDictionary, length, array, out array2))
				{
					if (encryptionDictionary.Revision == 5 || encryptionDictionary.Revision == 6)
					{
						password = bytes;
					}
					else
					{
						password = array2;
					}
				}
				else
				{
					if (!EncryptionHandler.IsUserPassword(bytes, encryptionDictionary, length, array))
					{
						continue;
					}
					password = bytes;
					isUserPassword = true;
				}
				this.encryptionKey = EncryptionHandler.CalculateEncryptionKey(password, encryptionDictionary, length, array, isUserPassword);
				flag2 = true;
				break;
			}
			if (!flag2)
			{
				throw new PdfDocumentEncryptedException("The document was encrypted and none of the provided passwords were the user or owner password.", encryptionDictionary);
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00037CF8 File Offset: 0x00035EF8
		private static bool IsUserPassword(byte[] passwordBytes, EncryptionDictionary encryptionDictionary, int length, byte[] documentIdBytes)
		{
			if (encryptionDictionary.Revision == 5 || encryptionDictionary.Revision == 6)
			{
				return EncryptionHandler.IsUserPasswordRevision5And6(passwordBytes, encryptionDictionary);
			}
			byte[] array = EncryptionHandler.CalculateKeyRevisions2To4(passwordBytes, encryptionDictionary, length, documentIdBytes);
			byte[] array3;
			if (encryptionDictionary.Revision >= 3)
			{
				using (MD5 md = MD5.Create())
				{
					EncryptionHandler.UpdateMd5(md, EncryptionHandler.PaddingBytes);
					EncryptionHandler.UpdateMd5(md, documentIdBytes);
					md.TransformFinalBlock(EmptyArray<byte>.Instance, 0, 0);
					byte[] hash = md.Hash;
					byte[] array2 = RC4.Encrypt(array, hash);
					byte j;
					byte i;
					for (i = 1; i <= 19; i = j + 1)
					{
						array2 = RC4.Encrypt(Enumerable.ToArray<byte>(Enumerable.Select<byte, byte>(array, (byte x) => x ^ i)), array2);
						j = i;
					}
					array3 = array2;
					goto IL_D3;
				}
			}
			array3 = RC4.Encrypt(array, EncryptionHandler.PaddingBytes);
			IL_D3:
			if (encryptionDictionary.Revision >= 3)
			{
				return Enumerable.SequenceEqual<byte>(Enumerable.Take<byte>(encryptionDictionary.UserBytes, 16), Enumerable.Take<byte>(array3, 16));
			}
			return Enumerable.SequenceEqual<byte>(encryptionDictionary.UserBytes, array3);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00037E18 File Offset: 0x00036018
		private static bool IsUserPasswordRevision5And6(byte[] passwordBytes, EncryptionDictionary encryptionDictionary)
		{
			byte[] array = EncryptionHandler.TruncatePasswordTo127Bytes(passwordBytes);
			byte[] array2 = new byte[32];
			byte[] array3 = new byte[8];
			Array.Copy(encryptionDictionary.UserBytes, array2, 32);
			Array.Copy(encryptionDictionary.UserBytes, 32, array3, 0, 8);
			byte[] array4;
			if (encryptionDictionary.Revision == 6)
			{
				array4 = EncryptionHandler.ComputeStupidIsoHash(array, array3, null);
			}
			else
			{
				array4 = EncryptionHandler.ComputeSha256Hash(array, array3, null);
			}
			return Enumerable.SequenceEqual<byte>(array4, array2);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00037E80 File Offset: 0x00036080
		private static bool IsOwnerPassword(byte[] passwordBytes, EncryptionDictionary encryptionDictionary, int length, byte[] documentIdBytes, out byte[] userPassword)
		{
			userPassword = null;
			if (encryptionDictionary.Revision == 5 || encryptionDictionary.Revision == 6)
			{
				return EncryptionHandler.IsOwnerPasswordRevision5And6(passwordBytes, encryptionDictionary);
			}
			byte[] paddedPassword = EncryptionHandler.GetPaddedPassword(passwordBytes);
			bool result;
			using (MD5 md = MD5.Create())
			{
				byte[] array = md.ComputeHash(paddedPassword);
				if (encryptionDictionary.Revision >= 3)
				{
					for (int j = 0; j < 50; j++)
					{
						array = md.ComputeHash(md.Hash);
					}
				}
				byte[] array2 = Enumerable.ToArray<byte>(Enumerable.Take<byte>(array, length));
				if (encryptionDictionary.Revision == 2)
				{
					userPassword = RC4.Encrypt(array2, encryptionDictionary.OwnerBytes);
				}
				else
				{
					byte[] array3 = null;
					int i2;
					int i;
					for (i = 0; i < 20; i = i2 + 1)
					{
						byte[] key = Enumerable.ToArray<byte>(Enumerable.Select<byte, byte>(array2, (byte x) => (byte)((int)x ^ 19 - i)));
						if (i == 0)
						{
							array3 = encryptionDictionary.OwnerBytes;
						}
						array3 = RC4.Encrypt(key, array3);
						i2 = i;
					}
					userPassword = array3;
				}
				result = EncryptionHandler.IsUserPassword(userPassword, encryptionDictionary, length, documentIdBytes);
			}
			return result;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00037FA4 File Offset: 0x000361A4
		private static bool IsOwnerPasswordRevision5And6(byte[] passwordBytes, EncryptionDictionary encryptionDictionary)
		{
			byte[] array = EncryptionHandler.TruncatePasswordTo127Bytes(passwordBytes);
			byte[] array2 = new byte[32];
			byte[] array3 = new byte[8];
			Array.Copy(encryptionDictionary.OwnerBytes, array2, array2.Length);
			Array.Copy(encryptionDictionary.OwnerBytes, array2.Length, array3, 0, array3.Length);
			byte[] array4;
			if (encryptionDictionary.Revision == 6)
			{
				array4 = EncryptionHandler.ComputeStupidIsoHash(array, array3, encryptionDictionary.UserBytes);
			}
			else
			{
				array4 = EncryptionHandler.ComputeSha256Hash(array, array3, encryptionDictionary.UserBytes);
			}
			return Enumerable.SequenceEqual<byte>(array4, array2);
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00038018 File Offset: 0x00036218
		public IToken Decrypt(IndirectReference reference, IToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			IToken result;
			try
			{
				token = this.DecryptInternal(reference, token);
				this.previouslyDecrypted.Add(reference);
				result = token;
			}
			catch (Exception inner)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(73, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The document was encrypted and decryption of a token failed. Token was: ");
				defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear(), this.encryptionDictionary, inner);
			}
			return result;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x000380A0 File Offset: 0x000362A0
		private IToken DecryptInternal(IndirectReference reference, IToken token)
		{
			StreamToken streamToken = token as StreamToken;
			if (streamToken != null)
			{
				CryptHandler cryptHandler = this.cryptHandler;
				bool flag;
				if (cryptHandler == null)
				{
					flag = false;
				}
				else
				{
					CryptDictionary streamDictionary = cryptHandler.StreamDictionary;
					bool? flag2 = (streamDictionary != null) ? new bool?(streamDictionary.IsIdentity) : default(bool?);
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (!flag)
				{
					CryptHandler cryptHandler2 = this.cryptHandler;
					bool flag4;
					if (cryptHandler2 == null)
					{
						flag4 = false;
					}
					else
					{
						CryptDictionary streamDictionary2 = cryptHandler2.StreamDictionary;
						CryptDictionary.Method? method = (streamDictionary2 != null) ? new CryptDictionary.Method?(streamDictionary2.Name) : default(CryptDictionary.Method?);
						CryptDictionary.Method method2 = CryptDictionary.Method.None;
						flag4 = (method.GetValueOrDefault() == method2 & method != null);
					}
					if (!flag4)
					{
						NameToken nameToken;
						if (streamToken.StreamDictionary.TryGet<NameToken>(NameToken.Type, ref nameToken))
						{
							if (NameToken.Xref.Equals(nameToken))
							{
								return token;
							}
							if (!this.encryptionDictionary.EncryptMetadata && NameToken.Metadata.Equals(nameToken))
							{
								return token;
							}
						}
						DictionaryToken dictionaryToken = (DictionaryToken)this.DecryptInternal(reference, streamToken.StreamDictionary);
						byte[] array = this.DecryptData(Enumerable.ToArray<byte>(streamToken.Data), reference);
						return new StreamToken(dictionaryToken, array);
					}
				}
				return token;
			}
			StringToken stringToken = token as StringToken;
			if (stringToken != null)
			{
				CryptHandler cryptHandler3 = this.cryptHandler;
				bool flag5;
				if (cryptHandler3 == null)
				{
					flag5 = false;
				}
				else
				{
					CryptDictionary stringDictionary = cryptHandler3.StringDictionary;
					bool? flag2 = (stringDictionary != null) ? new bool?(stringDictionary.IsIdentity) : default(bool?);
					bool flag3 = true;
					flag5 = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (!flag5)
				{
					CryptHandler cryptHandler4 = this.cryptHandler;
					bool flag6;
					if (cryptHandler4 == null)
					{
						flag6 = false;
					}
					else
					{
						CryptDictionary stringDictionary2 = cryptHandler4.StringDictionary;
						CryptDictionary.Method? method = (stringDictionary2 != null) ? new CryptDictionary.Method?(stringDictionary2.Name) : default(CryptDictionary.Method?);
						CryptDictionary.Method method2 = CryptDictionary.Method.None;
						flag6 = (method.GetValueOrDefault() == method2 & method != null);
					}
					if (!flag6)
					{
						byte[] data = OtherEncodings.StringAsLatin1Bytes(stringToken.Data);
						return EncryptionHandler.GetStringTokenFromDecryptedData(this.DecryptData(data, reference));
					}
				}
				return token;
			}
			HexToken hexToken = token as HexToken;
			if (hexToken == null)
			{
				DictionaryToken dictionaryToken2 = token as DictionaryToken;
				if (dictionaryToken2 == null)
				{
					ArrayToken arrayToken = token as ArrayToken;
					if (arrayToken != null)
					{
						IToken[] array2 = new IToken[arrayToken.Length];
						for (int i = 0; i < arrayToken.Length; i++)
						{
							array2[i] = this.DecryptInternal(reference, arrayToken.Data[i]);
						}
						token = new ArrayToken(array2);
					}
				}
				else
				{
					IToken token2;
					if (dictionaryToken2.TryGet(NameToken.Cf, ref token2))
					{
						return token;
					}
					NameToken nameToken2;
					bool flag7 = dictionaryToken2.TryGet<NameToken>(NameToken.Type, ref nameToken2) && (nameToken2.Equals(NameToken.Sig) || nameToken2.Equals(NameToken.DocTimeStamp));
					foreach (KeyValuePair<string, IToken> keyValuePair in dictionaryToken2.Data)
					{
						if ((!flag7 || !(keyValuePair.Key == NameToken.Contents.Data)) && (keyValuePair.Value is StringToken || keyValuePair.Value is ArrayToken || keyValuePair.Value is DictionaryToken || keyValuePair.Value is HexToken))
						{
							IToken token3 = this.DecryptInternal(reference, keyValuePair.Value);
							dictionaryToken2 = dictionaryToken2.With(keyValuePair.Key, token3);
						}
					}
					token = dictionaryToken2;
				}
			}
			else
			{
				byte[] data2 = Enumerable.ToArray<byte>(hexToken.Bytes);
				token = new HexToken(Hex.GetString(this.DecryptData(data2, reference)).ToCharArray());
			}
			return token;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00038418 File Offset: 0x00036618
		private static StringToken GetStringTokenFromDecryptedData(byte[] data)
		{
			if (data.Length >= 2)
			{
				if (data[0] == 254 && data[1] == 255)
				{
					return new StringToken(Encoding.BigEndianUnicode.GetString(data).Substring(1), 2);
				}
				if (data[0] == 255 && data[1] == 254)
				{
					return new StringToken(Encoding.Unicode.GetString(data).Substring(1), 1);
				}
			}
			return new StringToken(OtherEncodings.BytesAsLatin1String(data), 0);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00038490 File Offset: 0x00036690
		private byte[] DecryptData(byte[] data, IndirectReference reference)
		{
			if (this.useAes && this.encryptionKey.Length == 32)
			{
				return AesEncryptionHelper.Decrypt(data, this.encryptionKey);
			}
			byte[] objectKey = this.GetObjectKey(reference);
			if (this.useAes)
			{
				return AesEncryptionHelper.Decrypt(data, objectKey);
			}
			return RC4.Encrypt(objectKey, data);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x000384E0 File Offset: 0x000366E0
		private byte[] GetObjectKey(IndirectReference reference)
		{
			byte[] array = new byte[this.encryptionKey.Length + 5 + (this.useAes ? 4 : 0)];
			Array.Copy(this.encryptionKey, array, this.encryptionKey.Length);
			array[this.encryptionKey.Length] = (byte)reference.ObjectNumber;
			array[this.encryptionKey.Length + 1] = (byte)(reference.ObjectNumber >> 8);
			array[this.encryptionKey.Length + 2] = (byte)(reference.ObjectNumber >> 16);
			array[this.encryptionKey.Length + 3] = (byte)reference.Generation;
			array[this.encryptionKey.Length + 4] = (byte)(reference.Generation >> 8);
			if (this.useAes)
			{
				array[this.encryptionKey.Length + 5] = 115;
				array[this.encryptionKey.Length + 6] = 65;
				array[this.encryptionKey.Length + 7] = 108;
				array[this.encryptionKey.Length + 8] = 84;
			}
			byte[] result;
			using (MD5 md = MD5.Create())
			{
				md.ComputeHash(array);
				int num = Math.Min(16, this.encryptionKey.Length + 5);
				byte[] array2 = new byte[num];
				Array.Copy(md.Hash, array2, num);
				result = array2;
			}
			return result;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0003861C File Offset: 0x0003681C
		private static byte[] CalculateEncryptionKey(byte[] password, EncryptionDictionary encryptionDictionary, int length, byte[] documentId, bool isUserPassword)
		{
			if (encryptionDictionary.Revision >= 2 && encryptionDictionary.Revision <= 4)
			{
				return EncryptionHandler.CalculateKeyRevisions2To4(password, encryptionDictionary, length, documentId);
			}
			if (encryptionDictionary.Revision <= 6)
			{
				return EncryptionHandler.CalculateKeyRevisions5And6(password, encryptionDictionary, isUserPassword);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(43, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PDF encrypted with unrecognized revision: ");
			defaultInterpolatedStringHandler.AppendFormatted<EncryptionDictionary>(encryptionDictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00038690 File Offset: 0x00036890
		private static byte[] CalculateKeyRevisions2To4(byte[] password, EncryptionDictionary encryptionDictionary, int length, byte[] documentId)
		{
			byte[] paddedPassword = EncryptionHandler.GetPaddedPassword(password);
			int revision = encryptionDictionary.Revision;
			byte[] result;
			using (MD5 md = MD5.Create())
			{
				EncryptionHandler.UpdateMd5(md, paddedPassword);
				EncryptionHandler.UpdateMd5(md, encryptionDictionary.OwnerBytes);
				uint num = (uint)encryptionDictionary.UserAccessPermissions;
				EncryptionHandler.UpdateMd5(md, new byte[]
				{
					(byte)num
				});
				EncryptionHandler.UpdateMd5(md, new byte[]
				{
					(byte)(num >> 8)
				});
				EncryptionHandler.UpdateMd5(md, new byte[]
				{
					(byte)(num >> 16)
				});
				EncryptionHandler.UpdateMd5(md, new byte[]
				{
					(byte)(num >> 24)
				});
				EncryptionHandler.UpdateMd5(md, documentId);
				if (revision >= 4 && !encryptionDictionary.EncryptMetadata)
				{
					EncryptionHandler.UpdateMd5(md, new byte[]
					{
						byte.MaxValue,
						byte.MaxValue,
						byte.MaxValue,
						byte.MaxValue
					});
				}
				if (revision == 3 || revision == 4)
				{
					md.TransformFinalBlock(EmptyArray<byte>.Instance, 0, 0);
					byte[] array = md.Hash;
					using (MD5 md2 = MD5.Create())
					{
						for (int i = 0; i < 50; i++)
						{
							array = md2.ComputeHash(Enumerable.ToArray<byte>(Enumerable.Take<byte>(array, length)));
						}
					}
					byte[] array2 = new byte[length];
					Array.Copy(array, array2, length);
					result = array2;
				}
				else
				{
					md.TransformFinalBlock(EmptyArray<byte>.Instance, 0, 0);
					byte[] array3 = new byte[length];
					Array.Copy(md.Hash, array3, length);
					result = array3;
				}
			}
			return result;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00038824 File Offset: 0x00036A24
		private static byte[] CalculateKeyRevisions5And6(byte[] password, EncryptionDictionary encryptionDictionary, bool isUserPassword)
		{
			password = EncryptionHandler.TruncatePasswordTo127Bytes(password);
			byte[] array2;
			byte[] array3;
			if (!isUserPassword)
			{
				byte[] array = new byte[8];
				Array.Copy(encryptionDictionary.OwnerBytes, 40, array, 0, array.Length);
				if (encryptionDictionary.Revision == 6)
				{
					array2 = EncryptionHandler.ComputeStupidIsoHash(password, array, encryptionDictionary.UserBytes);
				}
				else
				{
					array2 = EncryptionHandler.ComputeSha256Hash(password, array, encryptionDictionary.UserBytes);
				}
				array3 = encryptionDictionary.OwnerEncryptionBytes;
			}
			else
			{
				byte[] array4 = new byte[8];
				Array.Copy(encryptionDictionary.UserBytes, 40, array4, 0, 8);
				if (encryptionDictionary.Revision == 6)
				{
					array2 = EncryptionHandler.ComputeStupidIsoHash(password, array4, null);
				}
				else
				{
					array2 = EncryptionHandler.ComputeSha256Hash(password, array4, null);
				}
				array3 = encryptionDictionary.UserEncryptionBytes;
			}
			byte[] array5 = new byte[16];
			byte[] result;
			using (Aes aes = Aes.Create("AesManaged"))
			{
				aes.Key = array2;
				aes.IV = array5;
				aes.Mode = 1;
				aes.Padding = 1;
				using (MemoryStream memoryStream = new MemoryStream(array3))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(array2, array5), 0))
						{
							cryptoStream.CopyTo(memoryStream2);
							result = memoryStream2.ToArray();
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00038994 File Offset: 0x00036B94
		private static byte[] ComputeSha256Hash(byte[] input1, byte[] input2, byte[] input3 = null)
		{
			byte[] hash;
			using (SHA256 sha = SHA256.Create())
			{
				sha.TransformBlock(input1, 0, input1.Length, null, 0);
				sha.TransformBlock(input2, 0, input2.Length, null, 0);
				if (input3 != null)
				{
					sha.TransformFinalBlock(input3, 0, input3.Length);
				}
				else
				{
					sha.TransformFinalBlock(EmptyArray<byte>.Instance, 0, 0);
				}
				hash = sha.Hash;
			}
			return hash;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00038A08 File Offset: 0x00036C08
		private static void UpdateMd5(MD5 md5, byte[] data)
		{
			md5.TransformBlock(data, 0, data.Length, null, 0);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00038A18 File Offset: 0x00036C18
		private static byte[] GetPaddedPassword(byte[] password)
		{
			if (password == null || password.Length == 0)
			{
				return EncryptionHandler.PaddingBytes;
			}
			byte[] array = new byte[32];
			int num = (password.Length <= 32) ? password.Length : 32;
			int num2 = 32 - num;
			Array.ConstrainedCopy(password, 0, array, 0, num);
			if (num2 > 0)
			{
				Array.ConstrainedCopy(EncryptionHandler.PaddingBytes, 0, array, num, num2);
			}
			return array;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00038A6C File Offset: 0x00036C6C
		private static byte[] TruncatePasswordTo127Bytes(byte[] password)
		{
			if (password.Length <= 127)
			{
				return password;
			}
			byte[] array = new byte[127];
			Array.Copy(password, array, 127);
			return array;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00038A94 File Offset: 0x00036C94
		private static byte[] ComputeStupidIsoHash(byte[] password, byte[] salt, byte[] vector)
		{
			if (vector == null)
			{
				vector = EmptyArray<byte>.Instance;
			}
			else
			{
				if (vector.Length != 0 && vector.Length < 48)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(98, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Vector for revision 6 owner password check (/U) is the wrong length, expected 48 bytes got ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(vector.Length);
					defaultInterpolatedStringHandler.AppendLiteral(" bytes.");
					throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (vector.Length > 48)
				{
					byte[] array = new byte[48];
					Array.Copy(vector, array, array.Length);
					vector = array;
				}
			}
			password = EncryptionHandler.TruncatePasswordTo127Bytes(password);
			byte[] array2;
			using (SHA256 sha = SHA256.Create())
			{
				sha.TransformBlock(password, 0, password.Length, null, 0);
				sha.TransformBlock(salt, 0, salt.Length, null, 0);
				sha.TransformBlock(vector, 0, vector.Length, null, 0);
				sha.TransformFinalBlock(EmptyArray<byte>.Instance, 0, 0);
				array2 = sha.Hash;
			}
			byte[] array3 = new byte[16];
			Array.Copy(array2, array3, 16);
			byte[] array4 = new byte[16];
			Array.Copy(array2, 16, array4, 0, array4.Length);
			byte[] array5 = null;
			int num = 0;
			while (num < 64 || num < (int)(array5[array5.Length - 1] + 32))
			{
				byte[] array6 = new byte[64 * (password.Length + array2.Length + vector.Length)];
				int num2 = 0;
				for (int i = 0; i < 64; i++)
				{
					Array.Copy(password, 0, array6, num2, password.Length);
					num2 += password.Length;
					Array.Copy(array2, 0, array6, num2, array2.Length);
					num2 += array2.Length;
					if (vector.Length != 0)
					{
						Array.Copy(vector, 0, array6, num2, vector.Length);
						num2 += vector.Length;
					}
				}
				using (Aes aes = Aes.Create())
				{
					aes.Key = array3;
					aes.IV = array4;
					aes.Mode = 1;
					aes.Padding = 1;
					using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
					{
						array5 = cryptoTransform.TransformFinalBlock(array6, 0, array6.Length);
					}
				}
				long num3 = Enumerable.Sum<byte>(Enumerable.Take<byte>(array5, 16), (byte v) => (long)((ulong)v)) % 3L;
				long num4 = num3;
				if (num4 <= 2L)
				{
					HashAlgorithm hashAlgorithm;
					switch ((uint)num4)
					{
					case 0U:
						hashAlgorithm = SHA256.Create();
						break;
					case 1U:
						hashAlgorithm = SHA384.Create();
						break;
					case 2U:
						hashAlgorithm = SHA512.Create();
						break;
					default:
						goto IL_233;
					}
					array2 = hashAlgorithm.ComputeHash(array5);
					Array.Copy(array2, array3, 16);
					Array.Copy(array2, 16, array4, 0, 16);
					num++;
					continue;
				}
				IL_233:
				throw new PdfDocumentEncryptedException("Invalid remainder from summing first sixteen bytes of this round's hash.");
			}
			if (array2.Length > 32)
			{
				byte[] array7 = new byte[32];
				Array.Copy(array2, array7, array7.Length);
				return array7;
			}
			return array2;
		}

		// Token: 0x040005AB RID: 1451
		private static readonly byte[] PaddingBytes = new byte[]
		{
			40,
			191,
			78,
			94,
			78,
			117,
			138,
			65,
			100,
			0,
			78,
			86,
			byte.MaxValue,
			250,
			1,
			8,
			46,
			46,
			0,
			182,
			208,
			104,
			62,
			128,
			47,
			12,
			169,
			254,
			100,
			83,
			105,
			122
		};

		// Token: 0x040005AC RID: 1452
		private readonly HashSet<IndirectReference> previouslyDecrypted = new HashSet<IndirectReference>();

		// Token: 0x040005AD RID: 1453
		[CanBeNull]
		private readonly EncryptionDictionary encryptionDictionary;

		// Token: 0x040005AE RID: 1454
		[CanBeNull]
		private readonly CryptHandler cryptHandler;

		// Token: 0x040005AF RID: 1455
		private readonly byte[] encryptionKey;

		// Token: 0x040005B0 RID: 1456
		private readonly bool useAes;
	}
}
