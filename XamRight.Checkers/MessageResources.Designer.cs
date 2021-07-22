﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XamRight.Checkers {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MessageResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MessageResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("XamRight.Checkers.MessageResources", typeof(MessageResources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add &apos;{0}&apos; suffix.
        /// </summary>
        internal static string AddSuffixCodeFixMessage {
            get {
                return ResourceManager.GetString("AddSuffixCodeFixMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Contents of BindableLayout.ItemTemplate&apos;s DataTemplate cannot be wrapped in a ViewCell.
        /// </summary>
        internal static string BindableLayoutDataTemplateWarning {
            get {
                return ResourceManager.GetString("BindableLayoutDataTemplateWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BindableLayout.ItemTemplate can only have one child node and the child node must be a DataTemplate tag.
        /// </summary>
        internal static string BindableLayoutItemTemplateWarning {
            get {
                return ResourceManager.GetString("BindableLayoutItemTemplateWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Contents of CollectionView.ItemTemplate&apos;s DataTemplate cannot be wrapped in a ViewCell.
        /// </summary>
        internal static string CollectionViewDataTemplateContentWarning {
            get {
                return ResourceManager.GetString("CollectionViewDataTemplateContentWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CollectionView.ItemTemplate can only have one child node and the child node must be a DataTemplate tag.
        /// </summary>
        internal static string CollectionViewItemTemplateContentWarning {
            get {
                return ResourceManager.GetString("CollectionViewItemTemplateContentWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The method added to the CollectionView&apos;s SelectionChanged command will not execute because the CollectionView&apos;s SelectionMode is set to SelectionMode.None.
        /// </summary>
        internal static string CollectionViewSelectionChangedWarning {
            get {
                return ResourceManager.GetString("CollectionViewSelectionChangedWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete unnecessary {0}.
        /// </summary>
        internal static string DeleteUnnecessaryAttributeCodeFixMessage {
            get {
                return ResourceManager.GetString("DeleteUnnecessaryAttributeCodeFixMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property Event on EventTrigger does not have a value.
        /// </summary>
        internal static string EventTriggerEventEmptyWarning {
            get {
                return ResourceManager.GetString("EventTriggerEventEmptyWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; declaration on child view of AbsoluteLayout is ignored. Use AbsoluteLayout.LayoutBounds property to position views.
        /// </summary>
        internal static string IgnoredLayoutAttrOnAbsoluteLayoutWarning {
            get {
                return ResourceManager.GetString("IgnoredLayoutAttrOnAbsoluteLayoutWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Insert &apos;{0}&apos; property.
        /// </summary>
        internal static string InsertPropertyCodeFixMessage {
            get {
                return ResourceManager.GetString("InsertPropertyCodeFixMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataTemplate for &apos;{0}&apos; can only have one child that must be of, or derive from, type ViewCell.
        /// </summary>
        internal static string ListViewDataTemplateContentWarning {
            get {
                return ResourceManager.GetString("ListViewDataTemplateContentWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The DataTemplate layout for &apos;{0}&apos; cannot be wrapped in a ViewCell.
        /// </summary>
        internal static string ListViewDataTemplateForHeaderFooterContentWarning {
            get {
                return ResourceManager.GetString("ListViewDataTemplateForHeaderFooterContentWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataTemplate can only have one child.
        /// </summary>
        internal static string ListViewDataTemplateMoreThanOneChildWarnning {
            get {
                return ResourceManager.GetString("ListViewDataTemplateMoreThanOneChildWarnning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HasUnevenRows on &apos;{0}&apos; is not set to true, so cell contents may not be fully visible when using accessibility setting to increase font size .
        /// </summary>
        internal static string ListViewDoesNotSetHasUnevenRowsWarning {
            get {
                return ResourceManager.GetString("ListViewDoesNotSetHasUnevenRowsWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} can only have one child node and the child node must be a DataTemplate tag.
        /// </summary>
        internal static string ListViewItemTemplateContentWarning {
            get {
                return ResourceManager.GetString("ListViewItemTemplateContentWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Layout in the &apos;{0}&apos; may be missing AutomationProperties to inform screen reader users about the context actions available for items in the &apos;{1}&apos;. Consider adding AutomationProperties describing what context actions are available and how to access them.
        /// </summary>
        internal static string MissingAutomationPropertiesToHintListViewContextActionsWarning {
            get {
                return ResourceManager.GetString("MissingAutomationPropertiesToHintListViewContextActionsWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The text set for the &apos;{0}&apos; property, to identify or describe the &apos;{1}&apos; control is also used on &apos;{2}&apos;. Consider setting unique descriptions for each control to distinguish their purpose, especially for screen reader users..
        /// </summary>
        internal static string MultipleElementsWithIdenticalTextWarning {
            get {
                return ResourceManager.GetString("MultipleElementsWithIdenticalTextWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do not nest a &apos;{0}&apos; within another scrollable layout; see parent layout on line &apos;{1}&apos;.
        /// </summary>
        internal static string NestedScrollableLayoutsWarning {
            get {
                return ResourceManager.GetString("NestedScrollableLayoutsWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EventTrigger does not serve a purpose because the Event property is not set.
        /// </summary>
        internal static string NoEventPropertyOnEventTriggerWarning {
            get {
                return ResourceManager.GetString("NoEventPropertyOnEventTriggerWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inaccurate usage of &apos;AndExpand&apos; suffix on &apos;{0}&apos; Options declaration. &apos;Expand&apos; is only relevant when applied to LayoutOptions in the direction of the Stack Orientation.
        /// </summary>
        internal static string RedundantLayoutExpansionOnStackLayoutWarning {
            get {
                return ResourceManager.GetString("RedundantLayoutExpansionOnStackLayoutWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; declaration on child view of RelativeLayout is ignored. Use RelativeLayout&apos;s Constraint properties to position views.
        /// </summary>
        internal static string RedundantLayoutOptionsOnRelativeLayoutChildWarning {
            get {
                return ResourceManager.GetString("RedundantLayoutOptionsOnRelativeLayoutChildWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Redundant &apos;{0}&apos; Options declaration. Start, Center, End, and Fill only apply to Layout Options opposite the direction of the Stack Orientation.
        /// </summary>
        internal static string RedundantLayoutOptionsOnStackLayoutWarning {
            get {
                return ResourceManager.GetString("RedundantLayoutOptionsOnStackLayoutWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remove {0}.
        /// </summary>
        internal static string RemoveCodeFixMessage {
            get {
                return ResourceManager.GetString("RemoveCodeFixMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Remove &apos;{0}&apos; suffix.
        /// </summary>
        internal static string RemoveSuffixCodeFixMessage {
            get {
                return ResourceManager.GetString("RemoveSuffixCodeFixMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Replace value of &apos;{0}&apos; with &apos;{1}&apos;.
        /// </summary>
        internal static string ReplaceAttributeValueCodeFixMessage {
            get {
                return ResourceManager.GetString("ReplaceAttributeValueCodeFixMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to StackLayout element has a single child {0}; consider removing StackLayout to improve layout performance.
        /// </summary>
        internal static string StackLayoutOneChildWarning {
            get {
                return ResourceManager.GetString("StackLayoutOneChildWarning", resourceCulture);
            }
        }
    }
}