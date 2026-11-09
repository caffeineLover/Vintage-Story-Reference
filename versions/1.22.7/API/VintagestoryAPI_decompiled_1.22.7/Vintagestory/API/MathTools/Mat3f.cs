namespace Vintagestory.API.MathTools;

public class Mat3f
{
	/// **
	public static float[] Create()
	{
		return new float[9] { 1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f };
	}

	/// **
	public static float[] FromMat4(float[] output, float[] a)
	{
		output[0] = a[0];
		output[1] = a[1];
		output[2] = a[2];
		output[3] = a[4];
		output[4] = a[5];
		output[5] = a[6];
		output[6] = a[8];
		output[7] = a[9];
		output[8] = a[10];
		return output;
	}

	/// **
	public static float[] CloneIt(float[] a)
	{
		return new float[9]
		{
			a[0],
			a[1],
			a[2],
			a[3],
			a[4],
			a[5],
			a[6],
			a[7],
			a[8]
		};
	}

	/// **
	public static float[] Copy(float[] output, float[] a)
	{
		output[0] = a[0];
		output[1] = a[1];
		output[2] = a[2];
		output[3] = a[3];
		output[4] = a[4];
		output[5] = a[5];
		output[6] = a[6];
		output[7] = a[7];
		output[8] = a[8];
		return output;
	}

	/// **
	public static float[] Identity_(float[] output)
	{
		output[0] = 1f;
		output[1] = 0f;
		output[2] = 0f;
		output[3] = 0f;
		output[4] = 1f;
		output[5] = 0f;
		output[6] = 0f;
		output[7] = 0f;
		output[8] = 1f;
		return output;
	}

	/// **
	public static float[] Transpose(float[] output, float[] a)
	{
		if (output == a)
		{
			float a2 = a[1];
			float a3 = a[2];
			float a12 = a[5];
			output[1] = a[3];
			output[2] = a[6];
			output[3] = a2;
			output[5] = a[7];
			output[6] = a3;
			output[7] = a12;
		}
		else
		{
			output[0] = a[0];
			output[1] = a[3];
			output[2] = a[6];
			output[3] = a[1];
			output[4] = a[4];
			output[5] = a[7];
			output[6] = a[2];
			output[7] = a[5];
			output[8] = a[8];
		}
		return output;
	}

	/// **
	public static float[] Invert(float[] output, float[] a)
	{
		float a2 = a[0];
		float a3 = a[1];
		float a4 = a[2];
		float a10 = a[3];
		float a11 = a[4];
		float a12 = a[5];
		float a20 = a[6];
		float a21 = a[7];
		float a22 = a[8];
		float b01 = a22 * a11 - a12 * a21;
		float b11 = (0f - a22) * a10 + a12 * a20;
		float b21 = a21 * a10 - a11 * a20;
		float det = a2 * b01 + a3 * b11 + a4 * b21;
		if (det == 0f)
		{
			return null;
		}
		det = 1f / det;
		output[0] = b01 * det;
		output[1] = ((0f - a22) * a3 + a4 * a21) * det;
		output[2] = (a12 * a3 - a4 * a11) * det;
		output[3] = b11 * det;
		output[4] = (a22 * a2 - a4 * a20) * det;
		output[5] = ((0f - a12) * a2 + a4 * a10) * det;
		output[6] = b21 * det;
		output[7] = ((0f - a21) * a2 + a3 * a20) * det;
		output[8] = (a11 * a2 - a3 * a10) * det;
		return output;
	}

	/// **
	public static float[] Adjoint(float[] output, float[] a)
	{
		float a2 = a[0];
		float a3 = a[1];
		float a4 = a[2];
		float a10 = a[3];
		float a11 = a[4];
		float a12 = a[5];
		float a20 = a[6];
		float a21 = a[7];
		float a22 = a[8];
		output[0] = a11 * a22 - a12 * a21;
		output[1] = a4 * a21 - a3 * a22;
		output[2] = a3 * a12 - a4 * a11;
		output[3] = a12 * a20 - a10 * a22;
		output[4] = a2 * a22 - a4 * a20;
		output[5] = a4 * a10 - a2 * a12;
		output[6] = a10 * a21 - a11 * a20;
		output[7] = a3 * a20 - a2 * a21;
		output[8] = a2 * a11 - a3 * a10;
		return output;
	}

	/// **
	public static float Determinant(float[] a)
	{
		float num = a[0];
		float a2 = a[1];
		float a3 = a[2];
		float a10 = a[3];
		float a11 = a[4];
		float a12 = a[5];
		float a20 = a[6];
		float a21 = a[7];
		float a22 = a[8];
		return num * (a22 * a11 - a12 * a21) + a2 * ((0f - a22) * a10 + a12 * a20) + a3 * (a21 * a10 - a11 * a20);
	}

	/// **
	public static float[] Multiply(float[] output, float[] a, float[] b)
	{
		float a2 = a[0];
		float a3 = a[1];
		float a4 = a[2];
		float a10 = a[3];
		float a11 = a[4];
		float a12 = a[5];
		float a20 = a[6];
		float a21 = a[7];
		float a22 = a[8];
		float b2 = b[0];
		float b3 = b[1];
		float b4 = b[2];
		float b10 = b[3];
		float b11 = b[4];
		float b12 = b[5];
		float b20 = b[6];
		float b21 = b[7];
		float b22 = b[8];
		output[0] = b2 * a2 + b3 * a10 + b4 * a20;
		output[1] = b2 * a3 + b3 * a11 + b4 * a21;
		output[2] = b2 * a4 + b3 * a12 + b4 * a22;
		output[3] = b10 * a2 + b11 * a10 + b12 * a20;
		output[4] = b10 * a3 + b11 * a11 + b12 * a21;
		output[5] = b10 * a4 + b11 * a12 + b12 * a22;
		output[6] = b20 * a2 + b21 * a10 + b22 * a20;
		output[7] = b20 * a3 + b21 * a11 + b22 * a21;
		output[8] = b20 * a4 + b21 * a12 + b22 * a22;
		return output;
	}

	/// **
	public static float[] Mul(float[] output, float[] a, float[] b)
	{
		return Multiply(output, a, b);
	}

	/// **
	public static float[] Translate(float[] output, float[] a, float[] v)
	{
		float a2 = a[0];
		float a3 = a[1];
		float a4 = a[2];
		float a10 = a[3];
		float a11 = a[4];
		float a12 = a[5];
		float a20 = a[6];
		float a21 = a[7];
		float a22 = a[8];
		float x = v[0];
		float y = v[1];
		output[0] = a2;
		output[1] = a3;
		output[2] = a4;
		output[3] = a10;
		output[4] = a11;
		output[5] = a12;
		output[6] = x * a2 + y * a10 + a20;
		output[7] = x * a3 + y * a11 + a21;
		output[8] = x * a4 + y * a12 + a22;
		return output;
	}

	/// **
	public static float[] Rotate(float[] output, float[] a, float rad)
	{
		float a2 = a[0];
		float a3 = a[1];
		float a4 = a[2];
		float a10 = a[3];
		float a11 = a[4];
		float a12 = a[5];
		float a20 = a[6];
		float a21 = a[7];
		float a22 = a[8];
		float s = GameMath.Sin(rad);
		float c = GameMath.Cos(rad);
		output[0] = c * a2 + s * a10;
		output[1] = c * a3 + s * a11;
		output[2] = c * a4 + s * a12;
		output[3] = c * a10 - s * a2;
		output[4] = c * a11 - s * a3;
		output[5] = c * a12 - s * a4;
		output[6] = a20;
		output[7] = a21;
		output[8] = a22;
		return output;
	}

	/// **
	public static float[] Scale(float[] output, float[] a, float[] v)
	{
		float x = v[0];
		float y = v[1];
		output[0] = x * a[0];
		output[1] = x * a[1];
		output[2] = x * a[2];
		output[3] = y * a[3];
		output[4] = y * a[4];
		output[5] = y * a[5];
		output[6] = a[6];
		output[7] = a[7];
		output[8] = a[8];
		return output;
	}

	/// **
	public static float[] FromMat2d(float[] output, float[] a)
	{
		output[0] = a[0];
		output[1] = a[1];
		output[2] = 0f;
		output[3] = a[2];
		output[4] = a[3];
		output[5] = 0f;
		output[6] = a[4];
		output[7] = a[5];
		output[8] = 1f;
		return output;
	}

	/// **
	public static float[] FromQuat(float[] output, float[] q)
	{
		float num = q[0];
		float y = q[1];
		float z = q[2];
		float w = q[3];
		float x2 = num + num;
		float y2 = y + y;
		float z2 = z + z;
		float xx = num * x2;
		float xy = num * y2;
		float xz = num * z2;
		float yy = y * y2;
		float yz = y * z2;
		float zz = z * z2;
		float wx = w * x2;
		float wy = w * y2;
		float wz = w * z2;
		output[0] = 1f - (yy + zz);
		output[3] = xy + wz;
		output[6] = xz - wy;
		output[1] = xy - wz;
		output[4] = 1f - (xx + zz);
		output[7] = yz + wx;
		output[2] = xz + wy;
		output[5] = yz - wx;
		output[8] = 1f - (xx + yy);
		return output;
	}

	/// **
	public static float[] NormalFromMat4(float[] output, float[] a)
	{
		float a2 = a[0];
		float a3 = a[1];
		float a4 = a[2];
		float a5 = a[3];
		float a10 = a[4];
		float a11 = a[5];
		float a12 = a[6];
		float a13 = a[7];
		float num = a[8];
		float a21 = a[9];
		float a22 = a[10];
		float a23 = a[11];
		float a30 = a[12];
		float a31 = a[13];
		float a32 = a[14];
		float a33 = a[15];
		float b00 = a2 * a11 - a3 * a10;
		float b1 = a2 * a12 - a4 * a10;
		float b2 = a2 * a13 - a5 * a10;
		float b3 = a3 * a12 - a4 * a11;
		float b4 = a3 * a13 - a5 * a11;
		float b5 = a4 * a13 - a5 * a12;
		float b6 = num * a31 - a21 * a30;
		float b7 = num * a32 - a22 * a30;
		float b8 = num * a33 - a23 * a30;
		float b9 = a21 * a32 - a22 * a31;
		float b10 = a21 * a33 - a23 * a31;
		float b11 = a22 * a33 - a23 * a32;
		float det = b00 * b11 - b1 * b10 + b2 * b9 + b3 * b8 - b4 * b7 + b5 * b6;
		if (det == 0f)
		{
			return null;
		}
		det = 1f / det;
		output[0] = (a11 * b11 - a12 * b10 + a13 * b9) * det;
		output[1] = (a12 * b8 - a10 * b11 - a13 * b7) * det;
		output[2] = (a10 * b10 - a11 * b8 + a13 * b6) * det;
		output[3] = (a4 * b10 - a3 * b11 - a5 * b9) * det;
		output[4] = (a2 * b11 - a4 * b8 + a5 * b7) * det;
		output[5] = (a3 * b8 - a2 * b10 - a5 * b6) * det;
		output[6] = (a31 * b5 - a32 * b4 + a33 * b3) * det;
		output[7] = (a32 * b2 - a30 * b5 - a33 * b1) * det;
		output[8] = (a30 * b4 - a31 * b2 + a33 * b00) * det;
		return output;
	}

	/// **
	private void f()
	{
	}
}
