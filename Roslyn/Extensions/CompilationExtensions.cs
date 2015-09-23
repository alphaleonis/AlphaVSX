using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Alphaleonis.Vsx
{
   public static class CompilationExtensions
   {
      public static INamedTypeSymbol RequireTypeByMetadataName(this Compilation semanticModel, string fullyQualifiedMetadataName)
      {
         INamedTypeSymbol type = semanticModel.GetTypeByMetadataName(fullyQualifiedMetadataName);
         if (type == null)
            throw new TypeNotFoundException($"Could not find the type named \"{fullyQualifiedMetadataName}\". Are you missing an assembly reference?");

         return type;
      }
   }





}
