using System;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000039 RID: 57
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct Color
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00004A48 File Offset: 0x00002C48
		public uint Hex { get; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00004A50 File Offset: 0x00002C50
		public byte Alpha
		{
			get
			{
				return (byte)((this.Hex & 4278190080U) >> 24);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00004A62 File Offset: 0x00002C62
		public byte Red
		{
			get
			{
				return (byte)((this.Hex & 16711680U) >> 16);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00004A74 File Offset: 0x00002C74
		public byte Green
		{
			get
			{
				return (byte)((this.Hex & 65280U) >> 8);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00004A85 File Offset: 0x00002C85
		public byte Blue
		{
			get
			{
				return (byte)(this.Hex & 255U);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00004A94 File Offset: 0x00002C94
		public Color(uint hex)
		{
			this.Hex = hex;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00004A9D File Offset: 0x00002C9D
		public Color WithAlpha(byte alpha)
		{
			return new Color((this.Hex & 16777215U) | (uint)((uint)alpha << 24));
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004AB8 File Offset: 0x00002CB8
		public Color WithAlpha(float alpha)
		{
			if (alpha < 0f || alpha > 1f)
			{
				throw new ArgumentOutOfRangeException("alpha", "Alpha value must be between 0 and 1.");
			}
			byte alpha2 = (byte)(255f * alpha);
			return this.WithAlpha(alpha2);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004AF5 File Offset: 0x00002CF5
		public static Color FromHex(string hex)
		{
			return ColorParser.ParseColorHex(hex);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00004AFD File Offset: 0x00002CFD
		public static Color FromRGB(byte red, byte green, byte blue)
		{
			return Color.FromARGB(byte.MaxValue, red, green, blue);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004B0C File Offset: 0x00002D0C
		public static Color FromARGB(byte alpha, byte red, byte green, byte blue)
		{
			return new Color((uint)((int)alpha << 24 | (int)red << 16 | (int)green << 8 | (int)blue));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00004B22 File Offset: 0x00002D22
		public static implicit operator string(Color color)
		{
			return color.ToString();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00004B31 File Offset: 0x00002D31
		public static implicit operator Color(string hex)
		{
			return Color.FromHex(hex);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00004B39 File Offset: 0x00002D39
		public static implicit operator uint(Color color)
		{
			return color.Hex;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00004B42 File Offset: 0x00002D42
		public static implicit operator Color(uint hex)
		{
			if (hex <= 16777215U)
			{
				hex |= 4278190080U;
			}
			return new Color(hex);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00004B5C File Offset: 0x00002D5C
		public override string ToString()
		{
			if (this.Alpha == 255)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 3);
				defaultInterpolatedStringHandler.AppendLiteral("#");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.Red, "X2");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.Green, "X2");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.Blue, "X2");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(1, 4);
			defaultInterpolatedStringHandler2.AppendLiteral("#");
			defaultInterpolatedStringHandler2.AppendFormatted<byte>(this.Alpha, "X2");
			defaultInterpolatedStringHandler2.AppendFormatted<byte>(this.Red, "X2");
			defaultInterpolatedStringHandler2.AppendFormatted<byte>(this.Green, "X2");
			defaultInterpolatedStringHandler2.AppendFormatted<byte>(this.Blue, "X2");
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}
	}
}
