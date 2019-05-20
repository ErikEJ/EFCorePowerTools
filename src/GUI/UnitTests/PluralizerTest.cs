using NUnit.Framework;
using Pluralize.NET;

namespace UnitTests
{
    [TestFixture]
    public class PluralizerTest
    {
        [Test]
        [TestCase("bison")]
        [TestCase("Djinn", Ignore = "Broken, by design?")]
        [TestCase("tobacco", Ignore = "Broken, by design?")]
        public void CanPluralizeUninflectedWord(string word)
        {
            // Act
            var result = new Pluralizer().Pluralize(word);

            // Assert
            Assert.That(result, Is.EqualTo(word));
        }

        [Test]
        [TestCase("bison")]
        [TestCase("Djinn")]
        [TestCase("tobacco")]
        public void CanSingularizeUninflectedWord(string word)
        {
            // Act
            var result = new Pluralizer().Singularize(word);

            // Assert
            Assert.That(result, Is.EqualTo(word));
        }

        [Test]
        [TestCase("am", "are", Ignore = "Broken, issue created")]
        [TestCase("is", "are")]
        [TestCase("was", "were")]
        [TestCase("has", "have")]
        public void CanPluralizeIrregularVerb(string word, string expectedResult)
        {
            var result = new Pluralizer().Pluralize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("are", "is")]
        [TestCase("were", "was")]
        [TestCase("have", "has")]
        public void CanSingularizeIrregularVerb(string word, string expectedResult)
        {
            var result = new Pluralizer().Singularize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase("child", "children")]
        [TestCase("corpus", "corpuses")]
        [TestCase("pie", "pies")]
        public void CanPluralizeIrregularWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Pluralize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("children", "child")]
        [TestCase("corpora", "corpora")]
        [TestCase("pies", "pie")]
        public void CanSingularizeIrregularWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Singularize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("alumna", "alumnae")]
        [TestCase("forum", "forums")]
        [TestCase("bacterium", "bacteria")]
        public void CanPluralizeAssimilatedClassicalInflectionWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Pluralize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("alumnae", "alumna")]
        [TestCase("forums", "forum")]
        [TestCase("bacteria", "bacterium")]
        public void CanSingularizeAssimilatedClassicalInflectionWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Singularize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("enigma", "enigmas")]
        [TestCase("drama", "dramas")]
        [TestCase("alto", "altos")]
        public void CanPluralizeClassicalInflectionWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Pluralize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        //TODO Investigate

        //[Test]
        //[TestCase("enigmata", "enigmata")]
        //[TestCase("dramata", "dramata")]
        //[TestCase("altos", "alto")]
        //public void CanSingularizeClassicalInflectionWord(string word, string expectedResult)
        //{
        //    var result = new Pluralizer().Singularize(word);

        //    Assert.That(result, Is.EqualTo(expectedResult));
        //}


        [Test]
        [TestCase("albino", "albinos")]
        [TestCase("guano", "guanos")]
        [TestCase("mango", "mangos")]
        public void CanPluralizeOSuffixWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Pluralize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("albinos", "albino")]
        [TestCase("guanos", "guano")]
        [TestCase("mangoes", "mango")]
        public void CanSingularizeOSuffixWord(string word, string expectedResult)
        {
            var result = new Pluralizer().Singularize(word);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("horseman", "horsemen")]
        [TestCase("woman", "women")]
        [TestCase("Deliveryman", "Deliverymen")]

        public void TestPluralRule_Suffix_Man(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("Mouse", "Mice")]
        [TestCase("house", "houses")]
        [TestCase("Louse", "Lice")]
        public void TestPluralRule_Suffix_Ouse(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("Tooth", "Teeth")]
        [TestCase("house", "houses")]
        [TestCase("bluetooth", "bluetooths")]
        public void TestPluralRule_Suffix_Tooth(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("goose", "geese")]
        [TestCase("mongoose", "mongooses")]
        public void TestPluralRule_Suffix_Goose(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("foot", "feet")]
        [TestCase("webfoot", "webfeet", Ignore = "Broken, but asking too much")]
        [TestCase("Bigfoot", "Bigfoots")]
        public void TestPluralRule_Suffix_Foot(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("protozoon", "protozoa", Ignore = "Broken, issue created")]
        [TestCase("spermatozoan", "spermatozoa", Ignore = "Broken, issue created")]
        [TestCase("hydrozoan", "hydrozoa", Ignore = "Broken, issue created")]
        public void TestPluralRule_Suffix_Zoon_Zoan(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo($"{singular.Substring(0, singular.Length - 4)}zoon"));
        }

        [Test]
        [TestCase("Proboscis", "Probosces", IgnoreReason = "Will always fail by design.")]
        [TestCase("parabiosis", "parabioses", IgnoreReason = "Will always fail by design.")]
        public void TestPluralRule_Suffix_Cis_Sis_Xis(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("brother", "brothers")]
        [TestCase("game", "games")]
        [TestCase("status", "statuses")]
        [TestCase("Status", "Statuses")]
        [TestCase("Cow", "Cows")]
        [TestCase("Axe", "Axes", IgnoreReason = "Will always fail by design.")]
        public void TestInflection_Random(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("matrix", "matrices")]
        public void TestPluralRule_Suffix_Trix(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("bureau", "bureaus")]
        [TestCase("adieu", "adieus", Ignore = "Investigate")]
        public void TestPluralRule_Suffix_Eau_Ieu(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("jinx", "jinxes")]
        [TestCase("sphinx", "sphinxes")]
        [TestCase("phalanx", "phalanxes")]
        [TestCase("larynx", "larynxes")]
        public void TestPluralRule_Suffix_Inx_Anx_Ynx(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("Status", "Statuses")]
        [TestCase("Dish", "Dishes")]
        [TestCase("miss", "misses")]
        [TestCase("birch", "birches")]
        public void TestPluralRule_Suffix_Ch_Sh_Ss_Us(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("dwarf", "dwarves")]
        [TestCase("wolf", "wolves")]
        [TestCase("calf", "calves")]
        [TestCase("shelf", "shelves")]
        [TestCase("leaf", "leaves")]
        public void TestPluralRule_Suffix_Alf_Elf_Olf_Eaf_Arf(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("Day", "Days")]
        [TestCase("key", "keys")]
        [TestCase("Boy", "Boys")]
        [TestCase("Guy", "Guys")]
        public void TestPluralRule_Suffix_Vowel_Y(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("Baby", "Babies")]
        public void TestPluralRule_Suffix_Y(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("video", "videos")]
        [TestCase("kangaroo", "kangaroos")]
        [TestCase("portfolio", "portfolios")]
        [TestCase("duo", "duos")]
        public void TestPluralRule_Suffix_Vowel_O(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("hero", "heroes")]
        [TestCase("potato", "potatoes")]
        public void TestPluralRule_Suffix_O(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("box", "boxes")]
        public void TestPluralRule_Suffix_X(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("Course Status", "Course Statuses")]
        [TestCase("limited Offer", "limited Offers")]
        [TestCase("Fun class", "Fun classes")]
        public void CanPluralizeWordsSeparatedBySpace(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("Course AverageStatus", "Course AverageStatuses")]
        [TestCase("limited GreatOffer", "limited GreatOffers")]
        [TestCase("Fun smallClass", "Fun smallClasses")]
        public void CanPluralizeWordWithSpaceAndUpperCase(string singular, string plural)
        {
            var pluralResult = new Pluralizer().Pluralize(singular);

            var singularResult = new Pluralizer().Singularize(plural);

            Assert.That(pluralResult, Is.EqualTo(plural));
            Assert.That(singularResult, Is.EqualTo(singular));
        }

        [Test]
        [TestCase("AxResponses", "AxResponse", Description = "Ending on -se")]
        [TestCase("CourseStatuses", "CourseStatus", Description = "Ending on -s")]
        [TestCase("HugeOxen", "HugeOx", Description = "Irregular", Ignore = "Broken")]
        public void CanSingularizePascalCaseCompoundWords(string word, string expectedResult)
        {
            // Act
            var result = new Pluralizer().Singularize(word);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("AxResponse", "AxResponses", Description = "Ending on -se")]
        [TestCase("CourseStatus", "CourseStatuses", Description = "Ending on -s")]
        [TestCase("HugeOx", "HugeOxen", Description = "Irregular", Ignore = "Broken, issue created")]
        public void CanPluralizePascalCaseCompoundWords(string word, string expectedResult)
        {
            // Act
            var result = new Pluralizer().Pluralize(word);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("Oranges", "Orange")]
        [TestCase("Exchanges", "Exchange")]
        public void Issue221(string word, string expectedResult)
        {
            // Act
            var result = new Pluralizer().Singularize(word);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
