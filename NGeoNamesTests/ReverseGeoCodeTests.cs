using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using NGeoNames.Parsers;
using NGeoNames.Entities;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace NGeoNamesTests
{
    [TestClass]
    public class ReverseGeoCodeTests
    {
        //[TestMethod]
        //public void Foo()
        //{
        //    var x = new ReverseGeoCode<ExtendedGeoName>(GeoFileReader.ReadExtendedGeoNames(@"d:\test\geo\GB.txt"));
        //    var center = x.CreateFromLatLong(51.5286416, 0);
        //    var q = x.RadialSearch(center, 100.0);
        //    foreach (var r in q.OrderBy(i => i.Longitude))
        //        Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:N6},{1:N6} {2} ({0:N6},{1:N6}) [{3}]", r.Latitude, r.Longitude, r.Name, r.Id));
        //}
    }
}
