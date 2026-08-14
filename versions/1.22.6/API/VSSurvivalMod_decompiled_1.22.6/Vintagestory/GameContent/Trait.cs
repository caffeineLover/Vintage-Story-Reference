using System.Collections.Generic;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class Trait
{
	public string Code;

	public EnumTraitType Type;

	public Dictionary<string, double> Attributes;

	public Dictionary<string, EnumStatBlendType> AttributeBlendTypes;
}
