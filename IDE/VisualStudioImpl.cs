using Microsoft.Practices.ServiceLocation;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System;

namespace Alphaleonis.Vsx
{
   [Component(true)]
   internal class VisualStudioImpl : IVisualStudio
   {      
      public VisualStudioImpl(IServiceLocator serviceLocator, IDialogService dialogService)
      {
         ServiceLocator = serviceLocator;
         DialogService = dialogService;
      }

      public IDialogService DialogService { get; }

      public IServiceLocator ServiceLocator { get; }
   }
}
