using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;

namespace NGeoNamesTests
{
    
    [TestClass]
    public class GeoUtilTests
    {
        [TestMethod]
        public void GeoFileUtil_ConvertsMetricToImperialCorrecly()
        {
            var target = GeoUtil.MilesToMeters(200); //200 mile to meters
            Assert.AreEqual(321868.8, target);
        }

        [TestMethod]
        public void GeoFileUtil_ConvertsImperialToMetricCorrecly()
        {
            var target = GeoUtil.MetersToMiles(500);  //500 meters to miles
            Assert.AreEqual(0.310685596118667, target, float.Epsilon);
        }
    }
}
