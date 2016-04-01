using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx.IDE
{
   internal class ItemProperties : DynamicObject
   {
      private readonly VsSolutionHierarchyNode m_node;
      private readonly Lazy<ProjectItem> m_dteItem;
      private readonly IVsBuildPropertyStorage m_vsBuildPropertyStorage;

      public ItemProperties(ItemNode item)
      {
         m_node = item.HierarchyNode;
         m_dteItem = item.m_dteProjectItem;
         m_vsBuildPropertyStorage = item.HierarchyNode.VsHierarchy as IVsBuildPropertyStorage;
      }

      public override IEnumerable<string> GetDynamicMemberNames()
      {
         try
         {
            return m_dteItem.Value.Properties?.Cast<Property>()?.Select(prop => prop.Name) ?? Enumerable.Empty<string>();
         }
         catch
         {
            return Enumerable.Empty<string>();
         }
      }

      public override bool TrySetMember(SetMemberBinder binder, object value)
      {
         if (value == null)
            throw new NotSupportedException("Cannot set null value for item properties.");

         Property property;
         try
         {
            property = m_dteItem?.Value?.Properties?.Item(binder.Name);
         }
         catch (ArgumentException)
         {
            property = null;
         }

         if (property != null)
         {
            try
            {
               property.Value = value;
               return true;
            }
            catch
            {
               return false;
            }
         }

         // Fallback to MSBuild item properties.
         if (m_vsBuildPropertyStorage != null)
         {
            return ErrorHandler.Succeeded(m_vsBuildPropertyStorage.SetItemAttribute(m_node.ItemId, binder.Name, value?.ToString()));
         }

         return false;
      }

      public override bool TryGetMember(GetMemberBinder binder, out object result)
      {         
         if (m_dteItem != null)
         {
            Property property;
            try
            {
               property = m_dteItem?.Value?.Properties?.Item(binder.Name);
            }
            catch (ArgumentException)
            {
               property = null;
            }

            if (property != null)
            {
               result = property.Value;
               return true;
            }
         }

         if (m_vsBuildPropertyStorage != null)
         {
            string str;
            if (ErrorHandler.Succeeded(m_vsBuildPropertyStorage.GetItemAttribute(m_node.ItemId, binder.Name, out str)))
            {
               result = str;
               return true;
            }
         }

         result = null;
         return false;
      }           
   }
}