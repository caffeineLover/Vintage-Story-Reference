using System;
using System.IO;
using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BarrelOutputStack : JsonItemStack, IConcreteCloneable<BarrelOutputStack>, ICloneable
{
	[DocumentAsJson("Optional", "0", false)]
	public float Litres;

	public override void FromBytes(BinaryReader reader, IClassRegistryAPI instancer)
	{
		base.FromBytes(reader, instancer);
		Litres = reader.ReadSingle();
	}

	public override void ToBytes(BinaryWriter writer)
	{
		base.ToBytes(writer);
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

	public override BarrelOutputStack Clone()
	{
		BarrelOutputStack result = new BarrelOutputStack();
		CloneTo(result);
		return result;
	}

	protected override void CloneTo(object stack)
	{
		base.CloneTo(stack);
		if (stack is BarrelOutputStack barrelOutput)
		{
			barrelOutput.Litres = Litres;
		}
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
				world.Logger.Warning($"({sourceForErrorLogging}) Barrel recipe output {Code} does not define a litres attribute but a stacksize, will assume stacksize=litres for backwards compatibility.");
				Litres = base.Quantity;
			}
			else
			{
				Litres = 1f;
			}
		}
		base.Quantity = (int)(liquidProperties.ItemsPerLitre * Litres);
	}
}
