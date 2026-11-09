using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.Systems;

public class ModSystemTfEditFromAttributes : ModSystem
{
	private ICoreClientAPI capi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return true;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Event.RegisterEventBusListener(OnEventBusEvent);
	}

	private void OnEventBusEvent(string eventName, ref EnumHandling handling, IAttribute data)
	{
		TreeAttribute tree = data as TreeAttribute;
		if (!(eventName == "onsettransform") && !(eventName == "ongettransform"))
		{
			return;
		}
		ItemStack itemstack = capi.World.Player.InventoryManager.ActiveHotbarSlot.Itemstack;
		if (itemstack == null)
		{
			return;
		}
		JsonObject metadata = itemstack.ItemAttributes?["transformsFromAttributes"];
		if (metadata == null || !metadata.Exists)
		{
			return;
		}
		string code = itemstack.Attributes.GetString(metadata["typeCode"].AsString());
		if (code == null)
		{
			return;
		}
		string target = tree.GetString("target");
		JsonObject types = metadata["types"];
		if (eventName == "ongettransform")
		{
			ModelTransform tf = types[code][target].AsObject<ModelTransform>();
			if (tf != null)
			{
				tf.ToTreeAttribute(tree);
				tree.SetBool("preventDefault", value: true);
			}
			return;
		}
		if (!types[code].Exists)
		{
			types.Token[code] = new JObject();
		}
		if (!types[code][target].Exists)
		{
			types[code].Token[target] = new JObject();
		}
		types[code].Token[target] = JToken.FromObject(ModelTransform.CreateFromTreeAttribute(tree));
		tree.SetBool("preventDefault", value: true);
	}
}
