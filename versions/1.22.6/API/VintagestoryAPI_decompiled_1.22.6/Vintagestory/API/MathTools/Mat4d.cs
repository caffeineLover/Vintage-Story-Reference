namespace Vintagestory.API.MathTools;

/// <summary>
/// 4x4 Matrix Math
/// </summary>
public class Mat4d
{
	private class GlMatrixMathd
	{
		public static double Abs(double len)
		{
			if (len < 0.0)
			{
				return 0.0 - len;
			}
			return len;
		}

		public static double GLMAT_EPSILON()
		{
			return 1.0 / 1000000.0;
		}
	}

	/// <summary>
	/// Creates a new identity mat4
	/// 0 4 8  12
	/// 1 5 9  13
	/// 2 6 10 14
	/// 3 7 11 15
	/// </summary>
	/// <returns>{mat4} a new 4x4 matrix</returns>
	public static double[] Create()
	{
		return new double[16]
		{
			1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0,
			1.0, 0.0, 0.0, 0.0, 0.0, 1.0
		};
	}

	public static float[] ToMat4f(float[] output, double[] input)
	{
		output[0] = (float)input[0];
		output[1] = (float)input[1];
		output[2] = (float)input[2];
		output[3] = (float)input[3];
		output[4] = (float)input[4];
		output[5] = (float)input[5];
		output[6] = (float)input[6];
		output[7] = (float)input[7];
		output[8] = (float)input[8];
		output[9] = (float)input[9];
		output[10] = (float)input[10];
		output[11] = (float)input[11];
		output[12] = (float)input[12];
		output[13] = (float)input[13];
		output[14] = (float)input[14];
		output[15] = (float)input[15];
		return output;
	}

	/// <summary>
	/// Creates a new mat4 initialized with values from an existing matrix
	/// </summary>
	/// <param name="a">a matrix to clone</param>
	/// <returns>{mat4} a new 4x4 matrix</returns>
	public static double[] CloneIt(double[] a)
	{
		return new double[16]
		{
			a[0],
			a[1],
			a[2],
			a[3],
			a[4],
			a[5],
			a[6],
			a[7],
			a[8],
			a[9],
			a[10],
			a[11],
			a[12],
			a[13],
			a[14],
			a[15]
		};
	}

	/// <summary>
	/// Copy the values from one mat4 to another
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the source matrix</param>
	/// <returns>{mat4} out</returns>
	public static double[] Copy(double[] output, double[] a)
	{
		for (int i = 0; i < output.Length; i += 4)
		{
			output[i] = a[i];
			output[i + 1] = a[i + 1];
			output[i + 2] = a[i + 2];
			output[i + 3] = a[i + 3];
		}
		return output;
	}

	/// <summary>
	/// Set a mat4 to the identity matrix
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <returns>{mat4} out</returns>
	public static double[] Identity(double[] output)
	{
		output[0] = 1.0;
		output[1] = 0.0;
		output[2] = 0.0;
		output[3] = 0.0;
		output[4] = 0.0;
		output[5] = 1.0;
		output[6] = 0.0;
		output[7] = 0.0;
		output[8] = 0.0;
		output[9] = 0.0;
		output[10] = 1.0;
		output[11] = 0.0;
		output[12] = 0.0;
		output[13] = 0.0;
		output[14] = 0.0;
		output[15] = 1.0;
		return output;
	}

	/// <summary>
	/// Transpose the values of a mat4
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the source matrix</param>
	/// <returns>{mat4} out</returns>
	public static double[] Transpose(double[] output, double[] a)
	{
		if (output == a)
		{
			double a2 = a[1];
			double a3 = a[2];
			double a4 = a[3];
			double a12 = a[6];
			double a13 = a[7];
			double a23 = a[11];
			output[1] = a[4];
			output[2] = a[8];
			output[3] = a[12];
			output[4] = a2;
			output[6] = a[9];
			output[7] = a[13];
			output[8] = a3;
			output[9] = a12;
			output[11] = a[14];
			output[12] = a4;
			output[13] = a13;
			output[14] = a23;
		}
		else
		{
			output[0] = a[0];
			output[1] = a[4];
			output[2] = a[8];
			output[3] = a[12];
			output[4] = a[1];
			output[5] = a[5];
			output[6] = a[9];
			output[7] = a[13];
			output[8] = a[2];
			output[9] = a[6];
			output[10] = a[10];
			output[11] = a[14];
			output[12] = a[3];
			output[13] = a[7];
			output[14] = a[11];
			output[15] = a[15];
		}
		return output;
	}

	/// <summary>
	/// Inverts a mat4
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the source matrix</param>
	/// <returns>{mat4} out</returns>
	public static double[] Invert(double[] output, double[] a)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double a20 = a[8];
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
		double b6 = a20 * a31 - a21 * a30;
		double b7 = a20 * a32 - a22 * a30;
		double b8 = a20 * a33 - a23 * a30;
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
		output[1] = (a4 * b10 - a3 * b11 - a5 * b9) * det;
		output[2] = (a31 * b5 - a32 * b4 + a33 * b3) * det;
		output[3] = (a22 * b4 - a21 * b5 - a23 * b3) * det;
		output[4] = (a12 * b8 - a10 * b11 - a13 * b7) * det;
		output[5] = (a2 * b11 - a4 * b8 + a5 * b7) * det;
		output[6] = (a32 * b2 - a30 * b5 - a33 * b1) * det;
		output[7] = (a20 * b5 - a22 * b2 + a23 * b1) * det;
		output[8] = (a10 * b10 - a11 * b8 + a13 * b6) * det;
		output[9] = (a3 * b8 - a2 * b10 - a5 * b6) * det;
		output[10] = (a30 * b4 - a31 * b2 + a33 * b00) * det;
		output[11] = (a21 * b2 - a20 * b4 - a23 * b00) * det;
		output[12] = (a11 * b7 - a10 * b9 - a12 * b6) * det;
		output[13] = (a2 * b9 - a3 * b7 + a4 * b6) * det;
		output[14] = (a31 * b1 - a30 * b3 - a32 * b00) * det;
		output[15] = (a20 * b3 - a21 * b1 + a22 * b00) * det;
		return output;
	}

	/// <summary>
	/// Calculates the adjugate of a mat4   
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the source matrix</param>
	/// <returns>{mat4} out</returns>
	public static double[] Adjoint(double[] output, double[] a)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double a20 = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		double a30 = a[12];
		double a31 = a[13];
		double a32 = a[14];
		double a33 = a[15];
		output[0] = a11 * (a22 * a33 - a23 * a32) - a21 * (a12 * a33 - a13 * a32) + a31 * (a12 * a23 - a13 * a22);
		output[1] = 0.0 - (a3 * (a22 * a33 - a23 * a32) - a21 * (a4 * a33 - a5 * a32) + a31 * (a4 * a23 - a5 * a22));
		output[2] = a3 * (a12 * a33 - a13 * a32) - a11 * (a4 * a33 - a5 * a32) + a31 * (a4 * a13 - a5 * a12);
		output[3] = 0.0 - (a3 * (a12 * a23 - a13 * a22) - a11 * (a4 * a23 - a5 * a22) + a21 * (a4 * a13 - a5 * a12));
		output[4] = 0.0 - (a10 * (a22 * a33 - a23 * a32) - a20 * (a12 * a33 - a13 * a32) + a30 * (a12 * a23 - a13 * a22));
		output[5] = a2 * (a22 * a33 - a23 * a32) - a20 * (a4 * a33 - a5 * a32) + a30 * (a4 * a23 - a5 * a22);
		output[6] = 0.0 - (a2 * (a12 * a33 - a13 * a32) - a10 * (a4 * a33 - a5 * a32) + a30 * (a4 * a13 - a5 * a12));
		output[7] = a2 * (a12 * a23 - a13 * a22) - a10 * (a4 * a23 - a5 * a22) + a20 * (a4 * a13 - a5 * a12);
		output[8] = a10 * (a21 * a33 - a23 * a31) - a20 * (a11 * a33 - a13 * a31) + a30 * (a11 * a23 - a13 * a21);
		output[9] = 0.0 - (a2 * (a21 * a33 - a23 * a31) - a20 * (a3 * a33 - a5 * a31) + a30 * (a3 * a23 - a5 * a21));
		output[10] = a2 * (a11 * a33 - a13 * a31) - a10 * (a3 * a33 - a5 * a31) + a30 * (a3 * a13 - a5 * a11);
		output[11] = 0.0 - (a2 * (a11 * a23 - a13 * a21) - a10 * (a3 * a23 - a5 * a21) + a20 * (a3 * a13 - a5 * a11));
		output[12] = 0.0 - (a10 * (a21 * a32 - a22 * a31) - a20 * (a11 * a32 - a12 * a31) + a30 * (a11 * a22 - a12 * a21));
		output[13] = a2 * (a21 * a32 - a22 * a31) - a20 * (a3 * a32 - a4 * a31) + a30 * (a3 * a22 - a4 * a21);
		output[14] = 0.0 - (a2 * (a11 * a32 - a12 * a31) - a10 * (a3 * a32 - a4 * a31) + a30 * (a3 * a12 - a4 * a11));
		output[15] = a2 * (a11 * a22 - a12 * a21) - a10 * (a3 * a22 - a4 * a21) + a20 * (a3 * a12 - a4 * a11);
		return output;
	}

	/// <summary>
	/// Calculates the determinant of a mat4
	/// </summary>
	/// <param name="a">{mat4} a the source matrix</param>
	/// <returns>{Number} determinant of a</returns>
	public static double Determinant(double[] a)
	{
		double num = a[0];
		double a2 = a[1];
		double a3 = a[2];
		double a4 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double a20 = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		double a30 = a[12];
		double a31 = a[13];
		double a32 = a[14];
		double a33 = a[15];
		double b00 = num * a11 - a2 * a10;
		double b1 = num * a12 - a3 * a10;
		double b2 = num * a13 - a4 * a10;
		double b3 = a2 * a12 - a3 * a11;
		double b4 = a2 * a13 - a4 * a11;
		double b5 = a3 * a13 - a4 * a12;
		double b6 = a20 * a31 - a21 * a30;
		double b7 = a20 * a32 - a22 * a30;
		double b8 = a20 * a33 - a23 * a30;
		double b9 = a21 * a32 - a22 * a31;
		double b10 = a21 * a33 - a23 * a31;
		double b11 = a22 * a33 - a23 * a32;
		return b00 * b11 - b1 * b10 + b2 * b9 + b3 * b8 - b4 * b7 + b5 * b6;
	}

	/// <summary>
	/// Multiplies two mat4's
	///
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the first operand</param>
	/// <param name="b">{mat4} b the second operand</param>
	/// <returns>{mat4} out</returns>
	public static double[] Multiply(double[] output, double[] a, double[] b)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double a20 = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		double a30 = a[12];
		double a31 = a[13];
		double a32 = a[14];
		double a33 = a[15];
		double b2 = b[0];
		double b3 = b[1];
		double b4 = b[2];
		double b5 = b[3];
		output[0] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[1] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[2] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[3] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		b2 = b[4];
		b3 = b[5];
		b4 = b[6];
		b5 = b[7];
		output[4] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[5] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[6] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[7] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		b2 = b[8];
		b3 = b[9];
		b4 = b[10];
		b5 = b[11];
		output[8] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[9] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[10] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[11] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		b2 = b[12];
		b3 = b[13];
		b4 = b[14];
		b5 = b[15];
		output[12] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[13] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[14] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[15] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		return output;
	}

	/// <summary>
	/// Multiplies two mat4's
	///
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the first operand</param>
	/// <param name="b">{mat4} b the second operand</param>
	/// <returns>{mat4} out</returns>
	public static double[] Multiply(double[] output, float[] a, double[] b)
	{
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double a20 = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		double a30 = a[12];
		double a31 = a[13];
		double a32 = a[14];
		double a33 = a[15];
		double b2 = b[0];
		double b3 = b[1];
		double b4 = b[2];
		double b5 = b[3];
		output[0] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[1] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[2] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[3] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		b2 = b[4];
		b3 = b[5];
		b4 = b[6];
		b5 = b[7];
		output[4] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[5] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[6] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[7] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		b2 = b[8];
		b3 = b[9];
		b4 = b[10];
		b5 = b[11];
		output[8] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[9] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[10] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[11] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		b2 = b[12];
		b3 = b[13];
		b4 = b[14];
		b5 = b[15];
		output[12] = b2 * a2 + b3 * a10 + b4 * a20 + b5 * a30;
		output[13] = b2 * a3 + b3 * a11 + b4 * a21 + b5 * a31;
		output[14] = b2 * a4 + b3 * a12 + b4 * a22 + b5 * a32;
		output[15] = b2 * a5 + b3 * a13 + b4 * a23 + b5 * a33;
		return output;
	}

	/// <summary>
	/// mat4.multiply
	/// </summary>
	/// <param name="output"></param>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static double[] Mul(double[] output, double[] a, double[] b)
	{
		return Multiply(output, a, b);
	}

	/// <summary>
	/// mat4.multiply
	/// </summary>
	/// <param name="output"></param>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	public static double[] Mul(double[] output, float[] a, double[] b)
	{
		return Multiply(output, a, b);
	}

	/// <summary>
	/// If we have a translation-only matrix - one with no rotation or scaling - return true.
	/// If the matrix includes some scaling or rotation components, return false.<br />
	/// The identity matrix returns true here because there is no scaling or rotation, even though the translation is zero in that special case.
	/// </summary>
	/// <param name="matrix"></param>
	/// <returns>true if a simple translation matrix was found, otherwise false</returns>
	public static bool IsTranslationOnly(double[] matrix)
	{
		if ((float)(matrix[1] + 1.0) != 1f || (float)(matrix[6] + 1.0) != 1f)
		{
			return false;
		}
		if ((float)(matrix[2] + 1.0) != 1f || (float)(matrix[4] + 1.0) != 1f)
		{
			return false;
		}
		if ((float)matrix[0] != 1f || (float)matrix[5] != 1f || (float)matrix[10] != 1f)
		{
			return false;
		}
		if ((float)(matrix[8] + 1.0) != 1f || (float)(matrix[9] + 1.0) != 1f)
		{
			return false;
		}
		return true;
	}

	/// <summary>
	/// Translate a mat4 by the given vector
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="input">{mat4} a the matrix to translate</param>
	/// <param name="x">{vec3} v vector to translate by</param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns>{mat4} out</returns>
	public static double[] Translate(double[] output, double[] input, double x, double y, double z)
	{
		if (input == output)
		{
			output[12] = input[0] * x + input[4] * y + input[8] * z + input[12];
			output[13] = input[1] * x + input[5] * y + input[9] * z + input[13];
			output[14] = input[2] * x + input[6] * y + input[10] * z + input[14];
			output[15] = input[3] * x + input[7] * y + input[11] * z + input[15];
		}
		else
		{
			double a00 = input[0];
			double a1 = input[1];
			double a2 = input[2];
			double a3 = input[3];
			double a10 = input[4];
			double a11 = input[5];
			double a12 = input[6];
			double a13 = input[7];
			double a20 = input[8];
			double a21 = input[9];
			double a22 = input[10];
			double a23 = input[11];
			output[0] = a00;
			output[1] = a1;
			output[2] = a2;
			output[3] = a3;
			output[4] = a10;
			output[5] = a11;
			output[6] = a12;
			output[7] = a13;
			output[8] = a20;
			output[9] = a21;
			output[10] = a22;
			output[11] = a23;
			output[12] = a00 * x + a10 * y + a20 * z + input[12];
			output[13] = a1 * x + a11 * y + a21 * z + input[13];
			output[14] = a2 * x + a12 * y + a22 * z + input[14];
			output[15] = a3 * x + a13 * y + a23 * z + input[15];
		}
		return output;
	}

	/// <summary>
	/// Translate a mat4 by the given vector
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="input">{mat4} a the matrix to translate</param>
	/// <param name="translate">{vec3} v vector to translate by</param>
	/// <returns>{mat4} out</returns>
	public static double[] Translate(double[] output, double[] input, double[] translate)
	{
		double x = translate[0];
		double y = translate[1];
		double z = translate[2];
		if (input == output)
		{
			output[12] = input[0] * x + input[4] * y + input[8] * z + input[12];
			output[13] = input[1] * x + input[5] * y + input[9] * z + input[13];
			output[14] = input[2] * x + input[6] * y + input[10] * z + input[14];
			output[15] = input[3] * x + input[7] * y + input[11] * z + input[15];
		}
		else
		{
			double a00 = input[0];
			double a1 = input[1];
			double a2 = input[2];
			double a3 = input[3];
			double a10 = input[4];
			double a11 = input[5];
			double a12 = input[6];
			double a13 = input[7];
			double a20 = input[8];
			double a21 = input[9];
			double a22 = input[10];
			double a23 = input[11];
			output[0] = a00;
			output[1] = a1;
			output[2] = a2;
			output[3] = a3;
			output[4] = a10;
			output[5] = a11;
			output[6] = a12;
			output[7] = a13;
			output[8] = a20;
			output[9] = a21;
			output[10] = a22;
			output[11] = a23;
			output[12] = a00 * x + a10 * y + a20 * z + input[12];
			output[13] = a1 * x + a11 * y + a21 * z + input[13];
			output[14] = a2 * x + a12 * y + a22 * z + input[14];
			output[15] = a3 * x + a13 * y + a23 * z + input[15];
		}
		return output;
	}

	/// <summary>
	/// Scales the mat4 by the dimensions in the given vec3
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the matrix to scale</param>
	/// <param name="v">{vec3} v the vec3 to scale the matrix by</param>
	/// <returns>{mat4} out</returns>
	public static double[] Scale(double[] output, double[] a, double[] v)
	{
		double x = v[0];
		double y = v[1];
		double z = v[2];
		output[0] = a[0] * x;
		output[1] = a[1] * x;
		output[2] = a[2] * x;
		output[3] = a[3] * x;
		output[4] = a[4] * y;
		output[5] = a[5] * y;
		output[6] = a[6] * y;
		output[7] = a[7] * y;
		output[8] = a[8] * z;
		output[9] = a[9] * z;
		output[10] = a[10] * z;
		output[11] = a[11] * z;
		output[12] = a[12];
		output[13] = a[13];
		output[14] = a[14];
		output[15] = a[15];
		return output;
	}

	public static void Scale(double[] matrix, double x, double y, double z)
	{
		matrix[0] *= x;
		matrix[1] *= x;
		matrix[2] *= x;
		matrix[3] *= x;
		matrix[4] *= y;
		matrix[5] *= y;
		matrix[6] *= y;
		matrix[7] *= y;
		matrix[8] *= z;
		matrix[9] *= z;
		matrix[10] *= z;
		matrix[11] *= z;
	}

	/// <summary>
	/// Rotates a mat4 by the given angle
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the matrix to rotate</param>
	/// <param name="rad">{Number} rad the angle to rotate the matrix by</param>
	/// <param name="axis">{vec3} axis the axis to rotate around</param>
	/// <returns>{mat4} out</returns>
	public static double[] Rotate(double[] output, double[] a, double rad, double[] axis)
	{
		double x = axis[0];
		double y = axis[1];
		double z = axis[2];
		return Rotate(output, a, rad, x, y, z);
	}

	/// <summary>
	/// Rotates a mat4 by the given angle
	/// </summary>
	/// <param name="output"></param>
	/// <param name="a"></param>
	/// <param name="rad"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static double[] Rotate(double[] output, double[] a, double rad, double x, double y, double z)
	{
		double len = GameMath.Sqrt(x * x + y * y + z * z);
		if (GlMatrixMathd.Abs(len) < GlMatrixMathd.GLMAT_EPSILON())
		{
			return null;
		}
		len = 1.0 / len;
		x *= len;
		y *= len;
		z *= len;
		double s = GameMath.Sin(rad);
		double c = GameMath.Cos(rad);
		double t = 1.0 - c;
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double a20 = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		double b00 = x * x * t + c;
		double b1 = y * x * t + z * s;
		double b2 = z * x * t - y * s;
		double b10 = x * y * t - z * s;
		double b11 = y * y * t + c;
		double b12 = z * y * t + x * s;
		double b20 = x * z * t + y * s;
		double b21 = y * z * t - x * s;
		double b22 = z * z * t + c;
		output[0] = a2 * b00 + a10 * b1 + a20 * b2;
		output[1] = a3 * b00 + a11 * b1 + a21 * b2;
		output[2] = a4 * b00 + a12 * b1 + a22 * b2;
		output[3] = a5 * b00 + a13 * b1 + a23 * b2;
		output[4] = a2 * b10 + a10 * b11 + a20 * b12;
		output[5] = a3 * b10 + a11 * b11 + a21 * b12;
		output[6] = a4 * b10 + a12 * b11 + a22 * b12;
		output[7] = a5 * b10 + a13 * b11 + a23 * b12;
		output[8] = a2 * b20 + a10 * b21 + a20 * b22;
		output[9] = a3 * b20 + a11 * b21 + a21 * b22;
		output[10] = a4 * b20 + a12 * b21 + a22 * b22;
		output[11] = a5 * b20 + a13 * b21 + a23 * b22;
		if (a != output)
		{
			output[12] = a[12];
			output[13] = a[13];
			output[14] = a[14];
			output[15] = a[15];
		}
		return output;
	}

	/// <summary>
	/// Rotates a matrix by the given angle around the X axis
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the matrix to rotate</param>
	/// <param name="rad">{Number} rad the angle to rotate the matrix by</param>
	/// <returns>{mat4} out</returns>
	public static double[] RotateX(double[] output, double[] a, double rad)
	{
		double s = GameMath.Sin(rad);
		double c = GameMath.Cos(rad);
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		double a20 = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		if (a != output)
		{
			output[0] = a[0];
			output[1] = a[1];
			output[2] = a[2];
			output[3] = a[3];
			output[12] = a[12];
			output[13] = a[13];
			output[14] = a[14];
			output[15] = a[15];
		}
		output[4] = a10 * c + a20 * s;
		output[5] = a11 * c + a21 * s;
		output[6] = a12 * c + a22 * s;
		output[7] = a13 * c + a23 * s;
		output[8] = a20 * c - a10 * s;
		output[9] = a21 * c - a11 * s;
		output[10] = a22 * c - a12 * s;
		output[11] = a23 * c - a13 * s;
		return output;
	}

	/// <summary>
	/// Rotates a matrix by the given angle around the Y axis
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the matrix to rotate</param>
	/// <param name="rad">{Number} rad the angle to rotate the matrix by</param>
	/// <returns>{mat4} out</returns>
	public static double[] RotateY(double[] output, double[] a, double rad)
	{
		double s = GameMath.Sin(rad);
		double c = GameMath.Cos(rad);
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a20 = a[8];
		double a21 = a[9];
		double a22 = a[10];
		double a23 = a[11];
		if (a != output)
		{
			output[4] = a[4];
			output[5] = a[5];
			output[6] = a[6];
			output[7] = a[7];
			output[12] = a[12];
			output[13] = a[13];
			output[14] = a[14];
			output[15] = a[15];
		}
		output[0] = a2 * c - a20 * s;
		output[1] = a3 * c - a21 * s;
		output[2] = a4 * c - a22 * s;
		output[3] = a5 * c - a23 * s;
		output[8] = a2 * s + a20 * c;
		output[9] = a3 * s + a21 * c;
		output[10] = a4 * s + a22 * c;
		output[11] = a5 * s + a23 * c;
		return output;
	}

	/// <summary>
	/// Rotates a matrix by the given angle around the Z axis
	/// </summary>
	/// <param name="output">{mat4} out the receiving matrix</param>
	/// <param name="a">{mat4} a the matrix to rotate</param>
	/// <param name="rad">{Number} rad the angle to rotate the matrix by</param>
	/// <returns>{mat4} out</returns>
	public static double[] RotateZ(double[] output, double[] a, double rad)
	{
		double s = GameMath.Sin(rad);
		double c = GameMath.Cos(rad);
		double a2 = a[0];
		double a3 = a[1];
		double a4 = a[2];
		double a5 = a[3];
		double a10 = a[4];
		double a11 = a[5];
		double a12 = a[6];
		double a13 = a[7];
		if (a != output)
		{
			output[8] = a[8];
			output[9] = a[9];
			output[10] = a[10];
			output[11] = a[11];
			output[12] = a[12];
			output[13] = a[13];
			output[14] = a[14];
			output[15] = a[15];
		}
		output[0] = a2 * c + a10 * s;
		output[1] = a3 * c + a11 * s;
		output[2] = a4 * c + a12 * s;
		output[3] = a5 * c + a13 * s;
		output[4] = a10 * c - a2 * s;
		output[5] = a11 * c - a3 * s;
		output[6] = a12 * c - a4 * s;
		output[7] = a13 * c - a5 * s;
		return output;
	}

	/// <summary>
	/// Creates a matrix from a quaternion rotation and vector translation
	/// This is equivalent to (but much faster than):
	///     mat4.identity(dest);
	///     mat4.translate(dest, vec);
	///     var quatMat = mat4.create();
	///     quat4.toMat4(quat, quatMat);
	///     mat4.multiply(dest, quatMat);
	/// </summary>
	/// <param name="output">{mat4} out mat4 receiving operation result</param>
	/// <param name="q">{quat4} q Rotation quaternion</param>
	/// <param name="v">{vec3} v Translation vector</param>
	/// <returns>{mat4} out</returns>
	public static double[] FromRotationTranslation(double[] output, double[] q, double[] v)
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
		output[1] = xy + wz;
		output[2] = xz - wy;
		output[3] = 0.0;
		output[4] = xy - wz;
		output[5] = 1.0 - (xx + zz);
		output[6] = yz + wx;
		output[7] = 0.0;
		output[8] = xz + wy;
		output[9] = yz - wx;
		output[10] = 1.0 - (xx + yy);
		output[11] = 0.0;
		output[12] = v[0];
		output[13] = v[1];
		output[14] = v[2];
		output[15] = 1.0;
		return output;
	}

	/// <summary>
	/// Calculates a 4x4 matrix from the given quaternion
	/// </summary>
	/// <param name="output">{mat4} out mat4 receiving operation result</param>
	/// <param name="q">{quat} q Quaternion to create matrix from</param>
	/// <returns>{mat4} out</returns>
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
		output[1] = xy + wz;
		output[2] = xz - wy;
		output[3] = 0.0;
		output[4] = xy - wz;
		output[5] = 1.0 - (xx + zz);
		output[6] = yz + wx;
		output[7] = 0.0;
		output[8] = xz + wy;
		output[9] = yz - wx;
		output[10] = 1.0 - (xx + yy);
		output[11] = 0.0;
		output[12] = 0.0;
		output[13] = 0.0;
		output[14] = 0.0;
		output[15] = 1.0;
		return output;
	}

	/// <summary>
	/// Generates a frustum matrix with the given bounds
	/// </summary>
	/// <param name="output">{mat4} out mat4 frustum matrix will be written into</param>
	/// <param name="left">{Number} left Left bound of the frustum</param>
	/// <param name="right">{Number} right Right bound of the frustum</param>
	/// <param name="bottom">{Number} bottom Bottom bound of the frustum</param>
	/// <param name="top">{Number} top Top bound of the frustum</param>
	/// <param name="near">{Number} near Near bound of the frustum</param>
	/// <param name="far">{Number} far Far bound of the frustum</param>
	/// <returns>{mat4} out</returns>
	public static double[] Frustum(double[] output, double left, double right, double bottom, double top, double near, double far)
	{
		double rl = 1.0 / (right - left);
		double tb = 1.0 / (top - bottom);
		double nf = 1.0 / (near - far);
		output[0] = near * 2.0 * rl;
		output[1] = 0.0;
		output[2] = 0.0;
		output[3] = 0.0;
		output[4] = 0.0;
		output[5] = near * 2.0 * tb;
		output[6] = 0.0;
		output[7] = 0.0;
		output[8] = (right + left) * rl;
		output[9] = (top + bottom) * tb;
		output[10] = (far + near) * nf;
		output[11] = -1.0;
		output[12] = 0.0;
		output[13] = 0.0;
		output[14] = far * near * 2.0 * nf;
		output[15] = 0.0;
		return output;
	}

	/// <summary>
	/// Generates a perspective projection matrix with the given bounds
	/// </summary>
	/// <param name="output">{mat4} out mat4 frustum matrix will be written into</param>
	/// <param name="fovy">{number} fovy Vertical field of view in radians</param>
	/// <param name="aspect">{number} aspect Aspect ratio. typically viewport width/height</param>
	/// <param name="near">{number} near Near bound of the frustum</param>
	/// <param name="far">{number} far Far bound of the frustum</param>
	/// <returns>{mat4} out</returns>
	public static double[] Perspective(double[] output, double fovy, double aspect, double near, double far)
	{
		double f = 1.0 / GameMath.Tan(fovy / 2.0);
		double nf = 1.0 / (near - far);
		output[0] = f / aspect;
		output[1] = 0.0;
		output[2] = 0.0;
		output[3] = 0.0;
		output[4] = 0.0;
		output[5] = f;
		output[6] = 0.0;
		output[7] = 0.0;
		output[8] = 0.0;
		output[9] = 0.0;
		output[10] = (far + near) * nf;
		output[11] = -1.0;
		output[12] = 0.0;
		output[13] = 0.0;
		output[14] = 2.0 * far * near * nf;
		output[15] = 0.0;
		return output;
	}

	/// <summary>
	/// Generates a orthogonal projection matrix with the given bounds
	/// </summary>
	/// <param name="output">{mat4} out mat4 frustum matrix will be written into</param>
	/// <param name="left">{number} left Left bound of the frustum</param>
	/// <param name="right">{number} right Right bound of the frustum</param>
	/// <param name="bottom">{number} bottom Bottom bound of the frustum</param>
	/// <param name="top">{number} top Top bound of the frustum</param>
	/// <param name="near">{number} near Near bound of the frustum</param>
	/// <param name="far">{number} far Far bound of the frustum</param>
	/// <returns>{mat4} out</returns>
	public static double[] Ortho(double[] output, double left, double right, double bottom, double top, double near, double far)
	{
		double lr = 1.0 / (left - right);
		double bt = 1.0 / (bottom - top);
		double nf = 1.0 / (near - far);
		output[0] = -2.0 * lr;
		output[1] = 0.0;
		output[2] = 0.0;
		output[3] = 0.0;
		output[4] = 0.0;
		output[5] = -2.0 * bt;
		output[6] = 0.0;
		output[7] = 0.0;
		output[8] = 0.0;
		output[9] = 0.0;
		output[10] = 2.0 * nf;
		output[11] = 0.0;
		output[12] = (left + right) * lr;
		output[13] = (top + bottom) * bt;
		output[14] = (far + near) * nf;
		output[15] = 1.0;
		return output;
	}

	/// <summary>
	/// Generates a look-at matrix with the given eye position, focal point, and up axis
	/// </summary>
	/// <param name="output">{mat4} out mat4 frustum matrix will be written into</param>
	/// <param name="eye">{vec3} eye Position of the viewer</param>
	/// <param name="center">{vec3} center Point the viewer is looking at</param>
	/// <param name="up">{vec3} up vec3 pointing up</param>
	/// <returns>{mat4} out</returns>
	public static double[] LookAt(double[] output, double[] eye, double[] center, double[] up)
	{
		double eyex = eye[0];
		double eyey = eye[1];
		double eyez = eye[2];
		double upx = up[0];
		double upy = up[1];
		double upz = up[2];
		double centerx = center[0];
		double centery = center[1];
		double centerz = center[2];
		if (GlMatrixMathd.Abs(eyex - centerx) < GlMatrixMathd.GLMAT_EPSILON() && GlMatrixMathd.Abs(eyey - centery) < GlMatrixMathd.GLMAT_EPSILON() && GlMatrixMathd.Abs(eyez - centerz) < GlMatrixMathd.GLMAT_EPSILON())
		{
			return Identity(output);
		}
		double z0 = eyex - centerx;
		double z1 = eyey - centery;
		double z2 = eyez - centerz;
		double len = 1f / GameMath.Sqrt(z0 * z0 + z1 * z1 + z2 * z2);
		z0 *= len;
		z1 *= len;
		z2 *= len;
		double x0 = upy * z2 - upz * z1;
		double x1 = upz * z0 - upx * z2;
		double x2 = upx * z1 - upy * z0;
		len = GameMath.Sqrt(x0 * x0 + x1 * x1 + x2 * x2);
		if (len == 0.0)
		{
			x0 = 0.0;
			x1 = 0.0;
			x2 = 0.0;
		}
		else
		{
			len = 1.0 / len;
			x0 *= len;
			x1 *= len;
			x2 *= len;
		}
		double y0 = z1 * x2 - z2 * x1;
		double y1 = z2 * x0 - z0 * x2;
		double y2 = z0 * x1 - z1 * x0;
		len = GameMath.Sqrt(y0 * y0 + y1 * y1 + y2 * y2);
		if (len == 0.0)
		{
			y0 = 0.0;
			y1 = 0.0;
			y2 = 0.0;
		}
		else
		{
			len = 1.0 / len;
			y0 *= len;
			y1 *= len;
			y2 *= len;
		}
		output[0] = x0;
		output[1] = y0;
		output[2] = z0;
		output[3] = 0.0;
		output[4] = x1;
		output[5] = y1;
		output[6] = z1;
		output[7] = 0.0;
		output[8] = x2;
		output[9] = y2;
		output[10] = z2;
		output[11] = 0.0;
		output[12] = 0.0 - (x0 * eyex + x1 * eyey + x2 * eyez);
		output[13] = 0.0 - (y0 * eyex + y1 * eyey + y2 * eyez);
		output[14] = 0.0 - (z0 * eyex + z1 * eyey + z2 * eyez);
		output[15] = 1.0;
		return output;
	}

	/// <summary>
	/// Multiply the matrix with a vec4. Reference: http://mathinsight.org/matrix_vector_multiplication
	/// Returns a new vec4 vector
	/// </summary>
	/// <param name="matrix"></param>
	/// <param name="vec4"></param>
	/// <returns></returns>
	public static double[] MulWithVec4(double[] matrix, double[] vec4)
	{
		double[] output = new double[4];
		for (int row = 0; row < 4; row++)
		{
			for (int col = 0; col < 4; col++)
			{
				output[row] += matrix[4 * col + row] * vec4[col];
			}
		}
		return output;
	}

	/// <summary>
	/// Multiply the matrix with a vec4. Reference: http://mathinsight.org/matrix_vector_multiplication
	/// </summary>
	/// <param name="matrix"></param>
	/// <param name="vec4"></param>
	/// <param name="outVal"></param>
	/// <returns></returns>
	public static void MulWithVec4(double[] matrix, double[] vec4, Vec4d outVal)
	{
		outVal.Set(0.0, 0.0, 0.0, 0.0);
		for (int row = 0; row < 4; row++)
		{
			for (int col = 0; col < 4; col++)
			{
				outVal[row] += matrix[4 * col + row] * vec4[col];
			}
		}
	}

	/// <summary>
	/// Multiply the matrix with a vec4. Reference: http://mathinsight.org/matrix_vector_multiplication
	///
	/// </summary>
	/// <param name="matrix"></param>
	/// <param name="inVal"></param>
	/// <param name="outVal"></param>
	/// <returns></returns>
	public static void MulWithVec4(double[] matrix, Vec4d inVal, Vec4d outVal)
	{
		outVal.Set(0.0, 0.0, 0.0, 0.0);
		for (int row = 0; row < 4; row++)
		{
			for (int col = 0; col < 4; col++)
			{
				outVal[row] += matrix[4 * col + row] * inVal[col];
			}
		}
	}
}
