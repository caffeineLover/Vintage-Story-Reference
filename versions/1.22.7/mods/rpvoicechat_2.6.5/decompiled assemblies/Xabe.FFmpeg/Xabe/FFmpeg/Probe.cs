using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xabe.FFmpeg
{
	// Token: 0x02000029 RID: 41
	public class Probe : IProbe
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00005B58 File Offset: 0x00003D58
		public static IProbe New()
		{
			return new Probe();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005B5F File Offset: 0x00003D5F
		public Task<string> Start(string args, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new FFprobeWrapper().Start(args, cancellationToken);
		}
	}
}
