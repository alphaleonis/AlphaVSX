using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Vsx
{
   internal class ComponentCatalog : TypeCatalog
   {
      #region Constructors

      public ComponentCatalog(Assembly assembly)
          : this(assembly.GetTypes())
      {
      }

      public ComponentCatalog(params Type[] types)
          : this((IEnumerable<Type>)types)
      {
      }

      public ComponentCatalog(IEnumerable<Type> types)
          : base(types.Select(t => t.IsDefined<ComponentAttribute>(true) && !t.IsAbstract ? new ComponentType(t) : t))
      {
      }

      #endregion

      #region Nested Types

      private class ComponentType : TypeDelegator
      {
         private readonly Type m_type;
         private readonly Lazy<IReadOnlyList<Attribute>> m_additionalAttributes;

         public ComponentType(Type type)
             : base(type)
         {
            m_type = type;
            m_additionalAttributes = new Lazy<IReadOnlyList<Attribute>>(GetAdditionalAttributes);
         }

         private IReadOnlyList<Attribute> GetAdditionalAttributes()
         {
            ComponentAttribute tka = m_type.GetCustomAttribute<ComponentAttribute>();
            List<Attribute> additionalAttributes = new List<Attribute>();
            if (tka != null)
            {
               if (!m_type.IsDefined<ExportAttribute>())
               {
                  additionalAttributes.Add(new ExportAttribute(tka.Name, m_type));
                  additionalAttributes.AddRange(m_type.GetInterfaces()
                     .Where(ifc => !ifc.Equals(typeof(IDisposable)))
                     .Select(ifc => new ExportAttribute(tka.Name, ifc)));
               }

               if (!m_type.IsDefined<PartCreationPolicyAttribute>())
               {
                  additionalAttributes.Add(new PartCreationPolicyAttribute(tka.IsSingleton ? CreationPolicy.Shared : CreationPolicy.NonShared));
               }
            }

            return additionalAttributes;
         }

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (m_additionalAttributes.Value.Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         public override object[] GetCustomAttributes(bool inherit)
         {
            return base.GetCustomAttributes(inherit).Concat(m_additionalAttributes.Value).ToArray();
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            object[] result = m_additionalAttributes.Value.Where(attr => attributeType.IsAssignableFrom(attr.GetType()))
                              .Concat(base.GetCustomAttributes(attributeType, inherit)).ToArray();

            // Apparently the MEF internals seems to expect the returned array to be of the type specified by 
            // attributeType rather than an object[]. So we create a typed array here, so MEF will be happy. :/
            if (attributeType != typeof(object))
            {
               var typedArray = Array.CreateInstance(attributeType, result.Length);
               Array.Copy(result, typedArray, result.Length);
               return (object[])typedArray;
            }

            return result;
         }

         public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
         {
            var constructors = base.GetConstructors(bindingAttr);

            if (bindingAttr.HasFlag(BindingFlags.Public) && !constructors.Any(ctor => ctor.IsDefined<ImportingConstructorAttribute>()))
            {
               // If no constructor has the ImportingConstructorAttribute, choose the public constructor with the most 
               // parameters and add this attribute to that one.
               var importingCtor = constructors.OrderByDescending(ctor => ctor.GetParameters().Length).FirstOrDefault();
               if (importingCtor != null)
                  constructors = constructors.Concat(new[] { new ImportingConstructorInfo(importingCtor) }).ToArray();
            }

            return constructors;
         }

         private class ImportingConstructorInfo : ConstructorInfoDelegator
         {
            public ImportingConstructorInfo(ConstructorInfo ctor)
                : base(ctor)
            {
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
               if (attributeType.IsAssignableFrom(typeof(ImportingConstructorAttribute)))
                  return true;

               return base.IsDefined(attributeType, inherit);
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
               IEnumerable<object> attributes = base.GetCustomAttributes(inherit);

               if (attributeType.IsAssignableFrom(typeof(ImportingConstructorAttribute)))
                  attributes = attributes.Concat(new[] { new ImportingConstructorAttribute() });

               return attributes.ToArray();
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
               return base.GetCustomAttributes(inherit)
                   .Concat(new[] { new ImportingConstructorAttribute() })
                   .ToArray();
            }
         }
      }

      #endregion
   }
}
