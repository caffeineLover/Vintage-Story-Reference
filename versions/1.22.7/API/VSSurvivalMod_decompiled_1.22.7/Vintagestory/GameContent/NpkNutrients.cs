namespace Vintagestory.GameContent;

public class NpkNutrients
{
	public float N;

	public float P;

	public float K;

	public NpkNutrients(float n, float p, float k)
	{
		N = n;
		P = p;
		K = k;
	}

	public NpkNutrients()
	{
	}

	public NpkNutrients Clone()
	{
		return new NpkNutrients(N, P, K);
	}

	public static NpkNutrients operator *(NpkNutrients left, float right)
	{
		return new NpkNutrients(left.N * right, left.P * right, left.K * right);
	}
}
