using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.EventSourceClassGenerator
{
   public static class ISymbolExtensions
   {
      public static ISymbol OverriddenMember(this ISymbol symbol)
      {
         switch (symbol.Kind)
         {
            case SymbolKind.Event:
               return ((IEventSymbol)symbol).OverriddenEvent;

            case SymbolKind.Method:
               return ((IMethodSymbol)symbol).OverriddenMethod;

            case SymbolKind.Property:
               return ((IPropertySymbol)symbol).OverriddenProperty;
         }

         return null;
      }
   }
}
