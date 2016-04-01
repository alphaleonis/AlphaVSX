using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal sealed class ProjectProperties : DynamicObject
   {
      private readonly Lazy<EnvDTE.Project> dteProject;
      private readonly IVsBuildPropertyStorage vsBuild;

      public ProjectProperties(ProjectNode project)
      {
         dteProject = project.m_dteProject;
         vsBuild = project.HierarchyNode.VsHierarchy as IVsBuildPropertyStorage;
      }

      public override IEnumerable<string> GetDynamicMemberNames()
      {
         try
         {
            return dteProject?.Value?.Properties?.Cast<Property>()?.Select(prop => prop.Name) ?? Enumerable.Empty<string>();
         }
         catch
         {
            return Enumerable.Empty<string>();
         }

      }

      public override bool TryGetMember(GetMemberBinder binder, out object result)
      {
         if (dteProject != null)
         {
            Property property;
            try
            {
               property = dteProject?.Value?.Properties?.Item(binder.Name);
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

         result = null;
         return false;
      }

      public override bool TrySetMember(SetMemberBinder binder, object value)
      {
         if (value == null)
            throw new NotSupportedException("Cannot set null value for item properties.");

         if (dteProject != null)
         {
            Property property;
            try
            {
               property = dteProject?.Value?.Properties.Item(binder.Name);
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
         }

         return false;
      }
   }
}

