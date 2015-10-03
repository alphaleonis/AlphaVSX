using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Alphaleonis.Vsx.IDE
{
   internal class VsSolutionHierarchyNode
   {
      #region Private Fields

      private readonly Lazy<VsSolutionHierarchyNode> m_parent;
      private readonly Lazy<IServiceProvider> m_serviceProvider;

      #endregion

      #region Constructors

      internal VsSolutionHierarchyNode(IVsHierarchy hierarchy, uint itemId)
          : this(hierarchy, itemId, null)
      {
      }

      internal VsSolutionHierarchyNode(IVsHierarchy hierarchy, uint itemId, Lazy<VsSolutionHierarchyNode> parent)
      {
         if (hierarchy == null)
            throw new ArgumentNullException(nameof(hierarchy), $"{nameof(hierarchy)} is null.");

         VsHierarchy = hierarchy;
         ItemId = itemId;

         IntPtr nestedHierarchyObj;
         uint nestedItemId;
         Guid hierGuid = typeof(IVsHierarchy).GUID;

         // Check first if this node has a nested hierarchy. If so, then there really are two 
         // identities for this node: 1. hierarchy/itemid 2. nestedHierarchy/nestedItemId.
         // We will recurse and call EnumHierarchyItems which will display this node using
         // the inner nestedHierarchy/nestedItemId identity.
         int hr = hierarchy.GetNestedHierarchy(itemId, ref hierGuid, out nestedHierarchyObj, out nestedItemId);
         if (hr == VSConstants.S_OK && nestedHierarchyObj != IntPtr.Zero)
         {
            IVsHierarchy nestedHierarchy = Marshal.GetObjectForIUnknown(nestedHierarchyObj) as IVsHierarchy;
            Marshal.Release(nestedHierarchyObj); // we are responsible to release the refcount on the out IntPtr parameter
            if (nestedHierarchy != null)
            {
               VsHierarchy = nestedHierarchy;
               ItemId = nestedItemId;
            }
         }
                  
         DisplayName = VsHierarchy.GetProperty<string>(__VSHPROPID.VSHPROPID_Name, ItemId);

         m_parent = parent ?? new Lazy<VsSolutionHierarchyNode>(() =>
         {
            if (VsHierarchy is IVsSolution)
               return null;

            var rootHierarchy = hierarchy.GetProperty<IVsHierarchy>(__VSHPROPID.VSHPROPID_ParentHierarchy, VSConstants.VSITEMID_ROOT);
            if (rootHierarchy == null)
               return null;

            var rootNode = new VsSolutionHierarchyNode(rootHierarchy, VSConstants.VSITEMID_ROOT);

            var parentNode = new VsSolutionHierarchyNode[] { rootNode }
                .Concat(rootNode.Children.BreadthFirstTraversal(node => node.Children))
                .FirstOrDefault(node => node.Children.Any(child => child.ItemId == ItemId));

            if (parentNode == null)
               return null;

            return new VsSolutionHierarchyNode(parentNode.VsHierarchy, parentNode.ItemId);
         });

         this.m_serviceProvider = new Lazy<IServiceProvider>(() =>
         {
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSp;
            hierarchy.GetSite(out oleSp);
            return oleSp != null ?
                new ServiceProvider(oleSp) :
                GlobalVsServiceProvider.Instance;
         });
      }

      #endregion

      #region Properties

      public string DisplayName { get; }

      public VsSolutionHierarchyNode Parent
      {
         get { return m_parent.Value; }
      }

      public IEnumerable<VsSolutionHierarchyNode> Children
      {
         get
         {
            int hr;
            object pVar;

            hr = VsHierarchy.GetProperty(ItemId, (int)(VsHierarchy is IVsSolution ? __VSHPROPID.VSHPROPID_FirstVisibleChild : __VSHPROPID.VSHPROPID_FirstChild), out pVar);
            ErrorHandler.ThrowOnFailure(hr);

            if (VSConstants.S_OK == hr)
            {
               uint childId = GetItemId(pVar);
               while (childId != VSConstants.VSITEMID_NIL)
               {
                  yield return new VsSolutionHierarchyNode(VsHierarchy, childId, new Lazy<VsSolutionHierarchyNode>(() => this));

                  hr = VsHierarchy.GetProperty(childId, (int)(VsHierarchy is IVsSolution ? __VSHPROPID.VSHPROPID_NextVisibleSibling : __VSHPROPID.VSHPROPID_NextSibling), out pVar);

                  if (VSConstants.S_OK == hr)
                  {
                     childId = GetItemId(pVar);
                  }
                  else
                  {
                     ErrorHandler.ThrowOnFailure(hr);
                     break;
                  }
               }
            }
         }
      }

      public IServiceProvider ServiceProvider
      {
         get
         {
            return this.m_serviceProvider.Value;
         }
      }

      public uint ItemId { get; }

      public IVsHierarchy VsHierarchy { get; }

      public object ExtenderObject
      {
         get
         {            
            return VsHierarchy.GetProperty<object>(__VSHPROPID.VSHPROPID_ExtObject, this.ItemId);
         }
      }

      #endregion

      #region Methods

      public override string ToString()
      {
         return DisplayName;
      }

      private static uint GetItemId(object pvar)
      {
         if (pvar == null) return VSConstants.VSITEMID_NIL;
         if (pvar is int) return (uint)(int)pvar;
         if (pvar is uint) return (uint)pvar;
         if (pvar is short) return (uint)(short)pvar;
         if (pvar is ushort) return (uint)(ushort)pvar;
         if (pvar is long) return (uint)(long)pvar;
         return VSConstants.VSITEMID_NIL;
      }

      #endregion

      #region Nested Types

      private class GlobalVsServiceProvider : IServiceProvider
      {
         public static readonly IServiceProvider Instance = new GlobalVsServiceProvider();

         public object GetService(Type serviceType)
         {
            return Package.GetGlobalService(serviceType);
         }
      }

      #endregion
   }
}

