// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;

namespace XamRight.Extensibility.Xml
{
    public interface IXmlSyntaxNode
    {
        /// <summary>
        /// The name of the element (e.g. for &lt;Label .../&gt; the Tag is 'Label')
        /// </summary>
        string Tag { get; }
        /// <summary>
        /// The text value in between the Open and Close Tags. For example, for the node &lt;Button&gt;ButtonText&lt;/Button&gt;, "ButtonText" would be the Text value
        /// </summary>
        string Text { get; }
        /// <summary>
        /// Position of the first character of this Xml node (e.g. for &lt;StackLayout&gt; the position of the '&lt;')
        /// </summary>
        IXmlFilePosition PositionOfOpenTag { get; }
        /// <summary>
        /// If this is not a self-closing element, the position of the ending Xml node (e.g. for &lt;/StackLayout&gt; the position of the '/').
        /// For self-closing elements, this is null.
        /// </summary>
        IXmlFilePosition PositionOfCloseTag { get; }
        IReadOnlyList<IXmlSyntaxAttribute> Attributes { get; }
        IReadOnlyList<IXmlSyntaxNode> Children { get; }
        /// <summary>
        /// Parent Xml node, or null for the root
        /// </summary>
        IXmlSyntaxNode Parent { get; }

        /// <summary>
        /// Is this a self-closing element (e.g. &lt;Label/&gt;)?
        /// </summary>
        bool IsEmptyElement { get; }
    }
}
