using System;

public class Packet_ServerSerializer
{
	private const int field = 8;

	public static Packet_Server DeserializeLengthDelimitedNew(CitoMemoryStream stream)
	{
		Packet_Server instance = new Packet_Server();
		DeserializeLengthDelimited(stream, instance);
		return instance;
	}

	public static Packet_Server DeserializeBuffer(byte[] buffer, int length, Packet_Server instance)
	{
		Deserialize(new CitoMemoryStream(buffer, length), instance);
		return instance;
	}

	public static Packet_Server Deserialize(CitoMemoryStream stream, Packet_Server instance)
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
			case 720:
				instance.Id = ProtocolParser.ReadUInt32(stream);
				break;
			case 618:
				if (instance.Token == null)
				{
					instance.Token = Packet_LoginTokenAnswerSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_LoginTokenAnswerSerializer.DeserializeLengthDelimited(stream, instance.Token);
				}
				break;
			case 10:
				if (instance.Identification == null)
				{
					instance.Identification = Packet_ServerIdentificationSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerIdentificationSerializer.DeserializeLengthDelimited(stream, instance.Identification);
				}
				break;
			case 18:
				if (instance.LevelInitialize == null)
				{
					instance.LevelInitialize = Packet_ServerLevelInitializeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerLevelInitializeSerializer.DeserializeLengthDelimited(stream, instance.LevelInitialize);
				}
				break;
			case 26:
				if (instance.LevelDataChunk == null)
				{
					instance.LevelDataChunk = Packet_ServerLevelProgressSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerLevelProgressSerializer.DeserializeLengthDelimited(stream, instance.LevelDataChunk);
				}
				break;
			case 34:
				if (instance.LevelFinalize == null)
				{
					instance.LevelFinalize = Packet_ServerLevelFinalizeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerLevelFinalizeSerializer.DeserializeLengthDelimited(stream, instance.LevelFinalize);
				}
				break;
			case 42:
				if (instance.SetBlock == null)
				{
					instance.SetBlock = Packet_ServerSetBlockSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerSetBlockSerializer.DeserializeLengthDelimited(stream, instance.SetBlock);
				}
				break;
			case 58:
				if (instance.Chatline == null)
				{
					instance.Chatline = Packet_ChatLineSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ChatLineSerializer.DeserializeLengthDelimited(stream, instance.Chatline);
				}
				break;
			case 66:
				if (instance.DisconnectPlayer == null)
				{
					instance.DisconnectPlayer = Packet_ServerDisconnectPlayerSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerDisconnectPlayerSerializer.DeserializeLengthDelimited(stream, instance.DisconnectPlayer);
				}
				break;
			case 74:
				if (instance.Chunks == null)
				{
					instance.Chunks = Packet_ServerChunksSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerChunksSerializer.DeserializeLengthDelimited(stream, instance.Chunks);
				}
				break;
			case 82:
				if (instance.UnloadChunk == null)
				{
					instance.UnloadChunk = Packet_UnloadServerChunkSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_UnloadServerChunkSerializer.DeserializeLengthDelimited(stream, instance.UnloadChunk);
				}
				break;
			case 90:
				if (instance.Calendar == null)
				{
					instance.Calendar = Packet_ServerCalendarSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerCalendarSerializer.DeserializeLengthDelimited(stream, instance.Calendar);
				}
				break;
			case 122:
				if (instance.MapChunk == null)
				{
					instance.MapChunk = Packet_ServerMapChunkSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerMapChunkSerializer.DeserializeLengthDelimited(stream, instance.MapChunk);
				}
				break;
			case 130:
				if (instance.Ping == null)
				{
					instance.Ping = Packet_ServerPingSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerPingSerializer.DeserializeLengthDelimited(stream, instance.Ping);
				}
				break;
			case 138:
				if (instance.PlayerPing == null)
				{
					instance.PlayerPing = Packet_ServerPlayerPingSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerPlayerPingSerializer.DeserializeLengthDelimited(stream, instance.PlayerPing);
				}
				break;
			case 146:
				if (instance.Sound == null)
				{
					instance.Sound = Packet_ServerSoundSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerSoundSerializer.DeserializeLengthDelimited(stream, instance.Sound);
				}
				break;
			case 154:
				if (instance.Assets == null)
				{
					instance.Assets = Packet_ServerAssetsSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerAssetsSerializer.DeserializeLengthDelimited(stream, instance.Assets);
				}
				break;
			case 170:
				if (instance.WorldMetaData == null)
				{
					instance.WorldMetaData = Packet_WorldMetaDataSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_WorldMetaDataSerializer.DeserializeLengthDelimited(stream, instance.WorldMetaData);
				}
				break;
			case 226:
				if (instance.QueryAnswer == null)
				{
					instance.QueryAnswer = Packet_ServerQueryAnswerSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerQueryAnswerSerializer.DeserializeLengthDelimited(stream, instance.QueryAnswer);
				}
				break;
			case 234:
				if (instance.Redirect == null)
				{
					instance.Redirect = Packet_ServerRedirectSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerRedirectSerializer.DeserializeLengthDelimited(stream, instance.Redirect);
				}
				break;
			case 242:
				if (instance.InventoryContents == null)
				{
					instance.InventoryContents = Packet_InventoryContentsSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_InventoryContentsSerializer.DeserializeLengthDelimited(stream, instance.InventoryContents);
				}
				break;
			case 250:
				if (instance.InventoryUpdate == null)
				{
					instance.InventoryUpdate = Packet_InventoryUpdateSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_InventoryUpdateSerializer.DeserializeLengthDelimited(stream, instance.InventoryUpdate);
				}
				break;
			case 258:
				if (instance.InventoryDoubleUpdate == null)
				{
					instance.InventoryDoubleUpdate = Packet_InventoryDoubleUpdateSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_InventoryDoubleUpdateSerializer.DeserializeLengthDelimited(stream, instance.InventoryDoubleUpdate);
				}
				break;
			case 274:
				if (instance.Entity == null)
				{
					instance.Entity = Packet_EntitySerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntitySerializer.DeserializeLengthDelimited(stream, instance.Entity);
				}
				break;
			case 282:
				if (instance.EntitySpawn == null)
				{
					instance.EntitySpawn = Packet_EntitySpawnSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntitySpawnSerializer.DeserializeLengthDelimited(stream, instance.EntitySpawn);
				}
				break;
			case 290:
				if (instance.EntityDespawn == null)
				{
					instance.EntityDespawn = Packet_EntityDespawnSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityDespawnSerializer.DeserializeLengthDelimited(stream, instance.EntityDespawn);
				}
				break;
			case 306:
				if (instance.EntityAttributes == null)
				{
					instance.EntityAttributes = Packet_EntityAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityAttributesSerializer.DeserializeLengthDelimited(stream, instance.EntityAttributes);
				}
				break;
			case 314:
				if (instance.EntityAttributeUpdate == null)
				{
					instance.EntityAttributeUpdate = Packet_EntityAttributeUpdateSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityAttributeUpdateSerializer.DeserializeLengthDelimited(stream, instance.EntityAttributeUpdate);
				}
				break;
			case 538:
				if (instance.EntityPacket == null)
				{
					instance.EntityPacket = Packet_EntityPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityPacketSerializer.DeserializeLengthDelimited(stream, instance.EntityPacket);
				}
				break;
			case 322:
				if (instance.Entities == null)
				{
					instance.Entities = Packet_EntitiesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntitiesSerializer.DeserializeLengthDelimited(stream, instance.Entities);
				}
				break;
			case 330:
				if (instance.PlayerData == null)
				{
					instance.PlayerData = Packet_PlayerDataSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_PlayerDataSerializer.DeserializeLengthDelimited(stream, instance.PlayerData);
				}
				break;
			case 338:
				if (instance.MapRegion == null)
				{
					instance.MapRegion = Packet_MapRegionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_MapRegionSerializer.DeserializeLengthDelimited(stream, instance.MapRegion);
				}
				break;
			case 354:
				if (instance.BlockEntityMessage == null)
				{
					instance.BlockEntityMessage = Packet_BlockEntityMessageSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BlockEntityMessageSerializer.DeserializeLengthDelimited(stream, instance.BlockEntityMessage);
				}
				break;
			case 362:
				if (instance.PlayerDeath == null)
				{
					instance.PlayerDeath = Packet_PlayerDeathSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_PlayerDeathSerializer.DeserializeLengthDelimited(stream, instance.PlayerDeath);
				}
				break;
			case 370:
				if (instance.ModeChange == null)
				{
					instance.ModeChange = Packet_PlayerModeSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_PlayerModeSerializer.DeserializeLengthDelimited(stream, instance.ModeChange);
				}
				break;
			case 378:
				if (instance.SetBlocks == null)
				{
					instance.SetBlocks = Packet_ServerSetBlocksSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerSetBlocksSerializer.DeserializeLengthDelimited(stream, instance.SetBlocks);
				}
				break;
			case 386:
				if (instance.BlockEntities == null)
				{
					instance.BlockEntities = Packet_BlockEntitiesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BlockEntitiesSerializer.DeserializeLengthDelimited(stream, instance.BlockEntities);
				}
				break;
			case 394:
				if (instance.PlayerGroups == null)
				{
					instance.PlayerGroups = Packet_PlayerGroupsSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_PlayerGroupsSerializer.DeserializeLengthDelimited(stream, instance.PlayerGroups);
				}
				break;
			case 402:
				if (instance.PlayerGroup == null)
				{
					instance.PlayerGroup = Packet_PlayerGroupSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_PlayerGroupSerializer.DeserializeLengthDelimited(stream, instance.PlayerGroup);
				}
				break;
			case 410:
				if (instance.EntityPosition == null)
				{
					instance.EntityPosition = Packet_EntityPositionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_EntityPositionSerializer.DeserializeLengthDelimited(stream, instance.EntityPosition);
				}
				break;
			case 418:
				if (instance.HighlightBlocks == null)
				{
					instance.HighlightBlocks = Packet_HighlightBlocksSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_HighlightBlocksSerializer.DeserializeLengthDelimited(stream, instance.HighlightBlocks);
				}
				break;
			case 426:
				if (instance.SelectedHotbarSlot == null)
				{
					instance.SelectedHotbarSlot = Packet_SelectedHotbarSlotSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SelectedHotbarSlotSerializer.DeserializeLengthDelimited(stream, instance.SelectedHotbarSlot);
				}
				break;
			case 442:
				if (instance.CustomPacket == null)
				{
					instance.CustomPacket = Packet_CustomPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_CustomPacketSerializer.DeserializeLengthDelimited(stream, instance.CustomPacket);
				}
				break;
			case 450:
				if (instance.NetworkChannels == null)
				{
					instance.NetworkChannels = Packet_NetworkChannelsSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_NetworkChannelsSerializer.DeserializeLengthDelimited(stream, instance.NetworkChannels);
				}
				break;
			case 458:
				if (instance.GotoGroup == null)
				{
					instance.GotoGroup = Packet_GotoGroupSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_GotoGroupSerializer.DeserializeLengthDelimited(stream, instance.GotoGroup);
				}
				break;
			case 466:
				if (instance.ExchangeBlock == null)
				{
					instance.ExchangeBlock = Packet_ServerExchangeBlockSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerExchangeBlockSerializer.DeserializeLengthDelimited(stream, instance.ExchangeBlock);
				}
				break;
			case 474:
				if (instance.BulkEntityAttributes == null)
				{
					instance.BulkEntityAttributes = Packet_BulkEntityAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BulkEntityAttributesSerializer.DeserializeLengthDelimited(stream, instance.BulkEntityAttributes);
				}
				break;
			case 482:
				if (instance.SpawnParticles == null)
				{
					instance.SpawnParticles = Packet_SpawnParticlesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_SpawnParticlesSerializer.DeserializeLengthDelimited(stream, instance.SpawnParticles);
				}
				break;
			case 490:
				if (instance.BulkEntityDebugAttributes == null)
				{
					instance.BulkEntityDebugAttributes = Packet_BulkEntityDebugAttributesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BulkEntityDebugAttributesSerializer.DeserializeLengthDelimited(stream, instance.BulkEntityDebugAttributes);
				}
				break;
			case 498:
				if (instance.SetBlocksNoRelight == null)
				{
					instance.SetBlocksNoRelight = Packet_ServerSetBlocksSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerSetBlocksSerializer.DeserializeLengthDelimited(stream, instance.SetBlocksNoRelight);
				}
				break;
			case 514:
				if (instance.BlockDamage == null)
				{
					instance.BlockDamage = Packet_BlockDamageSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_BlockDamageSerializer.DeserializeLengthDelimited(stream, instance.BlockDamage);
				}
				break;
			case 522:
				if (instance.Ambient == null)
				{
					instance.Ambient = Packet_AmbientSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_AmbientSerializer.DeserializeLengthDelimited(stream, instance.Ambient);
				}
				break;
			case 530:
				if (instance.NotifySlot == null)
				{
					instance.NotifySlot = Packet_NotifySlotSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_NotifySlotSerializer.DeserializeLengthDelimited(stream, instance.NotifySlot);
				}
				break;
			case 546:
				if (instance.IngameError == null)
				{
					instance.IngameError = Packet_IngameErrorSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_IngameErrorSerializer.DeserializeLengthDelimited(stream, instance.IngameError);
				}
				break;
			case 554:
				if (instance.IngameDiscovery == null)
				{
					instance.IngameDiscovery = Packet_IngameDiscoverySerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_IngameDiscoverySerializer.DeserializeLengthDelimited(stream, instance.IngameDiscovery);
				}
				break;
			case 562:
				if (instance.SetBlocksMinimal == null)
				{
					instance.SetBlocksMinimal = Packet_ServerSetBlocksSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerSetBlocksSerializer.DeserializeLengthDelimited(stream, instance.SetBlocksMinimal);
				}
				break;
			case 570:
				if (instance.SetDecors == null)
				{
					instance.SetDecors = Packet_ServerSetDecorsSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerSetDecorsSerializer.DeserializeLengthDelimited(stream, instance.SetDecors);
				}
				break;
			case 578:
				if (instance.RemoveBlockLight == null)
				{
					instance.RemoveBlockLight = Packet_RemoveBlockLightSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_RemoveBlockLightSerializer.DeserializeLengthDelimited(stream, instance.RemoveBlockLight);
				}
				break;
			case 586:
				if (instance.ServerReady == null)
				{
					instance.ServerReady = Packet_ServerReadySerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerReadySerializer.DeserializeLengthDelimited(stream, instance.ServerReady);
				}
				break;
			case 594:
				if (instance.UnloadMapRegion == null)
				{
					instance.UnloadMapRegion = Packet_UnloadMapRegionSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_UnloadMapRegionSerializer.DeserializeLengthDelimited(stream, instance.UnloadMapRegion);
				}
				break;
			case 602:
				if (instance.LandClaims == null)
				{
					instance.LandClaims = Packet_LandClaimsSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_LandClaimsSerializer.DeserializeLengthDelimited(stream, instance.LandClaims);
				}
				break;
			case 610:
				if (instance.Roles == null)
				{
					instance.Roles = Packet_RolesSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_RolesSerializer.DeserializeLengthDelimited(stream, instance.Roles);
				}
				break;
			case 626:
				if (instance.UdpPacket == null)
				{
					instance.UdpPacket = Packet_UdpPacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_UdpPacketSerializer.DeserializeLengthDelimited(stream, instance.UdpPacket);
				}
				break;
			case 634:
				if (instance.QueuePacket == null)
				{
					instance.QueuePacket = Packet_QueuePacketSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_QueuePacketSerializer.DeserializeLengthDelimited(stream, instance.QueuePacket);
				}
				break;
			case 666:
				if (instance.Calendarupdate == null)
				{
					instance.Calendarupdate = Packet_ServerCalendarUpdateSerializer.DeserializeLengthDelimitedNew(stream);
				}
				else
				{
					Packet_ServerCalendarUpdateSerializer.DeserializeLengthDelimited(stream, instance.Calendarupdate);
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

	public static Packet_Server DeserializeLengthDelimited(CitoMemoryStream stream, Packet_Server instance)
	{
		int lengthOfPart = ProtocolParser.ReadUInt32(stream);
		int savedLength = stream.GetLength();
		stream.SetLength(stream.Position() + lengthOfPart);
		Packet_Server result = Deserialize(stream, instance);
		stream.SetLength(savedLength);
		return result;
	}

	public static void Serialize(CitoStream stream, Packet_Server instance)
	{
		if (instance.Id != 1)
		{
			stream.WriteKey(90, 0);
			ProtocolParser.WriteUInt32(stream, instance.Id);
		}
		if (instance.Token != null)
		{
			stream.WriteKey(77, 2);
			Packet_LoginTokenAnswer i77 = instance.Token;
			Packet_LoginTokenAnswerSerializer.GetSize(i77);
			Packet_LoginTokenAnswerSerializer.SerializeWithSize(stream, i77);
		}
		if (instance.Identification != null)
		{
			stream.WriteByte(10);
			Packet_ServerIdentification i78 = instance.Identification;
			Packet_ServerIdentificationSerializer.GetSize(i78);
			Packet_ServerIdentificationSerializer.SerializeWithSize(stream, i78);
		}
		if (instance.LevelInitialize != null)
		{
			stream.WriteByte(18);
			Packet_ServerLevelInitialize i79 = instance.LevelInitialize;
			Packet_ServerLevelInitializeSerializer.GetSize(i79);
			Packet_ServerLevelInitializeSerializer.SerializeWithSize(stream, i79);
		}
		if (instance.LevelDataChunk != null)
		{
			stream.WriteByte(26);
			Packet_ServerLevelProgress i80 = instance.LevelDataChunk;
			Packet_ServerLevelProgressSerializer.GetSize(i80);
			Packet_ServerLevelProgressSerializer.SerializeWithSize(stream, i80);
		}
		if (instance.LevelFinalize != null)
		{
			stream.WriteByte(34);
			Packet_ServerLevelFinalize i81 = instance.LevelFinalize;
			Packet_ServerLevelFinalizeSerializer.GetSize(i81);
			Packet_ServerLevelFinalizeSerializer.SerializeWithSize(stream, i81);
		}
		if (instance.SetBlock != null)
		{
			stream.WriteByte(42);
			Packet_ServerSetBlock i82 = instance.SetBlock;
			Packet_ServerSetBlockSerializer.GetSize(i82);
			Packet_ServerSetBlockSerializer.SerializeWithSize(stream, i82);
		}
		if (instance.Chatline != null)
		{
			stream.WriteByte(58);
			Packet_ChatLine i83 = instance.Chatline;
			Packet_ChatLineSerializer.GetSize(i83);
			Packet_ChatLineSerializer.SerializeWithSize(stream, i83);
		}
		if (instance.DisconnectPlayer != null)
		{
			stream.WriteByte(66);
			Packet_ServerDisconnectPlayer i84 = instance.DisconnectPlayer;
			Packet_ServerDisconnectPlayerSerializer.GetSize(i84);
			Packet_ServerDisconnectPlayerSerializer.SerializeWithSize(stream, i84);
		}
		if (instance.Chunks != null)
		{
			stream.WriteByte(74);
			Packet_ServerChunks i85 = instance.Chunks;
			Packet_ServerChunksSerializer.GetSize(i85);
			Packet_ServerChunksSerializer.SerializeWithSize(stream, i85);
		}
		if (instance.UnloadChunk != null)
		{
			stream.WriteByte(82);
			Packet_UnloadServerChunk i86 = instance.UnloadChunk;
			Packet_UnloadServerChunkSerializer.GetSize(i86);
			Packet_UnloadServerChunkSerializer.SerializeWithSize(stream, i86);
		}
		if (instance.Calendar != null)
		{
			stream.WriteByte(90);
			Packet_ServerCalendar i87 = instance.Calendar;
			Packet_ServerCalendarSerializer.GetSize(i87);
			Packet_ServerCalendarSerializer.SerializeWithSize(stream, i87);
		}
		if (instance.MapChunk != null)
		{
			stream.WriteByte(122);
			Packet_ServerMapChunk i88 = instance.MapChunk;
			Packet_ServerMapChunkSerializer.GetSize(i88);
			Packet_ServerMapChunkSerializer.SerializeWithSize(stream, i88);
		}
		if (instance.Ping != null)
		{
			stream.WriteKey(16, 2);
			Packet_ServerPing i89 = instance.Ping;
			Packet_ServerPingSerializer.GetSize(i89);
			Packet_ServerPingSerializer.SerializeWithSize(stream, i89);
		}
		if (instance.PlayerPing != null)
		{
			stream.WriteKey(17, 2);
			Packet_ServerPlayerPing i90 = instance.PlayerPing;
			Packet_ServerPlayerPingSerializer.GetSize(i90);
			Packet_ServerPlayerPingSerializer.SerializeWithSize(stream, i90);
		}
		if (instance.Sound != null)
		{
			stream.WriteKey(18, 2);
			Packet_ServerSound i91 = instance.Sound;
			Packet_ServerSoundSerializer.GetSize(i91);
			Packet_ServerSoundSerializer.SerializeWithSize(stream, i91);
		}
		if (instance.Assets != null)
		{
			stream.WriteKey(19, 2);
			Packet_ServerAssets i92 = instance.Assets;
			Packet_ServerAssetsSerializer.GetSize(i92);
			Packet_ServerAssetsSerializer.SerializeWithSize(stream, i92);
		}
		if (instance.WorldMetaData != null)
		{
			stream.WriteKey(21, 2);
			Packet_WorldMetaData i93 = instance.WorldMetaData;
			Packet_WorldMetaDataSerializer.GetSize(i93);
			Packet_WorldMetaDataSerializer.SerializeWithSize(stream, i93);
		}
		if (instance.QueryAnswer != null)
		{
			stream.WriteKey(28, 2);
			Packet_ServerQueryAnswer i94 = instance.QueryAnswer;
			Packet_ServerQueryAnswerSerializer.GetSize(i94);
			Packet_ServerQueryAnswerSerializer.SerializeWithSize(stream, i94);
		}
		if (instance.Redirect != null)
		{
			stream.WriteKey(29, 2);
			Packet_ServerRedirect i95 = instance.Redirect;
			Packet_ServerRedirectSerializer.GetSize(i95);
			Packet_ServerRedirectSerializer.SerializeWithSize(stream, i95);
		}
		if (instance.InventoryContents != null)
		{
			stream.WriteKey(30, 2);
			Packet_InventoryContents i96 = instance.InventoryContents;
			Packet_InventoryContentsSerializer.GetSize(i96);
			Packet_InventoryContentsSerializer.SerializeWithSize(stream, i96);
		}
		if (instance.InventoryUpdate != null)
		{
			stream.WriteKey(31, 2);
			Packet_InventoryUpdate i97 = instance.InventoryUpdate;
			Packet_InventoryUpdateSerializer.GetSize(i97);
			Packet_InventoryUpdateSerializer.SerializeWithSize(stream, i97);
		}
		if (instance.InventoryDoubleUpdate != null)
		{
			stream.WriteKey(32, 2);
			Packet_InventoryDoubleUpdate i98 = instance.InventoryDoubleUpdate;
			Packet_InventoryDoubleUpdateSerializer.GetSize(i98);
			Packet_InventoryDoubleUpdateSerializer.SerializeWithSize(stream, i98);
		}
		if (instance.Entity != null)
		{
			stream.WriteKey(34, 2);
			Packet_Entity i99 = instance.Entity;
			Packet_EntitySerializer.GetSize(i99);
			Packet_EntitySerializer.SerializeWithSize(stream, i99);
		}
		if (instance.EntitySpawn != null)
		{
			stream.WriteKey(35, 2);
			Packet_EntitySpawn i100 = instance.EntitySpawn;
			Packet_EntitySpawnSerializer.GetSize(i100);
			Packet_EntitySpawnSerializer.SerializeWithSize(stream, i100);
		}
		if (instance.EntityDespawn != null)
		{
			stream.WriteKey(36, 2);
			Packet_EntityDespawn i101 = instance.EntityDespawn;
			Packet_EntityDespawnSerializer.GetSize(i101);
			Packet_EntityDespawnSerializer.SerializeWithSize(stream, i101);
		}
		if (instance.EntityAttributes != null)
		{
			stream.WriteKey(38, 2);
			Packet_EntityAttributes i102 = instance.EntityAttributes;
			Packet_EntityAttributesSerializer.GetSize(i102);
			Packet_EntityAttributesSerializer.SerializeWithSize(stream, i102);
		}
		if (instance.EntityAttributeUpdate != null)
		{
			stream.WriteKey(39, 2);
			Packet_EntityAttributeUpdate i103 = instance.EntityAttributeUpdate;
			Packet_EntityAttributeUpdateSerializer.GetSize(i103);
			Packet_EntityAttributeUpdateSerializer.SerializeWithSize(stream, i103);
		}
		if (instance.EntityPacket != null)
		{
			stream.WriteKey(67, 2);
			Packet_EntityPacket i104 = instance.EntityPacket;
			Packet_EntityPacketSerializer.GetSize(i104);
			Packet_EntityPacketSerializer.SerializeWithSize(stream, i104);
		}
		if (instance.Entities != null)
		{
			stream.WriteKey(40, 2);
			Packet_Entities i105 = instance.Entities;
			Packet_EntitiesSerializer.GetSize(i105);
			Packet_EntitiesSerializer.SerializeWithSize(stream, i105);
		}
		if (instance.PlayerData != null)
		{
			stream.WriteKey(41, 2);
			Packet_PlayerData i106 = instance.PlayerData;
			Packet_PlayerDataSerializer.GetSize(i106);
			Packet_PlayerDataSerializer.SerializeWithSize(stream, i106);
		}
		if (instance.MapRegion != null)
		{
			stream.WriteKey(42, 2);
			Packet_MapRegion i107 = instance.MapRegion;
			Packet_MapRegionSerializer.GetSize(i107);
			Packet_MapRegionSerializer.SerializeWithSize(stream, i107);
		}
		if (instance.BlockEntityMessage != null)
		{
			stream.WriteKey(44, 2);
			Packet_BlockEntityMessage i108 = instance.BlockEntityMessage;
			Packet_BlockEntityMessageSerializer.GetSize(i108);
			Packet_BlockEntityMessageSerializer.SerializeWithSize(stream, i108);
		}
		if (instance.PlayerDeath != null)
		{
			stream.WriteKey(45, 2);
			Packet_PlayerDeath i109 = instance.PlayerDeath;
			Packet_PlayerDeathSerializer.GetSize(i109);
			Packet_PlayerDeathSerializer.SerializeWithSize(stream, i109);
		}
		if (instance.ModeChange != null)
		{
			stream.WriteKey(46, 2);
			Packet_PlayerMode i110 = instance.ModeChange;
			Packet_PlayerModeSerializer.GetSize(i110);
			Packet_PlayerModeSerializer.SerializeWithSize(stream, i110);
		}
		if (instance.SetBlocks != null)
		{
			stream.WriteKey(47, 2);
			Packet_ServerSetBlocks i111 = instance.SetBlocks;
			Packet_ServerSetBlocksSerializer.GetSize(i111);
			Packet_ServerSetBlocksSerializer.SerializeWithSize(stream, i111);
		}
		if (instance.BlockEntities != null)
		{
			stream.WriteKey(48, 2);
			Packet_BlockEntities i112 = instance.BlockEntities;
			Packet_BlockEntitiesSerializer.GetSize(i112);
			Packet_BlockEntitiesSerializer.SerializeWithSize(stream, i112);
		}
		if (instance.PlayerGroups != null)
		{
			stream.WriteKey(49, 2);
			Packet_PlayerGroups i113 = instance.PlayerGroups;
			Packet_PlayerGroupsSerializer.GetSize(i113);
			Packet_PlayerGroupsSerializer.SerializeWithSize(stream, i113);
		}
		if (instance.PlayerGroup != null)
		{
			stream.WriteKey(50, 2);
			Packet_PlayerGroup i114 = instance.PlayerGroup;
			Packet_PlayerGroupSerializer.GetSize(i114);
			Packet_PlayerGroupSerializer.SerializeWithSize(stream, i114);
		}
		if (instance.EntityPosition != null)
		{
			stream.WriteKey(51, 2);
			Packet_EntityPosition i115 = instance.EntityPosition;
			Packet_EntityPositionSerializer.GetSize(i115);
			Packet_EntityPositionSerializer.SerializeWithSize(stream, i115);
		}
		if (instance.HighlightBlocks != null)
		{
			stream.WriteKey(52, 2);
			Packet_HighlightBlocks i116 = instance.HighlightBlocks;
			Packet_HighlightBlocksSerializer.GetSize(i116);
			Packet_HighlightBlocksSerializer.SerializeWithSize(stream, i116);
		}
		if (instance.SelectedHotbarSlot != null)
		{
			stream.WriteKey(53, 2);
			Packet_SelectedHotbarSlot i117 = instance.SelectedHotbarSlot;
			Packet_SelectedHotbarSlotSerializer.GetSize(i117);
			Packet_SelectedHotbarSlotSerializer.SerializeWithSize(stream, i117);
		}
		if (instance.CustomPacket != null)
		{
			stream.WriteKey(55, 2);
			Packet_CustomPacket i118 = instance.CustomPacket;
			Packet_CustomPacketSerializer.GetSize(i118);
			Packet_CustomPacketSerializer.SerializeWithSize(stream, i118);
		}
		if (instance.NetworkChannels != null)
		{
			stream.WriteKey(56, 2);
			Packet_NetworkChannels i119 = instance.NetworkChannels;
			Packet_NetworkChannelsSerializer.GetSize(i119);
			Packet_NetworkChannelsSerializer.SerializeWithSize(stream, i119);
		}
		if (instance.GotoGroup != null)
		{
			stream.WriteKey(57, 2);
			Packet_GotoGroup i120 = instance.GotoGroup;
			Packet_GotoGroupSerializer.GetSize(i120);
			Packet_GotoGroupSerializer.SerializeWithSize(stream, i120);
		}
		if (instance.ExchangeBlock != null)
		{
			stream.WriteKey(58, 2);
			Packet_ServerExchangeBlock i121 = instance.ExchangeBlock;
			Packet_ServerExchangeBlockSerializer.GetSize(i121);
			Packet_ServerExchangeBlockSerializer.SerializeWithSize(stream, i121);
		}
		if (instance.BulkEntityAttributes != null)
		{
			stream.WriteKey(59, 2);
			Packet_BulkEntityAttributes i122 = instance.BulkEntityAttributes;
			Packet_BulkEntityAttributesSerializer.GetSize(i122);
			Packet_BulkEntityAttributesSerializer.SerializeWithSize(stream, i122);
		}
		if (instance.SpawnParticles != null)
		{
			stream.WriteKey(60, 2);
			Packet_SpawnParticles i123 = instance.SpawnParticles;
			Packet_SpawnParticlesSerializer.GetSize(i123);
			Packet_SpawnParticlesSerializer.SerializeWithSize(stream, i123);
		}
		if (instance.BulkEntityDebugAttributes != null)
		{
			stream.WriteKey(61, 2);
			Packet_BulkEntityDebugAttributes i124 = instance.BulkEntityDebugAttributes;
			Packet_BulkEntityDebugAttributesSerializer.GetSize(i124);
			Packet_BulkEntityDebugAttributesSerializer.SerializeWithSize(stream, i124);
		}
		if (instance.SetBlocksNoRelight != null)
		{
			stream.WriteKey(62, 2);
			Packet_ServerSetBlocks i125 = instance.SetBlocksNoRelight;
			Packet_ServerSetBlocksSerializer.GetSize(i125);
			Packet_ServerSetBlocksSerializer.SerializeWithSize(stream, i125);
		}
		if (instance.BlockDamage != null)
		{
			stream.WriteKey(64, 2);
			Packet_BlockDamage i126 = instance.BlockDamage;
			Packet_BlockDamageSerializer.GetSize(i126);
			Packet_BlockDamageSerializer.SerializeWithSize(stream, i126);
		}
		if (instance.Ambient != null)
		{
			stream.WriteKey(65, 2);
			Packet_Ambient i127 = instance.Ambient;
			Packet_AmbientSerializer.GetSize(i127);
			Packet_AmbientSerializer.SerializeWithSize(stream, i127);
		}
		if (instance.NotifySlot != null)
		{
			stream.WriteKey(66, 2);
			Packet_NotifySlot i128 = instance.NotifySlot;
			Packet_NotifySlotSerializer.GetSize(i128);
			Packet_NotifySlotSerializer.SerializeWithSize(stream, i128);
		}
		if (instance.IngameError != null)
		{
			stream.WriteKey(68, 2);
			Packet_IngameError i129 = instance.IngameError;
			Packet_IngameErrorSerializer.GetSize(i129);
			Packet_IngameErrorSerializer.SerializeWithSize(stream, i129);
		}
		if (instance.IngameDiscovery != null)
		{
			stream.WriteKey(69, 2);
			Packet_IngameDiscovery i130 = instance.IngameDiscovery;
			Packet_IngameDiscoverySerializer.GetSize(i130);
			Packet_IngameDiscoverySerializer.SerializeWithSize(stream, i130);
		}
		if (instance.SetBlocksMinimal != null)
		{
			stream.WriteKey(70, 2);
			Packet_ServerSetBlocks i131 = instance.SetBlocksMinimal;
			Packet_ServerSetBlocksSerializer.GetSize(i131);
			Packet_ServerSetBlocksSerializer.SerializeWithSize(stream, i131);
		}
		if (instance.SetDecors != null)
		{
			stream.WriteKey(71, 2);
			Packet_ServerSetDecors i132 = instance.SetDecors;
			Packet_ServerSetDecorsSerializer.GetSize(i132);
			Packet_ServerSetDecorsSerializer.SerializeWithSize(stream, i132);
		}
		if (instance.RemoveBlockLight != null)
		{
			stream.WriteKey(72, 2);
			Packet_RemoveBlockLight i133 = instance.RemoveBlockLight;
			Packet_RemoveBlockLightSerializer.GetSize(i133);
			Packet_RemoveBlockLightSerializer.SerializeWithSize(stream, i133);
		}
		if (instance.ServerReady != null)
		{
			stream.WriteKey(73, 2);
			Packet_ServerReady i134 = instance.ServerReady;
			Packet_ServerReadySerializer.GetSize(i134);
			Packet_ServerReadySerializer.SerializeWithSize(stream, i134);
		}
		if (instance.UnloadMapRegion != null)
		{
			stream.WriteKey(74, 2);
			Packet_UnloadMapRegion i135 = instance.UnloadMapRegion;
			Packet_UnloadMapRegionSerializer.GetSize(i135);
			Packet_UnloadMapRegionSerializer.SerializeWithSize(stream, i135);
		}
		if (instance.LandClaims != null)
		{
			stream.WriteKey(75, 2);
			Packet_LandClaims i136 = instance.LandClaims;
			Packet_LandClaimsSerializer.GetSize(i136);
			Packet_LandClaimsSerializer.SerializeWithSize(stream, i136);
		}
		if (instance.Roles != null)
		{
			stream.WriteKey(76, 2);
			Packet_Roles i137 = instance.Roles;
			Packet_RolesSerializer.GetSize(i137);
			Packet_RolesSerializer.SerializeWithSize(stream, i137);
		}
		if (instance.UdpPacket != null)
		{
			stream.WriteKey(78, 2);
			Packet_UdpPacket i138 = instance.UdpPacket;
			Packet_UdpPacketSerializer.GetSize(i138);
			Packet_UdpPacketSerializer.SerializeWithSize(stream, i138);
		}
		if (instance.QueuePacket != null)
		{
			stream.WriteKey(79, 2);
			Packet_QueuePacket i139 = instance.QueuePacket;
			Packet_QueuePacketSerializer.GetSize(i139);
			Packet_QueuePacketSerializer.SerializeWithSize(stream, i139);
		}
		if (instance.Calendarupdate != null)
		{
			stream.WriteKey(83, 2);
			Packet_ServerCalendarUpdate i140 = instance.Calendarupdate;
			Packet_ServerCalendarUpdateSerializer.GetSize(i140);
			Packet_ServerCalendarUpdateSerializer.SerializeWithSize(stream, i140);
		}
	}

	public static void SerializeWithSize(CitoStream stream, Packet_Server instance)
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

	public static byte[] SerializeToBytes(Packet_Server instance)
	{
		CitoMemoryStream citoMemoryStream = new CitoMemoryStream();
		Serialize(citoMemoryStream, instance);
		return citoMemoryStream.ToArray();
	}

	public static void SerializeLengthDelimited(CitoStream stream, Packet_Server instance)
	{
		byte[] data = SerializeToBytes(instance);
		ProtocolParser.WriteUInt32_(stream, data.Length);
		stream.Write(data, 0, data.Length);
	}
}
