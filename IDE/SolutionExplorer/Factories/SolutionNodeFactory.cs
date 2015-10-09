using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class SolutionNodeFactory : ISolutionExplorerNodeFactory
   {
      private readonly Lazy<ISolutionExplorerNodeFactory> m_nodeFactory;

      public SolutionNodeFactory(Lazy<ISolutionExplorerNodeFactory> nodeFactory)
      {
         m_nodeFactory = nodeFactory;
      }

      public bool CanCreateFrom(VsSolutionHierarchyNode hierarchyNode)
      {
         return hierarchyNode.VsHierarchy is IVsSolution;
      }

      public ISolutionExplorerNode Create(VsSolutionHierarchyNode hierarchy, Lazy<ISolutionExplorerNode> parent)
      {
         if (hierarchy == null)
            throw new ArgumentNullException(nameof(hierarchy), $"{nameof(hierarchy)} is null.");

         if (parent != null)
            throw new ArgumentException($"{nameof(parent)} must be null for {nameof(SolutionNode)} creation.");

         return CanCreateFrom(hierarchy) ? new SolutionNode(hierarchy, m_nodeFactory.Value) : null;
      }
   }

   
}

