using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using System.Text;
using System.Linq;
using System;

namespace NGeoNamesTests
{
    [TestClass]
    public class GeoFileWriterTests
    {
        private static readonly CustomEntity[] testvalues = new [] { 
            new CustomEntity { Data = new [] { "Data L1☃F1", "Data L1☃F2", "Data L1☃F3"}},
            new CustomEntity { Data = new [] { "Data L2☃F1", "Data L2☃F2", "Data L2☃F3"}},
            new CustomEntity { Data = new [] { "Data L3☃F1", "Data L3☃F2", "Data L3☃F3"}},
        };

        [TestMethod]
        public void GeoFilWriter_ComposesFileCorrectly1()
        {
            new GeoFileWriter().WriteRecords<CustomEntity>(@"testdata\test_geofilewritercustom1.txt", testvalues, new CustomComposer(Encoding.UTF8, ','));

            var gf = new GeoFileReader();
            var target = gf.ReadRecords<CustomEntity>(@"testdata\test_geofilewritercustom1.txt", new CustomParser(3, 0, new[] { ',' }, Encoding.UTF8, false)).ToArray();
            Assert.AreEqual(3, target.Length);
            CollectionAssert.AreEqual(testvalues, target, new CustomEntityComparer());
        }

        [TestMethod]
        public void GeoFilWriter_ComposesFileCorrectly2()
        {
            new GeoFileWriter().WriteRecords<CustomEntity>(@"testdata\test_geofilewritercustom2.txt", testvalues, new CustomComposer(Encoding.UTF7, '!'));

            var gf = new GeoFileReader();
            var target = gf.ReadRecords<CustomEntity>(@"testdata\test_geofilewritercustom2.txt", new CustomParser(3, 0, new[] { '!' }, Encoding.UTF7, false)).ToArray();
            Assert.AreEqual(3, target.Length);
            CollectionAssert.AreEqual(testvalues, target, new CustomEntityComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GeoFilWriter_ThrowsOnFailureWhenAutodetectingFileType()
        {
            //When filetype == autodetect and an unknown extension is used an exception should be thrown
            new GeoFileWriter().WriteRecords<CustomEntity>(@"testdata\invalid.out.ext", testvalues, new CustomComposer(Encoding.UTF8, '\t'));
        }

        [TestMethod]
        public void GeoFilWriter_DoesNotThrowOnInvalidExtensionButSpecifiedFileType()
        {
            //When filetype is specified and an unknown extension is used it should be written fine
            new GeoFileWriter().WriteRecords<CustomEntity>(@"testdata\invalid.out.ext", testvalues, new CustomComposer(Encoding.UTF8, '\t'), FileType.Plain);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GeoFilWriter_ThrowsOnUnknownSpecifiedFileType()
        {
            //When and unknown filetype is specified an exception should be thrown
            new GeoFileWriter().WriteRecords<CustomEntity>(@"testdata\invalid.out.ext", testvalues, new CustomComposer(Encoding.UTF8, '\t'), (FileType)999);
        }
    }
}
