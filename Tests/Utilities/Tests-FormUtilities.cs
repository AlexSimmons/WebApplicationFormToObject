using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.UI;
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


    }
}
