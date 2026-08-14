using System.Collections.Generic;
using ProtoBuf;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

[ProtoContract]
public class AuctionsData
{
	[ProtoMember(1)]
	public Vintagestory.API.Datastructures.OrderedDictionary<long, Auction> auctions = new Vintagestory.API.Datastructures.OrderedDictionary<long, Auction>();

	[ProtoMember(2)]
	public long nextAuctionId;

	[ProtoMember(3)]
	public Dictionary<string, float> DebtToTraderByPlayer = new Dictionary<string, float>();
}
