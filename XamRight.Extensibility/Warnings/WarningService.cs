// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Extensibility.Xml;

namespace XamRight.Extensibility.Warnings
{
    public abstract class WarningService
    {
        public abstract void AddWarning(IXmlSyntaxNode node, IXmlFilePosition filePosition, int length, WarningDefinition warning, params string[] values);
        public abstract void AddInsertAttributeCodeFix(IXmlSyntaxNode node, WarningDefinition warning, string newAttrName, string newAttrValue, string codeFixHintMsgFormat, params string[] hintParams);
        public abstract void AddReplaceAttributeValueCodeFix(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute, WarningDefinition warning, string replaceValue, string codeFixHintMsgFormat, params string[] hintParams);
        public abstract void AddDeleteAttributeCodeFix(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute, WarningDefinition warning, string codeFixHintMsgFormat, params string[] hintParams);

        public void AddAttributeNameWarning(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute, WarningDefinition warning, params string[] values)
        {
            AddWarning(node, attribute.NamePosition, attribute.Name.Length, warning, values);
        }

        public void AddAttributeValueWarning(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute, WarningDefinition warning, params string[] values)
        {
            AddWarning(node, attribute.ValuePosition, attribute.Value.Length, warning, values);
        }

        public void AddNodeTagWarning(IXmlSyntaxNode node, WarningDefinition warning, params string[] values)
        {
            AddWarning(node, node.PositionOfOpenTag, node.Tag.Length, warning, values);
        }
    }
}
