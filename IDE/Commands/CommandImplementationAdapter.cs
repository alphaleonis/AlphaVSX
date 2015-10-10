using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   public class CommandImplementationAdapter : OleMenuCommand
   {
      public CommandImplementationAdapter(CommandID id, ICommandImplementation implementation)
          : base(OnExecute, null, OnBeforeQueryStatus, id)
      {
         Implementation = implementation;
      }

      protected virtual bool IsDynamic
      {
         get
         {
            return false;
         }
      }

      protected ICommandImplementation Implementation { get; }

      private static void OnExecute(object sender, EventArgs e)
      {
         var command = (CommandImplementationAdapter)sender;
         var menu = new OleMenuCommmandAdapter(command);

         command.Implementation.QueryStatus(menu);

         if (menu.Enabled)
         {
            command.Implementation.Execute(menu);
         }
      }

      private static void OnBeforeQueryStatus(object sender, EventArgs e)
      {
         var command = (CommandImplementationAdapter)sender;

         command.Enabled = false;
         var menu = new OleMenuCommmandAdapter(command);

         try
         {
            command.Implementation.QueryStatus(menu);
            command.Enabled = menu.Enabled;
            command.Visible = menu.Visible;
            command.Text = menu.Text;
            command.Checked = menu.Checked;
         }
         catch (Exception)
         {
            command.Enabled = command.Visible = false;
            throw;
         }
         finally
         {
            command.MatchedCommandId = 0;
         }
      }
   }
}

