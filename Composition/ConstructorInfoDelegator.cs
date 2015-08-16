using System;
using System.Globalization;
using System.Reflection;

namespace Alphaleonis.Vsx
{
   internal class ConstructorInfoDelegator : ConstructorInfo
   {
      private readonly ConstructorInfo m_constructor;

      public ConstructorInfoDelegator(ConstructorInfo constructor)
      {
         m_constructor = constructor;
      }

      public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
      {
         return m_constructor.Invoke(invokeAttr, binder, parameters, culture);
      }

      public override MethodAttributes Attributes
      {
         get { return m_constructor.Attributes; }
      }

      public override MethodImplAttributes GetMethodImplementationFlags()
      {
         return m_constructor.GetMethodImplementationFlags();
      }

      public override ParameterInfo[] GetParameters()
      {
         return m_constructor.GetParameters();
      }

      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
      {
         return m_constructor.Invoke(obj, invokeAttr, binder, parameters, culture);
      }

      public override RuntimeMethodHandle MethodHandle
      {
         get { return m_constructor.MethodHandle; }
      }

      public override Type DeclaringType
      {
         get { return m_constructor.DeclaringType; }
      }

      public override object[] GetCustomAttributes(Type attributeType, bool inherit)
      {
         return m_constructor.GetCustomAttributes(attributeType, inherit);
      }

      public override object[] GetCustomAttributes(bool inherit)
      {
         return m_constructor.GetCustomAttributes(inherit);
      }

      public override bool IsDefined(Type attributeType, bool inherit)
      {
         return m_constructor.IsDefined(attributeType, inherit);
      }

      public override string Name
      {
         get { return m_constructor.Name; }
      }

      public override Type ReflectedType
      {
         get { return m_constructor.ReflectedType; }
      }
   }
}
