using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Vsx.IDE
{
   public interface ICommandManager
   { 
      void AddCommand(ICommandImplementation commandImplementation, CommandID commandId);
      void AddAllCommandsFrom(Assembly assembly);
      void AddCommands<T>() where T : ICommandImplementation;
      void AddCommands(ICommandImplementation commandImplementation);

      void GlobalInvoke(CommandID commandId);
   }

   internal class CommandManager : ICommandManager
   {
      public CommandManager(IServiceLocator serviceProvider)
      {
         if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider), $"{nameof(serviceProvider)} is null.");

         ServiceProvider = serviceProvider;
      }

      public IServiceLocator ServiceProvider { get; }

      public void AddAllCommandsFrom(Assembly assembly)
      {
         AddCommands(assembly.DefinedTypes);
      }

      private void AddCommands(IEnumerable<Type> types)
      {
         types = types.Where(type => type.IsClass && type.Implements<ICommandImplementation>());

         foreach (var type in types)
            AddCommands(type);
      }

      public void AddCommands<T>() where T : ICommandImplementation
      {
         AddCommands(typeof(T));
      }

      private void AddCommands(Type commandType)
      {
         if (commandType == null)
            throw new ArgumentNullException(nameof(commandType), $"{nameof(commandType)} is null.");

         var attributes = commandType.GetCustomAttributes<CommandAttribute>(false);
         if (!attributes.Any())
            throw new ArgumentException($"The type {commandType.FullName} must be decorated with {nameof(CommandAttribute)} to be registered as a command.");

         IUnityContainer container = ServiceProvider.GetInstance<IUnityContainer>();
         ICommandImplementation commandExtension = container.Resolve(commandType) as ICommandImplementation;
         if (commandExtension == null)
            throw new ArgumentException($"The type {commandType.FullName} does not implement {nameof(ICommandImplementation)}.");

         foreach (var attr in attributes)
         {
            AddCommand(commandExtension, attr);
         }
      }

      public void AddCommand(ICommandImplementation commandImplementation, CommandAttribute commandAttribute)
      {
         if (commandImplementation == null)
            throw new ArgumentNullException(nameof(commandImplementation), $"{nameof(commandImplementation)} is null.");

         if (commandAttribute == null)
            throw new ArgumentNullException(nameof(commandAttribute), $"{nameof(commandAttribute)} is null.");

         OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
         if (commandService != null)
         {
            Guid groupId;
            if (!Guid.TryParse(commandAttribute.GroupId, out groupId))
               throw new ArgumentException($"Invalid GUID (\"{commandAttribute.GroupId}\" specified on {nameof(CommandAttribute)}.");

            CommandID commandId = new CommandID(Guid.Parse(commandAttribute.GroupId), commandAttribute.CommandId);

            AddCommand(commandImplementation, commandId); 
         }
      }

      public void AddCommand(ICommandImplementation commandImplementation, CommandID commandId)
      {
         OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
         if (commandService.FindCommand(commandId) == null)
         {
            CommandImplementationAdapter commandExtension = commandImplementation is IDynamicCommandImplementation ? 
               new DynamicCommandImplementationAdapter(commandId, commandImplementation as IDynamicCommandImplementation) : 
               new CommandImplementationAdapter(commandId, commandImplementation);
            commandService.AddCommand(commandExtension);
         }
      }

      public void AddCommands(ICommandImplementation commandImplementation)
      {
         if (commandImplementation == null)
            throw new ArgumentNullException(nameof(commandImplementation), $"{nameof(commandImplementation)} is null.");

         foreach (CommandAttribute attribute in commandImplementation.GetType().GetCustomAttributes<CommandAttribute>())
         {
            AddCommand(commandImplementation, attribute);
         }
      }

      public void GlobalInvoke(CommandID commandId)
      {
         OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
         if (commandService != null)
            commandService.GlobalInvoke(commandId);
      }
   }




}

