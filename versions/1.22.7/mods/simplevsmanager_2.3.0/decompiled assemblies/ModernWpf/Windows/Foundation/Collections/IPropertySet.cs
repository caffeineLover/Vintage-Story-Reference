using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation.Collections
{
	// Token: 0x0200008E RID: 142
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("8A43ED9F-F4E6-4421-ACF9-1DAB2986820C")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IPropertySet : IObservableMap<string, object>, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
	}
}
