using System;
using System.Text;

namespace Concentus.Common.CPlusPlus
{
	// Token: 0x02000081 RID: 129
	internal class Pointer<T>
	{
		// Token: 0x06000474 RID: 1140 RVA: 0x00027CBE File Offset: 0x00025EBE
		internal Pointer(int capacity)
		{
			this._array = new T[capacity];
			this._offset = 0;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00027CD9 File Offset: 0x00025ED9
		internal Pointer(T[] buffer)
		{
			this._array = buffer;
			this._offset = 0;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00027CEF File Offset: 0x00025EEF
		internal Pointer(T[] buffer, int absoluteOffset)
		{
			this._array = buffer;
			this._offset = absoluteOffset;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00027D05 File Offset: 0x00025F05
		internal int Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00027D0D File Offset: 0x00025F0D
		internal T[] Data
		{
			get
			{
				return this._array;
			}
		}

		// Token: 0x170000BE RID: 190
		internal T this[int index]
		{
			get
			{
				return this._array[index + this._offset];
			}
			set
			{
				this._array[index + this._offset] = value;
			}
		}

		// Token: 0x170000BF RID: 191
		internal T this[uint index]
		{
			get
			{
				return this[(int)index];
			}
			set
			{
				this[(int)index] = value;
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00027D53 File Offset: 0x00025F53
		internal Pointer<T> Iterate(out T returnVal)
		{
			returnVal = this._array[this._offset];
			return this.Point(1);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00027D73 File Offset: 0x00025F73
		internal Pointer<T> Point(int relativeOffset)
		{
			if (relativeOffset == 0)
			{
				return this;
			}
			return new Pointer<T>(this._array, this._offset + relativeOffset);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00027D8D File Offset: 0x00025F8D
		internal Pointer<T> Point(uint relativeOffset)
		{
			if (relativeOffset == 0U)
			{
				return this;
			}
			return new Pointer<T>(this._array, this._offset + (int)relativeOffset);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00027DA8 File Offset: 0x00025FA8
		private static string invert_endianness(string hexstring)
		{
			StringBuilder stringBuilder = new StringBuilder(hexstring.Length);
			for (int i = 0; i < hexstring.Length / 2; i++)
			{
				stringBuilder.Append(hexstring.Substring(hexstring.Length - (i + 1) * 2, 2));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00027DF4 File Offset: 0x00025FF4
		private static void PrintMemCopy<E>(E[] source, int sourceOffset, int length)
		{
			if (typeof(E) == typeof(int) || typeof(E) == typeof(uint))
			{
				string text = string.Empty;
				for (int i = 0; i < length; i++)
				{
					text += Pointer<T>.invert_endianness(string.Format("{0:x8}", source[i + sourceOffset]));
				}
				return;
			}
			if (typeof(E) == typeof(short) || typeof(E) == typeof(ushort))
			{
				string text2 = string.Empty;
				for (int j = 0; j < length; j++)
				{
					text2 += Pointer<T>.invert_endianness(string.Format("{0:x4}", source[j + sourceOffset]));
				}
				return;
			}
			if (typeof(E) == typeof(byte) || typeof(E) == typeof(sbyte))
			{
				string text3 = string.Empty;
				for (int k = 0; k < length; k++)
				{
					text3 += Pointer<T>.invert_endianness(string.Format("{0:x2}", source[k + sourceOffset]));
				}
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00027F50 File Offset: 0x00026150
		internal void MemCopyTo(Pointer<T> destination, int length)
		{
			if (destination != null)
			{
				Array.Copy(this._array, this._offset, destination._array, destination.Offset, length);
				return;
			}
			for (int i = 0; i < length; i++)
			{
				destination[i] = this._array[i + this._offset];
			}
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00027FA5 File Offset: 0x000261A5
		internal void MemCopyTo(T[] destination, int offset, int length)
		{
			Array.Copy(this._array, this._offset, destination, offset, length);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00027FBB File Offset: 0x000261BB
		internal void MemCopyFrom(T[] source, int sourceOffset, int length)
		{
			Array.Copy(source, sourceOffset, this._array, this._offset, length);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00027FD1 File Offset: 0x000261D1
		internal void MemSet(T value, int length)
		{
			this.MemSet(value, (uint)length);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00027FDC File Offset: 0x000261DC
		internal void MemSet(T value, uint length)
		{
			int num = this._offset;
			while ((long)num < (long)this._offset + (long)((ulong)length))
			{
				this._array[num] = value;
				num++;
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00028011 File Offset: 0x00026211
		internal void MemMoveTo(Pointer<T> other, int length)
		{
			if (this._array == other._array)
			{
				this.MemMove(other.Offset - this.Offset, length);
				return;
			}
			this.MemCopyTo(other, length);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00028040 File Offset: 0x00026240
		internal void MemMove(int move_dist, int length)
		{
			MemoryExtensions.AsSpan<T>(this._array, this._offset, length).CopyTo(MemoryExtensions.AsSpan<T>(this._array, this._offset + move_dist, length));
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0002807C File Offset: 0x0002627C
		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			Pointer<T> pointer = (Pointer<T>)obj;
			return pointer._offset == this._offset && pointer._array == this._array;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000280C6 File Offset: 0x000262C6
		public override int GetHashCode()
		{
			return this._array.GetHashCode() + this._offset.GetHashCode();
		}

		// Token: 0x0400042E RID: 1070
		private const bool CHECK_UNINIT_MEM = false;

		// Token: 0x0400042F RID: 1071
		private T[] _array;

		// Token: 0x04000430 RID: 1072
		private int _offset;
	}
}
