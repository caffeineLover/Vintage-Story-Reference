public class Packet_TagRegistry
{
	public string RegistryName;

	public string[] TagNames;

	public int TagNamesCount;

	public int TagNamesLength;

	public const int RegistryNameFieldID = 1;

	public const int TagNamesFieldID = 2;

	public int size;

	public void SetRegistryName(string value)
	{
		RegistryName = value;
	}

	public string[] GetTagNames()
	{
		return TagNames;
	}

	public void SetTagNames(string[] value, int count, int length)
	{
		TagNames = value;
		TagNamesCount = count;
		TagNamesLength = length;
	}

	public void SetTagNames(string[] value)
	{
		TagNames = value;
		TagNamesCount = value.Length;
		TagNamesLength = value.Length;
	}

	public int GetTagNamesCount()
	{
		return TagNamesCount;
	}

	public void TagNamesAdd(string value)
	{
		if (TagNamesCount >= TagNamesLength)
		{
			if ((TagNamesLength *= 2) == 0)
			{
				TagNamesLength = 1;
			}
			string[] newArray = new string[TagNamesLength];
			for (int i = 0; i < TagNamesCount; i++)
			{
				newArray[i] = TagNames[i];
			}
			TagNames = newArray;
		}
		TagNames[TagNamesCount++] = value;
	}

	internal void InitializeValues()
	{
	}
}
