using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xabe.FFmpeg
{
	// Token: 0x02000024 RID: 36
	public interface IProbe
	{
		// Token: 0x06000104 RID: 260
		Task<string> Start(string args, CancellationToken cancellationToken = default(CancellationToken));
	}
}
