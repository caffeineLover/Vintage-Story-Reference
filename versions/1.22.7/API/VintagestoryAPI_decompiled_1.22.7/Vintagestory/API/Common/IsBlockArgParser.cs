using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

public class IsBlockArgParser : ArgumentParserBase
{
	private ICoreAPI api;

	private int blockId;

	private Vec3d pos;

	private bool isFluid;

	private Dictionary<string, string> subargs;

	public IsBlockArgParser(string argName, ICoreAPI api, bool isMandatoryArg)
		: base(argName, isMandatoryArg)
	{
		this.api = api;
	}

	public override object GetValue()
	{
		if (pos == null)
		{
			return false;
		}
		BlockPos bpos = pos.AsBlockPos;
		if (api.World.BlockAccessor.GetBlock(bpos, (!isFluid) ? 1 : 2).Id != blockId)
		{
			return false;
		}
		if (subargs == null || subargs.Count == 0)
		{
			return true;
		}
		BlockEntity be = api.World.BlockAccessor.GetBlockEntity(bpos);
		if (be == null)
		{
			return false;
		}
		TreeAttribute tree = new TreeAttribute();
		be.ToTreeAttributes(tree);
		foreach (KeyValuePair<string, string> pair in subargs)
		{
			if (!tree.HasAttribute(pair.Key))
			{
				return false;
			}
			if (tree.GetAttribute(pair.Key).ToString() != pair.Value)
			{
				return false;
			}
		}
		return true;
	}

	public override void SetValue(object data)
	{
		throw new NotImplementedException();
	}

	public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults> onReady = null)
	{
		pos = null;
		if (args.RawArgs.Length != 4)
		{
			lastErrorMessage = "Required format: isBlock[code=...] x y z";
			return EnumParseResult.Bad;
		}
		if (args.RawArgs.PopUntil('[') != "isBlock")
		{
			lastErrorMessage = "Required format: isBlock[code=...] x y z";
			return EnumParseResult.Bad;
		}
		string errorMsg;
		string subargsstr = args.RawArgs.PopCodeBlock('[', ']', out errorMsg);
		if (errorMsg != null)
		{
			lastErrorMessage = errorMsg;
			return EnumParseResult.Bad;
		}
		if (subargsstr.Length > 2)
		{
			subargs = parseSubArgs(subargsstr);
			if (subargs == null)
			{
				return EnumParseResult.Bad;
			}
			if (!subargs.TryGetValue("code", out var code))
			{
				lastErrorMessage = "Requires [code=...] to be specified";
				return EnumParseResult.Bad;
			}
			Block block = api.World.GetBlock(new AssetLocation(code));
			if (block == null)
			{
				lastErrorMessage = "Code " + code + " is not a valid block";
				return EnumParseResult.Bad;
			}
			blockId = block.Id;
			isFluid = block.ForFluidsLayer;
			subargs.Remove("code");
		}
		Vec3d mapmiddlePos = new Vec3d(api.World.DefaultSpawnPosition.X, 0.0, api.World.DefaultSpawnPosition.Z);
		pos = args.RawArgs.PopFlexiblePos(args.Caller.Pos, mapmiddlePos);
		if (pos == null)
		{
			lastErrorMessage = Lang.Get("Invalid position, must be 3 numbers");
			return EnumParseResult.Bad;
		}
		return EnumParseResult.Good;
	}

	public static string Test(ICoreAPI api, Caller caller, string testcmd)
	{
		TextCommandCallingArgs packedArgs = new TextCommandCallingArgs
		{
			Caller = caller,
			RawArgs = new CmdArgs(testcmd)
		};
		IsBlockArgParser blockCondParser = new IsBlockArgParser("cond", api, isMandatoryArg: true);
		if (blockCondParser.TryProcess(packedArgs) == EnumParseResult.Bad)
		{
			return blockCondParser.LastErrorMessage;
		}
		return blockCondParser.TestCond();
	}

	private string TestCond()
	{
		if (pos == null)
		{
			return "No position specified";
		}
		BlockPos bpos = pos.AsBlockPos;
		Block bSolid = api.World.BlockAccessor.GetBlock(bpos, 1);
		Block bFluid = api.World.BlockAccessor.GetBlock(bpos, 2);
		StringBuilder sb = new StringBuilder();
		if (bSolid.Id > 0)
		{
			sb.AppendLine("Solid: " + bSolid.Code.ToShortString());
		}
		if (bFluid.Id > 0)
		{
			sb.AppendLine("Fluid: " + bFluid.Code.ToShortString());
		}
		BlockEntity be = api.World.BlockAccessor.GetBlockEntity(bpos);
		if (be == null)
		{
			sb.AppendLine("(no BlockEntity here)");
		}
		else
		{
			TreeAttribute tree = new TreeAttribute();
			be.ToTreeAttributes(tree);
			foreach (KeyValuePair<string, IAttribute> attr in tree)
			{
				if (!"posx".Equals(attr.Key) && !"posy".Equals(attr.Key) && !"posz".Equals(attr.Key))
				{
					sb.AppendLine("  " + attr.Key + "=" + attr.Value.ToString());
				}
			}
		}
		return sb.ToString();
	}
}
