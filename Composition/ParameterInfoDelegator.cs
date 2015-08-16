using System;
using System.Collections.Generic;
using System.Reflection;

namespace Alphaleonis.Vsx
{
   internal class ParameterInfoDelegator : ParameterInfo
   {
      private readonly ParameterInfo m_parameter;

      public ParameterInfoDelegator(ParameterInfo parameter)
      {
         m_parameter = parameter;
      }

      public override object[] GetCustomAttributes(bool inherit)
      {
         return m_parameter.GetCustomAttributes(inherit);
      }

      public override object[] GetCustomAttributes(Type attributeType, bool inherit)
      {
         return m_parameter.GetCustomAttributes(attributeType, inherit);
      }

      public override IList<CustomAttributeData> GetCustomAttributesData()
      {
         return m_parameter.GetCustomAttributesData();
      }

      public override Type[] GetOptionalCustomModifiers()
      {
         return m_parameter.GetOptionalCustomModifiers();
      }

      public override Type[] GetRequiredCustomModifiers()
      {
         return m_parameter.GetRequiredCustomModifiers();
      }

      public override bool IsDefined(Type attributeType, bool inherit)
      {
         return m_parameter.IsDefined(attributeType, inherit);
      }

      public override string ToString()
      {
         return m_parameter.ToString();
      }

      public override ParameterAttributes Attributes
      {
         get { return m_parameter.Attributes; }
      }

      public override object DefaultValue
      {
         get { return m_parameter.DefaultValue; }
      }

      public override MemberInfo Member
      {
         get { return m_parameter.Member; }
      }

      public override int MetadataToken
      {
         get { return m_parameter.MetadataToken; }
      }

      public override string Name
      {
         get { return m_parameter.Name; }
      }

      public override Type ParameterType
      {
         get { return m_parameter.ParameterType; }
      }

      public override int Position
      {
         get { return m_parameter.Position; }
      }

      public override object RawDefaultValue
      {
         get { return m_parameter.RawDefaultValue; }
      }

      public ParameterInfo UnderlyingParameter
      {
         get { return m_parameter; }
      }
   }
}
