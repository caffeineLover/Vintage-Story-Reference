using System;
using ProtoBuf;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.Common.Collectible.Block;

[ProtoContract]
public struct ConnectorMetaData : IEquatable<ConnectorMetaData>
{
	/// <summary>
	/// This is an offset when loaded from schematic, but an absolute position if its part of the OpenSet during dungeon generation
	/// </summary>
	[ProtoMember(1)]
	public FastVec3i Position;

	[ProtoMember(2)]
	public int FacingInt;

	public BlockFacing Facing;

	[ProtoMember(3)]
	public int Rotation;

	[ProtoMember(4)]
	public string Name;

	[ProtoMember(5)]
	public string[] Targets;

	[ProtoMember(6)]
	public string[] TargetsForParent;

	public string FromSchematicForDebug;

	/// <summary>
	/// True if this a properly set up BlockPosFacing
	/// </summary>
	public bool Valid => Facing != null;

	public ConnectorMetaData(FastVec3i position, BlockFacing facing, int rot, string name, string targets, string[] targetsforparent)
	{
		FacingInt = 0;
		Name = string.Empty;
		Targets = Array.Empty<string>();
		TargetsForParent = Array.Empty<string>();
		FromSchematicForDebug = null;
		Position = position;
		Facing = facing;
		Rotation = rot;
		Name = name ?? string.Empty;
		Targets = (string.IsNullOrEmpty(targets) ? Array.Empty<string>() : targets.Split(","));
		TargetsForParent = targetsforparent;
	}

	public ConnectorMetaData(FastVec3i position, BlockFacing facing, int rot, string name, string[] targets, string[] targetsforparent)
	{
		FacingInt = 0;
		Name = string.Empty;
		Targets = Array.Empty<string>();
		TargetsForParent = Array.Empty<string>();
		FromSchematicForDebug = null;
		Position = position;
		Facing = facing;
		Rotation = rot;
		Name = name ?? string.Empty;
		Targets = targets;
		TargetsForParent = targetsforparent;
	}

	public bool ConnectsTo(string name)
	{
		string[] targets = Targets;
		for (int i = 0; i < targets.Length; i++)
		{
			if (WildcardUtil.Match(targets[i], name))
			{
				return true;
			}
		}
		return false;
	}

	public bool ConnectsTo(ConnectorMetaData p)
	{
		if (Valid && p.Valid && p.Facing.Opposite == Facing)
		{
			if (Name.Length <= 0 || !p.ConnectsTo(Name))
			{
				if (p.Name.Length > 0)
				{
					return ConnectsTo(p.Name);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public bool ConnectsTo(ConnectorMetaData p, FastVec3i pos)
	{
		if (ConnectsTo(p))
		{
			return (Position + pos).Add(Facing) == p.Position;
		}
		return false;
	}

	public bool Equals(ConnectorMetaData other)
	{
		if (object.Equals(Position, other.Position) && object.Equals(Facing, other.Facing) && Name == other.Name)
		{
			return Targets == other.Targets;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is ConnectorMetaData other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, Facing, Name, Targets);
	}

	public ConnectorMetaData Clone()
	{
		return new ConnectorMetaData(Position, Facing, Rotation, Name, Targets, TargetsForParent);
	}

	public ConnectorMetaData Offset(FastVec3i startPos)
	{
		Position += startPos;
		return this;
	}

	public override string ToString()
	{
		return string.Format("n-{0} t-{1} ({2})", Name, string.Join(",", Targets), FromSchematicForDebug);
	}
}
