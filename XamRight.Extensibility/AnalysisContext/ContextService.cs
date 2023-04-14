// Copyright 2021 Critical Hit Technologies, LLC. All rights reserved.
// See LICENSE file in the project root for full license information.  
using System;
using System.Collections.Generic;
using System.Text;
using XamRight.Extensibility.Xml;

namespace XamRight.Extensibility.AnalysisContext
{
    public enum XamarinFormsVersionEnum
    {
        Pre_V_3_5,
        V_3_5,
        V_4_0,
        V_4_3,
        V_4_7
    }

    public enum MauiVersionEnum
    {
        Maui6,
        Maui7,
    }

    public abstract class ContextService
    {
        public abstract bool IsXamarinForms { get; }
        public abstract bool IsWpf { get; }
        public abstract bool IsMaui { get; }

        public abstract bool IsXamarinFormsVersionSupported(XamarinFormsVersionEnum version);
        public abstract bool IsMauiVersionSupported(MauiVersionEnum version);

        /// <summary>
        /// The basetype string here can be the fully qualified type name (eg. Xamarin.Forms.ListView) or the short name (eg. ListView) 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="basetype"></param>
        /// <returns></returns>
        public abstract bool DoesNodeSymbolDeriveFromBaseType(IXmlSyntaxNode node, string basetype);

        public bool DoesNodeSymbolDeriveFromAnyBaseTypes(IXmlSyntaxNode node, string[] baseTypes)
        {
            foreach (var baseType in baseTypes)
            {
                if (DoesNodeSymbolDeriveFromBaseType(node, baseType))
                    return true;
            }
            return false;
        }
    }
}
