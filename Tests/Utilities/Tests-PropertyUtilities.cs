using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationFormToObject.Utilities;

namespace Tests.Utilities
{

    [TestClass]
    public class Tests_PropertyUtilities
    {
        private PropertyUtilities _sut; // System Under Test (SUT)

        [TestInitialize]

        public void TestSetup()
        {
            _sut = new PropertyUtilities();
        }

        public class TestObject
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            public bool Prop3 { get; set; }
        }

        [TestMethod]
        public void GetProperties_ReturnsCorrectNumberOfProperties()
        {
            // Arrange
            var testObject = new TestObject();

            // Act
            var properties = _sut.GetProperties(testObject);

            // Assert
            Assert.AreEqual(3, properties.Length);
        }
    }
}

