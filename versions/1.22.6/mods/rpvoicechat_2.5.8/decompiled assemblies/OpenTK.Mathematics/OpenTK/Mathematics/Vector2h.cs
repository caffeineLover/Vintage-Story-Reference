using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000024 RID: 36
	[Serializable]
	public struct Vector2h : ISerializable, IEquatable<Vector2h>, IFormattable
	{
		// Token: 0x06000970 RID: 2416 RVA: 0x000247B3 File Offset: 0x000229B3
		public Vector2h(Half value)
		{
			this.X = value;
			this.Y = value;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x000247C3 File Offset: 0x000229C3
		public Vector2h(float value)
		{
			this.X = new Half(value);
			this.Y = new Half(value);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000247DD File Offset: 0x000229DD
		public Vector2h(Half x, Half y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000247ED File Offset: 0x000229ED
		public Vector2h(float x, float y)
		{
			this.X = new Half(x);
			this.Y = new Half(y);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00024807 File Offset: 0x00022A07
		public Vector2h(float x, float y, bool throwOnError)
		{
			this.X = new Half(x, throwOnError);
			this.Y = new Half(y, throwOnError);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00024823 File Offset: 0x00022A23
		public Vector2h(Vector2 v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00024847 File Offset: 0x00022A47
		public Vector2h(Vector2 v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0002486D File Offset: 0x00022A6D
		public Vector2h(in Vector2 v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00024891 File Offset: 0x00022A91
		public Vector2h(in Vector2 v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000248B7 File Offset: 0x00022AB7
		public Vector2h(Vector2d v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x000248DB File Offset: 0x00022ADB
		public Vector2h(Vector2d v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00024901 File Offset: 0x00022B01
		public Vector2h(in Vector2d v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00024925 File Offset: 0x00022B25
		public Vector2h(in Vector2d v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0002494B File Offset: 0x00022B4B
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0002495E File Offset: 0x00022B5E
		[XmlIgnore]
		public Vector2h Yx
		{
			readonly get
			{
				return new Vector2h(this.Y, this.X);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00024978 File Offset: 0x00022B78
		public readonly Vector2 ToVector2()
		{
			return new Vector2(this.X, this.Y);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00024995 File Offset: 0x00022B95
		public readonly Vector2d ToVector2d()
		{
			return new Vector2d(this.X, this.Y);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x000249B2 File Offset: 0x00022BB2
		public static implicit operator Vector2(Vector2h vec)
		{
			return new Vector2(vec.X, vec.Y);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000249CF File Offset: 0x00022BCF
		public static implicit operator Vector2d(Vector2h vec)
		{
			return new Vector2d(vec.X, vec.Y);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x000249EC File Offset: 0x00022BEC
		public static explicit operator Vector2i(Vector2h vec)
		{
			return new Vector2i((int)vec.X, (int)vec.Y);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00024A0D File Offset: 0x00022C0D
		public static implicit operator Vector2h([TupleElementNames(new string[]
		{
			"X",
			"Y"
		})] ValueTuple<Half, Half> values)
		{
			return new Vector2h(values.Item1, values.Item2);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00024A20 File Offset: 0x00022C20
		public static bool operator ==(Vector2h left, Vector2h right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00024A2A File Offset: 0x00022C2A
		public static bool operator !=(Vector2h left, Vector2h right)
		{
			return !(left == right);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00024A38 File Offset: 0x00022C38
		public Vector2h(SerializationInfo info, StreamingContext context)
		{
			this.X = (Half)info.GetValue("X", typeof(Half));
			this.Y = (Half)info.GetValue("Y", typeof(Half));
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00024A85 File Offset: 0x00022C85
		public readonly void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("X", this.X);
			info.AddValue("Y", this.Y);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00024AB3 File Offset: 0x00022CB3
		public void FromBinaryStream(BinaryReader bin)
		{
			this.X.FromBinaryStream(bin);
			this.Y.FromBinaryStream(bin);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00024ACD File Offset: 0x00022CCD
		public void ToBinaryStream(BinaryWriter bin)
		{
			this.X.ToBinaryStream(bin);
			this.Y.ToBinaryStream(bin);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00024AE7 File Offset: 0x00022CE7
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00024AF1 File Offset: 0x00022CF1
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00024AFB File Offset: 0x00022CFB
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00024B05 File Offset: 0x00022D05
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{2} {1})", this.X.ToString(format, formatProvider), this.Y.ToString(format, formatProvider), MathHelper.GetListSeparator(formatProvider));
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00024B31 File Offset: 0x00022D31
		public override bool Equals(object obj)
		{
			return obj is Vector2h && this.Equals((Vector2h)obj);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00024B49 File Offset: 0x00022D49
		public bool Equals(Vector2h other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00024B71 File Offset: 0x00022D71
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Half, Half>(this.X, this.Y);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00024B84 File Offset: 0x00022D84
		public static byte[] GetBytes(Vector2h h)
		{
			byte[] array = new byte[Vector2h.SizeInBytes];
			byte[] bytes = Half.GetBytes(h.X);
			array[0] = bytes[0];
			array[1] = bytes[1];
			bytes = Half.GetBytes(h.Y);
			array[2] = bytes[0];
			array[3] = bytes[1];
			return array;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00024BCB File Offset: 0x00022DCB
		public static Vector2h FromBytes(byte[] value, int startIndex)
		{
			return new Vector2h(Half.FromBytes(value, startIndex), Half.FromBytes(value, startIndex + 2));
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00024BE2 File Offset: 0x00022DE2
		public readonly void Deconstruct(out Half x, out Half y)
		{
			x = this.X;
			y = this.Y;
		}

		// Token: 0x04000098 RID: 152
		public Half X;

		// Token: 0x04000099 RID: 153
		public Half Y;

		// Token: 0x0400009A RID: 154
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2h>();
	}
}
