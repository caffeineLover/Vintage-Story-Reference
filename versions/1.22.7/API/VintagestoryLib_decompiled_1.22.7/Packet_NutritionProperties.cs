public class Packet_NutritionProperties
{
	public int FoodCategory;

	public int Satiety;

	public int Health;

	public byte[] EatenStack;

	public int Intoxication;

	public int Psychedelic;

	public int SaturationLossDelay;

	public const int FoodCategoryFieldID = 1;

	public const int SatietyFieldID = 2;

	public const int HealthFieldID = 3;

	public const int EatenStackFieldID = 4;

	public const int IntoxicationFieldID = 5;

	public const int PsychedelicFieldID = 6;

	public const int SaturationLossDelayFieldID = 7;

	public int size;

	public void SetFoodCategory(int value)
	{
		FoodCategory = value;
	}

	public void SetSatiety(int value)
	{
		Satiety = value;
	}

	public void SetHealth(int value)
	{
		Health = value;
	}

	public void SetEatenStack(byte[] value)
	{
		EatenStack = value;
	}

	public void SetIntoxication(int value)
	{
		Intoxication = value;
	}

	public void SetPsychedelic(int value)
	{
		Psychedelic = value;
	}

	public void SetSaturationLossDelay(int value)
	{
		SaturationLossDelay = value;
	}

	internal void InitializeValues()
	{
	}
}
