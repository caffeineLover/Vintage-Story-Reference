using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace ModernWpf
{
	// Token: 0x0200020C RID: 524
	internal readonly struct DpiScale2 : IEquatable<DpiScale2>
	{
		// Token: 0x06000F6D RID: 3949 RVA: 0x0002C036 File Offset: 0x0002A236
		public DpiScale2(double dpiScaleX, double dpiScaleY)
		{
			this.DpiScaleX = dpiScaleX;
			this.DpiScaleY = dpiScaleY;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0002C046 File Offset: 0x0002A246
		public DpiScale2(DpiScale dpiScale)
		{
			this = new DpiScale2(dpiScale.DpiScaleX, dpiScale.DpiScaleY);
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0002C05C File Offset: 0x0002A25C
		public double DpiScaleX { get; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0002C064 File Offset: 0x0002A264
		public double DpiScaleY { get; }

		// Token: 0x06000F71 RID: 3953 RVA: 0x0002C06C File Offset: 0x0002A26C
		[CompilerGenerated]
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DpiScale2");
			stringBuilder.Append(" { ");
			if (this.PrintMembers(stringBuilder))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0002C0B8 File Offset: 0x0002A2B8
		[CompilerGenerated]
		private bool PrintMembers(StringBuilder builder)
		{
			builder.Append("DpiScaleX = ");
			builder.Append(this.DpiScaleX.ToString());
			builder.Append(", DpiScaleY = ");
			builder.Append(this.DpiScaleY.ToString());
			return true;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0002C114 File Offset: 0x0002A314
		[CompilerGenerated]
		public static bool operator !=(DpiScale2 left, DpiScale2 right)
		{
			return !(left == right);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0002C120 File Offset: 0x0002A320
		[CompilerGenerated]
		public static bool operator ==(DpiScale2 left, DpiScale2 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0002C12A File Offset: 0x0002A32A
		[CompilerGenerated]
		public override int GetHashCode()
		{
			return EqualityComparer<double>.Default.GetHashCode(this.<DpiScaleX>k__BackingField) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<DpiScaleY>k__BackingField);
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0002C153 File Offset: 0x0002A353
		[CompilerGenerated]
		public override bool Equals(object obj)
		{
			return obj is DpiScale2 && this.Equals((DpiScale2)obj);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002C16B File Offset: 0x0002A36B
		[CompilerGenerated]
		public bool Equals(DpiScale2 other)
		{
			return EqualityComparer<double>.Default.Equals(this.<DpiScaleX>k__BackingField, other.<DpiScaleX>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<DpiScaleY>k__BackingField, other.<DpiScaleY>k__BackingField);
		}
	}
}
