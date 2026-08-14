using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VSEssentialsMod.Entity.AI.Task;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class AiTaskBaseTargetableConfig : AiTaskBaseConfig
{
	[JsonProperty]
	public EnumEntitySearchType SearchType;

	[JsonProperty]
	public float SeekingRange = 25f;

	[JsonProperty]
	public float MinTargetWeight;

	[JsonProperty]
	public float MaxTargetWeight;

	[JsonProperty]
	public ComplexTagCondition<TagSetFast> EntityTags;

	[JsonProperty]
	private string[]? entityCodes = Array.Empty<string>();

	[JsonProperty]
	public AssetLocation[] SkipEntityCodes = Array.Empty<AssetLocation>();

	[JsonProperty]
	public float TamingGenerations = 10f;

	[JsonProperty]
	public bool UseFearReductionFactor;

	[JsonProperty]
	public bool SeekingRangeAffectedByPlayerStat = true;

	[JsonProperty]
	public float SneakRangeReduction = 1f;

	[JsonProperty]
	public string? TriggerEmotionState;

	[JsonProperty]
	public EnumCreatureHostility CreatureHostility;

	[JsonProperty]
	public bool IgnoreDeadEntities = true;

	[JsonProperty]
	public bool FriendlyTarget;

	[JsonProperty]
	public bool RetaliateAttacks = true;

	[JsonProperty]
	public bool TargetEntitiesWithSameHerdId;

	[JsonProperty]
	public bool TargetEntitiesWithDifferentHerdId;

	[JsonProperty]
	public bool TargetOnlyInteractableEntities = true;

	[JsonProperty]
	public int[] TargetLightLevels = new int[4] { 0, 0, 32, 32 };

	[JsonProperty]
	public EnumLightLevelType TargetLightLevelType = EnumLightLevelType.MaxTimeOfDayLight;

	[JsonProperty]
	public int TargetSearchCooldownMs = 2000;

	[JsonProperty]
	public bool TargetPlayerInAllGameModes;

	public string[] TargetEntityCodesBeginsWith = Array.Empty<string>();

	public string[] TargetEntityCodesExact = Array.Empty<string>();

	public string TargetEntityFirstLetters = "";

	public bool NoEntityCodes
	{
		get
		{
			if (TargetEntityCodesExact.Length == 0)
			{
				return TargetEntityCodesBeginsWith.Length == 0;
			}
			return false;
		}
	}

	public bool TargetEverything
	{
		get
		{
			if (NoEntityCodes && EntityTags.IsEmpty)
			{
				return NoEntityWeight;
			}
			return false;
		}
	}

	public bool NoEntityWeight
	{
		get
		{
			if (MaxTargetWeight <= 0f)
			{
				return MinTargetWeight <= 0f;
			}
			return false;
		}
	}

	public bool IgnoreTargetLightLevel
	{
		get
		{
			if (TargetLightLevels[0] == 0 && TargetLightLevels[1] == 0 && TargetLightLevels[2] == 32)
			{
				return TargetLightLevels[3] == 32;
			}
			return false;
		}
	}

	public override void Init(EntityAgent entity)
	{
		base.Init(entity);
		if (entityCodes != null)
		{
			InitializeTargetCodes(entityCodes, out TargetEntityCodesExact, out TargetEntityCodesBeginsWith, out TargetEntityFirstLetters);
			entityCodes = null;
		}
		if (TargetLightLevels.Length != 4)
		{
			entity.Api.Logger.Error($"Invalid 'targetLightLevels' value (array length: {TargetLightLevels.Length}, should be 4) in AI task '{Code}' for entity '{entity.Code}'");
			throw new ArgumentException($"Invalid 'targetLightLevels' value (array length: {TargetLightLevels.Length}, should be 4) in AI task '{Code}' for entity '{entity.Code}'");
		}
		if (TargetLightLevels[0] > TargetLightLevels[1] || TargetLightLevels[1] > TargetLightLevels[2] || TargetLightLevels[2] > TargetLightLevels[3])
		{
			entity.Api.Logger.Error($"Invalid 'targetLightLevels' value: [{TargetLightLevels[0]},{TargetLightLevels[1]},{TargetLightLevels[2]},{TargetLightLevels[3]}], in AI task '{Code}' for entity '{entity.Code}'");
			throw new ArgumentException($"Invalid 'targetLightLevels' value: [{TargetLightLevels[0]},{TargetLightLevels[1]},{TargetLightLevels[2]},{TargetLightLevels[3]}], in AI task '{Code}' for entity '{entity.Code}'");
		}
	}

	protected static void InitializeTargetCodes(string[] codes, out string[] targetEntityCodesExact, out string[] targetEntityCodesBeginsWith, out string targetEntityFirstLetters)
	{
		List<string> targetEntityCodesList = new List<string>();
		List<string> beginsWith = new List<string>();
		targetEntityFirstLetters = "";
		foreach (string code in codes)
		{
			if (code.EndsWith('*'))
			{
				string text = code;
				beginsWith.Add(text.Substring(0, text.Length - 1));
			}
			else
			{
				targetEntityCodesList.Add(code);
			}
		}
		targetEntityCodesBeginsWith = beginsWith.ToArray();
		targetEntityCodesExact = new string[targetEntityCodesList.Count];
		int exactEntityCodeIndex = 0;
		foreach (string code2 in targetEntityCodesList)
		{
			if (code2.Length != 0)
			{
				targetEntityCodesExact[exactEntityCodeIndex++] = code2;
				char firstLetter = code2[0];
				if (targetEntityFirstLetters.IndexOf(firstLetter) < 0)
				{
					targetEntityFirstLetters += firstLetter;
				}
			}
		}
		string[] array = targetEntityCodesBeginsWith;
		foreach (string code3 in array)
		{
			if (code3.Length == 0)
			{
				targetEntityFirstLetters = "";
				break;
			}
			char firstLetter2 = code3[0];
			if (targetEntityFirstLetters.IndexOf(firstLetter2) < 0)
			{
				targetEntityFirstLetters += firstLetter2;
			}
		}
	}
}
