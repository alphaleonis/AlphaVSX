using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using VSLangProj;
using VSLangProj80;

namespace Alphaleonis.Vsx
{
   public static class DTEExtensions
   {  
      public static IEnumerable<ProjectItem> DescendantProjectItems(this _Solution solution)
      {
         return solution?.Projects?.Cast<Project>()?.SelectMany(proj => proj.DescendantProjectItems()) ?? Enumerable.Empty<ProjectItem>();
      }

      public static IEnumerable<ProjectItem> DescendantProjectItems(this Project project)
      {
         Stack<ProjectItems> stack = new Stack<ProjectItems>();

         if (project?.ProjectItems != null)
         {
            stack.Push(project.ProjectItems);

            while (stack.Count > 0)
            {
               ProjectItems current = stack.Pop();
               
               for (int i = 1; i <= current.Count; i++)
               {
                  ProjectItem item = current.Item(i);
                  if (item.SubProject != null)
                  {
                     if (item.SubProject.ProjectItems != null && item.SubProject.ProjectItems.Count > 0)
                        stack.Push(item.SubProject.ProjectItems);
                  }
                  else if (item.ProjectItems != null && item.ProjectItems.Count > 0)
                  {
                     stack.Push(item.ProjectItems);
                  }

                  yield return item;
               }
            }
         }
      }      

      public static T TryGetProperty<T>(this ProjectItem projectItem, string propertyName, T defaultValue = default(T))
      {
         try
         {
            Property property = projectItem?.Properties?.Item(propertyName);
            if (property != null && property.Value is T)
               return (T)property.Value;
         }
         catch (ArgumentException)
         {
         }

         return defaultValue;
      }
   }
}
