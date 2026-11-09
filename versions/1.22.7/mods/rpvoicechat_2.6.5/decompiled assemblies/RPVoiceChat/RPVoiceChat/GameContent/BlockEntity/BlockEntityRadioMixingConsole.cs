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
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E9 RID: 233
	public class BlockEntityRadioMixingConsole : BEWireNode, IWireTypedNode, IRadioProgramSource, IBlockEntityWithCustomLightPosition
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0002743F File Offset: 0x0002563F
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00027442 File Offset: 0x00025642
		protected override int MaxConnections
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x00027445 File Offset: 0x00025645
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Radio;
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00027448 File Offset: 0x00025648
		public override Vec3f GetWireAttachmentOffsetFor(BlockPos otherNodePos)
		{
			if (otherNodePos == null)
			{
				return base.GetWireAttachmentOffsetFor(otherNodePos);
			}
			Vec3f[] axisOffsetsCurrent = Enumerable.ToArray<Vec3f>(Enumerable.Select<Vec3f, Vec3f>(BlockEntityRadioMixingConsole.AxisOffsetsNorth, new Func<Vec3f, Vec3f>(this.RotateLocalOffsetByBlockSide)));
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
			index = GameMath.Clamp(index, 0, BlockEntityRadioMixingConsole.AxisOffsetsNorth.Length - 1);
			return axisOffsetsCurrent[index];
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000275DC File Offset: 0x000257DC
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
			BlockEntityRadioMixingConsole.<>c__DisplayClass15_0 CS$<>8__locals1;
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
			BlockEntityRadioMixingConsole.<AssignAxesByBestGlobalDistance>g__Search|15_0(0, 0f, ref CS$<>8__locals1);
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

		// Token: 0x06000750 RID: 1872 RVA: 0x000276F8 File Offset: 0x000258F8
		private static float DistanceSqXZ(Vec3f a, Vec3f b)
		{
			float num = a.X - b.X;
			float dz = a.Z - b.Z;
			return num * num + dz * dz;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00027728 File Offset: 0x00025928
		private static Vec3f ComputeRotatedAxisCenter(float fromX, float toX, float fromZ, float toZ, float rotationOriginX, float rotationOriginZ, float rotYDeg)
		{
			float num = (fromX + toX) * 0.5f / 16f;
			float cy = 0.3828125f;
			float cz = (fromZ + toZ) * 0.5f / 16f;
			Vec3f point = new Vec3f(num, cy, cz);
			Vec3f origin = new Vec3f(rotationOriginX / 16f, cy, rotationOriginZ / 16f);
			return BlockEntityRadioMixingConsole.RotateAroundPointXZ(point, origin, rotYDeg);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00027780 File Offset: 0x00025980
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

		// Token: 0x06000753 RID: 1875 RVA: 0x000277F8 File Offset: 0x000259F8
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

		// Token: 0x06000754 RID: 1876 RVA: 0x0002786C File Offset: 0x00025A6C
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
			return BlockEntityRadioMixingConsole.RotateAroundCenter(offsetNorth, rotY);
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0002792A File Offset: 0x00025B2A
		public bool IsOnAir
		{
			get
			{
				return this.isOnAir;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00027932 File Offset: 0x00025B32
		public string HlsStreamUrl
		{
			get
			{
				return this.hlsStreamUrl ?? "";
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x00027943 File Offset: 0x00025B43
		public string ActiveOperatorPlayerUid
		{
			get
			{
				return this.activeOperatorPlayerUid ?? "";
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00027954 File Offset: 0x00025B54
		public string ProgramRouteKey
		{
			get
			{
				return RadioProgramRouteKey.ForMixingConsole(this.Pos);
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00027961 File Offset: 0x00025B61
		public bool IsBusyForOtherPlayer(string playerUid)
		{
			return this.isOnAir && !this.IsOperator(playerUid);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00027977 File Offset: 0x00025B77
		public bool HasWiredBroadcastPath()
		{
			return base.NetworkUID != 0L && (Enumerable.Any<BlockEntityRadioEmitter>(RadioWireNetworkHelper.FindEmitters(this)) || Enumerable.Any<BlockEntitySpeaker>(RadioWireNetworkHelper.FindSpeakers(this)));
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x000279A0 File Offset: 0x00025BA0
		public bool HasActiveBroadcastOutput()
		{
			bool flag = Enumerable.Any<BlockEntitySpeaker>(RadioWireNetworkHelper.FindSpeakers(this));
			bool hasPoweredEmitter = Enumerable.Any<BlockEntityRadioEmitter>(RadioWireNetworkHelper.FindEmitters(this), (BlockEntityRadioEmitter emitter) => emitter.IsWirelessTransmitting);
			return flag || hasPoweredEmitter;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000279E8 File Offset: 0x00025BE8
		public bool IsOperator(string playerUid)
		{
			if (!this.isOnAir || string.IsNullOrWhiteSpace(this.activeOperatorPlayerUid))
			{
				return true;
			}
			ICoreAPI api = this.Api;
			return (api != null && api.Side == 1 && this.Api.World.PlayerByUid(this.activeOperatorPlayerUid) == null) || this.activeOperatorPlayerUid == playerUid;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00027A49 File Offset: 0x00025C49
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			base.OnConnectionsChanged += new Action(this.OnRadioWireConnectionsChanged);
			if (api.Side == 1)
			{
				RadioBlockIndex.RegisterMixingConsole(this.Pos);
				this.PublishProgramPresence();
			}
			this.SyncOnAirVisuals();
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00027A84 File Offset: 0x00025C84
		public Vec3d GetLightOrigin()
		{
			Vec3f local = this.RotateLocalOffsetByBlockSide(new Vec3f(0.5f, 0.72f, 0.42f));
			return this.Pos.ToVec3d().Add((double)local.X, (double)local.Y, (double)local.Z);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00027AD4 File Offset: 0x00025CD4
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			if (this.Api is ICoreClientAPI)
			{
				Block block = base.Block;
				AssetLocation assetLocation;
				if (block == null)
				{
					assetLocation = null;
				}
				else
				{
					CompositeShape shape2 = block.Shape;
					assetLocation = ((shape2 != null) ? shape2.Base : null);
				}
				if (!(assetLocation == null))
				{
					CompositeShape blockShape = base.Block.Shape;
					AssetLocation shapeLoc = blockShape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
					Shape shape = Shape.TryGet(this.Api, shapeLoc);
					if (shape == null)
					{
						return false;
					}
					Dictionary<string, AssetLocation> textures = new Dictionary<string, AssetLocation>();
					if (base.Block.Textures != null)
					{
						foreach (KeyValuePair<string, CompositeTexture> entry in base.Block.Textures)
						{
							CompositeTexture value = entry.Value;
							AssetLocation assetLocation2;
							if ((assetLocation2 = ((value != null) ? value.Base : null)) == null)
							{
								CompositeTexture value2 = entry.Value;
								if (value2 == null)
								{
									assetLocation2 = null;
								}
								else
								{
									BakedCompositeTexture baked = value2.Baked;
									assetLocation2 = ((baked != null) ? baked.BakedName : null);
								}
							}
							AssetLocation loc = assetLocation2;
							if (loc != null)
							{
								textures[entry.Key] = loc;
							}
						}
					}
					textures["onairglass"] = (this.isOnAir ? BlockEntityRadioMixingConsole.OnAirGlassOnTexture : BlockEntityRadioMixingConsole.OnAirGlassOffTexture);
					ICoreClientAPI coreClientAPI = (ICoreClientAPI)this.Api;
					ITextureAtlasAPI blockTextureAtlas = ((ICoreClientAPI)this.Api).BlockTextureAtlas;
					Dictionary<string, AssetLocation> dictionary = textures;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(10, 1);
					defaultInterpolatedStringHandler.AppendLiteral("For block ");
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(base.Block.Code);
					ContainedTextureSource texSource = new ContainedTextureSource(coreClientAPI, blockTextureAtlas, dictionary, defaultInterpolatedStringHandler.ToStringAndClear());
					MeshData mesh;
					tesselator.TesselateShape("rpvoicechat:radiomixingconsole", shape, ref mesh, texSource, new Vec3f(blockShape.rotateX, blockShape.rotateY, blockShape.rotateZ), 0, 0, 0, default(int?), null);
					mesher.AddMeshData(mesh, 1);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00027CB0 File Offset: 0x00025EB0
		private void SyncOnAirVisuals()
		{
			BEBehaviorLightable light = base.GetBehavior<BEBehaviorLightable>();
			if (light == null)
			{
				return;
			}
			light.SetLightColor(new Vec3f(1f, 0.05f, 0.02f));
			light.SetLightActive(this.isOnAir);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00027CF0 File Offset: 0x00025EF0
		private void OnRadioWireConnectionsChanged()
		{
			ICoreServerAPI sapi = this.Api as ICoreServerAPI;
			if (sapi != null)
			{
				WireTopologyConnectivity.NotifyNode(sapi, this);
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00027D14 File Offset: 0x00025F14
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
			RadioMixingConsoleDialog radioMixingConsoleDialog = this.dialog;
			if (radioMixingConsoleDialog != null && radioMixingConsoleDialog.IsOpened())
			{
				return true;
			}
			this.dialog = new RadioMixingConsoleDialog(capi, this);
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00027D7C File Offset: 0x00025F7C
		public void RequestSetHlsUrl(string desired)
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
				Operation = RadioSettingsOperation.SetMixingConsoleHlsUrl,
				Value = (desired ?? "")
			});
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00027DDC File Offset: 0x00025FDC
		public void RequestSetOnAir(bool enabled)
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
				Operation = RadioSettingsOperation.SetMixingConsoleOnAir,
				IntValue = ((enabled > false) ? 1 : 0)
			});
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00027E38 File Offset: 0x00026038
		public bool SetHlsUrl(string desired)
		{
			string normalized = BlockEntityRadioMixingConsole.NormalizeHlsUrl(desired);
			if (normalized == null)
			{
				return false;
			}
			this.hlsStreamUrl = normalized;
			this.PublishProgramPresence();
			this.MarkDirty(true, null);
			RadioMixingConsoleDialog radioMixingConsoleDialog = this.dialog;
			if (radioMixingConsoleDialog != null)
			{
				radioMixingConsoleDialog.RefreshData();
			}
			return true;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00027E78 File Offset: 0x00026078
		public MixingConsoleOnAirResult SetOnAir(IPlayer byPlayer, bool enabled)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || byPlayer == null)
			{
				return MixingConsoleOnAirResult.NotOperator;
			}
			if (enabled)
			{
				if (this.isOnAir)
				{
					return MixingConsoleOnAirResult.AlreadyOnAir;
				}
				if (base.NetworkUID == 0L)
				{
					return MixingConsoleOnAirResult.NotWired;
				}
				if (!this.HasWiredBroadcastPath())
				{
					return MixingConsoleOnAirResult.NoBroadcastPath;
				}
				this.isOnAir = true;
				this.activeOperatorPlayerUid = byPlayer.PlayerUID;
			}
			else
			{
				if (!this.IsOperator(byPlayer.PlayerUID))
				{
					return MixingConsoleOnAirResult.NotOperator;
				}
				this.ClearOnAirInternal();
			}
			this.SyncOnAirVisuals();
			this.PublishProgramPresence();
			this.MarkDirty(true, null);
			RadioMixingConsoleDialog radioMixingConsoleDialog = this.dialog;
			if (radioMixingConsoleDialog != null)
			{
				radioMixingConsoleDialog.RefreshData();
			}
			return MixingConsoleOnAirResult.Success;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00027F18 File Offset: 0x00026118
		public static string GetOnAirFailureLangKey(MixingConsoleOnAirResult result)
		{
			string result2;
			switch (result)
			{
			case MixingConsoleOnAirResult.AlreadyOnAir:
				result2 = "Radio.MixingConsole.Error.Busy";
				break;
			case MixingConsoleOnAirResult.NotOperator:
				result2 = "Radio.MixingConsole.Error.NotOperator";
				break;
			case MixingConsoleOnAirResult.NotWired:
				result2 = "Radio.MixingConsole.Error.NotWired";
				break;
			case MixingConsoleOnAirResult.NoBroadcastPath:
				result2 = "Radio.MixingConsole.Error.NoBroadcastPath";
				break;
			default:
				result2 = null;
				break;
			}
			return result2;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00027F64 File Offset: 0x00026164
		public void ClearOnAir()
		{
			if (!this.isOnAir && string.IsNullOrWhiteSpace(this.activeOperatorPlayerUid))
			{
				return;
			}
			this.ClearOnAirInternal();
			this.SyncOnAirVisuals();
			this.PublishProgramPresence();
			this.MarkDirty(true, null);
			RadioMixingConsoleDialog radioMixingConsoleDialog = this.dialog;
			if (radioMixingConsoleDialog != null)
			{
				radioMixingConsoleDialog.RefreshData();
			}
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				RadioVoiceRoutingSystem modSystem = this.Api.ModLoader.GetModSystem<RadioVoiceRoutingSystem>(true);
				if (modSystem == null)
				{
					return;
				}
				modSystem.ClearProgramRoute(this.ProgramRouteKey);
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00027FEA File Offset: 0x000261EA
		private void ClearOnAirInternal()
		{
			this.isOnAir = false;
			this.activeOperatorPlayerUid = "";
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00028000 File Offset: 0x00026200
		public void PublishProgramPresence()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || this.Pos == null)
			{
				return;
			}
			RadioRfPresenceRegistry.UpsertProgram(new RadioRfProgramPresence
			{
				Pos = this.Pos.Copy(),
				Dimension = this.Pos.dimension,
				NetworkId = base.NetworkUID,
				IsOnAir = this.isOnAir,
				HlsStreamUrl = (this.hlsStreamUrl ?? ""),
				ActiveOperatorPlayerUid = (this.activeOperatorPlayerUid ?? "")
			});
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000280A4 File Offset: 0x000262A4
		public static string NormalizeHlsUrl(string desired)
		{
			if (desired == null)
			{
				return "";
			}
			string trimmed = desired.Trim();
			if (trimmed.Length == 0)
			{
				return "";
			}
			if (trimmed.Length > 2048)
			{
				return null;
			}
			Uri uri;
			if (!Uri.TryCreate(trimmed, 1, ref uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
			{
				return null;
			}
			return uri.AbsoluteUri;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00028118 File Offset: 0x00026318
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			if (this.Api == null && ((worldForResolving != null) ? worldForResolving.Api : null) != null)
			{
				this.Api = worldForResolving.Api;
			}
			base.FromTreeAttributes(tree, worldForResolving);
			this.hlsStreamUrl = tree.GetString("rpvc:mixingConsoleHlsUrl", this.hlsStreamUrl);
			this.isOnAir = tree.GetBool("rpvc:mixingConsoleOnAir", false);
			this.activeOperatorPlayerUid = tree.GetString("rpvc:mixingConsoleOperatorUid", "");
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				ICoreClientAPI capi = this.Api as ICoreClientAPI;
				if (capi != null)
				{
					capi.Event.EnqueueMainThreadTask(delegate()
					{
						this.SyncOnAirVisuals();
						RadioMixingConsoleDialog radioMixingConsoleDialog2 = this.dialog;
						if (radioMixingConsoleDialog2 == null)
						{
							return;
						}
						radioMixingConsoleDialog2.RefreshData();
					}, "rpvoicechat:MixingConsoleFromTree");
					return;
				}
			}
			this.SyncOnAirVisuals();
			RadioMixingConsoleDialog radioMixingConsoleDialog = this.dialog;
			if (radioMixingConsoleDialog == null)
			{
				return;
			}
			radioMixingConsoleDialog.RefreshData();
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000281E8 File Offset: 0x000263E8
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("rpvc:mixingConsoleHlsUrl", this.hlsStreamUrl ?? "");
			tree.SetBool("rpvc:mixingConsoleOnAir", this.isOnAir);
			tree.SetString("rpvc:mixingConsoleOperatorUid", this.activeOperatorPlayerUid ?? "");
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00028244 File Offset: 0x00026444
		public override void OnBlockRemoved()
		{
			this.ClearOnAir();
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				RadioRfPresenceRegistry.RemoveProgram(this.Pos);
				RadioBlockIndex.UnregisterMixingConsole(this.Pos);
			}
			base.OnBlockRemoved();
			RadioMixingConsoleDialog radioMixingConsoleDialog = this.dialog;
			if (radioMixingConsoleDialog == null)
			{
				return;
			}
			radioMixingConsoleDialog.TryClose();
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002829B File Offset: 0x0002649B
		public override void OnBlockUnloaded()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.PublishProgramPresence();
				RadioBlockIndex.UnregisterMixingConsole(this.Pos);
			}
			base.OnBlockUnloaded();
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00028404 File Offset: 0x00026604
		[CompilerGenerated]
		internal static void <AssignAxesByBestGlobalDistance>g__Search|15_0(int depth, float cost, ref BlockEntityRadioMixingConsole.<>c__DisplayClass15_0 A_2)
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
					float nextCost = cost + BlockEntityRadioMixingConsole.DistanceSqXZ(A_2.entries[depth].Item2, A_2.axisOffsetsCurrent[axis]);
					if (nextCost < A_2.bestCost)
					{
						A_2.usedAxes[axis] = true;
						A_2.current[depth] = axis;
						BlockEntityRadioMixingConsole.<AssignAxesByBestGlobalDistance>g__Search|15_0(depth + 1, nextCost, ref A_2);
						A_2.usedAxes[axis] = false;
					}
				}
			}
		}

		// Token: 0x04000359 RID: 857
		private static readonly AssetLocation OnAirGlassOffTexture = new AssetLocation("rpvoicechat:block/radio/onairglass");

		// Token: 0x0400035A RID: 858
		private static readonly AssetLocation OnAirGlassOnTexture = new AssetLocation("rpvoicechat:block/radio/onairglass-on");

		// Token: 0x0400035B RID: 859
		private const int MaxHlsUrlLength = 2048;

		// Token: 0x0400035C RID: 860
		private RadioMixingConsoleDialog dialog;

		// Token: 0x0400035D RID: 861
		private string hlsStreamUrl = "";

		// Token: 0x0400035E RID: 862
		private bool isOnAir;

		// Token: 0x0400035F RID: 863
		private string activeOperatorPlayerUid = "";

		// Token: 0x04000360 RID: 864
		private static readonly Vec3f[] AxisOffsetsNorth = new Vec3f[]
		{
			BlockEntityRadioMixingConsole.ComputeRotatedAxisCenter(4f, 5f, -2f, -1f, 9f, 1f, 45f),
			BlockEntityRadioMixingConsole.ComputeRotatedAxisCenter(8f, 9f, -2f, -1f, 13f, 1f, 45f),
			BlockEntityRadioMixingConsole.ComputeRotatedAxisCenter(12f, 13f, -2f, -1f, 17f, 1f, 45f)
		};
	}
}
