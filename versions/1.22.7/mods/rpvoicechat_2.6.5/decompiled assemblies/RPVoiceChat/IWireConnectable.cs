using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.Systems;
using Vintagestory.API.MathTools;

// Token: 0x02000012 RID: 18
public interface IWireConnectable
{
	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000064 RID: 100
	BlockPos Position { get; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000065 RID: 101
	// (set) Token: 0x06000066 RID: 102
	long NetworkUID { get; set; }

	// Token: 0x06000067 RID: 103
	IReadOnlyList<WireConnection> GetConnections();

	// Token: 0x06000068 RID: 104
	void AddConnection(WireConnection connection);

	// Token: 0x06000069 RID: 105
	bool HasConnection(WireConnection connection);

	// Token: 0x0600006A RID: 106
	void MarkForUpdate();
}
