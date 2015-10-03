using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public class SolutionHierarchyVisitor
   {
      public virtual void Visit(ISolutionExplorerNode node)
      {
         if (node != null)
         {
            node.Accept(this);
         }
      }

      public virtual void Visit(ISolutionNode node)
      {
         DefaultVisit(node);
      }

      public virtual void Visit(ISolutionItemNode node)
      {
         DefaultVisit(node);
      }

      public virtual void Visit(IReferencesFolderNode node)
      {
         DefaultVisit(node);
      }

      public virtual void Visit(IReferenceNode node)
      {
         DefaultVisit(node);
      }

      public virtual void Visit(IProjectNode node)
      {
         DefaultVisit(node);
      }

      public virtual void Visit(IItemNode node)
      {
         DefaultVisit(node);
      }

      public virtual void Visit(IFolderNode node)
      {
         DefaultVisit(node);
      }

      public virtual void Visit(ISolutionFolderNode node)
      {
         DefaultVisit(node);
      }

      protected virtual void DefaultVisit(ISolutionExplorerNode node)
      {
      }
   }
}