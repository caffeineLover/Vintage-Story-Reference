using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.Server;

public class CmdEntity
{
	private ServerMain server;

	public CmdEntity(ServerMain server)
	{
		CmdEntity cmdEntity = this;
		this.server = server;
		IChatCommandApi cmdapi = server.api.ChatCommands;
		CommandArgumentParsers parsers = server.api.ChatCommands.Parsers;
		ServerCoreAPI sapi = server.api;
		cmdapi.GetOrCreate("entity").WithAlias("e").WithDesc("Entity control via entity selector")
			.RequiresPrivilege(Privilege.controlserver)
			.BeginSub("cmd")
			.WithDesc("Issue commands on existing entities")
			.WithArgs(parsers.Entities("target entities"))
			.BeginSub("stopanim")
			.WithDesc("Stop an entity animation")
			.WithArgs(parsers.Word("animation name"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				e.StopAnimation((string)args.LastArg);
				return TextCommandResult.Success("animation stopped");
			}))
			.EndSub()
			.BeginSub("starttask")
			.WithDesc("Start an ai task")
			.WithArgs(parsers.Word("task id"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				e.Notify("starttask", (string)args.LastArg);
				return TextCommandResult.Success("task start executed");
			}))
			.EndSub()
			.BeginSub("stoptask")
			.WithDesc("Stop an ai task")
			.WithArgs(parsers.Word("task id"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				e.Notify("stoptask", (string)args.LastArg);
				return TextCommandResult.Success("task stop executed");
			}))
			.EndSub()
			.BeginSub("setattr")
			.WithDesc("Set entity attributes")
			.WithArgs(parsers.WordRange("datatype", "float", "int", "string", "bool"), parsers.Word("name"), parsers.Word("value"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, (Entity e) => cmdEntity.entitySetAttr(e, args)))
			.EndSub()
			.BeginSub("attr")
			.WithDesc("Read entity attributes")
			.WithArgs(parsers.Word("name"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, (Entity e) => cmdEntity.entityReadAttr(e, args)))
			.EndSub()
			.BeginSub("setgen")
			.WithDesc("Set entity generation")
			.WithArgs(parsers.Int("generation"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				e.WatchedAttributes.SetInt("generation", (int)args[1]);
				return TextCommandResult.Success("generation set");
			}))
			.EndSub()
			.BeginSub("rmbh")
			.WithDesc("Remove behavior")
			.WithArgs(parsers.Word("behavior code"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				EntityBehavior behavior = e.GetBehavior((string)args[1]);
				if (behavior == null)
				{
					return TextCommandResult.Error("entity " + e.Code.ToShortString() + " has no such behavior");
				}
				e.RemoveBehavior(behavior);
				return TextCommandResult.Success("Ok, behavior removed");
			}))
			.EndSub()
			.BeginSub("setlact")
			.WithDesc("Set entity lactating")
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				e.WatchedAttributes.GetTreeAttribute("multiply")?.SetDouble("totalDaysLastBirth", server.api.World.Calendar.TotalDays);
				e.WatchedAttributes.MarkPathDirty("multiply");
				return TextCommandResult.Success("Ok, entity lactating set");
			}))
			.EndSub()
			.BeginSub("move")
			.WithDesc("move a creature")
			.WithArgs(parsers.Double("delta x"), parsers.Double("delta y"), parsers.Double("delta z"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				e.Pos.X += (double)args[1];
				e.Pos.Y += (double)args[2];
				e.Pos.Z += (double)args[3];
				return TextCommandResult.Success("Ok, entity moved");
			}))
			.EndSub()
			.BeginSub("kill")
			.WithDesc("kill a creature")
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				if (e == args.Caller.Entity)
				{
					return TextCommandResult.Success("Ignoring killing of caller");
				}
				e.Die(EnumDespawnReason.Death, new DamageSource
				{
					Source = EnumDamageSource.Player,
					SourcePos = args.Caller.Pos,
					SourceEntity = args.Caller.Entity
				});
				return TextCommandResult.Success("Ok, entity killed");
			}))
			.EndSub()
			.BeginSub("birth")
			.WithDesc("force a creature to give birth (if it can!)")
			.WithArgs(parsers.OptionalInt("number"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				EntityBehavior? behavior = e.GetBehavior("multiply");
				behavior?.TestCommand(args.Parsers[1].IsMissing ? ((object)1) : args[1]);
				return TextCommandResult.Success((behavior == null) ? (Lang.Get("item-creature-" + e.Code.Path) + " " + Lang.Get("can't bear young!")) : "OK!");
			}))
			.EndSub()
			.EndSub()
			.BeginSub("wipeall")
			.WithDesc("Removes all entities (except players) from all loaded chunks")
			.WithArgs(parsers.OptionalInt("killRadius"))
			.HandleWith(WipeAllHandler)
			.EndSub();
		cmdapi.GetOrCreate("entity").BeginSub("debug").WithDesc("Set entity debug mode")
			.WithArgs(parsers.Bool("on"))
			.HandleWith(delegate(TextCommandCallingArgs args)
			{
				server.Config.EntityDebugMode = (bool)args[0];
				server.ConfigNeedsSaving = true;
				return TextCommandResult.Success(Lang.Get("Ok, entity debug mode is now {0}", server.Config.EntityDebugMode ? Lang.Get("on") : Lang.Get("off")));
			})
			.EndSub()
			.BeginSub("spawndebug")
			.WithDesc("Set entity spawn debug mode")
			.WithArgs(parsers.Bool("on"))
			.HandleWith(delegate(TextCommandCallingArgs args)
			{
				server.SpawnDebug = (bool)args[0];
				return TextCommandResult.Success(Lang.Get("Ok, entity spawn debug mode is now {0}", server.SpawnDebug ? Lang.Get("on") : Lang.Get("off")));
			})
			.EndSub()
			.BeginSub("count")
			.WithDesc("Count entities by code/filter and show a summary")
			.WithArgs(parsers.OptionalEntities("entity filter"))
			.RequiresPrivilege(Privilege.gamemode)
			.HandleWith((TextCommandCallingArgs args) => cmdEntity.Count(args, grouped: false))
			.EndSub()
			.BeginSub("locateg")
			.WithDesc("Group entities together within the specified range and returns the position and amount. This is to find large groups of entities.")
			.WithArgs(parsers.OptionalEntities("entity filter"), parsers.OptionalInt("range", 100))
			.RequiresPrivilege(Privilege.gamemode)
			.HandleWith(OnLocateGroup)
			.EndSub()
			.BeginSub("find")
			.WithDesc("Returns the name and position of every matching entity which is loaded on the server.")
			.WithAdditionalInformation("Limited to 100 results to prevent spam; use more precise entity filters if that is too many.")
			.WithArgs(parsers.Entities("entity filter"))
			.RequiresPrivilege(Privilege.gamemode)
			.HandleWith(OnFind)
			.EndSub()
			.BeginSub("countg")
			.WithDesc("Count entities by code/filter and show a summary grouped by first code part")
			.WithArgs(parsers.OptionalEntities("entity filter"))
			.RequiresPrivilege(Privilege.gamemode)
			.HandleWith((TextCommandCallingArgs args) => cmdEntity.Count(args, grouped: true))
			.EndSub()
			.BeginSub("spawn")
			.WithAlias("sp")
			.WithDesc("Spawn entities at the caller's position")
			.WithArgs(parsers.EntityType("entity type"), parsers.Int("amount"), parsers.OptionalAll("attributes").WithExplanation("is (optionally) attributes to be applied to the entity, in JSON format"))
			.HandleWith(spawnEntities)
			.EndSub()
			.BeginSub("spawnat")
			.WithDesc("Spawn entities at given position, within a given radius")
			.WithArgs(parsers.EntityType("entity type"), parsers.Int("amount"), parsers.WorldPosition("position"), parsers.Double("spawn radius"))
			.HandleWith(spawnEntitiesAt)
			.EndSub()
			.BeginSub("remove")
			.WithAlias("re")
			.WithDesc("remove selected creatures")
			.WithArgs(parsers.Entities("target entities"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, delegate(Entity e)
			{
				if (e == args.Caller.Entity)
				{
					return TextCommandResult.Success("Ignoring removal of caller");
				}
				e.Die(EnumDespawnReason.Removed);
				return TextCommandResult.Success("Ok, entity removed");
			}))
			.EndSub()
			.BeginSub("removebyid")
			.WithDesc("remove selected creatures")
			.WithArgs(parsers.Long("id"))
			.HandleWith(delegate(TextCommandCallingArgs args)
			{
				long num = (long)args[0];
				if (num == args.Caller.Entity.EntityId)
				{
					return TextCommandResult.Success("Ignoring removal of caller");
				}
				if (sapi.World.LoadedEntities.TryGetValue(num, out var value))
				{
					value.Die(EnumDespawnReason.Removed);
					return TextCommandResult.Success("Ok, entity removed");
				}
				return TextCommandResult.Success("No entity found");
			})
			.EndSub()
			.BeginSub("set-angle")
			.WithAlias("sa")
			.WithDesc("Set the angle of the entity")
			.WithArgs(parsers.Entities("target entities"), parsers.WordRange("axis", "yaw", "pitch", "roll"), parsers.Float("degrees"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, (Entity e) => cmdEntity.setEntityAngle(e, args)))
			.EndSub()
			.BeginSub("export")
			.WithDescription("Export a entity spawnat command to server-main log file")
			.WithArgs(parsers.Entities("target entities"))
			.HandleWith((TextCommandCallingArgs args) => CmdUtil.EntityEach(args, (Entity e) => cmdEntity.exportEntity(e, args)))
			.EndSub()
			.BeginSub("list")
			.WithDesc("List all loaded entity types, with optional filter")
			.WithExamples("/entity list [type=villager*] to list all currently loaded villagers")
			.WithArgs(parsers.OptionalEntities("entity filter"))
			.RequiresPrivilege(Privilege.gamemode)
			.HandleWith(list)
			.EndSub();
	}

	private TextCommandResult list(TextCommandCallingArgs args)
	{
		StringBuilder text = new StringBuilder();
		int count = 0;
		HashSet<string> entityCodes = new HashSet<string>();
		ICollection<Entity> collection2;
		if (!args.Parsers[0].IsMissing)
		{
			ICollection<Entity> collection = args[0] as Entity[];
			collection2 = collection;
		}
		else
		{
			collection2 = server.LoadedEntities.Values;
		}
		foreach (Entity entity in collection2)
		{
			if (!(entity is EntityPlayer))
			{
				count++;
				string code = entity.Code.ToShortString();
				string name = entity.GetName();
				entityCodes.Add(code + "   (" + name + ")");
			}
		}
		string[] array = entityCodes.ToArray();
		Array.Sort(array);
		string[] array2 = array;
		foreach (string code2 in array2)
		{
			text.AppendLine(code2);
		}
		text.AppendLine("(" + count + " entity types found)");
		return TextCommandResult.Success(text.ToString());
	}

	private TextCommandResult exportEntity(Entity entity, TextCommandCallingArgs args)
	{
		server.api.Logger.Notification($"/entity spawnat {entity.Code} 1 ={entity.Pos.X:F2} ={entity.Pos.Y:F2} ={entity.Pos.Z:F2} 0");
		return TextCommandResult.Success("Ok, entity exported");
	}

	private TextCommandResult spawnEntitiesAt(TextCommandCallingArgs args)
	{
		EntityProperties entityType = (EntityProperties)args[0];
		int quantity = (int)args[1];
		Vec3d pos = (Vec3d)args[2];
		double radius = (double)args[3];
		Random rnd = server.api.World.Rand;
		long herdid = server.GetNextHerdId();
		int i = quantity;
		while (i-- > 0)
		{
			Entity entity = server.api.ClassRegistry.CreateEntity(entityType);
			if (entity is EntityAgent)
			{
				(entity as EntityAgent).HerdId = herdid;
			}
			entity.Pos.SetFrom(pos);
			entity.Pos.X += rnd.NextDouble() * 2.0 * radius - radius;
			entity.Pos.Z += rnd.NextDouble() * 2.0 * radius - radius;
			entity.Pos.Pitch = 0f;
			entity.Pos.Yaw = 0f;
			server.SpawnEntity(entity);
		}
		return TextCommandResult.Success(Lang.Get("{0}x{1} spawned.", quantity, entityType.Code));
	}

	private TextCommandResult spawnEntities(TextCommandCallingArgs args)
	{
		int quantity = (int)args[1];
		EntityProperties entityType = (EntityProperties)args[0];
		string optionalAttributes = (args.Parsers[2].IsMissing ? "" : (args[2] as string));
		Random rnd = server.api.World.Rand;
		long herdid = server.GetNextHerdId();
		bool badAttributes = false;
		int i = quantity;
		while (i-- > 0)
		{
			Entity entity = server.api.ClassRegistry.CreateEntity(entityType);
			if (entity is EntityAgent)
			{
				(entity as EntityAgent).HerdId = herdid;
			}
			entity.Pos.SetFrom(args.Caller.Entity.Pos);
			entity.Pos.X += rnd.NextDouble() / 10.0 - 0.05;
			entity.Pos.Z += rnd.NextDouble() / 10.0 - 0.05;
			entity.Pos.Pitch = 0f;
			entity.Pos.Yaw = 0f;
			entity.Pos.Motion.Set((0.125 - 0.25 * rnd.NextDouble()) / 2.0, (0.1 + 0.1 * rnd.NextDouble()) / 2.0, (0.125 - 0.25 * rnd.NextDouble()) / 2.0);
			if (optionalAttributes.Length > 0)
			{
				try
				{
					entity.SetAttributesFrom(JToken.Parse(optionalAttributes));
				}
				catch
				{
					badAttributes = true;
				}
			}
			server.SpawnEntity(entity);
		}
		string result = Lang.Get("{0}x{1} spawned.", quantity, entityType.Code);
		if (badAttributes)
		{
			result += "\nAttributes ignored as badly formatted; valid JSON format must be used";
		}
		return TextCommandResult.Success(result);
	}

	private TextCommandResult entitySetAttr(Entity entity, TextCommandCallingArgs args)
	{
		string datatype = (string)args[1];
		string name = (string)args[2];
		string value = (string)args[3];
		ITreeAttribute attr = entity.WatchedAttributes;
		string path = null;
		if (name.Contains("/"))
		{
			string[] array = name.Split('/');
			name = array[^1];
			string[] patharr = array.RemoveAt(array.Length - 1);
			path = string.Join("/", patharr);
			attr = entity.WatchedAttributes.GetAttributeByPath(path) as ITreeAttribute;
			if (attr == null)
			{
				return TextCommandResult.Error(Lang.Get("No such path - {0}", path), "nosuchpath");
			}
		}
		if (path != null)
		{
			entity.WatchedAttributes.MarkPathDirty(path);
		}
		switch (datatype)
		{
		case "float":
		{
			float val4 = value.ToFloat();
			attr.SetFloat(name, val4);
			return TextCommandResult.Success(name + " float value set to " + val4);
		}
		case "int":
		{
			int val3 = value.ToInt();
			attr.SetInt(name, val3);
			return TextCommandResult.Success(name + " int value set to " + val3);
		}
		case "string":
		{
			string val2 = value + args.RawArgs.PopAll();
			attr.SetString(name, val2);
			return TextCommandResult.Success(name + " string value set to " + val2);
		}
		case "bool":
		{
			bool val = value.ToBool();
			attr.SetBool(name, val);
			return TextCommandResult.Success(name + " bool value set to " + val);
		}
		default:
			return TextCommandResult.Error("Incorrect datatype, choose float, int, string or bool", "wrongdatatype");
		}
	}

	private TextCommandResult entityReadAttr(Entity entity, TextCommandCallingArgs args)
	{
		string name = (string)args[1];
		IAttribute attr = entity.WatchedAttributes.GetAttributeByPath(name);
		if (attr == null)
		{
			return TextCommandResult.Error(Lang.Get("No such path - {0}", name), "nosuchpath");
		}
		return TextCommandResult.Success(Lang.Get("Value is: {0}", attr.GetValue()));
	}

	private TextCommandResult setEntityAngle(Entity entity, TextCommandCallingArgs args)
	{
		string axis = (string)args[1];
		float degrees = (float)args[2];
		switch (axis)
		{
		case "yaw":
			entity.Pos.Yaw = (float)Math.PI / 180f * degrees;
			break;
		case "pitch":
			entity.Pos.Pitch = (float)Math.PI / 180f * degrees;
			break;
		case "roll":
			entity.Pos.Roll = (float)Math.PI / 180f * degrees;
			break;
		}
		return TextCommandResult.Success("Entity angle set");
	}

	private TextCommandResult OnLocateGroup(TextCommandCallingArgs args)
	{
		Dictionary<BlockPos, List<Entity>> ranged = new Dictionary<BlockPos, List<Entity>>();
		int range = (int)args[1];
		List<Entity> entities = ((!args.Parsers[0].IsMissing) ? (args[0] as Entity[]).ToList() : server.LoadedEntities.Values.ToList());
		if (entities.Count != 0)
		{
			Entity first = entities.First();
			ranged.Add(first.Pos.AsBlockPos, new List<Entity> { first });
		}
		foreach (Entity entity in entities.Skip(1))
		{
			bool found = false;
			foreach (var (pos, _) in ranged)
			{
				if (entity.Pos.HorDistanceTo(pos.ToVec3d()) < (double)range)
				{
					ranged[pos].Add(entity);
					found = true;
					break;
				}
			}
			if (!found)
			{
				ranged.Add(entity.Pos.AsBlockPos, new List<Entity> { entity });
			}
		}
		string result;
		if (ranged.Count == 0)
		{
			result = "No entities found";
		}
		else
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<BlockPos, List<Entity>> val in ranged)
			{
				sb.AppendLine(val.Key?.ToString() + " : " + val.Value.Count);
			}
			result = sb.ToString();
		}
		return TextCommandResult.Success(result);
	}

	private TextCommandResult OnFind(TextCommandCallingArgs args)
	{
		ICollection<Entity> collection2;
		if (!args.Parsers[0].IsMissing)
		{
			ICollection<Entity> collection = args[0] as Entity[];
			collection2 = collection;
		}
		else
		{
			collection2 = server.LoadedEntities.Values;
		}
		List<Entity> foundEntities = new List<Entity>(collection2);
		if (args.Caller.Entity is EntityPlayer player)
		{
			EntityRangeComparer rangeComparer = new EntityRangeComparer(new Vec3d(player.Pos));
			foundEntities.Sort(0, foundEntities.Count, rangeComparer);
		}
		BlockPos mapcenter = server.api.World.DefaultSpawnPosition.AsBlockPos;
		mapcenter.Y = 0;
		StringBuilder text = new StringBuilder();
		int count = 0;
		foreach (Entity entity in foundEntities)
		{
			string entry = entity.GetName() + "  at " + entity.Pos.AsBlockPos.Sub(mapcenter);
			text.AppendLine(entry);
			if (++count >= 100)
			{
				break;
			}
		}
		if (text.Length == 0)
		{
			text.Append("(no matches found)");
		}
		else
		{
			text.Insert(0, "===Found entities===\n");
			if (count >= 100)
			{
				text.Append("(" + count + " matches found, showing nearest 100)");
			}
			else if (count == 1)
			{
				text.Append("(1 match found)");
			}
			else
			{
				text.Append("(" + count + " matches found)");
			}
		}
		return TextCommandResult.Success(text.ToString());
	}

	private TextCommandResult Count(TextCommandCallingArgs args, bool grouped)
	{
		int totalCount = 0;
		int totalActiveCount = 0;
		Dictionary<string, int> quantities = new Dictionary<string, int>();
		IEnumerable<Entity> entities = ((!args.Parsers[0].IsMissing) ? (args[0] as Entity[]) : server.LoadedEntities.Values);
		foreach (Entity entity in entities)
		{
			string code = entity.Code.Path;
			if (grouped)
			{
				code = entity.FirstCodePart();
			}
			if (quantities.ContainsKey(code))
			{
				quantities[code]++;
			}
			else
			{
				quantities[code] = 1;
			}
			if (entity.State == EnumEntityState.Active)
			{
				totalActiveCount++;
			}
			totalCount++;
		}
		string result;
		if (quantities.Count == 0)
		{
			result = "No entities found";
		}
		else
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(Lang.Get("{0} total entities, of which {1} active.", totalCount, totalActiveCount));
			foreach (KeyValuePair<string, int> val in quantities)
			{
				sb.AppendLine(val.Key + ": " + val.Value);
			}
			result = sb.ToString();
		}
		return TextCommandResult.Success(result);
	}

	private bool entityTypeMatches(EntityProperties type, EntityProperties referenceType, string searchCode, bool isWildcard)
	{
		if (isWildcard)
		{
			string pattern = Regex.Escape(searchCode).Replace("\\*", "(.*)");
			return Regex.IsMatch(type.Code.Path.ToLowerInvariant(), "^" + pattern + "$");
		}
		return type.Code.Path == referenceType.Code.Path;
	}

	private TextCommandResult WipeAllHandler(TextCommandCallingArgs args)
	{
		int rangeSquared;
		if (args.Parsers[0].IsMissing)
		{
			rangeSquared = 0;
		}
		else
		{
			rangeSquared = (int)args[0];
			rangeSquared *= rangeSquared + 1;
		}
		int centerX = args.Caller.Pos.XInt;
		int centerZ = args.Caller.Pos.ZInt;
		int count = 0;
		foreach (KeyValuePair<long, Entity> val in server.LoadedEntities)
		{
			if (!(val.Value is EntityPlayer) && (rangeSquared <= 0 || val.Value.Pos.InHorizontalRangeOf(centerX, centerZ, rangeSquared)))
			{
				val.Value.Die(EnumDespawnReason.Removed);
				count++;
			}
		}
		return TextCommandResult.Success("Killed " + count + " entities");
	}
}
