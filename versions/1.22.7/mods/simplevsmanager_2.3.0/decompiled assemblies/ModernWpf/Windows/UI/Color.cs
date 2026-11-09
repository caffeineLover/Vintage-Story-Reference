using System;
using System.Text;
using Windows.Foundation;
using WinRT;

namespace Windows.UI
{
	// Token: 0x0200001C RID: 28
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	internal struct Color : IFormattable
	{
		// Token: 0x0600020C RID: 524 RVA: 0x000042F0 File Offset: 0x000024F0
		public static Color FromArgb(byte a, byte r, byte g, byte b)
		{
			return new Color
			{
				A = a,
				R = r,
				G = g,
				B = b
			};
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00004326 File Offset: 0x00002526
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000432E File Offset: 0x0000252E
		public byte A
		{
			get
			{
				return this._A;
			}
			set
			{
				this._A = value;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00004337 File Offset: 0x00002537
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000433F File Offset: 0x0000253F
		public byte R
		{
			get
			{
				return this._R;
			}
			set
			{
				this._R = value;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00004348 File Offset: 0x00002548
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00004350 File Offset: 0x00002550
		public byte G
		{
			get
			{
				return this._G;
			}
			set
			{
				this._G = value;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00004359 File Offset: 0x00002559
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00004361 File Offset: 0x00002561
		public byte B
		{
			get
			{
				return this._B;
			}
			set
			{
				this._B = value;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000436A File Offset: 0x0000256A
		public override string ToString()
		{
			return this.ConvertToString(null, null);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00004374 File Offset: 0x00002574
		public string ToString(IFormatProvider provider)
		{
			return this.ConvertToString(null, provider);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000437E File Offset: 0x0000257E
		string IFormattable.ToString(string format, IFormatProvider provider)
		{
			return this.ConvertToString(format, provider);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00004388 File Offset: 0x00002588
		internal string ConvertToString(string format, IFormatProvider provider)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (format == null)
			{
				stringBuilder.AppendFormat(provider, "#{0:X2}", this._A);
				stringBuilder.AppendFormat(provider, "{0:X2}", this._R);
				stringBuilder.AppendFormat(provider, "{0:X2}", this._G);
				stringBuilder.AppendFormat(provider, "{0:X2}", this._B);
			}
			else
			{
				char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
				stringBuilder.AppendFormat(provider, string.Concat(new string[]
				{
					"sc#{1:",
					format,
					"}{0} {2:",
					format,
					"}{0} {3:",
					format,
					"}{0} {4:",
					format,
					"}"
				}), new object[]
				{
					numericListSeparator,
					this._A,
					this._R,
					this._G,
					this._B
				});
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000044A3 File Offset: 0x000026A3
		public override int GetHashCode()
		{
			return this._A.GetHashCode() ^ this._R.GetHashCode() ^ this._G.GetHashCode() ^ this._B.GetHashCode();
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000044D4 File Offset: 0x000026D4
		public override bool Equals(object o)
		{
			return o is Color && this == (Color)o;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000044F1 File Offset: 0x000026F1
		public bool Equals(Color color)
		{
			return this == color;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00004500 File Offset: 0x00002700
		public static bool operator ==(Color color1, Color color2)
		{
			return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B && color1.A == color2.A;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000454F File Offset: 0x0000274F
		public static bool operator !=(Color color1, Color color2)
		{
			return !(color1 == color2);
		}

		// Token: 0x04000044 RID: 68
		private byte _A;

		// Token: 0x04000045 RID: 69
		private byte _R;

		// Token: 0x04000046 RID: 70
		private byte _G;

		// Token: 0x04000047 RID: 71
		private byte _B;
	}
}
