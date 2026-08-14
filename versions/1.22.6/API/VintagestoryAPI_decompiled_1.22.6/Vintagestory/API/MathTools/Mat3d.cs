namespace Vintagestory.API.MathTools;

public class Mat3d
{
	/// **
	public static double[] Create()
	{
		return new double[9] { 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 };
	}

	/// **
	public static double[] FromMat4(double[] output, double[] a)
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
	public static double[] CloneIt(double[] a)
	{
		return new double[9]
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
	public static double[] Copy(double[] output, double[] a)
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
	public static double[] Identity_(double[] output)
	{
		output[0] = 1.0;
		output[1] = 0.0;
		output[2] = 0.0;
		output[3] = 0.0;
		output[4] = 1.0;
		output[5] = 0.0;
		output[6] = 0.0;
		output[7] = 0.0;
		output[8] = 1.0;
		return output;
	}

	/// **
	public static double[] Transpose(double[] output, double[] a)
	{
		if (output == a)
		{
			double a2 = a[1];
			double a3 = a[2];
			double a12 = a[5];
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
	public static double[] Invert(double[] output, double[] a)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a10 = a[3];
		double a11 = a[4];
		double a12 = a[5];
		double a20 = a[6];
		double a21 = a[7];
		double a22 = a[8];
		double b01 = a22 * a11 - a12 * a21;
		double b11 = (0.0 - a22) * a10 + a12 * a20;
		double b21 = a21 * a10 - a11 * a20;
		double det = a2 * b01 + a3 * b11 + a4 * b21;
		if (det == 0.0)
		{
			return null;
		}
		det = 1.0 / det;
		output[0] = b01 * det;
		output[1] = ((0.0 - a22) * a3 + a4 * a21) * det;
		output[2] = (a12 * a3 - a4 * a11) * det;
		output[3] = b11 * det;
		output[4] = (a22 * a2 - a4 * a20) * det;
		output[5] = ((0.0 - a12) * a2 + a4 * a10) * det;
		output[6] = b21 * det;
		output[7] = ((0.0 - a21) * a2 + a3 * a20) * det;
		output[8] = (a11 * a2 - a3 * a10) * det;
		return output;
	}

	/// **
	public static double[] Adjoint(double[] output, double[] a)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a10 = a[3];
		double a11 = a[4];
		double a12 = a[5];
		double a20 = a[6];
		double a21 = a[7];
		double a22 = a[8];
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
	public static double Determinant(double[] a)
	{
		double num = a[0];
		double a2 = a[1];
		double a3 = a[2];
		double a10 = a[3];
		double a11 = a[4];
		double a12 = a[5];
		double a20 = a[6];
		double a21 = a[7];
		double a22 = a[8];
		return num * (a22 * a11 - a12 * a21) + a2 * ((0.0 - a22) * a10 + a12 * a20) + a3 * (a21 * a10 - a11 * a20);
	}

	/// **
	public static double[] Multiply(double[] output, double[] a, double[] b)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a10 = a[3];
		double a11 = a[4];
		double a12 = a[5];
		double a20 = a[6];
		double a21 = a[7];
		double a22 = a[8];
		double b2 = b[0];
		double b3 = b[1];
		double b4 = b[2];
		double b10 = b[3];
		double b11 = b[4];
		double b12 = b[5];
		double b20 = b[6];
		double b21 = b[7];
		double b22 = b[8];
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
	public static double[] Mul(double[] output, double[] a, double[] b)
	{
		return Multiply(output, a, b);
	}

	/// **
	public static double[] Translate(double[] output, double[] a, double[] v)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a10 = a[3];
		double a11 = a[4];
		double a12 = a[5];
		double a20 = a[6];
		double a21 = a[7];
		double a22 = a[8];
		double x = v[0];
		double y = v[1];
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
	public static double[] Rotate(double[] output, double[] a, double rad)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a10 = a[3];
		double a11 = a[4];
		double a12 = a[5];
		double a20 = a[6];
		double a21 = a[7];
		double a22 = a[8];
		double s = GameMath.Sin(rad);
		double c = GameMath.Cos(rad);
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
	public static double[] Scale(double[] output, double[] a, double[] v)
	{
		double x = v[0];
		double y = v[1];
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
	public static double[] FromMat2d(double[] output, double[] a)
	{
		output[0] = a[0];
		output[1] = a[1];
		output[2] = 0.0;
		output[3] = a[2];
		output[4] = a[3];
		output[5] = 0.0;
		output[6] = a[4];
		output[7] = a[5];
		output[8] = 1.0;
		return output;
	}

	/// **
	public static double[] FromQuat(double[] output, double[] q)
	{
		double num = q[0];
		double y = q[1];
		double z = q[2];
		double w = q[3];
		double x2 = num + num;
		double y2 = y + y;
		double z2 = z + z;
		double xx = num * x2;
		double xy = num * y2;
		double xz = num * z2;
		double yy = y * y2;
		double yz = y * z2;
		double zz = z * z2;
		double wx = w * x2;
		double wy = w * y2;
		double wz = w * z2;
		output[0] = 1.0 - (yy + zz);
		output[3] = xy + wz;
		output[6] = xz - wy;
		output[1] = xy - wz;
		output[4] = 1.0 - (xx + zz);
		output[7] = yz + wx;
		output[2] = xz + wy;
		output[5] = yz - wx;
		output[8] = 1.0 - (xx + yy);
		return output;
	}

	/// **
	public static double[] NormalFromMat4(double[] output, double[] a)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double num = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		double a30 = a[12];
		double a31 = a[13];
		double a32 = a[14];
		double a33 = a[15];
		double b00 = a2 * a11 - a3 * a10;
		double b1 = a2 * a12 - a4 * a10;
		double b2 = a2 * a13 - a5 * a10;
		double b3 = a3 * a12 - a4 * a11;
		double b4 = a3 * a13 - a5 * a11;
		double b5 = a4 * a13 - a5 * a12;
		double b6 = num * a31 - a21 * a30;
		double b7 = num * a32 - a22 * a30;
		double b8 = num * a33 - a23 * a30;
		double b9 = a21 * a32 - a22 * a31;
		double b10 = a21 * a33 - a23 * a31;
		double b11 = a22 * a33 - a23 * a32;
		double det = b00 * b11 - b1 * b10 + b2 * b9 + b3 * b8 - b4 * b7 + b5 * b6;
		if (det == 0.0)
		{
			return null;
		}
		det = 1.0 / det;
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
