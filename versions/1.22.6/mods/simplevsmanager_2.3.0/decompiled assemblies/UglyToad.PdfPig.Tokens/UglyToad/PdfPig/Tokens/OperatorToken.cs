using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000010 RID: 16
	public class OperatorToken : IDataToken<string>, IToken, IEquatable<IToken>
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00004F1B File Offset: 0x0000311B
		public string Data { get; }

		// Token: 0x0600005B RID: 91 RVA: 0x00004F24 File Offset: 0x00003124
		private OperatorToken(string data)
		{
			object @lock = OperatorToken.Lock;
			string text;
			lock (@lock)
			{
				if (!OperatorToken.PooledNames.TryGetValue(data, ref text))
				{
					text = data;
					OperatorToken.PooledNames[data] = text;
				}
			}
			this.Data = text;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004F88 File Offset: 0x00003188
		public static OperatorToken Create(string data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			uint num = <PrivateImplementationDetails>.ComputeStringHash(data);
			if (num <= 1838632776U)
			{
				if (num <= 989170495U)
				{
					if (num <= 650911164U)
					{
						if (num != 290142450U)
						{
							if (num == 650911164U)
							{
								if (data == "ET")
								{
									return OperatorToken.Et;
								}
							}
						}
						else if (data == "endstream")
						{
							return OperatorToken.EndStream;
						}
					}
					else if (num != 685599235U)
					{
						if (num != 872155479U)
						{
							if (num == 989170495U)
							{
								if (data == "Tf")
								{
									return OperatorToken.Tf;
								}
							}
						}
						else if (data == "endobj")
						{
							return OperatorToken.EndObject;
						}
					}
					else if (data == "BT")
					{
						return OperatorToken.Bt;
					}
				}
				else if (num <= 1481930064U)
				{
					if (num != 1278716217U)
					{
						if (num == 1481930064U)
						{
							if (data == "re")
							{
								return OperatorToken.Re;
							}
						}
					}
					else if (data == "dict")
					{
						return OperatorToken.Dict;
					}
				}
				else if (num != 1601138021U)
				{
					if (num != 1769118190U)
					{
						if (num == 1838632776U)
						{
							if (data == "xref")
							{
								return OperatorToken.Xref;
							}
						}
					}
					else if (data == "put")
					{
						return OperatorToken.Put;
					}
				}
				else if (data == "stream")
				{
					return OperatorToken.StartStream;
				}
			}
			else if (num <= 3456888823U)
			{
				if (num <= 2901640080U)
				{
					if (num != 2767745204U)
					{
						if (num == 2901640080U)
						{
							if (data == "for")
							{
								return OperatorToken.For;
							}
						}
					}
					else if (data == "W*")
					{
						return OperatorToken.WStar;
					}
				}
				else if (num != 3310976652U)
				{
					if (num != 3343205242U)
					{
						if (num == 3456888823U)
						{
							if (data == "readonly")
							{
								return OperatorToken.Readonly;
							}
						}
					}
					else if (data == "obj")
					{
						return OperatorToken.StartObject;
					}
				}
				else if (data == "def")
				{
					return OperatorToken.Def;
				}
			}
			else if (num <= 3607893173U)
			{
				if (num != 3543200294U)
				{
					if (num != 3557560316U)
					{
						if (num == 3607893173U)
						{
							if (data == "R")
							{
								return OperatorToken.R;
							}
						}
					}
					else if (data == "Q")
					{
						return OperatorToken.QPop;
					}
				}
				else if (data == "dup")
				{
					return OperatorToken.Dup;
				}
			}
			else if (num != 3943445553U)
			{
				if (num != 4094444124U)
				{
					if (num == 4256701913U)
					{
						if (data == "eexec")
						{
							return OperatorToken.Eexec;
						}
					}
				}
				else if (data == "q")
				{
					return OperatorToken.QPush;
				}
			}
			else if (data == "n")
			{
				return OperatorToken.N;
			}
			return new OperatorToken(data);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005328 File Offset: 0x00003528
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			OperatorToken operatorToken = obj as OperatorToken;
			return operatorToken != null && this.Data == operatorToken.Data;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005358 File Offset: 0x00003558
		public override int GetHashCode()
		{
			return this.Data.GetHashCode();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005365 File Offset: 0x00003565
		public override string ToString()
		{
			return this.Data;
		}

		// Token: 0x0400024B RID: 587
		private static readonly object Lock = new object();

		// Token: 0x0400024C RID: 588
		private static readonly Dictionary<string, string> PooledNames = new Dictionary<string, string>();

		// Token: 0x0400024D RID: 589
		public static readonly OperatorToken Bt = new OperatorToken("BT");

		// Token: 0x0400024E RID: 590
		public static readonly OperatorToken Def = new OperatorToken("def");

		// Token: 0x0400024F RID: 591
		public static readonly OperatorToken Dict = new OperatorToken("dict");

		// Token: 0x04000250 RID: 592
		public static readonly OperatorToken Dup = new OperatorToken("dup");

		// Token: 0x04000251 RID: 593
		public static readonly OperatorToken Eexec = new OperatorToken("eexec");

		// Token: 0x04000252 RID: 594
		public static readonly OperatorToken EndObject = new OperatorToken("endobj");

		// Token: 0x04000253 RID: 595
		public static readonly OperatorToken EndStream = new OperatorToken("endstream");

		// Token: 0x04000254 RID: 596
		public static readonly OperatorToken Et = new OperatorToken("ET");

		// Token: 0x04000255 RID: 597
		public static readonly OperatorToken For = new OperatorToken("for");

		// Token: 0x04000256 RID: 598
		public static readonly OperatorToken N = new OperatorToken("n");

		// Token: 0x04000257 RID: 599
		public static readonly OperatorToken Put = new OperatorToken("put");

		// Token: 0x04000258 RID: 600
		public static readonly OperatorToken QPop = new OperatorToken("Q");

		// Token: 0x04000259 RID: 601
		public static readonly OperatorToken QPush = new OperatorToken("q");

		// Token: 0x0400025A RID: 602
		public static readonly OperatorToken R = new OperatorToken("R");

		// Token: 0x0400025B RID: 603
		public static readonly OperatorToken Re = new OperatorToken("re");

		// Token: 0x0400025C RID: 604
		public static readonly OperatorToken Readonly = new OperatorToken("readonly");

		// Token: 0x0400025D RID: 605
		public static readonly OperatorToken StartObject = new OperatorToken("obj");

		// Token: 0x0400025E RID: 606
		public static readonly OperatorToken StartStream = new OperatorToken("stream");

		// Token: 0x0400025F RID: 607
		public static readonly OperatorToken Tf = new OperatorToken("Tf");

		// Token: 0x04000260 RID: 608
		public static readonly OperatorToken WStar = new OperatorToken("W*");

		// Token: 0x04000261 RID: 609
		public static readonly OperatorToken Xref = new OperatorToken("xref");
	}
}
