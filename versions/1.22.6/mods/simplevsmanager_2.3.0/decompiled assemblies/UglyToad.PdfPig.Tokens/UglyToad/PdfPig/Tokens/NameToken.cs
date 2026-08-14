using System;
using System.Collections.Concurrent;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x0200000C RID: 12
	public class NameToken : IDataToken<string>, IToken, IEquatable<IToken>
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public string Data { get; }

		// Token: 0x06000036 RID: 54 RVA: 0x00002ABC File Offset: 0x00000CBC
		private NameToken(string text)
		{
			NameToken.NameMap[text] = this;
			this.Data = text;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public static NameToken Create(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			NameToken result;
			if (!NameToken.NameMap.TryGetValue(name, ref result))
			{
				return new NameToken(name);
			}
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B0A File Offset: 0x00000D0A
		public override bool Equals(object obj)
		{
			return this.Equals(obj as NameToken);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B18 File Offset: 0x00000D18
		public bool Equals(NameToken other)
		{
			return string.Equals(this.Data, (other != null) ? other.Data : null);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B31 File Offset: 0x00000D31
		public bool Equals(IToken obj)
		{
			return this.Equals(obj as NameToken);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B3F File Offset: 0x00000D3F
		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002B4C File Offset: 0x00000D4C
		public static implicit operator string(NameToken name)
		{
			if (name == null)
			{
				return null;
			}
			return name.Data;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B59 File Offset: 0x00000D59
		public static bool operator ==(NameToken name1, NameToken name2)
		{
			return name1 == name2 || (name1 != null && name1.Equals(name2));
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B6D File Offset: 0x00000D6D
		public static bool operator !=(NameToken name1, NameToken name2)
		{
			return !(name1 == name2);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B79 File Offset: 0x00000D79
		public override string ToString()
		{
			return "/" + this.Data;
		}

		// Token: 0x0400000E RID: 14
		private static readonly ConcurrentDictionary<string, NameToken> NameMap = new ConcurrentDictionary<string, NameToken>();

		// Token: 0x0400000F RID: 15
		public static readonly NameToken A = new NameToken("A");

		// Token: 0x04000010 RID: 16
		public static readonly NameToken Aa = new NameToken("AA");

		// Token: 0x04000011 RID: 17
		public static readonly NameToken AcroForm = new NameToken("AcroForm");

		// Token: 0x04000012 RID: 18
		public static readonly NameToken ActualText = new NameToken("ActualText");

		// Token: 0x04000013 RID: 19
		public static readonly NameToken AdbePkcs7Detached = new NameToken("adbe.pkcs7.detached");

		// Token: 0x04000014 RID: 20
		public static readonly NameToken AdbePkcs7Sha1 = new NameToken("adbe.pkcs7.sha1");

		// Token: 0x04000015 RID: 21
		public static readonly NameToken AdbeX509RsaSha1 = new NameToken("adbe.x509.rsa_sha1");

		// Token: 0x04000016 RID: 22
		public static readonly NameToken AdobePpklite = new NameToken("Adobe.PPKLite");

		// Token: 0x04000017 RID: 23
		public static readonly NameToken Aesv2 = new NameToken("AESV2");

		// Token: 0x04000018 RID: 24
		public static readonly NameToken Aesv3 = new NameToken("AESV3");

		// Token: 0x04000019 RID: 25
		public static readonly NameToken After = new NameToken("After");

		// Token: 0x0400001A RID: 26
		public static readonly NameToken Ais = new NameToken("AIS");

		// Token: 0x0400001B RID: 27
		public static readonly NameToken Alt = new NameToken("Alt");

		// Token: 0x0400001C RID: 28
		public static readonly NameToken Alpha = new NameToken("Alpha");

		// Token: 0x0400001D RID: 29
		public static readonly NameToken Alternate = new NameToken("Alternate");

		// Token: 0x0400001E RID: 30
		public static readonly NameToken Annotation3D = new NameToken("3D");

		// Token: 0x0400001F RID: 31
		public static readonly NameToken Annot = new NameToken("Annot");

		// Token: 0x04000020 RID: 32
		public static readonly NameToken Annots = new NameToken("Annots");

		// Token: 0x04000021 RID: 33
		public static readonly NameToken AntiAlias = new NameToken("AntiAlias");

		// Token: 0x04000022 RID: 34
		public static readonly NameToken Ap = new NameToken("AP");

		// Token: 0x04000023 RID: 35
		public static readonly NameToken ApRef = new NameToken("APRef");

		// Token: 0x04000024 RID: 36
		public static readonly NameToken App = new NameToken("App");

		// Token: 0x04000025 RID: 37
		public static readonly NameToken ArtBox = new NameToken("ArtBox");

		// Token: 0x04000026 RID: 38
		public static readonly NameToken Artifact = new NameToken("Artifact");

		// Token: 0x04000027 RID: 39
		public static readonly NameToken As = new NameToken("AS");

		// Token: 0x04000028 RID: 40
		public static readonly NameToken Ascent = new NameToken("Ascent");

		// Token: 0x04000029 RID: 41
		public static readonly NameToken AsciiHexDecode = new NameToken("ASCIIHexDecode");

		// Token: 0x0400002A RID: 42
		public static readonly NameToken AsciiHexDecodeAbbreviation = new NameToken("AHx");

		// Token: 0x0400002B RID: 43
		public static readonly NameToken Ascii85Decode = new NameToken("ASCII85Decode");

		// Token: 0x0400002C RID: 44
		public static readonly NameToken Ascii85DecodeAbbreviation = new NameToken("A85");

		// Token: 0x0400002D RID: 45
		public static readonly NameToken Attached = new NameToken("Attached");

		// Token: 0x0400002E RID: 46
		public static readonly NameToken AuthEvent = new NameToken("AuthEvent");

		// Token: 0x0400002F RID: 47
		public static readonly NameToken Author = new NameToken("Author");

		// Token: 0x04000030 RID: 48
		public static readonly NameToken AvgWidth = new NameToken("AvgWidth");

		// Token: 0x04000031 RID: 49
		public static readonly NameToken B = new NameToken("B");

		// Token: 0x04000032 RID: 50
		public static readonly NameToken Background = new NameToken("Background");

		// Token: 0x04000033 RID: 51
		public static readonly NameToken BaseEncoding = new NameToken("BaseEncoding");

		// Token: 0x04000034 RID: 52
		public static readonly NameToken BaseFont = new NameToken("BaseFont");

		// Token: 0x04000035 RID: 53
		public static readonly NameToken BaseState = new NameToken("BaseState");

		// Token: 0x04000036 RID: 54
		public static readonly NameToken Bbox = new NameToken("BBox");

		// Token: 0x04000037 RID: 55
		public static readonly NameToken Bc = new NameToken("BC");

		// Token: 0x04000038 RID: 56
		public static readonly NameToken Be = new NameToken("BE");

		// Token: 0x04000039 RID: 57
		public static readonly NameToken Before = new NameToken("Before");

		// Token: 0x0400003A RID: 58
		public static readonly NameToken Bg = new NameToken("BG");

		// Token: 0x0400003B RID: 59
		public static readonly NameToken BitsPerComponent = new NameToken("BitsPerComponent");

		// Token: 0x0400003C RID: 60
		public static readonly NameToken BitsPerCoordinate = new NameToken("BitsPerCoordinate");

		// Token: 0x0400003D RID: 61
		public static readonly NameToken BitsPerFlag = new NameToken("BitsPerFlag");

		// Token: 0x0400003E RID: 62
		public static readonly NameToken BitsPerSample = new NameToken("BitsPerSample");

		// Token: 0x0400003F RID: 63
		public static readonly NameToken BlackIs1 = new NameToken("BlackIs1");

		// Token: 0x04000040 RID: 64
		public static readonly NameToken BlackPoint = new NameToken("BlackPoint");

		// Token: 0x04000041 RID: 65
		public static readonly NameToken BleedBox = new NameToken("BleedBox");

		// Token: 0x04000042 RID: 66
		public static readonly NameToken Bm = new NameToken("BM");

		// Token: 0x04000043 RID: 67
		public static readonly NameToken Border = new NameToken("Border");

		// Token: 0x04000044 RID: 68
		public static readonly NameToken Bottom = new NameToken("Bottom");

		// Token: 0x04000045 RID: 69
		public static readonly NameToken Bounds = new NameToken("Bounds");

		// Token: 0x04000046 RID: 70
		public static readonly NameToken Bpc = new NameToken("BPC");

		// Token: 0x04000047 RID: 71
		public static readonly NameToken Bs = new NameToken("BS");

		// Token: 0x04000048 RID: 72
		public static readonly NameToken Btn = new NameToken("Btn");

		// Token: 0x04000049 RID: 73
		public static readonly NameToken Byterange = new NameToken("ByteRange");

		// Token: 0x0400004A RID: 74
		public static readonly NameToken C = new NameToken("C");

		// Token: 0x0400004B RID: 75
		public static readonly NameToken C0 = new NameToken("C0");

		// Token: 0x0400004C RID: 76
		public static readonly NameToken C1 = new NameToken("C1");

		// Token: 0x0400004D RID: 77
		public static readonly NameToken Ca = new NameToken("CA");

		// Token: 0x0400004E RID: 78
		public static readonly NameToken CaNs = new NameToken("ca");

		// Token: 0x0400004F RID: 79
		public static readonly NameToken Calgray = new NameToken("CalGray");

		// Token: 0x04000050 RID: 80
		public static readonly NameToken Calrgb = new NameToken("CalRGB");

		// Token: 0x04000051 RID: 81
		public static readonly NameToken Cap = new NameToken("Cap");

		// Token: 0x04000052 RID: 82
		public static readonly NameToken CapHeight = new NameToken("CapHeight");

		// Token: 0x04000053 RID: 83
		public static readonly NameToken Caret = new NameToken("Caret");

		// Token: 0x04000054 RID: 84
		public static readonly NameToken Catalog = new NameToken("Catalog");

		// Token: 0x04000055 RID: 85
		public static readonly NameToken CcittfaxDecode = new NameToken("CCITTFaxDecode");

		// Token: 0x04000056 RID: 86
		public static readonly NameToken CcittfaxDecodeAbbreviation = new NameToken("CCF");

		// Token: 0x04000057 RID: 87
		public static readonly NameToken CenterWindow = new NameToken("CenterWindow");

		// Token: 0x04000058 RID: 88
		public static readonly NameToken Cf = new NameToken("CF");

		// Token: 0x04000059 RID: 89
		public static readonly NameToken Cfm = new NameToken("CFM");

		// Token: 0x0400005A RID: 90
		public static readonly NameToken Ch = new NameToken("Ch");

		// Token: 0x0400005B RID: 91
		public static readonly NameToken CharProcs = new NameToken("CharProcs");

		// Token: 0x0400005C RID: 92
		public static readonly NameToken CharSet = new NameToken("CharSet");

		// Token: 0x0400005D RID: 93
		public static readonly NameToken CiciSignit = new NameToken("CICI.SignIt");

		// Token: 0x0400005E RID: 94
		public static readonly NameToken CidFontType0 = new NameToken("CIDFontType0");

		// Token: 0x0400005F RID: 95
		public static readonly NameToken CidFontType0C = new NameToken("CIDFontType0C");

		// Token: 0x04000060 RID: 96
		public static readonly NameToken CidFontType2 = new NameToken("CIDFontType2");

		// Token: 0x04000061 RID: 97
		public static readonly NameToken CidInit = new NameToken("CIDInit");

		// Token: 0x04000062 RID: 98
		public static readonly NameToken CidToGidMap = new NameToken("CIDToGIDMap");

		// Token: 0x04000063 RID: 99
		public static readonly NameToken CidSet = new NameToken("CIDSet");

		// Token: 0x04000064 RID: 100
		public static readonly NameToken CidSystemInfo = new NameToken("CIDSystemInfo");

		// Token: 0x04000065 RID: 101
		public static readonly NameToken Circle = new NameToken("Circle");

		// Token: 0x04000066 RID: 102
		public static readonly NameToken Cl = new NameToken("CL");

		// Token: 0x04000067 RID: 103
		public static readonly NameToken ClrF = new NameToken("ClrF");

		// Token: 0x04000068 RID: 104
		public static readonly NameToken ClrFf = new NameToken("ClrFf");

		// Token: 0x04000069 RID: 105
		public static readonly NameToken Cmap = new NameToken("CMap");

		// Token: 0x0400006A RID: 106
		public static readonly NameToken Cmapname = new NameToken("CMapName");

		// Token: 0x0400006B RID: 107
		public static readonly NameToken CmapType = new NameToken("CMapType");

		// Token: 0x0400006C RID: 108
		public static readonly NameToken Cmyk = new NameToken("CMYK");

		// Token: 0x0400006D RID: 109
		public static readonly NameToken Co = new NameToken("CO");

		// Token: 0x0400006E RID: 110
		public static readonly NameToken ColorBurn = new NameToken("ColorBurn");

		// Token: 0x0400006F RID: 111
		public static readonly NameToken ColorDodge = new NameToken("ColorDodge");

		// Token: 0x04000070 RID: 112
		public static readonly NameToken Colorants = new NameToken("Colorants");

		// Token: 0x04000071 RID: 113
		public static readonly NameToken Colors = new NameToken("Colors");

		// Token: 0x04000072 RID: 114
		public static readonly NameToken ColorSpace = new NameToken("ColorSpace");

		// Token: 0x04000073 RID: 115
		public static readonly NameToken Columns = new NameToken("Columns");

		// Token: 0x04000074 RID: 116
		public static readonly NameToken Compatible = new NameToken("Compatible");

		// Token: 0x04000075 RID: 117
		public static readonly NameToken Components = new NameToken("Components");

		// Token: 0x04000076 RID: 118
		public static readonly NameToken ContactInfo = new NameToken("ContactInfo");

		// Token: 0x04000077 RID: 119
		public static readonly NameToken Contents = new NameToken("Contents");

		// Token: 0x04000078 RID: 120
		public static readonly NameToken Coords = new NameToken("Coords");

		// Token: 0x04000079 RID: 121
		public static readonly NameToken Count = new NameToken("Count");

		// Token: 0x0400007A RID: 122
		public static readonly NameToken Cp = new NameToken("CP");

		// Token: 0x0400007B RID: 123
		public static readonly NameToken CreationDate = new NameToken("CreationDate");

		// Token: 0x0400007C RID: 124
		public static readonly NameToken Creator = new NameToken("Creator");

		// Token: 0x0400007D RID: 125
		public static readonly NameToken CropBox = new NameToken("CropBox");

		// Token: 0x0400007E RID: 126
		public static readonly NameToken Crypt = new NameToken("Crypt");

		// Token: 0x0400007F RID: 127
		public static readonly NameToken CryptAlgorithm = new NameToken("CryptAlgorithm");

		// Token: 0x04000080 RID: 128
		public static readonly NameToken CryptFilter = new NameToken("CryptFilter");

		// Token: 0x04000081 RID: 129
		public static readonly NameToken Cs = new NameToken("CS");

		// Token: 0x04000082 RID: 130
		public static readonly NameToken D = new NameToken("D");

		// Token: 0x04000083 RID: 131
		public static readonly NameToken Da = new NameToken("DA");

		// Token: 0x04000084 RID: 132
		public static readonly NameToken Darken = new NameToken("Darken");

		// Token: 0x04000085 RID: 133
		public static readonly NameToken Date = new NameToken("Date");

		// Token: 0x04000086 RID: 134
		public static readonly NameToken DctDecode = new NameToken("DCTDecode");

		// Token: 0x04000087 RID: 135
		public static readonly NameToken DctDecodeAbbreviation = new NameToken("DCT");

		// Token: 0x04000088 RID: 136
		public static readonly NameToken Decode = new NameToken("Decode");

		// Token: 0x04000089 RID: 137
		public static readonly NameToken DecodeParms = new NameToken("DecodeParms");

		// Token: 0x0400008A RID: 138
		public static readonly NameToken Default = new NameToken("default");

		// Token: 0x0400008B RID: 139
		public static readonly NameToken DefaultCmyk = new NameToken("DefaultCMYK");

		// Token: 0x0400008C RID: 140
		public static readonly NameToken DefaultGray = new NameToken("DefaultGray");

		// Token: 0x0400008D RID: 141
		public static readonly NameToken DefaultRgb = new NameToken("DefaultRGB");

		// Token: 0x0400008E RID: 142
		public static readonly NameToken Desc = new NameToken("Desc");

		// Token: 0x0400008F RID: 143
		public static readonly NameToken DescendantFonts = new NameToken("DescendantFonts");

		// Token: 0x04000090 RID: 144
		public static readonly NameToken Descent = new NameToken("Descent");

		// Token: 0x04000091 RID: 145
		public static readonly NameToken Dest = new NameToken("Dest");

		// Token: 0x04000092 RID: 146
		public static readonly NameToken DestOutputProfile = new NameToken("DestOutputProfile");

		// Token: 0x04000093 RID: 147
		public static readonly NameToken Dests = new NameToken("Dests");

		// Token: 0x04000094 RID: 148
		public static readonly NameToken Devicecmyk = new NameToken("DeviceCMYK");

		// Token: 0x04000095 RID: 149
		public static readonly NameToken Devicegray = new NameToken("DeviceGray");

		// Token: 0x04000096 RID: 150
		public static readonly NameToken Devicen = new NameToken("DeviceN");

		// Token: 0x04000097 RID: 151
		public static readonly NameToken Devicergb = new NameToken("DeviceRGB");

		// Token: 0x04000098 RID: 152
		public static readonly NameToken Di = new NameToken("Di");

		// Token: 0x04000099 RID: 153
		public static readonly NameToken Difference = new NameToken("Difference");

		// Token: 0x0400009A RID: 154
		public static readonly NameToken Differences = new NameToken("Differences");

		// Token: 0x0400009B RID: 155
		public static readonly NameToken DigestMethod = new NameToken("DigestMethod");

		// Token: 0x0400009C RID: 156
		public static readonly NameToken DigestRipemd160 = new NameToken("RIPEMD160");

		// Token: 0x0400009D RID: 157
		public static readonly NameToken DigestSha1 = new NameToken("SHA1");

		// Token: 0x0400009E RID: 158
		public static readonly NameToken DigestSha256 = new NameToken("SHA256");

		// Token: 0x0400009F RID: 159
		public static readonly NameToken DigestSha384 = new NameToken("SHA384");

		// Token: 0x040000A0 RID: 160
		public static readonly NameToken DigestSha512 = new NameToken("SHA512");

		// Token: 0x040000A1 RID: 161
		public static readonly NameToken Direction = new NameToken("Direction");

		// Token: 0x040000A2 RID: 162
		public static readonly NameToken DisplayDocTitle = new NameToken("DisplayDocTitle");

		// Token: 0x040000A3 RID: 163
		public static readonly NameToken Dl = new NameToken("DL");

		// Token: 0x040000A4 RID: 164
		public static readonly NameToken Dm = new NameToken("Dm");

		// Token: 0x040000A5 RID: 165
		public static readonly NameToken Doc = new NameToken("Doc");

		// Token: 0x040000A6 RID: 166
		public static readonly NameToken DocOpen = new NameToken("DocOpen");

		// Token: 0x040000A7 RID: 167
		public static readonly NameToken DocChecksum = new NameToken("DocChecksum");

		// Token: 0x040000A8 RID: 168
		public static readonly NameToken DocTimeStamp = new NameToken("DocTimeStamp");

		// Token: 0x040000A9 RID: 169
		public static readonly NameToken Docmdp = new NameToken("DocMDP");

		// Token: 0x040000AA RID: 170
		public static readonly NameToken Domain = new NameToken("Domain");

		// Token: 0x040000AB RID: 171
		public static readonly NameToken Dos = new NameToken("DOS");

		// Token: 0x040000AC RID: 172
		public static readonly NameToken Dp = new NameToken("DP");

		// Token: 0x040000AD RID: 173
		public static readonly NameToken Dr = new NameToken("DR");

		// Token: 0x040000AE RID: 174
		public static readonly NameToken Ds = new NameToken("DS");

		// Token: 0x040000AF RID: 175
		public static readonly NameToken Duplex = new NameToken("Duplex");

		// Token: 0x040000B0 RID: 176
		public static readonly NameToken Dur = new NameToken("Dur");

		// Token: 0x040000B1 RID: 177
		public static readonly NameToken Dv = new NameToken("DV");

		// Token: 0x040000B2 RID: 178
		public static readonly NameToken Dw = new NameToken("DW");

		// Token: 0x040000B3 RID: 179
		public static readonly NameToken Dw2 = new NameToken("DW2");

		// Token: 0x040000B4 RID: 180
		public static readonly NameToken E = new NameToken("E");

		// Token: 0x040000B5 RID: 181
		public static readonly NameToken EarlyChange = new NameToken("EarlyChange");

		// Token: 0x040000B6 RID: 182
		public static readonly NameToken Ef = new NameToken("EF");

		// Token: 0x040000B7 RID: 183
		public static readonly NameToken EfOpen = new NameToken("EFOpen");

		// Token: 0x040000B8 RID: 184
		public static readonly NameToken EmbeddedFdfs = new NameToken("EmbeddedFDFs");

		// Token: 0x040000B9 RID: 185
		public static readonly NameToken EmbeddedFiles = new NameToken("EmbeddedFiles");

		// Token: 0x040000BA RID: 186
		public static readonly NameToken Empty = new NameToken("");

		// Token: 0x040000BB RID: 187
		public static readonly NameToken Encode = new NameToken("Encode");

		// Token: 0x040000BC RID: 188
		public static readonly NameToken EncodedByteAlign = new NameToken("EncodedByteAlign");

		// Token: 0x040000BD RID: 189
		public static readonly NameToken Encoding = new NameToken("Encoding");

		// Token: 0x040000BE RID: 190
		public static readonly NameToken Encoding90MsRksjH = new NameToken("90ms-RKSJ-H");

		// Token: 0x040000BF RID: 191
		public static readonly NameToken Encoding90MsRksjV = new NameToken("90ms-RKSJ-V");

		// Token: 0x040000C0 RID: 192
		public static readonly NameToken EncodingEtenB5H = new NameToken("ETen-B5-H");

		// Token: 0x040000C1 RID: 193
		public static readonly NameToken EncodingEtenB5V = new NameToken("ETen-B5-V");

		// Token: 0x040000C2 RID: 194
		public static readonly NameToken Encrypt = new NameToken("Encrypt");

		// Token: 0x040000C3 RID: 195
		public static readonly NameToken EncryptMetaData = new NameToken("EncryptMetadata");

		// Token: 0x040000C4 RID: 196
		public static readonly NameToken EndOfLine = new NameToken("EndOfLine");

		// Token: 0x040000C5 RID: 197
		public static readonly NameToken EntrustPpkef = new NameToken("Entrust.PPKEF");

		// Token: 0x040000C6 RID: 198
		public static readonly NameToken Exclusion = new NameToken("Exclusion");

		// Token: 0x040000C7 RID: 199
		public static readonly NameToken ExtGState = new NameToken("ExtGState");

		// Token: 0x040000C8 RID: 200
		public static readonly NameToken Extend = new NameToken("Extend");

		// Token: 0x040000C9 RID: 201
		public static readonly NameToken Extends = new NameToken("Extends");

		// Token: 0x040000CA RID: 202
		public static readonly NameToken F = new NameToken("F");

		// Token: 0x040000CB RID: 203
		public static readonly NameToken FDecodeParms = new NameToken("FDecodeParms");

		// Token: 0x040000CC RID: 204
		public static readonly NameToken FFilter = new NameToken("FFilter");

		// Token: 0x040000CD RID: 205
		public static readonly NameToken Fb = new NameToken("FB");

		// Token: 0x040000CE RID: 206
		public static readonly NameToken Fdf = new NameToken("FDF");

		// Token: 0x040000CF RID: 207
		public static readonly NameToken Ff = new NameToken("Ff");

		// Token: 0x040000D0 RID: 208
		public static readonly NameToken Fields = new NameToken("Fields");

		// Token: 0x040000D1 RID: 209
		public static readonly NameToken FileAttachment = new NameToken("FileAttachment");

		// Token: 0x040000D2 RID: 210
		public static readonly NameToken Filespec = new NameToken("Filespec");

		// Token: 0x040000D3 RID: 211
		public static readonly NameToken Filter = new NameToken("Filter");

		// Token: 0x040000D4 RID: 212
		public static readonly NameToken First = new NameToken("First");

		// Token: 0x040000D5 RID: 213
		public static readonly NameToken FirstChar = new NameToken("FirstChar");

		// Token: 0x040000D6 RID: 214
		public static readonly NameToken Fit = new NameToken("Fit");

		// Token: 0x040000D7 RID: 215
		public static readonly NameToken FitB = new NameToken("FitB");

		// Token: 0x040000D8 RID: 216
		public static readonly NameToken FitBH = new NameToken("FitBH");

		// Token: 0x040000D9 RID: 217
		public static readonly NameToken FitBV = new NameToken("FitBV");

		// Token: 0x040000DA RID: 218
		public static readonly NameToken FitH = new NameToken("FitH");

		// Token: 0x040000DB RID: 219
		public static readonly NameToken FitR = new NameToken("FitR");

		// Token: 0x040000DC RID: 220
		public static readonly NameToken FitV = new NameToken("FitV");

		// Token: 0x040000DD RID: 221
		public static readonly NameToken FitWindow = new NameToken("FitWindow");

		// Token: 0x040000DE RID: 222
		public static readonly NameToken Fl = new NameToken("FL");

		// Token: 0x040000DF RID: 223
		public static readonly NameToken Flags = new NameToken("Flags");

		// Token: 0x040000E0 RID: 224
		public static readonly NameToken FlateDecode = new NameToken("FlateDecode");

		// Token: 0x040000E1 RID: 225
		public static readonly NameToken FlateDecodeAbbreviation = new NameToken("Fl");

		// Token: 0x040000E2 RID: 226
		public static readonly NameToken Font = new NameToken("Font");

		// Token: 0x040000E3 RID: 227
		public static readonly NameToken FontBbox = new NameToken("FontBBox");

		// Token: 0x040000E4 RID: 228
		public static readonly NameToken FontDescriptor = new NameToken("FontDescriptor");

		// Token: 0x040000E5 RID: 229
		public static readonly NameToken FontFamily = new NameToken("FontFamily");

		// Token: 0x040000E6 RID: 230
		public static readonly NameToken FontFile = new NameToken("FontFile");

		// Token: 0x040000E7 RID: 231
		public static readonly NameToken FontFile2 = new NameToken("FontFile2");

		// Token: 0x040000E8 RID: 232
		public static readonly NameToken FontFile3 = new NameToken("FontFile3");

		// Token: 0x040000E9 RID: 233
		public static readonly NameToken FontMatrix = new NameToken("FontMatrix");

		// Token: 0x040000EA RID: 234
		public static readonly NameToken FontName = new NameToken("FontName");

		// Token: 0x040000EB RID: 235
		public static readonly NameToken FontStretch = new NameToken("FontStretch");

		// Token: 0x040000EC RID: 236
		public static readonly NameToken FontWeight = new NameToken("FontWeight");

		// Token: 0x040000ED RID: 237
		public static readonly NameToken Form = new NameToken("Form");

		// Token: 0x040000EE RID: 238
		public static readonly NameToken Formtype = new NameToken("FormType");

		// Token: 0x040000EF RID: 239
		public static readonly NameToken Frm = new NameToken("FRM");

		// Token: 0x040000F0 RID: 240
		public static readonly NameToken FreeText = new NameToken("FreeText");

		// Token: 0x040000F1 RID: 241
		public static readonly NameToken Ft = new NameToken("FT");

		// Token: 0x040000F2 RID: 242
		public static readonly NameToken Function = new NameToken("Function");

		// Token: 0x040000F3 RID: 243
		public static readonly NameToken FunctionType = new NameToken("FunctionType");

		// Token: 0x040000F4 RID: 244
		public static readonly NameToken Functions = new NameToken("Functions");

		// Token: 0x040000F5 RID: 245
		public static readonly NameToken G = new NameToken("G");

		// Token: 0x040000F6 RID: 246
		public static readonly NameToken Gamma = new NameToken("Gamma");

		// Token: 0x040000F7 RID: 247
		public static readonly NameToken GoTo = new NameToken("GoTo");

		// Token: 0x040000F8 RID: 248
		public static readonly NameToken GoToE = new NameToken("GoToE");

		// Token: 0x040000F9 RID: 249
		public static readonly NameToken GoToR = new NameToken("GoToR");

		// Token: 0x040000FA RID: 250
		public static readonly NameToken Group = new NameToken("Group");

		// Token: 0x040000FB RID: 251
		public static readonly NameToken GtsPdfa1 = new NameToken("GTS_PDFA1");

		// Token: 0x040000FC RID: 252
		public static readonly NameToken H = new NameToken("H");

		// Token: 0x040000FD RID: 253
		public static readonly NameToken HardLight = new NameToken("HardLight");

		// Token: 0x040000FE RID: 254
		public static readonly NameToken Height = new NameToken("Height");

		// Token: 0x040000FF RID: 255
		public static readonly NameToken HideMenubar = new NameToken("HideMenubar");

		// Token: 0x04000100 RID: 256
		public static readonly NameToken HideToolbar = new NameToken("HideToolbar");

		// Token: 0x04000101 RID: 257
		public static readonly NameToken HideWindowui = new NameToken("HideWindowUI");

		// Token: 0x04000102 RID: 258
		public static readonly NameToken Highlight = new NameToken("Highlight");

		// Token: 0x04000103 RID: 259
		public static readonly NameToken I = new NameToken("I");

		// Token: 0x04000104 RID: 260
		public static readonly NameToken Ic = new NameToken("IC");

		// Token: 0x04000105 RID: 261
		public static readonly NameToken Iccbased = new NameToken("ICCBased");

		// Token: 0x04000106 RID: 262
		public static readonly NameToken Id = new NameToken("ID");

		// Token: 0x04000107 RID: 263
		public static readonly NameToken IdTree = new NameToken("IDTree");

		// Token: 0x04000108 RID: 264
		public static readonly NameToken Identity = new NameToken("Identity");

		// Token: 0x04000109 RID: 265
		public static readonly NameToken IdentityH = new NameToken("Identity-H");

		// Token: 0x0400010A RID: 266
		public static readonly NameToken IdentityV = new NameToken("Identity-V");

		// Token: 0x0400010B RID: 267
		public static readonly NameToken If = new NameToken("IF");

		// Token: 0x0400010C RID: 268
		public static readonly NameToken Im = new NameToken("IM");

		// Token: 0x0400010D RID: 269
		public static readonly NameToken Image = new NameToken("Image");

		// Token: 0x0400010E RID: 270
		public static readonly NameToken ImageB = new NameToken("ImageB");

		// Token: 0x0400010F RID: 271
		public static readonly NameToken ImageC = new NameToken("ImageC");

		// Token: 0x04000110 RID: 272
		public static readonly NameToken ImageI = new NameToken("ImageI");

		// Token: 0x04000111 RID: 273
		public static readonly NameToken ImageMask = new NameToken("ImageMask");

		// Token: 0x04000112 RID: 274
		public static readonly NameToken Index = new NameToken("Index");

		// Token: 0x04000113 RID: 275
		public static readonly NameToken Indexed = new NameToken("Indexed");

		// Token: 0x04000114 RID: 276
		public static readonly NameToken Info = new NameToken("Info");

		// Token: 0x04000115 RID: 277
		public static readonly NameToken Ink = new NameToken("Ink");

		// Token: 0x04000116 RID: 278
		public static readonly NameToken Inklist = new NameToken("InkList");

		// Token: 0x04000117 RID: 279
		public static readonly NameToken Intent = new NameToken("Intent");

		// Token: 0x04000118 RID: 280
		public static readonly NameToken Interpolate = new NameToken("Interpolate");

		// Token: 0x04000119 RID: 281
		public static readonly NameToken Irt = new NameToken("IRT");

		// Token: 0x0400011A RID: 282
		public static readonly NameToken It = new NameToken("IT");

		// Token: 0x0400011B RID: 283
		public static readonly NameToken ItalicAngle = new NameToken("ItalicAngle");

		// Token: 0x0400011C RID: 284
		public static readonly NameToken JavaScript = new NameToken("JavaScript");

		// Token: 0x0400011D RID: 285
		public static readonly NameToken Jbig2Decode = new NameToken("JBIG2Decode");

		// Token: 0x0400011E RID: 286
		public static readonly NameToken Jbig2Globals = new NameToken("JBIG2Globals");

		// Token: 0x0400011F RID: 287
		public static readonly NameToken JpxDecode = new NameToken("JPXDecode");

		// Token: 0x04000120 RID: 288
		public static readonly NameToken Js = new NameToken("JS");

		// Token: 0x04000121 RID: 289
		public static readonly NameToken K = new NameToken("K");

		// Token: 0x04000122 RID: 290
		public static readonly NameToken Keywords = new NameToken("Keywords");

		// Token: 0x04000123 RID: 291
		public static readonly NameToken Kids = new NameToken("Kids");

		// Token: 0x04000124 RID: 292
		public static readonly NameToken L = new NameToken("L");

		// Token: 0x04000125 RID: 293
		public static readonly NameToken Lab = new NameToken("Lab");

		// Token: 0x04000126 RID: 294
		public static readonly NameToken Lang = new NameToken("Lang");

		// Token: 0x04000127 RID: 295
		public static readonly NameToken Last = new NameToken("Last");

		// Token: 0x04000128 RID: 296
		public static readonly NameToken LastChar = new NameToken("LastChar");

		// Token: 0x04000129 RID: 297
		public static readonly NameToken LastModified = new NameToken("LastModified");

		// Token: 0x0400012A RID: 298
		public static readonly NameToken Lc = new NameToken("LC");

		// Token: 0x0400012B RID: 299
		public static readonly NameToken Le = new NameToken("LE");

		// Token: 0x0400012C RID: 300
		public static readonly NameToken Leading = new NameToken("Leading");

		// Token: 0x0400012D RID: 301
		public static readonly NameToken Left = new NameToken("Left");

		// Token: 0x0400012E RID: 302
		public static readonly NameToken LegalAttestation = new NameToken("LegalAttestation");

		// Token: 0x0400012F RID: 303
		public static readonly NameToken Length = new NameToken("Length");

		// Token: 0x04000130 RID: 304
		public static readonly NameToken Length1 = new NameToken("Length1");

		// Token: 0x04000131 RID: 305
		public static readonly NameToken Length2 = new NameToken("Length2");

		// Token: 0x04000132 RID: 306
		public static readonly NameToken Lighten = new NameToken("Lighten");

		// Token: 0x04000133 RID: 307
		public static readonly NameToken Limits = new NameToken("Limits");

		// Token: 0x04000134 RID: 308
		public static readonly NameToken Line = new NameToken("Line");

		// Token: 0x04000135 RID: 309
		public static readonly NameToken Link = new NameToken("Link");

		// Token: 0x04000136 RID: 310
		public static readonly NameToken Lj = new NameToken("LJ");

		// Token: 0x04000137 RID: 311
		public static readonly NameToken Ll = new NameToken("LL");

		// Token: 0x04000138 RID: 312
		public static readonly NameToken Lle = new NameToken("LLE");

		// Token: 0x04000139 RID: 313
		public static readonly NameToken Llo = new NameToken("LLO");

		// Token: 0x0400013A RID: 314
		public static readonly NameToken Location = new NameToken("Location");

		// Token: 0x0400013B RID: 315
		public static readonly NameToken Luminosity = new NameToken("Luminosity");

		// Token: 0x0400013C RID: 316
		public static readonly NameToken Lw = new NameToken("LW");

		// Token: 0x0400013D RID: 317
		public static readonly NameToken LzwDecode = new NameToken("LZWDecode");

		// Token: 0x0400013E RID: 318
		public static readonly NameToken LzwDecodeAbbreviation = new NameToken("LZW");

		// Token: 0x0400013F RID: 319
		public static readonly NameToken M = new NameToken("M");

		// Token: 0x04000140 RID: 320
		public static readonly NameToken Mac = new NameToken("Mac");

		// Token: 0x04000141 RID: 321
		public static readonly NameToken MacExpertEncoding = new NameToken("MacExpertEncoding");

		// Token: 0x04000142 RID: 322
		public static readonly NameToken MacRomanEncoding = new NameToken("MacRomanEncoding");

		// Token: 0x04000143 RID: 323
		public static readonly NameToken Marked = new NameToken("Marked");

		// Token: 0x04000144 RID: 324
		public static readonly NameToken MarkInfo = new NameToken("MarkInfo");

		// Token: 0x04000145 RID: 325
		public static readonly NameToken Mask = new NameToken("Mask");

		// Token: 0x04000146 RID: 326
		public static readonly NameToken Matrix = new NameToken("Matrix");

		// Token: 0x04000147 RID: 327
		public static readonly NameToken Matte = new NameToken("Matte");

		// Token: 0x04000148 RID: 328
		public static readonly NameToken MaxLen = new NameToken("MaxLen");

		// Token: 0x04000149 RID: 329
		public static readonly NameToken MaxWidth = new NameToken("MaxWidth");

		// Token: 0x0400014A RID: 330
		public static readonly NameToken Mcid = new NameToken("MCID");

		// Token: 0x0400014B RID: 331
		public static readonly NameToken Mdp = new NameToken("MDP");

		// Token: 0x0400014C RID: 332
		public static readonly NameToken Measure = new NameToken("Measure");

		// Token: 0x0400014D RID: 333
		public static readonly NameToken MediaBox = new NameToken("MediaBox");

		// Token: 0x0400014E RID: 334
		public static readonly NameToken Metadata = new NameToken("Metadata");

		// Token: 0x0400014F RID: 335
		public static readonly NameToken MissingWidth = new NameToken("MissingWidth");

		// Token: 0x04000150 RID: 336
		public static readonly NameToken Mix = new NameToken("Mix");

		// Token: 0x04000151 RID: 337
		public static readonly NameToken MixingHints = new NameToken("MixingHints");

		// Token: 0x04000152 RID: 338
		public static readonly NameToken Mk = new NameToken("MK");

		// Token: 0x04000153 RID: 339
		public static readonly NameToken Ml = new NameToken("ML");

		// Token: 0x04000154 RID: 340
		public static readonly NameToken MmType1 = new NameToken("MMType1");

		// Token: 0x04000155 RID: 341
		public static readonly NameToken ModDate = new NameToken("ModDate");

		// Token: 0x04000156 RID: 342
		public static readonly NameToken Movie = new NameToken("Movie");

		// Token: 0x04000157 RID: 343
		public static readonly NameToken Multiply = new NameToken("Multiply");

		// Token: 0x04000158 RID: 344
		public static readonly NameToken N = new NameToken("N");

		// Token: 0x04000159 RID: 345
		public static readonly NameToken Name = new NameToken("Name");

		// Token: 0x0400015A RID: 346
		public static readonly NameToken Names = new NameToken("Names");

		// Token: 0x0400015B RID: 347
		public static readonly NameToken NeedAppearances = new NameToken("NeedAppearances");

		// Token: 0x0400015C RID: 348
		public static readonly NameToken Next = new NameToken("Next");

		// Token: 0x0400015D RID: 349
		public static readonly NameToken Nm = new NameToken("NM");

		// Token: 0x0400015E RID: 350
		public static readonly NameToken NonEfontNoWarn = new NameToken("NonEFontNoWarn");

		// Token: 0x0400015F RID: 351
		public static readonly NameToken NonFullScreenPageMode = new NameToken("NonFullScreenPageMode");

		// Token: 0x04000160 RID: 352
		public static readonly NameToken None = new NameToken("None");

		// Token: 0x04000161 RID: 353
		public static readonly NameToken Normal = new NameToken("Normal");

		// Token: 0x04000162 RID: 354
		public static readonly NameToken Nums = new NameToken("Nums");

		// Token: 0x04000163 RID: 355
		public static readonly NameToken O = new NameToken("O");

		// Token: 0x04000164 RID: 356
		public static readonly NameToken Obj = new NameToken("Obj");

		// Token: 0x04000165 RID: 357
		public static readonly NameToken ObjStm = new NameToken("ObjStm");

		// Token: 0x04000166 RID: 358
		public static readonly NameToken Oc = new NameToken("OC");

		// Token: 0x04000167 RID: 359
		public static readonly NameToken Ocg = new NameToken("OCG");

		// Token: 0x04000168 RID: 360
		public static readonly NameToken Ocgs = new NameToken("OCGs");

		// Token: 0x04000169 RID: 361
		public static readonly NameToken Ocproperties = new NameToken("OCProperties");

		// Token: 0x0400016A RID: 362
		public static readonly NameToken Oe = new NameToken("OE");

		// Token: 0x0400016B RID: 363
		public static readonly NameToken Off = new NameToken("OFF");

		// Token: 0x0400016C RID: 364
		public static readonly NameToken OffAcroform = new NameToken("Off");

		// Token: 0x0400016D RID: 365
		public static readonly NameToken On = new NameToken("ON");

		// Token: 0x0400016E RID: 366
		public static readonly NameToken Op = new NameToken("OP");

		// Token: 0x0400016F RID: 367
		public static readonly NameToken OpNs = new NameToken("op");

		// Token: 0x04000170 RID: 368
		public static readonly NameToken OpenAction = new NameToken("OpenAction");

		// Token: 0x04000171 RID: 369
		public static readonly NameToken OpenType = new NameToken("OpenType");

		// Token: 0x04000172 RID: 370
		public static readonly NameToken Opm = new NameToken("OPM");

		// Token: 0x04000173 RID: 371
		public static readonly NameToken Opt = new NameToken("Opt");

		// Token: 0x04000174 RID: 372
		public static readonly NameToken Order = new NameToken("Order");

		// Token: 0x04000175 RID: 373
		public static readonly NameToken Ordering = new NameToken("Ordering");

		// Token: 0x04000176 RID: 374
		public static readonly NameToken Os = new NameToken("OS");

		// Token: 0x04000177 RID: 375
		public static readonly NameToken Outlines = new NameToken("Outlines");

		// Token: 0x04000178 RID: 376
		public static readonly NameToken OutputCondition = new NameToken("OutputCondition");

		// Token: 0x04000179 RID: 377
		public static readonly NameToken OutputConditionIdentifier = new NameToken("OutputConditionIdentifier");

		// Token: 0x0400017A RID: 378
		public static readonly NameToken OutputIntent = new NameToken("OutputIntent");

		// Token: 0x0400017B RID: 379
		public static readonly NameToken OutputIntents = new NameToken("OutputIntents");

		// Token: 0x0400017C RID: 380
		public static readonly NameToken Overlay = new NameToken("Overlay");

		// Token: 0x0400017D RID: 381
		public static readonly NameToken P = new NameToken("P");

		// Token: 0x0400017E RID: 382
		public static readonly NameToken Page = new NameToken("Page");

		// Token: 0x0400017F RID: 383
		public static readonly NameToken PageLabels = new NameToken("PageLabels");

		// Token: 0x04000180 RID: 384
		public static readonly NameToken PageLayout = new NameToken("PageLayout");

		// Token: 0x04000181 RID: 385
		public static readonly NameToken PageMode = new NameToken("PageMode");

		// Token: 0x04000182 RID: 386
		public static readonly NameToken Pages = new NameToken("Pages");

		// Token: 0x04000183 RID: 387
		public static readonly NameToken PaintType = new NameToken("PaintType");

		// Token: 0x04000184 RID: 388
		public static readonly NameToken Panose = new NameToken("Panose");

		// Token: 0x04000185 RID: 389
		public static readonly NameToken Params = new NameToken("Params");

		// Token: 0x04000186 RID: 390
		public static readonly NameToken Parent = new NameToken("Parent");

		// Token: 0x04000187 RID: 391
		public static readonly NameToken ParentTree = new NameToken("ParentTree");

		// Token: 0x04000188 RID: 392
		public static readonly NameToken ParentTreeNextKey = new NameToken("ParentTreeNextKey");

		// Token: 0x04000189 RID: 393
		public static readonly NameToken Pattern = new NameToken("Pattern");

		// Token: 0x0400018A RID: 394
		public static readonly NameToken PatternType = new NameToken("PatternType");

		// Token: 0x0400018B RID: 395
		public static readonly NameToken PdfDocEncoding = new NameToken("PDFDocEncoding");

		// Token: 0x0400018C RID: 396
		public static readonly NameToken Perms = new NameToken("Perms");

		// Token: 0x0400018D RID: 397
		public static readonly NameToken Pg = new NameToken("Pg");

		// Token: 0x0400018E RID: 398
		public static readonly NameToken Polygon = new NameToken("Polygon");

		// Token: 0x0400018F RID: 399
		public static readonly NameToken PolyLine = new NameToken("PolyLine");

		// Token: 0x04000190 RID: 400
		public static readonly NameToken Popup = new NameToken("Popup");

		// Token: 0x04000191 RID: 401
		public static readonly NameToken PreRelease = new NameToken("PreRelease");

		// Token: 0x04000192 RID: 402
		public static readonly NameToken Predictor = new NameToken("Predictor");

		// Token: 0x04000193 RID: 403
		public static readonly NameToken Prev = new NameToken("Prev");

		// Token: 0x04000194 RID: 404
		public static readonly NameToken PrintArea = new NameToken("PrintArea");

		// Token: 0x04000195 RID: 405
		public static readonly NameToken PrintClip = new NameToken("PrintClip");

		// Token: 0x04000196 RID: 406
		public static readonly NameToken PrintScaling = new NameToken("PrintScaling");

		// Token: 0x04000197 RID: 407
		public static readonly NameToken PrinterMark = new NameToken("PrinterMark");

		// Token: 0x04000198 RID: 408
		public static readonly NameToken ProcSet = new NameToken("ProcSet");

		// Token: 0x04000199 RID: 409
		public static readonly NameToken Process = new NameToken("Process");

		// Token: 0x0400019A RID: 410
		public static readonly NameToken Producer = new NameToken("Producer");

		// Token: 0x0400019B RID: 411
		public static readonly NameToken PropBuild = new NameToken("Prop_Build");

		// Token: 0x0400019C RID: 412
		public static readonly NameToken Properties = new NameToken("Properties");

		// Token: 0x0400019D RID: 413
		public static readonly NameToken Ps = new NameToken("PS");

		// Token: 0x0400019E RID: 414
		public static readonly NameToken PubSec = new NameToken("PubSec");

		// Token: 0x0400019F RID: 415
		public static readonly NameToken Q = new NameToken("Q");

		// Token: 0x040001A0 RID: 416
		public static readonly NameToken Quadpoints = new NameToken("QuadPoints");

		// Token: 0x040001A1 RID: 417
		public static readonly NameToken R = new NameToken("R");

		// Token: 0x040001A2 RID: 418
		public static readonly NameToken Range = new NameToken("Range");

		// Token: 0x040001A3 RID: 419
		public static readonly NameToken Rc = new NameToken("RC");

		// Token: 0x040001A4 RID: 420
		public static readonly NameToken Rd = new NameToken("RD");

		// Token: 0x040001A5 RID: 421
		public static readonly NameToken Reason = new NameToken("Reason");

		// Token: 0x040001A6 RID: 422
		public static readonly NameToken Reasons = new NameToken("Reasons");

		// Token: 0x040001A7 RID: 423
		public static readonly NameToken Repeat = new NameToken("Repeat");

		// Token: 0x040001A8 RID: 424
		public static readonly NameToken Recipients = new NameToken("Recipients");

		// Token: 0x040001A9 RID: 425
		public static readonly NameToken Rect = new NameToken("Rect");

		// Token: 0x040001AA RID: 426
		public static readonly NameToken Registry = new NameToken("Registry");

		// Token: 0x040001AB RID: 427
		public static readonly NameToken RegistryName = new NameToken("RegistryName");

		// Token: 0x040001AC RID: 428
		public static readonly NameToken Rename = new NameToken("Rename");

		// Token: 0x040001AD RID: 429
		public static readonly NameToken Resources = new NameToken("Resources");

		// Token: 0x040001AE RID: 430
		public static readonly NameToken Rgb = new NameToken("RGB");

		// Token: 0x040001AF RID: 431
		public static readonly NameToken Ri = new NameToken("RI");

		// Token: 0x040001B0 RID: 432
		public static readonly NameToken Right = new NameToken("Right");

		// Token: 0x040001B1 RID: 433
		public static readonly NameToken RoleMap = new NameToken("RoleMap");

		// Token: 0x040001B2 RID: 434
		public static readonly NameToken Root = new NameToken("Root");

		// Token: 0x040001B3 RID: 435
		public static readonly NameToken Rotate = new NameToken("Rotate");

		// Token: 0x040001B4 RID: 436
		public static readonly NameToken Rows = new NameToken("Rows");

		// Token: 0x040001B5 RID: 437
		public static readonly NameToken RunLengthDecode = new NameToken("RunLengthDecode");

		// Token: 0x040001B6 RID: 438
		public static readonly NameToken RunLengthDecodeAbbreviation = new NameToken("RL");

		// Token: 0x040001B7 RID: 439
		public static readonly NameToken Rv = new NameToken("RV");

		// Token: 0x040001B8 RID: 440
		public static readonly NameToken S = new NameToken("S");

		// Token: 0x040001B9 RID: 441
		public static readonly NameToken Sa = new NameToken("SA");

		// Token: 0x040001BA RID: 442
		public static readonly NameToken Screen = new NameToken("Screen");

		// Token: 0x040001BB RID: 443
		public static readonly NameToken Se = new NameToken("SE");

		// Token: 0x040001BC RID: 444
		public static readonly NameToken Separation = new NameToken("Separation");

		// Token: 0x040001BD RID: 445
		public static readonly NameToken SetF = new NameToken("SetF");

		// Token: 0x040001BE RID: 446
		public static readonly NameToken SetFf = new NameToken("SetFf");

		// Token: 0x040001BF RID: 447
		public static readonly NameToken Shading = new NameToken("Shading");

		// Token: 0x040001C0 RID: 448
		public static readonly NameToken ShadingType = new NameToken("ShadingType");

		// Token: 0x040001C1 RID: 449
		public static readonly NameToken Sig = new NameToken("Sig");

		// Token: 0x040001C2 RID: 450
		public static readonly NameToken SigFlags = new NameToken("SigFlags");

		// Token: 0x040001C3 RID: 451
		public static readonly NameToken Size = new NameToken("Size");

		// Token: 0x040001C4 RID: 452
		public static readonly NameToken Sm = new NameToken("SM");

		// Token: 0x040001C5 RID: 453
		public static readonly NameToken Smask = new NameToken("SMask");

		// Token: 0x040001C6 RID: 454
		public static readonly NameToken SoftLight = new NameToken("SoftLight");

		// Token: 0x040001C7 RID: 455
		public static readonly NameToken Sound = new NameToken("Sound");

		// Token: 0x040001C8 RID: 456
		public static readonly NameToken Square = new NameToken("Square");

		// Token: 0x040001C9 RID: 457
		public static readonly NameToken Squiggly = new NameToken("Squiggly");

		// Token: 0x040001CA RID: 458
		public static readonly NameToken Ss = new NameToken("SS");

		// Token: 0x040001CB RID: 459
		public static readonly NameToken St = new NameToken("St");

		// Token: 0x040001CC RID: 460
		public static readonly NameToken Stamp = new NameToken("Stamp");

		// Token: 0x040001CD RID: 461
		public static readonly NameToken StandardEncoding = new NameToken("StandardEncoding");

		// Token: 0x040001CE RID: 462
		public static readonly NameToken State = new NameToken("State");

		// Token: 0x040001CF RID: 463
		public static readonly NameToken StateModel = new NameToken("StateModel");

		// Token: 0x040001D0 RID: 464
		public static readonly NameToken Status = new NameToken("Status");

		// Token: 0x040001D1 RID: 465
		public static readonly NameToken StdCf = new NameToken("StdCF");

		// Token: 0x040001D2 RID: 466
		public static readonly NameToken StemH = new NameToken("StemH");

		// Token: 0x040001D3 RID: 467
		public static readonly NameToken StemV = new NameToken("StemV");

		// Token: 0x040001D4 RID: 468
		public static readonly NameToken StmF = new NameToken("StmF");

		// Token: 0x040001D5 RID: 469
		public static readonly NameToken StrF = new NameToken("StrF");

		// Token: 0x040001D6 RID: 470
		public static readonly NameToken StrikeOut = new NameToken("StrikeOut");

		// Token: 0x040001D7 RID: 471
		public static readonly NameToken StructParent = new NameToken("StructParent");

		// Token: 0x040001D8 RID: 472
		public static readonly NameToken StructParents = new NameToken("StructParents");

		// Token: 0x040001D9 RID: 473
		public static readonly NameToken StructTreeRoot = new NameToken("StructTreeRoot");

		// Token: 0x040001DA RID: 474
		public static readonly NameToken Style = new NameToken("Style");

		// Token: 0x040001DB RID: 475
		public static readonly NameToken SubFilter = new NameToken("SubFilter");

		// Token: 0x040001DC RID: 476
		public static readonly NameToken Subj = new NameToken("Subj");

		// Token: 0x040001DD RID: 477
		public static readonly NameToken Subject = new NameToken("Subject");

		// Token: 0x040001DE RID: 478
		public static readonly NameToken Subtype = new NameToken("Subtype");

		// Token: 0x040001DF RID: 479
		public static readonly NameToken Supplement = new NameToken("Supplement");

		// Token: 0x040001E0 RID: 480
		public static readonly NameToken Sv = new NameToken("SV");

		// Token: 0x040001E1 RID: 481
		public static readonly NameToken Sw = new NameToken("SW");

		// Token: 0x040001E2 RID: 482
		public static readonly NameToken Sy = new NameToken("Sy");

		// Token: 0x040001E3 RID: 483
		public static readonly NameToken Synchronous = new NameToken("Synchronous");

		// Token: 0x040001E4 RID: 484
		public static readonly NameToken T = new NameToken("T");

		// Token: 0x040001E5 RID: 485
		public static readonly NameToken Target = new NameToken("Target");

		// Token: 0x040001E6 RID: 486
		public static readonly NameToken Templates = new NameToken("Templates");

		// Token: 0x040001E7 RID: 487
		public static readonly NameToken Text = new NameToken("Text");

		// Token: 0x040001E8 RID: 488
		public static readonly NameToken Threads = new NameToken("Threads");

		// Token: 0x040001E9 RID: 489
		public static readonly NameToken Thumb = new NameToken("Thumb");

		// Token: 0x040001EA RID: 490
		public static readonly NameToken Ti = new NameToken("TI");

		// Token: 0x040001EB RID: 491
		public static readonly NameToken TilingType = new NameToken("TilingType");

		// Token: 0x040001EC RID: 492
		public static readonly NameToken TimeStamp = new NameToken("TimeStamp");

		// Token: 0x040001ED RID: 493
		public static readonly NameToken Title = new NameToken("Title");

		// Token: 0x040001EE RID: 494
		public static readonly NameToken Tk = new NameToken("TK");

		// Token: 0x040001EF RID: 495
		public static readonly NameToken Tm = new NameToken("TM");

		// Token: 0x040001F0 RID: 496
		public static readonly NameToken Top = new NameToken("Top");

		// Token: 0x040001F1 RID: 497
		public static readonly NameToken ToUnicode = new NameToken("ToUnicode");

		// Token: 0x040001F2 RID: 498
		public static readonly NameToken Tr = new NameToken("TR");

		// Token: 0x040001F3 RID: 499
		public static readonly NameToken Tr2 = new NameToken("TR2");

		// Token: 0x040001F4 RID: 500
		public static readonly NameToken Trapped = new NameToken("Trapped");

		// Token: 0x040001F5 RID: 501
		public static readonly NameToken Trans = new NameToken("Trans");

		// Token: 0x040001F6 RID: 502
		public static readonly NameToken Transparency = new NameToken("Transparency");

		// Token: 0x040001F7 RID: 503
		public static readonly NameToken TrapNet = new NameToken("TrapNet");

		// Token: 0x040001F8 RID: 504
		public static readonly NameToken Tref = new NameToken("TRef");

		// Token: 0x040001F9 RID: 505
		public static readonly NameToken TrimBox = new NameToken("TrimBox");

		// Token: 0x040001FA RID: 506
		public static readonly NameToken TrueType = new NameToken("TrueType");

		// Token: 0x040001FB RID: 507
		public static readonly NameToken TrustedMode = new NameToken("TrustedMode");

		// Token: 0x040001FC RID: 508
		public static readonly NameToken Tu = new NameToken("TU");

		// Token: 0x040001FD RID: 509
		public static readonly NameToken Tx = new NameToken("Tx");

		// Token: 0x040001FE RID: 510
		public static readonly NameToken Type = new NameToken("Type");

		// Token: 0x040001FF RID: 511
		public static readonly NameToken Type0 = new NameToken("Type0");

		// Token: 0x04000200 RID: 512
		public static readonly NameToken Type1 = new NameToken("Type1");

		// Token: 0x04000201 RID: 513
		public static readonly NameToken Type1C = new NameToken("Type1C");

		// Token: 0x04000202 RID: 514
		public static readonly NameToken Type3 = new NameToken("Type3");

		// Token: 0x04000203 RID: 515
		public static readonly NameToken U = new NameToken("U");

		// Token: 0x04000204 RID: 516
		public static readonly NameToken Ue = new NameToken("UE");

		// Token: 0x04000205 RID: 517
		public static readonly NameToken Uf = new NameToken("UF");

		// Token: 0x04000206 RID: 518
		public static readonly NameToken Underline = new NameToken("Underline");

		// Token: 0x04000207 RID: 519
		public static readonly NameToken Unchanged = new NameToken("Unchanged");

		// Token: 0x04000208 RID: 520
		public static readonly NameToken Unix = new NameToken("Unix");

		// Token: 0x04000209 RID: 521
		public static readonly NameToken Uri = new NameToken("URI");

		// Token: 0x0400020A RID: 522
		public static readonly NameToken Url = new NameToken("URL");

		// Token: 0x0400020B RID: 523
		public static readonly NameToken Usage = new NameToken("Usage");

		// Token: 0x0400020C RID: 524
		public static readonly NameToken UserUnit = new NameToken("UserUnit");

		// Token: 0x0400020D RID: 525
		public static readonly NameToken V = new NameToken("V");

		// Token: 0x0400020E RID: 526
		public static readonly NameToken V2 = new NameToken("V2");

		// Token: 0x0400020F RID: 527
		public static readonly NameToken VE = new NameToken("VE");

		// Token: 0x04000210 RID: 528
		public static readonly NameToken VerisignPpkvs = new NameToken("VeriSign.PPKVS");

		// Token: 0x04000211 RID: 529
		public static readonly NameToken Version = new NameToken("Version");

		// Token: 0x04000212 RID: 530
		public static readonly NameToken Vertices = new NameToken("Vertices");

		// Token: 0x04000213 RID: 531
		public static readonly NameToken VerticesPerRow = new NameToken("VerticesPerRow");

		// Token: 0x04000214 RID: 532
		public static readonly NameToken ViewArea = new NameToken("ViewArea");

		// Token: 0x04000215 RID: 533
		public static readonly NameToken ViewClip = new NameToken("ViewClip");

		// Token: 0x04000216 RID: 534
		public static readonly NameToken ViewerPreferences = new NameToken("ViewerPreferences");

		// Token: 0x04000217 RID: 535
		public static readonly NameToken Volume = new NameToken("Volume");

		// Token: 0x04000218 RID: 536
		public static readonly NameToken Vp = new NameToken("VP");

		// Token: 0x04000219 RID: 537
		public static readonly NameToken W = new NameToken("W");

		// Token: 0x0400021A RID: 538
		public static readonly NameToken W2 = new NameToken("W2");

		// Token: 0x0400021B RID: 539
		public static readonly NameToken Watermark = new NameToken("Watermark");

		// Token: 0x0400021C RID: 540
		public static readonly NameToken WhitePoint = new NameToken("WhitePoint");

		// Token: 0x0400021D RID: 541
		public static readonly NameToken Widget = new NameToken("Widget");

		// Token: 0x0400021E RID: 542
		public static readonly NameToken Width = new NameToken("Width");

		// Token: 0x0400021F RID: 543
		public static readonly NameToken Widths = new NameToken("Widths");

		// Token: 0x04000220 RID: 544
		public static readonly NameToken WinAnsiEncoding = new NameToken("WinAnsiEncoding");

		// Token: 0x04000221 RID: 545
		public static readonly NameToken Xfa = new NameToken("XFA");

		// Token: 0x04000222 RID: 546
		public static readonly NameToken Xheight = new NameToken("XHeight");

		// Token: 0x04000223 RID: 547
		public static readonly NameToken Xml = new NameToken("XML");

		// Token: 0x04000224 RID: 548
		public static readonly NameToken Xobject = new NameToken("XObject");

		// Token: 0x04000225 RID: 549
		public static readonly NameToken Xref = new NameToken("XRef");

		// Token: 0x04000226 RID: 550
		public static readonly NameToken XrefStm = new NameToken("XRefStm");

		// Token: 0x04000227 RID: 551
		public static readonly NameToken XStep = new NameToken("XStep");

		// Token: 0x04000228 RID: 552
		public static readonly NameToken XYZ = new NameToken("XYZ");

		// Token: 0x04000229 RID: 553
		public static readonly NameToken YStep = new NameToken("YStep");

		// Token: 0x0400022A RID: 554
		public static readonly NameToken Yes = new NameToken("Yes");
	}
}
