using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace Vintagestory.ServerMods;

public class ModSystemWorldGenStructures : ModSystem
{
	protected IStructureProvider[] providers;

	public event Action<List<GeneratedStructure>> OnStructuresDeleted;

	public event Action<int, int> OnRegenFinalized;

	public void TriggerStructuresDeleted(List<GeneratedStructure> structures)
	{
		this.OnStructuresDeleted?.Invoke(structures);
	}

	public void TriggerRegenFinalized(int chunkX, int chunkZ)
	{
		this.OnRegenFinalized?.Invoke(chunkX, chunkZ);
	}

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void StartPre(ICoreAPI api)
	{
		providers = Array.Empty<IStructureProvider>();
	}

	public void RegisterProvider(IStructureProvider provider)
	{
		providers = providers.Append(provider);
	}

	public IStructureLocation GetIntersectingStructure(int x, int z, int category)
	{
		for (int i = 0; i < providers.Length; i++)
		{
			IStructureLocation structure = providers[i].GetBlockingStructureAt(x, z, category);
			if (structure != null)
			{
				return structure;
			}
		}
		return null;
	}
}
