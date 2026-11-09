using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

public class EntitiesArgParser : ArgumentParserBase
{
	private Entity[] entities;

	private ICoreAPI api;

	public EntitiesArgParser(string argName, ICoreAPI api, bool isMandatoryArg)
		: base(argName, isMandatoryArg)
	{
		this.api = api;
	}

	public override string GetSyntaxExplanation(string indent)
	{
		return indent + GetSyntax() + " is either a player name, or else one of the following selection codes:\n" + indent + "  s[] for self\n" + indent + "  l[] for the entity currently looked at\n" + indent + "  p[] for all players\n" + indent + "  e[] with filters for entities, or e[!] for all entities.\n" + indent + "  Inside the square brackets, one or more filters can be added, to be more selective. Filters include name, type, class, tag, alive, range. Type can have wildcards. For example, <code>e[type=gazelle*,range=3,alive=true]</code>. The filters minx/miny/minz/maxx/maxy/maxz can also be used to specify a volume to search, coordinates are relative to the command caller's position.\n" + indent + "  This argument may be omitted if the remainder of the command makes sense, in which case it will be interpreted as self.";
	}

	public override object GetValue()
	{
		return entities;
	}

	public override void SetValue(object data)
	{
		entities = (Entity[])data;
	}

	public override void PreProcess(TextCommandCallingArgs args)
	{
		entities = null;
		base.PreProcess(args);
		if (base.IsMissing)
		{
			entities = new Entity[1] { args.Caller.Entity };
		}
	}

	public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults> onReady = null)
	{
		EnumParseResult result = parse(args);
		if (entities.Length > 100 && !args.Caller.HasPrivilege("largeentityselection"))
		{
			lastErrorMessage = Lang.Get("More than 100 entities selected ({0}). If you really need to select more, acquire the privilege 'largeentityselection'", entities.Length);
			entities = Array.Empty<Entity>();
			return EnumParseResult.Bad;
		}
		return result;
		EnumParseResult parse(TextCommandCallingArgs textCommandCallingArgs)
		{
			string maybeplayername = textCommandCallingArgs.RawArgs.PeekWord();
			IPlayer mplr = api.World.AllOnlinePlayers.FirstOrDefault((IPlayer p) => p.PlayerName.Equals(maybeplayername, StringComparison.InvariantCultureIgnoreCase));
			if (mplr != null)
			{
				textCommandCallingArgs.RawArgs.PopWord();
				this.entities = new Entity[1] { mplr.Entity };
				return EnumParseResult.Good;
			}
			string text = maybeplayername;
			char v = ((text != null && text.Length > 0) ? maybeplayername[0] : ' ');
			if (v != 'p' && v != 'e' && v != 'l' && v != 's')
			{
				lastErrorMessage = Lang.Get("Not a player name and not a selector p, e, l or s: {0}'", maybeplayername);
				this.entities = new Entity[1] { textCommandCallingArgs.Caller.Entity };
				return EnumParseResult.DependsOnSubsequent;
			}
			v = textCommandCallingArgs.RawArgs.PopChar() ?? ' ';
			if (maybeplayername.Equals("e[!]") && v == 'e')
			{
				textCommandCallingArgs.RawArgs.PopWord();
				if (api.Side == EnumAppSide.Server)
				{
					this.entities = (api as ICoreServerAPI).World.LoadedEntities.Values.ToArray();
				}
				else
				{
					this.entities = (api as ICoreClientAPI).World.LoadedEntities.Values.ToArray();
				}
				return EnumParseResult.Good;
			}
			Dictionary<string, string> subargs;
			if (textCommandCallingArgs.RawArgs.PeekChar() == '[')
			{
				string errorMsg;
				string subargsstr = textCommandCallingArgs.RawArgs.PopCodeBlock('[', ']', out errorMsg);
				if (errorMsg != null)
				{
					lastErrorMessage = errorMsg;
					return EnumParseResult.Bad;
				}
				subargs = parseSubArgs(subargsstr);
				if (subargs == null)
				{
					return EnumParseResult.Bad;
				}
			}
			else
			{
				if (textCommandCallingArgs.RawArgs.PeekChar() != ' ')
				{
					lastErrorMessage = "Invalid selector, needs to be p,e,l,s followed by [";
					return EnumParseResult.Bad;
				}
				textCommandCallingArgs.RawArgs.PopWord();
				subargs = new Dictionary<string, string>();
			}
			Vec3d sourcePos = textCommandCallingArgs.Caller.Pos;
			Entity callingEntity = textCommandCallingArgs.Caller.Entity;
			if (subargs.Count == 0 && v == 'e')
			{
				lastErrorMessage = "No selector defined, use e[!] to select all entities";
				return EnumParseResult.Bad;
			}
			EntityMatchConditions conds = EntityMatchConditions.FromArgs(subargs);
			if (sourcePos != null)
			{
				bool hasBox = false;
				string[] codes = new string[6] { "minx", "miny", "minz", "maxx", "maxy", "maxz" };
				int[] values = new int[6];
				for (int i = 0; i < codes.Length; i++)
				{
					if (subargs.TryGetValue(codes[i], out var val))
					{
						values[i] = val.ToInt() + i / 3;
						subargs.Remove(codes[i]);
						hasBox = true;
					}
				}
				if (hasBox)
				{
					BlockPos center = sourcePos.AsBlockPos;
					conds.Box = new Cuboidi(values).Translate(center.X, center.Y, center.Z);
				}
			}
			if (subargs.Count > 0)
			{
				lastErrorMessage = "Unknown selector '" + string.Join(", ", subargs.Keys) + "'";
				return EnumParseResult.Bad;
			}
			List<Entity> foundEntities = new List<Entity>();
			if (conds.Range.HasValue && sourcePos == null)
			{
				lastErrorMessage = "Can't use range argument without source pos";
				return EnumParseResult.Bad;
			}
			switch (v)
			{
			case 'p':
			{
				IPlayer[] allOnlinePlayers = api.World.AllOnlinePlayers;
				foreach (IPlayer plr in allOnlinePlayers)
				{
					if (entityMatches(plr.Entity, sourcePos, conds))
					{
						foundEntities.Add(plr.Entity);
					}
				}
				this.entities = foundEntities.ToArray();
				return EnumParseResult.Good;
			}
			case 'e':
				if (!conds.Range.HasValue)
				{
					ICollection<Entity> entities = ((api.Side != EnumAppSide.Server) ? (api as ICoreClientAPI).World.LoadedEntities.Values : (api as ICoreServerAPI).World.LoadedEntities.Values);
					foreach (Entity e in entities)
					{
						if (entityMatches(e, sourcePos, conds))
						{
							foundEntities.Add(e);
						}
					}
					this.entities = foundEntities.ToArray();
				}
				else
				{
					float r = conds.Range.Value;
					this.entities = api.World.GetEntitiesAround(sourcePos, r, r, (Entity e2) => entityMatches(e2, sourcePos, conds));
				}
				return EnumParseResult.Good;
			case 'l':
			{
				if (!(callingEntity is EntityPlayer eplr))
				{
					lastErrorMessage = "Can't use 'l' without source player";
					return EnumParseResult.Bad;
				}
				if (eplr.Player.CurrentEntitySelection == null)
				{
					lastErrorMessage = "Not looking at an entity";
					return EnumParseResult.Bad;
				}
				Entity lookedAtEntity = eplr.Player.CurrentEntitySelection.Entity;
				if (entityMatches(lookedAtEntity, sourcePos, conds))
				{
					this.entities = new Entity[1] { lookedAtEntity };
				}
				else
				{
					this.entities = Array.Empty<Entity>();
				}
				return EnumParseResult.Good;
			}
			case 's':
				if (entityMatches(callingEntity, sourcePos, conds))
				{
					this.entities = new Entity[1] { callingEntity };
				}
				else
				{
					this.entities = Array.Empty<Entity>();
				}
				return EnumParseResult.Good;
			default:
				lastErrorMessage = "Wrong selector, needs to be a player name or p,e,l or s";
				return EnumParseResult.Bad;
			}
		}
	}

	public static bool entityMatches(Entity e, Vec3d sourcePos, EntityMatchConditions conds)
	{
		if (conds.Id.HasValue && e.EntityId != conds.Id)
		{
			return false;
		}
		if (conds.Range.HasValue && e.Pos.DistanceTo(sourcePos) > (double?)conds.Range)
		{
			return false;
		}
		if (conds.Box != null && !conds.Box.ContainsOrTouches(e.Pos))
		{
			return false;
		}
		if (conds.Classstr != null && conds.Classstr != e.Class.ToLowerInvariant())
		{
			return false;
		}
		if (conds.Type != null && !WildcardUtil.Match(conds.Type, e.Code))
		{
			return false;
		}
		if (conds.Alive.HasValue && e.Alive != conds.Alive)
		{
			return false;
		}
		if (conds.Name != null && !WildcardUtil.Match(conds.Name, e.GetName()))
		{
			return false;
		}
		if (conds.Tagstr != null && !e.HasTags(conds.Tagstr))
		{
			return false;
		}
		return true;
	}
}
