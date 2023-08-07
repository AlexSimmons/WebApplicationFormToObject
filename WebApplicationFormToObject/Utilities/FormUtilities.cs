using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplicationFormToObject.Utilities
{
    public class FormUtilities
    {
        /// <summary>
        /// breadth-first search of controls.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Control FindControl(Control root, string id)
        {
            if (root == null || string.IsNullOrEmpty(id))
            {
                return null;
            }

            Queue<Control> controlQueue = new Queue<Control>();
            controlQueue.Enqueue(root);

            while (controlQueue.Count > 0)
            {
                var currentControl = controlQueue.Dequeue();

                if (currentControl.ID?.EndsWith(id) == true)
                {
                    return currentControl;
                }

                foreach (Control child in currentControl.Controls)
                {
                    controlQueue.Enqueue(child);
                }
            }

            return null;
        }


        /// <summary>
        /// Sets the value for a given ASP.NET control based on its type. 
        /// Also, controls the enabled state of WebControls based on the 'lockFields' parameter.
        /// </summary>
        /// <param name="control">The ASP.NET control whose value is to be set.</param>
        /// <param name="value">The value to be set to the control.</param>
        /// <param name="lockFields">If true, WebControls' 'Enabled' property will be set to false; otherwise, true.</param>
        /// <param name="ignoreStrings">If true, TextBox controls will not be set with the provided value.</param>
        /// <remarks>
        /// The method handles various control types like TextBox, Label, Literal, LinkButton, Button, Image, CheckBox,
        /// DropDownList, RadioButtonList, HiddenField, and HtmlControl. For certain HtmlControls like radio buttons and checkboxes,
        /// their attributes are also set.
        /// </remarks>
        public void SetControlValue(Control control, object value, bool lockFields, bool ignoreStrings)
        {
            if (value == null) return;

            var stringValue = value.ToString();

            // Handle properties common to all WebControls
            if (control is WebControl webControl)
            {
                webControl.Enabled = !lockFields;
            }

            // TextBox control
            if (control is TextBox textBox && !ignoreStrings)
            {
                textBox.Text = stringValue;
                return;
            }

            // Label control
            if (control is Label label)
            {
                label.Text = stringValue;
                return;
            }

            // Literal control
            if (control is Literal literal)
            {
                literal.Text = stringValue;
                return;
            }

            // LinkButton control
            if (control is LinkButton linkButton)
            {
                linkButton.CommandArgument = stringValue;
                linkButton.Visible = !lockFields;
                return;
            }

            // Button control
            if (control is Button button)
            {
                button.Text = stringValue;
                return;
            }

            // Image control
            if (control is Image image)
            {
                image.ImageUrl = stringValue;
                return;
            }

            // CheckBox control
            if (control is CheckBox checkBox)
            {
                checkBox.Checked = Convert.ToBoolean(value);
                return;
            }

            // DropDownList control
            if (control is DropDownList dropDownList)
            {
                if (value is bool booleanValue)
                {
                    dropDownList.SelectedValue = booleanValue ? "1" : "0";
                }
                else
                {
                    dropDownList.SelectedValue = stringValue;
                }
                return;
            }

            // RadioButtonList control
            if (control is RadioButtonList radioButtonList)
            {
                foreach (ListItem item in radioButtonList.Items)
                {
                    if (item.Value == stringValue)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                return;
            }

            // HiddenField control
            if (control is HiddenField hiddenField)
            {
                hiddenField.Value = stringValue;
                return;
            }

            // HtmlControl (with radio and checkbox specifics)
            if (control is HtmlControl htmlControl)
            {
                htmlControl.Attributes["value"] = stringValue;

                var controlType = htmlControl.Attributes["type"];
                if (controlType == "radio" || controlType == "checkbox")
                {
                    SetRadioCheckboxAttribute(control);
                }
                return;
            }
        }


        /// <summary>
        /// Sets the "checked" attribute of the given HTML control to "true".
        /// If the attribute already exists, it updates its value; otherwise, it adds the attribute.
        /// </summary>
        /// <param name="control">The control whose "checked" attribute needs to be set or added.</param>
        public void SetRadioCheckboxAttribute(Control control)
        {
            var htmlControl = control as HtmlControl;
            if (htmlControl == null) return;

            if (htmlControl.Attributes["checked"] != null)
            {
                htmlControl.Attributes["checked"] = "true";
            }
            else
            {
                htmlControl.Attributes.Add("checked", "true");
            }
        }

        /// <summary>
        /// Retrieves the value from the provided control.
        /// For input controls like TextBox, the text content is returned.
        /// For CheckBox controls, the checked state (boolean) is returned.
        /// For DropDownList controls, the selected value from the HTTP request context is returned.
        /// If the control type doesn't match any of the predefined types, null is returned.
        /// Note: This method makes a direct reference to HttpContext.Current.Request for DropDownList, 
        /// which may introduce tight coupling to the web context. Consider refactoring if aiming for better decoupling and testability.
        /// </summary>
        /// <param name="control">The web control from which the value needs to be fetched.</param>
        /// <returns>The value contained in the control or null if the control type is unsupported.</returns>

        public object GetValue(Control control)
        {
            switch (control)
            {
                case TextBox textBox:
                    return textBox.Text;

                case Label label:
                    return label.Text;

                case Literal literal:
                    return literal.Text;

                case LinkButton linkButton:
                    return linkButton.Text;

                case Button button:
                    return button.Text;

                case Image image:
                    return image.ImageUrl;

                case CheckBox checkBox:
                    return checkBox.Checked;

                case DropDownList dropDownList:
                    return dropDownList.SelectedValue;

                case RadioButtonList radioButtonList:

                    var selectedText = "";
                    foreach (ListItem item in radioButtonList.Items)
                    {
                        if (item.Selected)
                        {
                            selectedText = item.Text;
                            break;
                        }
                    }

                    return selectedText;

                case HiddenField hiddenField:
                    return hiddenField.Value;

                default:
                    return null;
            }
        }


    }
}