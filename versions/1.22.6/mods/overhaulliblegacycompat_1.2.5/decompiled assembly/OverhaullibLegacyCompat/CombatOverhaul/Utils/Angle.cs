using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000021 RID: 33
	public readonly struct Angle
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009A1B File Offset: 0x00007C1B
		public float Radians
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00009A23 File Offset: 0x00007C23
		public float Degrees
		{
			get
			{
				return this._value * 57.295776f;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00009A31 File Offset: 0x00007C31
		public float Minutes
		{
			get
			{
				return this._value * 57.295776f * 60f;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00009A45 File Offset: 0x00007C45
		public float Seconds
		{
			get
			{
				return this._value * 57.295776f * 3600f;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00009A5C File Offset: 0x00007C5C
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 1);
			defaultInterpolatedStringHandler.AppendFormatted<float>(this._value * 57.295776f, "F2");
			defaultInterpolatedStringHandler.AppendLiteral(" deg");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00009AA0 File Offset: 0x00007CA0
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			Angle? angle;
			float? num = ((Angle?)obj != null) ? new float?(angle.GetValueOrDefault()._value) : default(float?);
			float value = this._value;
			return num.GetValueOrDefault() == value & num != null;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00009AF3 File Offset: 0x00007CF3
		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000249 RID: 585 RVA: 0x00009B00 File Offset: 0x00007D00
		public static Angle Zero
		{
			get
			{
				return new Angle(0f);
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009B0C File Offset: 0x00007D0C
		public static Angle FromRadians(float radians)
		{
			return new Angle(radians);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009B14 File Offset: 0x00007D14
		public static Angle FromDegrees(float degrees)
		{
			return new Angle(degrees * 0.017453292f);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00009B22 File Offset: 0x00007D22
		public static Angle FromMinutes(float minutes)
		{
			return new Angle(minutes * 0.017453292f / 60f);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009B36 File Offset: 0x00007D36
		public static Angle FromSeconds(float seconds)
		{
			return new Angle(seconds * 0.017453292f / 3600f);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009B4A File Offset: 0x00007D4A
		public static Angle FromRadians(double radians)
		{
			return Angle.FromRadians((float)radians);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009B53 File Offset: 0x00007D53
		public static Angle FromDegrees(double degrees)
		{
			return Angle.FromDegrees((float)degrees);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009B5C File Offset: 0x00007D5C
		public static Angle FromMinutes(double minutes)
		{
			return Angle.FromMinutes((float)minutes);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009B65 File Offset: 0x00007D65
		public static Angle FromSeconds(double seconds)
		{
			return Angle.FromSeconds((float)seconds);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009B70 File Offset: 0x00007D70
		public static Angle BetweenVectors(Vector3d a, Vector3d b)
		{
			double dot = Vector3d.Dot(a, b);
			double magProduct = a.Length * b.Length;
			if (magProduct == 0.0)
			{
				return Angle.FromRadians(0f);
			}
			return Angle.FromRadians((float)Math.Acos(Math.Clamp(dot / magProduct, -1.0, 1.0)));
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00009BD4 File Offset: 0x00007DD4
		public static Angle BetweenVectors(Vector3 a, Vector3 b)
		{
			float dot = Vector3.Dot(a, b);
			float magProduct = a.Length * b.Length;
			if (magProduct == 0f)
			{
				return Angle.FromRadians(0f);
			}
			return Angle.FromRadians(MathF.Acos(Math.Clamp(dot / magProduct, -1f, 1f)));
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009C28 File Offset: 0x00007E28
		public static Angle operator +(Angle a, Angle b)
		{
			return new Angle(a._value + b._value);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00009C3C File Offset: 0x00007E3C
		public static Angle operator -(Angle a, Angle b)
		{
			return new Angle(a._value - b._value);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00009C50 File Offset: 0x00007E50
		public static Angle operator *(Angle a, float b)
		{
			return new Angle(a._value * b);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00009C5F File Offset: 0x00007E5F
		public static Angle operator *(float a, Angle b)
		{
			return new Angle(a * b._value);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009C6E File Offset: 0x00007E6E
		public static Angle operator /(Angle a, float b)
		{
			return new Angle(a._value / b);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00009C7D File Offset: 0x00007E7D
		public static float operator /(Angle a, Angle b)
		{
			return a._value / b._value;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009C8C File Offset: 0x00007E8C
		public static bool operator ==(Angle a, Angle b)
		{
			return MathF.Abs(a._value - b._value) < Angle.Epsilon(a._value, b._value);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009CB3 File Offset: 0x00007EB3
		public static bool operator !=(Angle a, Angle b)
		{
			return MathF.Abs(a._value - b._value) >= Angle.Epsilon(a._value, b._value);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009CDD File Offset: 0x00007EDD
		public static bool operator <(Angle a, Angle b)
		{
			return a._value < b._value && a != b;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009CF6 File Offset: 0x00007EF6
		public static bool operator >(Angle a, Angle b)
		{
			return a._value > b._value && a != b;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009D0F File Offset: 0x00007F0F
		public static bool operator <=(Angle a, Angle b)
		{
			return a._value <= b._value;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009D22 File Offset: 0x00007F22
		public static bool operator >=(Angle a, Angle b)
		{
			return a._value >= b._value;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009D35 File Offset: 0x00007F35
		private Angle(float radians)
		{
			this._value = radians;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009D3E File Offset: 0x00007F3E
		private static float Epsilon(float a, float b)
		{
			return MathF.Max(Math.Abs(a), Math.Abs(b)) * 1E-10f;
		}

		// Token: 0x040000FB RID: 251
		private readonly float _value;

		// Token: 0x040000FC RID: 252
		private const float _epsilonFactor = 1E-10f;
	}
}
