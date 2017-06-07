using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EPK.Web.Infrastructure.Utilities
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return true;
            return false;
        }
        public static bool IsNotNull(this object obj)
        {
            return !IsNull(obj);
        }
        public static DateTime f_CDate(this object value)
        {
            DateTime lOut;
            value = value != null ? value : "";
            DateTime.TryParse(value.ToString(), out lOut);
            return lOut;
        }
        public static int f_CInt(this object aValue)
        {
            int lOut;
            aValue = aValue ?? 0;
            int.TryParse(aValue.ToString(), out lOut);
            return lOut;
        }
        public static int GetNumberInString(this string stringvalue)
        {
            try
            {
                var number = Regex.Match(stringvalue, @"\d+").Value;
                return Int32.Parse(number);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static TDestination TransformTo<TDestination>(this object sourceObject, string excludeProperties = "") where TDestination : class
        {
            if (!sourceObject.IsNull())
            {
                if (!excludeProperties.StringIsNullEmptyWhiteSpace())
                {
                    excludeProperties = excludeProperties.Trim().ToLower();
                    excludeProperties = "[" + excludeProperties + "]";
                }
                var destinationObject = Activator.CreateInstance(typeof(TDestination));
                typeof(TDestination).GetProperties().AsParallel().ForAll(destProperty =>
                {
                    PropertyInfo sourceProperty = sourceObject.GetType().GetProperty(destProperty.Name);
                    try
                    {
                        if (sourceProperty != null && !excludeProperties.Contains("[" + sourceProperty.Name.ToLower() + "]") && destProperty.CanWrite)
                        {
                            object sourceValue = sourceProperty.GetValue(sourceObject);
                            object desValue = destProperty.GetValue(destinationObject);
                            if (!Equals(sourceValue, desValue))
                                destProperty.SetValue(destinationObject, sourceValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Transform <TDestination>", ex);
                    }
                });
                return (destinationObject as TDestination);
            }
            return null;
        }
    }
}
