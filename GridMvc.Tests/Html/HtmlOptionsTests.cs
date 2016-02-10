using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GridMvc.Html;
using Moq;
using NUnit.Framework;
using PowerAssert;

namespace GridMvc.Tests.Html
{
    [TestFixture]
    public class HtmlOptionsTests
    {
        private TestGrid _grid;
        private GridHtmlOptions<TestModel> _opt;

        [SetUp]
        public void Init()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new StringWriter()));
            _grid = new TestGrid(Enumerable.Empty<TestModel>());
            var viewContextMock = new Mock<ViewContext>();
            _opt = new GridHtmlOptions<TestModel>(_grid, viewContextMock.Object, "_Grid");
        }

        [Test]
        public void TestMainMethods()
        {
            _opt.WithPaging(5);
            PAssert.IsTrue(() => _grid.EnablePaging);
            PAssert.IsTrue(() => _grid.Pager.PageSize == 5);

            _opt.WithMultipleFilters();
            PAssert.IsTrue(() => _grid.RenderOptions.AllowMultipleFilters);

			_opt.Named("test");
            PAssert.IsTrue(() => _grid.RenderOptions.GridName == "test");

            _opt.Selectable(true);
            PAssert.IsTrue(() => _grid.RenderOptions.Selectable);
        }
    }
}
