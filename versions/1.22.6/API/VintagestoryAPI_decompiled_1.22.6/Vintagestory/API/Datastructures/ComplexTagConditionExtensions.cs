using System.IO;

namespace Vintagestory.API.Datastructures;

public static class ComplexTagConditionExtensions
{
	public static bool Matches(this in ComplexTagCondition<TagSet> complexCondition, in TagSet matchAgainst)
	{
		if (complexCondition.conditions == null)
		{
			return true;
		}
		ComplexTagCondition<TagSet>.Condition[] conditions;
		if (complexCondition.isDisjunctive)
		{
			conditions = complexCondition.conditions;
			for (int i = 0; i < conditions.Length; i++)
			{
				ComplexTagCondition<TagSet>.Condition condition = conditions[i];
				if (!condition.ForbiddenTags.Overlaps(in matchAgainst) && condition.RequiredTags.IsFullyContainedIn(in matchAgainst))
				{
					return true;
				}
			}
			return false;
		}
		conditions = complexCondition.conditions;
		for (int i = 0; i < conditions.Length; i++)
		{
			ComplexTagCondition<TagSet>.Condition condition2 = conditions[i];
			if (condition2.ForbiddenTags.Overlaps(in matchAgainst) || !condition2.RequiredTags.Overlaps(in matchAgainst))
			{
				return false;
			}
		}
		return true;
	}

	public static bool Matches(this in ComplexTagCondition<TagSetFast> complexCondition, in TagSetFast matchAgainst)
	{
		if (complexCondition.conditions == null)
		{
			return true;
		}
		ComplexTagCondition<TagSetFast>.Condition[] conditions;
		if (complexCondition.isDisjunctive)
		{
			conditions = complexCondition.conditions;
			for (int i = 0; i < conditions.Length; i++)
			{
				ComplexTagCondition<TagSetFast>.Condition condition = conditions[i];
				if ((((matchAgainst & condition.RequiredTags) ^ condition.RequiredTags) | (matchAgainst & condition.ForbiddenTags)).IsEmpty)
				{
					return true;
				}
			}
			return false;
		}
		conditions = complexCondition.conditions;
		for (int i = 0; i < conditions.Length; i++)
		{
			ComplexTagCondition<TagSetFast>.Condition condition2 = conditions[i];
			if (!condition2.RequiredTags.Overlaps(in matchAgainst) || condition2.ForbiddenTags.Overlaps(in matchAgainst))
			{
				return false;
			}
		}
		return true;
	}

	public static void ToBytes(this in ComplexTagCondition<TagSetFast> complexCondition, BinaryWriter writer)
	{
		if (complexCondition.conditions == null)
		{
			writer.Write((ushort)0);
			return;
		}
		writer.Write((ushort)complexCondition.conditions.Length);
		writer.Write(complexCondition.isDisjunctive);
		ComplexTagCondition<TagSetFast>.Condition[] conditions = complexCondition.conditions;
		for (int i = 0; i < conditions.Length; i++)
		{
			ComplexTagCondition<TagSetFast>.Condition condition = conditions[i];
			condition.RequiredTags.ToBytes(writer);
			condition.ForbiddenTags.ToBytes(writer);
		}
	}

	public static ComplexTagCondition<TagSetFast> FastFromBytes(BinaryReader reader)
	{
		ushort length = reader.ReadUInt16();
		if (length == 0)
		{
			return default(ComplexTagCondition<TagSetFast>);
		}
		ComplexTagCondition<TagSetFast> result = new ComplexTagCondition<TagSetFast>
		{
			conditions = new ComplexTagCondition<TagSetFast>.Condition[length],
			isDisjunctive = reader.ReadBoolean()
		};
		for (ushort i = 0; i < length; i++)
		{
			result.conditions[i].RequiredTags = TagSetFast.FromBytes(reader);
			result.conditions[i].ForbiddenTags = TagSetFast.FromBytes(reader);
		}
		return result;
	}

	public static void ToBytes(this in ComplexTagCondition<TagSet> complexCondition, BinaryWriter writer)
	{
		if (complexCondition.conditions == null)
		{
			writer.Write((ushort)0);
			return;
		}
		writer.Write((ushort)complexCondition.conditions.Length);
		writer.Write(complexCondition.isDisjunctive);
		ComplexTagCondition<TagSet>.Condition[] conditions = complexCondition.conditions;
		for (int i = 0; i < conditions.Length; i++)
		{
			ComplexTagCondition<TagSet>.Condition condition = conditions[i];
			condition.RequiredTags.ToBytes(writer);
			condition.ForbiddenTags.ToBytes(writer);
		}
	}

	public static ComplexTagCondition<TagSet> FromBytes(BinaryReader reader)
	{
		ushort length = reader.ReadUInt16();
		if (length == 0)
		{
			return default(ComplexTagCondition<TagSet>);
		}
		ComplexTagCondition<TagSet> result = new ComplexTagCondition<TagSet>
		{
			conditions = new ComplexTagCondition<TagSet>.Condition[length],
			isDisjunctive = reader.ReadBoolean()
		};
		for (ushort i = 0; i < length; i++)
		{
			result.conditions[i].RequiredTags = TagSet.FromBytes(reader);
			result.conditions[i].ForbiddenTags = TagSet.FromBytes(reader);
		}
		return result;
	}
}
