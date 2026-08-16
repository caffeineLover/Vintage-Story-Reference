using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011D RID: 285
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayerDamageModel
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00044266 File Offset: 0x00042466
		public DamageZoneStats[] DamageZones { get; }

		// Token: 0x06000DFE RID: 3582 RVA: 0x00044270 File Offset: 0x00042470
		public PlayerDamageModel(DamageZoneStatsJson[] zones)
		{
			this.DamageZones = Enumerable.ToArray<DamageZoneStats>(Enumerable.Where<DamageZoneStats>(Enumerable.Select<DamageZoneStatsJson, DamageZoneStats>(zones, (DamageZoneStatsJson zone) => zone.ToStats()), (DamageZoneStats zone) => zone.ZoneType > PlayerBodyPart.None));
			this._random = new NatFloat(0.5f, 0.5f, 0);
			this._weights = new Dictionary<PlayerBodyPart, float>();
			foreach (PlayerBodyPart zone2 in Enum.GetValues<PlayerBodyPart>())
			{
				this._weights[zone2] = 0f;
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00044324 File Offset: 0x00042524
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"zone",
			"damageMultiplier"
		})]
		public ValueTuple<PlayerBodyPart, float> GetZone(DirectionOffset? direction = null, PlayerBodyPart target = PlayerBodyPart.None, float multiplier = 1f)
		{
			IEnumerable<DamageZoneStats> enumerable;
			if (direction != null)
			{
				enumerable = Enumerable.Where<DamageZoneStats>(this.DamageZones, (DamageZoneStats zone) => zone.Directions.Check(direction.Value));
			}
			else
			{
				IEnumerable<DamageZoneStats> damageZones = this.DamageZones;
				enumerable = damageZones;
			}
			IEnumerable<DamageZoneStats> zones = enumerable;
			PlayerBodyPart playerBodyPart;
			float num;
			foreach (KeyValuePair<PlayerBodyPart, float> keyValuePair in this._weights)
			{
				keyValuePair.Deconstruct(ref playerBodyPart, ref num);
				PlayerBodyPart zone3 = playerBodyPart;
				this._weights[zone3] = 0f;
			}
			float sum = 0f;
			foreach (DamageZoneStats zone2 in zones)
			{
				float zoneMultiplier = ((target | zone2.ZoneType) != PlayerBodyPart.None) ? multiplier : 1f;
				sum += zone2.Coverage * zoneMultiplier;
				Dictionary<PlayerBodyPart, float> weights = this._weights;
				playerBodyPart = zone2.ZoneType;
				weights[playerBodyPart] += zone2.Coverage * zoneMultiplier;
			}
			foreach (KeyValuePair<PlayerBodyPart, float> keyValuePair in this._weights)
			{
				keyValuePair.Deconstruct(ref playerBodyPart, ref num);
				PlayerBodyPart playerBodyPart2 = playerBodyPart;
				Dictionary<PlayerBodyPart, float> weights = this._weights;
				playerBodyPart = playerBodyPart2;
				weights[playerBodyPart] /= sum;
			}
			float randomValue = this._random.nextFloat();
			sum = 0f;
			foreach (KeyValuePair<PlayerBodyPart, float> keyValuePair in this._weights)
			{
				keyValuePair.Deconstruct(ref playerBodyPart, ref num);
				PlayerBodyPart zone = playerBodyPart;
				float weight = num;
				sum += weight;
				if (sum >= randomValue)
				{
					return new ValueTuple<PlayerBodyPart, float>(zone, Enumerable.Average(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(zones, (DamageZoneStats element) => (element.ZoneType & zone) > PlayerBodyPart.None), (DamageZoneStats element) => element.DamageMultiplier)));
				}
			}
			return new ValueTuple<PlayerBodyPart, float>(PlayerBodyPart.None, 1f);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0004459C File Offset: 0x0004279C
		public float GetMultiplier(PlayerBodyPart zone)
		{
			return Enumerable.Average(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(this.DamageZones, (DamageZoneStats element) => (element.ZoneType & zone) > PlayerBodyPart.None), (DamageZoneStats element) => element.DamageMultiplier));
		}

		// Token: 0x040006E3 RID: 1763
		private readonly NatFloat _random;

		// Token: 0x040006E4 RID: 1764
		private readonly Dictionary<PlayerBodyPart, float> _weights;
	}
}
