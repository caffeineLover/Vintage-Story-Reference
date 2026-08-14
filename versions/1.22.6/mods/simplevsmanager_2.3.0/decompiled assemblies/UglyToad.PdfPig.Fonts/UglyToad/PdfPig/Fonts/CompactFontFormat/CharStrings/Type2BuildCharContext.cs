using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings
{
	// Token: 0x0200009F RID: 159
	internal class Type2BuildCharContext
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0001A4E8 File Offset: 0x000186E8
		public CharStringStack Stack { get; } = new CharStringStack();

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001A4F0 File Offset: 0x000186F0
		public List<PdfSubpath> Path { get; } = new List<PdfSubpath>();

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0001A4F8 File Offset: 0x000186F8
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0001A500 File Offset: 0x00018700
		public PdfPoint CurrentLocation { get; set; } = new PdfPoint(0, 0);

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0001A509 File Offset: 0x00018709
		// (set) Token: 0x060003CA RID: 970 RVA: 0x0001A511 File Offset: 0x00018711
		public double? Width { get; set; }

		// Token: 0x060003CB RID: 971 RVA: 0x0001A51A File Offset: 0x0001871A
		public void AddRelativeHorizontalLine(double dx)
		{
			this.AddRelativeLine(dx, 0.0);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001A52C File Offset: 0x0001872C
		public void AddRelativeVerticalLine(double dy)
		{
			this.AddRelativeLine(0.0, dy);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001A540 File Offset: 0x00018740
		public void AddRelativeMoveTo(double dx, double dy)
		{
			this.BeforeMoveTo();
			PdfPoint currentLocation;
			currentLocation..ctor(this.CurrentLocation.X + dx, this.CurrentLocation.Y + dy);
			this.Path[this.Path.Count - 1].MoveTo(currentLocation.X, currentLocation.Y);
			this.CurrentLocation = currentLocation;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001A5AC File Offset: 0x000187AC
		public void AddHorizontalMoveTo(double dx)
		{
			this.BeforeMoveTo();
			this.Path[this.Path.Count - 1].MoveTo(this.CurrentLocation.X + dx, this.CurrentLocation.Y);
			this.CurrentLocation = this.CurrentLocation.MoveX(dx);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001A610 File Offset: 0x00018810
		public void AddVerticallMoveTo(double dy)
		{
			this.BeforeMoveTo();
			this.Path[this.Path.Count - 1].MoveTo(this.CurrentLocation.X, this.CurrentLocation.Y + dy);
			this.CurrentLocation = this.CurrentLocation.MoveY(dy);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001A674 File Offset: 0x00018874
		public void AddRelativeBezierCurve(double dx1, double dy1, double dx2, double dy2, double dx3, double dy3)
		{
			double num = this.CurrentLocation.X + dx1;
			double num2 = this.CurrentLocation.Y + dy1;
			double num3 = num + dx2;
			double num4 = num2 + dy2;
			double num5 = num3 + dx3;
			double num6 = num4 + dy3;
			this.Path[this.Path.Count - 1].BezierCurveTo(num, num2, num3, num4, num5, num6);
			this.CurrentLocation = new PdfPoint(num5, num6);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001A6F0 File Offset: 0x000188F0
		public void AddRelativeLine(double dx, double dy)
		{
			PdfPoint currentLocation;
			currentLocation..ctor(this.CurrentLocation.X + dx, this.CurrentLocation.Y + dy);
			this.Path[this.Path.Count - 1].LineTo(currentLocation.X, currentLocation.Y);
			this.CurrentLocation = currentLocation;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001A756 File Offset: 0x00018956
		public void AddVerticalStemHints([TupleElementNames(new string[]
		{
			"start",
			"end"
		})] IReadOnlyList<ValueTuple<double, double>> hints)
		{
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001A758 File Offset: 0x00018958
		public void AddHorizontalStemHints([TupleElementNames(new string[]
		{
			"start",
			"end"
		})] IReadOnlyList<ValueTuple<double, double>> hints)
		{
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001A75A File Offset: 0x0001895A
		public void AddToTransientArray(double value, int location)
		{
			this.transientArray[location] = value;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001A769 File Offset: 0x00018969
		private void BeforeMoveTo()
		{
			if (this.Path.Count > 0)
			{
				this.Path[this.Path.Count - 1].CloseSubpath();
			}
			this.Path.Add(new PdfSubpath());
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001A7A6 File Offset: 0x000189A6
		public double GetFromTransientArray(int location)
		{
			double result = this.transientArray[location];
			this.transientArray.Remove(location);
			return result;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001A7C1 File Offset: 0x000189C1
		public static int CountToBias(int count)
		{
			if (count < 1240)
			{
				return 107;
			}
			if (count < 33900)
			{
				return 1131;
			}
			return 32768;
		}

		// Token: 0x0400035C RID: 860
		private readonly Dictionary<int, double> transientArray = new Dictionary<int, double>();
	}
}
