using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.Vsx.IDE
{
   [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
   public sealed class CommandAttribute : Attribute
   {
      public CommandAttribute(string groupGuid, int commandId)
      {
         GroupId = groupGuid;
         CommandId = commandId;
      }

      public string GroupId { get; }

      public int CommandId { get; }
   }

}
