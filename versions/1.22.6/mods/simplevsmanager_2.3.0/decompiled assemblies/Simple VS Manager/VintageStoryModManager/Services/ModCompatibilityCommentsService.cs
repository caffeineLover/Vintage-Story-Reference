using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModCompatibilityCommentsService
	{
		// Token: 0x060009CC RID: 2508 RVA: 0x0003B07C File Offset: 0x0003927C
		static ModCompatibilityCommentsService()
		{
			if (!Enumerable.Any<ProductInfoHeaderValue>(ModCompatibilityCommentsService.HttpClient.DefaultRequestHeaders.UserAgent))
			{
				ModCompatibilityCommentsService.HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("SimpleVSManager", "1.0"));
				ModCompatibilityCommentsService.HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ExperimentalCompReview", "1.0"));
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0003B26C File Offset: 0x0003946C
		public Task<ModCompatibilityCommentsService.ExperimentalCompReviewResult> GetTop3CommentsAsync(string modSlug, [Nullable(2)] string latestVsVersion, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModCompatibilityCommentsService.<GetTop3CommentsAsync>d__8 <GetTop3CommentsAsync>d__;
			<GetTop3CommentsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModCompatibilityCommentsService.ExperimentalCompReviewResult>.Create();
			<GetTop3CommentsAsync>d__.modSlug = modSlug;
			<GetTop3CommentsAsync>d__.latestVsVersion = latestVsVersion;
			<GetTop3CommentsAsync>d__.cancellationToken = cancellationToken;
			<GetTop3CommentsAsync>d__.<>1__state = -1;
			<GetTop3CommentsAsync>d__.<>t__builder.Start<ModCompatibilityCommentsService.<GetTop3CommentsAsync>d__8>(ref <GetTop3CommentsAsync>d__);
			return <GetTop3CommentsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0003B2BF File Offset: 0x000394BF
		public static string SerializeResult(ModCompatibilityCommentsService.ExperimentalCompReviewResult result)
		{
			ArgumentNullException.ThrowIfNull(result, "result");
			return JsonSerializer.Serialize<ModCompatibilityCommentsService.ExperimentalCompReviewResult>(result, ModCompatibilityCommentsService.JsonOptions);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0003B2D8 File Offset: 0x000394D8
		private static string NormalizeSlug(string slug)
		{
			string text = slug.Trim();
			Uri uri;
			if ((text.StartsWith("http://", 5) || text.StartsWith("https://", 5)) && Uri.TryCreate(text, 1, ref uri))
			{
				text = uri.AbsolutePath;
			}
			text = text.Trim('/');
			int num = text.IndexOf('?', 4);
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			int num2 = text.IndexOf('#', 4);
			if (num2 >= 0)
			{
				text = text.Substring(0, num2);
			}
			if (text.StartsWith("show/mod/", 5))
			{
				text = text.Substring("show/mod/".Length);
			}
			return text;
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0003B374 File Offset: 0x00039574
		private static Task<ModCompatibilityCommentsService.ModMetadata> FetchModMetadataAsync(string normalizedSlug, CancellationToken cancellationToken)
		{
			ModCompatibilityCommentsService.<FetchModMetadataAsync>d__11 <FetchModMetadataAsync>d__;
			<FetchModMetadataAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModCompatibilityCommentsService.ModMetadata>.Create();
			<FetchModMetadataAsync>d__.normalizedSlug = normalizedSlug;
			<FetchModMetadataAsync>d__.cancellationToken = cancellationToken;
			<FetchModMetadataAsync>d__.<>1__state = -1;
			<FetchModMetadataAsync>d__.<>t__builder.Start<ModCompatibilityCommentsService.<FetchModMetadataAsync>d__11>(ref <FetchModMetadataAsync>d__);
			return <FetchModMetadataAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0003B3C0 File Offset: 0x000395C0
		private static int? TryGetStatusCode(JsonElement rootElement)
		{
			JsonElement jsonElement;
			if (!rootElement.TryGetProperty("statuscode", ref jsonElement))
			{
				return default(int?);
			}
			int num;
			if (jsonElement.ValueKind == 4 && jsonElement.TryGetInt32(ref num))
			{
				return new int?(num);
			}
			int num2;
			if (jsonElement.ValueKind == 3 && int.TryParse(jsonElement.GetString(), 7, CultureInfo.InvariantCulture, ref num2))
			{
				return new int?(num2);
			}
			return default(int?);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0003B434 File Offset: 0x00039634
		private static Task<string> FetchCommentsHtmlAsync(string pagePath, CancellationToken cancellationToken)
		{
			ModCompatibilityCommentsService.<FetchCommentsHtmlAsync>d__13 <FetchCommentsHtmlAsync>d__;
			<FetchCommentsHtmlAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<FetchCommentsHtmlAsync>d__.pagePath = pagePath;
			<FetchCommentsHtmlAsync>d__.cancellationToken = cancellationToken;
			<FetchCommentsHtmlAsync>d__.<>1__state = -1;
			<FetchCommentsHtmlAsync>d__.<>t__builder.Start<ModCompatibilityCommentsService.<FetchCommentsHtmlAsync>d__13>(ref <FetchCommentsHtmlAsync>d__);
			return <FetchCommentsHtmlAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0003B480 File Offset: 0x00039680
		private static IReadOnlyList<ModCompatibilityCommentsService.Comment> ExtractComments(string html, string pagePath, DateTime lastReleaseUtc)
		{
			if (string.IsNullOrWhiteSpace(html))
			{
				return Array.Empty<ModCompatibilityCommentsService.Comment>();
			}
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);
			HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'comment') and @data-timestamp]");
			if (htmlNodeCollection == null || htmlNodeCollection.Count == 0)
			{
				return Array.Empty<ModCompatibilityCommentsService.Comment>();
			}
			List<ModCompatibilityCommentsService.Comment> list = new List<ModCompatibilityCommentsService.Comment>(htmlNodeCollection.Count);
			foreach (HtmlNode htmlNode in htmlNodeCollection)
			{
				DateTime? dateTime = ModCompatibilityCommentsService.TryGetTimestampUtc(htmlNode);
				if (dateTime != null && !(dateTime.Value < lastReleaseUtc))
				{
					HtmlNode htmlNode2 = htmlNode.SelectSingleNode(".//div[contains(@class,'body')]");
					if (htmlNode2 != null)
					{
						string text = HtmlEntity.DeEntitize(htmlNode2.InnerText ?? string.Empty).Trim();
						if (!string.IsNullOrWhiteSpace(text))
						{
							HtmlNode htmlNode3 = htmlNode.SelectSingleNode(".//div[contains(@class,'title')]");
							HtmlNode htmlNode4 = (htmlNode3 != null) ? htmlNode3.SelectSingleNode(".//a[contains(@href,'/show/user')]") : null;
							string author = string.IsNullOrWhiteSpace((htmlNode4 != null) ? htmlNode4.InnerText : null) ? "Unknown" : HtmlEntity.DeEntitize(htmlNode4.InnerText).Trim();
							bool flag;
							if (htmlNode3 == null)
							{
								flag = false;
							}
							else
							{
								string innerText = htmlNode3.InnerText;
								int? num = (innerText != null) ? new int?(innerText.IndexOf("Author", 5)) : default(int?);
								int num2 = 0;
								flag = (num.GetValueOrDefault() >= num2 & num != null);
							}
							bool isModAuthor = flag;
							string permalink = ModCompatibilityCommentsService.BuildPermalink(htmlNode, pagePath);
							list.Add(new ModCompatibilityCommentsService.Comment
							{
								Author = author,
								IsModAuthor = isModAuthor,
								TimestampUtc = dateTime.Value,
								Text = ModCompatibilityCommentsService.SquashWhitespace(text),
								Permalink = permalink
							});
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0003B660 File Offset: 0x00039860
		private static IReadOnlyList<ModCompatibilityCommentsService.ScoredComment> ScoreComments(IReadOnlyList<ModCompatibilityCommentsService.Comment> comments, [Nullable(2)] string latestVsVersion)
		{
			if (comments.Count == 0)
			{
				return Array.Empty<ModCompatibilityCommentsService.ScoredComment>();
			}
			List<ModCompatibilityCommentsService.ScoredComment> list = new List<ModCompatibilityCommentsService.ScoredComment>(comments.Count);
			foreach (ModCompatibilityCommentsService.Comment comment in comments)
			{
				Dictionary<string, double> dictionary = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
				double num = 0.0;
				string text = comment.Text.ToLowerInvariant();
				bool hasExactVersion = false;
				if (!string.IsNullOrEmpty(latestVsVersion))
				{
					if (ModCompatibilityCommentsService.MentionsExact(latestVsVersion, text))
					{
						num += 2.5;
						dictionary["exactVersion"] = 2.5;
						hasExactVersion = true;
					}
					else if (ModCompatibilityCommentsService.MentionsSameMinor(latestVsVersion, text))
					{
						num += 1.5;
						dictionary["sameMinor"] = 1.5;
					}
				}
				int num2 = ModCompatibilityCommentsService.CountMatches(text, ModCompatibilityCommentsService.PositiveCues, 3);
				if (num2 > 0)
				{
					num += (double)num2;
					dictionary["positiveCues"] = (double)num2;
				}
				int num3 = ModCompatibilityCommentsService.CountMatches(text, ModCompatibilityCommentsService.NegativeCues, 3);
				if (num3 > 0)
				{
					num -= (double)num3;
					dictionary["negativeCues"] = (double)(-(double)num3);
				}
				if (ModCompatibilityCommentsService.HasReproDetails(text))
				{
					num += 1.0;
					dictionary["details"] = 1.0;
				}
				if (comment.IsModAuthor)
				{
					num += 0.5;
					dictionary["authorBoost"] = 0.5;
				}
				if (text.Length < 20)
				{
					num -= 0.5;
					dictionary["lengthPenalty"] = -0.5;
				}
				if (ModCompatibilityCommentsService.IsOffTopic(text))
				{
					num -= 0.5;
					dictionary["offtopicPenalty"] = -0.5;
				}
				if (ModCompatibilityCommentsService.HasMeaningfulSignal(num, hasExactVersion, dictionary))
				{
					double relevance = Math.Abs(num);
					string polarity = (num > 0.0) ? "positive" : ((num < 0.0) ? "negative" : "mixed/unclear");
					list.Add(new ModCompatibilityCommentsService.ScoredComment(comment, num, relevance, polarity, dictionary, hasExactVersion));
				}
			}
			return Enumerable.ToList<ModCompatibilityCommentsService.ScoredComment>(Enumerable.ThenByDescending<ModCompatibilityCommentsService.ScoredComment, int>(Enumerable.ThenByDescending<ModCompatibilityCommentsService.ScoredComment, bool>(Enumerable.ThenByDescending<ModCompatibilityCommentsService.ScoredComment, DateTime>(Enumerable.OrderByDescending<ModCompatibilityCommentsService.ScoredComment, double>(Enumerable.ToList<ModCompatibilityCommentsService.ScoredComment>(Enumerable.Select<IGrouping<string, ModCompatibilityCommentsService.ScoredComment>, ModCompatibilityCommentsService.ScoredComment>(Enumerable.GroupBy<ModCompatibilityCommentsService.ScoredComment, string>(list, (ModCompatibilityCommentsService.ScoredComment c) => ModCompatibilityCommentsService.NormalizeForDedup(c.Base.Text)), (IGrouping<string, ModCompatibilityCommentsService.ScoredComment> group) => Enumerable.First<ModCompatibilityCommentsService.ScoredComment>(Enumerable.ThenByDescending<ModCompatibilityCommentsService.ScoredComment, DateTime>(Enumerable.OrderByDescending<ModCompatibilityCommentsService.ScoredComment, double>(group, (ModCompatibilityCommentsService.ScoredComment item) => item.Relevance), (ModCompatibilityCommentsService.ScoredComment item) => item.Base.TimestampUtc)))), (ModCompatibilityCommentsService.ScoredComment item) => item.Relevance), (ModCompatibilityCommentsService.ScoredComment item) => item.Base.TimestampUtc), (ModCompatibilityCommentsService.ScoredComment item) => item.HasExactVersion), (ModCompatibilityCommentsService.ScoredComment item) => item.Base.Text.Length));
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0003B998 File Offset: 0x00039B98
		private static bool HasMeaningfulSignal(double score, bool hasExactVersion, IReadOnlyDictionary<string, double> breakdown)
		{
			return hasExactVersion || (breakdown.ContainsKey("sameMinor") || breakdown.ContainsKey("positiveCues") || breakdown.ContainsKey("negativeCues") || breakdown.ContainsKey("details")) || Math.Abs(score) >= 1.0;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0003B9F4 File Offset: 0x00039BF4
		private static ModCompatibilityCommentsService.ExperimentalCompReviewComment BuildCommentResult(ModCompatibilityCommentsService.ScoredComment comment)
		{
			return new ModCompatibilityCommentsService.ExperimentalCompReviewComment
			{
				TimestampUtc = comment.Base.TimestampUtc.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture),
				Author = comment.Base.Author,
				IsModAuthor = comment.Base.IsModAuthor,
				Polarity = comment.Polarity,
				Relevance = Math.Round(comment.Relevance, 2),
				ScoreBreakdown = comment.Breakdown,
				Snippet = ModCompatibilityCommentsService.TrimOneLine(comment.Base.Text, 280),
				Permalink = comment.Base.Permalink,
				WhyPicked = ModCompatibilityCommentsService.BuildWhyPicked(comment)
			};
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0003BAB8 File Offset: 0x00039CB8
		private static DateTime? TryGetTimestampUtc(HtmlNode node)
		{
			string attributeValue = node.GetAttributeValue("data-timestamp", string.Empty);
			long num;
			if (!string.IsNullOrWhiteSpace(attributeValue) && long.TryParse(attributeValue, 7, CultureInfo.InvariantCulture, ref num))
			{
				try
				{
					return new DateTime?(DateTimeOffset.FromUnixTimeSeconds(num).UtcDateTime);
				}
				catch (ArgumentOutOfRangeException)
				{
				}
			}
			HtmlNode htmlNode = node.SelectSingleNode(".//div[contains(@class,'title')]//span[@title]");
			string text = (htmlNode != null) ? htmlNode.GetAttributeValue("title", string.Empty) : null;
			DateTime dateTime;
			if (!string.IsNullOrWhiteSpace(text) && DateTime.TryParse(text, CultureInfo.InvariantCulture, 80, ref dateTime))
			{
				return new DateTime?(dateTime.ToUniversalTime());
			}
			DateTime? result = ModCompatibilityCommentsService.ParseRelativeTimestamp(HtmlEntity.DeEntitize(((htmlNode != null) ? htmlNode.InnerText : null) ?? string.Empty));
			if (result != null)
			{
				return result;
			}
			return default(DateTime?);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0003BBA0 File Offset: 0x00039DA0
		[NullableContext(2)]
		private static DateTime? ParseRelativeTimestamp(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return default(DateTime?);
			}
			Match match = Regex.Match(text, "\\b(\\d+)\\s+(minute|hour|day|week|month|year)s?\\s+ago\\b", 1);
			if (!match.Success)
			{
				return default(DateTime?);
			}
			int num;
			if (!int.TryParse(match.Groups[1].Value, 7, CultureInfo.InvariantCulture, ref num))
			{
				return default(DateTime?);
			}
			string text2 = match.Groups[2].Value.ToLowerInvariant();
			DateTimeOffset utcNow = DateTimeOffset.UtcNow;
			DateTime? result;
			if (!(text2 == "minute"))
			{
				if (!(text2 == "hour"))
				{
					if (!(text2 == "day"))
					{
						if (!(text2 == "week"))
						{
							if (!(text2 == "month"))
							{
								if (!(text2 == "year"))
								{
									result = default(DateTime?);
								}
								else
								{
									result = new DateTime?(utcNow.AddYears(-num).UtcDateTime);
								}
							}
							else
							{
								result = new DateTime?(utcNow.AddMonths(-num).UtcDateTime);
							}
						}
						else
						{
							result = new DateTime?(utcNow.AddDays((double)(-7 * num)).UtcDateTime);
						}
					}
					else
					{
						result = new DateTime?(utcNow.AddDays((double)(-(double)num)).UtcDateTime);
					}
				}
				else
				{
					result = new DateTime?(utcNow.AddHours((double)(-(double)num)).UtcDateTime);
				}
			}
			else
			{
				result = new DateTime?(utcNow.AddMinutes((double)(-(double)num)).UtcDateTime);
			}
			return result;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0003BD3C File Offset: 0x00039F3C
		[return: Nullable(2)]
		private static string BuildPermalink(HtmlNode node, string pagePath)
		{
			string attributeValue = node.GetAttributeValue("id", string.Empty);
			if (!string.IsNullOrWhiteSpace(attributeValue))
			{
				return string.Format(CultureInfo.InvariantCulture, "https://mods.vintagestory.at/{0}#{1}", pagePath, attributeValue);
			}
			HtmlNode htmlNode = node.SelectSingleNode(".//a[starts-with(@href,'#cmt-')]");
			string text = (htmlNode != null) ? htmlNode.GetAttributeValue("href", string.Empty) : null;
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			if (text.StartsWith("#", 4))
			{
				return string.Format(CultureInfo.InvariantCulture, "https://mods.vintagestory.at/{0}{1}", pagePath, text);
			}
			if (text.StartsWith("/", 4))
			{
				return "https://mods.vintagestory.at" + text;
			}
			return text;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0003BDDB File Offset: 0x00039FDB
		private static bool MentionsExact(string latestVsVersion, string textLower)
		{
			return Regex.IsMatch(textLower, Regex.Escape(latestVsVersion.Trim()), 1);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0003BDF0 File Offset: 0x00039FF0
		private static bool MentionsSameMinor(string latestVsVersion, string textLower)
		{
			Match match = Regex.Match(latestVsVersion, "^(?<major>\\d+)\\.(?<minor>\\d+)");
			if (!match.Success)
			{
				return false;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 2);
			defaultInterpolatedStringHandler.AppendLiteral("\\b");
			defaultInterpolatedStringHandler.AppendFormatted(match.Groups["major"].Value);
			defaultInterpolatedStringHandler.AppendLiteral("\\.");
			defaultInterpolatedStringHandler.AppendFormatted(match.Groups["minor"].Value);
			defaultInterpolatedStringHandler.AppendLiteral("\\b");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			return Regex.IsMatch(textLower, text, 1);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0003BE88 File Offset: 0x0003A088
		private static int CountMatches(string textLower, IEnumerable<string> cues, int cap)
		{
			int num = 0;
			foreach (string text in cues)
			{
				if (textLower.Contains(text, 5))
				{
					num++;
					if (num >= cap)
					{
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0003BEE0 File Offset: 0x0003A0E0
		private static bool HasReproDetails(string textLower)
		{
			return !string.IsNullOrEmpty(textLower) && (textLower.Contains("stack trace", 5) || textLower.Contains("stacktrace", 5) || textLower.Contains("json error", 5) || textLower.Contains("nullref", 5) || textLower.Contains("null reference", 5) || (Regex.IsMatch(textLower, "\\bexception\\b", 1) || Regex.IsMatch(textLower, "\\berror code\\b", 1)) || (Regex.IsMatch(textLower, "\\bwhen i\\b", 1) || Regex.IsMatch(textLower, "\\bwhen opening\\b", 1) || Regex.IsMatch(textLower, "\\bwhen loading\\b", 1)) || (textLower.Contains("server", 5) && (textLower.Contains("client", 5) || textLower.Contains("multiplayer", 5))));
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0003BFB8 File Offset: 0x0003A1B8
		private static bool IsOffTopic(string textLower)
		{
			bool flag = ModCompatibilityCommentsService.VersionRegex.IsMatch(textLower);
			bool flag2 = Enumerable.Any<string>(ModCompatibilityCommentsService.PositiveCues, (string cue) => textLower.Contains(cue, 5)) || Enumerable.Any<string>(ModCompatibilityCommentsService.NegativeCues, (string cue) => textLower.Contains(cue, 5));
			return !flag && !flag2;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0003C01C File Offset: 0x0003A21C
		private static string NormalizeForDedup(string text)
		{
			string text2 = text.ToLowerInvariant();
			text2 = Regex.Replace(text2, "\\s+", " ").Trim();
			if (text2.Length <= 300)
			{
				return text2;
			}
			return text2.Substring(0, 300);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0003C064 File Offset: 0x0003A264
		private static string TrimOneLine(string text, int maxLength)
		{
			string text2 = Regex.Replace(text, "\\s+", " ").Trim();
			if (text2.Length <= maxLength)
			{
				return text2;
			}
			return text2.Substring(0, maxLength - 1) + "…";
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0003C0A8 File Offset: 0x0003A2A8
		private static string BuildWhyPicked(ModCompatibilityCommentsService.ScoredComment comment)
		{
			List<string> list = new List<string>();
			if (comment.Breakdown.ContainsKey("exactVersion"))
			{
				list.Add("Exact version mention");
			}
			else if (comment.Breakdown.ContainsKey("sameMinor"))
			{
				list.Add("Same minor version mention");
			}
			double num;
			if (comment.Breakdown.TryGetValue("positiveCues", ref num) && num > 0.0)
			{
				list.Add("Positive compatibility cues");
			}
			double num2;
			if (comment.Breakdown.TryGetValue("negativeCues", ref num2) && num2 < 0.0)
			{
				list.Add("Reported compatibility issues");
			}
			if (comment.Breakdown.ContainsKey("details"))
			{
				list.Add("Diagnostic details");
			}
			if (comment.Breakdown.ContainsKey("authorBoost"))
			{
				list.Add("Mod author insight");
			}
			if (list.Count == 0)
			{
				list.Add("High relevance score");
			}
			return string.Join(", ", list);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0003C1A5 File Offset: 0x0003A3A5
		private static string SquashWhitespace(string text)
		{
			return Regex.Replace(text, "\\s+", " ").Trim();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0003C1BC File Offset: 0x0003A3BC
		[return: Nullable(2)]
		private static string GetStringProperty(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (!element.TryGetProperty(propertyName, ref jsonElement))
			{
				return null;
			}
			switch (jsonElement.ValueKind)
			{
			case 3:
				return jsonElement.GetString();
			case 4:
			{
				long num;
				double num2;
				return jsonElement.TryGetInt64(ref num) ? num.ToString(CultureInfo.InvariantCulture) : (jsonElement.TryGetDouble(ref num2) ? num2.ToString(CultureInfo.InvariantCulture) : jsonElement.GetRawText());
			}
			case 7:
				return null;
			}
			return jsonElement.ToString();
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0003C258 File Offset: 0x0003A458
		private static string NormalizePageComponent(string value)
		{
			string text = value.Trim();
			if (text.Length == 0)
			{
				return string.Empty;
			}
			return text.Trim('/');
		}

		// Token: 0x0400049C RID: 1180
		private static readonly string ModApiUrlTemplate = DevConfig.ModCompatibilityApiUrlTemplate;

		// Token: 0x0400049D RID: 1181
		private static readonly string ModPageUrlTemplate = DevConfig.ModCompatibilityPageUrlTemplate;

		// Token: 0x0400049E RID: 1182
		private static readonly HttpClient HttpClient = new HttpClient(new HttpClientHandler
		{
			AutomaticDecompression = 3
		})
		{
			Timeout = TimeSpan.FromSeconds(15.0)
		};

		// Token: 0x0400049F RID: 1183
		private static readonly Regex VersionRegex = new Regex("\\b1\\.\\d{1,2}(\\.\\d{1,2})?(-rc\\.\\d+|-pre(\\.\\d+)?)?\\b", 9);

		// Token: 0x040004A0 RID: 1184
		private static readonly string[] PositiveCues = new string[]
		{
			"works",
			"working",
			"compatible",
			"fine",
			"loads",
			"no issues",
			"ok",
			"plays",
			"runs",
			"up to date",
			"stable",
			"on 1.21",
			"on 1.20"
		};

		// Token: 0x040004A1 RID: 1185
		private static readonly string[] NegativeCues = new string[]
		{
			"not working",
			"doesn't work",
			"doesnt work",
			"broken",
			"crash",
			"crashes",
			"error",
			"exception",
			"incompatible",
			"needs update",
			"no longer",
			"json error",
			"nullref",
			"fails",
			"can't load",
			"cant load",
			"won't",
			"wont"
		};

		// Token: 0x040004A2 RID: 1186
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			DefaultIgnoreCondition = 3
		};

		// Token: 0x02000292 RID: 658
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ModMetadata : IEquatable<ModCompatibilityCommentsService.ModMetadata>
		{
			// Token: 0x06001663 RID: 5731 RVA: 0x000828CF File Offset: 0x00080ACF
			public ModMetadata(DateTime? LastReleaseUtc, string PagePath, string CanonicalIdentifier)
			{
				this.LastReleaseUtc = LastReleaseUtc;
				this.PagePath = PagePath;
				this.CanonicalIdentifier = CanonicalIdentifier;
				base..ctor();
			}

			// Token: 0x170004A4 RID: 1188
			// (get) Token: 0x06001664 RID: 5732 RVA: 0x000828EC File Offset: 0x00080AEC
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(ModCompatibilityCommentsService.ModMetadata);
				}
			}

			// Token: 0x170004A5 RID: 1189
			// (get) Token: 0x06001665 RID: 5733 RVA: 0x000828F8 File Offset: 0x00080AF8
			// (set) Token: 0x06001666 RID: 5734 RVA: 0x00082900 File Offset: 0x00080B00
			public DateTime? LastReleaseUtc { get; set; }

			// Token: 0x170004A6 RID: 1190
			// (get) Token: 0x06001667 RID: 5735 RVA: 0x00082909 File Offset: 0x00080B09
			// (set) Token: 0x06001668 RID: 5736 RVA: 0x00082911 File Offset: 0x00080B11
			public string PagePath { get; set; }

			// Token: 0x170004A7 RID: 1191
			// (get) Token: 0x06001669 RID: 5737 RVA: 0x0008291A File Offset: 0x00080B1A
			// (set) Token: 0x0600166A RID: 5738 RVA: 0x00082922 File Offset: 0x00080B22
			public string CanonicalIdentifier { get; set; }

			// Token: 0x0600166B RID: 5739 RVA: 0x0008292C File Offset: 0x00080B2C
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ModMetadata");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600166C RID: 5740 RVA: 0x00082978 File Offset: 0x00080B78
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("LastReleaseUtc = ");
				builder.Append(this.LastReleaseUtc.ToString());
				builder.Append(", PagePath = ");
				builder.Append(this.PagePath);
				builder.Append(", CanonicalIdentifier = ");
				builder.Append(this.CanonicalIdentifier);
				return true;
			}

			// Token: 0x0600166D RID: 5741 RVA: 0x000829E4 File Offset: 0x00080BE4
			[CompilerGenerated]
			public static bool operator !=(ModCompatibilityCommentsService.ModMetadata left, ModCompatibilityCommentsService.ModMetadata right)
			{
				return !(left == right);
			}

			// Token: 0x0600166E RID: 5742 RVA: 0x000829F0 File Offset: 0x00080BF0
			[CompilerGenerated]
			public static bool operator ==(ModCompatibilityCommentsService.ModMetadata left, ModCompatibilityCommentsService.ModMetadata right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600166F RID: 5743 RVA: 0x00082A04 File Offset: 0x00080C04
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(this.<LastReleaseUtc>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<PagePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<CanonicalIdentifier>k__BackingField);
			}

			// Token: 0x06001670 RID: 5744 RVA: 0x00082A66 File Offset: 0x00080C66
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModCompatibilityCommentsService.ModMetadata);
			}

			// Token: 0x06001671 RID: 5745 RVA: 0x00082A74 File Offset: 0x00080C74
			[CompilerGenerated]
			public bool Equals(ModCompatibilityCommentsService.ModMetadata other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<DateTime?>.Default.Equals(this.<LastReleaseUtc>k__BackingField, other.<LastReleaseUtc>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<PagePath>k__BackingField, other.<PagePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<CanonicalIdentifier>k__BackingField, other.<CanonicalIdentifier>k__BackingField));
			}

			// Token: 0x06001673 RID: 5747 RVA: 0x00082AED File Offset: 0x00080CED
			[CompilerGenerated]
			private ModMetadata([Nullable(1)] ModCompatibilityCommentsService.ModMetadata original)
			{
				this.LastReleaseUtc = original.<LastReleaseUtc>k__BackingField;
				this.PagePath = original.<PagePath>k__BackingField;
				this.CanonicalIdentifier = original.<CanonicalIdentifier>k__BackingField;
			}

			// Token: 0x06001674 RID: 5748 RVA: 0x00082B19 File Offset: 0x00080D19
			[CompilerGenerated]
			public void Deconstruct(out DateTime? LastReleaseUtc, out string PagePath, out string CanonicalIdentifier)
			{
				LastReleaseUtc = this.LastReleaseUtc;
				PagePath = this.PagePath;
				CanonicalIdentifier = this.CanonicalIdentifier;
			}
		}

		// Token: 0x02000293 RID: 659
		[Nullable(0)]
		[RequiredMember]
		private sealed class Comment : IEquatable<ModCompatibilityCommentsService.Comment>
		{
			// Token: 0x170004A8 RID: 1192
			// (get) Token: 0x06001675 RID: 5749 RVA: 0x00082B37 File Offset: 0x00080D37
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ModCompatibilityCommentsService.Comment);
				}
			}

			// Token: 0x170004A9 RID: 1193
			// (get) Token: 0x06001676 RID: 5750 RVA: 0x00082B43 File Offset: 0x00080D43
			// (set) Token: 0x06001677 RID: 5751 RVA: 0x00082B4B File Offset: 0x00080D4B
			[RequiredMember]
			public string Author { get; set; }

			// Token: 0x170004AA RID: 1194
			// (get) Token: 0x06001678 RID: 5752 RVA: 0x00082B54 File Offset: 0x00080D54
			// (set) Token: 0x06001679 RID: 5753 RVA: 0x00082B5C File Offset: 0x00080D5C
			[RequiredMember]
			public bool IsModAuthor { get; set; }

			// Token: 0x170004AB RID: 1195
			// (get) Token: 0x0600167A RID: 5754 RVA: 0x00082B65 File Offset: 0x00080D65
			// (set) Token: 0x0600167B RID: 5755 RVA: 0x00082B6D File Offset: 0x00080D6D
			[RequiredMember]
			public DateTime TimestampUtc { get; set; }

			// Token: 0x170004AC RID: 1196
			// (get) Token: 0x0600167C RID: 5756 RVA: 0x00082B76 File Offset: 0x00080D76
			// (set) Token: 0x0600167D RID: 5757 RVA: 0x00082B7E File Offset: 0x00080D7E
			[RequiredMember]
			public string Text { get; set; }

			// Token: 0x170004AD RID: 1197
			// (get) Token: 0x0600167E RID: 5758 RVA: 0x00082B87 File Offset: 0x00080D87
			// (set) Token: 0x0600167F RID: 5759 RVA: 0x00082B8F File Offset: 0x00080D8F
			[Nullable(2)]
			public string Permalink { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x06001680 RID: 5760 RVA: 0x00082B98 File Offset: 0x00080D98
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Comment");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001681 RID: 5761 RVA: 0x00082BE4 File Offset: 0x00080DE4
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Author = ");
				builder.Append(this.Author);
				builder.Append(", IsModAuthor = ");
				builder.Append(this.IsModAuthor.ToString());
				builder.Append(", TimestampUtc = ");
				builder.Append(this.TimestampUtc.ToString());
				builder.Append(", Text = ");
				builder.Append(this.Text);
				builder.Append(", Permalink = ");
				builder.Append(this.Permalink);
				return true;
			}

			// Token: 0x06001682 RID: 5762 RVA: 0x00082C90 File Offset: 0x00080E90
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ModCompatibilityCommentsService.Comment left, ModCompatibilityCommentsService.Comment right)
			{
				return !(left == right);
			}

			// Token: 0x06001683 RID: 5763 RVA: 0x00082C9C File Offset: 0x00080E9C
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ModCompatibilityCommentsService.Comment left, ModCompatibilityCommentsService.Comment right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06001684 RID: 5764 RVA: 0x00082CB0 File Offset: 0x00080EB0
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Author>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsModAuthor>k__BackingField)) * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(this.<TimestampUtc>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Text>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Permalink>k__BackingField);
			}

			// Token: 0x06001685 RID: 5765 RVA: 0x00082D40 File Offset: 0x00080F40
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModCompatibilityCommentsService.Comment);
			}

			// Token: 0x06001686 RID: 5766 RVA: 0x00082D50 File Offset: 0x00080F50
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ModCompatibilityCommentsService.Comment other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Author>k__BackingField, other.<Author>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsModAuthor>k__BackingField, other.<IsModAuthor>k__BackingField) && EqualityComparer<DateTime>.Default.Equals(this.<TimestampUtc>k__BackingField, other.<TimestampUtc>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Text>k__BackingField, other.<Text>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Permalink>k__BackingField, other.<Permalink>k__BackingField));
			}

			// Token: 0x06001688 RID: 5768 RVA: 0x00082E00 File Offset: 0x00081000
			[CompilerGenerated]
			[SetsRequiredMembers]
			private Comment(ModCompatibilityCommentsService.Comment original)
			{
				this.Author = original.<Author>k__BackingField;
				this.IsModAuthor = original.<IsModAuthor>k__BackingField;
				this.TimestampUtc = original.<TimestampUtc>k__BackingField;
				this.Text = original.<Text>k__BackingField;
				this.Permalink = original.<Permalink>k__BackingField;
			}

			// Token: 0x06001689 RID: 5769 RVA: 0x00082E4F File Offset: 0x0008104F
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public Comment()
			{
			}
		}

		// Token: 0x02000294 RID: 660
		[Nullable(0)]
		private sealed class ScoredComment : IEquatable<ModCompatibilityCommentsService.ScoredComment>
		{
			// Token: 0x0600168A RID: 5770 RVA: 0x00082E57 File Offset: 0x00081057
			public ScoredComment(ModCompatibilityCommentsService.Comment Base, double Score, double Relevance, string Polarity, IReadOnlyDictionary<string, double> Breakdown, bool HasExactVersion)
			{
				this.Base = Base;
				this.Score = Score;
				this.Relevance = Relevance;
				this.Polarity = Polarity;
				this.Breakdown = Breakdown;
				this.HasExactVersion = HasExactVersion;
				base..ctor();
			}

			// Token: 0x170004AE RID: 1198
			// (get) Token: 0x0600168B RID: 5771 RVA: 0x00082E8C File Offset: 0x0008108C
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ModCompatibilityCommentsService.ScoredComment);
				}
			}

			// Token: 0x170004AF RID: 1199
			// (get) Token: 0x0600168C RID: 5772 RVA: 0x00082E98 File Offset: 0x00081098
			// (set) Token: 0x0600168D RID: 5773 RVA: 0x00082EA0 File Offset: 0x000810A0
			public ModCompatibilityCommentsService.Comment Base { get; set; }

			// Token: 0x170004B0 RID: 1200
			// (get) Token: 0x0600168E RID: 5774 RVA: 0x00082EA9 File Offset: 0x000810A9
			// (set) Token: 0x0600168F RID: 5775 RVA: 0x00082EB1 File Offset: 0x000810B1
			public double Score { get; set; }

			// Token: 0x170004B1 RID: 1201
			// (get) Token: 0x06001690 RID: 5776 RVA: 0x00082EBA File Offset: 0x000810BA
			// (set) Token: 0x06001691 RID: 5777 RVA: 0x00082EC2 File Offset: 0x000810C2
			public double Relevance { get; set; }

			// Token: 0x170004B2 RID: 1202
			// (get) Token: 0x06001692 RID: 5778 RVA: 0x00082ECB File Offset: 0x000810CB
			// (set) Token: 0x06001693 RID: 5779 RVA: 0x00082ED3 File Offset: 0x000810D3
			public string Polarity { get; set; }

			// Token: 0x170004B3 RID: 1203
			// (get) Token: 0x06001694 RID: 5780 RVA: 0x00082EDC File Offset: 0x000810DC
			// (set) Token: 0x06001695 RID: 5781 RVA: 0x00082EE4 File Offset: 0x000810E4
			public IReadOnlyDictionary<string, double> Breakdown { get; set; }

			// Token: 0x170004B4 RID: 1204
			// (get) Token: 0x06001696 RID: 5782 RVA: 0x00082EED File Offset: 0x000810ED
			// (set) Token: 0x06001697 RID: 5783 RVA: 0x00082EF5 File Offset: 0x000810F5
			public bool HasExactVersion { get; set; }

			// Token: 0x06001698 RID: 5784 RVA: 0x00082F00 File Offset: 0x00081100
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ScoredComment");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001699 RID: 5785 RVA: 0x00082F4C File Offset: 0x0008114C
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Base = ");
				builder.Append(this.Base);
				builder.Append(", Score = ");
				builder.Append(this.Score.ToString());
				builder.Append(", Relevance = ");
				builder.Append(this.Relevance.ToString());
				builder.Append(", Polarity = ");
				builder.Append(this.Polarity);
				builder.Append(", Breakdown = ");
				builder.Append(this.Breakdown);
				builder.Append(", HasExactVersion = ");
				builder.Append(this.HasExactVersion.ToString());
				return true;
			}

			// Token: 0x0600169A RID: 5786 RVA: 0x0008301F File Offset: 0x0008121F
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ModCompatibilityCommentsService.ScoredComment left, ModCompatibilityCommentsService.ScoredComment right)
			{
				return !(left == right);
			}

			// Token: 0x0600169B RID: 5787 RVA: 0x0008302B File Offset: 0x0008122B
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ModCompatibilityCommentsService.ScoredComment left, ModCompatibilityCommentsService.ScoredComment right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600169C RID: 5788 RVA: 0x00083040 File Offset: 0x00081240
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ModCompatibilityCommentsService.Comment>.Default.GetHashCode(this.<Base>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Score>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Relevance>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Polarity>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyDictionary<string, double>>.Default.GetHashCode(this.<Breakdown>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<HasExactVersion>k__BackingField);
			}

			// Token: 0x0600169D RID: 5789 RVA: 0x000830E7 File Offset: 0x000812E7
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModCompatibilityCommentsService.ScoredComment);
			}

			// Token: 0x0600169E RID: 5790 RVA: 0x000830F8 File Offset: 0x000812F8
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ModCompatibilityCommentsService.ScoredComment other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ModCompatibilityCommentsService.Comment>.Default.Equals(this.<Base>k__BackingField, other.<Base>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<Score>k__BackingField, other.<Score>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<Relevance>k__BackingField, other.<Relevance>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Polarity>k__BackingField, other.<Polarity>k__BackingField) && EqualityComparer<IReadOnlyDictionary<string, double>>.Default.Equals(this.<Breakdown>k__BackingField, other.<Breakdown>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<HasExactVersion>k__BackingField, other.<HasExactVersion>k__BackingField));
			}

			// Token: 0x060016A0 RID: 5792 RVA: 0x000831C4 File Offset: 0x000813C4
			[CompilerGenerated]
			private ScoredComment(ModCompatibilityCommentsService.ScoredComment original)
			{
				this.Base = original.<Base>k__BackingField;
				this.Score = original.<Score>k__BackingField;
				this.Relevance = original.<Relevance>k__BackingField;
				this.Polarity = original.<Polarity>k__BackingField;
				this.Breakdown = original.<Breakdown>k__BackingField;
				this.HasExactVersion = original.<HasExactVersion>k__BackingField;
			}

			// Token: 0x060016A1 RID: 5793 RVA: 0x0008321F File Offset: 0x0008141F
			[CompilerGenerated]
			public void Deconstruct(out ModCompatibilityCommentsService.Comment Base, out double Score, out double Relevance, out string Polarity, out IReadOnlyDictionary<string, double> Breakdown, out bool HasExactVersion)
			{
				Base = this.Base;
				Score = this.Score;
				Relevance = this.Relevance;
				Polarity = this.Polarity;
				Breakdown = this.Breakdown;
				HasExactVersion = this.HasExactVersion;
			}
		}

		// Token: 0x02000295 RID: 661
		[NullableContext(2)]
		[Nullable(0)]
		[RequiredMember]
		public sealed class ExperimentalCompReviewResult : IEquatable<ModCompatibilityCommentsService.ExperimentalCompReviewResult>
		{
			// Token: 0x170004B5 RID: 1205
			// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00083254 File Offset: 0x00081454
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(ModCompatibilityCommentsService.ExperimentalCompReviewResult);
				}
			}

			// Token: 0x170004B6 RID: 1206
			// (get) Token: 0x060016A3 RID: 5795 RVA: 0x00083260 File Offset: 0x00081460
			// (set) Token: 0x060016A4 RID: 5796 RVA: 0x00083268 File Offset: 0x00081468
			[Nullable(1)]
			[RequiredMember]
			[JsonPropertyName("mod")]
			public string Mod { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x170004B7 RID: 1207
			// (get) Token: 0x060016A5 RID: 5797 RVA: 0x00083271 File Offset: 0x00081471
			// (set) Token: 0x060016A6 RID: 5798 RVA: 0x00083279 File Offset: 0x00081479
			[JsonPropertyName("latestVsVersion")]
			[JsonIgnore(Condition = 3)]
			public string LatestVsVersion { get; set; }

			// Token: 0x170004B8 RID: 1208
			// (get) Token: 0x060016A7 RID: 5799 RVA: 0x00083282 File Offset: 0x00081482
			// (set) Token: 0x060016A8 RID: 5800 RVA: 0x0008328A File Offset: 0x0008148A
			[JsonPropertyName("modLastReleaseUtc")]
			[JsonIgnore(Condition = 3)]
			public string ModLastReleaseUtc { get; set; }

			// Token: 0x170004B9 RID: 1209
			// (get) Token: 0x060016A9 RID: 5801 RVA: 0x00083293 File Offset: 0x00081493
			// (set) Token: 0x060016AA RID: 5802 RVA: 0x0008329B File Offset: 0x0008149B
			[Nullable(1)]
			[JsonPropertyName("top3")]
			public IReadOnlyList<ModCompatibilityCommentsService.ExperimentalCompReviewComment> Top3 { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x170004BA RID: 1210
			// (get) Token: 0x060016AB RID: 5803 RVA: 0x000832A4 File Offset: 0x000814A4
			// (set) Token: 0x060016AC RID: 5804 RVA: 0x000832AC File Offset: 0x000814AC
			[JsonPropertyName("reason")]
			[JsonIgnore(Condition = 3)]
			public string Reason { get; set; }

			// Token: 0x060016AD RID: 5805 RVA: 0x000832B8 File Offset: 0x000814B8
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ExperimentalCompReviewResult");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060016AE RID: 5806 RVA: 0x00083304 File Offset: 0x00081504
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Mod = ");
				builder.Append(this.Mod);
				builder.Append(", LatestVsVersion = ");
				builder.Append(this.LatestVsVersion);
				builder.Append(", ModLastReleaseUtc = ");
				builder.Append(this.ModLastReleaseUtc);
				builder.Append(", Top3 = ");
				builder.Append(this.Top3);
				builder.Append(", Reason = ");
				builder.Append(this.Reason);
				return true;
			}

			// Token: 0x060016AF RID: 5807 RVA: 0x00083394 File Offset: 0x00081594
			[CompilerGenerated]
			public static bool operator !=(ModCompatibilityCommentsService.ExperimentalCompReviewResult left, ModCompatibilityCommentsService.ExperimentalCompReviewResult right)
			{
				return !(left == right);
			}

			// Token: 0x060016B0 RID: 5808 RVA: 0x000833A0 File Offset: 0x000815A0
			[CompilerGenerated]
			public static bool operator ==(ModCompatibilityCommentsService.ExperimentalCompReviewResult left, ModCompatibilityCommentsService.ExperimentalCompReviewResult right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060016B1 RID: 5809 RVA: 0x000833B4 File Offset: 0x000815B4
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Mod>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<LatestVsVersion>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModLastReleaseUtc>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyList<ModCompatibilityCommentsService.ExperimentalCompReviewComment>>.Default.GetHashCode(this.<Top3>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Reason>k__BackingField);
			}

			// Token: 0x060016B2 RID: 5810 RVA: 0x00083444 File Offset: 0x00081644
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModCompatibilityCommentsService.ExperimentalCompReviewResult);
			}

			// Token: 0x060016B3 RID: 5811 RVA: 0x00083454 File Offset: 0x00081654
			[CompilerGenerated]
			public bool Equals(ModCompatibilityCommentsService.ExperimentalCompReviewResult other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Mod>k__BackingField, other.<Mod>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<LatestVsVersion>k__BackingField, other.<LatestVsVersion>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ModLastReleaseUtc>k__BackingField, other.<ModLastReleaseUtc>k__BackingField) && EqualityComparer<IReadOnlyList<ModCompatibilityCommentsService.ExperimentalCompReviewComment>>.Default.Equals(this.<Top3>k__BackingField, other.<Top3>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Reason>k__BackingField, other.<Reason>k__BackingField));
			}

			// Token: 0x060016B5 RID: 5813 RVA: 0x00083504 File Offset: 0x00081704
			[CompilerGenerated]
			[SetsRequiredMembers]
			private ExperimentalCompReviewResult([Nullable(1)] ModCompatibilityCommentsService.ExperimentalCompReviewResult original)
			{
				this.Mod = original.<Mod>k__BackingField;
				this.LatestVsVersion = original.<LatestVsVersion>k__BackingField;
				this.ModLastReleaseUtc = original.<ModLastReleaseUtc>k__BackingField;
				this.Top3 = original.<Top3>k__BackingField;
				this.Reason = original.<Reason>k__BackingField;
			}

			// Token: 0x060016B6 RID: 5814 RVA: 0x00083553 File Offset: 0x00081753
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public ExperimentalCompReviewResult()
			{
				this.Top3 = Array.Empty<ModCompatibilityCommentsService.ExperimentalCompReviewComment>();
				base..ctor();
			}
		}

		// Token: 0x02000296 RID: 662
		[Nullable(0)]
		[RequiredMember]
		public sealed class ExperimentalCompReviewComment : IEquatable<ModCompatibilityCommentsService.ExperimentalCompReviewComment>
		{
			// Token: 0x170004BB RID: 1211
			// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00083566 File Offset: 0x00081766
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ModCompatibilityCommentsService.ExperimentalCompReviewComment);
				}
			}

			// Token: 0x170004BC RID: 1212
			// (get) Token: 0x060016B8 RID: 5816 RVA: 0x00083572 File Offset: 0x00081772
			// (set) Token: 0x060016B9 RID: 5817 RVA: 0x0008357A File Offset: 0x0008177A
			[RequiredMember]
			[JsonPropertyName("timestampUtc")]
			public string TimestampUtc { get; set; }

			// Token: 0x170004BD RID: 1213
			// (get) Token: 0x060016BA RID: 5818 RVA: 0x00083583 File Offset: 0x00081783
			// (set) Token: 0x060016BB RID: 5819 RVA: 0x0008358B File Offset: 0x0008178B
			[RequiredMember]
			[JsonPropertyName("author")]
			public string Author { get; set; }

			// Token: 0x170004BE RID: 1214
			// (get) Token: 0x060016BC RID: 5820 RVA: 0x00083594 File Offset: 0x00081794
			// (set) Token: 0x060016BD RID: 5821 RVA: 0x0008359C File Offset: 0x0008179C
			[JsonPropertyName("isModAuthor")]
			public bool IsModAuthor { get; set; }

			// Token: 0x170004BF RID: 1215
			// (get) Token: 0x060016BE RID: 5822 RVA: 0x000835A5 File Offset: 0x000817A5
			// (set) Token: 0x060016BF RID: 5823 RVA: 0x000835AD File Offset: 0x000817AD
			[RequiredMember]
			[JsonPropertyName("polarity")]
			public string Polarity { get; set; }

			// Token: 0x170004C0 RID: 1216
			// (get) Token: 0x060016C0 RID: 5824 RVA: 0x000835B6 File Offset: 0x000817B6
			// (set) Token: 0x060016C1 RID: 5825 RVA: 0x000835BE File Offset: 0x000817BE
			[JsonPropertyName("relevance")]
			public double Relevance { get; set; }

			// Token: 0x170004C1 RID: 1217
			// (get) Token: 0x060016C2 RID: 5826 RVA: 0x000835C7 File Offset: 0x000817C7
			// (set) Token: 0x060016C3 RID: 5827 RVA: 0x000835CF File Offset: 0x000817CF
			[JsonPropertyName("scoreBreakdown")]
			public IReadOnlyDictionary<string, double> ScoreBreakdown { get; set; }

			// Token: 0x170004C2 RID: 1218
			// (get) Token: 0x060016C4 RID: 5828 RVA: 0x000835D8 File Offset: 0x000817D8
			// (set) Token: 0x060016C5 RID: 5829 RVA: 0x000835E0 File Offset: 0x000817E0
			[RequiredMember]
			[JsonPropertyName("snippet")]
			public string Snippet { get; set; }

			// Token: 0x170004C3 RID: 1219
			// (get) Token: 0x060016C6 RID: 5830 RVA: 0x000835E9 File Offset: 0x000817E9
			// (set) Token: 0x060016C7 RID: 5831 RVA: 0x000835F1 File Offset: 0x000817F1
			[Nullable(2)]
			[JsonPropertyName("permalink")]
			[JsonIgnore(Condition = 3)]
			public string Permalink { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170004C4 RID: 1220
			// (get) Token: 0x060016C8 RID: 5832 RVA: 0x000835FA File Offset: 0x000817FA
			// (set) Token: 0x060016C9 RID: 5833 RVA: 0x00083602 File Offset: 0x00081802
			[RequiredMember]
			[JsonPropertyName("whyPicked")]
			public string WhyPicked { get; set; }

			// Token: 0x060016CA RID: 5834 RVA: 0x0008360C File Offset: 0x0008180C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ExperimentalCompReviewComment");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060016CB RID: 5835 RVA: 0x00083658 File Offset: 0x00081858
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("TimestampUtc = ");
				builder.Append(this.TimestampUtc);
				builder.Append(", Author = ");
				builder.Append(this.Author);
				builder.Append(", IsModAuthor = ");
				builder.Append(this.IsModAuthor.ToString());
				builder.Append(", Polarity = ");
				builder.Append(this.Polarity);
				builder.Append(", Relevance = ");
				builder.Append(this.Relevance.ToString());
				builder.Append(", ScoreBreakdown = ");
				builder.Append(this.ScoreBreakdown);
				builder.Append(", Snippet = ");
				builder.Append(this.Snippet);
				builder.Append(", Permalink = ");
				builder.Append(this.Permalink);
				builder.Append(", WhyPicked = ");
				builder.Append(this.WhyPicked);
				return true;
			}

			// Token: 0x060016CC RID: 5836 RVA: 0x00083768 File Offset: 0x00081968
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ModCompatibilityCommentsService.ExperimentalCompReviewComment left, ModCompatibilityCommentsService.ExperimentalCompReviewComment right)
			{
				return !(left == right);
			}

			// Token: 0x060016CD RID: 5837 RVA: 0x00083774 File Offset: 0x00081974
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ModCompatibilityCommentsService.ExperimentalCompReviewComment left, ModCompatibilityCommentsService.ExperimentalCompReviewComment right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060016CE RID: 5838 RVA: 0x00083788 File Offset: 0x00081988
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<TimestampUtc>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Author>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsModAuthor>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Polarity>k__BackingField)) * -1521134295 + EqualityComparer<double>.Default.GetHashCode(this.<Relevance>k__BackingField)) * -1521134295 + EqualityComparer<IReadOnlyDictionary<string, double>>.Default.GetHashCode(this.<ScoreBreakdown>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Snippet>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Permalink>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<WhyPicked>k__BackingField);
			}

			// Token: 0x060016CF RID: 5839 RVA: 0x00083874 File Offset: 0x00081A74
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModCompatibilityCommentsService.ExperimentalCompReviewComment);
			}

			// Token: 0x060016D0 RID: 5840 RVA: 0x00083884 File Offset: 0x00081A84
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ModCompatibilityCommentsService.ExperimentalCompReviewComment other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<TimestampUtc>k__BackingField, other.<TimestampUtc>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Author>k__BackingField, other.<Author>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsModAuthor>k__BackingField, other.<IsModAuthor>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Polarity>k__BackingField, other.<Polarity>k__BackingField) && EqualityComparer<double>.Default.Equals(this.<Relevance>k__BackingField, other.<Relevance>k__BackingField) && EqualityComparer<IReadOnlyDictionary<string, double>>.Default.Equals(this.<ScoreBreakdown>k__BackingField, other.<ScoreBreakdown>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Snippet>k__BackingField, other.<Snippet>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Permalink>k__BackingField, other.<Permalink>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<WhyPicked>k__BackingField, other.<WhyPicked>k__BackingField));
			}

			// Token: 0x060016D2 RID: 5842 RVA: 0x000839A0 File Offset: 0x00081BA0
			[CompilerGenerated]
			[SetsRequiredMembers]
			private ExperimentalCompReviewComment(ModCompatibilityCommentsService.ExperimentalCompReviewComment original)
			{
				this.TimestampUtc = original.<TimestampUtc>k__BackingField;
				this.Author = original.<Author>k__BackingField;
				this.IsModAuthor = original.<IsModAuthor>k__BackingField;
				this.Polarity = original.<Polarity>k__BackingField;
				this.Relevance = original.<Relevance>k__BackingField;
				this.ScoreBreakdown = original.<ScoreBreakdown>k__BackingField;
				this.Snippet = original.<Snippet>k__BackingField;
				this.Permalink = original.<Permalink>k__BackingField;
				this.WhyPicked = original.<WhyPicked>k__BackingField;
			}

			// Token: 0x060016D3 RID: 5843 RVA: 0x00083A1F File Offset: 0x00081C1F
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public ExperimentalCompReviewComment()
			{
				this.ScoreBreakdown = new Dictionary<string, double>();
				base..ctor();
			}
		}

		// Token: 0x02000297 RID: 663
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000FE1 RID: 4065
			[Nullable(0)]
			public static Func<ModCompatibilityCommentsService.ScoredComment, ModCompatibilityCommentsService.ExperimentalCompReviewComment> <0>__BuildCommentResult;
		}
	}
}
