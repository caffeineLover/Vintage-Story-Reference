using System;

public class Packet_ClientSerializer
{
	private const int field = 8;

	public static Packet_Client DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_Client instance = new Packet_Client();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_Client DeserializeBuffer(byte[] buffer, int length, Packet_Client instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_Client Deserialize(CitoMemoryStream stream, Packet_Client instance)
	{
		instance.InitializeValues();
		int keyInt;
		while (true)
		{
			keyInt = stream.ReadByte();
			if ((keyInt & 0x80) != 0)
			{
				keyInt = ProtocolParser.ReadKeyAsInt(keyInt, stream);
				if ((keyInt & 0x4000) != 0)
				{
					break;
				}
			}
			switch (keyInt)
			{
			case 0:
				return null;
			case 266:
				if (instance.LoginTokenQuery == null)
				{
					instance.LoginTokenQuery = Packet_LoginTokenQuerySerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_LoginTokenQuerySerializer.DeserializeLengthDelimited(stream, instance.LoginTokenQuery);
				}
				break;
			case 8:
				instance.Id = ProtocolParser.ReadUInt32(stream);
				break;
			case 18:
				if (instance.Identification == null)
				{
					instance.Identification = Packet_ClientIdentificationSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientIdentificationSerializer.DeserializeLengthDelimited(stream, instance.Identification);
				}
				break;
			case 26:
				if (instance.BlockPlaceOrBreak == null)
				{
					instance.BlockPlaceOrBreak = Packet_ClientBlockPlaceOrBreakSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientBlockPlaceOrBreakSerializer.DeserializeLengthDelimited(stream, instance.BlockPlaceOrBreak);
				}
				break;
			case 34:
				if (instance.Chatline == null)
				{
					instance.Chatline = Packet_ChatLineSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ChatLineSerializer.DeserializeLengthDelimited(stream, instance.Chatline);
				}
				break;
			case 42:
				if (instance.RequestJoin == null)
				{
					instance.RequestJoin = Packet_ClientRequestJoinSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientRequestJoinSerializer.DeserializeLengthDelimited(stream, instance.RequestJoin);
				}
				break;
			case 50:
				if (instance.PingReply == null)
				{
					instance.PingReply = Packet_ClientPingReplySerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientPingReplySerializer.DeserializeLengthDelimited(stream, instance.PingReply);
				}
				break;
			case 58:
				if (instance.SpecialKey_ == null)
				{
					instance.SpecialKey_ = Packet_ClientSpecialKeySerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientSpecialKeySerializer.DeserializeLengthDelimited(stream, instance.SpecialKey_);
				}
				break;
			case 66:
				if (instance.SelectedHotbarSlot == null)
				{
					instance.SelectedHotbarSlot = Packet_SelectedHotbarSlotSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SelectedHotbarSlotSerializer.DeserializeLengthDelimited(stream, instance.SelectedHotbarSlot);
				}
				break;
			case 74:
				if (instance.Leave == null)
				{
					instance.Leave = Packet_ClientLeaveSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientLeaveSerializer.DeserializeLengthDelimited(stream, instance.Leave);
				}
				break;
			case 82:
				if (instance.Query == null)
				{
					instance.Query = Packet_ClientServerQuerySerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientServerQuerySerializer.DeserializeLengthDelimited(stream, instance.Query);
				}
				break;
			case 114:
				if (instance.MoveItemstack == null)
				{
					instance.MoveItemstack = Packet_MoveItemstackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_MoveItemstackSerializer.DeserializeLengthDelimited(stream, instance.MoveItemstack);
				}
				break;
			case 122:
				if (instance.Flipitemstacks == null)
				{
					instance.Flipitemstacks = Packet_FlipItemstacksSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_FlipItemstacksSerializer.DeserializeLengthDelimited(stream, instance.Flipitemstacks);
				}
				break;
			case 130:
				if (instance.EntityInteraction == null)
				{
					instance.EntityInteraction = Packet_EntityInteractionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityInteractionSerializer.DeserializeLengthDelimited(stream, instance.EntityInteraction);
				}
				break;
			case 146:
				if (instance.EntityPosition == null)
				{
					instance.EntityPosition = Packet_EntityPositionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityPositionSerializer.DeserializeLengthDelimited(stream, instance.EntityPosition);
				}
				break;
			case 154:
				if (instance.ActivateInventorySlot == null)
				{
					instance.ActivateInventorySlot = Packet_ActivateInventorySlotSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ActivateInventorySlotSerializer.DeserializeLengthDelimited(stream, instance.ActivateInventorySlot);
				}
				break;
			case 162:
				if (instance.CreateItemstack == null)
				{
					instance.CreateItemstack = Packet_CreateItemstackSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CreateItemstackSerializer.DeserializeLengthDelimited(stream, instance.CreateItemstack);
				}
				break;
			case 170:
				if (instance.RequestModeChange == null)
				{
					instance.RequestModeChange = Packet_PlayerModeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_PlayerModeSerializer.DeserializeLengthDelimited(stream, instance.RequestModeChange);
				}
				break;
			case 178:
				if (instance.MoveKeyChange == null)
				{
					instance.MoveKeyChange = Packet_MoveKeyChangeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_MoveKeyChangeSerializer.DeserializeLengthDelimited(stream, instance.MoveKeyChange);
				}
				break;
			case 186:
				if (instance.BlockEntityPacket == null)
				{
					instance.BlockEntityPacket = Packet_BlockEntityPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BlockEntityPacketSerializer.DeserializeLengthDelimited(stream, instance.BlockEntityPacket);
				}
				break;
			case 250:
				if (instance.EntityPacket == null)
				{
					instance.EntityPacket = Packet_EntityPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityPacketSerializer.DeserializeLengthDelimited(stream, instance.EntityPacket);
				}
				break;
			case 194:
				if (instance.CustomPacket == null)
				{
					instance.CustomPacket = Packet_CustomPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CustomPacketSerializer.DeserializeLengthDelimited(stream, instance.CustomPacket);
				}
				break;
			case 202:
				if (instance.HandInteraction == null)
				{
					instance.HandInteraction = Packet_ClientHandInteractionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientHandInteractionSerializer.DeserializeLengthDelimited(stream, instance.HandInteraction);
				}
				break;
			case 210:
				if (instance.ToolMode == null)
				{
					instance.ToolMode = Packet_ToolModeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ToolModeSerializer.DeserializeLengthDelimited(stream, instance.ToolMode);
				}
				break;
			case 218:
				if (instance.BlockDamage == null)
				{
					instance.BlockDamage = Packet_BlockDamageSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BlockDamageSerializer.DeserializeLengthDelimited(stream, instance.BlockDamage);
				}
				break;
			case 226:
				if (instance.ClientPlaying == null)
				{
					instance.ClientPlaying = Packet_ClientPlayingSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ClientPlayingSerializer.DeserializeLengthDelimited(stream, instance.ClientPlaying);
				}
				break;
			case 242:
				if (instance.InvOpenedClosed == null)
				{
					instance.InvOpenedClosed = Packet_InvOpenCloseSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_InvOpenCloseSerializer.DeserializeLengthDelimited(stream, instance.InvOpenedClosed);
				}
				break;
			case 258:
				if (instance.RuntimeSetting == null)
				{
					instance.RuntimeSetting = Packet_RuntimeSettingSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_RuntimeSettingSerializer.DeserializeLengthDelimited(stream, instance.RuntimeSetting);
				}
				break;
			case 274:
				if (instance.UdpPacket == null)
				{
					instance.UdpPacket = Packet_UdpPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_UdpPacketSerializer.DeserializeLengthDelimited(stream, instance.UdpPacket);
				}
				break;
			default:
				ProtocolParser.SkipKey(stream, Key.Create(keyInt));
				break;
			}
		}
		if (keyInt >= 0)
		{
			return null;
		}
		return instance;
	}

	public static Packet_Client DeserializeLengthDelimited(CitoMemoryStream stream, Packet_Client instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_Client result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_Client instance)
	{
		if (instance.LoginTokenQuery != null)
		{
			stream.WriteKey(33, 2);
			Packet_LoginTokenQuery i33 = instance.LoginTokenQuery;
			Packet_LoginTokenQuerySerializer.GetSize(i33);
			Packet_LoginTokenQuerySerializer.SerializeWithSize(stream, i33);
		}
		if (instance.Id != 1)
		{
			stream.WriteByte(8);
			ProtocolParser.WriteUInt32(stream, instance.Id);
		}
		if (instance.Identification != null)
		{
			stream.WriteByte(18);
			Packet_ClientIdentification i34 = instance.Identification;
			Packet_ClientIdentificationSerializer.GetSize(i34);
			Packet_ClientIdentificationSerializer.SerializeWithSize(stream, i34);
		}
		if (instance.BlockPlaceOrBreak != null)
		{
			stream.WriteByte(26);
			Packet_ClientBlockPlaceOrBreak i35 = instance.BlockPlaceOrBreak;
			Packet_ClientBlockPlaceOrBreakSerializer.GetSize(i35);
			Packet_ClientBlockPlaceOrBreakSerializer.SerializeWithSize(stream, i35);
		}
		if (instance.Chatline != null)
		{
			stream.WriteByte(34);
			Packet_ChatLine i36 = instance.Chatline;
			Packet_ChatLineSerializer.GetSize(i36);
			Packet_ChatLineSerializer.SerializeWithSize(stream, i36);
		}
		if (instance.RequestJoin != null)
		{
			stream.WriteByte(42);
			Packet_ClientRequestJoin i37 = instance.RequestJoin;
			Packet_ClientRequestJoinSerializer.GetSize(i37);
			Packet_ClientRequestJoinSerializer.SerializeWithSize(stream, i37);
		}
		if (instance.PingReply != null)
		{
			stream.WriteByte(50);
			Packet_ClientPingReply i38 = instance.PingReply;
			Packet_ClientPingReplySerializer.GetSize(i38);
			Packet_ClientPingReplySerializer.SerializeWithSize(stream, i38);
		}
		if (instance.SpecialKey_ != null)
		{
			stream.WriteByte(58);
			Packet_ClientSpecialKey i39 = instance.SpecialKey_;
			Packet_ClientSpecialKeySerializer.GetSize(i39);
			Packet_ClientSpecialKeySerializer.SerializeWithSize(stream, i39);
		}
		if (instance.SelectedHotbarSlot != null)
		{
			stream.WriteByte(66);
			Packet_SelectedHotbarSlot i40 = instance.SelectedHotbarSlot;
			Packet_SelectedHotbarSlotSerializer.GetSize(i40);
			Packet_SelectedHotbarSlotSerializer.SerializeWithSize(stream, i40);
		}
		if (instance.Leave != null)
		{
			stream.WriteByte(74);
			Packet_ClientLeave i41 = instance.Leave;
			Packet_ClientLeaveSerializer.GetSize(i41);
			Packet_ClientLeaveSerializer.SerializeWithSize(stream, i41);
		}
		if (instance.Query != null)
		{
			stream.WriteByte(82);
			Packet_ClientServerQuery i42 = instance.Query;
			Packet_ClientServerQuerySerializer.GetSize(i42);
			Packet_ClientServerQuerySerializer.SerializeWithSize(stream, i42);
		}
		if (instance.MoveItemstack != null)
		{
			stream.WriteByte(114);
			Packet_MoveItemstack i43 = instance.MoveItemstack;
			Packet_MoveItemstackSerializer.GetSize(i43);
			Packet_MoveItemstackSerializer.SerializeWithSize(stream, i43);
		}
		if (instance.Flipitemstacks != null)
		{
			stream.WriteByte(122);
			Packet_FlipItemstacks i44 = instance.Flipitemstacks;
			Packet_FlipItemstacksSerializer.GetSize(i44);
			Packet_FlipItemstacksSerializer.SerializeWithSize(stream, i44);
		}
		if (instance.EntityInteraction != null)
		{
			stream.WriteKey(16, 2);
			Packet_EntityInteraction i45 = instance.EntityInteraction;
			Packet_EntityInteractionSerializer.GetSize(i45);
			Packet_EntityInteractionSerializer.SerializeWithSize(stream, i45);
		}
		if (instance.EntityPosition != null)
		{
			stream.WriteKey(18, 2);
			Packet_EntityPosition i46 = instance.EntityPosition;
			Packet_EntityPositionSerializer.GetSize(i46);
			Packet_EntityPositionSerializer.SerializeWithSize(stream, i46);
		}
		if (instance.ActivateInventorySlot != null)
		{
			stream.WriteKey(19, 2);
			Packet_ActivateInventorySlot i47 = instance.ActivateInventorySlot;
			Packet_ActivateInventorySlotSerializer.GetSize(i47);
			Packet_ActivateInventorySlotSerializer.SerializeWithSize(stream, i47);
		}
		if (instance.CreateItemstack != null)
		{
			stream.WriteKey(20, 2);
			Packet_CreateItemstack i48 = instance.CreateItemstack;
			Packet_CreateItemstackSerializer.GetSize(i48);
			Packet_CreateItemstackSerializer.SerializeWithSize(stream, i48);
		}
		if (instance.RequestModeChange != null)
		{
			stream.WriteKey(21, 2);
			Packet_PlayerMode i49 = instance.RequestModeChange;
			Packet_PlayerModeSerializer.GetSize(i49);
			Packet_PlayerModeSerializer.SerializeWithSize(stream, i49);
		}
		if (instance.MoveKeyChange != null)
		{
			stream.WriteKey(22, 2);
			Packet_MoveKeyChange i50 = instance.MoveKeyChange;
			Packet_MoveKeyChangeSerializer.GetSize(i50);
			Packet_MoveKeyChangeSerializer.SerializeWithSize(stream, i50);
		}
		if (instance.BlockEntityPacket != null)
		{
			stream.WriteKey(23, 2);
			Packet_BlockEntityPacket i51 = instance.BlockEntityPacket;
			Packet_BlockEntityPacketSerializer.GetSize(i51);
			Packet_BlockEntityPacketSerializer.SerializeWithSize(stream, i51);
		}
		if (instance.EntityPacket != null)
		{
			stream.WriteKey(31, 2);
			Packet_EntityPacket i52 = instance.EntityPacket;
			Packet_EntityPacketSerializer.GetSize(i52);
			Packet_EntityPacketSerializer.SerializeWithSize(stream, i52);
		}
		if (instance.CustomPacket != null)
		{
			stream.WriteKey(24, 2);
			Packet_CustomPacket i53 = instance.CustomPacket;
			Packet_CustomPacketSerializer.GetSize(i53);
			Packet_CustomPacketSerializer.SerializeWithSize(stream, i53);
		}
		if (instance.HandInteraction != null)
		{
			stream.WriteKey(25, 2);
			Packet_ClientHandInteraction i54 = instance.HandInteraction;
			Packet_ClientHandInteractionSerializer.GetSize(i54);
			Packet_ClientHandInteractionSerializer.SerializeWithSize(stream, i54);
		}
		if (instance.ToolMode != null)
		{
			stream.WriteKey(26, 2);
			Packet_ToolMode i55 = instance.ToolMode;
			Packet_ToolModeSerializer.GetSize(i55);
			Packet_ToolModeSerializer.SerializeWithSize(stream, i55);
		}
		if (instance.BlockDamage != null)
		{
			stream.WriteKey(27, 2);
			Packet_BlockDamage i56 = instance.BlockDamage;
			Packet_BlockDamageSerializer.GetSize(i56);
			Packet_BlockDamageSerializer.SerializeWithSize(stream, i56);
		}
		if (instance.ClientPlaying != null)
		{
			stream.WriteKey(28, 2);
			Packet_ClientPlaying i57 = instance.ClientPlaying;
			Packet_ClientPlayingSerializer.GetSize(i57);
			Packet_ClientPlayingSerializer.SerializeWithSize(stream, i57);
		}
		if (instance.InvOpenedClosed != null)
		{
			stream.WriteKey(30, 2);
			Packet_InvOpenClose i58 = instance.InvOpenedClosed;
			Packet_InvOpenCloseSerializer.GetSize(i58);
			Packet_InvOpenCloseSerializer.SerializeWithSize(stream, i58);
		}
		if (instance.RuntimeSetting != null)
		{
			stream.WriteKey(32, 2);
			Packet_RuntimeSetting i59 = instance.RuntimeSetting;
			Packet_RuntimeSettingSerializer.GetSize(i59);
			Packet_RuntimeSettingSerializer.SerializeWithSize(stream, i59);
		}
		if (instance.UdpPacket != null)
		{
			stream.WriteKey(34, 2);
			Packet_UdpPacket i60 = instance.UdpPacket;
			Packet_UdpPacketSerializer.GetSize(i60);
			Packet_UdpPacketSerializer.SerializeWithSize(stream, i60);
		}
	}

	public static void SerializeWithSize(CitoStream stream, Packet_Client instance)
	{
		ProtocolParser.WriteUInt32_(stream, instance.size);
		int positionSaved = stream.Position();
		Serialize(stream, instance);
		int len = stream.Position() - positionSaved;
		if (len != instance.size)
		{
			throw new Exception("Sizing mismatch: " + instance.size + " != " + len);
		}
	}

	public static byte[] SerializeToBytes(Packet_Client instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_Client instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
