using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000015 RID: 21
	internal class TagData : ITagData
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public TagData(string vendor, string[] comments)
		{
			this.EncoderVendor = vendor;
			Dictionary<string, IReadOnlyList<string>> dictionary = new Dictionary<string, IReadOnlyList<string>>();
			for (int i = 0; i < comments.Length; i++)
			{
				string[] array = comments[i].Split(new char[]
				{
					'='
				});
				if (array.Length == 1)
				{
					array = new string[]
					{
						array[0],
						string.Empty
					};
				}
				int num = array[0].IndexOf('[');
				if (num > -1)
				{
					array[1] = array[0].Substring(num + 1, array[0].Length - num - 2).ToUpper(CultureInfo.CurrentCulture) + ": " + array[1];
					array[0] = array[0].Substring(0, num);
				}
				IReadOnlyList<string> readOnlyList;
				if (dictionary.TryGetValue(array[0].ToUpperInvariant(), ref readOnlyList))
				{
					((List<string>)readOnlyList).Add(array[1]);
				}
				else
				{
					Dictionary<string, IReadOnlyList<string>> dictionary2 = dictionary;
					string text = array[0].ToUpperInvariant();
					List<string> list = new List<string>();
					list.Add(array[1]);
					dictionary2.Add(text, list);
				}
			}
			this._tags = dictionary;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005BCC File Offset: 0x00003DCC
		public string GetTagSingle(string key, bool concatenate = false)
		{
			IReadOnlyList<string> tagMulti = this.GetTagMulti(key);
			if (tagMulti.Count <= 0)
			{
				return string.Empty;
			}
			if (concatenate)
			{
				return string.Join(Environment.NewLine, Enumerable.ToArray<string>(tagMulti));
			}
			return tagMulti[tagMulti.Count - 1];
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005C14 File Offset: 0x00003E14
		public IReadOnlyList<string> GetTagMulti(string key)
		{
			IReadOnlyList<string> result;
			if (this._tags.TryGetValue(key.ToUpperInvariant(), ref result))
			{
				return result;
			}
			return TagData.s_emptyList;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00005C3D File Offset: 0x00003E3D
		public IReadOnlyDictionary<string, IReadOnlyList<string>> All
		{
			get
			{
				return this._tags;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00005C45 File Offset: 0x00003E45
		public string EncoderVendor { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00005C4D File Offset: 0x00003E4D
		public string Title
		{
			get
			{
				return this.GetTagSingle("TITLE", false);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00005C5B File Offset: 0x00003E5B
		public string Version
		{
			get
			{
				return this.GetTagSingle("VERSION", false);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005C69 File Offset: 0x00003E69
		public string Album
		{
			get
			{
				return this.GetTagSingle("ALBUM", false);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005C77 File Offset: 0x00003E77
		public string TrackNumber
		{
			get
			{
				return this.GetTagSingle("TRACKNUMBER", false);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005C85 File Offset: 0x00003E85
		public string Artist
		{
			get
			{
				return this.GetTagSingle("ARTIST", false);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00005C93 File Offset: 0x00003E93
		public IReadOnlyList<string> Performers
		{
			get
			{
				return this.GetTagMulti("PERFORMER");
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005CA0 File Offset: 0x00003EA0
		public string Copyright
		{
			get
			{
				return this.GetTagSingle("COPYRIGHT", false);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00005CAE File Offset: 0x00003EAE
		public string License
		{
			get
			{
				return this.GetTagSingle("LICENSE", false);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00005CBC File Offset: 0x00003EBC
		public string Organization
		{
			get
			{
				return this.GetTagSingle("ORGANIZATION", false);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00005CCA File Offset: 0x00003ECA
		public string Description
		{
			get
			{
				return this.GetTagSingle("DESCRIPTION", false);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005CD8 File Offset: 0x00003ED8
		public IReadOnlyList<string> Genres
		{
			get
			{
				return this.GetTagMulti("GENRE");
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005CE5 File Offset: 0x00003EE5
		public IReadOnlyList<string> Dates
		{
			get
			{
				return this.GetTagMulti("DATE");
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005CF2 File Offset: 0x00003EF2
		public IReadOnlyList<string> Locations
		{
			get
			{
				return this.GetTagMulti("LOCATION");
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00005CFF File Offset: 0x00003EFF
		public string Contact
		{
			get
			{
				return this.GetTagSingle("CONTACT", false);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005D0D File Offset: 0x00003F0D
		public string Isrc
		{
			get
			{
				return this.GetTagSingle("ISRC", false);
			}
		}

		// Token: 0x04000075 RID: 117
		private static IReadOnlyList<string> s_emptyList = new List<string>();

		// Token: 0x04000076 RID: 118
		private Dictionary<string, IReadOnlyList<string>> _tags;
	}
}
