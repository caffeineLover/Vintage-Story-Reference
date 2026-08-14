using System.IO;
using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BarrelRecipeIngredient : CraftingRecipeIngredient
{
	[DocumentAsJson("Optional", "Consume All", false)]
	public int? ConsumeQuantity;

	[DocumentAsJson("Optional", "None", false)]
	public float Litres = -1f;

	[DocumentAsJson("Optional", "Consume All", false)]
	public float? ConsumeLitres;

	public override BarrelRecipeIngredient Clone()
	{
		BarrelRecipeIngredient result = new BarrelRecipeIngredient();
		CloneTo(result);
		return result;
	}

	public override void FromBytes(BinaryReader reader, IWorldAccessor resolver)
	{
		base.FromBytes(reader, resolver);
		if (reader.ReadBoolean())
		{
			ConsumeQuantity = reader.ReadInt32();
		}
		else
		{
			ConsumeQuantity = null;
		}
		if (reader.ReadBoolean())
		{
			ConsumeLitres = reader.ReadSingle();
		}
		else
		{
			ConsumeLitres = null;
		}
		Litres = reader.ReadSingle();
	}

	public override void ToBytes(BinaryWriter writer)
	{
		base.ToBytes(writer);
		if (ConsumeQuantity.HasValue)
		{
			writer.Write(value: true);
			writer.Write(ConsumeQuantity.Value);
		}
		else
		{
			writer.Write(value: false);
		}
		if (ConsumeLitres.HasValue)
		{
			writer.Write(value: true);
			writer.Write(ConsumeLitres.Value);
		}
		else
		{
			writer.Write(value: false);
		}
		writer.Write(Litres);
	}

	public override bool Resolve(IWorldAccessor world, string sourceForErrorLogging)
	{
		if (!base.Resolve(world, sourceForErrorLogging))
		{
			return false;
		}
		ResolveLiquidProperties(world, sourceForErrorLogging);
		return true;
	}

	protected virtual void ResolveLiquidProperties(IWorldAccessor world, string sourceForErrorLogging)
	{
		WaterTightContainableProps liquidProperties = BlockLiquidContainerBase.GetContainableProps(base.ResolvedItemStack);
		if (liquidProperties == null)
		{
			return;
		}
		if (Litres < 0f)
		{
			if (base.Quantity > 0)
			{
				world.Logger.Warning($"({sourceForErrorLogging}) Barrel recipe ingredient '{base.Code}' does not define a litres attribute but a quantity, will assume quantity=litres for backwards compatibility.");
				Litres = base.Quantity;
				ConsumeLitres = ConsumeQuantity;
			}
			else
			{
				Litres = 1f;
			}
		}
		base.Quantity = (int)(liquidProperties.ItemsPerLitre * Litres);
		if (ConsumeLitres.HasValue)
		{
			ConsumeQuantity = (int)(liquidProperties.ItemsPerLitre * ConsumeLitres).Value;
		}
	}

	protected override void CloneTo(object cloneTo)
	{
		base.CloneTo(cloneTo);
		if (cloneTo is BarrelRecipeIngredient ingredient)
		{
			ingredient.ConsumeQuantity = ConsumeQuantity;
			ingredient.Litres = Litres;
			ingredient.ConsumeLitres = ConsumeLitres;
		}
	}
}
