using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Collections.Specialized;
using System.Collections.Generic;
using WebApplicationFormToObject.Utilities;

namespace WebApplicationFormToObject
{
    public class Mapper
    {
        public readonly string controlNameEndFormatting = "_{0}";

        private FormUtilities _formUtilities;
        private ConversionUtilities _conversionUtilities;
        private PropertyUtilities _propertyUtilities;

        public Mapper(FormUtilities formUtilities, ConversionUtilities conversionUtilities, PropertyUtilities propertyUtilities)
        {
            _formUtilities = formUtilities ?? new FormUtilities();
            _conversionUtilities = conversionUtilities ?? new ConversionUtilities();
            _propertyUtilities = propertyUtilities ?? new PropertyUtilities();
        }

        public Mapper()
        {
            _formUtilities = new FormUtilities();
            _conversionUtilities = new ConversionUtilities();   
            _propertyUtilities = new PropertyUtilities();
        }


        /// <summary>
        /// Load the properties of a given object into the form.
        /// </summary>
        /// <param name="form">The HtmlForm where the object's properties will be loaded.</param>
        /// <param name="objectToLoad">The object that contains the properties to load.</param>
        /// <param name="lockFields">If set to true, the fields in the form will be locked/disabled.</param>
        /// <param name="ignoreStrings">If set to true, the method will ignore TextBox controls that hold strings. This can be useful when you want to load properties but ignore the text contents of TextBoxes.</param>
        public void LoadObjectToForm(HtmlForm form, object objectToLoad, bool lockFields = false, bool ignoreStrings = false)
        {
            if (form == null || objectToLoad == null) return;

            // 1) Get a list of the properties of the object.
            PropertyInfo[] properties = _propertyUtilities.GetProperties(objectToLoad);

            // 2) Loop through each property looking for a control on the form with the same name
            foreach (PropertyInfo property in properties)
            {
                Control control = _formUtilities.FindControl(form, string.Format(controlNameEndFormatting, property.Name));
                if (control != null)
                {
                    _formUtilities.SetControlValue(control, _conversionUtilities.GetValueToLoad(property, objectToLoad), lockFields, ignoreStrings);
                }
            }
        }


        /// <summary>
        /// Attempts to save the properties of the provided object from the corresponding form controls.
        /// </summary>
        /// <param name="form">The HTML form containing the controls corresponding to the object properties.</param>
        /// <param name="objectToSave">The object whose properties will be set based on the form controls.</param>
        /// <param name="allowNullableParameter">If true, allows setting nullable properties; otherwise, non-nullable properties will only be set if the value is not null.</param>
        /// <returns>Returns true if the save operation succeeds without errors; otherwise, returns false.</returns>
        public bool SaveFormToObject(HtmlForm form, object objectToSave, bool allowNullableParameter = false)
        {
            if (form == null || objectToSave == null) return true;

            var errors = new NameValueCollection();

            // 1) Get a list of the properties of the object.
            var properties = _propertyUtilities.GetProperties(objectToSave);

            // 2) Loop through each property looking for a control on the form with the same name
            foreach (PropertyInfo property in properties)
            {
                if (property.GetSetMethod() != null)
                {
                    var control = _formUtilities.FindControl(form, string.Format(controlNameEndFormatting, property.Name));
                    if (control != null)
                    {
                        try
                        {
                            var valueOfControl = _formUtilities.GetValue(control);
                            if (valueOfControl != null)
                            {
                                var valueToSave = _conversionUtilities.ConvertValueToPropertyType(valueOfControl, property.PropertyType, allowNullableParameter);

                                if (allowNullableParameter || valueToSave != null)
                                {
                                    property.SetValue(objectToSave, valueToSave, null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add(control.ID, ex.Message);
                        }
                    }
                }
            }

            return (errors.Count == 0);
        }

    }
}
