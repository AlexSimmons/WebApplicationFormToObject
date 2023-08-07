using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplicationFormToObject;
using WebApplicationFormToObject.Utilities;

namespace Tests
{
    [TestClass]

    public class Tests_Mapper
    {

        private Mapper _sut; // System Under Test (SUT)
        private HtmlForm _form;
        private TestObject _testObject;

        public class TestObject
        {
            public string TextBoxText { get; set; }
            public string LabelProperty { get; set; }
            public string LiteralProperty { get; set; }
            public string LinkButtonProperty { get; set; }
            public string ButtonProperty { get; set; }
            public string ImageProperty { get; set; }

            public string DropDownListProperty { get; set; }
            public bool CheckBoxProperty { get; set; }

            public string RadioButtonListProperty { get; set; }

            public string HiddenFieldProperty { get; set; }
        }


        [TestInitialize]
        public void TestInitialize()
        {
            _sut = new Mapper();
            _form = new HtmlForm();
            _testObject = new TestObject();
        }

        #region LoadObjectToForm

        [TestMethod]
        public void LoadObjectToForm_LoadsTextBoxValueFromObjectProperty()
        {
            // Arrange
            var textBox = new TextBox { ID = "txb_TextBoxText" };
            _form.Controls.Add(textBox);
            _testObject.TextBoxText = "Test Value";

            // Act
            _sut.LoadObjectToForm(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", textBox.Text);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsLabelValueFromObjectProperty()
        {
            // Arrange
            var label = new System.Web.UI.WebControls.Label { ID = "lbl_LabelProperty" };
            _form.Controls.Add(label);
            _testObject.LabelProperty = "Test Value";

            // Act
            _sut.LoadObjectToForm(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", label.Text);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsLiteralValueFromObjectProperty()
        {
            var literal = new Literal { ID = "lit_LiteralProperty" };
            _form.Controls.Add(literal);
            _testObject.LiteralProperty = "Test Value";
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual("Test Value", literal.Text);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsLinkButtonValueFromObjectProperty()
        {
            var linkButton = new LinkButton { ID = "lnk_LinkButtonProperty" };
            _form.Controls.Add(linkButton);
            _testObject.LinkButtonProperty = "Test Value";
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual("Test Value", linkButton.CommandArgument);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsButtonValueFromObjectProperty()
        {
            var button = new Button { ID = "btn_ButtonProperty" };
            _form.Controls.Add(button);
            _testObject.ButtonProperty = "Test Value";
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual("Test Value", button.Text);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsImageValueFromObjectProperty()
        {
            var image = new Image { ID = "img_ImageProperty" };
            _form.Controls.Add(image);
            _testObject.ImageProperty = "TestImageUrl";
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual("TestImageUrl", image.ImageUrl);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsCheckBoxValueFromObjectProperty()
        {
            var checkBox = new CheckBox { ID = "cbx_CheckBoxProperty" };
            _form.Controls.Add(checkBox);
            _testObject.CheckBoxProperty = true;
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual(true, checkBox.Checked);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsDropDownListValueFromObjectProperty()
        {
            var dropDownList = new DropDownList { ID = "ddl_DropDownListProperty" };
            dropDownList.Items.Add(new ListItem("Test Value", "Test Value"));
            _form.Controls.Add(dropDownList);
            _testObject.DropDownListProperty = "Test Value";
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual("Test Value", dropDownList.SelectedValue);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsRadioButtonListValueFromObjectProperty()
        {
            var radioButtonList = new RadioButtonList { ID = "rbl_RadioButtonListProperty" };
            radioButtonList.Items.Add(new ListItem("Test Value", "Test Value"));
            radioButtonList.Items[0].Selected = true;

            _form.Controls.Add(radioButtonList);
            _testObject.RadioButtonListProperty = "Test Value";
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual("Test Value", radioButtonList.SelectedValue);
        }

        [TestMethod]
        public void LoadObjectToForm_LoadsHiddenFieldValueFromObjectProperty()
        {
            var hiddenField = new HiddenField { ID = "hf_HiddenFieldProperty" };
            _form.Controls.Add(hiddenField);
            _testObject.HiddenFieldProperty = "Test Value";
            _sut.LoadObjectToForm(_form, _testObject);
            Assert.AreEqual("Test Value", hiddenField.Value);
        }

        #endregion

        #region SaveFormToObject

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromTextBoxValue()
        {
            // Arrange
            var textBox = new TextBox { ID = "txb_TextBoxText", Text = "Test Value" };
            _form.Controls.Add(textBox);
            _testObject.TextBoxText = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.TextBoxText);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromLabelValue()
        {
            // Arrange
            var label = new System.Web.UI.WebControls.Label { ID = "lbl_LabelProperty", Text = "Test Value" };
            _form.Controls.Add(label);
            _testObject.LabelProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.LabelProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromLiteralValue()
        {
            // Arrange
            var literal = new Literal { ID = "lit_LiteralProperty", Text = "Test Value" };
            _form.Controls.Add(literal);
            _testObject.LiteralProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.LiteralProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromLinkButtonValue()
        {
            // Arrange
            var linkButton = new LinkButton { ID = "lnk_LinkButtonProperty", Text = "Test Value" };
            _form.Controls.Add(linkButton);
            _testObject.LinkButtonProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.LinkButtonProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromButtonValue()
        {
            // Arrange
            var button = new Button { ID = "btn_ButtonProperty", Text = "Test Value" };
            _form.Controls.Add(button);
            _testObject.ButtonProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.ButtonProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromImageValue()
        {
            // Arrange
            var image = new Image { ID = "img_ImageProperty", ImageUrl = "TestImageUrl" };
            _form.Controls.Add(image);
            _testObject.ImageProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("TestImageUrl", _testObject.ImageProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromCheckBoxValue()
        {
            // Arrange
            var checkBox = new CheckBox { ID = "cbx_CheckBoxProperty", Checked = true };
            _form.Controls.Add(checkBox);
            _testObject.CheckBoxProperty = false;

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual(true, _testObject.CheckBoxProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromDropDownListValue()
        {
            // Arrange
            var dropDownList = new DropDownList { ID = "ddl_DropDownListProperty" };
            dropDownList.Items.Add(new ListItem("Test Value", "Test Value"));
            dropDownList.SelectedValue = "Test Value";
            _form.Controls.Add(dropDownList);
            _testObject.DropDownListProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.DropDownListProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromRadioButtonListValue()
        {
            // Arrange
            var radioButtonList = new RadioButtonList { ID = "rbl_RadioButtonListProperty" };
            radioButtonList.Items.Add(new ListItem("Test Value", "Test Value"));
            radioButtonList.SelectedValue = "Test Value";
            _form.Controls.Add(radioButtonList);
            _testObject.RadioButtonListProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.RadioButtonListProperty);
        }

        [TestMethod]
        public void SaveFormToObject_LoadsObjectPropertyFromHiddenFieldValue()
        {
            // Arrange
            var hiddenField = new HiddenField { ID = "hf_HiddenFieldProperty", Value = "Test Value" };
            _form.Controls.Add(hiddenField);
            _testObject.HiddenFieldProperty = "";

            // Act
            _sut.SaveFormToObject(_form, _testObject);

            // Assert
            Assert.AreEqual("Test Value", _testObject.HiddenFieldProperty);
        }

        #endregion

    }
}
