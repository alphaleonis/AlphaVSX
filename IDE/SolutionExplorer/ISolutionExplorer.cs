using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public interface ISolutionExplorer
   {
      ISolutionNode Solution { get; }
   }

   [Component(true)]
   internal class SolutionExplorer : ISolutionExplorer
   {
      private readonly IServiceProvider m_serviceProvider;
      private readonly ISolutionExplorerNodeFactory m_nodeFactory;

      public SolutionExplorer(IServiceProvider serviceProvider, ISolutionExplorerNodeFactory nodeFactory)
      {
         m_nodeFactory = nodeFactory;
         m_serviceProvider = serviceProvider;
      }

      public ISolutionNode Solution
      {
         get
         {
            return (ISolutionNode)m_nodeFactory.Create(new VsSolutionHierarchyNode(m_serviceProvider.GetService<IVsSolution>() as IVsHierarchy, VSConstants.VSITEMID_ROOT), null);
         }
      }      
   }
}

