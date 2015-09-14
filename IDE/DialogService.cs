
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace Alphaleonis.Vsx
{
   [Component(true)]   
   internal class DialogService : IDialogService
   {
      #region Private Fields

      private readonly IVsUIShell m_vsShell;

      #endregion

      #region Constructor

      [ImportingConstructor]
      public DialogService(IVsUIShell uiShell)
      {
         m_vsShell = uiShell;
      }

      #endregion

      #region Public Methods

      public MessageBoxResult ShowMessageBox(string message, string title = "Microsoft Visual Studio", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, MessageBoxResult defaultResult = MessageBoxResult.OK)
      {
         Guid classId = Guid.Empty;
         int result;

         m_vsShell.ShowMessageBox(0, ref classId, title, message, string.Empty, 0,
             ToOleButton(button),
             ToOleDefault(defaultResult, button),
             ToOleIcon(icon),
             0, out result);

         return FromOleResult(result);
      }

      #endregion

      #region Private Methods

      private static OLEMSGBUTTON ToOleButton(MessageBoxButton button)
      {
         switch (button)
         {
            case MessageBoxButton.OKCancel:
               return OLEMSGBUTTON.OLEMSGBUTTON_OKCANCEL;

            case MessageBoxButton.YesNo:
               return OLEMSGBUTTON.OLEMSGBUTTON_YESNO;

            case MessageBoxButton.YesNoCancel:
               return OLEMSGBUTTON.OLEMSGBUTTON_YESNOCANCEL;

            default:
               return OLEMSGBUTTON.OLEMSGBUTTON_OK;
         }
      }

      private static OLEMSGDEFBUTTON ToOleDefault(MessageBoxResult defaultResult, MessageBoxButton button)
      {
         switch (button)
         {
            case MessageBoxButton.OK:
               return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;

            case MessageBoxButton.OKCancel:
               if (defaultResult == MessageBoxResult.Cancel)
                  return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND;

               return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;

            case MessageBoxButton.YesNoCancel:
               if (defaultResult == MessageBoxResult.No)
                  return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND;
               if (defaultResult == MessageBoxResult.Cancel)
                  return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_THIRD;

               return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;

            case MessageBoxButton.YesNo:
               if (defaultResult == MessageBoxResult.No)
                  return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND;

               return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;
         }

         return OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST;
      }

      private static OLEMSGICON ToOleIcon(MessageBoxImage icon)
      {
         switch (icon)
         {
            case MessageBoxImage.Asterisk:
               return OLEMSGICON.OLEMSGICON_INFO;

            case MessageBoxImage.Error:
               return OLEMSGICON.OLEMSGICON_CRITICAL;

            case MessageBoxImage.Exclamation:
               return OLEMSGICON.OLEMSGICON_WARNING;

            case MessageBoxImage.Question:
               return OLEMSGICON.OLEMSGICON_QUERY;

            default:
               return OLEMSGICON.OLEMSGICON_NOICON;
         }
      }

      private static MessageBoxResult FromOleResult(int value)
      {
         switch (value)
         {
            case 1:
               return MessageBoxResult.OK;

            case 2:
               return MessageBoxResult.Cancel;

            case 6:
               return MessageBoxResult.Yes;

            case 7:
               return MessageBoxResult.No;
         }

         return MessageBoxResult.No;
      }

      internal static class OleMessageBoxResult
      {
         public const int IDABORT = 3;
         public const int IDCANCEL = 2;
         public const int IDCLOSE = 8;
         public const int IDCONTINUE = 11;
         public const int IDHELP = 9;
         public const int IDIGNORE = 5;
         public const int IDNO = 7;
         public const int IDOK = 1;
         public const int IDRETRY = 4;
         public const int IDTRYAGAIN = 10;
         public const int IDYES = 6;
      }
      #endregion
   }
}
