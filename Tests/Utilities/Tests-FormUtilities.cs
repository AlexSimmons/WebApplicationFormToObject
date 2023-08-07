using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplicationFormToObject.Utilities;

namespace Tests.Utilities
{

    [TestClass]
    public class Tests_FormUtilities
    {
        private FormUtilities _sut; // System Under Test (SUT)

        [TestInitialize]
        public void TestSetup()
        {
            _sut = new FormUtilities();
        }


        #region FindControl Tests

        [TestMethod]
        public void FindControl_ReturnsExpectedControl_WhenControlExists()
        {
            // Arrange
            Control root = new Control { ID = "root" };
            Control child1 = new Control { ID = "child1" };
            Control child2 = new Control { ID = "child2_sub" }; // Control we want to find

            root.Controls.Add(child1);
            child1.Controls.Add(child2);

            // Act
            Control result = _sut.FindControl(root, "sub"); // We're trying to find a control with an ID ending in "sub"

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(child2, result); // Ensure we found the correct control
        }

        [TestMethod]
        public void FindControl_ReturnsNull_WhenControlDoesNotExist()
        {
            // Arrange
            Control root = new Control { ID = "root" };
            Control child1 = new Control { ID = "child1" };

            root.Controls.Add(child1);

            // Act
            Control result = _sut.FindControl(root, "sub");

            // Assert
            Assert.IsNull(result); // Ensure no control was found
        }

        [TestMethod]
        public void FindControl_ReturnsNull_WhenRootIsNull()
        {
            // Act
            Control result = _sut.FindControl(null, "someId");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindControl_ReturnsNull_WhenIdIsEmpty()
        {
            // Arrange
            Control root = new Control { ID = "root" };

            // Act
            Control result = _sut.FindControl(root, "");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindControl_ReturnsNull_WhenIdIsNull()
        {
            // Arrange
            Control root = new Control { ID = "root" };

            // Act
            Control result = _sut.FindControl(root, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindControl_ReturnsFirstMatchingControl_WhenMultipleControlsHaveSameSuffix()
        {
            // Arrange
            Control root = new Control { ID = "root" };
            Control child1 = new Control { ID = "child_sub" }; // this should be returned
            Control child2 = new Control { ID = "another_sub" }; // this should be ignored

            root.Controls.Add(child1);
            root.Controls.Add(child2);

            // Act
            Control result = _sut.FindControl(root, "sub");

            // Assert
            Assert.AreEqual(child1, result);
        }

        [TestMethod]
        public void FindControl_ReturnsCorrectControl_VerifyingBreadthFirstSearch()
        {
            // Arrange
            Control root = new Control { ID = "root" };
            Control child1 = new Control { ID = "child1" };
            Control child2 = new Control { ID = "child2_sub" }; // this should be returned if it's breadth-first
            Control grandChild = new Control { ID = "grandchild_sub" }; // this would be returned if it's depth-first

            root.Controls.Add(child1);
            child1.Controls.Add(grandChild);
            root.Controls.Add(child2);

            // Act
            Control result = _sut.FindControl(root, "sub");

            // Assert
            Assert.AreEqual(child2, result, $"Expected control with ID: {child2.ID}. Found control with ID: {result?.ID}");
        }

        #endregion


        #region SetControlValue Tests

        [TestMethod]
        public void SetControlValue_SetValueToTextBox_ControlUpdated()
        {
            // Arrange
            var control = new TextBox();

            // Act
            _sut.SetControlValue(control, "testValue", false, false);

            // Assert
            Assert.AreEqual("testValue", (control as TextBox).Text);
        }

        [TestMethod]
        public void SetControlValue_SetValueToTextBoxWithIgnoreStrings_ControlNotUpdated()
        {
            // Arrange
            var control = new TextBox();

            // Act
            _sut.SetControlValue(control, "testValue", false, true);

            // Assert
            Assert.AreEqual(string.Empty, (control as TextBox).Text);
        }

        [TestMethod]
        public void SetControlValue_SetValueToLiteral_ControlUpdated()
        {
            // Arrange
            var control = new Literal();

            // Act
            _sut.SetControlValue(control, "testValue", false, false);

            // Assert
            Assert.AreEqual("testValue", (control as Literal).Text);
        }

        [TestMethod]
        public void SetControlValue_SetValueToRadioButtonList_SelectedCorrectly()
        {
            // Arrange
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.Items.Add(new ListItem("Option1"));
            radioButtonList.Items.Add(new ListItem("Option2"));
            radioButtonList.Items.Add(new ListItem("Option3"));

            string expectedValue = "Option2";

            // Act
            _sut.SetControlValue(radioButtonList, expectedValue, false, false);

            // Assert
            Assert.IsTrue(radioButtonList.Items.FindByText(expectedValue).Selected);
        }

        [TestMethod]
        public void SetControlValue_SetValueToLinkButton_ControlUpdatedAndVisible()
        {
            // Arrange
            var control = new LinkButton();

            // Act
            _sut.SetControlValue(control, "testValue", false, false);

            // Assert
            Assert.AreEqual("testValue", (control as LinkButton).CommandArgument);
            Assert.IsTrue((control as LinkButton).Visible);
        }

        [TestMethod]
        public void SetControlValue_SetValueToLinkButtonWithLock_ControlNotVisible()
        {
            // Arrange
            var control = new LinkButton();

            // Act
            _sut.SetControlValue(control, "testValue", true, false);

            // Assert
            Assert.IsFalse((control as LinkButton).Visible);
        }

        [TestMethod]
        public void SetControlValue_SetValueToButton_ControlUpdated()
        {
            // Arrange
            var control = new Button();

            // Act
            _sut.SetControlValue(control, "testValue", false, false);

            // Assert
            Assert.AreEqual("testValue", (control as Button).Text);
        }

        [TestMethod]
        public void SetControlValue_SetValueToImage_ControlUpdated()
        {
            // Arrange
            var control = new Image();

            // Act
            _sut.SetControlValue(control, "imageUrl.jpg", false, false);

            // Assert
            Assert.AreEqual("imageUrl.jpg", (control as Image).ImageUrl);
        }

        [TestMethod]
        public void SetControlValue_SetValueToCheckBox_ControlUpdated()
        {
            // Arrange
            var control = new CheckBox();

            // Act
            _sut.SetControlValue(control, true, false, false);

            // Assert
            Assert.IsTrue((control as CheckBox).Checked);
        }

        [TestMethod]
        public void SetControlValue_SetValueToDropDownListBooleanValue_ControlUpdated()
        {
            // Arrange
            var control = new DropDownList();
            control.Items.Add(new ListItem("True", "1"));
            control.Items.Add(new ListItem("False", "0"));

            // Act
            _sut.SetControlValue(control, true, false, false);

            // Assert
            Assert.AreEqual("1", (control as DropDownList).SelectedValue);
        }

        [TestMethod]
        public void SetControlValue_SetValueToDropDownListStringValue_ControlUpdated()
        {
            // Arrange
            var control = new DropDownList();
            control.Items.Add(new ListItem("Option 1", "Option1"));
            control.Items.Add(new ListItem("Option 2", "Option2"));

            // Act
            _sut.SetControlValue(control, "Option1", false, false);

            // Assert
            Assert.AreEqual("Option1", (control as DropDownList).SelectedValue);
        }

        [TestMethod]
        public void SetControlValue_SetValueToRadioButtonList_ControlUpdated()
        {
            // Arrange
            var control = new RadioButtonList();
            control.Items.Add("Option1");
            control.Items.Add("Option2");

            // Act
            _sut.SetControlValue(control, "Option1", false, false);

            // Assert
            Assert.AreEqual("Option1", (control as RadioButtonList).SelectedValue);
        }

        [TestMethod]
        public void SetControlValue_SetValueToHiddenField_ControlUpdated()
        {
            // Arrange
            var control = new HiddenField();

            // Act
            _sut.SetControlValue(control, "hiddenValue", false, false);

            // Assert
            Assert.AreEqual("hiddenValue", (control as HiddenField).Value);
        }

        [TestMethod]
        public void SetControlValue_SetValueToCheckBox_Checked()
        {
            // Arrange
            var control = new CheckBox();
            control.Checked = true;

            // Act
            _sut.SetControlValue(control, "true", false, false);

            // Assert
            Assert.IsTrue(control.Checked);
        }

        [TestMethod]
        public void SetControlValue_SetValueToGeneralHtmlControl_ValueSet()
        {
            // Arrange
            var control = new HtmlGenericControl("span");

            // Act
            _sut.SetControlValue(control, "valueHere", false, false);

            // Assert
            Assert.AreEqual("valueHere", control.Attributes["value"]);
        }

        [TestMethod]
        public void SetControlValue_SetValueToHtmlControlWithLock_Disabled()
        {
            // Arrange
            var control = new HtmlInputText();

            // Act
            _sut.SetControlValue(control, "inputValue", true, false);

            // Assert
            Assert.IsFalse(control.Disabled);
        }

        [TestMethod]
        public void SetControlValue_SetValueToHtmlControlTextbox_ControlUpdated()
        {
            // Arrange
            var control = new HtmlInputText();

            // Act
            _sut.SetControlValue(control, "inputValue", false, false);

            // Assert
            Assert.AreEqual("inputValue", (control as HtmlInputText).Value);
        }


        [TestMethod]
        public void SetControlValue_LockWebControl_WebControlDisabled()
        {
            // Arrange
            var control = new TextBox();

            // Act
            _sut.SetControlValue(control, "testValue", true, false);

            // Assert
            Assert.IsFalse((control as WebControl).Enabled);
        }

        [TestMethod]
        public void SetControlValue_UnlockWebControl_WebControlEnabled()
        {
            // Arrange
            var control = new TextBox();

            // Act
            _sut.SetControlValue(control, "testValue", false, false);

            // Assert
            Assert.IsTrue((control as WebControl).Enabled);
        }

        #endregion


        #region SetRadioCheckboxAttribute

        [TestMethod]
        public void SetRadioCheckboxAttribute_SettingAttributeToChecked_WhenAttributeExists()
        {
            // Arrange
            HtmlControl control = new HtmlGenericControl("div");
            control.Attributes.Add("checked", "false");

            // Act
            _sut.SetRadioCheckboxAttribute(control);

            // Assert
            Assert.AreEqual("true", control.Attributes["checked"]);
        }

        [TestMethod]
        public void SetRadioCheckboxAttribute_AddingAttributeChecked_WhenAttributeDoesNotExist()
        {
            // Arrange
            HtmlControl control = new HtmlGenericControl("div");

            // Act
            _sut.SetRadioCheckboxAttribute(control);

            // Assert
            Assert.AreEqual("true", control.Attributes["checked"]);
        }

        #endregion

        #region GetValue

        [TestMethod]
        public void GetValue_ReturnsTextBoxText()
        {
            // Arrange
            TextBox textBox = new TextBox { Text = "TestText" };

            // Act
            var result = _sut.GetValue(textBox);

            // Assert
            Assert.AreEqual(textBox.Text, result);
        }

        [TestMethod]
        public void GetValue_ReturnsLabelText()
        {
            // Arrange
            Label label = new Label { Text = "TestLabel" };

            // Act
            var result = _sut.GetValue(label);

            // Assert
            Assert.AreEqual(label.Text, result);
        }

        [TestMethod]
        public void GetValue_ReturnsLiteralText()
        {
            // Arrange
            Literal literal = new Literal { Text = "TestLiteral" };

            // Act
            var result = _sut.GetValue(literal);

            // Assert
            Assert.AreEqual(literal.Text, result);
        }

        [TestMethod]
        public void GetValue_ReturnsLinkButtonText()
        {
            // Arrange
            LinkButton linkButton = new LinkButton { Text = "TestLinkButton" };

            // Act
            var result = _sut.GetValue(linkButton);

            // Assert
            Assert.AreEqual(linkButton.Text, result);
        }

        [TestMethod]
        public void GetValue_ReturnsButtonText()
        {
            // Arrange
            Button button = new Button { Text = "TestButton" };

            // Act
            var result = _sut.GetValue(button);

            // Assert
            Assert.AreEqual(button.Text, result);
        }

        [TestMethod]
        public void GetValue_ReturnsImageUrl()
        {
            // Arrange
            Image image = new Image { ImageUrl = "TestImageUrl" };

            // Act
            var result = _sut.GetValue(image);

            // Assert
            Assert.AreEqual(image.ImageUrl, result);
        }

        [TestMethod]
        public void GetValue_ReturnsCheckBoxValue()
        {
            // Arrange
            CheckBox checkBox = new CheckBox { Checked = true };

            // Act
            var result = _sut.GetValue(checkBox);

            // Assert
            Assert.AreEqual(checkBox.Checked, result);
        }

        [TestMethod]
        public void GetValue_ReturnsDropDownListValue()
        {
            // Arrange
            DropDownList dropDownList = new DropDownList();
            dropDownList.Items.Add(new ListItem("Test1", "1"));
            dropDownList.Items.Add(new ListItem("Test2", "2"));
            dropDownList.Items[0].Selected = true;

            // Act
            var result = _sut.GetValue(dropDownList);

            // Assert
            Assert.AreEqual(dropDownList.Items[0].Value, result);
        }

        [TestMethod]
        public void GetValue_ReturnsRadioButtonListValue()
        {
            // Arrange
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.Items.Add(new ListItem("Test1", "1"));
            radioButtonList.Items.Add(new ListItem("Test2", "2"));
            radioButtonList.Items[1].Selected = true;

            // Act
            var result = _sut.GetValue(radioButtonList);

            // Assert
            Assert.AreEqual(radioButtonList.Items[1].Text, result);
        }

        [TestMethod]
        public void GetValue_ReturnsHiddenFieldValue()
        {
            // Arrange
            HiddenField hiddenField = new HiddenField { Value = "TestHiddenField" };

            // Act
            var result = _sut.GetValue(hiddenField);

            // Assert
            Assert.AreEqual(hiddenField.Value, result);
        }

        [TestMethod]
        public void GetValue_ReturnsNullForUnsupportedControlType()
        {
            // Arrange
            Panel panel = new Panel(); // Unsupported control

            // Act
            var result = _sut.GetValue(panel);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

    }

}