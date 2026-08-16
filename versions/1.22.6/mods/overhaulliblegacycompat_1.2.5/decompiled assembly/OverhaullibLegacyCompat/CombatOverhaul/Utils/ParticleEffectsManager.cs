using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.Utils
{
	// Token: 0x0200002A RID: 42
	[NullableContext(1)]
	[Nullable(0)]
	public class ParticleEffectsManager
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000C3E0 File Offset: 0x0000A5E0
		public ParticleEffectsManager(ICoreAPI api)
		{
			foreach (IAsset asset in api.Assets.GetManyInCategory("config", "particle-effects.json", null, true))
			{
				try
				{
					string domain = asset.Location.Domain;
					byte[] data = asset.Data;
					data = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, data);
					foreach (KeyValuePair<string, JToken> keyValuePair in JObject.Parse(Encoding.Unicode.GetString(data)))
					{
						string text;
						JToken jtoken;
						keyValuePair.Deconstruct(ref text, ref jtoken);
						string code = text;
						AdvancedParticleProperties particleProperties = new JsonObject(jtoken).AsObject<AdvancedParticleProperties>(null);
						particleProperties.VelocityEvolve = null;
						this._particleProperties.Add(domain + ":" + code, particleProperties);
					}
				}
				catch (Exception exception)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Error on parsing particle effects for '");
					defaultInterpolatedStringHandler.AppendFormatted(asset.Location.Domain);
					defaultInterpolatedStringHandler.AppendLiteral("':\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			this._api = api;
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi != null)
			{
				this._clientChannel = clientApi.Network.RegisterChannel("CombatOverhaul:particle-effects").RegisterMessageType<ParticleEffectsPacket>();
			}
			ICoreServerAPI serverApi = api as ICoreServerAPI;
			if (serverApi != null)
			{
				serverApi.Network.RegisterChannel("CombatOverhaul:particle-effects").RegisterMessageType<ParticleEffectsPacket>().SetMessageHandler<ParticleEffectsPacket>(delegate(IServerPlayer player, ParticleEffectsPacket packet)
				{
					this.SpawnParticleEffect(packet, player);
				});
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000C5EC File Offset: 0x0000A7EC
		public AdvancedParticleProperties Get(string code, string domain)
		{
			string key = domain + ":" + code;
			AdvancedParticleProperties value;
			if (!this._particleProperties.TryGetValue(key, ref value))
			{
				throw new InvalidOperationException("Particle effect '" + key + "' not found");
			}
			return value;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000C630 File Offset: 0x0000A830
		public AdvancedParticleProperties Get(string code)
		{
			AdvancedParticleProperties value;
			if (!this._particleProperties.TryGetValue(code, ref value))
			{
				throw new InvalidOperationException("Particle effect '" + code + "' not found");
			}
			return value;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000C664 File Offset: 0x0000A864
		public void Spawn(EntityPlayer player, string code, Vector3 position, Vector3 velocity, float intensity)
		{
			ParticleEffectsPacket packet = ParticleEffectsManager.PreparePacket(code, player, position, velocity, intensity);
			if (this._api.Side == 2)
			{
				IClientNetworkChannel clientChannel = this._clientChannel;
				if (clientChannel != null)
				{
					clientChannel.SendPacket<ParticleEffectsPacket>(packet);
				}
				this.SpawnParticleEffect(packet, player.Player);
				return;
			}
			this.SpawnParticleEffect(packet, null);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		public void Spawn(string code, Vector3 position, Vector3 velocity, float intensity)
		{
			ParticleEffectsPacket packet = ParticleEffectsManager.PreparePacket(code, position, velocity, intensity);
			this.SpawnParticleEffect(packet, null);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000C6D4 File Offset: 0x0000A8D4
		public void Spawn(string code, Vector3d position, Vector3 velocity, float intensity)
		{
			ParticleEffectsPacket packet = ParticleEffectsManager.PreparePacket(code, position, velocity, intensity);
			this.SpawnParticleEffect(packet, null);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		public void Draw(string id)
		{
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000C6F8 File Offset: 0x0000A8F8
		private void SpawnDebugParticles(string code, float offset)
		{
			ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
			Vec3d vec3d = (coreClientAPI != null) ? coreClientAPI.World.Player.Entity.Pos.AheadCopy((double)offset).XYZ : null;
			ICoreClientAPI coreClientAPI2 = this._api as ICoreClientAPI;
			Vector3d position = (vec3d + ((coreClientAPI2 != null) ? coreClientAPI2.World.Player.Entity.LocalEyePos : null)).ToOpenTK();
			this.Spawn(code, position, Vector3.Zero, 1f);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000C77C File Offset: 0x0000A97C
		private static ParticleEffectsPacket PreparePacket(string code, Entity player, Vector3 position, Vector3 velocity, float intensity)
		{
			Vector3 worldPosition = ParticleEffectsManager.FromCameraReferenceFrame(player, position);
			Vector3 worldVelocity = ParticleEffectsManager.FromCameraReferenceFrame(player, velocity);
			Vec3f vanillaPlayerPosition = player.SidedPos.AheadCopy(0.0).XYZFloat.Add(0f, (float)player.LocalEyePos.Y, 0f);
			Vector3 effectPosition = new Vector3(vanillaPlayerPosition.X, vanillaPlayerPosition.Y, vanillaPlayerPosition.Z) + worldPosition;
			return new ParticleEffectsPacket
			{
				Code = code,
				Position = new double[]
				{
					(double)effectPosition.X,
					(double)effectPosition.Y,
					(double)effectPosition.Z
				},
				Velocity = new float[]
				{
					worldVelocity.X,
					worldVelocity.Y,
					worldVelocity.Z
				},
				Intensity = intensity
			};
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000C85C File Offset: 0x0000AA5C
		private static ParticleEffectsPacket PreparePacket(string code, Vector3 position, Vector3 velocity, float intensity)
		{
			return new ParticleEffectsPacket
			{
				Code = code,
				Position = new double[]
				{
					(double)position.X,
					(double)position.Y,
					(double)position.Z
				},
				Velocity = new float[]
				{
					velocity.X,
					velocity.Y,
					velocity.Z
				},
				Intensity = intensity
			};
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		private static ParticleEffectsPacket PreparePacket(string code, Vector3d position, Vector3 velocity, float intensity)
		{
			return new ParticleEffectsPacket
			{
				Code = code,
				Position = new double[]
				{
					position.X,
					position.Y,
					position.Z
				},
				Velocity = new float[]
				{
					velocity.X,
					velocity.Y,
					velocity.Z
				},
				Intensity = intensity
			};
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000C950 File Offset: 0x0000AB50
		[NullableContext(2)]
		private void SpawnParticleEffect(ParticleEffectsPacket packet, IPlayer player)
		{
			AdvancedParticleProperties effect;
			try
			{
				effect = this.Get(packet.Code);
			}
			catch (Exception exception)
			{
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error on spawning particles: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			effect.basePos = new Vec3d(packet.Position[0], packet.Position[1], packet.Position[2]);
			NatFloat velocityX = effect.Velocity[0].Clone();
			NatFloat velocityY = effect.Velocity[1].Clone();
			NatFloat velocityZ = effect.Velocity[2].Clone();
			effect.Velocity[0].avg += packet.Velocity[0];
			effect.Velocity[1].avg += packet.Velocity[1];
			effect.Velocity[2].avg += packet.Velocity[2];
			NatFloat quantity = effect.Quantity.Clone();
			effect.Quantity.avg *= packet.Intensity;
			effect.Quantity.var *= packet.Intensity;
			this._api.World.SpawnParticles(effect, player);
			effect.Quantity = quantity;
			effect.Velocity[0] = velocityX;
			effect.Velocity[1] = velocityY;
			effect.Velocity[2] = velocityZ;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000CAD4 File Offset: 0x0000ACD4
		private static Vector3 FromCameraReferenceFrame(Entity player, Vector3 position)
		{
			Vec3f vanillaViewVector = player.SidedPos.GetViewVector().Normalize();
			Vector3 vector = new Vector3(vanillaViewVector.X, vanillaViewVector.Y, vanillaViewVector.Z);
			Vector3 vertical;
			vertical..ctor(0f, 1f, 0f);
			Vector3 localZ = vector;
			Vector3 vector2 = Vector3.Normalize(Vector3.Cross(localZ, vertical));
			Vector3 localY = Vector3.Cross(vector2, localZ);
			return vector2 * position.X + localY * position.Y + localZ * position.Z;
		}

		// Token: 0x04000113 RID: 275
		public const string ParticleEffectsFileName = "particle-effects.json";

		// Token: 0x04000114 RID: 276
		private int _selected;

		// Token: 0x04000115 RID: 277
		private float _debugParticlesOffset = 1f;

		// Token: 0x04000116 RID: 278
		private readonly ICoreAPI _api;

		// Token: 0x04000117 RID: 279
		private readonly Dictionary<string, AdvancedParticleProperties> _particleProperties = new Dictionary<string, AdvancedParticleProperties>();

		// Token: 0x04000118 RID: 280
		[Nullable(2)]
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x04000119 RID: 281
		private const string _networkChannelId = "CombatOverhaul:particle-effects";
	}
}
