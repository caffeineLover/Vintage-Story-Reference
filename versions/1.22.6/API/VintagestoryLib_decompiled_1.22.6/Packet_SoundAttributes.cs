public class Packet_SoundAttributes
{
	public string Location;

	public Packet_NatFloat Pitch;

	public Packet_NatFloat Volume;

	public int Type;

	public int Range;

	public const int LocationFieldID = 1;

	public const int PitchFieldID = 2;

	public const int VolumeFieldID = 3;

	public const int TypeFieldID = 4;

	public const int RangeFieldID = 5;

	public int size;

	public void SetLocation(string value)
	{
		Location = value;
	}

	public void SetPitch(Packet_NatFloat value)
	{
		Pitch = value;
	}

	public void SetVolume(Packet_NatFloat value)
	{
		Volume = value;
	}

	public void SetType(int value)
	{
		Type = value;
	}

	public void SetRange(int value)
	{
		Range = value;
	}

	internal void InitializeValues()
	{
	}
}
