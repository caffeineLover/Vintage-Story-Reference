using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Implementations;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200012B RID: 299
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ServerDamageBlockProfiles
	{
		// Token: 0x06000E6D RID: 3693 RVA: 0x000458A4 File Offset: 0x00043AA4
		internal static bool TryResolve(IServerPlayer player, DamageBlockPacket requested, out DamageBlockPacket authorized)
		{
			authorized = null;
			if (requested.Directions == null || requested.Directions.Length < 4 || requested.Directions.Length > 64)
			{
				return false;
			}
			if (!Enum.IsDefined<EnumDamageBlockKind>(requested.Kind))
			{
				return false;
			}
			foreach (DamageBlockPacket candidate in ServerDamageBlockProfiles.GetAllowedPackets(player, requested.MainHand))
			{
				if (ServerDamageBlockProfiles.Matches(requested, candidate))
				{
					candidate.Id = requested.Id;
					authorized = candidate;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00045944 File Offset: 0x00043B44
		internal static IReadOnlyList<DamageBlockPacket> GetAllowedPackets(IServerPlayer player, bool mainHand)
		{
			ItemStack stack = (mainHand ? player.Entity.RightHandItemSlot : player.Entity.LeftHandItemSlot).Itemstack;
			bool flag;
			if (stack == null)
			{
				flag = (null != null);
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				flag = (((collectible != null) ? collectible.Attributes : null) != null);
			}
			if (!flag)
			{
				return Array.Empty<DamageBlockPacket>();
			}
			if (stack.Collectible.GetCollectibleInterface<IHasServerBlockCallback>() == null)
			{
				return Array.Empty<DamageBlockPacket>();
			}
			IReadOnlyList<DamageBlockPacket> result;
			try
			{
				IEnumerable<ServerDamageBlockProfiles.ConfiguredProfile> enumerable = (stack.Collectible is StanceBasedMeleeWeapon) ? ServerDamageBlockProfiles.GetLegacyProfiles(player.Entity, stack, mainHand) : ServerDamageBlockProfiles.GetModernProfiles(player.Entity, stack, mainHand);
				List<DamageBlockPacket> packets = new List<DamageBlockPacket>();
				foreach (ServerDamageBlockProfiles.ConfiguredProfile profile in enumerable)
				{
					DamageBlockPacket packet = profile.Block.ToPacket();
					packet.MainHand = mainHand;
					packet.Kind = profile.Kind;
					packets.Add(packet);
				}
				result = packets;
			}
			catch
			{
				result = Array.Empty<DamageBlockPacket>();
			}
			return result;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00045A54 File Offset: 0x00043C54
		internal static bool IsStillAuthorized(IServerPlayer player, DamageBlockStats current)
		{
			foreach (DamageBlockPacket candidate in ServerDamageBlockProfiles.GetAllowedPackets(player, current.MainHand))
			{
				if (current.ZoneType == (PlayerBodyPart)candidate.Zones && current.Kind == candidate.Kind && ServerDamageBlockProfiles.DirectionsEffectivelyEqual(current.Directions.ToArray(), candidate.Directions) && string.Equals(current.Sound, candidate.Sound, 4) && ServerDamageBlockProfiles.TiersEqual(current.BlockTier, candidate.BlockTier) && current.CanBlockProjectiles == candidate.CanBlockProjectiles && (int)current.StaggerTime.TotalMilliseconds == candidate.StaggerTimeMs && current.StaggerTier == candidate.StaggerTier)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00045B3C File Offset: 0x00043D3C
		private static IEnumerable<ServerDamageBlockProfiles.ConfiguredProfile> GetModernProfiles(EntityPlayer player, ItemStack stack, bool mainHand)
		{
			ServerDamageBlockProfiles.<GetModernProfiles>d__3 <GetModernProfiles>d__ = new ServerDamageBlockProfiles.<GetModernProfiles>d__3(-2);
			<GetModernProfiles>d__.<>3__player = player;
			<GetModernProfiles>d__.<>3__stack = stack;
			<GetModernProfiles>d__.<>3__mainHand = mainHand;
			return <GetModernProfiles>d__;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00045B5C File Offset: 0x00043D5C
		[return: Nullable(2)]
		private static StanceStats GetModernStance(MeleeWeaponStats stats, EntityPlayer player, bool mainHand)
		{
			ItemSlot otherSlot = mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot;
			ItemStack itemstack = otherSlot.Itemstack;
			string text;
			if (itemstack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = collectible.Code;
					text = ((code != null) ? code.ToString() : null);
				}
			}
			string otherCode = text ?? string.Empty;
			if (!mainHand)
			{
				return ServerDamageBlockProfiles.GetMatchingDualWieldStance(stats.OffHandDualWieldStances, otherCode) ?? stats.OffHandStance;
			}
			if (otherSlot.Empty && stats.TwoHandedStance != null)
			{
				return stats.TwoHandedStance;
			}
			return ServerDamageBlockProfiles.GetMatchingDualWieldStance(stats.MainHandDualWieldStances, otherCode) ?? stats.OneHandedStance;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00045BF8 File Offset: 0x00043DF8
		[return: Nullable(2)]
		private static StanceStats GetMatchingDualWieldStance(Dictionary<string, StanceStats> stances, string otherCode)
		{
			if (otherCode.Length == 0)
			{
				return null;
			}
			foreach (KeyValuePair<string, StanceStats> keyValuePair in stances)
			{
				string text;
				StanceStats stanceStats;
				keyValuePair.Deconstruct(ref text, ref stanceStats);
				string text2 = text;
				StanceStats stance = stanceStats;
				if (WildcardUtil.Match(text2, otherCode))
				{
					return stance;
				}
			}
			return null;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00045C6C File Offset: 0x00043E6C
		private static IEnumerable<ServerDamageBlockProfiles.ConfiguredProfile> GetLegacyProfiles(EntityPlayer player, ItemStack stack, bool mainHand)
		{
			ServerDamageBlockProfiles.<GetLegacyProfiles>d__6 <GetLegacyProfiles>d__ = new ServerDamageBlockProfiles.<GetLegacyProfiles>d__6(-2);
			<GetLegacyProfiles>d__.<>3__player = player;
			<GetLegacyProfiles>d__.<>3__stack = stack;
			<GetLegacyProfiles>d__.<>3__mainHand = mainHand;
			return <GetLegacyProfiles>d__;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00045C8A File Offset: 0x00043E8A
		private static IEnumerable<DamageBlockJson> GetLegacyParries(StanceBasedMeleeWeaponGripStats grip)
		{
			ServerDamageBlockProfiles.<GetLegacyParries>d__7 <GetLegacyParries>d__ = new ServerDamageBlockProfiles.<GetLegacyParries>d__7(-2);
			<GetLegacyParries>d__.<>3__grip = grip;
			return <GetLegacyParries>d__;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00045C9C File Offset: 0x00043E9C
		private static DamageBlockJson ApplyTierBonus(DamageBlockJson source, int bonus)
		{
			DamageBlockJson result = source.Clone();
			if (result.BlockTier == null)
			{
				return result;
			}
			foreach (string text in Enumerable.ToArray<string>(result.BlockTier.Keys))
			{
				Dictionary<string, int> blockTier = result.BlockTier;
				string text2 = text;
				blockTier[text2] += bonus;
			}
			return result;
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00045CF8 File Offset: 0x00043EF8
		private static bool HasProjectileParryTrait(EntityPlayer player)
		{
			bool result;
			try
			{
				result = player.Api.ModLoader.GetModSystem<CharacterSystem>(true).HasTrait(player.Player, "canParryProjectiles");
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00045D40 File Offset: 0x00043F40
		private static bool Matches(DamageBlockPacket requested, DamageBlockPacket authorized)
		{
			return requested.MainHand == authorized.MainHand && (requested.Kind == EnumDamageBlockKind.Unknown || requested.Kind == authorized.Kind) && requested.Zones == authorized.Zones && ServerDamageBlockProfiles.DirectionsEqual(requested.Directions, authorized.Directions) && string.Equals(requested.Sound, authorized.Sound, 4) && ServerDamageBlockProfiles.TiersEqual(requested.BlockTier, authorized.BlockTier) && requested.CanBlockProjectiles == authorized.CanBlockProjectiles && requested.StaggerTimeMs == authorized.StaggerTimeMs && requested.StaggerTier == authorized.StaggerTier;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00045DF8 File Offset: 0x00043FF8
		private static bool DirectionsEqual(float[] requested, float[] authorized)
		{
			if (requested.Length != authorized.Length)
			{
				return false;
			}
			for (int index = 0; index < requested.Length; index++)
			{
				if (!float.IsFinite(requested[index]))
				{
					return false;
				}
				if (Math.Abs(requested[index] - authorized[index]) > 0.001f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00045E40 File Offset: 0x00044040
		private static bool DirectionsEffectivelyEqual(float[] current, float[] authorized)
		{
			if (authorized.Length < current.Length)
			{
				return false;
			}
			for (int index = 0; index < current.Length; index++)
			{
				if (!float.IsFinite(authorized[index]))
				{
					return false;
				}
				if (Math.Abs(current[index] - authorized[index]) > 0.001f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00045E88 File Offset: 0x00044088
		[NullableContext(2)]
		private static bool TiersEqual(Dictionary<EnumDamageType, int> requested, Dictionary<EnumDamageType, int> authorized)
		{
			if (requested == null || authorized == null)
			{
				return requested == null && authorized == null;
			}
			if (requested.Count != authorized.Count)
			{
				return false;
			}
			foreach (KeyValuePair<EnumDamageType, int> keyValuePair in authorized)
			{
				EnumDamageType enumDamageType;
				int num;
				keyValuePair.Deconstruct(ref enumDamageType, ref num);
				EnumDamageType damageType = enumDamageType;
				int tier = num;
				int requestedTier;
				if (!requested.TryGetValue(damageType, ref requestedTier) || requestedTier != tier)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0200027C RID: 636
		[Nullable(0)]
		private readonly struct ConfiguredProfile : IEquatable<ServerDamageBlockProfiles.ConfiguredProfile>
		{
			// Token: 0x06001786 RID: 6022 RVA: 0x0006C9B1 File Offset: 0x0006ABB1
			public ConfiguredProfile(DamageBlockJson Block, EnumDamageBlockKind Kind)
			{
				this.Block = Block;
				this.Kind = Kind;
			}

			// Token: 0x17000553 RID: 1363
			// (get) Token: 0x06001787 RID: 6023 RVA: 0x0006C9C1 File Offset: 0x0006ABC1
			// (set) Token: 0x06001788 RID: 6024 RVA: 0x0006C9C9 File Offset: 0x0006ABC9
			public DamageBlockJson Block { get; set; }

			// Token: 0x17000554 RID: 1364
			// (get) Token: 0x06001789 RID: 6025 RVA: 0x0006C9D2 File Offset: 0x0006ABD2
			// (set) Token: 0x0600178A RID: 6026 RVA: 0x0006C9DA File Offset: 0x0006ABDA
			public EnumDamageBlockKind Kind { get; set; }

			// Token: 0x0600178B RID: 6027 RVA: 0x0006C9E4 File Offset: 0x0006ABE4
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ConfiguredProfile");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600178C RID: 6028 RVA: 0x0006CA30 File Offset: 0x0006AC30
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Block = ");
				builder.Append(this.Block);
				builder.Append(", Kind = ");
				builder.Append(this.Kind.ToString());
				return true;
			}

			// Token: 0x0600178D RID: 6029 RVA: 0x0006CA7E File Offset: 0x0006AC7E
			[CompilerGenerated]
			public static bool operator !=(ServerDamageBlockProfiles.ConfiguredProfile left, ServerDamageBlockProfiles.ConfiguredProfile right)
			{
				return !(left == right);
			}

			// Token: 0x0600178E RID: 6030 RVA: 0x0006CA8A File Offset: 0x0006AC8A
			[CompilerGenerated]
			public static bool operator ==(ServerDamageBlockProfiles.ConfiguredProfile left, ServerDamageBlockProfiles.ConfiguredProfile right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600178F RID: 6031 RVA: 0x0006CA94 File Offset: 0x0006AC94
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<DamageBlockJson>.Default.GetHashCode(this.<Block>k__BackingField) * -1521134295 + EqualityComparer<EnumDamageBlockKind>.Default.GetHashCode(this.<Kind>k__BackingField);
			}

			// Token: 0x06001790 RID: 6032 RVA: 0x0006CABD File Offset: 0x0006ACBD
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is ServerDamageBlockProfiles.ConfiguredProfile && this.Equals((ServerDamageBlockProfiles.ConfiguredProfile)obj);
			}

			// Token: 0x06001791 RID: 6033 RVA: 0x0006CAD5 File Offset: 0x0006ACD5
			[CompilerGenerated]
			public bool Equals(ServerDamageBlockProfiles.ConfiguredProfile other)
			{
				return EqualityComparer<DamageBlockJson>.Default.Equals(this.<Block>k__BackingField, other.<Block>k__BackingField) && EqualityComparer<EnumDamageBlockKind>.Default.Equals(this.<Kind>k__BackingField, other.<Kind>k__BackingField);
			}

			// Token: 0x06001792 RID: 6034 RVA: 0x0006CB07 File Offset: 0x0006AD07
			[CompilerGenerated]
			public void Deconstruct(out DamageBlockJson Block, out EnumDamageBlockKind Kind)
			{
				Block = this.Block;
				Kind = this.Kind;
			}
		}
	}
}
