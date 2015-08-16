using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.EventSourceClassGenerator
{
   public static class INamespaceOrTypeSymbolExtensions
   {
      private static readonly SymbolDisplayFormat s_fullNameSymbolDisplayFormat = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces, genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);

      public static string GetFullName(this INamespaceOrTypeSymbol type)
      {
         if (type == null)
            return null;

         return type.ToDisplayString(s_fullNameSymbolDisplayFormat);
      }
   }
}
