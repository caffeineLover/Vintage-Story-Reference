using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace RPVoiceChat.Util
{
	// Token: 0x02000028 RID: 40
	public static class LocationUtils
	{
		// Token: 0x060000EA RID: 234 RVA: 0x0000747D File Offset: 0x0000567D
		public static Vec3d GetLocationOfPlayer(ICoreClientAPI api)
		{
			if (api == null)
			{
				throw new Exception("api is null");
			}
			return LocationUtils.GetLocationOfPlayer(api.World.Player);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000074A0 File Offset: 0x000056A0
		public static Vec3d GetLocationOfPlayer(IPlayer player)
		{
			if (player == null)
			{
				throw new Exception("player is null");
			}
			if (player.Entity.Swimming || (player.Entity.CurrentControls & 512) != null)
			{
				return player.Entity.Pos.XYZ + new Vec3d(0.0, 0.5, 0.0);
			}
			return LocationUtils.GetSpeakerLocation(player.Entity.Pos);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00007521 File Offset: 0x00005721
		public static Vec3d GetLocationOfPlayer(EntityPos pos)
		{
			if (pos == null)
			{
				throw new Exception("entity is null");
			}
			return LocationUtils.GetSpeakerLocation(pos);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00007537 File Offset: 0x00005737
		public static Vec3f GetRelativeSpeakerLocation(EntityPos speakerPos, EntityPos listenerPos)
		{
			return LocationUtils.GetRelativeSpeakerLocation(speakerPos.XYZFloat, listenerPos);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00007548 File Offset: 0x00005748
		public static Vec3f GetRelativeSpeakerLocation(Vec3f speakerPos, EntityPos listenerPos)
		{
			Vec3f relativeSpeakerCoords = speakerPos - listenerPos.XYZFloat;
			double num = (double)(listenerPos.Yaw + listenerPos.HeadYaw) - 3.141592653589793;
			double cs = Math.Cos(num);
			double sn = Math.Sin(num);
			return new Vec3d((double)relativeSpeakerCoords.X * cs - (double)relativeSpeakerCoords.Z * sn, (double)relativeSpeakerCoords.Y, (double)relativeSpeakerCoords.X * sn + (double)relativeSpeakerCoords.Z * cs).ToVec3f();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000075C0 File Offset: 0x000057C0
		public static float GetWallThickness(ICoreClientAPI capi, IPlayer source, IPlayer target)
		{
			Vec3d origin = LocationUtils.GetLocationOfPlayer(source);
			Vec3d destination = LocationUtils.GetLocationOfPlayer(target);
			return LocationUtils.GetWallThickness(capi, origin, destination);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000075E4 File Offset: 0x000057E4
		public static float GetWallThickness(ICoreClientAPI capi, Vec3d origin, Vec3d destination)
		{
			List<Tuple<BlockPos, Block>> item = LocationUtils.RayTraceThrough(capi, origin, destination).Item1;
			float thickness = 0f;
			foreach (Tuple<BlockPos, Block> tuple in item)
			{
				BlockPos blockPos = tuple.Item1;
				Block block = tuple.Item2;
				if (!Enumerable.Contains<string>(LocationUtils.transparentBlocks, (block != null) ? block.Class : null))
				{
					Cuboidf[] collisionBoxes = new Cuboidf[0];
					try
					{
						collisionBoxes = (((block != null) ? block.GetCollisionBoxes(capi.World.BlockAccessor, blockPos) : null) ?? collisionBoxes);
					}
					catch (Exception e)
					{
						Logger client = Logger.client;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Couldn't retrieve collision boxes for ");
						defaultInterpolatedStringHandler.AppendFormatted(block.Class);
						defaultInterpolatedStringHandler.AppendLiteral(" at ");
						defaultInterpolatedStringHandler.AppendFormatted<BlockPos>(blockPos);
						defaultInterpolatedStringHandler.AppendLiteral(":\n");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
						client.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					foreach (Cuboidf box in collisionBoxes)
					{
						thickness += box.Length * box.Height * box.Width;
					}
				}
			}
			return thickness;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00007754 File Offset: 0x00005954
		public static Tuple<List<Tuple<BlockPos, Block>>, List<Entity>> RayTraceThrough(ICoreClientAPI capi, Vec3d from, Vec3d to)
		{
			List<Tuple<BlockPos, Block>> visitedBlocks = new List<Tuple<BlockPos, Block>>();
			List<Entity> visitedEntities = new List<Entity>();
			EntityFilter entityFilter = (Entity entity) => !visitedEntities.Contains(entity) && !(entity.Class == "EntityPlayer");
			BlockFilter blockFilter = delegate(BlockPos pos, Block block)
			{
				Tuple<BlockPos, Block> blockEntry2 = new Tuple<BlockPos, Block>(pos.Copy(), block);
				return !visitedBlocks.Contains(blockEntry2);
			};
			try
			{
				for (int i = 0; i < LocationUtils.maxRayTraceDistance; i++)
				{
					BlockSelection blockSelection = new BlockSelection();
					EntitySelection entitySelection = new EntitySelection();
					capi.World.RayTraceForSelection(from, to, ref blockSelection, ref entitySelection, blockFilter, entityFilter);
					if (blockSelection == null && entitySelection == null)
					{
						break;
					}
					if (((entitySelection != null) ? entitySelection.Entity : null) != null)
					{
						visitedEntities.Add(entitySelection.Entity);
					}
					if (((blockSelection != null) ? blockSelection.Block : null) != null && blockSelection != null && blockSelection.Position != null)
					{
						Tuple<BlockPos, Block> blockEntry = new Tuple<BlockPos, Block>(blockSelection.Position.Copy(), blockSelection.Block);
						visitedBlocks.Add(blockEntry);
					}
				}
			}
			catch (Exception e)
			{
				Logger client = Logger.client;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Ray trace API threw an exception:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
				client.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new Tuple<List<Tuple<BlockPos, Block>>, List<Entity>>(visitedBlocks, visitedEntities);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007894 File Offset: 0x00005A94
		public static bool IsReverbArea(ICoreClientAPI capi, EntityPos playerPos)
		{
			Room room = capi.ModLoader.GetModSystem<RoomRegistry>(true).GetRoomForPosition(playerPos.AsBlockPos);
			return (room.ExitCount == 0 || room.SkylightCount < room.NonSkylightCount) && !room.IsSmallRoom;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000078DA File Offset: 0x00005ADA
		private static Vec3d GetSpeakerLocation(EntityPos pos)
		{
			return new Vec3d(pos.X, pos.Y + 1.5, pos.Z);
		}

		// Token: 0x0400005D RID: 93
		private static int maxRayTraceDistance = 50;

		// Token: 0x0400005E RID: 94
		private static string[] transparentBlocks = new string[]
		{
			"BlockGroundStorage",
			"BlockPie",
			"BlockSupportBeam"
		};
	}
}
