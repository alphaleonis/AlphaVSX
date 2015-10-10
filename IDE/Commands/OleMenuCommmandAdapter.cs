using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Vsx.IDE
{
   internal class OleMenuCommmandAdapter : IMenuCommand
   {
      private readonly OleMenuCommand m_command;

      public OleMenuCommmandAdapter(OleMenuCommand command)
      {
         m_command = command;
      }

      public bool Checked
      {
         get
         {
            return m_command.Checked;
         }

         set
         {
            m_command.Checked = value;
         }
      }

      public Guid CommandSetId
      {
         get
         {
            return m_command.CommandID.Guid;
         }
      }

      public int CommandID
      {
         get
         {
            return m_command.CommandID.ID;
         }
      }

      public int MatchedCommandID
      {
         get
         {
            return m_command.MatchedCommandId;
         }
      }

      public int CommandIndex
      {
         get
         {
            return m_command.MatchedCommandId == 0 ? 0 : m_command.MatchedCommandId - m_command.CommandID.ID;
         }
      }

      public bool Enabled
      {
         get
         {
            return m_command.Enabled;
         }

         set
         {
            m_command.Enabled = value;
         }
      }

      public string Text
      {
         get
         {
            return m_command.Text;
         }

         set
         {
            m_command.Text = value;
         }
      }

      public bool Visible
      {
         get
         {
            return m_command.Visible;
         }

         set
         {
            m_command.Visible = value;
         }
      }
   }
}

