using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Trivia;

namespace TriviaTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Main_Should()
        {
            int seed = 1;
            RunMainWithSeed(seed);
        }

        [TestMethod] 
        public void Main_Should_HaveSameOutputWith10Seeds()
        {
            for (int i = 0; i < 1000; i++)
            {
                RunMainWithSeed(i);
            }
        }

        private static void RunMainWithSeed(int seed)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            Console.SetOut(sw);
            GameRunner.Main(new string[] { $"{seed}" });

            string output = sb.ToString();

            Assert.IsNotNull(output);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"output-{seed}.txt");
            Trace.WriteLine("The path: " + path);
            
            if (File.Exists(path))
            {
                string expectedResult = File.ReadAllText(path);
                Assert.IsTrue(String.Compare(expectedResult, output) == 0);
            }
            else
            {
                File.WriteAllText(path, output);
            }
        }
    }
}