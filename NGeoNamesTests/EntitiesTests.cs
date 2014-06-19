using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames.Entities;

namespace NGeoNamesTests
{
    [TestClass]
    public class EntitiesTests
    {
        [TestMethod]
        public void DistanceTo_ReturnsCorrectResult()
        {
            //http://en.wikipedia.org/w/index.php?title=Great-circle_distance&oldid=414838052
            var a = new GeoName { Latitude = 36.1172, Longitude = -86.6672 };   //Nashville International Airport (BNA) in Nashville, TN, USA: N 36°7.2', W 86°40.2'
            var b = new GeoName { Latitude = 33.9344, Longitude = -118.4 };     //Los Angeles International Airport (LAX) in Los Angeles, CA, USA: N 33°56.4', W 118°24.0'

            var x1 = a.DistanceTo(b);                       // DistanceTo(GeoName) overload
            var x2 = a.DistanceTo(b.Latitude, b.Longitude); // DistanceTo(double, double) overload
            var actual = 2887.26;                           // According to wikipedia

            //We use a slightly different radius of the earth than the wikipedia example uses (6372.8 in wikipedia vs. 6371 which is most commonly used)
            Assert.AreEqual(actual, x1, actual * .0005);     // All we want is to be within .05% of the "actual" (according to wikipedia) value
            Assert.AreEqual(actual, x2, actual * .0005);     // All we want is to be within .05% of the "actual" (according to wikipedia) value

            //Actually, we're only 0,0135% off...
        }

        [TestMethod]
        public void DistanceTo_WrapsAroundLongitudeCorrectly()
        {
            var a = new GeoName { Latitude = 51.377020, Longitude = 179.431888, Name = "Amchitka Island" };
            var b = new GeoName { Latitude = 51.272322, Longitude = -179.134396, Name = "Amatignak Island" };

            var actual = 100.3;   // +/- a bit; checked with http://www.freemaptools.com/measure-distance.htm and (classic) google maps measure tool

            var result = a.DistanceTo(b);
            Assert.AreEqual(actual, result, actual * .0005);    // All we want is to be within .05% of the "actual" (according to our own measurements) value
        }
    }
}
