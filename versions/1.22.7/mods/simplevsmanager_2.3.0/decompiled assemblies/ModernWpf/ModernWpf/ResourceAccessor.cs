using System;
using System.Reflection;
using System.Resources;

namespace ModernWpf
{
	// Token: 0x0200020E RID: 526
	internal class ResourceAccessor
	{
		// Token: 0x06000F7C RID: 3964 RVA: 0x0002C219 File Offset: 0x0002A419
		public ResourceAccessor(Type controlType)
		{
			if (controlType == null)
			{
				throw new ArgumentNullException("controlType");
			}
			this._controlType = controlType;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0002C238 File Offset: 0x0002A438
		public string GetLocalizedStringResource(string resourceName)
		{
			if (this._resourceManager == null)
			{
				Assembly assembly = this._controlType.Assembly;
				string name = assembly.GetName().Name;
				string name2 = this._controlType.Name;
				string text = name + "." + name2 + ".Strings.Resources";
				this._resourceManager = new ResourceManager(text, assembly);
			}
			return this._resourceManager.GetString(resourceName);
		}

		// Token: 0x0400035A RID: 858
		public const string SR_BasicRatingString = "BasicRatingString";

		// Token: 0x0400035B RID: 859
		public const string SR_CommunityRatingString = "CommunityRatingString";

		// Token: 0x0400035C RID: 860
		public const string SR_RatingsControlName = "RatingsControlName";

		// Token: 0x0400035D RID: 861
		public const string SR_RatingControlName = "RatingControlName";

		// Token: 0x0400035E RID: 862
		public const string SR_RatingUnset = "RatingUnset";

		// Token: 0x0400035F RID: 863
		public const string SR_NavigationButtonClosedName = "NavigationButtonClosedName";

		// Token: 0x04000360 RID: 864
		public const string SR_NavigationButtonOpenName = "NavigationButtonOpenName";

		// Token: 0x04000361 RID: 865
		public const string SR_NavigationViewItemDefaultControlName = "NavigationViewItemDefaultControlName";

		// Token: 0x04000362 RID: 866
		public const string SR_NavigationBackButtonName = "NavigationBackButtonName";

		// Token: 0x04000363 RID: 867
		public const string SR_NavigationBackButtonToolTip = "NavigationBackButtonToolTip";

		// Token: 0x04000364 RID: 868
		public const string SR_NavigationCloseButtonName = "NavigationCloseButtonName";

		// Token: 0x04000365 RID: 869
		public const string SR_NavigationOverflowButtonName = "NavigationOverflowButtonName";

		// Token: 0x04000366 RID: 870
		public const string SR_NavigationOverflowButtonText = "NavigationOverflowButtonText";

		// Token: 0x04000367 RID: 871
		public const string SR_NavigationOverflowButtonToolTip = "NavigationOverflowButtonToolTip";

		// Token: 0x04000368 RID: 872
		public const string SR_SettingsButtonName = "SettingsButtonName";

		// Token: 0x04000369 RID: 873
		public const string SR_NavigationViewSearchButtonName = "NavigationViewSearchButtonName";

		// Token: 0x0400036A RID: 874
		public const string SR_TextAlphaLabel = "TextAlphaLabel";

		// Token: 0x0400036B RID: 875
		public const string SR_AutomationNameAlphaSlider = "AutomationNameAlphaSlider";

		// Token: 0x0400036C RID: 876
		public const string SR_AutomationNameAlphaTextBox = "AutomationNameAlphaTextBox";

		// Token: 0x0400036D RID: 877
		public const string SR_AutomationNameHueSlider = "AutomationNameHueSlider";

		// Token: 0x0400036E RID: 878
		public const string SR_AutomationNameSaturationSlider = "AutomationNameSaturationSlider";

		// Token: 0x0400036F RID: 879
		public const string SR_AutomationNameValueSlider = "AutomationNameValueSlider";

		// Token: 0x04000370 RID: 880
		public const string SR_TextBlueLabel = "TextBlueLabel";

		// Token: 0x04000371 RID: 881
		public const string SR_AutomationNameBlueTextBox = "AutomationNameBlueTextBox";

		// Token: 0x04000372 RID: 882
		public const string SR_AutomationNameColorModelComboBox = "AutomationNameColorModelComboBox";

		// Token: 0x04000373 RID: 883
		public const string SR_AutomationNameColorSpectrum = "AutomationNameColorSpectrum";

		// Token: 0x04000374 RID: 884
		public const string SR_TextGreenLabel = "TextGreenLabel";

		// Token: 0x04000375 RID: 885
		public const string SR_AutomationNameGreenTextBox = "AutomationNameGreenTextBox";

		// Token: 0x04000376 RID: 886
		public const string SR_HelpTextColorSpectrum = "HelpTextColorSpectrum";

		// Token: 0x04000377 RID: 887
		public const string SR_AutomationNameHexTextBox = "AutomationNameHexTextBox";

		// Token: 0x04000378 RID: 888
		public const string SR_ContentHSVComboBoxItem = "ContentHSVComboBoxItem";

		// Token: 0x04000379 RID: 889
		public const string SR_TextHueLabel = "TextHueLabel";

		// Token: 0x0400037A RID: 890
		public const string SR_AutomationNameHueTextBox = "AutomationNameHueTextBox";

		// Token: 0x0400037B RID: 891
		public const string SR_LocalizedControlTypeColorSpectrum = "LocalizedControlTypeColorSpectrum";

		// Token: 0x0400037C RID: 892
		public const string SR_TextRedLabel = "TextRedLabel";

		// Token: 0x0400037D RID: 893
		public const string SR_AutomationNameRedTextBox = "AutomationNameRedTextBox";

		// Token: 0x0400037E RID: 894
		public const string SR_ContentRGBComboBoxItem = "ContentRGBComboBoxItem";

		// Token: 0x0400037F RID: 895
		public const string SR_TextSaturationLabel = "TextSaturationLabel";

		// Token: 0x04000380 RID: 896
		public const string SR_AutomationNameSaturationTextBox = "AutomationNameSaturationTextBox";

		// Token: 0x04000381 RID: 897
		public const string SR_TextValueLabel = "TextValueLabel";

		// Token: 0x04000382 RID: 898
		public const string SR_ValueStringColorSpectrumWithColorName = "ValueStringColorSpectrumWithColorName";

		// Token: 0x04000383 RID: 899
		public const string SR_ValueStringColorSpectrumWithoutColorName = "ValueStringColorSpectrumWithoutColorName";

		// Token: 0x04000384 RID: 900
		public const string SR_ValueStringHueSliderWithColorName = "ValueStringHueSliderWithColorName";

		// Token: 0x04000385 RID: 901
		public const string SR_ValueStringHueSliderWithoutColorName = "ValueStringHueSliderWithoutColorName";

		// Token: 0x04000386 RID: 902
		public const string SR_ValueStringSaturationSliderWithColorName = "ValueStringSaturationSliderWithColorName";

		// Token: 0x04000387 RID: 903
		public const string SR_ValueStringSaturationSliderWithoutColorName = "ValueStringSaturationSliderWithoutColorName";

		// Token: 0x04000388 RID: 904
		public const string SR_ValueStringValueSliderWithColorName = "ValueStringValueSliderWithColorName";

		// Token: 0x04000389 RID: 905
		public const string SR_ValueStringValueSliderWithoutColorName = "ValueStringValueSliderWithoutColorName";

		// Token: 0x0400038A RID: 906
		public const string SR_AutomationNameValueTextBox = "AutomationNameValueTextBox";

		// Token: 0x0400038B RID: 907
		public const string SR_ToolTipStringAlphaSlider = "ToolTipStringAlphaSlider";

		// Token: 0x0400038C RID: 908
		public const string SR_ToolTipStringHueSliderWithColorName = "ToolTipStringHueSliderWithColorName";

		// Token: 0x0400038D RID: 909
		public const string SR_ToolTipStringHueSliderWithoutColorName = "ToolTipStringHueSliderWithoutColorName";

		// Token: 0x0400038E RID: 910
		public const string SR_ToolTipStringSaturationSliderWithColorName = "ToolTipStringSaturationSliderWithColorName";

		// Token: 0x0400038F RID: 911
		public const string SR_ToolTipStringSaturationSliderWithoutColorName = "ToolTipStringSaturationSliderWithoutColorName";

		// Token: 0x04000390 RID: 912
		public const string SR_ToolTipStringValueSliderWithColorName = "ToolTipStringValueSliderWithColorName";

		// Token: 0x04000391 RID: 913
		public const string SR_ToolTipStringValueSliderWithoutColorName = "ToolTipStringValueSliderWithoutColorName";

		// Token: 0x04000392 RID: 914
		public const string SR_AutomationNameMoreButtonCollapsed = "AutomationNameMoreButtonCollapsed";

		// Token: 0x04000393 RID: 915
		public const string SR_AutomationNameMoreButtonExpanded = "AutomationNameMoreButtonExpanded";

		// Token: 0x04000394 RID: 916
		public const string SR_HelpTextMoreButton = "HelpTextMoreButton";

		// Token: 0x04000395 RID: 917
		public const string SR_TextMoreButtonLabelCollapsed = "TextMoreButtonLabelCollapsed";

		// Token: 0x04000396 RID: 918
		public const string SR_TextMoreButtonLabelExpanded = "TextMoreButtonLabelExpanded";

		// Token: 0x04000397 RID: 919
		public const string SR_BadgeItemPlural1 = "BadgeItemPlural1";

		// Token: 0x04000398 RID: 920
		public const string SR_BadgeItemPlural2 = "BadgeItemPlural2";

		// Token: 0x04000399 RID: 921
		public const string SR_BadgeItemPlural3 = "BadgeItemPlural3";

		// Token: 0x0400039A RID: 922
		public const string SR_BadgeItemPlural4 = "BadgeItemPlural4";

		// Token: 0x0400039B RID: 923
		public const string SR_BadgeItemPlural5 = "BadgeItemPlural5";

		// Token: 0x0400039C RID: 924
		public const string SR_BadgeItemPlural6 = "BadgeItemPlural6";

		// Token: 0x0400039D RID: 925
		public const string SR_BadgeItemPlural7 = "BadgeItemPlural7";

		// Token: 0x0400039E RID: 926
		public const string SR_BadgeItemSingular = "BadgeItemSingular";

		// Token: 0x0400039F RID: 927
		public const string SR_BadgeItemTextOverride = "BadgeItemTextOverride";

		// Token: 0x040003A0 RID: 928
		public const string SR_BadgeIcon = "BadgeIcon";

		// Token: 0x040003A1 RID: 929
		public const string SR_BadgeIconTextOverride = "BadgeIconTextOverride";

		// Token: 0x040003A2 RID: 930
		public const string SR_PersonName = "PersonName";

		// Token: 0x040003A3 RID: 931
		public const string SR_GroupName = "GroupName";

		// Token: 0x040003A4 RID: 932
		public const string SR_CancelDraggingString = "CancelDraggingString";

		// Token: 0x040003A5 RID: 933
		public const string SR_DefaultItemString = "DefaultItemString";

		// Token: 0x040003A6 RID: 934
		public const string SR_DropIntoNodeString = "DropIntoNodeString";

		// Token: 0x040003A7 RID: 935
		public const string SR_FallBackPlaceString = "FallBackPlaceString";

		// Token: 0x040003A8 RID: 936
		public const string SR_PagerControlPageTextName = "PagerControlPageText";

		// Token: 0x040003A9 RID: 937
		public const string SR_PagerControlPrefixTextName = "PagerControlPrefixText";

		// Token: 0x040003AA RID: 938
		public const string SR_PagerControlSuffixTextName = "PagerControlPrefixText";

		// Token: 0x040003AB RID: 939
		public const string SR_PagerControlFirstPageButtonTextName = "PagerControlFirstPageButtonText";

		// Token: 0x040003AC RID: 940
		public const string SR_PagerControlPreviousPageButtonTextName = "PagerControlPreviousPageButtonText";

		// Token: 0x040003AD RID: 941
		public const string SR_PagerControlNextPageButtonTextName = "PagerControlNextPageButtonText";

		// Token: 0x040003AE RID: 942
		public const string SR_PagerControlLastPageButtonTextName = "PagerControlLastPageButtonText";

		// Token: 0x040003AF RID: 943
		public const string SR_PipsPagerNameText = "PipsPagerNameText";

		// Token: 0x040003B0 RID: 944
		public const string SR_PipsPagerNextPageButtonText = "PipsPagerNextPageButtonText";

		// Token: 0x040003B1 RID: 945
		public const string SR_PipsPagerPreviousPageButtonText = "PipsPagerPreviousPageButtonText";

		// Token: 0x040003B2 RID: 946
		public const string SR_PipsPagerPageText = "PipsPagerPageText";

		// Token: 0x040003B3 RID: 947
		public const string SR_PlaceAfterString = "PlaceAfterString";

		// Token: 0x040003B4 RID: 948
		public const string SR_PlaceBeforeString = "PlaceBeforeString";

		// Token: 0x040003B5 RID: 949
		public const string SR_PlaceBetweenString = "PlaceBetweenString";

		// Token: 0x040003B6 RID: 950
		public const string SR_ProgressRingName = "ProgressRingName";

		// Token: 0x040003B7 RID: 951
		public const string SR_ProgressRingIndeterminateStatus = "ProgressRingIndeterminateStatus";

		// Token: 0x040003B8 RID: 952
		public const string SR_ProgressBarIndeterminateStatus = "ProgressBarIndeterminateStatus";

		// Token: 0x040003B9 RID: 953
		public const string SR_ProgressBarPausedStatus = "ProgressBarPausedStatus";

		// Token: 0x040003BA RID: 954
		public const string SR_ProgressBarErrorStatus = "ProgressBarErrorStatus";

		// Token: 0x040003BB RID: 955
		public const string SR_RatingLocalizedControlType = "RatingLocalizedControlType";

		// Token: 0x040003BC RID: 956
		public const string SR_SplitButtonSecondaryButtonName = "SplitButtonSecondaryButtonName";

		// Token: 0x040003BD RID: 957
		public const string SR_ProofingMenuItemLabel = "ProofingMenuItemLabel";

		// Token: 0x040003BE RID: 958
		public const string SR_TextCommandLabelCut = "TextCommandLabelCut";

		// Token: 0x040003BF RID: 959
		public const string SR_TextCommandLabelCopy = "TextCommandLabelCopy";

		// Token: 0x040003C0 RID: 960
		public const string SR_TextCommandLabelPaste = "TextCommandLabelPaste";

		// Token: 0x040003C1 RID: 961
		public const string SR_TextCommandLabelSelectAll = "TextCommandLabelSelectAll";

		// Token: 0x040003C2 RID: 962
		public const string SR_TextCommandLabelBold = "TextCommandLabelBold";

		// Token: 0x040003C3 RID: 963
		public const string SR_TextCommandLabelItalic = "TextCommandLabelItalic";

		// Token: 0x040003C4 RID: 964
		public const string SR_TextCommandLabelUnderline = "TextCommandLabelUnderline";

		// Token: 0x040003C5 RID: 965
		public const string SR_TextCommandLabelUndo = "TextCommandLabelUndo";

		// Token: 0x040003C6 RID: 966
		public const string SR_TextCommandLabelRedo = "TextCommandLabelRedo";

		// Token: 0x040003C7 RID: 967
		public const string SR_TextCommandDescriptionCut = "TextCommandDescriptionCut";

		// Token: 0x040003C8 RID: 968
		public const string SR_TextCommandDescriptionCopy = "TextCommandDescriptionCopy";

		// Token: 0x040003C9 RID: 969
		public const string SR_TextCommandDescriptionPaste = "TextCommandDescriptionPaste";

		// Token: 0x040003CA RID: 970
		public const string SR_TextCommandDescriptionSelectAll = "TextCommandDescriptionSelectAll";

		// Token: 0x040003CB RID: 971
		public const string SR_TextCommandDescriptionBold = "TextCommandDescriptionBold";

		// Token: 0x040003CC RID: 972
		public const string SR_TextCommandDescriptionItalic = "TextCommandDescriptionItalic";

		// Token: 0x040003CD RID: 973
		public const string SR_TextCommandDescriptionUnderline = "TextCommandDescriptionUnderline";

		// Token: 0x040003CE RID: 974
		public const string SR_TextCommandDescriptionUndo = "TextCommandDescriptionUndo";

		// Token: 0x040003CF RID: 975
		public const string SR_TextCommandDescriptionRedo = "TextCommandDescriptionRedo";

		// Token: 0x040003D0 RID: 976
		public const string SR_TextCommandKeyboardAcceleratorKeyCut = "TextCommandKeyboardAcceleratorKeyCut";

		// Token: 0x040003D1 RID: 977
		public const string SR_TextCommandKeyboardAcceleratorKeyCopy = "TextCommandKeyboardAcceleratorKeyCopy";

		// Token: 0x040003D2 RID: 978
		public const string SR_TextCommandKeyboardAcceleratorKeyPaste = "TextCommandKeyboardAcceleratorKeyPaste";

		// Token: 0x040003D3 RID: 979
		public const string SR_TextCommandKeyboardAcceleratorKeySelectAll = "TextCommandKeyboardAcceleratorKeySelectAll";

		// Token: 0x040003D4 RID: 980
		public const string SR_TextCommandKeyboardAcceleratorKeyBold = "TextCommandKeyboardAcceleratorKeyBold";

		// Token: 0x040003D5 RID: 981
		public const string SR_TextCommandKeyboardAcceleratorKeyItalic = "TextCommandKeyboardAcceleratorKeyItalic";

		// Token: 0x040003D6 RID: 982
		public const string SR_TextCommandKeyboardAcceleratorKeyUnderline = "TextCommandKeyboardAcceleratorKeyUnderline";

		// Token: 0x040003D7 RID: 983
		public const string SR_TextCommandKeyboardAcceleratorKeyUndo = "TextCommandKeyboardAcceleratorKeyUndo";

		// Token: 0x040003D8 RID: 984
		public const string SR_TextCommandKeyboardAcceleratorKeyRedo = "TextCommandKeyboardAcceleratorKeyRedo";

		// Token: 0x040003D9 RID: 985
		public const string SR_TeachingTipAlternateCloseButtonName = "TeachingTipAlternateCloseButtonName";

		// Token: 0x040003DA RID: 986
		public const string SR_TeachingTipAlternateCloseButtonTooltip = "TeachingTipAlternateCloseButtonTooltip";

		// Token: 0x040003DB RID: 987
		public const string SR_TeachingTipCustomLandmarkName = "TeachingTipCustomLandmarkName";

		// Token: 0x040003DC RID: 988
		public const string SR_TeachingTipNotification = "TeachingTipNotification";

		// Token: 0x040003DD RID: 989
		public const string SR_TeachingTipNotificationWithoutAppName = "TeachingTipNotificationWithoutAppName";

		// Token: 0x040003DE RID: 990
		public const string SR_TabViewAddButtonName = "TabViewAddButtonName";

		// Token: 0x040003DF RID: 991
		public const string SR_TabViewAddButtonTooltip = "TabViewAddButtonTooltip";

		// Token: 0x040003E0 RID: 992
		public const string SR_TabViewCloseButtonName = "TabViewCloseButtonName";

		// Token: 0x040003E1 RID: 993
		public const string SR_TabViewCloseButtonTooltip = "TabViewCloseButtonTooltip";

		// Token: 0x040003E2 RID: 994
		public const string SR_TabViewCloseButtonTooltipWithKA = "TabViewCloseButtonTooltipWithKA";

		// Token: 0x040003E3 RID: 995
		public const string SR_TabViewScrollDecreaseButtonTooltip = "TabViewScrollDecreaseButtonTooltip";

		// Token: 0x040003E4 RID: 996
		public const string SR_TabViewScrollIncreaseButtonTooltip = "TabViewScrollIncreaseButtonTooltip";

		// Token: 0x040003E5 RID: 997
		public const string SR_NumberBoxUpSpinButtonName = "NumberBoxUpSpinButtonName";

		// Token: 0x040003E6 RID: 998
		public const string SR_NumberBoxDownSpinButtonName = "NumberBoxDownSpinButtonName";

		// Token: 0x040003E7 RID: 999
		public const string SR_ExpanderDefaultControlName = "ExpanderDefaultControlName";

		// Token: 0x040003E8 RID: 1000
		public const string SR_InfoBarCloseButtonName = "InfoBarCloseButtonName";

		// Token: 0x040003E9 RID: 1001
		public const string SR_InfoBarOpenedNotification = "InfoBarOpenedNotification";

		// Token: 0x040003EA RID: 1002
		public const string SR_InfoBarClosedNotification = "InfoBarClosedNotification";

		// Token: 0x040003EB RID: 1003
		public const string SR_InfoBarCustomLandmarkName = "InfoBarCustomLandmarkName";

		// Token: 0x040003EC RID: 1004
		public const string SR_InfoBarCloseButtonTooltip = "InfoBarCloseButtonTooltip";

		// Token: 0x040003ED RID: 1005
		public const string IR_NoiseAsset_256X256_PNG = "NoiseAsset_256X256_PNG";

		// Token: 0x040003EE RID: 1006
		private readonly Type _controlType;

		// Token: 0x040003EF RID: 1007
		private ResourceManager _resourceManager;
	}
}
