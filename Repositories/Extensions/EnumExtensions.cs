using System.Reflection;
using System.Runtime.Serialization;

public static class EnumExtensions
{
  public static string GetEnumMemberValue<T>(this T enumValue) where T : Enum
  {
    FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
    object[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false);
    if (attributes.Length > 0)
    {
      EnumMemberAttribute attribute = (EnumMemberAttribute)attributes[0];
      return attribute.Value;
    }
    return null; // Or throw an exception if no attribute is found
  }
}