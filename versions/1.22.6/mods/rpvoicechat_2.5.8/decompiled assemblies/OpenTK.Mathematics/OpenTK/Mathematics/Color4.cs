using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace OpenTK.Mathematics
{
	// Token: 0x02000002 RID: 2
	[Serializable]
	public struct Color4 : IEquatable<Color4>, IFormattable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Color4(float r, float g, float b, float a)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
		public Color4(byte r, byte g, byte b, byte a)
		{
			this.R = (float)r / 255f;
			this.G = (float)g / 255f;
			this.B = (float)b / 255f;
			this.A = (float)a / 255f;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020AC File Offset: 0x000002AC
		public readonly int ToArgb()
		{
			return (int)((uint)(this.A * 255f) << 24 | (uint)(this.R * 255f) << 16 | (uint)(this.G * 255f) << 8 | (uint)(this.B * 255f));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020F8 File Offset: 0x000002F8
		public static bool operator ==(Color4 left, Color4 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002102 File Offset: 0x00000302
		public static bool operator !=(Color4 left, Color4 right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000210F File Offset: 0x0000030F
		public static implicit operator Color4(Color color)
		{
			return new Color4(color.R, color.G, color.B, color.A);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002132 File Offset: 0x00000332
		public static explicit operator Color(Color4 color)
		{
			return Color.FromArgb((int)(color.A * 255f), (int)(color.R * 255f), (int)(color.G * 255f), (int)(color.B * 255f));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000216D File Offset: 0x0000036D
		[MethodImpl(256)]
		public unsafe static explicit operator Vector4(Color4 c)
		{
			return *Unsafe.As<Color4, Vector4>(ref c);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000217B File Offset: 0x0000037B
		public override bool Equals(object obj)
		{
			return obj is Color4 && this.Equals((Color4)obj);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002193 File Offset: 0x00000393
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<float, float, float, float>(this.R, this.G, this.B, this.A);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021B2 File Offset: 0x000003B2
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021BC File Offset: 0x000003BC
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021C6 File Offset: 0x000003C6
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021D0 File Offset: 0x000003D0
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string listSeparator = MathHelper.GetListSeparator(formatProvider);
			string text = this.R.ToString(format, formatProvider);
			string text2 = this.G.ToString(format, formatProvider);
			string text3 = this.B.ToString(format, formatProvider);
			string text4 = this.A.ToString(format, formatProvider);
			return string.Concat(new string[]
			{
				"{(R",
				listSeparator,
				" G",
				listSeparator,
				" B",
				listSeparator,
				" A) = (",
				text,
				listSeparator,
				" ",
				text2,
				listSeparator,
				" ",
				text3,
				listSeparator,
				" ",
				text4,
				")}"
			});
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000229B File Offset: 0x0000049B
		public static Color4 Transparent
		{
			get
			{
				return new Color4(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022B2 File Offset: 0x000004B2
		public static Color4 AliceBlue
		{
			get
			{
				return new Color4(240, 248, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022CD File Offset: 0x000004CD
		public static Color4 AntiqueWhite
		{
			get
			{
				return new Color4(250, 235, 215, byte.MaxValue);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000022E8 File Offset: 0x000004E8
		public static Color4 Aqua
		{
			get
			{
				return new Color4(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000022FF File Offset: 0x000004FF
		public static Color4 Aquamarine
		{
			get
			{
				return new Color4(127, byte.MaxValue, 212, byte.MaxValue);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002317 File Offset: 0x00000517
		public static Color4 Azure
		{
			get
			{
				return new Color4(240, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002332 File Offset: 0x00000532
		public static Color4 Beige
		{
			get
			{
				return new Color4(245, 245, 220, byte.MaxValue);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000234D File Offset: 0x0000054D
		public static Color4 Bisque
		{
			get
			{
				return new Color4(byte.MaxValue, 228, 196, byte.MaxValue);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002368 File Offset: 0x00000568
		public static Color4 Black
		{
			get
			{
				return new Color4(0, 0, 0, byte.MaxValue);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002377 File Offset: 0x00000577
		public static Color4 BlanchedAlmond
		{
			get
			{
				return new Color4(byte.MaxValue, 235, 205, byte.MaxValue);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002392 File Offset: 0x00000592
		public static Color4 Blue
		{
			get
			{
				return new Color4(0, 0, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000023A5 File Offset: 0x000005A5
		public static Color4 BlueViolet
		{
			get
			{
				return new Color4(138, 43, 226, byte.MaxValue);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000023BD File Offset: 0x000005BD
		public static Color4 Brown
		{
			get
			{
				return new Color4(165, 42, 42, byte.MaxValue);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000023D2 File Offset: 0x000005D2
		public static Color4 BurlyWood
		{
			get
			{
				return new Color4(222, 184, 135, byte.MaxValue);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000023ED File Offset: 0x000005ED
		public static Color4 CadetBlue
		{
			get
			{
				return new Color4(95, 158, 160, byte.MaxValue);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002405 File Offset: 0x00000605
		public static Color4 Chartreuse
		{
			get
			{
				return new Color4(127, byte.MaxValue, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002419 File Offset: 0x00000619
		public static Color4 Chocolate
		{
			get
			{
				return new Color4(210, 105, 30, byte.MaxValue);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000242E File Offset: 0x0000062E
		public static Color4 Coral
		{
			get
			{
				return new Color4(byte.MaxValue, 127, 80, byte.MaxValue);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002443 File Offset: 0x00000643
		public static Color4 CornflowerBlue
		{
			get
			{
				return new Color4(100, 149, 237, byte.MaxValue);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000245B File Offset: 0x0000065B
		public static Color4 Cornsilk
		{
			get
			{
				return new Color4(byte.MaxValue, 248, 220, byte.MaxValue);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002476 File Offset: 0x00000676
		public static Color4 Crimson
		{
			get
			{
				return new Color4(220, 20, 60, byte.MaxValue);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000248B File Offset: 0x0000068B
		public static Color4 Cyan
		{
			get
			{
				return new Color4(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000024A2 File Offset: 0x000006A2
		public static Color4 DarkBlue
		{
			get
			{
				return new Color4(0, 0, 139, byte.MaxValue);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000024B5 File Offset: 0x000006B5
		public static Color4 DarkCyan
		{
			get
			{
				return new Color4(0, 139, 139, byte.MaxValue);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000024CC File Offset: 0x000006CC
		public static Color4 DarkGoldenrod
		{
			get
			{
				return new Color4(184, 134, 11, byte.MaxValue);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000024E4 File Offset: 0x000006E4
		public static Color4 DarkGray
		{
			get
			{
				return new Color4(169, 169, 169, byte.MaxValue);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000024FF File Offset: 0x000006FF
		public static Color4 DarkGreen
		{
			get
			{
				return new Color4(0, 100, 0, byte.MaxValue);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600002A RID: 42 RVA: 0x0000250F File Offset: 0x0000070F
		public static Color4 DarkKhaki
		{
			get
			{
				return new Color4(189, 183, 107, byte.MaxValue);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002527 File Offset: 0x00000727
		public static Color4 DarkMagenta
		{
			get
			{
				return new Color4(139, 0, 139, byte.MaxValue);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000253E File Offset: 0x0000073E
		public static Color4 DarkOliveGreen
		{
			get
			{
				return new Color4(85, 107, 47, byte.MaxValue);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002550 File Offset: 0x00000750
		public static Color4 DarkOrange
		{
			get
			{
				return new Color4(byte.MaxValue, 140, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002567 File Offset: 0x00000767
		public static Color4 DarkOrchid
		{
			get
			{
				return new Color4(153, 50, 204, byte.MaxValue);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000257F File Offset: 0x0000077F
		public static Color4 DarkRed
		{
			get
			{
				return new Color4(139, 0, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002592 File Offset: 0x00000792
		public static Color4 DarkSalmon
		{
			get
			{
				return new Color4(233, 150, 122, byte.MaxValue);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000025AA File Offset: 0x000007AA
		public static Color4 DarkSeaGreen
		{
			get
			{
				return new Color4(143, 188, 139, byte.MaxValue);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000025C5 File Offset: 0x000007C5
		public static Color4 DarkSlateBlue
		{
			get
			{
				return new Color4(72, 61, 139, byte.MaxValue);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000025DA File Offset: 0x000007DA
		public static Color4 DarkSlateGray
		{
			get
			{
				return new Color4(47, 79, 79, byte.MaxValue);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000025EC File Offset: 0x000007EC
		public static Color4 DarkTurquoise
		{
			get
			{
				return new Color4(0, 206, 209, byte.MaxValue);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002603 File Offset: 0x00000803
		public static Color4 DarkViolet
		{
			get
			{
				return new Color4(148, 0, 211, byte.MaxValue);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0000261A File Offset: 0x0000081A
		public static Color4 DeepPink
		{
			get
			{
				return new Color4(byte.MaxValue, 20, 147, byte.MaxValue);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002632 File Offset: 0x00000832
		public static Color4 DeepSkyBlue
		{
			get
			{
				return new Color4(0, 191, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002649 File Offset: 0x00000849
		public static Color4 DimGray
		{
			get
			{
				return new Color4(105, 105, 105, byte.MaxValue);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0000265B File Offset: 0x0000085B
		public static Color4 DodgerBlue
		{
			get
			{
				return new Color4(30, 144, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002673 File Offset: 0x00000873
		public static Color4 Firebrick
		{
			get
			{
				return new Color4(178, 34, 34, byte.MaxValue);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002688 File Offset: 0x00000888
		public static Color4 FloralWhite
		{
			get
			{
				return new Color4(byte.MaxValue, 250, 240, byte.MaxValue);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000026A3 File Offset: 0x000008A3
		public static Color4 ForestGreen
		{
			get
			{
				return new Color4(34, 139, 34, byte.MaxValue);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600003D RID: 61 RVA: 0x000026B8 File Offset: 0x000008B8
		public static Color4 Fuchsia
		{
			get
			{
				return new Color4(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600003E RID: 62 RVA: 0x000026CF File Offset: 0x000008CF
		public static Color4 Gainsboro
		{
			get
			{
				return new Color4(220, 220, 220, byte.MaxValue);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000026EA File Offset: 0x000008EA
		public static Color4 GhostWhite
		{
			get
			{
				return new Color4(248, 248, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002705 File Offset: 0x00000905
		public static Color4 Gold
		{
			get
			{
				return new Color4(byte.MaxValue, 215, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000041 RID: 65 RVA: 0x0000271C File Offset: 0x0000091C
		public static Color4 Goldenrod
		{
			get
			{
				return new Color4(218, 165, 32, byte.MaxValue);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002734 File Offset: 0x00000934
		public static Color4 Gray
		{
			get
			{
				return new Color4(128, 128, 128, byte.MaxValue);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0000274F File Offset: 0x0000094F
		public static Color4 Green
		{
			get
			{
				return new Color4(0, 128, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002762 File Offset: 0x00000962
		public static Color4 GreenYellow
		{
			get
			{
				return new Color4(173, byte.MaxValue, 47, byte.MaxValue);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000277A File Offset: 0x0000097A
		public static Color4 Honeydew
		{
			get
			{
				return new Color4(240, byte.MaxValue, 240, byte.MaxValue);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002795 File Offset: 0x00000995
		public static Color4 HotPink
		{
			get
			{
				return new Color4(byte.MaxValue, 105, 180, byte.MaxValue);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000027AD File Offset: 0x000009AD
		public static Color4 IndianRed
		{
			get
			{
				return new Color4(205, 92, 92, byte.MaxValue);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000027C2 File Offset: 0x000009C2
		public static Color4 Indigo
		{
			get
			{
				return new Color4(75, 0, 130, byte.MaxValue);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000027D6 File Offset: 0x000009D6
		public static Color4 Ivory
		{
			get
			{
				return new Color4(byte.MaxValue, byte.MaxValue, 240, byte.MaxValue);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000027F1 File Offset: 0x000009F1
		public static Color4 Khaki
		{
			get
			{
				return new Color4(240, 230, 140, byte.MaxValue);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000280C File Offset: 0x00000A0C
		public static Color4 Lavender
		{
			get
			{
				return new Color4(230, 230, 250, byte.MaxValue);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002827 File Offset: 0x00000A27
		public static Color4 LavenderBlush
		{
			get
			{
				return new Color4(byte.MaxValue, 240, 245, byte.MaxValue);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002842 File Offset: 0x00000A42
		public static Color4 LawnGreen
		{
			get
			{
				return new Color4(124, 252, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002856 File Offset: 0x00000A56
		public static Color4 LemonChiffon
		{
			get
			{
				return new Color4(byte.MaxValue, 250, 205, byte.MaxValue);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002871 File Offset: 0x00000A71
		public static Color4 LightBlue
		{
			get
			{
				return new Color4(173, 216, 230, byte.MaxValue);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000288C File Offset: 0x00000A8C
		public static Color4 LightCoral
		{
			get
			{
				return new Color4(240, 128, 128, byte.MaxValue);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000028A7 File Offset: 0x00000AA7
		public static Color4 LightCyan
		{
			get
			{
				return new Color4(224, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000028C2 File Offset: 0x00000AC2
		public static Color4 LightGoldenrodYellow
		{
			get
			{
				return new Color4(250, 250, 210, byte.MaxValue);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000028DD File Offset: 0x00000ADD
		public static Color4 LightGreen
		{
			get
			{
				return new Color4(144, 238, 144, byte.MaxValue);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000028F8 File Offset: 0x00000AF8
		public static Color4 LightGray
		{
			get
			{
				return new Color4(211, 211, 211, byte.MaxValue);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002913 File Offset: 0x00000B13
		public static Color4 LightPink
		{
			get
			{
				return new Color4(byte.MaxValue, 182, 193, byte.MaxValue);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000056 RID: 86 RVA: 0x0000292E File Offset: 0x00000B2E
		public static Color4 LightSalmon
		{
			get
			{
				return new Color4(byte.MaxValue, 160, 122, byte.MaxValue);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002946 File Offset: 0x00000B46
		public static Color4 LightSeaGreen
		{
			get
			{
				return new Color4(32, 178, 170, byte.MaxValue);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000058 RID: 88 RVA: 0x0000295E File Offset: 0x00000B5E
		public static Color4 LightSkyBlue
		{
			get
			{
				return new Color4(135, 206, 250, byte.MaxValue);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002979 File Offset: 0x00000B79
		public static Color4 LightSlateGray
		{
			get
			{
				return new Color4(119, 136, 153, byte.MaxValue);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002991 File Offset: 0x00000B91
		public static Color4 LightSteelBlue
		{
			get
			{
				return new Color4(176, 196, 222, byte.MaxValue);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000029AC File Offset: 0x00000BAC
		public static Color4 LightYellow
		{
			get
			{
				return new Color4(byte.MaxValue, byte.MaxValue, 224, byte.MaxValue);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000029C7 File Offset: 0x00000BC7
		public static Color4 Lime
		{
			get
			{
				return new Color4(0, byte.MaxValue, 0, byte.MaxValue);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000029DA File Offset: 0x00000BDA
		public static Color4 LimeGreen
		{
			get
			{
				return new Color4(50, 205, 50, byte.MaxValue);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000029EF File Offset: 0x00000BEF
		public static Color4 Linen
		{
			get
			{
				return new Color4(250, 240, 230, byte.MaxValue);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002A0A File Offset: 0x00000C0A
		public static Color4 Magenta
		{
			get
			{
				return new Color4(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002A21 File Offset: 0x00000C21
		public static Color4 Maroon
		{
			get
			{
				return new Color4(128, 0, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002A34 File Offset: 0x00000C34
		public static Color4 MediumAquamarine
		{
			get
			{
				return new Color4(102, 205, 170, byte.MaxValue);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002A4C File Offset: 0x00000C4C
		public static Color4 MediumBlue
		{
			get
			{
				return new Color4(0, 0, 205, byte.MaxValue);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002A5F File Offset: 0x00000C5F
		public static Color4 MediumOrchid
		{
			get
			{
				return new Color4(186, 85, 211, byte.MaxValue);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002A77 File Offset: 0x00000C77
		public static Color4 MediumPurple
		{
			get
			{
				return new Color4(147, 112, 219, byte.MaxValue);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002A8F File Offset: 0x00000C8F
		public static Color4 MediumSeaGreen
		{
			get
			{
				return new Color4(60, 179, 113, byte.MaxValue);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002AA4 File Offset: 0x00000CA4
		public static Color4 MediumSlateBlue
		{
			get
			{
				return new Color4(123, 104, 238, byte.MaxValue);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002AB9 File Offset: 0x00000CB9
		public static Color4 MediumSpringGreen
		{
			get
			{
				return new Color4(0, 250, 154, byte.MaxValue);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002AD0 File Offset: 0x00000CD0
		public static Color4 MediumTurquoise
		{
			get
			{
				return new Color4(72, 209, 204, byte.MaxValue);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002AE8 File Offset: 0x00000CE8
		public static Color4 MediumVioletRed
		{
			get
			{
				return new Color4(199, 21, 133, byte.MaxValue);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002B00 File Offset: 0x00000D00
		public static Color4 MidnightBlue
		{
			get
			{
				return new Color4(25, 25, 112, byte.MaxValue);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002B12 File Offset: 0x00000D12
		public static Color4 MintCream
		{
			get
			{
				return new Color4(245, byte.MaxValue, 250, byte.MaxValue);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002B2D File Offset: 0x00000D2D
		public static Color4 MistyRose
		{
			get
			{
				return new Color4(byte.MaxValue, 228, 225, byte.MaxValue);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002B48 File Offset: 0x00000D48
		public static Color4 Moccasin
		{
			get
			{
				return new Color4(byte.MaxValue, 228, 181, byte.MaxValue);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002B63 File Offset: 0x00000D63
		public static Color4 NavajoWhite
		{
			get
			{
				return new Color4(byte.MaxValue, 222, 173, byte.MaxValue);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002B7E File Offset: 0x00000D7E
		public static Color4 Navy
		{
			get
			{
				return new Color4(0, 0, 128, byte.MaxValue);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002B91 File Offset: 0x00000D91
		public static Color4 OldLace
		{
			get
			{
				return new Color4(253, 245, 230, byte.MaxValue);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00002BAC File Offset: 0x00000DAC
		public static Color4 Olive
		{
			get
			{
				return new Color4(128, 128, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002BC3 File Offset: 0x00000DC3
		public static Color4 OliveDrab
		{
			get
			{
				return new Color4(107, 142, 35, byte.MaxValue);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public static Color4 Orange
		{
			get
			{
				return new Color4(byte.MaxValue, 165, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002BEF File Offset: 0x00000DEF
		public static Color4 OrangeRed
		{
			get
			{
				return new Color4(byte.MaxValue, 69, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002C03 File Offset: 0x00000E03
		public static Color4 Orchid
		{
			get
			{
				return new Color4(218, 112, 214, byte.MaxValue);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002C1B File Offset: 0x00000E1B
		public static Color4 PaleGoldenrod
		{
			get
			{
				return new Color4(238, 232, 170, byte.MaxValue);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002C36 File Offset: 0x00000E36
		public static Color4 PaleGreen
		{
			get
			{
				return new Color4(152, 251, 152, byte.MaxValue);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002C51 File Offset: 0x00000E51
		public static Color4 PaleTurquoise
		{
			get
			{
				return new Color4(175, 238, 238, byte.MaxValue);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002C6C File Offset: 0x00000E6C
		public static Color4 PaleVioletRed
		{
			get
			{
				return new Color4(219, 112, 147, byte.MaxValue);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002C84 File Offset: 0x00000E84
		public static Color4 PapayaWhip
		{
			get
			{
				return new Color4(byte.MaxValue, 239, 213, byte.MaxValue);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002C9F File Offset: 0x00000E9F
		public static Color4 PeachPuff
		{
			get
			{
				return new Color4(byte.MaxValue, 218, 185, byte.MaxValue);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002CBA File Offset: 0x00000EBA
		public static Color4 Peru
		{
			get
			{
				return new Color4(205, 133, 63, byte.MaxValue);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002CD2 File Offset: 0x00000ED2
		public static Color4 Pink
		{
			get
			{
				return new Color4(byte.MaxValue, 192, 203, byte.MaxValue);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002CED File Offset: 0x00000EED
		public static Color4 Plum
		{
			get
			{
				return new Color4(221, 160, 221, byte.MaxValue);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002D08 File Offset: 0x00000F08
		public static Color4 PowderBlue
		{
			get
			{
				return new Color4(176, 224, 230, byte.MaxValue);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002D23 File Offset: 0x00000F23
		public static Color4 Purple
		{
			get
			{
				return new Color4(128, 0, 128, byte.MaxValue);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002D3A File Offset: 0x00000F3A
		public static Color4 Red
		{
			get
			{
				return new Color4(byte.MaxValue, 0, 0, byte.MaxValue);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002D4D File Offset: 0x00000F4D
		public static Color4 RosyBrown
		{
			get
			{
				return new Color4(188, 143, 143, byte.MaxValue);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002D68 File Offset: 0x00000F68
		public static Color4 RoyalBlue
		{
			get
			{
				return new Color4(65, 105, 225, byte.MaxValue);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002D7D File Offset: 0x00000F7D
		public static Color4 SaddleBrown
		{
			get
			{
				return new Color4(139, 69, 19, byte.MaxValue);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002D92 File Offset: 0x00000F92
		public static Color4 Salmon
		{
			get
			{
				return new Color4(250, 128, 114, byte.MaxValue);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002DAA File Offset: 0x00000FAA
		public static Color4 SandyBrown
		{
			get
			{
				return new Color4(244, 164, 96, byte.MaxValue);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002DC2 File Offset: 0x00000FC2
		public static Color4 SeaGreen
		{
			get
			{
				return new Color4(46, 139, 87, byte.MaxValue);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00002DD7 File Offset: 0x00000FD7
		public static Color4 SeaShell
		{
			get
			{
				return new Color4(byte.MaxValue, 245, 238, byte.MaxValue);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00002DF2 File Offset: 0x00000FF2
		public static Color4 Sienna
		{
			get
			{
				return new Color4(160, 82, 45, byte.MaxValue);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002E07 File Offset: 0x00001007
		public static Color4 Silver
		{
			get
			{
				return new Color4(192, 192, 192, byte.MaxValue);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002E22 File Offset: 0x00001022
		public static Color4 SkyBlue
		{
			get
			{
				return new Color4(135, 206, 235, byte.MaxValue);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00002E3D File Offset: 0x0000103D
		public static Color4 SlateBlue
		{
			get
			{
				return new Color4(106, 90, 205, byte.MaxValue);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00002E52 File Offset: 0x00001052
		public static Color4 SlateGray
		{
			get
			{
				return new Color4(112, 128, 144, byte.MaxValue);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00002E6A File Offset: 0x0000106A
		public static Color4 Snow
		{
			get
			{
				return new Color4(byte.MaxValue, 250, 250, byte.MaxValue);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002E85 File Offset: 0x00001085
		public static Color4 SpringGreen
		{
			get
			{
				return new Color4(0, byte.MaxValue, 127, byte.MaxValue);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002E99 File Offset: 0x00001099
		public static Color4 SteelBlue
		{
			get
			{
				return new Color4(70, 130, 180, byte.MaxValue);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002EB1 File Offset: 0x000010B1
		public static Color4 Tan
		{
			get
			{
				return new Color4(210, 180, 140, byte.MaxValue);
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00002ECC File Offset: 0x000010CC
		public static Color4 Teal
		{
			get
			{
				return new Color4(0, 128, 128, byte.MaxValue);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002EE3 File Offset: 0x000010E3
		public static Color4 Thistle
		{
			get
			{
				return new Color4(216, 191, 216, byte.MaxValue);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00002EFE File Offset: 0x000010FE
		public static Color4 Tomato
		{
			get
			{
				return new Color4(byte.MaxValue, 99, 71, byte.MaxValue);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00002F13 File Offset: 0x00001113
		public static Color4 Turquoise
		{
			get
			{
				return new Color4(64, 224, 208, byte.MaxValue);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00002F2B File Offset: 0x0000112B
		public static Color4 Violet
		{
			get
			{
				return new Color4(238, 130, 238, byte.MaxValue);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00002F46 File Offset: 0x00001146
		public static Color4 Wheat
		{
			get
			{
				return new Color4(245, 222, 179, byte.MaxValue);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00002F61 File Offset: 0x00001161
		public static Color4 White
		{
			get
			{
				return new Color4(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00002F7C File Offset: 0x0000117C
		public static Color4 WhiteSmoke
		{
			get
			{
				return new Color4(245, 245, 245, byte.MaxValue);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00002F97 File Offset: 0x00001197
		public static Color4 Yellow
		{
			get
			{
				return new Color4(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00002FAE File Offset: 0x000011AE
		public static Color4 YellowGreen
		{
			get
			{
				return new Color4(154, 205, 50, byte.MaxValue);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002FC8 File Offset: 0x000011C8
		public static Color4 FromSrgb(Color4 srgb)
		{
			float r;
			if (srgb.R <= 0.04045f)
			{
				r = srgb.R / 12.92f;
			}
			else
			{
				r = MathF.Pow((srgb.R + 0.055f) / 1.055f, 2.4f);
			}
			float g;
			if (srgb.G <= 0.04045f)
			{
				g = srgb.G / 12.92f;
			}
			else
			{
				g = MathF.Pow((srgb.G + 0.055f) / 1.055f, 2.4f);
			}
			float b;
			if (srgb.B <= 0.04045f)
			{
				b = srgb.B / 12.92f;
			}
			else
			{
				b = MathF.Pow((srgb.B + 0.055f) / 1.055f, 2.4f);
			}
			return new Color4(r, g, b, srgb.A);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003090 File Offset: 0x00001290
		public static Color4 ToSrgb(Color4 rgb)
		{
			float r;
			if ((double)rgb.R <= 0.0031308)
			{
				r = 12.92f * rgb.R;
			}
			else
			{
				r = 1.055f * MathF.Pow(rgb.R, 0.41666666f) - 0.055f;
			}
			float g;
			if ((double)rgb.G <= 0.0031308)
			{
				g = 12.92f * rgb.G;
			}
			else
			{
				g = 1.055f * MathF.Pow(rgb.G, 0.41666666f) - 0.055f;
			}
			float b;
			if ((double)rgb.B <= 0.0031308)
			{
				b = 12.92f * rgb.B;
			}
			else
			{
				b = 1.055f * MathF.Pow(rgb.B, 0.41666666f) - 0.055f;
			}
			return new Color4(r, g, b, rgb.A);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003168 File Offset: 0x00001368
		public static Color4 FromHsl(Vector4 hsl)
		{
			float num = hsl.X * 360f;
			float y = hsl.Y;
			float z = hsl.Z;
			float num2 = (1f - Math.Abs(2f * z - 1f)) * y;
			float num3 = num / 60f;
			float num4 = num2 * (1f - Math.Abs(num3 % 2f - 1f));
			float num5;
			float num6;
			float num7;
			if (num3 >= 0f && num3 < 1f)
			{
				num5 = num2;
				num6 = num4;
				num7 = 0f;
			}
			else if (num3 >= 1f && num3 < 2f)
			{
				num5 = num4;
				num6 = num2;
				num7 = 0f;
			}
			else if (num3 >= 2f && num3 < 3f)
			{
				num5 = 0f;
				num6 = num2;
				num7 = num4;
			}
			else if (num3 >= 3f && num3 < 4f)
			{
				num5 = 0f;
				num6 = num4;
				num7 = num2;
			}
			else if (num3 >= 4f && num3 < 5f)
			{
				num5 = num4;
				num6 = 0f;
				num7 = num2;
			}
			else if (num3 >= 5f && num3 <= 6f)
			{
				num5 = num2;
				num6 = 0f;
				num7 = num4;
			}
			else
			{
				num5 = 0f;
				num6 = 0f;
				num7 = 0f;
			}
			float num8 = z - num2 / 2f;
			if (num8 < 0f)
			{
				num8 = 0f;
			}
			return new Color4(num5 + num8, num6 + num8, num7 + num8, hsl.W);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000032E0 File Offset: 0x000014E0
		public static Vector4 ToHsl(Color4 rgb)
		{
			float num = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
			float num2 = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
			float num3 = num - num2;
			float num4 = 0f;
			if (num3 == 0f)
			{
				num4 = 0f;
			}
			else if (num == rgb.R)
			{
				num4 = (rgb.G - rgb.B) / num3 % 6f;
				if (num4 < 0f)
				{
					num4 += 6f;
				}
			}
			else if (num == rgb.G)
			{
				num4 = (rgb.B - rgb.R) / num3 + 2f;
			}
			else if (num == rgb.B)
			{
				num4 = (rgb.R - rgb.G) / num3 + 4f;
			}
			float num5 = num4 / 6f;
			if (num5 < 0f)
			{
				num5 += 1f;
			}
			float num6 = (num + num2) / 2f;
			float y = 0f;
			if (1f - Math.Abs(2f * num6 - 1f) != 0f)
			{
				y = num3 / (1f - Math.Abs(2f * num6 - 1f));
			}
			return new Vector4(num5, y, num6, rgb.A);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003430 File Offset: 0x00001630
		public static Color4 FromHsv(Vector4 hsv)
		{
			float num = hsv.X * 360f;
			float y = hsv.Y;
			float z = hsv.Z;
			float num2 = z * y;
			float num3 = num / 60f;
			float num4 = num2 * (1f - Math.Abs(num3 % 2f - 1f));
			float num5;
			float num6;
			float num7;
			if (num3 >= 0f && num3 < 1f)
			{
				num5 = num2;
				num6 = num4;
				num7 = 0f;
			}
			else if (num3 >= 1f && num3 < 2f)
			{
				num5 = num4;
				num6 = num2;
				num7 = 0f;
			}
			else if (num3 >= 2f && num3 < 3f)
			{
				num5 = 0f;
				num6 = num2;
				num7 = num4;
			}
			else if (num3 >= 3f && num3 < 4f)
			{
				num5 = 0f;
				num6 = num4;
				num7 = num2;
			}
			else if (num3 >= 4f && num3 < 5f)
			{
				num5 = num4;
				num6 = 0f;
				num7 = num2;
			}
			else if (num3 >= 5f && num3 <= 6f)
			{
				num5 = num2;
				num6 = 0f;
				num7 = num4;
			}
			else
			{
				num5 = 0f;
				num6 = 0f;
				num7 = 0f;
			}
			float num8 = z - num2;
			return new Color4(num5 + num8, num6 + num8, num7 + num8, hsv.W);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003578 File Offset: 0x00001778
		public static Vector4 ToHsv(Color4 rgb)
		{
			float num = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
			float num2 = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
			float num3 = num - num2;
			float num4 = 0f;
			if (num3 == 0f)
			{
				num4 = 0f;
			}
			else if (num == rgb.R)
			{
				num4 = (rgb.G - rgb.B) / num3 % 6f;
				if (num4 < 0f)
				{
					num4 += 6f;
				}
			}
			else if (num == rgb.G)
			{
				num4 = (rgb.B - rgb.R) / num3 + 2f;
			}
			else if (num == rgb.B)
			{
				num4 = (rgb.R - rgb.G) / num3 + 4f;
			}
			float x = num4 * 60f / 360f;
			float y = 0f;
			if (num != 0f)
			{
				y = num3 / num;
			}
			return new Vector4(x, y, num, rgb.A);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000367C File Offset: 0x0000187C
		public static Color4 FromXyz(Vector4 xyz)
		{
			float r = 0.41847f * xyz.X + -0.15866f * xyz.Y + -0.082835f * xyz.Z;
			float g = -0.091169f * xyz.X + 0.25243f * xyz.Y + 0.015708f * xyz.Z;
			float b = 0.0009209f * xyz.X + -0.0025498f * xyz.Y + 0.1786f * xyz.Z;
			return new Color4(r, g, b, xyz.W);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000370C File Offset: 0x0000190C
		public static Vector4 ToXyz(Color4 rgb)
		{
			float x = (0.49f * rgb.R + 0.31f * rgb.G + 0.2f * rgb.B) / 0.17697f;
			float y = (0.17697f * rgb.R + 0.8124f * rgb.G + 0.01063f * rgb.B) / 0.17697f;
			float z = (0f * rgb.R + 0.01f * rgb.G + 0.99f * rgb.B) / 0.17697f;
			return new Vector4(x, y, z, rgb.A);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000037AC File Offset: 0x000019AC
		public static Color4 FromYcbcr(Vector4 ycbcr)
		{
			float r = 1f * ycbcr.X + 0f * ycbcr.Y + 1.402f * ycbcr.Z;
			float g = 1f * ycbcr.X + -0.344136f * ycbcr.Y + -0.714136f * ycbcr.Z;
			float b = 1f * ycbcr.X + 1.772f * ycbcr.Y + 0f * ycbcr.Z;
			return new Color4(r, g, b, ycbcr.W);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000383C File Offset: 0x00001A3C
		public static Vector4 ToYcbcr(Color4 rgb)
		{
			float x = 0.299f * rgb.R + 0.587f * rgb.G + 0.114f * rgb.B;
			float y = -0.168736f * rgb.R + -0.331264f * rgb.G + 0.5f * rgb.B;
			float z = 0.5f * rgb.R + -0.418688f * rgb.G + -0.081312f * rgb.B;
			return new Vector4(x, y, z, rgb.A);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000038CC File Offset: 0x00001ACC
		public static Color4 FromHcy(Vector4 hcy)
		{
			float num = hcy.X * 360f;
			float y = hcy.Y;
			float z = hcy.Z;
			float num2 = num / 60f;
			float num3 = y * (1f - Math.Abs(num2 % 2f - 1f));
			float num4;
			float num5;
			float num6;
			if (num2 >= 0f && num2 < 1f)
			{
				num4 = y;
				num5 = num3;
				num6 = 0f;
			}
			else if (num2 >= 1f && num2 < 2f)
			{
				num4 = num3;
				num5 = y;
				num6 = 0f;
			}
			else if (num2 >= 2f && num2 < 3f)
			{
				num4 = 0f;
				num5 = y;
				num6 = num3;
			}
			else if (num2 >= 3f && num2 < 4f)
			{
				num4 = 0f;
				num5 = num3;
				num6 = y;
			}
			else if (num2 >= 4f && num2 < 5f)
			{
				num4 = num3;
				num5 = 0f;
				num6 = y;
			}
			else if (num2 >= 5f && num2 < 6f)
			{
				num4 = y;
				num5 = 0f;
				num6 = num3;
			}
			else
			{
				num4 = 0f;
				num5 = 0f;
				num6 = 0f;
			}
			float num7 = z - 0.3f * num4 + 0.59f * num5 + 0.11f * num6;
			return new Color4(num4 + num7, num5 + num7, num6 + num7, hcy.W);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003A24 File Offset: 0x00001C24
		public static Vector4 ToHcy(Color4 rgb)
		{
			float num = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
			float num2 = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
			float num3 = num - num2;
			float num4 = 0f;
			if (num == rgb.R)
			{
				num4 = (rgb.G - rgb.B) / num3 % 6f;
			}
			else if (num == rgb.G)
			{
				num4 = (rgb.B - rgb.R) / num3 + 2f;
			}
			else if (num == rgb.B)
			{
				num4 = (rgb.R - rgb.G) / num3 + 4f;
			}
			float x = num4 * 60f / 360f;
			float z = 0.3f * rgb.R + 0.59f * rgb.G + 0.11f * rgb.B;
			return new Vector4(x, num3, z, rgb.A);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003B19 File Offset: 0x00001D19
		public readonly bool Equals(Color4 other)
		{
			return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
		}

		// Token: 0x04000001 RID: 1
		public float R;

		// Token: 0x04000002 RID: 2
		public float G;

		// Token: 0x04000003 RID: 3
		public float B;

		// Token: 0x04000004 RID: 4
		public float A;
	}
}
