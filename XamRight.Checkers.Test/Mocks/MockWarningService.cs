// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamRight.Extensibility.Warnings;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Test.Mocks
{
    public class MockWarningService : WarningService
    {
        public List<MockWarningData> Warnings { get; } = new List<MockWarningData>();
        public List<MockCodeFixData> CodeFixes { get; } = new List<MockCodeFixData>();

        public override void AddInsertAttributeCodeFix(IXmlSyntaxNode node, WarningDefinition warning, string newAttrName, string newAttrValue, string codeFixHintMsgFormat, params string[] hintParams)
        {
            CodeFixes.Add(new MockCodeFixData()
            {
                Kind = MockCodeFixData.FixKind.AddAttribute,
                Node = node,
                Attribute = null,
                CodeFixNameParam = newAttrName,
                CodeFixValueParam = newAttrValue,
                WarningDefinition = warning,
                HintFormat = codeFixHintMsgFormat,
                HintParams = hintParams
            });
        }

        public override void AddDeleteAttributeCodeFix(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute, WarningDefinition warning, string codeFixHintMsgFormat, params string[] hintParams)
        {
            CodeFixes.Add(new MockCodeFixData()
            {
                Kind = MockCodeFixData.FixKind.DeleteAttribute,
                Node = node,
                Attribute = attribute,
                WarningDefinition = warning,
                HintFormat = codeFixHintMsgFormat,
                HintParams = hintParams
            });
        }

        public override void AddReplaceAttributeValueCodeFix(IXmlSyntaxNode node, IXmlSyntaxAttribute attribute, WarningDefinition warning, string replaceValue, string codeFixHintMsgFormat, params string[] hintParams)
        {
            CodeFixes.Add(new MockCodeFixData()
            {
                Kind = MockCodeFixData.FixKind.ReplaceAttribute,
                Node = node,
                Attribute = attribute,
                CodeFixValueParam = replaceValue,
                WarningDefinition = warning,
                HintFormat = codeFixHintMsgFormat,
                HintParams = hintParams
            });
        }

        public override void AddWarning(IXmlSyntaxNode node, IXmlFilePosition filePosition, int length, WarningDefinition warning, params string[] values)
        {
            Warnings.Add(new MockWarningData()
            {
                Node = node,
                FilePosition = filePosition,
                Length = length,
                WarningDefinition = warning,
                MsgParams = values
            });
        }
    }
}
