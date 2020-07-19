using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGeoNamesTests
{
    [TestClass]
    public class GeoFileWriterTests
    {
        private static readonly IAsyncEnumerable<CustomEntity> testvalues = new[] {
            new CustomEntity { Data = new [] { "Data L1☃F1", "Data L1☃F2", "Data L1☃F3"}},
            new CustomEntity { Data = new [] { "Data L2☃F1", "Data L2☃F2", "Data L2☃F3"}},
            new CustomEntity { Data = new [] { "Data L3☃F1", "Data L3☃F2", "Data L3☃F3"}},
        }.ToAsyncEnumerable();

        [TestMethod]
        public async Task GeoFileWriter_ComposesFileCorrectly1()
        {

            await GeoFileWriter.WriteRecordsAsync(@"testdata\test_geofilewritercustom1.txt", testvalues, new CustomComposer(Encoding.UTF8, ','));

            var target = await GeoFileReader.ReadRecordsAsync(@"testdata\test_geofilewritercustom1.txt", new CustomParser(3, 0, new[] { ',' }, Encoding.UTF8, false)).ToArrayAsync();
            Assert.AreEqual(3, target.Length);
            CollectionAssert.AreEqual(await testvalues.ToArrayAsync(), target, new CustomEntityComparer());
        }

        [TestMethod]
        public async Task GeoFileWriter_ComposesFileCorrectly2()
        {
            await GeoFileWriter.WriteRecordsAsync(@"testdata\test_geofilewritercustom2.txt", testvalues, new CustomComposer(Encoding.UTF7, '!'));

            var target = await GeoFileReader.ReadRecordsAsync(@"testdata\test_geofilewritercustom2.txt", new CustomParser(3, 0, new[] { '!' }, Encoding.UTF7, false)).ToArrayAsync();
            Assert.AreEqual(3, target.Length);
            CollectionAssert.AreEqual(await testvalues.ToArrayAsync(), target, new CustomEntityComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public async Task GeoFileWriter_ThrowsOnFailureWhenAutodetectingFileType()
        {
            //When filetype == autodetect and an unknown extension is used an exception should be thrown
            await GeoFileWriter.WriteRecordsAsync(@"testdata\invalid.out.ext", testvalues, new CustomComposer(Encoding.UTF8, '\t'));
        }

        [TestMethod]
        public async Task GeoFileWriter_DoesNotThrowOnInvalidExtensionButSpecifiedFileType()
        {
            //When filetype is specified and an unknown extension is used it should be written fine
            await GeoFileWriter.WriteRecordsAsync(@"testdata\invalid.out.ext", testvalues, new CustomComposer(Encoding.UTF8, '\t'), FileType.Plain);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public async Task GeoFileWriter_ThrowsOnUnknownSpecifiedFileType()
        {
            //When and unknown filetype is specified an exception should be thrown
            await GeoFileWriter.WriteRecordsAsync(@"testdata\invalid.out.ext", testvalues, new CustomComposer(Encoding.UTF8, '\t'), (FileType)999);
        }
    }
}
