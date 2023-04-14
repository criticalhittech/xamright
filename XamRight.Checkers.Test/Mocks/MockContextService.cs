// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamRight.Extensibility.AnalysisContext;
using XamRight.Extensibility.Xml;

namespace XamRight.Checkers.Test.Mocks
{
    public class MockContextService : ContextService
    {
        private bool isXamarinForms;
        private bool isMaui;
        private bool isWpf;

        public enum XamlMode
        {
            XamarinForms,
            Maui,
            Wpf
        }

        public MockContextService(XamlMode mode)
        {
            isXamarinForms = mode == XamlMode.XamarinForms;
            isWpf = mode == XamlMode.Wpf;
            isMaui = mode == XamlMode.Maui;
        }

        public override bool IsXamarinForms => isXamarinForms;
        public override bool IsWpf => isWpf;
        public override bool IsMaui => isMaui;

        public override bool DoesNodeSymbolDeriveFromBaseType(IXmlSyntaxNode node, string basetype)
        {
            if (basetype.Contains("."))
                basetype = basetype.Substring(basetype.LastIndexOf('.') + 1);

            if (node.Tag == basetype || node.Tag.EndsWith(basetype))
                return true;
            return false;
        }

        public override bool IsXamarinFormsVersionSupported(XamarinFormsVersionEnum version)
        {
            return IsXamarinForms;
        }

        public override bool IsMauiVersionSupported(MauiVersionEnum version)
        {
            return IsMaui;
        }
    }
}
