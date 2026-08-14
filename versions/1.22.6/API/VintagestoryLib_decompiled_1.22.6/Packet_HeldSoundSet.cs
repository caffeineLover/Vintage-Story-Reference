public class Packet_HeldSoundSet
{
	public Packet_SoundAttributes Idle;

	public Packet_SoundAttributes Equip;

	public Packet_SoundAttributes Unequip;

	public Packet_SoundAttributes Attack;

	public Packet_SoundAttributes InvPickup;

	public Packet_SoundAttributes InvPlace;

	public const int IdleFieldID = 1;

	public const int EquipFieldID = 2;

	public const int UnequipFieldID = 3;

	public const int AttackFieldID = 4;

	public const int InvPickupFieldID = 5;

	public const int InvPlaceFieldID = 6;

	public int size;

	public void SetIdle(Packet_SoundAttributes value)
	{
		Idle = value;
	}

	public void SetEquip(Packet_SoundAttributes value)
	{
		Equip = value;
	}

	public void SetUnequip(Packet_SoundAttributes value)
	{
		Unequip = value;
	}

	public void SetAttack(Packet_SoundAttributes value)
	{
		Attack = value;
	}

	public void SetInvPickup(Packet_SoundAttributes value)
	{
		InvPickup = value;
	}

	public void SetInvPlace(Packet_SoundAttributes value)
	{
		InvPlace = value;
	}

	internal void InitializeValues()
	{
	}
}
