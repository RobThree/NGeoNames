using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using NGeoNames.Entities;
using System.Linq;

namespace NGeoNamesTests
{
    [TestClass]
    public class ReverseGeoCodeTests
    {
        [TestMethod]
        public void ReverseGeoCode_RadialSearch_ReturnsCorrectResults()
        {
            // Read a file with data with points in and around London in a 20Km radius
            var data = GeoFileReader.ReadExtendedGeoNames(@"testdata\test_GB.txt").ToArray();
            var rg = new ReverseGeoCode<ExtendedGeoName>(data);
            var center = rg.CreateFromLatLong(51.5286416, 0);   //Exactly at 0 longitude so we test left/right of prime meridian

            Assert.AreEqual(47, data.Length);   //File should contain 47 records total

            var expected_ids = new[] { 2640729, 2639577, 2642465, 2637627, 2633709, 2643339, 2634677, 2636503, 2652053, 2654710, 2643743, 2646003, 2643741, 2653941, 6690870, 2655775, 2651621, 2650497, 2656194, 2653266, 2648657, 2637433, 2652618, 2646057 };

            // Search from the/a center in London for all points in a 10Km radius
            var searchresults = rg.RadialSearch(center, 100000.0).ToArray();
            // Number of results should match length of expected_id array
            Assert.AreEqual(expected_ids.Length, searchresults.Length);
            // Check if each result is in the expected results array
            foreach (var r in searchresults)
                Assert.IsTrue(expected_ids.Contains(r.Id));
        }

        [TestMethod]
        public void ReverseGeoCode_RadialSearch_ReturnsMaxCountResults()
        {
            // Read a file with data with points in and around London in a 20Km radius
            var data = GeoFileReader.ReadExtendedGeoNames(@"testdata\test_GB.txt").ToArray();
            var rg = new ReverseGeoCode<ExtendedGeoName>(data);
            var center = rg.CreateFromLatLong(51.5286416, 0);   //Exactly at 0 longitude so we test left/right of prime meridian
            var maxresults = 10;

            Assert.AreEqual(47, data.Length);   //File should contain 47 records total

            var expected_ids = new[] { 2643741, 2646003, 2643743, 6690870, 2651621, 2655775, 2636503, 2634677, 2656194, 2653266 };
            Assert.AreEqual(maxresults, expected_ids.Length);

            // Search from the/a center in London for all points in a 10Km radius, allowing only maxresults results
            var searchresults = rg.RadialSearch(center, 100000.0, maxresults).ToArray();
            // Number of results should match length of expected_id array
            Assert.AreEqual(expected_ids.Length, searchresults.Length);
            // Check if each result is in the expected results array
            foreach (var r in searchresults)
                Assert.IsTrue(expected_ids.Contains(r.Id));
        }

        [TestMethod]
        public void ReverseGeoCode_NearestNeighbourSearch_ReturnsCorrectResults()
        {
            // Read a file with data with points in and around London in a 20Km radius
            var data = GeoFileReader.ReadExtendedGeoNames(@"testdata\test_GB.txt").ToArray();
            var rg = new ReverseGeoCode<ExtendedGeoName>(data);
            var center = rg.CreateFromLatLong(51.5286416, 0);   //Exactly at 0 longitude so we test left/right of prime meridian

            Assert.AreEqual(47, data.Length);   //File should contain 47 records total

            var expected_ids = new[] { 2640729, 2639577, 2642465, 2637627, 2633709, 2643339, 2634677, 2636503, 2652053, 2654710, 2643743, 2646003, 2643741, 2653941, 6690870, 2655775, 2651621, 2650497, 2656194, 2653266, 2648657, 2637433, 2652618, 2646057 };

            // Search from the/a center in London for the first X points (where X == expected_ids.length)
            var searchresults = rg.NearestNeighbourSearch(center, expected_ids.Length).ToArray();
            // Number of results should match length of expected_id array
            Assert.AreEqual(expected_ids.Length, searchresults.Length);
            // Check if each result is in the expected results array
            foreach (var r in searchresults)
                Assert.IsTrue(expected_ids.Contains(r.Id));
        }
    }
}
