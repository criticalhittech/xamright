// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Test.XamlUtilities
{
    public class XmlParser
    {
        private string _filename;

        public XmlParser(string filename)
        {
            _filename = filename;
        }

        public IXmlSyntaxNode ParseFile()
        {
            using (var inputStream = new FileStream(_filename, FileMode.Open))
            {
                using (var reader = XmlReader.Create(inputStream))
                {
                    while (reader.NodeType != XmlNodeType.Element)
                        reader.Read();
                    return ParseSubtree(reader, null);
                }
            }
        }

        private static XmlFilePosition CurrentFilePostion(XmlReader reader)
        {
            if (reader is IXmlLineInfo xmlInfo)
                return new XmlFilePosition() { LineNumber = xmlInfo.LineNumber, ColumnNumber = xmlInfo.LinePosition };
            return null;
        }

        private XmlNode ParseSubtree(XmlReader subtreeReader, XmlNode parent)
        {
            var attributes = new List<XmlSyntaxAttribute>();
            var children = new List<XmlNode>();
            var result = new XmlNode()
            {
                Parent = parent,
                Tag = subtreeReader.Name,
                PositionOfOpenTag = CurrentFilePostion(subtreeReader),
                Attributes = attributes,
                Children = children,
                IsEmptyElement = subtreeReader.IsEmptyElement
            };

            if (subtreeReader.HasAttributes)
            {
                var attributeCount = subtreeReader.AttributeCount;
                for (int i = 0; i < attributeCount; i++)
                {
                    subtreeReader.MoveToAttribute(i);
                    var newAttribute = new XmlSyntaxAttribute()
                    {
                        Name = subtreeReader.Name,
                        Value = subtreeReader.Value,
                        NamePosition = CurrentFilePostion(subtreeReader)
                    };

                    subtreeReader.ReadAttributeValue();
                    newAttribute.ValuePosition = CurrentFilePostion(subtreeReader);
                    attributes.Add(newAttribute);
                }
            }

            while (subtreeReader.Read())
            {
                if (subtreeReader.NodeType == XmlNodeType.Text)
                {
                    result.Text = subtreeReader.Value;
                }
                if (subtreeReader.NodeType == XmlNodeType.Element)
                {
                    var innerReader = subtreeReader.ReadSubtree();
                    innerReader.Read();
                    var child = ParseSubtree(innerReader, result);
                    children.Add(child);
                }
                if (subtreeReader.NodeType == XmlNodeType.EndElement && subtreeReader.Name == result.Tag)
                {
                    result.PositionOfCloseTag = CurrentFilePostion(subtreeReader);
                }
            }

            return result;
        }
    }
}
