using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WinFormsGameBalda
{
    class Files
    {
        private FormMain obj = new FormMain();

        public Files()
        {
        }

        /// <summary>
        /// Adds record to .txt file
        /// </summary>
        /// <param name="score">number of points</param>
        public void AddMaxScore(int score)
        {
            using (var stream = new FileStream("Records.txt", FileMode.OpenOrCreate))
            { }
            using (var writer = File.AppendText("Records.txt"))
            {
                writer.WriteLine(score);
            }
        }

        /// <summary>
        /// Determine if selected word was used
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool AddToFile(string word)
        {
            var str = "";
            using (var reader = new StreamReader("Usedwords.txt"))
            {
                while ((str = reader.ReadLine()) != null)
                    if (str.Equals(word))
                        return true;
            }
            using (var writer = File.AppendText("Usedwords.txt"))
            {
                writer.WriteLine(word.ToString());
                return false;
            }
        }

        /// <summary>
        /// Reads from file and checking the word on existing in vocabualary
        /// </summary>
        /// <param name="sb">word</param>
        public bool ReadFromFile()
        {
            using (var reader = new StreamReader("Voc.txt"))
            {
                var str = "";
                for (int i = 0; (str = reader.ReadLine()) != null; i++)
                    if (str == obj.StrBuild.ToString())
                    {
                        return true;
                    }
            }
            MessageBox.Show("This word, does not exist!", "Warning");
            return false;
        }

        /// <summary>
        /// Increase's the Players Score for the correct selected word
        /// </summary>
        public void AddScore(bool pr)
        {
            if (!pr) obj.ScorePL1 += 100 * obj.StrBuild.Length;
            else  obj.ScorePL2 += 100 * obj.StrBuild.Length;
        }

        /// <summary>
        /// Creates the new file for writing used words
        /// </summary>
        public void CreateTemporaryFile()
        {
            using (var stream = new FileStream("Usedwords.txt", FileMode.Create))
            { }
        }
    }
}
