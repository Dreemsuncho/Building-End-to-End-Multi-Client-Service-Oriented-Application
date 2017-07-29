using System;
using System.Collections.Generic;
using System.ComponentModel;

using NUnit.Framework;

using Core.Common.Contracts;

namespace Core.Common.Tests
{
    [TestFixture]
    public class ObjectBaseTests
    {
        [Test]
        public void test_clean_property_change()
        {
            TestClass objTest = new TestClass();
            bool propertyChanged = false;

            objTest.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "CleanProp")
                    propertyChanged = true;
            };

            objTest.CleanProp = "test value";

            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public void test_dirty_property_change()
        {
            TestClass objTest = new TestClass();

            Assert.IsFalse(objTest.IsDirty);

            objTest.DirtyProp = "test value";

            Assert.IsTrue(objTest.IsDirty);
        }

        [Test]
        public void test_property_change_single_subscription()
        {
            TestClass objTest = new TestClass();
            int changeCounter = 0;
            PropertyChangedEventHandler handler1 = new PropertyChangedEventHandler((s, e) => { changeCounter++; });
            PropertyChangedEventHandler handler2 = new PropertyChangedEventHandler((s, e) => { changeCounter++; });

            objTest.PropertyChanged += handler1;
            objTest.PropertyChanged += handler1; // should not duplicate
            objTest.PropertyChanged += handler1; // should not duplicate
            objTest.PropertyChanged += handler2;
            objTest.PropertyChanged += handler2; // should not duplicate

            objTest.CleanProp = "test value";

            Assert.IsTrue(changeCounter == 2);
        }

        [Test]
        public void test_property_change_dual_syntax()
        {
            TestClass objTest = new TestClass();
            bool propertyChanged = false;

            objTest.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "CleanProp" || 
                    e.PropertyName == "StringProp")
                    propertyChanged = true;
            };

            objTest.CleanProp = "test value";
            Assert.IsTrue(propertyChanged);

            propertyChanged = false;

            objTest.StringProp = "test value";
            Assert.IsTrue(propertyChanged);
        }

        [Test]
        public void test_child_dirty_tracking()
        {
            TestClass objTest = new TestClass();

            Assert.IsFalse(objTest.IsAnythingDirty());

            objTest.Child.ChildName = "test value";

            Assert.IsTrue(objTest.IsAnythingDirty());

            objTest.CleanAll();

            Assert.IsFalse(objTest.IsAnythingDirty());
        }

        [Test]
        public void test_dirty_object_aggregating()
        {
            TestClass objTest = new TestClass();

            IList<IDirtyCapable> dirtyObjects = objTest.GetDirtyObjects();

            Assert.IsTrue(dirtyObjects.Count == 0);

            objTest.Child.ChildName = "test value";
            dirtyObjects = objTest.GetDirtyObjects();

            Assert.IsTrue(dirtyObjects.Count == 1);

            objTest.DirtyProp = "test value";
            dirtyObjects = objTest.GetDirtyObjects();

            Assert.IsTrue(dirtyObjects.Count == 2);

            objTest.CleanAll();
            dirtyObjects = objTest.GetDirtyObjects();

            Assert.IsTrue(dirtyObjects.Count == 0);
        }

        [Test]
        public void test_object_validation()
        {
            TestClass objTest = new TestClass();

            Assert.IsFalse(objTest.IsValid);

            objTest.StringProp = "Some value";

            Assert.IsTrue(objTest.IsValid);
        }
    }
}
