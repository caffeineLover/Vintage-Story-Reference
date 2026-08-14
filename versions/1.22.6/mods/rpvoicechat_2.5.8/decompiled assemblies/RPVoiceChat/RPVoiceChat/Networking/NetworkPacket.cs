using System;
using System.IO;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000054 RID: 84
	public abstract class NetworkPacket
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000243 RID: 579
		protected abstract PacketType Code { get; }

		// Token: 0x06000244 RID: 580 RVA: 0x0000D44A File Offset: 0x0000B64A
		public byte[] ToBytes()
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(BitConverter.GetBytes((int)this.Code));
			Serializer.Serialize<NetworkPacket>(memoryStream, this);
			return memoryStream.ToArray();
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D474 File Offset: 0x0000B674
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
