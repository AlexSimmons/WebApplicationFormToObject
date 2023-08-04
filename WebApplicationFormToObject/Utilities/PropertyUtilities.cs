using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApplicationFormToObject.Utilities
{
    public class PropertyUtilities
    {
        /// <summary>
        /// Retrieves an array of all properties from the given object.
        /// </summary>
        /// <param name="myObject">The object whose properties we want to retrieve.</param>
        /// <returns>An array of PropertyInfo objects reflecting all the properties of the object.</returns>
        public PropertyInfo[] GetProperties(object myObject)
        {
            return myObject.GetType().GetProperties();
        }

    }
}