using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using GridMvc.Columns;
using GridMvc.DataAnnotations;
using GridMvc.Tests.DataAnnotations.Models;
using NUnit.Framework;
using PowerAssert;

namespace GridMvc.Tests.DataAnnotations
{

    [TestFixture]
    public class GridDataAnnotationTests
    {
        private Grid<TestGridAnnotationModel> _grid;
        [SetUp]
        public void Init()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter()));
            _grid = new Grid<TestGridAnnotationModel>(Enumerable.Empty<TestGridAnnotationModel>().AsQueryable());
        }

        [Test]
        public void TestPaging()
        {
            PAssert.IsTrue(() => _grid.EnablePaging);
            PAssert.IsTrue(() => _grid.Pager.PageSize == 20);
        }

        [Test]
        public void TestColumnsDataAnnotation()
        {
            _grid.AutoGenerateColumns();
            int i = 0;
            foreach (var pi in typeof(TestGridAnnotationModel).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (pi.GetAttribute<NotMappedColumnAttribute>() != null)
                    continue;

                var gridOpt = pi.GetAttribute<GridColumnAttribute>();

                if (gridOpt != null)
                {
                    var column = _grid.Columns.ElementAt(i) as IGridColumn<TestGridAnnotationModel>;
                    if (column == null)
                        Assert.Fail();

                    PAssert.IsTrue(() => column.EncodeEnabled == gridOpt.EncodeEnabled);
                    PAssert.IsTrue(() => column.FilterEnabled == gridOpt.FilterEnabled);
                    PAssert.IsTrue(() => column.SanitizeEnabled == gridOpt.SanitizeEnabled);

                    if (!string.IsNullOrEmpty(gridOpt.Title))
                        PAssert.IsTrue(() => column.Title == gridOpt.Title);

                    if (!string.IsNullOrEmpty(gridOpt.Width))
                        PAssert.IsTrue(() => column.Width == gridOpt.Width);
                }
                i++;
            }
            PAssert.IsTrue(() => _grid.Columns.Count() == 3);
        }
    }
}
