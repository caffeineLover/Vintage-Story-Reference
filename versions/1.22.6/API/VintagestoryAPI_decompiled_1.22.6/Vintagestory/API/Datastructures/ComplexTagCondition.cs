using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;

namespace Vintagestory.API.Datastructures;

/// <summary>
/// Holds a complex tag condition, either as a set of disjunctive entries ([a and b and c] or [d and e])
/// or as a set of conjunctive entries ([a or b or c] and [d or e]).<br />
/// In both cases each of the inner sets can also have a set of forbidden tags that is always disjunctive.<br />
/// <br />
/// The syntax for using these in json follows one simple rule: If no junction verb is specified, the group is treated as conjunctive (a and b and c)<br />
/// ["all", "of", "these"]<br />
/// [["all", "of", "these"]]<br />
/// [["all", "of", "these"], (or) { allOf: ["all", "of", "these"], noneOf: ["any", "of", "these"] }]<br />
/// { allOf: ["all", "of", "these"], noneOf: ["any", "of", "these"] }<br />
/// { anyOf: ["any", "of", "these"], noneOf: ["any", "of", "these"] }<br />
/// { anyOf: [["all", "of", "these"], { allOf: ["all", "of", "these"], noneOf: ["any", "of", "these"] }] }<br />
/// { allOf: [["anyOf", "of", "these"], (and) { anyOf: ["any", "of", "these"], noneOf: ["any", "of", "these"] }] }<br />
/// </summary>
/// <remarks> You cannot mix junction verbs on the same layer. </remarks>
[DocumentAsJson]
[JsonConverter(typeof(GenericComplexConditionConverter))]
public struct ComplexTagCondition<TTagSet> : IEquatable<ComplexTagCondition<TTagSet>> where TTagSet : IEquatable<TTagSet>
{
	public struct Condition
	{
		public TTagSet RequiredTags;

		public TTagSet ForbiddenTags;

		public readonly bool Equals(Condition other)
		{
			if (RequiredTags != null && other.RequiredTags != null)
			{
				if (!RequiredTags.Equals(other.RequiredTags))
				{
					return false;
				}
			}
			else if (RequiredTags != null || other.RequiredTags != null)
			{
				return false;
			}
			if (ForbiddenTags != null && other.ForbiddenTags != null)
			{
				if (!ForbiddenTags.Equals(other.ForbiddenTags))
				{
					return false;
				}
			}
			else if (ForbiddenTags != null || other.ForbiddenTags != null)
			{
				return false;
			}
			return true;
		}

		public override readonly string ToString()
		{
			return new StringBuilder(64).Append("Required: ").Append(RequiredTags).Append(", Forbidden: ")
				.Append(ForbiddenTags)
				.ToString();
		}
	}

	public Condition[]? conditions;

	public bool isDisjunctive;

	[MemberNotNullWhen(false, "conditions")]
	public readonly bool IsEmpty
	{
		[MemberNotNullWhen(false, "conditions")]
		get
		{
			if (conditions != null)
			{
				return conditions.Length == 0;
			}
			return true;
		}
	}

	public override readonly int GetHashCode()
	{
		int hash = (isDisjunctive ? 1193046 : 6592625);
		if (conditions != null)
		{
			Condition[] array = conditions;
			for (int i = 0; i < array.Length; i++)
			{
				Condition condition = array[i];
				hash ^= condition.GetHashCode();
			}
		}
		return hash;
	}

	public static bool operator ==(in ComplexTagCondition<TTagSet> self, in ComplexTagCondition<TTagSet> other)
	{
		return self.Equals(other);
	}

	public static bool operator !=(in ComplexTagCondition<TTagSet> self, in ComplexTagCondition<TTagSet> other)
	{
		return !self.Equals(other);
	}

	public override readonly bool Equals([NotNullWhen(true)] object? obj)
	{
		if (obj is ComplexTagCondition<TTagSet> other)
		{
			return Equals(other);
		}
		return false;
	}

	public readonly bool Equals(ComplexTagCondition<TTagSet> other)
	{
		if (conditions == null || other.conditions == null)
		{
			if (conditions == null)
			{
				return other.conditions == null;
			}
			return false;
		}
		if (conditions.Length != other.conditions.Length)
		{
			return false;
		}
		if (isDisjunctive != other.isDisjunctive)
		{
			return false;
		}
		for (int i = 0; i < conditions.Length; i++)
		{
			if (!conditions[i].Equals(other.conditions[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override readonly string ToString()
	{
		if (IsEmpty)
		{
			return "empty";
		}
		return new StringBuilder(128).AppendJoin(";", conditions).ToString();
	}
}
