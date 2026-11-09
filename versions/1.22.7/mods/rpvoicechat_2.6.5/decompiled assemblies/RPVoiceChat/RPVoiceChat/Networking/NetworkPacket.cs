using System;
using System.IO;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000071 RID: 113
	public abstract class NetworkPacket
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002DE RID: 734
		protected abstract PacketType Code { get; }

		// Token: 0x060002DF RID: 735 RVA: 0x00010F16 File Offset: 0x0000F116
		public byte[] ToBytes()
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(BitConverter.GetBytes((int)this.Code));
			Serializer.Serialize<NetworkPacket>(memoryStream, this);
			return memoryStream.ToArray();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00010F40 File Offset: 0x0000F140
		public static T FromBytes<T>(byte[] data) where T : NetworkPacket
		{
			MemoryStream memoryStream = new MemoryStream(data);
			PacketType code = (PacketType)BitConverter.ToInt32(data, 0);
			memoryStream.Position = 4L;
			T packet = Serializer.Deserialize<T>(memoryStream);
			if (code != packet.Code)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(50, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Parsed network packet was of type ");
				defaultInterpolatedStringHandler.AppendFormatted<PacketType>(code);
				defaultInterpolatedStringHandler.AppendLiteral(" while expected ");
				defaultInterpolatedStringHandler.AppendFormatted<PacketType>(packet.Code);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return packet;
		}
	}
}
