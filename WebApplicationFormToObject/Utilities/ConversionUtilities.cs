using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApplicationFormToObject.Utilities
{
    public class ConversionUtilities
    {

        /// <summary>
        /// Retrieves the value of the specified property from the given object.
        /// - For enum types, the integer representation is returned.
        /// - For DateTime types, a short date string representation is returned.
        /// </summary>
        /// <param name="property">The property whose value needs to be fetched.</param>
        /// <param name="myObject">The object from which the property value will be retrieved.</param>
        /// <returns>The processed value of the property.</returns>
        public object GetValueToLoad(PropertyInfo property, object myObject)
        {
            object valueToLoad = property.GetValue(myObject, null);

            if (property.PropertyType.IsEnum)
            {
                return (int)Enum.Parse(property.PropertyType, valueToLoad.ToString());
            }

            if (property.PropertyType == typeof(DateTime) ||
                (property.PropertyType == typeof(Nullable<DateTime>) && valueToLoad != null))
            {
                return DateTime.Parse(valueToLoad.ToString()).ToShortDateString();
            }

            return valueToLoad;
        }


        /// <summary>
        /// Converts the value of a control to the corresponding property type.
        /// </summary>
        /// <param name="valueOfControl">Value of the control to convert.</param>
        /// <param name="type">Target type to convert to.</param>
        /// <param name="allowNullables">Indicates if nullable types should be processed.</param>
        /// <returns>Converted value.</returns>
        public object ConvertValueToPropertyType(object valueOfControl, Type type, bool allowNullables)
        {
            string valueString = valueOfControl?.ToString();

            if (!string.IsNullOrEmpty(valueString))
            {
                // If the target type is an enumeration
                if (type.IsEnum)
                {
                    return Enum.Parse(type, valueString);
                }

                // Handle nullable types
                if (type.Name == "Nullable`1")
                {
                    type = Nullable.GetUnderlyingType(type);

                    if (!allowNullables && type == typeof(bool))
                    {
                        switch (valueString)
                        {
                            case "1":
                                return true;
                            case "0":
                                return false;
                            case "-1":
                                return null;
                        }
                    }
                    else if (type == typeof(bool))
                    {
                        switch (valueString)
                        {
                            case "1":
                                return true;
                            case "0":
                                return false;
                            default:
                                return null;
                        }
                    }
                }

                // Convert the value to the specified type
                return Convert.ChangeType(valueOfControl, type);
            }

            // Handle cases where the value string is empty or null
            return (type.Name == "Nullable`1") ? null : string.Empty;
        }


    }
}