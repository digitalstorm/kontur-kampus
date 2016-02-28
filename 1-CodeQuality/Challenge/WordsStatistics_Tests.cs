using System;
using System.Linq;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
    public class WordsStatistics_Tests
    {
        public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation();
            // меняется на разные реализации при запуске exe

        public IWordsStatistics stat;

        [SetUp]
        public void SetUp()
        {
            stat = createStat();
        }

        [Test]
        public void no_stats_if_no_words()
        {
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }

        [Test]
        public void same_word_twice()
        {
            stat.AddWord("xxx");
            stat.AddWord("xxx");
            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "xxx")}, stat.GetStatistics());
        }

        [Test]
        public void single_word()
        {
            stat.AddWord("hello");
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, "hello")}, stat.GetStatistics());
        }

        [Test]
        public void two_same_words_one_other()
        {
            stat.AddWord("hello");
            stat.AddWord("world");
            stat.AddWord("world");
            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "world"), Tuple.Create(1, "hello")}, stat.GetStatistics());
        }

        [Test]
        public void upper_case_word()
        {
            stat.AddWord("HELLO");
            CollectionAssert.AreEqual(new[] {Tuple.Create(1, "hello")}, stat.GetStatistics());
        }

        [Test]
        public void upper_case_and_same_lower_case()
        {
            stat.AddWord("HELLO");
            stat.AddWord("hello");
            CollectionAssert.AreEqual(new[] {Tuple.Create(2, "hello")}, stat.GetStatistics());
        }

        [Test]
        public void test_lexicographical_order_when_frequency_coincides()
        {
            stat.AddWord("world");
            stat.AddWord("hello");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello"), Tuple.Create(1, "world") }, stat.GetStatistics());
        }

        [Test]
        public void test_lexicographical_order_with_numbers()
        {
            stat.AddWord("2");
            stat.AddWord("11");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "11"), Tuple.Create(1, "2") }, stat.GetStatistics());
        }

        [Test]
        public void test_numbers_before_letters()
        {
            stat.AddWord("a");
            stat.AddWord("1");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "1"), Tuple.Create(1, "a") }, stat.GetStatistics());
        }

        [Test]
        public void test_empty_word_ignores()
        {
            stat.AddWord("");
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }

        [Test]
        public void test_null_word_ignores()
        {
            stat.AddWord(null);
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }

        [Test]
        public void test_long_words_cut()
        {
            stat.AddWord("a1234567891");
            stat.AddWord("a123456789");
            stat.AddWord("b123456789");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "a123456789"), Tuple.Create(1, "b123456789") }, stat.GetStatistics());
        }

        [Test]
        public void test_can_add_after_get_statistics()
        {
            CollectionAssert.IsEmpty(stat.GetStatistics());
            stat.AddWord("hello");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
        }

        [Test]
        public void test_xxx()
        {
            for (int i = 0; i < 1000; i++)
                stat.AddWord(i.ToString());
            Assert.AreEqual(1000, stat.GetStatistics().Count());
        }

        [Test]
        [Timeout(100)]
        public void test_add_is_fast()
        {
            for (int i = 0; i < 1000; i++)
                stat.AddWord(i.ToString());
        }

        [Test]
        [Timeout(50)]
        public void test_add_is_very_fast()
        {
            for (int i = 0; i < 1000; i++)
                stat.AddWord(i.ToString());
        }

        [Test]
        public void test_for_qwe()
        {
            var letters = "йцукенгшщзфывапролдячсмитьёхъжэбюqwertyuiopasdfghjklzxcvbnm";
            for (int i = 0; i < letters.Length; i++)
            {
                var word = letters[i].ToString();
                stat.AddWord(word);
                var upperWord = word.ToUpper();
                stat.AddWord(upperWord);
                Assert.AreEqual(2, stat.GetStatistics().First(t => t.Item2 == word).Item1);
            }
        }

        [Test]
        public void test_for_sta()
        {
            var anotherStat = createStat();
            anotherStat.AddWord("xxx");

            stat.AddWord("xxx");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "xxx") }, stat.GetStatistics());
        }
    }
}