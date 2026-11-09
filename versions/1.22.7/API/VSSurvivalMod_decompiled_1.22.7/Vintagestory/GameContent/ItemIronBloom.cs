using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class ItemIronBloom : Item, IAnvilWorkable
{
	public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
	{
		base.OnBeforeRender(capi, itemstack, target, ref renderinfo);
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		if (itemStack.Attributes.HasAttribute("voxels"))
		{
			return Lang.Get("Partially worked iron bloom");
		}
		return base.GetHeldItemName(itemStack);
	}

	public MeshData GenMesh(ICoreClientAPI capi, ItemStack stack)
	{
		return null;
	}

	public int GetWorkItemHashCode(ItemStack stack)
	{
		return stack.Attributes.GetHashCode();
	}

	public int GetRequiredAnvilTier(ItemStack stack)
	{
		return 2;
	}

	public List<SmithingRecipe> GetMatchingRecipes(ItemStack stack)
	{
		return (from r in api.GetSmithingRecipes()
			where r.Ingredient.SatisfiesAsIngredient(stack)
			orderby r.Output.ResolvedItemstack.Collectible.Code
			select r).ToList();
	}

	public bool CanWork(ItemStack stack)
	{
		float temperature = stack.Collectible.GetTemperature(api.World, stack);
		float meltingpoint = stack.Collectible.GetMeltingPoint(api.World, null, new DummySlot(stack));
		JsonObject attributes = stack.Collectible.Attributes;
		if (attributes != null && attributes["workableTemperature"].Exists)
		{
			return stack.Collectible.Attributes["workableTemperature"].AsFloat(meltingpoint / 2f) <= temperature;
		}
		return temperature >= meltingpoint / 2f;
	}

	public ItemStack TryPlaceOn(ItemStack stack, BlockEntityAnvil beAnvil)
	{
		if (beAnvil.WorkItemStack != null)
		{
			return null;
		}
		if (stack.Attributes.HasAttribute("voxels"))
		{
			try
			{
				beAnvil.Voxels = BlockEntityAnvil.deserializeVoxels(stack.Attributes.GetBytes("voxels"));
				beAnvil.SelectedRecipeId = stack.Attributes.GetInt("selectedRecipeId");
			}
			catch (Exception)
			{
				CreateVoxelsFromIronBloom(ref beAnvil.Voxels);
			}
		}
		else
		{
			CreateVoxelsFromIronBloom(ref beAnvil.Voxels);
		}
		ItemStack itemStack = stack.Clone();
		itemStack.StackSize = 1;
		return itemStack.Clone();
	}

	public virtual int VoxelCountForHandbook(ItemStack stack)
	{
		return 42;
	}

	private void CreateVoxelsFromIronBloom(ref byte[,,] voxels)
	{
		ItemIngot.CreateVoxelsFromIngot(api, ref voxels);
		Random rand = api.World.Rand;
		int length = 9;
		int height = 5;
		int width = 6;
		int metalVoxels = 0;
		for (int x = 3; x < 12; x++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int z = 5; z < 11; z++)
				{
					if (y == 0 && voxels[x, y, z] == 1)
					{
						metalVoxels++;
						continue;
					}
					float dist = (float)(Math.Max(0, Math.Abs(x - 7) - 1) + Math.Max(0, Math.Abs(z - 8) - 1)) + Math.Max(0f, (float)y - 1f);
					if (!(rand.NextDouble() < (double)(dist / 3f - 0.4f + ((float)y - 1.5f) / 4f)))
					{
						if (rand.NextDouble() > (double)(dist / 2f))
						{
							voxels[x, y, z] = 1;
						}
						else
						{
							voxels[x, y, z] = 2;
						}
						if (voxels[x, y, z] == 1)
						{
							metalVoxels++;
						}
					}
				}
			}
		}
		int tries = 0;
		while (metalVoxels < ItemIngot.VoxelCount && tries < 1000)
		{
			int num = rand.Next(length * width);
			int x2 = num / width;
			int z2 = num % width;
			x2 += 3;
			z2 += 5;
			if (voxels[x2, height, z2] != 1)
			{
				for (int y2 = height; y2 > 0; y2--)
				{
					voxels[x2, y2, z2] = voxels[x2, y2 - 1, z2];
				}
				voxels[x2, 0, z2] = 1;
				metalVoxels++;
			}
			tries++;
		}
	}

	public ItemStack GetBaseMaterial(ItemStack stack)
	{
		return stack;
	}

	public EnumHelveWorkableMode GetHelveWorkableMode(ItemStack stack, BlockEntityAnvil beAnvil)
	{
		return EnumHelveWorkableMode.FullyWorkable;
	}
}
