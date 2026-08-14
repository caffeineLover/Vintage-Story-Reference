using System;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	public struct Quaterniond : IEquatable<Quaterniond>, IFormattable
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00004E17 File Offset: 0x00003017
		public Quaterniond(Vector3d v, double w)
		{
			this.Xyz = v;
			this.W = w;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004E27 File Offset: 0x00003027
		public Quaterniond(double x, double y, double z, double w)
		{
			this = new Quaterniond(new Vector3d(x, y, z), w);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004E3C File Offset: 0x0000303C
		public Quaterniond(double pitch, double yaw, double roll)
		{
			yaw *= 0.5;
			pitch *= 0.5;
			roll *= 0.5;
			double num = Math.Cos(yaw);
			double num2 = Math.Cos(pitch);
			double num3 = Math.Cos(roll);
			double num4 = Math.Sin(yaw);
			double num5 = Math.Sin(pitch);
			double num6 = Math.Sin(roll);
			this.W = num * num2 * num3 - num4 * num5 * num6;
			this.Xyz.X = num4 * num2 * num3 + num * num5 * num6;
			this.Xyz.Y = num * num5 * num3 - num4 * num2 * num6;
			this.Xyz.Z = num * num2 * num6 + num4 * num5 * num3;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004EF7 File Offset: 0x000030F7
		public Quaterniond(Vector3d eulerAngles)
		{
			this = new Quaterniond(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00004F11 File Offset: 0x00003111
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00004F1E File Offset: 0x0000311E
		[XmlIgnore]
		public double X
		{
			readonly get
			{
				return this.Xyz.X;
			}
			set
			{
				this.Xyz.X = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00004F2C File Offset: 0x0000312C
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00004F39 File Offset: 0x00003139
		[XmlIgnore]
		public double Y
		{
			readonly get
			{
				return this.Xyz.Y;
			}
			set
			{
				this.Xyz.Y = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00004F47 File Offset: 0x00003147
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00004F54 File Offset: 0x00003154
		[XmlIgnore]
		public double Z
		{
			readonly get
			{
				return this.Xyz.Z;
			}
			set
			{
				this.Xyz.Z = value;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004F64 File Offset: 0x00003164
		public void ToAxisAngle(out Vector3d axis, out double angle)
		{
			Vector4d vector4d = this.ToAxisAngle();
			axis = vector4d.Xyz;
			angle = vector4d.W;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004F90 File Offset: 0x00003190
		public Vector4d ToAxisAngle()
		{
			Quaterniond quaterniond = this;
			if (Math.Abs(quaterniond.W) > 1.0)
			{
				quaterniond.Normalize();
			}
			Vector4d result = new Vector4d
			{
				W = 2.0 * Math.Acos(quaterniond.W)
			};
			double num = Math.Sqrt(1.0 - quaterniond.W * quaterniond.W);
			if (num > 0.0001)
			{
				result.Xyz = quaterniond.Xyz / num;
			}
			else
			{
				result.Xyz = Vector3d.UnitX;
			}
			return result;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005032 File Offset: 0x00003232
		public readonly void ToEulerAngles(out Vector3d angles)
		{
			angles = this.ToEulerAngles();
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005040 File Offset: 0x00003240
		public readonly Vector3d ToEulerAngles()
		{
			Quaterniond quaterniond = this;
			double num = quaterniond.W * quaterniond.W;
			double num2 = quaterniond.X * quaterniond.X;
			double num3 = quaterniond.Y * quaterniond.Y;
			double num4 = quaterniond.Z * quaterniond.Z;
			double num5 = num2 + num3 + num4 + num;
			double num6 = quaterniond.X * quaterniond.Z + quaterniond.W * quaterniond.Y;
			Vector3d result;
			if (num6 > 0.4999995 * num5)
			{
				result.Z = 2.0 * Math.Atan2(quaterniond.X, quaterniond.W);
				result.Y = 1.5707963705062866;
				result.X = 0.0;
			}
			else if (num6 < -0.4999995 * num5)
			{
				result.Z = -2.0 * Math.Atan2(quaterniond.X, quaterniond.W);
				result.Y = -1.5707963705062866;
				result.X = 0.0;
			}
			else
			{
				result.Z = Math.Atan2(2.0 * (quaterniond.W * quaterniond.Z - quaterniond.X * quaterniond.Y), num + num2 - num3 - num4);
				result.Y = Math.Asin(2.0 * num6 / num5);
				result.X = Math.Atan2(2.0 * (quaterniond.W * quaterniond.X - quaterniond.Y * quaterniond.Z), num - num2 - num3 + num4);
			}
			return result;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005205 File Offset: 0x00003405
		public readonly double Length
		{
			get
			{
				return Math.Sqrt(this.W * this.W + this.Xyz.LengthSquared);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005225 File Offset: 0x00003425
		public readonly double LengthSquared
		{
			get
			{
				return this.W * this.W + this.Xyz.LengthSquared;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005240 File Offset: 0x00003440
		public readonly Quaterniond Normalized()
		{
			Quaterniond result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000525C File Offset: 0x0000345C
		public void Invert()
		{
			Quaterniond.Invert(this, out this);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005268 File Offset: 0x00003468
		public readonly Quaterniond Inverted()
		{
			Quaterniond result = this;
			result.Invert();
			return result;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005284 File Offset: 0x00003484
		public void Normalize()
		{
			double num = 1.0 / this.Length;
			this.Xyz *= num;
			this.W *= num;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000052C2 File Offset: 0x000034C2
		public void Conjugate()
		{
			this.Xyz = -this.Xyz;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000052D5 File Offset: 0x000034D5
		public static Quaterniond Add(Quaterniond left, Quaterniond right)
		{
			return new Quaterniond(left.Xyz + right.Xyz, left.W + right.W);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000052FA File Offset: 0x000034FA
		public static void Add(in Quaterniond left, in Quaterniond right, out Quaterniond result)
		{
			result = new Quaterniond(left.Xyz + right.Xyz, left.W + right.W);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005325 File Offset: 0x00003525
		public static Quaterniond Sub(Quaterniond left, Quaterniond right)
		{
			return new Quaterniond(left.Xyz - right.Xyz, left.W - right.W);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000534A File Offset: 0x0000354A
		public static void Sub(in Quaterniond left, in Quaterniond right, out Quaterniond result)
		{
			result = new Quaterniond(left.Xyz - right.Xyz, left.W - right.W);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005378 File Offset: 0x00003578
		public static Quaterniond Multiply(Quaterniond left, Quaterniond right)
		{
			Quaterniond result;
			Quaterniond.Multiply(left, right, out result);
			return result;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005394 File Offset: 0x00003594
		public static void Multiply(in Quaterniond left, in Quaterniond right, out Quaterniond result)
		{
			result = new Quaterniond(right.W * left.Xyz + left.W * right.Xyz + Vector3d.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3d.Dot(left.Xyz, right.Xyz));
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005408 File Offset: 0x00003608
		public static void Multiply(in Quaterniond quaternion, double scale, out Quaterniond result)
		{
			result = new Quaterniond(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005435 File Offset: 0x00003635
		public static Quaterniond Multiply(Quaterniond quaternion, double scale)
		{
			return new Quaterniond(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000545F File Offset: 0x0000365F
		public static Quaterniond Conjugate(Quaterniond q)
		{
			return new Quaterniond(-q.Xyz, q.W);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005477 File Offset: 0x00003677
		public static void Conjugate(in Quaterniond q, out Quaterniond result)
		{
			result = new Quaterniond(-q.Xyz, q.W);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005498 File Offset: 0x00003698
		public static Quaterniond Invert(Quaterniond q)
		{
			Quaterniond result;
			Quaterniond.Invert(q, out result);
			return result;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000054B0 File Offset: 0x000036B0
		public static void Invert(in Quaterniond q, out Quaterniond result)
		{
			double lengthSquared = q.LengthSquared;
			if (lengthSquared != 0.0)
			{
				double num = 1.0 / lengthSquared;
				result = new Quaterniond(q.Xyz * -num, q.W * num);
				return;
			}
			result = q;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000550C File Offset: 0x0000370C
		public static Quaterniond Normalize(Quaterniond q)
		{
			Quaterniond result;
			Quaterniond.Normalize(q, out result);
			return result;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005524 File Offset: 0x00003724
		public static void Normalize(in Quaterniond q, out Quaterniond result)
		{
			double num = 1.0 / q.Length;
			result = new Quaterniond(q.Xyz * num, q.W * num);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005564 File Offset: 0x00003764
		public static Quaterniond FromAxisAngle(Vector3d axis, double angle)
		{
			if (axis.LengthSquared == 0.0)
			{
				return Quaterniond.Identity;
			}
			Quaterniond identity = Quaterniond.Identity;
			angle *= 0.5;
			axis.Normalize();
			identity.Xyz = axis * Math.Sin(angle);
			identity.W = Math.Cos(angle);
			return Quaterniond.Normalize(identity);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000055C9 File Offset: 0x000037C9
		public static Quaterniond FromEulerAngles(double pitch, double yaw, double roll)
		{
			return new Quaterniond(pitch, yaw, roll);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000055D3 File Offset: 0x000037D3
		public static Quaterniond FromEulerAngles(Vector3d eulerAngles)
		{
			return new Quaterniond(eulerAngles);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000055DC File Offset: 0x000037DC
		public static void FromEulerAngles(in Vector3d eulerAngles, out Quaterniond result)
		{
			double num = Math.Cos(eulerAngles.Y * 0.5);
			double num2 = Math.Cos(eulerAngles.X * 0.5);
			double num3 = Math.Cos(eulerAngles.Z * 0.5);
			double num4 = Math.Sin(eulerAngles.Y * 0.5);
			double num5 = Math.Sin(eulerAngles.X * 0.5);
			double num6 = Math.Sin(eulerAngles.Z * 0.5);
			result.W = num * num2 * num3 - num4 * num5 * num6;
			result.Xyz.X = num4 * num5 * num3 + num * num2 * num6;
			result.Xyz.Y = num4 * num2 * num3 + num * num5 * num6;
			result.Xyz.Z = num * num5 * num3 - num4 * num2 * num6;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000056CA File Offset: 0x000038CA
		public static void ToEulerAngles(in Quaterniond q, out Vector3d result)
		{
			q.ToEulerAngles(out result);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000056D4 File Offset: 0x000038D4
		public static Quaterniond FromMatrix(Matrix3d matrix)
		{
			Quaterniond result;
			Quaterniond.FromMatrix(matrix, out result);
			return result;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000056EC File Offset: 0x000038EC
		public static void FromMatrix(in Matrix3d matrix, out Quaterniond result)
		{
			double trace = matrix.Trace;
			if (trace > 0.0)
			{
				double num = Math.Sqrt(trace + 1.0) * 2.0;
				double num2 = 1.0 / num;
				result.W = num * 0.25;
				result.Xyz.X = (matrix.Row2.Y - matrix.Row1.Z) * num2;
				result.Xyz.Y = (matrix.Row0.Z - matrix.Row2.X) * num2;
				result.Xyz.Z = (matrix.Row1.X - matrix.Row0.Y) * num2;
				return;
			}
			double x = matrix.Row0.X;
			double y = matrix.Row1.Y;
			double z = matrix.Row2.Z;
			if (x > y && x > z)
			{
				double num3 = Math.Sqrt(1.0 + x - y - z) * 2.0;
				double num4 = 1.0 / num3;
				result.W = (matrix.Row2.Y - matrix.Row1.Z) * num4;
				result.Xyz.X = num3 * 0.25;
				result.Xyz.Y = (matrix.Row0.Y + matrix.Row1.X) * num4;
				result.Xyz.Z = (matrix.Row0.Z + matrix.Row2.X) * num4;
				return;
			}
			if (y > z)
			{
				double num5 = Math.Sqrt(1.0 + y - x - z) * 2.0;
				double num6 = 1.0 / num5;
				result.W = (matrix.Row0.Z - matrix.Row2.X) * num6;
				result.Xyz.X = (matrix.Row0.Y + matrix.Row1.X) * num6;
				result.Xyz.Y = num5 * 0.25;
				result.Xyz.Z = (matrix.Row1.Z + matrix.Row2.Y) * num6;
				return;
			}
			double num7 = Math.Sqrt(1.0 + z - x - y) * 2.0;
			double num8 = 1.0 / num7;
			result.W = (matrix.Row1.X - matrix.Row0.Y) * num8;
			result.Xyz.X = (matrix.Row0.Z + matrix.Row2.X) * num8;
			result.Xyz.Y = (matrix.Row1.Z + matrix.Row2.Y) * num8;
			result.Xyz.Z = num7 * 0.25;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005A08 File Offset: 0x00003C08
		public static Quaterniond Slerp(Quaterniond q1, Quaterniond q2, double blend)
		{
			if (q1.LengthSquared == 0.0)
			{
				if (q2.LengthSquared == 0.0)
				{
					return Quaterniond.Identity;
				}
				return q2;
			}
			else
			{
				if (q2.LengthSquared == 0.0)
				{
					return q1;
				}
				double num = q1.W * q2.W + Vector3d.Dot(q1.Xyz, q2.Xyz);
				if (num >= 1.0 || num <= -1.0)
				{
					return q1;
				}
				if (num < 0.0)
				{
					q2.Xyz = -q2.Xyz;
					q2.W = -q2.W;
					num = -num;
				}
				double num5;
				double num6;
				if (num < 0.99)
				{
					double num2 = Math.Acos(num);
					double num3 = Math.Sin(num2);
					double num4 = 1.0 / num3;
					num5 = Math.Sin(num2 * (1.0 - blend)) * num4;
					num6 = Math.Sin(num2 * blend) * num4;
				}
				else
				{
					num5 = 1.0 - blend;
					num6 = blend;
				}
				Quaterniond q3 = new Quaterniond(num5 * q1.Xyz + num6 * q2.Xyz, num5 * q1.W + num6 * q2.W);
				if (q3.LengthSquared > 0.0)
				{
					return Quaterniond.Normalize(q3);
				}
				return Quaterniond.Identity;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005B6A File Offset: 0x00003D6A
		public static Quaterniond operator +(Quaterniond left, Quaterniond right)
		{
			left.Xyz += right.Xyz;
			left.W += right.W;
			return left;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005B9B File Offset: 0x00003D9B
		public static Quaterniond operator -(Quaterniond left, Quaterniond right)
		{
			left.Xyz -= right.Xyz;
			left.W -= right.W;
			return left;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005BCC File Offset: 0x00003DCC
		public static Quaterniond operator *(Quaterniond left, Quaterniond right)
		{
			Quaterniond.Multiply(left, right, out left);
			return left;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005BDA File Offset: 0x00003DDA
		public static Quaterniond operator *(Quaterniond quaternion, double scale)
		{
			Quaterniond.Multiply(quaternion, scale, out quaternion);
			return quaternion;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005BE7 File Offset: 0x00003DE7
		public static Quaterniond operator *(double scale, Quaterniond quaternion)
		{
			return new Quaterniond(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005C11 File Offset: 0x00003E11
		public static bool operator ==(Quaterniond left, Quaterniond right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005C1B File Offset: 0x00003E1B
		public static bool operator !=(Quaterniond left, Quaterniond right)
		{
			return !(left == right);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005C27 File Offset: 0x00003E27
		public override readonly bool Equals(object obj)
		{
			return obj is Quaterniond && this.Equals((Quaterniond)obj);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005C3F File Offset: 0x00003E3F
		public readonly bool Equals(Quaterniond other)
		{
			return this.Xyz.Equals(other.Xyz) && this.W == other.W;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005C64 File Offset: 0x00003E64
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3d, double>(this.Xyz, this.W);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005C77 File Offset: 0x00003E77
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005C81 File Offset: 0x00003E81
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005C8B File Offset: 0x00003E8B
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005C98 File Offset: 0x00003E98
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			string listSeparator = MathHelper.GetListSeparator(formatProvider);
			string text = this.Xyz.ToString(format, formatProvider);
			string text2 = this.W.ToString(format, formatProvider);
			return string.Concat(new string[]
			{
				"V: ",
				text,
				listSeparator,
				" W: ",
				text2
			});
		}

		// Token: 0x0400000E RID: 14
		public Vector3d Xyz;

		// Token: 0x0400000F RID: 15
		public double W;

		// Token: 0x04000010 RID: 16
		public static readonly Quaterniond Identity = new Quaterniond(0.0, 0.0, 0.0, 1.0);
	}
}
