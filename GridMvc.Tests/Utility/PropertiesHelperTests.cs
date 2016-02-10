using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GridMvc.Utility;
using NUnit.Framework;
using PowerAssert;

namespace GridMvc.Tests.Utility
{
    [TestFixture]
    public class PropertiesHelperTests
    {


        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void TestColumnNameBuilding()
        {
            Expression<Func<TestModel, DateTime>> expr = m => m.Created;

            string name = PropertiesHelper.BuildColumnNameFromMemberExpression((MemberExpression)expr.Body);
            PAssert.IsTrue(() => name == "Created");

        }

        [Test]
        public void TestColumnNameBuildingChilds()
        {
            Expression<Func<TestModel, string>> expr = m => m.Child.ChildTitle;
            string name = PropertiesHelper.BuildColumnNameFromMemberExpression((MemberExpression)expr.Body);
            PAssert.IsTrue(() => name == "Child.ChildTitle");

        }

        [Test]
        public void TestGetPropertyFromColumnName()
        {
            const string columnName = "Created";
            IEnumerable<PropertyInfo> sequence;
            var pi = PropertiesHelper.GetPropertyFromColumnName(columnName, typeof(TestModel), out sequence);
            PAssert.IsTrue(() => sequence.Count() == 1);
            PAssert.IsTrue(() => pi.Name == "Created");
        }

        [Test]
        public void TestGetPropertyFromColumnNameChilds()
        {
            const string columnName = "Child.ChildTitle";
            IEnumerable<PropertyInfo> sequence;
            var pi = PropertiesHelper.GetPropertyFromColumnName(columnName, typeof(TestModel), out sequence);
            PAssert.IsTrue(() => sequence.Count() == 2);
            PAssert.IsTrue(() => pi.Name == "ChildTitle");
        }



    }
}
