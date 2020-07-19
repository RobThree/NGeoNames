using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGeoNames;
using NGeoNames.Parsers;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGeoNamesTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public async Task ParserThrowsOnInvalidData()
        {
            await GeoFileReader.ReadAdmin1Codes(@"testdata\invalid_admin1CodesASCII.txt").ToArrayAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task ParserThrowsOnNonExistingFile()
        {
            await GeoFileReader.ReadAdmin1Codes(@"testdata\non_existing_file.txt").ToArrayAsync();
        }

        [TestMethod]
        public async Task Admin1CodesParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadAdmin1Codes(@"testdata\test_admin1CodesASCII.txt").ToArrayAsync();
            Assert.AreEqual(4, target.Length);

            Assert.AreEqual("CF.04", target[0].Code);
            Assert.AreEqual("Mambéré-Kadéï", target[0].Name);
            Assert.AreEqual("Mambere-Kadei", target[0].NameASCII);
            Assert.AreEqual(2386161, target[0].GeoNameId);

            Assert.AreEqual("This.is.a.VERY.LONG.ID", target[1].Code);
            Assert.AreEqual("Iğdır", target[1].Name);
            Assert.AreEqual("Igdir", target[1].NameASCII);
            Assert.AreEqual(0, target[1].GeoNameId);

            Assert.AreEqual("TR.80", target[2].Code);
            Assert.AreEqual("Şırnak", target[2].Name);
            Assert.AreEqual("Sirnak", target[2].NameASCII);
            Assert.AreEqual(443189, target[2].GeoNameId);

            Assert.AreEqual("UA.26", target[3].Code);
            Assert.AreEqual("Zaporiz’ka Oblast’", target[3].Name);
            Assert.AreEqual("Zaporiz'ka Oblast'", target[3].NameASCII);
            Assert.AreEqual(687699, target[3].GeoNameId);
        }

        [TestMethod]
        public async Task Admin2CodesParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadAdmin2Codes(@"testdata\test_admin2Codes.txt").ToArrayAsync();
            Assert.AreEqual(2, target.Length);

            Assert.AreEqual("AF.01.7052666", target[0].Code);
            Assert.AreEqual("Darwāz-e Bālā", target[0].Name);
            Assert.AreEqual("Darwaz-e Bala", target[0].NameASCII);
            Assert.AreEqual(7052666, target[0].GeoNameId);

            Assert.AreEqual("CA.10.11", target[1].Code);
            Assert.AreEqual("Gaspésie-Îles-de-la-Madeleine", target[1].Name);
            Assert.AreEqual("Gaspesie-Iles-de-la-Madeleine", target[1].NameASCII);
            Assert.AreEqual(0, target[1].GeoNameId);
        }

        [TestMethod]
        public async Task AlternateNamesParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadAlternateNames(@"testdata\test_alternateNames.txt").ToArrayAsync();
            Assert.AreEqual(17, target.Length);

            Assert.AreEqual(2488123, target[0].Id);
            Assert.AreEqual(4, target[0].GeoNameId);
            Assert.AreEqual("رودخانه زاکلی", target[0].Name);
            Assert.AreEqual("fa", target[0].ISOLanguage);       //Ensure we have an ISO code
            Assert.IsNull(target[0].Type);                      //When ISO code is specified, type should be null
            Assert.IsFalse(target[0].IsPreferredName);  //All these...
            Assert.IsFalse(target[0].IsColloquial);     //...bits should...
            Assert.IsFalse(target[0].IsShortName);      //...be set...
            Assert.IsFalse(target[0].IsHistoric);       //...to false

            Assert.AreEqual("http://en.wikipedia.org/wiki/Takht-e_Qeysar", target[1].Name);
            Assert.IsNull(target[1].ISOLanguage);           //Should be null when a type is specified (e.g. length of ISO code field > 3)
            Assert.AreEqual("link", target[1].Type);        //Ensure we have a type

            Assert.AreEqual("Нагольная", target[2].Name);

            Assert.AreEqual("บ้านน้ำฉ่า", target[3].Name);
            Assert.AreEqual("th", target[3].ISOLanguage);

            Assert.AreEqual("글렌로시스", target[4].Name);
            Assert.AreEqual("ko", target[4].ISOLanguage);

            //Postal code
            Assert.AreEqual("TW13", target[5].Name);
            Assert.AreEqual("post", target[5].Type);

            //IATA - International Air Transport Association; airport code
            Assert.AreEqual("FAB", target[6].Name);
            Assert.AreEqual("iata", target[6].Type);

            //ICAO - International Civil Aviation Organization airport code
            Assert.AreEqual("LSGG", target[7].Name);
            Assert.AreEqual("icao", target[7].Type);

            //ICAO - International Civil Aviation Organization airport code
            Assert.AreEqual("GSN", target[8].Name);
            Assert.AreEqual("faac", target[8].Type);

            Assert.AreEqual("Saipan International Airport", target[9].Name);
            Assert.AreEqual("", target[9].ISOLanguage); //No language,...
            Assert.IsNull(target[9].Type);              //...no type

            //Link
            Assert.AreEqual("http://en.wikipedia.org/wiki/Saipan_International_Airport", target[10].Name);
            Assert.AreEqual("link", target[10].Type);

            //fr_1793 - French Revolution name
            Assert.AreEqual("Ile-de-la-Liberté", target[11].Name);
            Assert.AreEqual("fr_1793", target[11].Type);

            Assert.AreEqual("ཞིང་རི", target[12].Name);
            Assert.AreEqual("bo", target[12].ISOLanguage);

            //Abbreviation
            Assert.AreEqual("TRTO", target[13].Name);
            Assert.AreEqual("abbr", target[13].Type);

            //Check flags/bits
            Assert.AreEqual("The Jekyll & Hyde Pub", target[14].Name);
            Assert.IsTrue(target[14].IsPreferredName);
            Assert.IsFalse(target[14].IsColloquial);
            Assert.IsTrue(target[14].IsShortName);
            Assert.IsFalse(target[14].IsHistoric);

            Assert.AreEqual("Torre Fiumicelli", target[15].Name);
            Assert.IsFalse(target[15].IsPreferredName);
            Assert.IsTrue(target[15].IsColloquial);
            Assert.IsFalse(target[15].IsShortName);
            Assert.IsFalse(target[15].IsHistoric);

            Assert.AreEqual("Abbaye Saint-Antoine-des-Champs", target[16].Name);
            Assert.IsFalse(target[16].IsPreferredName);
            Assert.IsFalse(target[16].IsColloquial);
            Assert.IsFalse(target[16].IsShortName);
            Assert.IsTrue(target[16].IsHistoric);
        }

        [TestMethod]
        public async Task AlternateNamesParserV2_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadAlternateNamesV2(@"testdata\test_alternateNamesV2.txt").ToArrayAsync();
            Assert.AreEqual(5, target.Length);

            Assert.AreEqual(12453592, target[0].Id);
            Assert.AreEqual(7258581, target[0].GeoNameId);
            Assert.AreEqual("Rossmoor CDP", target[0].Name);
            Assert.AreEqual(string.Empty, target[0].ISOLanguage);
            Assert.IsNull(target[0].Type);
            Assert.IsFalse(target[0].IsPreferredName);
            Assert.IsFalse(target[0].IsColloquial);
            Assert.IsFalse(target[0].IsPreferredName);
            Assert.IsTrue(target[0].IsHistoric);

            Assert.AreEqual(string.Empty, target[0].From);
            Assert.AreEqual("19 February 2008", target[0].To);

            Assert.AreEqual("1961", target[1].From);
            Assert.AreEqual(string.Empty, target[1].To);

            Assert.AreEqual("قولەی کانی ماران", target[2].From);
            Assert.AreEqual("Qulai Kanimaran", target[2].To);

            Assert.AreEqual("19150903", target[3].From);
            Assert.AreEqual(string.Empty, target[3].To);

            Assert.AreEqual("1589", target[4].From);
            Assert.AreEqual("1925", target[4].To);
        }

        [TestMethod]
        public async Task CountryInfoParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadCountryInfo(@"testdata\test_countryInfo.txt").ToArrayAsync();
            Assert.AreEqual(2, target.Length);  //Should've skipped comment lines

            Assert.AreEqual("BZ", target[0].ISO_Alpha2);
            Assert.AreEqual("BLZ", target[0].ISO_Alpha3);
            Assert.AreEqual("084", target[0].ISO_Numeric);  //Check if leading zeroes are preserved
            Assert.AreEqual("BH", target[0].FIPS);
            Assert.AreEqual("Belize", target[0].Country);
            Assert.AreEqual("Belmopan", target[0].Capital);
            Assert.AreEqual(22966, target[0].Area);
            Assert.AreEqual(314522, target[0].Population);
            Assert.AreEqual("NA", target[0].Continent);
            Assert.AreEqual(".bz", target[0].Tld);
            Assert.AreEqual("BZD", target[0].CurrencyCode);
            Assert.AreEqual("Dollar", target[0].CurrencyName);
            Assert.AreEqual("+501", target[0].Phone);       //Intl. dialingcodes should be prefixed with a + even though they're not in the file
            Assert.AreEqual(string.Empty, target[0].PostalCodeFormat);
            Assert.AreEqual(string.Empty, target[0].PostalCodeRegex);
            CollectionAssert.AreEqual(new[] { "en-BZ", "es" }, target[0].Languages);
            Assert.AreEqual(3582678, target[0].GeoNameId);
            CollectionAssert.AreEqual(new[] { "GT", "MX" }, target[0].Neighbours);
            Assert.AreEqual(string.Empty, target[0].EquivalentFipsCode);

            Assert.AreEqual("XX", target[1].ISO_Alpha2);
            Assert.AreEqual("XXX", target[1].ISO_Alpha3);
            Assert.AreEqual("999", target[1].ISO_Numeric);
            Assert.AreEqual(string.Empty, target[1].FIPS);
            Assert.AreEqual("MADE UP COUNTRY", target[1].Country);
            Assert.AreEqual(string.Empty, target[1].Capital);
            Assert.IsNull(target[1].Area);  //Can be unspecified/null
            Assert.AreEqual(0, target[1].Population);
            Assert.AreEqual("EU", target[1].Continent);
            Assert.AreEqual(".XX", target[1].Tld);
            Assert.AreEqual("XXX", target[1].CurrencyCode);
            Assert.AreEqual("X-Dollar", target[1].CurrencyName);
            Assert.AreEqual("+1", target[1].Phone);       //Intl. dialingcodes should be prefixed with a + even though they're not in the file
            Assert.AreEqual("@# #@@|@## #@@|@@# #@@|@@## #@@|@#@ #@@|@@#@ #@@|GIR0AA", target[1].PostalCodeFormat);
            Assert.AreEqual(@"^(([A-Z]\d{2}[A-Z]{2})|([A-Z]\d{3}[A-Z]{2})|([A-Z]{2}\d{2}[A-Z]{2})|([A-Z]{2}\d{3}[A-Z]{2})|([A-Z]\d[A-Z]\d[A-Z]{2})|([A-Z]{2}\d[A-Z]\d[A-Z]{2})|(GIR0AA))$", target[1].PostalCodeRegex);
            Assert.AreEqual(0, target[1].Languages.Length);
            Assert.IsNull(target[1].GeoNameId);
            Assert.AreEqual(0, target[1].Neighbours.Length);
            Assert.AreEqual("XXX", target[1].EquivalentFipsCode);
        }

        [TestMethod]
        public async Task ExtendedGeoNameParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadExtendedGeoNames(@"testdata\test_extendedgeonames.txt").ToArrayAsync();
            Assert.AreEqual(7, target.Length);

            //No alternate names/countrycodes and empty admincodes
            Assert.AreEqual(string.Empty, target[0].Admincodes[0]);
            Assert.AreEqual(string.Empty, target[0].Admincodes[1]);
            Assert.AreEqual(string.Empty, target[0].Admincodes[2]);
            Assert.AreEqual(string.Empty, target[0].Admincodes[3]);
            Assert.AreEqual(0, target[0].AlternateCountryCodes.Length);
            Assert.AreEqual(0, target[0].AlternateNames.Length);

            //Specified alternate names/countrycodes and specified admincodes
            Assert.AreEqual("09", target[1].Admincodes[0]);
            Assert.AreEqual("900", target[1].Admincodes[1]);
            Assert.AreEqual("923", target[1].Admincodes[2]);
            Assert.AreEqual("2771781", target[1].Admincodes[3]);
            Assert.AreEqual(1, target[1].AlternateCountryCodes.Length);
            Assert.AreEqual(2, target[1].AlternateNames.Length);

            //Lots of alternate countrycodes/alternatenames
            CollectionAssert.AreEqual(new[] { "DZ", "LY", "MR", "TN", "EG", "MA", "EH", "ML", "NE", "TD", "SD" }, target[2].AlternateCountryCodes);
            #region Charset test
            CollectionAssert.AreEqual(new[] {
                "An Bhoisnia agus Heirseagovein",
                "An Bhoisnia agus Heirseagóvéin",
                "An Bhoisnia-Heirseagaivein",
                "An Bhoisnia-Heirseagaivéin",
                "Bhosnia le Herzegovina",
                "Bo-xni-a Hec-xe-go-vi-na",
                "Bo-xni-a Hec-xe-go-vi-na (Bosnia va Herzegovina)",
                "Bos'nija i Gercagavina",
                "Bosenia me Hesegowina",
                "Bosini mpe Hezegovine",
                "Bosini mpé Hezegovine",
                "Bosiniya na Herigozevine",
                "Bosiniya na Herizegovina",
                "Bosmudin boln Khercegudin Orn",
                "Bosna",
                "Bosna Hersek",
                "Bosna a Hercegovina",
                "Bosna a Hercegowina",
                "Bosna agus Hearsagobhana",
                "Bosna doo Hetsog Bikeyah",
                "Bosna dóó Hetsog Bikéyah",
                "Bosna i Hercegovina",
                "Bosna i Khercegovina",
                "Bosna in Hercegovina",
                "Bosna kap Hercegovina",
                "Bosna u Hersek",
                "Bosna va Hercegovina",
                "Bosna ve Hersek",
                "Bosna và Hercegovina",
                "Bosna-Gercegovina",
                "Bosna-Hersek",
                "Bosnaen e Haerzegovaen",
                "Bosneja i Hercegovina",
                "Bosneje er Hercuogovena",
                "Bosni",
                "Bosni a Gercegovina a",
                "Bosni aemae Gercegovinae",
                "Bosni ak Erzegovin",
                "Bosni ba Gercegovina",
                "Bosni ba Khercegovina",
                "Bosni tata Gercegovina",
                "Bosnia",
                "Bosnia & Herzegovina",
                "Bosnia - Erzegobine",
                "Bosnia - Hercegovina - Bosna i Khercegovina",
                "Bosnia - Hercegovina - Босна и Херцеговина",
                "Bosnia Ercegovina",
                "Bosnia Erzegovina",
                "Bosnia Herzegovina",
                "Bosnia Herzogovina",
                "Bosnia Hèrzègovina",
                "Bosnia a Hercegovina",
                "Bosnia a Herzegovina",
                "Bosnia aamma Herzegovina",
                "Bosnia and Hercegovina",
                "Bosnia and Herzegovina",
                "Bosnia as Herzegovina",
                "Bosnia at Herzegovina",
                "Bosnia ati Herjegofina",
                "Bosnia dan Herzegovina",
                "Bosnia e Ercegovina",
                "Bosnia e Erzegovina",
                "Bosnia e Erzegòvina",
                "Bosnia e Hercegovina",
                "Bosnia e Hertsegovina",
                "Bosnia e Herzegovina",
                "Bosnia ed Erzegovina",
                "Bosnia et Herzegovina",
                "Bosnia ev Hercʻegovina",
                "Bosnia ha Herzegovina",
                "Bosnia i Gercagavina",
                "Bosnia i Gercogovina",
                "Bosnia i Hercegovina",
                "Bosnia i Hercegowina",
                "Bosnia i Hersegovina",
                "Bosnia ihuan Hertzegovina",
                "Bosnia ja Hercegovina",
                "Bosnia ja Hertsegoviina",
                "Bosnia ja Hertsegovina",
                "Bosnia ja Herzegovina",
                "Bosnia jeung Herzegovina",
                "Bosnia jeung Hérzégovina",
                "Bosnia kai Erzegobine",
                "Bosnia ken Herzegovina",
                "Bosnia kple Herzergovina nutome",
                "Bosnia ma Herzegovina",
                "Bosnia na Erzegovina",
                "Bosnia na Herzegovina",
                "Bosnia na Hezegovina",
                "Bosnia ne Hɛzegovina",
                "Bosnia og Hercegovina",
                "Bosnia og Hersegovina",
                "Bosnia si Hertegovina",
                "Bosnia sy Herzegovina",
                "Bosnia ta Gercogovina",
                "Bosnia tan Hersegobina",
                "Bosnia ug Herzegovina",
                "Bosnia y Hercegovina",
                "Bosnia y Herzegovina",
                "Bosnia è Erzegovina",
                "Bosnia īhuān Hertzegovina",
                "Bosnia și Herțegovina",
                "Bosnia-Ercegovina",
                "Bosnia-Erzegovina",
                "Bosnia-Hercegovina",
                "Bosnia-Hersegovina",
                "Bosnia-Hertsegovina",
                "Bosnia-Herzegovina",
                "Bosnia-ha-Herzegovina",
                "Bosnie Herzegovina",
                "Bosnie an Herzegovinae",
                "Bosnie en Herzegovina",
                "Bosnie en Herzegowina",
                "Bosnie-Erzegovine",
                "Bosnie-Hercegovina",
                "Bosnie-Herzegovena",
                "Bosnie-Herzegovina",
                "Bosnie-Herzegovine",
                "Bosnie-Herzegowina",
                "Bosnie-Herzégovine",
                "Bosnie-Hèrzègovena",
                "Bosnie-Érzégovine",
                "Bosniehreh Gercegovinehreh",
                "Bosnien an Herzegowina",
                "Bosnien och Hercegovina",
                "Bosnien un Herzegowina",
                "Bosnien und Herzegowina",
                "Bosnien-Hercegovina",
                "Bosnien-Herzegowina",
                "Bosnii Hersegowiin",
                "Bosnii da Gercegovin",
                "Bosnii na Herzegovinni",
                "Bosnij da Gercegovina",
                "Bosnija",
                "Bosnija bla Gercegovina",
                "Bosnija da Gercegovina",
                "Bosnija di Khercegovina",
                "Bosnija i Gercegovina",
                "Bosnija ir Hercegovina",
                "Bosnija no Gercegovina",
                "Bosnija un Hercegovina",
                "Bosnija uonna Khercegovina",
                "Bosnijo e Hercegowina",
                "Bosnikondre",
                "Bosnio kaj Hercegovino",
                "Bosnio-Hercegovino",
                "Bosniska a Hercegowina",
                "Bosniska-Hercegowinska",
                "Bosniya",
                "Bosniya Harzagobina",
                "Bosniya Hersigoviina",
                "Bosniya ham Gertsegovina",
                "Bosniya hem Hertegovina",
                "Bosniya hem Herțegovina",
                "Bosniya u Herzegovina",
                "Bosniya va Gersegovina",
                "Bosniya və Herseqovina",
                "Bosniya və Herzokovina",
                "Bosniya we Gersegowina",
                "Bosniya û Herzegovîna",
                "Bosnië Herzegovina",
                "Bosnië en Herzegovina",
                "Bosnië en Herzegowina",
                "Bosnië-Hercegovina",
                "Bosnië-Herzegovina",
                "Bosnië-Herzegowina",
                "Bosniýa we Gersegowina",
                "Bosni–Hercegovina",
                "Bosnja dhe Hercegovina",
                "Bosnje",
                "Bosnya a Hersegowina",
                "Bosnya asin Hersegobina",
                "Bosnya ngan Hersegovina",
                "Bosnän e Härzegovän",
                "Bosnía og Hersegóvína",
                "Bosnïi na Herzegovînni",
                "Bosnėjė ėr Hercuogovėna",
                "Bossnije-Haezzejovina",
                "Bosznia es Hercegovina",
                "Bosznia és Hercegovina",
                "Bosznia-Hercegovina",
                "Boteniya me Erdegobina",
                "Boziniya Hezegovina",
                "Bozni-Ɛrizigovini",
                "Boznia ne Herzegovina",
                "Boznija Herzegovina",
                "Boznija u Herzegovina",
                "Boßnije-Häzzejovina",
                "Bośnia i Hercegowina",
                "Bośńa a Hercegowina",
                "Bożnija u Ħerżegovina",
                "Bożnija Ħerżegovina",
                "Busna-Hirsiquwina",
                "Bòsnia Erzegovina",
                "Bòsnia e Ercegovina",
                "Bòsnia e Erzegòvina",
                "Bòsnia i Hercegovina",
                "Bòsnia-Erçegòvina",
                "Bòsnijô ë Hercegòwina",
                "Bósnia Ercegovina",
                "Bósnia e Herzegovina",
                "Bósnia-Herzegóvina",
                "Bósníà àti Hẹrjẹgòfínà",
                "Bô-xni-a Héc-xê-gô-vi-na",
                "Bô-xni-a Héc-xê-gô-vi-na (Bosnia và Herzegovina)",
                "IBhosinya ne Hezegovi",
                "Mbosini ne Hezegovine",
                "Narodna Republika Bosna i Hercegovina",
                "Orileede Bosinia ati Etisegofina",
                "Orílẹ́ède Bọ̀síníà àti Ẹtisẹgófínà",
                "People's Republic of Bosnia and Hercegovina",
                "People’s Republic of Bosnia and Hercegovina",
                "Po-su-ni-a lau Het-set-ko-vi-na",
                "Pongia-Herekomina",
                "Posinia mo Hesikovinia",
                "Posinia mo Hesikōvinia",
                "Pô-sṳ-nì-â lâu Het-set-kô-vì-ná",
                "Pōngia-Herekōmina",
                "Republic of Bosnia and Herzegovina",
                "Republika Bosna i Hercegovina",
                "Socialist Republic of Bosnia and Hercegovina",
                "Socijalisticka Republika Bosna i Hercegovina",
                "Socijalistička Republika Bosna i Hercegovina",
                "Vonia ha Hesegovina",
                "Vosnia kai Erzegovini",
                "albwsnh w alhrsk",
                "albwsnt w alhrsk",
                "albwsnt walhrsk",
                "basaniya baro harjegobhina",
                "basaniya o harjegobhina",
                "basaniya'o harjegobhina",
                "basniya",
                "basniya mariyu hirjigovina",
                "bo si ni ya",
                "bo si ni ya he hei sai ge wei na",
                "bo si ni ya he hei shan gong he guo",
                "bosani'a ate harazegovina",
                "bosani'a ebam harjagobhina",
                "bosani'a o harjagobhina",
                "bosaniya harjigovina",
                "boseunia heleuchegobina",
                "boseuniaheleuchegobina",
                "bosnia da hertsegovina",
                "bosnia do hertsegovina",
                "bosniya ane harjhegovina",
                "bosniya ani harjegovina",
                "bosniya ani harjhagovhina",
                "bosniya aura harazegovina",
                "bosniya aura harzegovina",
                "bosniya mariyu herjegovina",
                "bosniya mattu harjegovina",
                "bosniya mattu herjegovina",
                "bosniya ra harjagobhina",
                "bosniya ra harjagobhiniya",
                "bosniya va harjagovina",
                "bsny w hrzgwyn",
                "bwsny hrzgwwyn",
                "bwsny w hrzgwyn",
                "bwsnyh whrzgwbynh",
                "bwsnyyە vە ھېrsېgwvyna",
                "bwsnʾ whrtsgwbynʾ",
                "i-Bosnia ne-Herzegovina",
                "pocuniya ercekovina",
                "posniya marrum hersikovina",
                "Βοσνία - Ερζεγοβίνη",
                "Βοσνία και Ερζεγοβίνη",
                "Босмудин болн Херцегудин Орн",
                "Босна",
                "Босна и Херцеговина",
                "Босна-Герцеговина",
                "Босни æмæ Герцеговинæ",
                "Босни а Герцеговина а",
                "Босни ба Герцеговина",
                "Босни ба Херцеговина",
                "Босни тата Герцеговина",
                "Босний да Герцеговина",
                "Босниэрэ Герцеговинэрэ",
                "Босния",
                "Босния бла Герцеговина",
                "Босния ва Ҳерсеговина",
                "Босния да Герцеговина",
                "Босния ди Херцеговина",
                "Босния және Герцеговина",
                "Босния и Герцеговина",
                "Босния но Герцеговина",
                "Босния уонна Херцеговина",
                "Босния һәм Герцеговина",
                "Боснія та Герцоговина",
                "Боснія і Герцагавіна",
                "Боснія і Герцеговина",
                "Боснія і Герцеґовина",
                "Боснія і Герцоговина",
                "Босьнія і Герцагавіна",
                "Բոսնիա և Հերցեգովինա",
                "Բոսնիա-Հերցեգովինա",
                "באסניע און הערצעגאווינע",
                "בוסניה והרצגובינה",
                "البوسنة و الهرسك",
                "البوسنة والهرسك",
                "البوسنه و الهرسك",
                "بسنی و هرزگوین",
                "بوسنىيە ۋە ھېرسېگوۋىنا",
                "بوسنی هرزگووین",
                "بوسنی و هرزگوین",
                "بوسنیا اور ہرزیگووینا",
                "بوسنیا تے ہرزیگووینا",
                "بوسنیا و ہرزیگووینا",
                "بۆسنیا و ھەرزەگۆڤینا",
                "ܒܘܣܢܐ ܘ ܗܪܣܟ",
                "ܒܘܣܢܐ ܘܗܪܬܣܓܘܒܝܢܐ",
                "ބޮސްނިޔާ އެންޑް ހެރްޒިގޮވީނާ",
                "बास्निया",
                "बॉस्निया आणि हर्झगोव्हिना",
                "बॉस्निया और हर्ज़ेगोविना",
                "बोसनिया हर्जिगोविना",
                "बोस्निया अणि हर्जेगोविना",
                "बोस्निया और हरज़ेगोविना",
                "बोस्निया र हर्जगोभिना",
                "बोस्निया र हर्जगोभिनिया",
                "बोस्निया व हर्जगोविना",
                "বসনিয়া ও হার্জেগোভিনা",
                "বসনিয়া বারো হার্জেগোভিনা",
                "বসনিয়াও হার্জেগোভিনা",
                "ਬੋਸਨੀਆ ਅਤੇ ਹਰਜ਼ੇਗੋਵੀਨਾ",
                "બોસ્નિયા અને હર્ઝેગોવિના",
                "ବୋସନିଆ ଏବଂ ହର୍ଜଗୋଭିନା",
                "ବୋସନିଆ ଓ ହର୍ଜଗୋଭିନା",
                "பொசுனியா எர்செகோவினா",
                "போஸ்னியா மற்றும் ஹெர்ஸிகோவினா",
                "బాస్నియా మరియు హీర్జిగోవినా",
                "బోస్నియా మరియు హెర్జెగొవీనా",
                "ಬೊಸ್ನಿಯ ಮತ್ತು ಹೆರ್ಜೆಗೊವಿನ",
                "ಬೋಸ್ನಿಯಾ ಮತ್ತು ಹರ್ಜೆಗೋವಿನಾ",
                "ബോസ്നിയ ഹെർസെഗോവിന",
                "ബോസ്നിയയും ഹെര്‍സഗോവിനയും",
                "බොස්නියා සහ හර්සගෝවිනා",
                "බොස්නියාව සහ හර්සගොවීනාව",
                "บอสเนียและเฮอร์เซโกวีนา",
                "ประเทศบอสเนียและเฮอร์เซโกวีนา",
                "ບັອດສເນຍ ແລະ ເຮີດໂກວິເນຍ",
                "ປະເທດບົດສະນີແຮກເຊໂກວີນ",
                "བོསྣི་ཡ་དང་ཧརྫོ་གོ་ཝི་ན།",
                "བྷོསུ་ནིཡ་དང་ཧར་ཛེ་གྷོ་ཝི་ན།",
                "ဘော့စနီးယား နှင့် ဟာဇီဂိုဘီးနား",
                "ဘော့စနီးယားနှင့် ဟာဇီဂိုဗီးနားနိုင်ငံ",
                "ბოსნია და ჰერცეგოვინა",
                "ბოსნია დო ჰერცეგოვინა",
                "ቦስኒያ እና ሄርዞጎቪኒያ",
                "ቦስኒያና ሄርጸጎቪና",
                "ᏆᏍᏂᏯ ᎠᎴ ᎲᏤᎪᏫᎾ",
                "បូស្ន៉ី",
                "ボスニア・ヘルツェゴビナ",
                "ボスニア・ヘルツェゴビナ共和国",
                "波斯尼亚",
                "波斯尼亚和黑塞哥维那",
                "波斯尼亚和黑山共和国",
                "波斯尼亞",
                "보스니아 헤르체고비나",
                "보스니아헤르체고비나"
            }, target[3].AlternateNames);
            #endregion

            //Timezone should NOT have underscores
            Assert.AreEqual("America/Argentina/Buenos Aires", target[4].Timezone);


            //Other misc checks
            Assert.AreEqual(new DateTime(2009, 6, 28), target[0].ModificationDate); //DateTime parsing
            Assert.AreEqual(3812366000, target[5].Population);    //Large (as in: > int.MaxValue) population
            Assert.AreEqual(8848, target[6].Elevation);     //Elevation
        }

        [TestMethod]
        public async Task FeatureCodeParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadFeatureCodes(@"testdata\test_featureCodes_en.txt").ToArrayAsync();
            Assert.AreEqual(3, target.Length);

            //A.ADM1 should result in a "A" class and "ADM1" code
            Assert.AreEqual("A", target[0].Class);
            Assert.AreEqual("ADM1", target[0].Code);

            Assert.AreEqual("first-order administrative division", target[0].Name);
            Assert.AreEqual("a primary administrative division of a country, such as a state in the United States", target[0].Description);

            ///When no dot in the featurecode is found, the class property should contain the entire string and code property should be null
            Assert.AreEqual("XXX", target[1].Class);
            Assert.IsNull(target[1].Code);

            //When the featurecode is "null" both the code and class property should be null
            Assert.IsNull(target[2].Class);
            Assert.IsNull(target[2].Code);
        }

        [TestMethod]
        public async Task GeoNameParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadGeoNames(@"testdata\test_geonames.txt").ToArrayAsync();
            Assert.AreEqual(2, target.Length);

            //Positive lat/long
            Assert.AreEqual(1136469, target[0].Id);
            Assert.AreEqual("Khōst", target[0].Name);
            Assert.AreEqual(33.33951, target[0].Latitude);
            Assert.AreEqual(69.92041, target[0].Longitude);

            //Negative lat/long
            Assert.AreEqual(3865840, target[1].Id);
            Assert.AreEqual("Añatuya", target[1].Name);
            Assert.AreEqual(-28.46064, target[1].Latitude);
            Assert.AreEqual(-62.83472, target[1].Longitude);
        }

        [TestMethod]
        public async Task HierarchyParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadHierarchy(@"testdata\test_hierarchy.txt").ToArrayAsync();
            Assert.AreEqual(4, target.Length);

            //"Normal" record
            Assert.AreEqual(6295630, target[0].ParentId);
            Assert.AreEqual(6255146, target[0].ChildId);
            Assert.AreEqual("ADM", target[0].Type);

            //Zero-child
            Assert.AreEqual(6255149, target[1].ParentId);
            Assert.AreEqual(0, target[1].ChildId);
            Assert.AreEqual("ADM", target[1].Type);

            //No-type
            Assert.AreEqual(672027, target[2].ParentId);
            Assert.AreEqual(663875, target[2].ChildId);
            Assert.AreEqual(string.Empty, target[2].Type);

            //-1 parent
            Assert.AreEqual(-1, target[3].ParentId);
            Assert.AreEqual(3623365, target[3].ChildId);
            Assert.AreEqual(string.Empty, target[3].Type);
        }

        [TestMethod]
        public async Task IsoLanguageCodeParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadISOLanguageCodes(@"testdata\test_iso-languagecodes.txt").ToArrayAsync();
            Assert.AreEqual(2, target.Length);  //First line in file should've been skipped


            Assert.AreEqual("alw", target[0].ISO_639_3);
            Assert.AreEqual(string.Empty, target[0].ISO_639_2);
            Assert.AreEqual(string.Empty, target[0].ISO_639_1);
            Assert.AreEqual("Alaba-K’abeena", target[0].LanguageName);

            Assert.AreEqual("zul", target[1].ISO_639_3);
            Assert.AreEqual("zul", target[1].ISO_639_2);
            Assert.AreEqual("zu", target[1].ISO_639_1);
            Assert.AreEqual("Zulu", target[1].LanguageName);
        }

        [TestMethod]
        public async Task TimeZonesParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadTimeZones(@"testdata\test_timeZones.txt").ToArrayAsync();
            Assert.AreEqual(5, target.Length);    //First line in file should've been skipped

            //Zero offsets
            Assert.AreEqual("MR", target[0].CountryCode);
            Assert.AreEqual("Africa/Nouakchott", target[0].TimeZoneId);
            Assert.AreEqual(0, target[0].GMTOffset);
            Assert.AreEqual(0, target[0].DSTOffset);
            Assert.AreEqual(0, target[0].RawOffset);

            //Negative offsets
            Assert.AreEqual("US", target[1].CountryCode);
            Assert.AreEqual("America/Adak", target[1].TimeZoneId);
            Assert.AreEqual(-10, target[1].GMTOffset);
            Assert.AreEqual(-9, target[1].DSTOffset);
            Assert.AreEqual(-10, target[1].RawOffset);

            //Float offsets
            Assert.AreEqual("AU", target[2].CountryCode);
            Assert.AreEqual("Australia/Darwin", target[2].TimeZoneId);
            Assert.AreEqual(9.5, target[2].GMTOffset);
            Assert.AreEqual(9.5, target[2].DSTOffset);
            Assert.AreEqual(9.5, target[2].RawOffset);

            Assert.AreEqual("AU", target[3].CountryCode);
            Assert.AreEqual("Australia/Eucla", target[3].TimeZoneId);
            Assert.AreEqual(8.75, target[3].GMTOffset);
            Assert.AreEqual(8.75, target[3].DSTOffset);
            Assert.AreEqual(8.75, target[3].RawOffset);

            //TimeZoneId should NOT have underscores
            Assert.AreEqual("Africa/Dar es Salaam", target[4].TimeZoneId);
        }

        [TestMethod]
        public async Task UserTagParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadUserTags(@"testdata\test_userTags.txt").ToArrayAsync();
            Assert.AreEqual(3, target.Length);

            //"Normal" tag
            Assert.AreEqual(2599253, target[0].GeoNameId);
            Assert.AreEqual("opengeodb", target[0].Tag);

            //Tags contain all sorts of randum stuff like URLs
            Assert.AreEqual(6255065, target[1].GeoNameId);
            Assert.AreEqual("http://de.wikipedia.org/wiki/Gotthardgebäude", target[1].Tag);

            Assert.AreEqual(6941058, target[2].GeoNameId);
            Assert.AreEqual("lyžařské", target[2].Tag);
        }

        [TestMethod]
        public async Task ContinentParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadContinents(@"testdata\test_continentCodes.txt").ToArrayAsync();
            Assert.AreEqual(1, target.Length);  //First line in file should've been skipped

            Assert.AreEqual("EU", target[0].Code);
            Assert.AreEqual("Europe", target[0].Name);
            Assert.AreEqual(6255148, target[0].GeoNameId);
        }

        [TestMethod]
        public async Task FeatureClassesParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadFeatureClasses(@"testdata\test_featureClasses_en.txt").ToArrayAsync();
            Assert.AreEqual(1, target.Length);  //First line in file should've been skipped

            Assert.AreEqual("X", target[0].Class);
            Assert.AreEqual("Test", target[0].Description);
        }

        [TestMethod]
        public async Task PostalCodeParser_ParsesFileCorrectly()
        {
            var target = await GeoFileReader.ReadPostalcodes(@"testdata\test_postalcodes.txt").ToArrayAsync();
            Assert.AreEqual(4, target.Length);  //First line in file should've been skipped

            Assert.AreEqual("AU", target[0].CountryCode);
            Assert.AreEqual("0200", target[0].PostalCode);
            Assert.AreEqual("Australian National University", target[0].PlaceName);
            Assert.IsTrue(double.IsNaN(target[0].Latitude));
            Assert.IsTrue(double.IsNaN(target[0].Longitude));
            Assert.IsNull(target[0].Accuracy);

            Assert.AreEqual("CZ", target[1].CountryCode);
            Assert.AreEqual("561 13", target[1].PostalCode);
            Assert.AreEqual("Orlické Podhůří-Rozsocha x)", target[1].PlaceName);
            Assert.AreEqual(50.0333, target[1].Latitude);
            Assert.AreEqual(16.2833, target[1].Longitude);
            Assert.IsNull(target[1].Accuracy);

            Assert.AreEqual("RU", target[2].CountryCode);
            Assert.AreEqual("216270", target[2].PostalCode);
            Assert.AreEqual("Пржевальское", target[2].PlaceName);
            Assert.AreEqual(55.5075, target[2].Latitude);
            Assert.AreEqual(31.85, target[2].Longitude);
            Assert.AreEqual(4, target[2].Accuracy);

            Assert.AreEqual("CH", target[3].CountryCode);
            Assert.AreEqual("2023", target[3].PostalCode);
            Assert.AreEqual("Gorgier", target[3].PlaceName);
            Assert.AreEqual("NE", target[3].AdminCode[0]);
            Assert.AreEqual("2401", target[3].AdminCode[1]);
            Assert.AreEqual("6410", target[3].AdminCode[2]);
            Assert.AreEqual("Canton de Neuchâtel", target[3].AdminName[0]);
            Assert.AreEqual("District de Boudry", target[3].AdminName[1]);
            Assert.AreEqual("Gorgier", target[3].AdminName[2]);
        }

        [TestMethod]
        public async Task CustomParser_IsUsedCorrectly()
        {
            var target = await GeoFileReader.ReadRecordsAsync(@"testdata\test_custom.txt", new CustomParser(19, 5, new[] { '☃' }, Encoding.UTF7, true)).ToArrayAsync();

            Assert.AreEqual(2, target.Length);
            CollectionAssert.AreEqual(target[0].Data, target[1].Data);
        }

        [TestMethod]
        public async Task FileReader_HandlesEmptyFilesCorrectly()
        {
            //Could use ANY entity
            var target1 = await GeoFileReader.ReadRecordsAsync(@"testdata\emptyfile.txt", new CustomParser(19, 5, new[] { '☃' }, Encoding.UTF7, true)).ToArrayAsync();

            Assert.AreEqual(0, target1.Length);

            //Here's a second one
            var target2 = await GeoFileReader.ReadExtendedGeoNames(@"testdata\emptyfile.txt").ToArrayAsync();

            Assert.AreEqual(0, target2.Length);
        }

        [TestMethod]
        public async Task FileReader_HandlesGZippedFilesCorrectly()
        {
            var target = await GeoFileReader.ReadCountryInfo(@"testdata\countryInfo_gzip_compat.txt.gz").ToArrayAsync();

            Assert.AreEqual(2, target.Length);
        }

        [TestMethod]
        public async Task FileReader_CanReadBuiltInContinentsCorrectly()
        {
            var target = (await GeoFileReader.ReadBuiltInContinents()).ToArray();

            Assert.AreEqual(7, target.Length);
            CollectionAssert.AreEqual(target.OrderBy(c => c.Code).Select(c => c.Code).ToArray(), new[] { "AF", "AN", "AS", "EU", "NA", "OC", "SA" });
        }

        [TestMethod]
        public async Task FileReader_CanReadBuiltInFeatureClassesCorrectly()
        {
            var target = (await GeoFileReader.ReadBuiltInFeatureClasses()).ToArray();

            Assert.AreEqual(9, target.Length);
            CollectionAssert.AreEqual(target.OrderBy(c => c.Class).Select(c => c.Class).ToArray(), new[] { "A", "H", "L", "P", "R", "S", "T", "U", "V" });
        }

        [TestMethod]
        public async Task FileReader_Admin1Codes_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_admin1CodesASCII.txt"))
                await GeoFileReader.ReadAdmin1Codes(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_Admin2Codes_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_admin2Codes.txt"))
                await GeoFileReader.ReadAdmin2Codes(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_AlternateNames_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_alternateNames.txt"))
                await GeoFileReader.ReadAlternateNames(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_AlternateNamesV2_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_alternateNamesV2.txt"))
                await GeoFileReader.ReadAlternateNamesV2(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_Continent_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_continentCodes.txt"))
                await GeoFileReader.ReadContinents(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_CountryInfo_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_CountryInfo.txt"))
                await GeoFileReader.ReadCountryInfo(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_ExtendedGeoNames_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_extendedgeonames.txt"))
                await GeoFileReader.ReadExtendedGeoNames(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_FeatureClasses_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_featureclasses_en.txt"))
                await GeoFileReader.ReadFeatureClasses(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_FeatureCodes_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_featurecodes_en.txt"))
                await GeoFileReader.ReadFeatureCodes(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_GeoNames_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_geonames.txt"))
                await GeoFileReader.ReadGeoNames(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_Hierarchy_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_hierarchy.txt"))
                await GeoFileReader.ReadHierarchy(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_ISOLanguageCode_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_iso-languagecodes.txt"))
                await GeoFileReader.ReadISOLanguageCodes(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_TimeZone_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_timeZones.txt"))
                await GeoFileReader.ReadTimeZones(s).CountAsync();
        }

        [TestMethod]
        public async Task FileReader_UserTags_StreamOverload()
        {
            using (var s = File.OpenRead(@"testdata\test_usertags.txt"))
                await GeoFileReader.ReadUserTags(s).CountAsync();
        }
    }
}
