using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace Vintagestory.Server.Systems;

public static class EntityPosExtensions
{
	private const int maxMovePerPacket = 128;

	private const int maxMotionChangePerPacket = 64;

	public static bool SetFromPacket(this EntityPos pos, Packet_EntityPosition packet, Entity entity, bool sanityCheck = true)
	{
		double x = CollectibleNet.DeserializeDoublePrecise(packet.X);
		double y = CollectibleNet.DeserializeDoublePrecise(packet.Y);
		double z = CollectibleNet.DeserializeDoublePrecise(packet.Z);
		if (sanityCheck)
		{
			if (Math.Abs(pos.X - x) > 128.0)
			{
				return false;
			}
			if (Math.Abs(pos.Y - y) > 128.0)
			{
				return false;
			}
			if (Math.Abs(pos.Z - z) > 128.0)
			{
				return false;
			}
		}
		pos.X = x;
		pos.Y = y;
		pos.Z = z;
		float yaw = CollectibleNet.DeserializeFloatPrecise(packet.Yaw) % ((float)Math.PI * 2f);
		float pitch = CollectibleNet.DeserializeFloatPrecise(packet.Pitch) % ((float)Math.PI * 2f);
		float roll = CollectibleNet.DeserializeFloatPrecise(packet.Roll) % ((float)Math.PI * 2f);
		pos.Yaw = ((yaw < 0f) ? (yaw + (float)Math.PI * 2f) : yaw);
		pos.Pitch = ((pitch < 0f) ? (pitch + (float)Math.PI * 2f) : pitch);
		pos.Roll = ((roll < 0f) ? (roll + (float)Math.PI * 2f) : roll);
		yaw = CollectibleNet.DeserializeFloatPrecise(packet.HeadYaw) % ((float)Math.PI * 2f);
		pitch = CollectibleNet.DeserializeFloatPrecise(packet.HeadPitch) % ((float)Math.PI * 2f);
		pos.HeadYaw = ((yaw < 0f) ? (yaw + (float)Math.PI * 2f) : yaw);
		pos.HeadPitch = ((pitch < 0f) ? (pitch + (float)Math.PI * 2f) : pitch);
		x = CollectibleNet.DeserializeDoublePrecise(packet.MotionX);
		y = CollectibleNet.DeserializeDoublePrecise(packet.MotionY);
		z = CollectibleNet.DeserializeDoublePrecise(packet.MotionZ);
		Vec3d curMotion = pos.Motion;
		if (sanityCheck)
		{
			if (Math.Abs(curMotion.X - x) > 64.0)
			{
				return false;
			}
			if (Math.Abs(curMotion.Y - y) > 64.0)
			{
				return false;
			}
			if (Math.Abs(curMotion.Z - z) > 64.0)
			{
				return false;
			}
		}
		curMotion.X = x;
		curMotion.Y = y;
		curMotion.Z = z;
		if (entity is EntityAgent agent)
		{
			yaw = CollectibleNet.DeserializeFloatPrecise(packet.BodyYaw) % ((float)Math.PI * 2f);
			agent.BodyYawServer = ((yaw < 0f) ? (yaw + (float)Math.PI * 2f) : yaw);
		}
		((entity.SidedProperties == null) ? null : entity.GetInterface<IMountable>()?.ControllingControls)?.FromInt(packet.MountControls);
		return true;
	}
}
