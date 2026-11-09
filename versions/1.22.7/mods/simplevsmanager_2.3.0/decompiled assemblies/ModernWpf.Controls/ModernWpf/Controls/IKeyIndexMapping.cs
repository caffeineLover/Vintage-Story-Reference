using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200008F RID: 143
	public interface IKeyIndexMapping
	{
		// Token: 0x06000718 RID: 1816
		string KeyFromIndex(int index);

		// Token: 0x06000719 RID: 1817
		int IndexFromKey(string key);
	}
}
