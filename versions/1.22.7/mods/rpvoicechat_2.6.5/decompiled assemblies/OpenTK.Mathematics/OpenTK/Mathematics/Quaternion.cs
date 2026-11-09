using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public struct Quaternion : IEquatable<Quaternion>, IFormattable
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00003FE8 File Offset: 0x000021E8
		public Quaternion(Vector3 v, float w)
		{
			this.Xyz = v;
			this.W = w;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00003FF8 File Offset: 0x000021F8
		public Quaternion(float x, float y, float z, float w)
		{
			this = new Quaternion(new Vector3(x, y, z), w);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000400C File Offset: 0x0000220C
		public Quaternion(float rotationX, float rotationY, float rotationZ)
		{
			rotationX *= 0.5f;
			rotationY *= 0.5f;
			rotationZ *= 0.5f;
			float num = MathF.Cos(rotationX);
			float num2 = MathF.Cos(rotationY);
			float num3 = MathF.Cos(rotationZ);
			float num4 = MathF.Sin(rotationX);
			float num5 = MathF.Sin(rotationY);
			float num6 = MathF.Sin(rotationZ);
			this.W = num * num2 * num3 - num4 * num5 * num6;
			this.Xyz.X = num4 * num2 * num3 + num * num5 * num6;
			this.Xyz.Y = num * num5 * num3 - num4 * num2 * num6;
			this.Xyz.Z = num * num2 * num6 + num4 * num5 * num3;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000040BB File Offset: 0x000022BB
		public Quaternion(Vector3 eulerAngles)
		{
			this = new Quaternion(eulerAngles.X, eulerAngles.Y, eulerAngles.Z);
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000040D5 File Offset: 0x000022D5
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x000040E2 File Offset: 0x000022E2
		[XmlIgnore]
		public float X
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

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000040F0 File Offset: 0x000022F0
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x000040FD File Offset: 0x000022FD
		[XmlIgnore]
		public float Y
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

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000410B File Offset: 0x0000230B
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00004118 File Offset: 0x00002318
		[XmlIgnore]
		public float Z
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

		// Token: 0x060000D7 RID: 215 RVA: 0x00004128 File Offset: 0x00002328
		public void ToAxisAngle(out Vector3 axis, out float angle)
		{
			Vector4 vector = this.ToAxisAngle();
			axis = vector.Xyz;
			angle = vector.W;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004154 File Offset: 0x00002354
		public Vector4 ToAxisAngle()
		{
			Quaternion quaternion = this;
			if (Math.Abs(quaternion.W) > 1f)
			{
				quaternion.Normalize();
			}
			Vector4 result = new Vector4
			{
				W = 2f * MathF.Acos(quaternion.W)
			};
			float num = MathF.Sqrt(1f - quaternion.W * quaternion.W);
			if (num > 0.0001f)
			{
				result.Xyz = quaternion.Xyz / num;
			}
			else
			{
				result.Xyz = Vector3.UnitX;
			}
			return result;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000041E6 File Offset: 0x000023E6
		public readonly void ToEulerAngles(out Vector3 angles)
		{
			angles = this.ToEulerAngles();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000041F4 File Offset: 0x000023F4
		public readonly Vector3 ToEulerAngles()
		{
			Quaternion quaternion = this;
			float num = quaternion.W * quaternion.W;
			float num2 = quaternion.X * quaternion.X;
			float num3 = quaternion.Y * quaternion.Y;
			float num4 = quaternion.Z * quaternion.Z;
			float num5 = num2 + num3 + num4 + num;
			float num6 = quaternion.X * quaternion.Z + quaternion.W * quaternion.Y;
			Vector3 result;
			if (num6 > 0.4999995f * num5)
			{
				result.Z = 2f * MathF.Atan2(quaternion.X, quaternion.W);
				result.Y = 1.5707964f;
				result.X = 0f;
			}
			else if (num6 < -0.4999995f * num5)
			{
				result.Z = -2f * MathF.Atan2(quaternion.X, quaternion.W);
				result.Y = -1.5707964f;
				result.X = 0f;
			}
			else
			{
				result.Z = MathF.Atan2(2f * (quaternion.W * quaternion.Z - quaternion.X * quaternion.Y), num + num2 - num3 - num4);
				result.Y = MathF.Asin(2f * num6 / num5);
				result.X = MathF.Atan2(2f * (quaternion.W * quaternion.X - quaternion.Y * quaternion.Z), num - num2 - num3 + num4);
			}
			return result;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000438D File Offset: 0x0000258D
		public readonly float Length
		{
			get
			{
				return MathF.Sqrt(this.W * this.W + this.Xyz.LengthSquared);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000043AD File Offset: 0x000025AD
		public readonly float LengthSquared
		{
			get
			{
				return this.W * this.W + this.Xyz.LengthSquared;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000043C8 File Offset: 0x000025C8
		public readonly Quaternion Normalized()
		{
			Quaternion result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000043E4 File Offset: 0x000025E4
		public void Invert()
		{
			Quaternion.Invert(this, out this);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000043F0 File Offset: 0x000025F0
		public readonly Quaternion Inverted()
		{
			Quaternion result = this;
			result.Invert();
			return result;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000440C File Offset: 0x0000260C
		public void Normalize()
		{
			float num = 1f / this.Length;
			this.Xyz *= num;
			this.W *= num;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004446 File Offset: 0x00002646
		public void Conjugate()
		{
			this.Xyz = -this.Xyz;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004459 File Offset: 0x00002659
		public static Quaternion Add(Quaternion left, Quaternion right)
		{
			return new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000447E File Offset: 0x0000267E
		public static void Add(in Quaternion left, in Quaternion right, out Quaternion result)
		{
			result = new Quaternion(left.Xyz + right.Xyz, left.W + right.W);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000044A9 File Offset: 0x000026A9
		public static Quaternion Sub(Quaternion left, Quaternion right)
		{
			return new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000044CE File Offset: 0x000026CE
		public static void Sub(in Quaternion left, in Quaternion right, out Quaternion result)
		{
			result = new Quaternion(left.Xyz - right.Xyz, left.W - right.W);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000044FC File Offset: 0x000026FC
		public static Quaternion Multiply(Quaternion left, Quaternion right)
		{
			Quaternion result;
			Quaternion.Multiply(left, right, out result);
			return result;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004518 File Offset: 0x00002718
		public static void Multiply(in Quaternion left, in Quaternion right, out Quaternion result)
		{
			result = new Quaternion(right.W * left.Xyz + left.W * right.Xyz + Vector3.Cross(left.Xyz, right.Xyz), left.W * right.W - Vector3.Dot(left.Xyz, right.Xyz));
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000458C File Offset: 0x0000278C
		public static void Multiply(in Quaternion quaternion, float scale, out Quaternion result)
		{
			result = new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000045B9 File Offset: 0x000027B9
		public static Quaternion Multiply(Quaternion quaternion, float scale)
		{
			return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000045E3 File Offset: 0x000027E3
		public static Quaternion Conjugate(Quaternion q)
		{
			return new Quaternion(-q.Xyz, q.W);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000045FB File Offset: 0x000027FB
		public static void Conjugate(in Quaternion q, out Quaternion result)
		{
			result = new Quaternion(-q.Xyz, q.W);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000461C File Offset: 0x0000281C
		public static Quaternion Invert(Quaternion q)
		{
			Quaternion result;
			Quaternion.Invert(q, out result);
			return result;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004634 File Offset: 0x00002834
		public static void Invert(in Quaternion q, out Quaternion result)
		{
			float lengthSquared = q.LengthSquared;
			if ((double)lengthSquared != 0.0)
			{
				float num = 1f / lengthSquared;
				result = new Quaternion(q.Xyz * -num, q.W * num);
				return;
			}
			result = q;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000468C File Offset: 0x0000288C
		public static Quaternion Normalize(Quaternion q)
		{
			Quaternion result;
			Quaternion.Normalize(q, out result);
			return result;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000046A4 File Offset: 0x000028A4
		public static void Normalize(in Quaternion q, out Quaternion result)
		{
			float num = 1f / q.Length;
			result = new Quaternion(q.Xyz * num, q.W * num);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000046E0 File Offset: 0x000028E0
		public static Quaternion FromAxisAngle(Vector3 axis, float angle)
		{
			if (axis.LengthSquared == 0f)
			{
				return Quaternion.Identity;
			}
			Quaternion identity = Quaternion.Identity;
			angle *= 0.5f;
			axis.Normalize();
			identity.Xyz = axis * MathF.Sin(angle);
			identity.W = MathF.Cos(angle);
			return Quaternion.Normalize(identity);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000473D File Offset: 0x0000293D
		public static Quaternion FromEulerAngles(float pitch, float yaw, float roll)
		{
			return new Quaternion(pitch, yaw, roll);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004747 File Offset: 0x00002947
		public static Quaternion FromEulerAngles(Vector3 eulerAngles)
		{
			return new Quaternion(eulerAngles);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004750 File Offset: 0x00002950
		public static void FromEulerAngles(in Vector3 eulerAngles, out Quaternion result)
		{
			float num = MathF.Cos(eulerAngles.X * 0.5f);
			float num2 = MathF.Cos(eulerAngles.Y * 0.5f);
			float num3 = MathF.Cos(eulerAngles.Z * 0.5f);
			float num4 = MathF.Sin(eulerAngles.X * 0.5f);
			float num5 = MathF.Sin(eulerAngles.Y * 0.5f);
			float num6 = MathF.Sin(eulerAngles.Z * 0.5f);
			result.W = num * num2 * num3 - num4 * num5 * num6;
			result.Xyz.X = num4 * num2 * num3 + num * num5 * num6;
			result.Xyz.Y = num * num5 * num3 - num4 * num2 * num6;
			result.Xyz.Z = num * num2 * num6 + num4 * num5 * num3;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004826 File Offset: 0x00002A26
		public static void ToEulerAngles(in Quaternion q, out Vector3 result)
		{
			q.ToEulerAngles(out result);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004830 File Offset: 0x00002A30
		public static Quaternion FromMatrix(Matrix3 matrix)
		{
			Quaternion result;
			Quaternion.FromMatrix(matrix, out result);
			return result;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004848 File Offset: 0x00002A48
		public static void FromMatrix(in Matrix3 matrix, out Quaternion result)
		{
			float trace = matrix.Trace;
			if (trace > 0f)
			{
				float num = MathF.Sqrt(trace + 1f) * 2f;
				float num2 = 1f / num;
				result.W = num * 0.25f;
				result.Xyz.X = (matrix.Row2.Y - matrix.Row1.Z) * num2;
				result.Xyz.Y = (matrix.Row0.Z - matrix.Row2.X) * num2;
				result.Xyz.Z = (matrix.Row1.X - matrix.Row0.Y) * num2;
				return;
			}
			float x = matrix.Row0.X;
			float y = matrix.Row1.Y;
			float z = matrix.Row2.Z;
			if (x > y && x > z)
			{
				float num3 = MathF.Sqrt(1f + x - y - z) * 2f;
				float num4 = 1f / num3;
				result.W = (matrix.Row2.Y - matrix.Row1.Z) * num4;
				result.Xyz.X = num3 * 0.25f;
				result.Xyz.Y = (matrix.Row0.Y + matrix.Row1.X) * num4;
				result.Xyz.Z = (matrix.Row0.Z + matrix.Row2.X) * num4;
				return;
			}
			if (y > z)
			{
				float num5 = MathF.Sqrt(1f + y - x - z) * 2f;
				float num6 = 1f / num5;
				result.W = (matrix.Row0.Z - matrix.Row2.X) * num6;
				result.Xyz.X = (matrix.Row0.Y + matrix.Row1.X) * num6;
				result.Xyz.Y = num5 * 0.25f;
				result.Xyz.Z = (matrix.Row1.Z + matrix.Row2.Y) * num6;
				return;
			}
			float num7 = MathF.Sqrt(1f + z - x - y) * 2f;
			float num8 = 1f / num7;
			result.W = (matrix.Row1.X - matrix.Row0.Y) * num8;
			result.Xyz.X = (matrix.Row0.Z + matrix.Row2.X) * num8;
			result.Xyz.Y = (matrix.Row1.Z + matrix.Row2.Y) * num8;
			result.Xyz.Z = num7 * 0.25f;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004B20 File Offset: 0x00002D20
		public static Quaternion Slerp(Quaternion q1, Quaternion q2, float blend)
		{
			if (q1.LengthSquared == 0f)
			{
				if (q2.LengthSquared == 0f)
				{
					return Quaternion.Identity;
				}
				return q2;
			}
			else
			{
				if (q2.LengthSquared == 0f)
				{
					return q1;
				}
				float num = q1.W * q2.W + Vector3.Dot(q1.Xyz, q2.Xyz);
				if (num >= 1f || num <= -1f)
				{
					return q1;
				}
				if (num < 0f)
				{
					q2.Xyz = -q2.Xyz;
					q2.W = -q2.W;
					num = -num;
				}
				float num5;
				float num6;
				if (num < 0.99f)
				{
					float num2 = MathF.Acos(num);
					float num3 = MathF.Sin(num2);
					float num4 = 1f / num3;
					num5 = MathF.Sin(num2 * (1f - blend)) * num4;
					num6 = MathF.Sin(num2 * blend) * num4;
				}
				else
				{
					num5 = 1f - blend;
					num6 = blend;
				}
				Quaternion q3 = new Quaternion(num5 * q1.Xyz + num6 * q2.Xyz, num5 * q1.W + num6 * q2.W);
				if (q3.LengthSquared > 0f)
				{
					return Quaternion.Normalize(q3);
				}
				return Quaternion.Identity;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004C56 File Offset: 0x00002E56
		public static Quaternion operator +(Quaternion left, Quaternion right)
		{
			left.Xyz += right.Xyz;
			left.W += right.W;
			return left;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004C87 File Offset: 0x00002E87
		public static Quaternion operator -(Quaternion left, Quaternion right)
		{
			left.Xyz -= right.Xyz;
			left.W -= right.W;
			return left;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004CB8 File Offset: 0x00002EB8
		public static Quaternion operator *(Quaternion left, Quaternion right)
		{
			Quaternion.Multiply(left, right, out left);
			return left;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004CC6 File Offset: 0x00002EC6
		public static Quaternion operator *(Quaternion quaternion, float scale)
		{
			Quaternion.Multiply(quaternion, scale, out quaternion);
			return quaternion;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004CD3 File Offset: 0x00002ED3
		public static Quaternion operator *(float scale, Quaternion quaternion)
		{
			return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004CFD File Offset: 0x00002EFD
		public static bool operator ==(Quaternion left, Quaternion right)
		{
			return left.Equals(right);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004D07 File Offset: 0x00002F07
		public static bool operator !=(Quaternion left, Quaternion right)
		{
			return !(left == right);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004D13 File Offset: 0x00002F13
		public unsafe static explicit operator Quaternion(Quaternion quat)
		{
			return *Unsafe.As<Quaternion, Quaternion>(ref quat);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004D21 File Offset: 0x00002F21
		public unsafe static explicit operator Quaternion(Quaternion quat)
		{
			return *Unsafe.As<Quaternion, Quaternion>(ref quat);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004D2F File Offset: 0x00002F2F
		public override readonly bool Equals(object obj)
		{
			return obj is Quaternion && this.Equals((Quaternion)obj);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004D47 File Offset: 0x00002F47
		public readonly bool Equals(Quaternion other)
		{
			return this.Xyz.Equals(other.Xyz) && this.W == other.W;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004D6C File Offset: 0x00002F6C
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3, float>(this.Xyz, this.W);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004D7F File Offset: 0x00002F7F
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004D89 File Offset: 0x00002F89
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004D93 File Offset: 0x00002F93
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004DA0 File Offset: 0x00002FA0
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

		// Token: 0x0400000B RID: 11
		public Vector3 Xyz;

		// Token: 0x0400000C RID: 12
		public float W;

		// Token: 0x0400000D RID: 13
		public static readonly Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);
	}
}
