using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x0200000C RID: 12
	public class NumberBoxAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00002902 File Offset: 0x00000B02
		public NumberBoxAutomationPeer(NumberBox owner) : base(owner)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000290B File Offset: 0x00000B0B
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 3)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000291A File Offset: 0x00000B1A
		protected override string GetClassNameCore()
		{
			return "NumberBox";
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002924 File Offset: 0x00000B24
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (string.IsNullOrEmpty(text))
			{
				NumberBox numberBox = base.Owner as NumberBox;
				if (numberBox != null)
				{
					object header = numberBox.Header;
					text = ((header != null) ? header.ToString() : null);
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000296C File Offset: 0x00000B6C
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 16;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002970 File Offset: 0x00000B70
		private NumberBox GetImpl()
		{
			NumberBox result = null;
			NumberBox numberBox = base.Owner as NumberBox;
			if (numberBox != null)
			{
				result = numberBox;
			}
			return result;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002991 File Offset: 0x00000B91
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002994 File Offset: 0x00000B94
		public double Minimum
		{
			get
			{
				return this.GetImpl().Minimum;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000029A1 File Offset: 0x00000BA1
		public double Maximum
		{
			get
			{
				return this.GetImpl().Maximum;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000029AE File Offset: 0x00000BAE
		public double Value
		{
			get
			{
				return this.GetImpl().Value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000029BB File Offset: 0x00000BBB
		public double SmallChange
		{
			get
			{
				return this.GetImpl().SmallChange;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000029C8 File Offset: 0x00000BC8
		public double LargeChange
		{
			get
			{
				return this.GetImpl().LargeChange;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000029D5 File Offset: 0x00000BD5
		public void SetValue(double value)
		{
			this.GetImpl().Value = value;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000029E3 File Offset: 0x00000BE3
		public void RaiseValueChangedEvent(double oldValue, double newValue)
		{
			base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty, oldValue, newValue);
		}
	}
}
