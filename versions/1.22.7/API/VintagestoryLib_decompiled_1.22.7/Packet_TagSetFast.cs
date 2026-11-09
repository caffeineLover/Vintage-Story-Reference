public class Packet_TagSetFast
{
	public long Part1;

	public long Part2;

	public long Part3;

	public long Part4;

	public const int Part1FieldID = 1;

	public const int Part2FieldID = 2;

	public const int Part3FieldID = 3;

	public const int Part4FieldID = 4;

	public int size;

	public void SetPart1(long value)
	{
		Part1 = value;
	}

	public void SetPart2(long value)
	{
		Part2 = value;
	}

	public void SetPart3(long value)
	{
		Part3 = value;
	}

	public void SetPart4(long value)
	{
		Part4 = value;
	}

	internal void InitializeValues()
	{
	}
}
