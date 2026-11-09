using System.IO;

namespace Vintagestory.API.Common;

/// <summary>
/// Allows you to add a list of item stacks to put various into creative menu tabs.
/// </summary>
/// <example>
/// <code language="json">
/// <!--Sorry for the long example.-->
///             "creativeinventoryStacksByType": {
///             	"*-fired": [
///             		{
///             			"tabs": [ "general", "decorative" ],
///             			"stacks": [
///             				{
///             					"type": "block",
///             					"code": "bowl-fired",
///             					"attributes": {
///             						"ucontents": [
///             							{
///             								"type": "item",
///             								"code": "waterportion",
///             								"makefull": true
///             							}
///             						]
///             					}
///             				},
///             				{
///             					"type": "block",
///             					"code": "bowl-fired",
///             					"attributes": {
///             						"ucontents": [
///             							{
///             								"type": "item",
///             								"code": "honeyportion",
///             								"makefull": true
///             							}
///             						]
///             					}
///             				},
///             				{
///             					"type": "block",
///             					"code": "bowl-fired"
///             				},
///             				{
///             					"type": "block",
///             					"code": "bowl-raw"
///             				}
///             			]
///             		}
///             	]
///             },
/// </code>
/// </example>
[DocumentAsJson]
public class CreativeTabAndStackList
{
	/// <summary>
	/// A list of creative tabs to put items into. Note that all itemstacks in <see cref="F:Vintagestory.API.Common.CreativeTabAndStackList.Stacks" /> will be placed in all tabs.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public string[] Tabs;

	/// <summary>
	/// A list of item stacks to put in tabs. Note that every itemstack here will be placed in every <see cref="F:Vintagestory.API.Common.CreativeTabAndStackList.Tabs" /> entry.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public JsonItemStack[] Stacks;

	/// <summary>
	/// Reads the blocks and items from the Json files and converts them to an array of tabs which contain those blocks and items.
	/// </summary>
	/// <param name="reader">The reader to read the json.</param>
	/// <param name="registry">The registry of blocks and items.</param>
	public void FromBytes(BinaryReader reader, IClassRegistryAPI registry)
	{
		Tabs = new string[reader.ReadInt32()];
		for (int i = 0; i < Tabs.Length; i++)
		{
			Tabs[i] = reader.ReadString();
		}
		Stacks = new JsonItemStack[reader.ReadInt32()];
		for (int j = 0; j < Stacks.Length; j++)
		{
			Stacks[j] = new JsonItemStack();
			Stacks[j].FromBytes(reader, registry);
		}
	}

	/// <summary>
	/// Writes all the data to the BinaryWriter.
	/// </summary>
	/// <param name="writer">The writer to write the save data</param>
	/// <param name="registry">The registry of blocks and items.</param>
	public void ToBytes(BinaryWriter writer, IClassRegistryAPI registry)
	{
		writer.Write(Tabs.Length);
		for (int i = 0; i < Tabs.Length; i++)
		{
			writer.Write(Tabs[i]);
		}
		writer.Write(Stacks.Length);
		for (int j = 0; j < Stacks.Length; j++)
		{
			Stacks[j].ToBytes(writer);
		}
	}
}
