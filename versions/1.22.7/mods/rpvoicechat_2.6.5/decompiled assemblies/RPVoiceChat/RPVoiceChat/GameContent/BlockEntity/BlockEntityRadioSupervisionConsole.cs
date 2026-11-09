using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000EB RID: 235
	public class BlockEntityRadioSupervisionConsole : BEWireNode, INetworkRoot, IWireTypedNode
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00028D55 File Offset: 0x00026F55
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00028D58 File Offset: 0x00026F58
		protected override int MaxConnections
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00028D5B File Offset: 0x00026F5B
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.RadioConsole;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x00028D5E File Offset: 0x00026F5E
		public long CreatedNetworkID
		{
			get
			{
				return this.originalCreatedNetworkID;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00028D66 File Offset: 0x00026F66
		public string Frequency
		{
			get
			{
				return this.frequency ?? "";
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x00028D77 File Offset: 0x00026F77
		public string DisplayName
		{
			get
			{
				return this.displayName ?? "";
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00028D88 File Offset: 0x00026F88
		public override Vec3f GetWireAttachmentOffsetFor(BlockPos otherNodePos)
		{
			if (otherNodePos == null)
			{
				return base.GetWireAttachmentOffsetFor(otherNodePos);
			}
			Vec3f[] axisOffsetsCurrent = Enumerable.ToArray<Vec3f>(Enumerable.Select<Vec3f, Vec3f>(BlockEntityRadioSupervisionConsole.AxisOffsetsNorth, new Func<Vec3f, Vec3f>(this.RotateLocalOffsetByBlockSide)));
			List<ValueTuple<BlockPos, Vec3f>> entries = Enumerable.ToList<ValueTuple<BlockPos, Vec3f>>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, int>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, int>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, int>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, float>(Enumerable.OrderBy<ValueTuple<BlockPos, Vec3f>, float>(Enumerable.Select<BlockPos, ValueTuple<BlockPos, Vec3f>>(Enumerable.Where<BlockPos>(Enumerable.Select<WireConnection, BlockPos>(base.GetConnections(), (WireConnection c) => c.GetOtherBlockPos(this.Pos)), (BlockPos p) => p != null), (BlockPos p) => new ValueTuple<BlockPos, Vec3f>(p, new Vec3f((float)(p.X - this.Pos.X) + 0.5f, 0.5f, (float)(p.Z - this.Pos.Z) + 0.5f))), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item2.X), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item2.Z), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item1.X), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item1.Y), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item1.Z));
			if (entries.Count == 0)
			{
				return base.GetWireAttachmentOffsetFor(otherNodePos);
			}
			int index;
			if (!this.AssignAxesByBestGlobalDistance(entries, axisOffsetsCurrent).TryGetValue(new ValueTuple<int, int, int>(otherNodePos.X, otherNodePos.Y, otherNodePos.Z), ref index))
			{
				return base.GetWireAttachmentOffsetFor(otherNodePos);
			}
			index = GameMath.Clamp(index, 0, BlockEntityRadioSupervisionConsole.AxisOffsetsNorth.Length - 1);
			return axisOffsetsCurrent[index];
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00028F1C File Offset: 0x0002711C
		[return: TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})]
		private Dictionary<ValueTuple<int, int, int>, int> AssignAxesByBestGlobalDistance([TupleElementNames(new string[]
		{
			"Pos",
			"LocalCurrent"
		})] IReadOnlyList<ValueTuple<BlockPos, Vec3f>> entries, IReadOnlyList<Vec3f> axisOffsetsCurrent)
		{
			BlockEntityRadioSupervisionConsole.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.entries = entries;
			CS$<>8__locals1.axisOffsetsCurrent = axisOffsetsCurrent;
			CS$<>8__locals1.count = Math.Min(CS$<>8__locals1.entries.Count, CS$<>8__locals1.axisOffsetsCurrent.Count);
			Dictionary<ValueTuple<int, int, int>, int> assignedAxisByNode = new Dictionary<ValueTuple<int, int, int>, int>();
			if (CS$<>8__locals1.count == 0)
			{
				return assignedAxisByNode;
			}
			CS$<>8__locals1.usedAxes = new bool[CS$<>8__locals1.axisOffsetsCurrent.Count];
			CS$<>8__locals1.current = new int[CS$<>8__locals1.count];
			CS$<>8__locals1.best = new int[CS$<>8__locals1.count];
			Array.Fill<int>(CS$<>8__locals1.best, -1);
			CS$<>8__locals1.bestCost = float.MaxValue;
			BlockEntityRadioSupervisionConsole.<AssignAxesByBestGlobalDistance>g__Search|18_0(0, 0f, ref CS$<>8__locals1);
			for (int i = 0; i < CS$<>8__locals1.count; i++)
			{
				int axisIndex = CS$<>8__locals1.best[i];
				if (axisIndex >= 0)
				{
					assignedAxisByNode[new ValueTuple<int, int, int>(CS$<>8__locals1.entries[i].Item1.X, CS$<>8__locals1.entries[i].Item1.Y, CS$<>8__locals1.entries[i].Item1.Z)] = axisIndex;
				}
			}
			return assignedAxisByNode;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00029038 File Offset: 0x00027238
		private static float DistanceSqXZ(Vec3f a, Vec3f b)
		{
			float num = a.X - b.X;
			float dz = a.Z - b.Z;
			return num * num + dz * dz;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00029068 File Offset: 0x00027268
		private static Vec3f ComputeRotatedAxisCenter(float fromX, float toX, float fromY, float toY, float fromZ, float toZ, float rotationOriginX, float rotationOriginZ, float rotYDeg)
		{
			float num = (fromX + toX) * 0.5f / 16f;
			float cy = (fromY + toY) * 0.5f / 16f;
			float cz = (fromZ + toZ) * 0.5f / 16f;
			Vec3f point = new Vec3f(num, cy, cz);
			Vec3f origin = new Vec3f(rotationOriginX / 16f, cy, rotationOriginZ / 16f);
			return BlockEntityRadioSupervisionConsole.RotateAroundPointXZ(point, origin, rotYDeg);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000290CC File Offset: 0x000272CC
		private static Vec3f RotateAroundPointXZ(Vec3f point, Vec3f origin, float rotDeg)
		{
			if (Math.Abs(rotDeg) < 0.001f)
			{
				return point;
			}
			float num = rotDeg * 0.017453292f;
			float cos = GameMath.Cos(num);
			float sin = GameMath.Sin(num);
			float num2 = point.X - origin.X;
			float dz = point.Z - origin.Z;
			float x = num2 * cos + dz * sin;
			float z = -num2 * sin + dz * cos;
			return new Vec3f(x + origin.X, point.Y, z + origin.Z);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00029144 File Offset: 0x00027344
		private static Vec3f RotateAroundCenter(Vec3f point, float rotDeg)
		{
			if (Math.Abs(rotDeg) < 0.001f)
			{
				return point;
			}
			float num = rotDeg * 0.017453292f;
			float cos = GameMath.Cos(num);
			float sin = GameMath.Sin(num);
			float num2 = point.X - 0.5f;
			float dz = point.Z - 0.5f;
			float x = num2 * cos + dz * sin;
			float z = -num2 * sin + dz * cos;
			return new Vec3f(x + 0.5f, point.Y, z + 0.5f);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000291B8 File Offset: 0x000273B8
		private Vec3f RotateLocalOffsetByBlockSide(Vec3f offsetNorth)
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = false;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = ((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			float num2;
			if (flag)
			{
				float num;
				if (!(side == "north"))
				{
					if (!(side == "east"))
					{
						if (!(side == "west"))
						{
							if (!(side == "south"))
							{
								num = 0f;
							}
							else
							{
								num = 180f;
							}
						}
						else
						{
							num = 90f;
						}
					}
					else
					{
						num = 270f;
					}
				}
				else
				{
					num = 0f;
				}
				num2 = num;
			}
			else
			{
				num2 = 0f;
			}
			float rotY = num2;
			return BlockEntityRadioSupervisionConsole.RotateAroundCenter(offsetNorth, rotY);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00029276 File Offset: 0x00027476
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (api.Side == 1)
			{
				RadioBlockIndex.RegisterSupervisionConsole(this.Pos);
			}
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00029293 File Offset: 0x00027493
		public override void OnNetworkCreated(long networkID)
		{
			base.OnNetworkCreated(networkID);
			if (this.originalCreatedNetworkID == 0L)
			{
				this.originalCreatedNetworkID = networkID;
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x000292B4 File Offset: 0x000274B4
		public bool OnInteract(IPlayer byPlayer)
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				return true;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi == null)
			{
				return true;
			}
			RadioConsoleDialog radioConsoleDialog = this.dialog;
			if (radioConsoleDialog != null && radioConsoleDialog.IsOpened())
			{
				return true;
			}
			this.dialog = new RadioConsoleDialog(capi, this);
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002931C File Offset: 0x0002751C
		public void RequestSetFrequency(string desired)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetFrequency,
				Value = (desired ?? "")
			});
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002937C File Offset: 0x0002757C
		public void RequestSetDisplayName(string desired)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetDisplayName,
				Value = (desired ?? "")
			});
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000293DB File Offset: 0x000275DB
		public void RequestSaveSettings(string desiredFrequency, string desiredDisplayName)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			this.RequestSetFrequency(desiredFrequency);
			this.RequestSetDisplayName(desiredDisplayName);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00029408 File Offset: 0x00027608
		public bool TrySetFrequency(string desired)
		{
			string normalized = (desired ?? "").Trim();
			if (RadioFrequencyUtil.Matches(normalized, this.frequency))
			{
				return true;
			}
			ICoreAPI api = this.Api;
			if (((api != null) ? api.World : null) != null && !RadioTransmitFrequencyGuard.IsFrequencyAvailable(this.Api.World, normalized, this.Pos))
			{
				return false;
			}
			this.frequency = normalized;
			this.MarkDirty(false, null);
			RadioConsoleDialog radioConsoleDialog = this.dialog;
			if (radioConsoleDialog != null)
			{
				radioConsoleDialog.RefreshData();
			}
			ICoreAPI api2 = this.Api;
			if (api2 != null && api2.Side == 1 && base.NetworkUID != 0L)
			{
				RadioRfPresenceRegistry.UpdateWiredSourceFrequency(base.NetworkUID, this.frequency);
				foreach (BlockEntityRadioEmitter blockEntityRadioEmitter in RadioWireNetworkHelper.FindEmitters(this))
				{
					blockEntityRadioEmitter.PublishRfPresence();
				}
			}
			return true;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000294F4 File Offset: 0x000276F4
		public void SetDisplayName(string desired)
		{
			this.displayName = (desired ?? "").Trim();
			this.MarkDirty(false, null);
			RadioConsoleDialog radioConsoleDialog = this.dialog;
			if (radioConsoleDialog == null)
			{
				return;
			}
			radioConsoleDialog.RefreshData();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00029524 File Offset: 0x00027724
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.frequency = tree.GetString("rpvc:radioFrequency", this.frequency);
			this.displayName = tree.GetString("rpvc:radioDisplayName", this.displayName);
			this.originalCreatedNetworkID = tree.GetLong("rpvc:radioConsoleCreatedNetworkId", this.originalCreatedNetworkID);
			RadioConsoleDialog radioConsoleDialog = this.dialog;
			if (radioConsoleDialog == null)
			{
				return;
			}
			radioConsoleDialog.RefreshData();
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00029590 File Offset: 0x00027790
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("rpvc:radioFrequency", this.frequency ?? "");
			tree.SetString("rpvc:radioDisplayName", this.displayName ?? "");
			if (this.originalCreatedNetworkID != 0L)
			{
				tree.SetLong("rpvc:radioConsoleCreatedNetworkId", this.originalCreatedNetworkID);
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x000295F1 File Offset: 0x000277F1
		public override void OnBlockRemoved()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				RadioBlockIndex.UnregisterSupervisionConsole(this.Pos);
			}
			base.OnBlockRemoved();
			RadioConsoleDialog radioConsoleDialog = this.dialog;
			if (radioConsoleDialog == null)
			{
				return;
			}
			radioConsoleDialog.TryClose();
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002962C File Offset: 0x0002782C
		public override void OnBlockUnloaded()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				RadioBlockIndex.UnregisterSupervisionConsole(this.Pos);
			}
			base.OnBlockUnloaded();
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00029754 File Offset: 0x00027954
		[CompilerGenerated]
		internal static void <AssignAxesByBestGlobalDistance>g__Search|18_0(int depth, float cost, ref BlockEntityRadioSupervisionConsole.<>c__DisplayClass18_0 A_2)
		{
			if (depth == A_2.count)
			{
				if (cost < A_2.bestCost)
				{
					A_2.bestCost = cost;
					Array.Copy(A_2.current, A_2.best, A_2.count);
				}
				return;
			}
			for (int axis = 0; axis < A_2.axisOffsetsCurrent.Count; axis++)
			{
				if (!A_2.usedAxes[axis])
				{
					float nextCost = cost + BlockEntityRadioSupervisionConsole.DistanceSqXZ(A_2.entries[depth].Item2, A_2.axisOffsetsCurrent[axis]);
					if (nextCost < A_2.bestCost)
					{
						A_2.usedAxes[axis] = true;
						A_2.current[depth] = axis;
						BlockEntityRadioSupervisionConsole.<AssignAxesByBestGlobalDistance>g__Search|18_0(depth + 1, nextCost, ref A_2);
						A_2.usedAxes[axis] = false;
					}
				}
			}
		}

		// Token: 0x0400036F RID: 879
		private RadioConsoleDialog dialog;

		// Token: 0x04000370 RID: 880
		private string frequency = "100.0";

		// Token: 0x04000371 RID: 881
		private string displayName = "";

		// Token: 0x04000372 RID: 882
		private long originalCreatedNetworkID;

		// Token: 0x04000373 RID: 883
		private static readonly Vec3f[] AxisOffsetsNorth = new Vec3f[]
		{
			BlockEntityRadioSupervisionConsole.ComputeRotatedAxisCenter(5f, 6f, 14f, 15.25f, -2f, -1f, 10f, 1f, 45f),
			BlockEntityRadioSupervisionConsole.ComputeRotatedAxisCenter(11f, 12f, 14f, 15.25f, -2f, -1f, 16f, 1f, 45f)
		};
	}
}
