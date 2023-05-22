using System;
using System.Collections.Generic;
using System.Text;

namespace XamRight.Checkers.Infrastructure
{
    internal class AccessibilityProperties
    {
        public const string AutomationPropertiesIsInAccessibleTreeProp = "AutomationProperties.IsInAccessibleTree";
        public const string AutomationPropertiesNameProp = "AutomationProperties.Name";
        public const string AutomationPropertiesHelpTextProp = "AutomationProperties.HelpText";
        public const string AutomationPropertiesLabeledByProp = "AutomationProperties.LabeledBy";
        public const string AutomationPropertiesLiveSettingProp = "AutomationProperties.LiveSetting";

        public const string SemanticPropertiesDescriptionProp = "SemanticProperties.Description";
        public const string SemanticPropertiesHintProp = "SemanticProperties.Hint";

        #region Xamarin properties
        public static readonly HashSet<string> XamAutomationProperties = new HashSet<string>
        {
            AutomationPropertiesNameProp,
            AutomationPropertiesHelpTextProp,
            AutomationPropertiesLabeledByProp,
        };
        #endregion

        #region MAUI
        public static readonly HashSet<string> MauiAutomationProperties = new HashSet<string>
        {
            AutomationPropertiesNameProp,
            AutomationPropertiesHelpTextProp,
            AutomationPropertiesLabeledByProp,
            SemanticPropertiesDescriptionProp,
            SemanticPropertiesHintProp
        };
        #endregion

        #region WPF properties
        public static readonly HashSet<string> WpfAutomationProperties = new HashSet<string>
        {
            AutomationPropertiesNameProp,
            AutomationPropertiesHelpTextProp,
            AutomationPropertiesLiveSettingProp
        };
        #endregion
    }
}
