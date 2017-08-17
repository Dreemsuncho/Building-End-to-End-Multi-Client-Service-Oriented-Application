using System;
using System.Collections.Generic;
using System.ComponentModel;

using NUnit.Framework;

using Core.Common.Contracts;

namespace Core.Common.Tests
{
    [TestFixture]
    public class ObjectBase_Tests
    {
        [Test]
        public void test_clean_property_change()
        {
            // Arrange
            TestClass objTest = new TestClass();
            bool propertyChanged = false;

            objTest.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "CleanProp")
                    propertyChanged = true;
            };

            // Act
            objTest.CleanProp = "test value";

            // Assert
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public void test_dirty_property_change()
        {
            // Arrange
            TestClass objTest = new TestClass();

            // Act & Assert
            Assert.IsFalse(objTest.IsDirty);

            // Act
            objTest.DirtyProp = "test value";
            // Assert
            Assert.IsTrue(objTest.IsDirty);
        }

        [Test]
        public void test_property_change_single_subscription()
        {
            // Arrange
            TestClass objTest = new TestClass();
            int changeCounter = 0;
            PropertyChangedEventHandler handler1 = new PropertyChangedEventHandler((s, e) => { changeCounter++; });
            PropertyChangedEventHandler handler2 = new PropertyChangedEventHandler((s, e) => { changeCounter++; });

            objTest.PropertyChanged += handler1;
            objTest.PropertyChanged += handler1; // should not duplicate
            objTest.PropertyChanged += handler1; // should not duplicate
            objTest.PropertyChanged += handler2;
            objTest.PropertyChanged += handler2; // should not duplicate

            // Act
            objTest.CleanProp = "test value";

            // Assert
            Assert.IsTrue(changeCounter == 2);
        }

        [Test]
        public void test_property_change_dual_syntax()
        {
            // Arrange
            TestClass objTest = new TestClass();
            bool propertyChanged = false;

            objTest.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "CleanProp" || 
                    e.PropertyName == "StringProp")
                    propertyChanged = true;
            };

            // Act
            objTest.CleanProp = "test value";
            // Assert
            Assert.IsTrue(propertyChanged);

            propertyChanged = false;

            // Act
            objTest.StringProp = "test value";
            // Assert
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public void test_child_dirty_tracking()
        {
            // Arrange
            TestClass objTest = new TestClass();
            // Act & Assert
            Assert.IsFalse(objTest.IsAnythingDirty());
            // Act
            objTest.Child.ChildName = "test value";
            // Assert
            Assert.IsTrue(objTest.IsAnythingDirty());

            objTest.CleanAll();
            // Assert
            Assert.IsFalse(objTest.IsAnythingDirty());
        }

        [Test]
        public void test_dirty_object_aggregating()
        {
            // Arrange
            TestClass objTest = new TestClass();

            // Act
            var dirtyObjects = objTest.GetDirtyObjects();
            // Assert
            Assert.IsTrue(dirtyObjects.Count == 0);

            objTest.Child.ChildName = "test value";

            // Act
            dirtyObjects = objTest.GetDirtyObjects();
            // Assert
            Assert.IsTrue(dirtyObjects.Count == 1);

            objTest.DirtyProp = "test value";

            // Act
            dirtyObjects = objTest.GetDirtyObjects();
            // Assert
            Assert.IsTrue(dirtyObjects.Count == 2);

            objTest.CleanAll();

            // Act
            dirtyObjects = objTest.GetDirtyObjects();
            // Assert
            Assert.IsTrue(dirtyObjects.Count == 0);
        }

        [Test]
        public void test_object_validation()
        {
            // Arrange
            TestClass objTest = new TestClass();

            // Act & Assert
            Assert.IsFalse(objTest.IsValid);

            // Act
            objTest.StringProp = "Some value";
            // Asert
            Assert.IsTrue(objTest.IsValid);
        }
    }
}
