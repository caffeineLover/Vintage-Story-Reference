using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.API.Common;

public abstract class PositionArgumentParserBase : ArgumentParserBase
{
	protected PositionArgumentParserBase(string argName, bool isMandatoryArg)
		: base(argName, isMandatoryArg)
	{
	}

	protected EnumParseResult tryGetPositionBySelector(char v, TextCommandCallingArgs args, ref Vec3d pos, ICoreAPI api)
	{
		string subargsstr = args.RawArgs.PopWord();
		Dictionary<string, string> subargs = parseSubArgs(subargsstr);
		if (subargs == null)
		{
			return EnumParseResult.Bad;
		}
		Vec3d sourcePos = args.Caller.Pos;
		Entity callingEntity = args.Caller.Entity;
		EntityMatchConditions conds = EntityMatchConditions.FromArgs(subargs);
		if (conds.Range.HasValue && sourcePos == null)
		{
			lastErrorMessage = "Can't use range argument without source position";
			return EnumParseResult.Bad;
		}
		switch (v)
		{
		case 'p':
		{
			IPlayer nearestPlr = null;
			IPlayer[] allOnlinePlayers = api.World.AllOnlinePlayers;
			foreach (IPlayer plr in allOnlinePlayers)
			{
				if (!EntitiesArgParser.entityMatches(plr.Entity, sourcePos, conds))
				{
					continue;
				}
				if (nearestPlr == null)
				{
					nearestPlr = plr;
					continue;
				}
				if (sourcePos == null)
				{
					lastErrorMessage = "Two matching players found. Can't get nearest player without source position";
					return EnumParseResult.Bad;
				}
				if (nearestPlr.Entity.Pos.DistanceTo(sourcePos) > plr.Entity.Pos.DistanceTo(sourcePos))
				{
					nearestPlr = plr;
				}
			}
			pos = nearestPlr?.Entity.Pos.XYZ;
			return EnumParseResult.Good;
		}
		case 'e':
		{
			ICollection<Entity> entities = ((api.Side != EnumAppSide.Server) ? (api as ICoreClientAPI).World.LoadedEntities.Values : (api as ICoreServerAPI).World.LoadedEntities.Values);
			Entity nearestEntity = null;
			if (conds.ConditionCount == 0 && v == 'e')
			{
				lastErrorMessage = "No selector defined, use e[!] to select all entities";
				return EnumParseResult.Bad;
			}
			foreach (Entity e in entities)
			{
				if (!EntitiesArgParser.entityMatches(e, sourcePos, conds))
				{
					continue;
				}
				if (nearestEntity == null)
				{
					nearestEntity = e;
					continue;
				}
				if (sourcePos == null)
				{
					lastErrorMessage = "Two matching entities found. Can't get nearest entity without source position";
					return EnumParseResult.Bad;
				}
				if (nearestEntity.Pos.DistanceTo(sourcePos) > e.Pos.DistanceTo(sourcePos))
				{
					nearestEntity = e;
				}
			}
			pos = nearestEntity?.Pos.XYZ;
			return EnumParseResult.Good;
		}
		case 'l':
			if (!(callingEntity is EntityPlayer eplr))
			{
				lastErrorMessage = "Can't use 'l' without source player";
				return EnumParseResult.Bad;
			}
			if (eplr.Player.CurrentEntitySelection == null && eplr.Player.CurrentBlockSelection == null)
			{
				lastErrorMessage = "Not looking at an entity or block";
				return EnumParseResult.Bad;
			}
			pos = eplr.Player.CurrentEntitySelection?.Entity.Pos.XYZ ?? eplr.Player.CurrentBlockSelection.Position.ToVec3d();
			return EnumParseResult.Good;
		case 's':
			pos = callingEntity.Pos.XYZ;
			return EnumParseResult.Good;
		default:
			lastErrorMessage = "Wrong selector, needs to be p,e,l or s";
			return EnumParseResult.Bad;
		}
	}
}
