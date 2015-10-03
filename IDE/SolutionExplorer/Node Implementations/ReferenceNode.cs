using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class ReferenceNode : ProjectItemNode, IReferenceNode
   {
      private readonly Lazy<VSLangProj.Reference> m_dteReference;

      public ReferenceNode(VsSolutionHierarchyNode hierarchyNode, Lazy<ISolutionExplorerNode> parentNode, ISolutionExplorerNodeFactory nodeFactory)
          : base(SolutionExplorerNodeKind.Reference, hierarchyNode, parentNode, nodeFactory)
      {      
         m_dteReference = new Lazy<VSLangProj.Reference>(() => (VSLangProj.Reference)hierarchyNode.ExtenderObject);         
      }


      public string Description
      {
         get
         {            
            return m_dteReference.Value?.Description;
         }
      }

      public bool CopyLocal
      {
         get
         {
            return m_dteReference.Value.CopyLocal;
         }

         set
         {
            m_dteReference.Value.CopyLocal = true;
         }
      }

      public string Path
      {
         get
         {
            return m_dteReference.Value.Path;            
         }
      }

      public void Remove()
      {
         m_dteReference.Value.Remove();
      }

      public override void Accept(SolutionHierarchyVisitor visitor)
      {         
         visitor.Visit(this);
      }
   }
}