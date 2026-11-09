using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BarrelRecipe : RecipeBase, IByteSerializable, IConcreteCloneable<BarrelRecipe>, ICloneable
{
	[DocumentAsJson("Required", "", false)]
	public BarrelOutputStack? Output { get; set; }

	[DocumentAsJson("Required", "", false)]
	public double SealHours { get; set; }

	[DocumentAsJson("Required", "", false)]
	public string? Code { get; set; }

	[DocumentAsJson("Required", "", false)]
	public BarrelRecipeIngredient[]? Ingredients { get; set; }

	public override IEnumerable<IRecipeIngredient> RecipeIngredients => Ingredients ?? throw new InvalidOperationException($"Barrel recipe '{base.Name}' does not have ingredients specified");

	public override IRecipeOutput RecipeOutput => Output ?? throw new InvalidOperationException($"Barrel recipe '{base.Name}' does not have output specified");

	public override void OnParsed(IWorldAccessor world)
	{
		if (Ingredients == null)
		{
			return;
		}
		int ingredientIndex = 1;
		BarrelRecipeIngredient[] ingredients = Ingredients;
		foreach (CraftingRecipeIngredient craftingRecipeIngredient in ingredients)
		{
			if (craftingRecipeIngredient.Id == null)
			{
				string text = (craftingRecipeIngredient.Id = ingredientIndex++.ToString());
			}
		}
	}

	public bool Matches(ItemSlot[] inputSlots, out int outputStackSize)
	{
		outputStackSize = 0;
		List<(ItemSlot, BarrelRecipeIngredient)> matched = PairInput(inputSlots);
		if (matched.Count == 0)
		{
			return false;
		}
		outputStackSize = GetOutputSize(matched);
		return outputStackSize >= 0;
	}

	public bool Matches(IPlayer forPlayer, ItemSlot[] inputSlots, out int outputStackSize)
	{
		outputStackSize = 0;
		if (!forPlayer.Entity.Api.Event.TriggerMatchesRecipe(forPlayer, this, inputSlots))
		{
			return false;
		}
		return Matches(inputSlots, out outputStackSize);
	}

	public bool TryCraftNow(ICoreAPI api, double nowSealedHours, ItemSlot[] inputSlots)
	{
		if (SealHours > 0.0 && nowSealedHours < SealHours)
		{
			return false;
		}
		List<(ItemSlot, BarrelRecipeIngredient)> matched = PairInput(inputSlots);
		int outputStackSize = GetOutputSize(matched);
		if (outputStackSize < 0 || Output?.ResolvedItemStack == null)
		{
			return false;
		}
		ItemStack mixedStack = Output.ResolvedItemStack.Clone();
		mixedStack.StackSize = outputStackSize;
		CarryOverFreshness(api, mixedStack, inputSlots);
		ItemStack remainStack = null;
		foreach (var (slot, ingredient) in matched)
		{
			if (ingredient.ConsumeQuantity.HasValue && slot.Itemstack != null)
			{
				remainStack = slot.Itemstack;
				remainStack.StackSize -= ingredient.ConsumeQuantity.Value * (mixedStack.StackSize / Output.StackSize);
				if (remainStack.StackSize <= 0)
				{
					remainStack = null;
				}
				break;
			}
		}
		ItemSlot inputItemSlot = inputSlots[0];
		ItemSlot liquidSlot = inputSlots[1];
		if (ShouldBeInLiquidSlot(mixedStack))
		{
			inputItemSlot.Itemstack = remainStack;
			liquidSlot.Itemstack = mixedStack;
		}
		else
		{
			liquidSlot.Itemstack = remainStack;
			inputItemSlot.Itemstack = mixedStack;
		}
		inputItemSlot.MarkDirty();
		liquidSlot.MarkDirty();
		return true;
	}

	public override void ToBytes(BinaryWriter writer)
	{
		base.ToBytes(writer);
		if (Code == null || Ingredients == null || Output == null)
		{
			throw new InvalidOperationException("Cannot serialize barrel recipes: some of the properties are null");
		}
		writer.Write(Code);
		writer.Write(Ingredients.Length);
		for (int i = 0; i < Ingredients.Length; i++)
		{
			Ingredients[i].ToBytes(writer);
		}
		Output.ToBytes(writer);
		writer.Write(SealHours);
	}

	public override void FromBytes(BinaryReader reader, IWorldAccessor resolver)
	{
		base.FromBytes(reader, resolver);
		Code = reader.ReadString();
		BarrelRecipeIngredient[] Ingredients = (this.Ingredients = new BarrelRecipeIngredient[reader.ReadInt32()]);
		for (int i = 0; i < Ingredients.Length; i++)
		{
			BarrelRecipeIngredient ingredient = new BarrelRecipeIngredient();
			ingredient.FromBytes(reader, resolver);
			ingredient.Resolve(resolver, "Barrel Recipe (FromBytes)", this);
			Ingredients[i] = ingredient;
		}
		Output = new BarrelOutputStack();
		Output.FromBytes(reader, resolver.ClassRegistry);
		Output.Resolve(resolver, "Barrel Recipe (FromBytes)");
		SealHours = reader.ReadDouble();
	}

	public override bool Resolve(IWorldAccessor world, string sourceForErrorLogging)
	{
		bool resolved = true;
		if (Ingredients == null || Output == null)
		{
			world.Logger.Error($"Cannot resolve barrel recipe '{base.Name}', either Ingredients or Output are not specified");
			return false;
		}
		BarrelRecipeIngredient[] ingredients = Ingredients;
		foreach (BarrelRecipeIngredient ingredient in ingredients)
		{
			resolved &= ingredient.Resolve(world, sourceForErrorLogging, this);
		}
		return resolved & Output.Resolve(world, sourceForErrorLogging);
	}

	public override BarrelRecipe Clone()
	{
		BarrelRecipe recipe = new BarrelRecipe();
		CloneTo(recipe);
		return recipe;
	}

	protected override void CloneTo(object recipe)
	{
		base.CloneTo(recipe);
		if (!(recipe is BarrelRecipe barrelRecipe))
		{
			throw new ArgumentException("CloneTo should take object of same class or it subclass");
		}
		barrelRecipe.Output = Output?.Clone();
		barrelRecipe.SealHours = SealHours;
		barrelRecipe.Code = Code;
		if (Ingredients != null)
		{
			barrelRecipe.Ingredients = new BarrelRecipeIngredient[Ingredients.Length];
			for (int i = 0; i < Ingredients.Length; i++)
			{
				barrelRecipe.Ingredients[i] = Ingredients[i].Clone();
			}
		}
	}

	protected virtual bool ShouldBeInLiquidSlot(ItemStack? stack)
	{
		if (stack == null)
		{
			return false;
		}
		return stack.ItemAttributes?["waterTightContainerProps"].Exists == true;
	}

	protected virtual List<(ItemSlot slot, BarrelRecipeIngredient ingredient)> PairInput(ItemSlot[] inputSlots)
	{
		int stackCount = 0;
		ItemSlot[] array = inputSlots;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Empty)
			{
				stackCount++;
			}
		}
		if (Ingredients == null || stackCount != Ingredients.Length)
		{
			return new List<(ItemSlot, BarrelRecipeIngredient)>();
		}
		List<(ItemSlot, BarrelRecipeIngredient)> matched = new List<(ItemSlot, BarrelRecipeIngredient)>();
		List<BarrelRecipeIngredient> ingredientsToProcess = Ingredients.ToList();
		array = inputSlots;
		foreach (ItemSlot inputSlot in array)
		{
			if (inputSlot.Itemstack != null)
			{
				BarrelRecipeIngredient ingredient = ingredientsToProcess.Find((BarrelRecipeIngredient ingredient2) => MatchStackToIngredient(inputSlot.Itemstack, ingredient2));
				if (ingredient == null)
				{
					return new List<(ItemSlot, BarrelRecipeIngredient)>();
				}
				matched.Add((inputSlot, ingredient));
				ingredientsToProcess.Remove(ingredient);
			}
		}
		if (matched.Count < Ingredients.Length)
		{
			return new List<(ItemSlot, BarrelRecipeIngredient)>();
		}
		return matched;
	}

	protected virtual int GetOutputSize(List<(ItemSlot slot, BarrelRecipeIngredient ingredient)> matched)
	{
		int outQuantityMul = -1;
		foreach (var (slot, ingredient) in matched)
		{
			if (!ingredient.ConsumeQuantity.HasValue)
			{
				outQuantityMul = slot.StackSize / ingredient.Quantity;
			}
		}
		if (outQuantityMul == -1)
		{
			return -1;
		}
		foreach (var (slot2, ingredient2) in matched)
		{
			if (!ingredient2.ConsumeQuantity.HasValue)
			{
				if (slot2.StackSize % ingredient2.Quantity != 0)
				{
					return -1;
				}
				if (outQuantityMul != slot2.StackSize / ingredient2.Quantity)
				{
					return -1;
				}
			}
			else if (slot2.StackSize < ingredient2.Quantity * outQuantityMul)
			{
				return -1;
			}
		}
		return Output.StackSize * outQuantityMul;
	}

	protected virtual void CarryOverFreshness(ICoreAPI api, ItemStack mixedStack, ItemSlot[] inputSlots)
	{
		TransitionableProperties perishProps = mixedStack.Collectible.GetTransitionableProperties(api.World, mixedStack, null)?.FirstOrDefault((TransitionableProperties p) => p.Type == EnumTransitionType.Perish);
		if (perishProps != null)
		{
			CollectibleObject.CarryOverFreshness(api, inputSlots, new ItemStack[1] { mixedStack }, perishProps);
		}
	}
}
