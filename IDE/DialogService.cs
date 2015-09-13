
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace Alphaleonis.Vsx
{

   /// <summary>
   /// Default implementation of the <see cref="IMessageBoxService"/>.
   /// </summary>
   [ComponentAttribute(true)]
   [Export(typeof(IDialogService))]
   internal class DialogService : IDialogService
   {
      public const string DefaultTitle = "Microsoft Visual Studio";
      public const MessageBoxButton DefaultButton = MessageBoxButton.OK;
      public const MessageBoxImage DefaultIcon = MessageBoxImage.None;
      public const MessageBoxResult DefaultResult = MessageBoxResult.OK;

      private readonly IVsUIShell m_vsShell;

      //private IUIThread uiThread;

      /// <summary>
      /// Default constructor for runtime behavior that can't be mocked.
      /// </summary>
      [ImportingConstructor]
      public DialogService(IVsUIShell uiShell/*, IUIThread uiThread*/)
      {     
         this.m_vsShell = uiShell;
         //this.uiThread = uiThread;
      }

      public bool? ShowMessageBox(string message,
          string title = DefaultTitle,
          MessageBoxButton button = DefaultButton,
          MessageBoxImage icon = DefaultIcon,
          MessageBoxResult defaultResult = DefaultResult)
      {
         var classId = Guid.Empty;
         var result = 0;

         m_vsShell.ShowMessageBox(0, ref classId, title, message, string.Empty, 0,
             ToOleButton(button),
             ToOleDefault(defaultResult, button),
             ToOleIcon(icon),
             0, out result);

         if (result == OleMessageBoxResult.IDOK || result == OleMessageBoxResult.IDYES)
            return true;
         else if (result == OleMessageBoxResult.IDNO)
            return false;

         return null;
      }

      //public MessageBoxResult Prompt(string message,
      //    string title = DefaultTitle,
      //    MessageBoxButton button = DefaultButton,
      //    MessageBoxImage icon = MessageBoxImage.Question,
      //    MessageBoxResult defaultResult = DefaultResult)
      //{
      //   return uiShell.Prompt(message, title, button, icon, defaultResult);
      //}

      //public string InputBox(string message, string title = DefaultTitle)
      //{
      //   return uiThread.Invoke(() =>
      //   {
      //      var dialog = new InputBox();
      //      dialog.Message = message;
      //      dialog.Title = title;
      //      dialog.ShowInTaskbar = false;
      //      IntPtr parent;
      //      uiShell.GetDialogOwnerHwnd(out parent);

      //      if (Microsoft.Internal.VisualStudio.PlatformUI.WindowHelper.ShowModal(dialog, parent) != 0)
      //         return dialog.ResponseText;

      //      return null;
      //   });
      //}

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

      private static MessageBoxResult FromOle(int value)
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
   }
}
