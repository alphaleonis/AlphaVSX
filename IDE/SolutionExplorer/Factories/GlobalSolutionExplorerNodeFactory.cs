using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class GlobalSolutionExplorerNodeFactory : ISolutionExplorerNodeFactory
   {
      private readonly IEnumerable<ISolutionExplorerNodeFactory> m_factories;

      public GlobalSolutionExplorerNodeFactory(IEnumerable<ISolutionExplorerNodeFactory> factories)
      {
         m_factories = factories;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchyNode)
      {
         return m_factories.Any(factory => factory.CanCreateFrom(hierarchyNode));
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         return m_factories.FirstOrDefault(factory => factory.CanCreateFrom(hierarchy))?.Create(hierarchy, parent);
      }
   }
}