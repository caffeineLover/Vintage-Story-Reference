using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x0200000F RID: 15
	public class RatingControlAutomationPeer : FrameworkElementAutomationPeer, IValueProvider, IRangeValueProvider
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00002A8D File Offset: 0x00000C8D
		public RatingControlAutomationPeer(RatingControl owner) : base(owner)
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002A96 File Offset: 0x00000C96
		protected override string GetLocalizedControlTypeCore()
		{
			return RatingControlAutomationPeer.ResourceAccessor.GetLocalizedStringResource("RatingLocalizedControlType");
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002AA7 File Offset: 0x00000CA7
		public bool IsReadOnly
		{
			get
			{
				return this.GetRatingControl().IsReadOnly;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002AB4 File Offset: 0x00000CB4
		string IValueProvider.Value
		{
			get
			{
				double value = this.GetRatingControl().Value;
				string result;
				if (value == -1.0)
				{
					double placeholderValue = this.GetRatingControl().PlaceholderValue;
					if (placeholderValue == -1.0)
					{
						result = RatingControlAutomationPeer.ResourceAccessor.GetLocalizedStringResource("RatingUnset");
					}
					else
					{
						result = this.GenerateValue_ValueString(RatingControlAutomationPeer.ResourceAccessor.GetLocalizedStringResource("CommunityRatingString"), placeholderValue);
					}
				}
				else
				{
					result = this.GenerateValue_ValueString(RatingControlAutomationPeer.ResourceAccessor.GetLocalizedStringResource("BasicRatingString"), value);
				}
				return result;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002B34 File Offset: 0x00000D34
		public void SetValue(string value)
		{
			double value2;
			if (double.TryParse(value, ref value2))
			{
				this.GetRatingControl().Value = value2;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002B57 File Offset: 0x00000D57
		public double SmallChange
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002B62 File Offset: 0x00000D62
		public double LargeChange
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002B6D File Offset: 0x00000D6D
		public double Maximum
		{
			get
			{
				return (double)this.GetRatingControl().MaxRating;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002B7B File Offset: 0x00000D7B
		public double Minimum
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002B88 File Offset: 0x00000D88
		public double Value
		{
			get
			{
				double value = this.GetRatingControl().Value;
				if (value == -1.0)
				{
					return 0.0;
				}
				return value;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public void SetValue(double value)
		{
			this.GetRatingControl().Value = value;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002BC6 File Offset: 0x00000DC6
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 2 || patternInterface == 3)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002BD9 File Offset: 0x00000DD9
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 15;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002BE0 File Offset: 0x00000DE0
		internal void RaisePropertyChangedEvent(double newValue)
		{
			double value = this.GetRatingControl().Value;
			if (newValue == -1.0)
			{
				newValue = 0.0;
			}
			base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, value.ToString(), newValue.ToString());
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty, value, newValue);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002C40 File Offset: 0x00000E40
		private RatingControl GetRatingControl()
		{
			return (RatingControl)base.Owner;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002C50 File Offset: 0x00000E50
		private int DetermineFractionDigits(double value)
		{
			value *= 100.0;
			int num = (int)value;
			if (num % 100 == 0)
			{
				return 0;
			}
			if (num % 10 == 0)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002C80 File Offset: 0x00000E80
		private string GenerateValue_ValueString(string resourceString, double ratingValue)
		{
			string text = this.GetRatingControl().MaxRating.ToString();
			string text2 = ratingValue.ToString("F" + this.DetermineFractionDigits(ratingValue).ToString());
			return string.Format(resourceString, text2, text);
		}

		// Token: 0x04000007 RID: 7
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(RatingControl));
	}
}
