using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace HtmlAgilityPack
{
	// Token: 0x02000022 RID: 34
	[NullableContext(1)]
	[Nullable(0)]
	public class HtmlWeb
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
		internal static HttpClient GetSharedHttpClient(HtmlWeb web)
		{
			return HtmlWeb.SharedHttpClient.GetOrAdd(web.UserAgent, delegate(string x)
			{
				HttpClient httpClient = new HttpClient();
				httpClient.Timeout = TimeSpan.FromMilliseconds((double)web.Timeout);
				httpClient.DefaultRequestHeaders.Add("User-Agent", web.UserAgent);
				return httpClient;
			});
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000D71B File Offset: 0x0000B91B
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000D723 File Offset: 0x0000B923
		public DecompressionMethods AutomaticDecompression { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000D761 File Offset: 0x0000B961
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000D72C File Offset: 0x0000B92C
		public int? MaxAutoRedirects
		{
			get
			{
				return this._maxAutoRedirects;
			}
			set
			{
				int? num = value;
				int num2 = 0;
				if (num.GetValueOrDefault() <= num2 & num != null)
				{
					throw new ArgumentOutOfRangeException();
				}
				this._maxAutoRedirects = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000D769 File Offset: 0x0000B969
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000D771 File Offset: 0x0000B971
		public int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException();
				}
				this._timeout = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000D788 File Offset: 0x0000B988
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000D790 File Offset: 0x0000B990
		public bool AutoDetectEncoding
		{
			get
			{
				return this._autoDetectEncoding;
			}
			set
			{
				this._autoDetectEncoding = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000D799 File Offset: 0x0000B999
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0000D7A1 File Offset: 0x0000B9A1
		public Encoding OverrideEncoding
		{
			get
			{
				return this._encoding;
			}
			set
			{
				this._encoding = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000D7AA File Offset: 0x0000B9AA
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000D7B2 File Offset: 0x0000B9B2
		public bool CacheOnly
		{
			get
			{
				return this._cacheOnly;
			}
			set
			{
				if (value && !this.UsingCache)
				{
					throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
				}
				this._cacheOnly = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000D7D1 File Offset: 0x0000B9D1
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000D7D9 File Offset: 0x0000B9D9
		public bool UsingCacheIfExists
		{
			get
			{
				return this._usingCacheIfExists;
			}
			set
			{
				this._usingCacheIfExists = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000D7E2 File Offset: 0x0000B9E2
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0000D7EA File Offset: 0x0000B9EA
		public string CachePath
		{
			get
			{
				return this._cachePath;
			}
			set
			{
				this._cachePath = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000D7F3 File Offset: 0x0000B9F3
		public bool FromCache
		{
			get
			{
				return this._fromCache;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000D7FB File Offset: 0x0000B9FB
		public int RequestDuration
		{
			get
			{
				return this._requestDuration;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000D803 File Offset: 0x0000BA03
		public Uri ResponseUri
		{
			get
			{
				return this._responseUri;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000D80B File Offset: 0x0000BA0B
		public HttpStatusCode StatusCode
		{
			get
			{
				return this._statusCode;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000D813 File Offset: 0x0000BA13
		// (set) Token: 0x060001FC RID: 508 RVA: 0x0000D81B File Offset: 0x0000BA1B
		public int StreamBufferSize
		{
			get
			{
				return this._streamBufferSize;
			}
			set
			{
				if (this._streamBufferSize <= 0)
				{
					throw new ArgumentException("Size must be greater than zero.");
				}
				this._streamBufferSize = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000D838 File Offset: 0x0000BA38
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0000D840 File Offset: 0x0000BA40
		public bool UseCookies
		{
			get
			{
				return this._useCookies;
			}
			set
			{
				this._useCookies = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000D849 File Offset: 0x0000BA49
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000D851 File Offset: 0x0000BA51
		public bool CaptureRedirect { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000D85A File Offset: 0x0000BA5A
		// (set) Token: 0x06000202 RID: 514 RVA: 0x0000D862 File Offset: 0x0000BA62
		public string UserAgent
		{
			get
			{
				return this._userAgent;
			}
			set
			{
				this._userAgent = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000D86B File Offset: 0x0000BA6B
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000D87D File Offset: 0x0000BA7D
		public bool UsingCache
		{
			get
			{
				return this._cachePath != null && this._usingCache;
			}
			set
			{
				if (value && this._cachePath == null)
				{
					throw new HtmlWebException("You need to define a CachePath first.");
				}
				this._usingCache = value;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000D89C File Offset: 0x0000BA9C
		public static string GetContentTypeForExtension(string extension, string def)
		{
			if (string.IsNullOrEmpty(extension))
			{
				return def;
			}
			string result = "";
			if (!extension.StartsWith("."))
			{
				extension = "." + extension;
			}
			if (!MimeTypeMap.Mappings.TryGetValue(extension, ref result))
			{
				result = def;
			}
			return result;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
		public static string GetExtensionForContentType(string contentType, string def)
		{
			if (string.IsNullOrEmpty(contentType))
			{
				return def;
			}
			if (contentType.StartsWith("."))
			{
				throw new ArgumentException("Requested mime type is not valid: " + contentType);
			}
			string result = "";
			if (!MimeTypeMap.Mappings.TryGetValue(contentType, ref result))
			{
				result = def;
			}
			return result;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000D935 File Offset: 0x0000BB35
		public object CreateInstance(string url, Type type)
		{
			return this.CreateInstance(url, null, null, type);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000D941 File Offset: 0x0000BB41
		public void Get(string url, string path)
		{
			this.Get(url, path, "GET");
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000D950 File Offset: 0x0000BB50
		public void Get(string url, string path, WebProxy proxy, NetworkCredential credentials)
		{
			this.Get(url, path, proxy, credentials, "GET");
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000D964 File Offset: 0x0000BB64
		public void Get(string url, string path, string method)
		{
			Uri uri = new Uri(url);
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				this.Get(uri, method, path, null, null, null);
				return;
			}
			throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
		public void Get(string url, string path, WebProxy proxy, NetworkCredential credentials, string method)
		{
			Uri uri = new Uri(url);
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				this.Get(uri, method, path, null, proxy, credentials);
				return;
			}
			throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000DA28 File Offset: 0x0000BC28
		public string GetCachePath(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (!this.UsingCache)
			{
				throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
			}
			string result;
			if (uri.AbsolutePath == "/")
			{
				result = Path.Combine(this._cachePath, ".htm");
			}
			else
			{
				string text = uri.AbsolutePath;
				string text2 = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
				for (int i = 0; i < text2.Length; i++)
				{
					text = text.Replace(text2.get_Chars(i).ToString(), "");
				}
				if (uri.AbsolutePath.get_Chars(uri.AbsolutePath.Length - 1) == Path.AltDirectorySeparatorChar)
				{
					result = Path.Combine(this._cachePath, (uri.Host + text.TrimEnd(Path.AltDirectorySeparatorChar)).Replace('/', '\\') + ".htm");
				}
				else
				{
					result = Path.Combine(this._cachePath, uri.Host + text.Replace('/', '\\'));
				}
			}
			return result;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000DB4A File Offset: 0x0000BD4A
		public HtmlDocument Load(string url)
		{
			return this.Load(url, "GET");
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000DB58 File Offset: 0x0000BD58
		public HtmlDocument Load(Uri uri)
		{
			return this.Load(uri, "GET");
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000DB68 File Offset: 0x0000BD68
		public HtmlDocument Load(string url, string proxyHost, int proxyPort, string userId, string password)
		{
			WebProxy webProxy = new WebProxy(proxyHost, proxyPort);
			webProxy.BypassProxyOnLocal = true;
			NetworkCredential networkCredential = null;
			if (userId != null && password != null)
			{
				networkCredential = new NetworkCredential(userId, password);
				CredentialCache credentialCache = new CredentialCache();
				credentialCache.Add(webProxy.Address, "Basic", networkCredential);
				credentialCache.Add(webProxy.Address, "Digest", networkCredential);
			}
			return this.Load(url, "GET", webProxy, networkCredential);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		public HtmlDocument Load(Uri uri, string proxyHost, int proxyPort, string userId, string password)
		{
			WebProxy webProxy = new WebProxy(proxyHost, proxyPort);
			webProxy.BypassProxyOnLocal = true;
			NetworkCredential networkCredential = null;
			if (userId != null && password != null)
			{
				networkCredential = new NetworkCredential(userId, password);
				CredentialCache credentialCache = new CredentialCache();
				credentialCache.Add(webProxy.Address, "Basic", networkCredential);
				credentialCache.Add(webProxy.Address, "Digest", networkCredential);
			}
			return this.Load(uri, "GET", webProxy, networkCredential);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000DC38 File Offset: 0x0000BE38
		public HtmlDocument Load(string url, string method)
		{
			Uri uri = new Uri(url);
			return this.Load(uri, method);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000DC54 File Offset: 0x0000BE54
		public HtmlDocument Load(Uri uri, string method)
		{
			if (this.UsingCache)
			{
				this._usingCacheAndLoad = true;
			}
			HtmlDocument htmlDocument;
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				htmlDocument = this.LoadUrl(uri, method, null, null);
			}
			else
			{
				if (!(uri.Scheme == Uri.UriSchemeFile))
				{
					throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
				}
				htmlDocument = new HtmlDocument();
				htmlDocument.OptionAutoCloseOnEnd = false;
				htmlDocument.OptionAutoCloseOnEnd = true;
				if (this.OverrideEncoding != null)
				{
					htmlDocument.Load(uri.OriginalString, this.OverrideEncoding);
				}
				else
				{
					htmlDocument.DetectEncodingAndLoad(uri.OriginalString, this._autoDetectEncoding);
				}
			}
			if (this.PreHandleDocument != null)
			{
				this.PreHandleDocument(htmlDocument);
			}
			return htmlDocument;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000DD28 File Offset: 0x0000BF28
		public HtmlDocument Load(string url, string method, WebProxy proxy, NetworkCredential credentials)
		{
			Uri uri = new Uri(url);
			return this.Load(uri, method, proxy, credentials);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000DD48 File Offset: 0x0000BF48
		public HtmlDocument Load(Uri uri, string method, WebProxy proxy, NetworkCredential credentials)
		{
			if (this.UsingCache)
			{
				this._usingCacheAndLoad = true;
			}
			HtmlDocument htmlDocument;
			if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
			{
				htmlDocument = this.LoadUrl(uri, method, proxy, credentials);
			}
			else
			{
				if (!(uri.Scheme == Uri.UriSchemeFile))
				{
					throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
				}
				htmlDocument = new HtmlDocument();
				htmlDocument.OptionAutoCloseOnEnd = false;
				htmlDocument.OptionAutoCloseOnEnd = true;
				htmlDocument.DetectEncodingAndLoad(uri.OriginalString, this._autoDetectEncoding);
			}
			if (this.PreHandleDocument != null)
			{
				this.PreHandleDocument(htmlDocument);
			}
			return htmlDocument;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000DE00 File Offset: 0x0000C000
		public void LoadHtmlAsXml(string htmlUrl, XmlTextWriter writer)
		{
			this.Load(htmlUrl).Save(writer);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000DE10 File Offset: 0x0000C010
		private static void FilePreparePath(string target)
		{
			if (File.Exists(target))
			{
				FileAttributes attributes = File.GetAttributes(target);
				File.SetAttributes(target, attributes & -2);
				return;
			}
			string directoryName = Path.GetDirectoryName(target);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000DE4D File Offset: 0x0000C04D
		private static DateTime RemoveMilliseconds(DateTime t)
		{
			return new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, 0);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000DE80 File Offset: 0x0000C080
		private static DateTime RemoveMilliseconds(DateTimeOffset? offset)
		{
			DateTimeOffset dateTimeOffset = offset ?? DateTimeOffset.Now;
			return new DateTime(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, dateTimeOffset.Hour, dateTimeOffset.Minute, dateTimeOffset.Second, 0);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000DED8 File Offset: 0x0000C0D8
		private static long SaveStream(Stream stream, string path, DateTime touchDate, int streamBufferSize)
		{
			HtmlWeb.FilePreparePath(path);
			long num = 0L;
			using (FileStream fileStream = new FileStream(path, 2, 2))
			{
				using (BinaryReader binaryReader = new BinaryReader(stream))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
					{
						byte[] array;
						do
						{
							array = binaryReader.ReadBytes(streamBufferSize);
							num += (long)array.Length;
							if (array.Length != 0)
							{
								binaryWriter.Write(array);
							}
						}
						while (array.Length != 0);
						binaryWriter.Flush();
					}
				}
			}
			File.SetLastWriteTime(path, touchDate);
			return num;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000DF80 File Offset: 0x0000C180
		private HttpStatusCode Get(Uri uri, string method, string path, HtmlDocument doc, IWebProxy proxy, ICredentials creds)
		{
			string text = null;
			bool flag = false;
			HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
			if (this.MaxAutoRedirects != null)
			{
				httpWebRequest.MaximumAutomaticRedirections = this.MaxAutoRedirects.Value;
			}
			httpWebRequest.Timeout = this.Timeout;
			httpWebRequest.Method = method;
			httpWebRequest.UserAgent = this.UserAgent;
			httpWebRequest.AutomaticDecompression = this.AutomaticDecompression;
			if (this.CaptureRedirect)
			{
				httpWebRequest.AllowAutoRedirect = false;
			}
			if (proxy != null)
			{
				if (creds != null)
				{
					proxy.Credentials = creds;
					httpWebRequest.Credentials = creds;
				}
				else
				{
					proxy.Credentials = CredentialCache.DefaultCredentials;
					httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
				}
				httpWebRequest.Proxy = proxy;
			}
			this._fromCache = false;
			this._requestDuration = 0;
			int tickCount = Environment.TickCount;
			if (this.UsingCache)
			{
				text = this.GetCachePath(httpWebRequest.RequestUri);
				if (File.Exists(text))
				{
					httpWebRequest.IfModifiedSince = File.GetLastAccessTime(text);
					flag = true;
				}
			}
			if (this._cacheOnly || this._usingCacheIfExists)
			{
				if (File.Exists(text))
				{
					if (path != null)
					{
						IOLibrary.CopyAlways(text, path);
						if (text != null)
						{
							File.SetLastWriteTime(path, File.GetLastWriteTime(text));
						}
					}
					this._fromCache = true;
					return 304;
				}
				if (this._cacheOnly)
				{
					throw new HtmlWebException("File was not found at cache path: '" + text + "'");
				}
			}
			if (this._useCookies)
			{
				httpWebRequest.CookieContainer = new CookieContainer();
			}
			if (this.PreRequest != null && !this.PreRequest(httpWebRequest))
			{
				return 205;
			}
			HttpWebResponse httpWebResponse;
			try
			{
				httpWebResponse = (httpWebRequest.GetResponse() as HttpWebResponse);
			}
			catch (WebException ex)
			{
				this._requestDuration = Environment.TickCount - tickCount;
				httpWebResponse = (HttpWebResponse)ex.Response;
				if (httpWebResponse == null)
				{
					if (flag)
					{
						if (path != null)
						{
							IOLibrary.CopyAlways(text, path);
							File.SetLastWriteTime(path, File.GetLastWriteTime(text));
						}
						return 304;
					}
					throw;
				}
			}
			catch (Exception)
			{
				this._requestDuration = Environment.TickCount - tickCount;
				throw;
			}
			if (this.PostResponse != null)
			{
				this.PostResponse(httpWebRequest, httpWebResponse);
			}
			this._requestDuration = Environment.TickCount - tickCount;
			this._responseUri = httpWebResponse.ResponseUri;
			HttpStatusCode statusCode = httpWebResponse.StatusCode;
			bool flag2 = HtmlWeb.IsHtmlContent(httpWebResponse.ContentType);
			bool flag3 = string.IsNullOrEmpty(httpWebResponse.ContentType);
			string text2 = "";
			if (!string.IsNullOrEmpty(httpWebResponse.CharacterSet))
			{
				text2 = httpWebResponse.CharacterSet.Replace("\"", "");
			}
			Encoding encoding = null;
			try
			{
				encoding = ((!string.IsNullOrEmpty(flag2 ? text2 : httpWebResponse.ContentEncoding)) ? Encoding.GetEncoding(flag2 ? text2 : httpWebResponse.ContentEncoding) : null);
			}
			catch (Exception)
			{
			}
			if (this.OverrideEncoding != null)
			{
				encoding = this.OverrideEncoding;
			}
			if (this.CaptureRedirect && httpWebResponse.StatusCode == 302)
			{
				string text3 = httpWebResponse.Headers["Location"];
				Uri uri2;
				if (!Uri.TryCreate(text3, 1, ref uri2))
				{
					uri2 = new Uri(uri, text3);
				}
				return this.Get(uri2, "GET", path, doc, proxy, creds);
			}
			if (httpWebResponse.StatusCode != 304)
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				if (responseStream != null)
				{
					if (this.UsingCache)
					{
						HtmlWeb.SaveStream(responseStream, text, HtmlWeb.RemoveMilliseconds(httpWebResponse.LastModified), this._streamBufferSize);
						this.SaveCacheHeaders(httpWebRequest.RequestUri, httpWebResponse);
						if (path != null)
						{
							IOLibrary.CopyAlways(text, path);
							File.SetLastWriteTime(path, File.GetLastWriteTime(text));
						}
						if (this._usingCacheAndLoad)
						{
							doc.Load(text);
						}
					}
					else
					{
						if (doc != null && flag2)
						{
							if (encoding == null)
							{
								doc.Load(responseStream, true);
							}
							else
							{
								doc.Load(responseStream, encoding);
							}
						}
						if (doc != null && flag3)
						{
							try
							{
								if (encoding == null)
								{
									doc.Load(responseStream, true);
								}
								else
								{
									doc.Load(responseStream, encoding);
								}
							}
							catch
							{
							}
						}
					}
					httpWebResponse.Close();
				}
				return statusCode;
			}
			if (this.UsingCache)
			{
				this._fromCache = true;
				if (path != null)
				{
					IOLibrary.CopyAlways(text, path);
					File.SetLastWriteTime(path, File.GetLastWriteTime(text));
				}
				return httpWebResponse.StatusCode;
			}
			throw new HtmlWebException("Server has send a NotModifed code, without cache enabled.");
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000E3C4 File Offset: 0x0000C5C4
		private string GetCacheHeader(Uri requestUri, string name, string def)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.GetCacheHeadersPath(requestUri));
			XmlNode xmlNode = xmlDocument.SelectSingleNode("//h[translate(@n, 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')='" + name.ToUpperInvariant() + "']");
			if (xmlNode == null)
			{
				return def;
			}
			return xmlNode.Attributes.get_ItemOf(name).Value;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000E414 File Offset: 0x0000C614
		private string GetCacheHeadersPath(Uri uri)
		{
			return this.GetCachePath(uri) + ".h.xml";
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E427 File Offset: 0x0000C627
		private bool IsCacheHtmlContent(string path)
		{
			return HtmlWeb.IsHtmlContent(HtmlWeb.GetContentTypeForExtension(Path.GetExtension(path), null));
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000E43A File Offset: 0x0000C63A
		private static bool IsHtmlContent(string contentType)
		{
			return contentType.StartsWith("text/html", 5);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000E448 File Offset: 0x0000C648
		private static bool IsGZipEncoding(string contentEncoding)
		{
			return contentEncoding.StartsWith("gzip", 5);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000E458 File Offset: 0x0000C658
		private HtmlDocument LoadUrl(Uri uri, string method, WebProxy proxy, NetworkCredential creds)
		{
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.OptionAutoCloseOnEnd = false;
			htmlDocument.OptionFixNestedTags = true;
			this._statusCode = this.Get(uri, method, null, htmlDocument, proxy, creds);
			if (this._statusCode == 304)
			{
				htmlDocument.DetectEncodingAndLoad(this.GetCachePath(uri));
			}
			return htmlDocument;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000E4A8 File Offset: 0x0000C6A8
		private void SaveCacheHeaders(Uri requestUri, HttpWebResponse resp)
		{
			string cacheHeadersPath = this.GetCacheHeadersPath(requestUri);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<c></c>");
			XmlNode firstChild = xmlDocument.FirstChild;
			foreach (object obj in resp.Headers)
			{
				string text = (string)obj;
				XmlNode xmlNode = xmlDocument.CreateElement("h");
				XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("n");
				xmlAttribute.Value = text;
				xmlNode.Attributes.Append(xmlAttribute);
				xmlAttribute = xmlDocument.CreateAttribute("v");
				xmlAttribute.Value = resp.Headers[text];
				xmlNode.Attributes.Append(xmlAttribute);
				firstChild.AppendChild(xmlNode);
			}
			xmlDocument.Save(cacheHeadersPath);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000E590 File Offset: 0x0000C790
		public Task<HtmlDocument> LoadFromWebAsync(string url)
		{
			return this.LoadFromWebAsync(new Uri(url), null, null);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public Task<HtmlDocument> LoadFromWebAsync(string url, CancellationToken cancellationToken)
		{
			return this.LoadFromWebAsync(new Uri(url), null, null, cancellationToken);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000E5B1 File Offset: 0x0000C7B1
		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding)
		{
			return this.LoadFromWebAsync(new Uri(url), encoding, null, CancellationToken.None);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000E5C6 File Offset: 0x0000C7C6
		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, CancellationToken cancellationToken)
		{
			return this.LoadFromWebAsync(new Uri(url), encoding, null, cancellationToken);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000E5D7 File Offset: 0x0000C7D7
		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password)
		{
			return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password), CancellationToken.None);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000E5F3 File Offset: 0x0000C7F3
		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password, CancellationToken cancellationToken)
		{
			return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password), cancellationToken);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000E60C File Offset: 0x0000C80C
		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password, string domain)
		{
			return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password, domain), CancellationToken.None);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000E62A File Offset: 0x0000C82A
		public Task<HtmlDocument> LoadFromWebAsync(string url, Encoding encoding, string userName, string password, string domain, CancellationToken cancellationToken)
		{
			return this.LoadFromWebAsync(new Uri(url), encoding, new NetworkCredential(userName, password, domain), cancellationToken);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000E645 File Offset: 0x0000C845
		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password, string domain)
		{
			return this.LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password, domain), CancellationToken.None);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000E662 File Offset: 0x0000C862
		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password, string domain, CancellationToken cancellationToken)
		{
			return this.LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password, domain), cancellationToken);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000E67C File Offset: 0x0000C87C
		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password)
		{
			return this.LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password), CancellationToken.None);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E697 File Offset: 0x0000C897
		public Task<HtmlDocument> LoadFromWebAsync(string url, string userName, string password, CancellationToken cancellationToken)
		{
			return this.LoadFromWebAsync(new Uri(url), null, new NetworkCredential(userName, password), cancellationToken);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E6AF File Offset: 0x0000C8AF
		public Task<HtmlDocument> LoadFromWebAsync(string url, NetworkCredential credentials)
		{
			return this.LoadFromWebAsync(new Uri(url), null, credentials, CancellationToken.None);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E6C4 File Offset: 0x0000C8C4
		public Task<HtmlDocument> LoadFromWebAsync(string url, NetworkCredential credentials, CancellationToken cancellationToken)
		{
			return this.LoadFromWebAsync(new Uri(url), null, credentials, cancellationToken);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E6D5 File Offset: 0x0000C8D5
		public Task<HtmlDocument> LoadFromWebAsync(Uri uri, Encoding encoding, NetworkCredential credentials)
		{
			return this.LoadFromWebAsync(uri, encoding, credentials, CancellationToken.None);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E6E8 File Offset: 0x0000C8E8
		public Task<HtmlDocument> LoadFromWebAsync(Uri uri, Encoding encoding, NetworkCredential credentials, CancellationToken cancellationToken)
		{
			HtmlWeb.<LoadFromWebAsync>d__117 <LoadFromWebAsync>d__;
			<LoadFromWebAsync>d__.<>t__builder = AsyncTaskMethodBuilder<HtmlDocument>.Create();
			<LoadFromWebAsync>d__.<>4__this = this;
			<LoadFromWebAsync>d__.uri = uri;
			<LoadFromWebAsync>d__.encoding = encoding;
			<LoadFromWebAsync>d__.credentials = credentials;
			<LoadFromWebAsync>d__.cancellationToken = cancellationToken;
			<LoadFromWebAsync>d__.<>1__state = -1;
			<LoadFromWebAsync>d__.<>t__builder.Start<HtmlWeb.<LoadFromWebAsync>d__117>(ref <LoadFromWebAsync>d__);
			return <LoadFromWebAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E74C File Offset: 0x0000C94C
		public object CreateInstance(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, Type type)
		{
			return this.CreateInstance(htmlUrl, xsltUrl, xsltArgs, type, null);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000E75C File Offset: 0x0000C95C
		public object CreateInstance(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, Type type, string xmlPath)
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			if (xsltUrl == null)
			{
				this.LoadHtmlAsXml(htmlUrl, xmlTextWriter);
			}
			else if (xmlPath == null)
			{
				this.LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, xmlTextWriter);
			}
			else
			{
				this.LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, xmlTextWriter, xmlPath);
			}
			xmlTextWriter.Flush();
			XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(stringWriter.ToString()));
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			object result;
			try
			{
				result = xmlSerializer.Deserialize(xmlTextReader);
			}
			catch (InvalidOperationException ex)
			{
				string text = (ex != null) ? ex.ToString() : null;
				string text2 = ", --- xml:";
				StringWriter stringWriter2 = stringWriter;
				throw new Exception(text + text2 + ((stringWriter2 != null) ? stringWriter2.ToString() : null));
			}
			return result;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000E804 File Offset: 0x0000CA04
		public void LoadHtmlAsXml(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, XmlTextWriter writer)
		{
			this.LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, writer, null);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E814 File Offset: 0x0000CA14
		public void LoadHtmlAsXml(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, XmlTextWriter writer, string xmlPath)
		{
			if (htmlUrl == null)
			{
				throw new ArgumentNullException("htmlUrl");
			}
			HtmlDocument htmlDocument = this.Load(htmlUrl);
			if (xmlPath != null)
			{
				XmlTextWriter xmlTextWriter = new XmlTextWriter(xmlPath, htmlDocument.Encoding);
				htmlDocument.Save(xmlTextWriter);
				xmlTextWriter.Close();
			}
			if (xsltArgs == null)
			{
				xsltArgs = new XsltArgumentList();
			}
			xsltArgs.AddParam("url", "", htmlUrl);
			xsltArgs.AddParam("requestDuration", "", this.RequestDuration);
			xsltArgs.AddParam("fromCache", "", this.FromCache);
			XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
			xslCompiledTransform.Load(xsltUrl);
			xslCompiledTransform.Transform(htmlDocument, xsltArgs, writer);
		}

		// Token: 0x040000AC RID: 172
		private bool _autoDetectEncoding = true;

		// Token: 0x040000AD RID: 173
		private bool _cacheOnly;

		// Token: 0x040000AE RID: 174
		private string _cachePath;

		// Token: 0x040000AF RID: 175
		private bool _fromCache;

		// Token: 0x040000B0 RID: 176
		private int _requestDuration;

		// Token: 0x040000B1 RID: 177
		private Uri _responseUri;

		// Token: 0x040000B2 RID: 178
		private HttpStatusCode _statusCode = 200;

		// Token: 0x040000B3 RID: 179
		private int _streamBufferSize = 1024;

		// Token: 0x040000B4 RID: 180
		private bool _useCookies;

		// Token: 0x040000B5 RID: 181
		private bool _usingCache;

		// Token: 0x040000B6 RID: 182
		private bool _usingCacheAndLoad;

		// Token: 0x040000B7 RID: 183
		private bool _usingCacheIfExists;

		// Token: 0x040000B8 RID: 184
		private string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:x.x.x) Gecko/20041107 Firefox/x.x";

		// Token: 0x040000B9 RID: 185
		private int _timeout = 100000;

		// Token: 0x040000BA RID: 186
		private int? _maxAutoRedirects;

		// Token: 0x040000BB RID: 187
		public HtmlWeb.PostResponseHandler PostResponse;

		// Token: 0x040000BC RID: 188
		public HtmlWeb.PreHandleDocumentHandler PreHandleDocument;

		// Token: 0x040000BD RID: 189
		public HtmlWeb.PreRequestHandler PreRequest;

		// Token: 0x040000BE RID: 190
		internal static ConcurrentDictionary<string, HttpClient> SharedHttpClient = new ConcurrentDictionary<string, HttpClient>();

		// Token: 0x040000C0 RID: 192
		private Encoding _encoding;

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x06000338 RID: 824
		[NullableContext(0)]
		public delegate void PostResponseHandler(HttpWebRequest request, HttpWebResponse response);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x0600033C RID: 828
		[NullableContext(0)]
		public delegate void PreHandleDocumentHandler(HtmlDocument document);

		// Token: 0x0200004C RID: 76
		// (Invoke) Token: 0x06000340 RID: 832
		[NullableContext(0)]
		public delegate bool PreRequestHandler(HttpWebRequest request);
	}
}
