using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WebApplicationFormToObject.Utilities;
using static Tests.Utilities.Tests_PropertyUtilities;

namespace Tests.Utilities
{
    [TestClass]
    public class Tests_ConversionUtilities
    {
        private ConversionUtilities _sut; // System Under Test (SUT)

        [TestInitialize]

        public void TestSetup()
        {
            _sut = new ConversionUtilities();
        }

        [TestMethod]
        public void GetValueToLoad_ReturnsInteger_ForEnumProperty()
        {
            // Arrange
            var myObject = new TestObject { MyEnumProperty = MyEnum.SomeValue };
            var property = myObject.GetType().GetProperty("MyEnumProperty");

            // Act
            var result = _sut.GetValueToLoad(property, myObject);

            // Assert
            Assert.AreEqual((int)MyEnum.SomeValue, result);
        }

        [TestMethod]
        public void GetValueToLoad_ReturnsShortDateString_ForDateTimeProperty()
        {
            // Arrange
            var myObject = new TestObject { MyDateTimeProperty = new DateTime(2023, 12, 31) };
            var property = myObject.GetType().GetProperty("MyDateTimeProperty");
            var resultDate = myObject.MyDateTimeProperty.ToShortDateString();

            // Act
            var result = _sut.GetValueToLoad(property, myObject);

            // Assert
            Assert.AreEqual(resultDate, result);
        }

        [TestMethod]
        public void GetValueToLoad_ReturnsShortDateString_ForNullableDateTimeProperty()
        {
            // Arrange
            var myObject = new TestObject { MyNullableDateTimeProperty = new DateTime(2023, 12, 31) };
            var property = myObject.GetType().GetProperty("MyNullableDateTimeProperty");
            var resultDate = myObject.MyNullableDateTimeProperty.Value.ToShortDateString();


            // Act
            var result = _sut.GetValueToLoad(property, myObject);

            // Assert
            Assert.AreEqual(resultDate, result);
        }

        [TestMethod]
        public void GetValueToLoad_ReturnsNull_ForNullableDateTimeProperty_WhenValueIsNull()
        {
            // Arrange
            var myObject = new TestObject { MyNullableDateTimeProperty = null };
            var property = myObject.GetType().GetProperty("MyNullableDateTimeProperty");

            // Act
            var result = _sut.GetValueToLoad(property, myObject);

            // Assert
            Assert.IsNull(result);
        }
    }

    public class TestObject
    {
        public MyEnum MyEnumProperty { get; set; }
        public DateTime MyDateTimeProperty { get; set; }
        public DateTime? MyNullableDateTimeProperty { get; set; }
        // ... other properties
    }

    public enum MyEnum
    {
        SomeValue
    }

}