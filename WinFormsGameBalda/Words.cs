using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsGameBalda
{
    class Words
    {
        public Words()
        {
        }

        /// <summary>
        /// Initializes the start word indexes for correctly work with next chars
        /// </summary>
        public void InitializeStartWord(string[][] Massive,ref int[][] check)
        {
            for (int i = 0; i < Massive.Length; i++)
                for (int j = 0; j < Massive[i].Length; j++)
                    if (Massive[i][j] != "")
                    {
                        AddCheck(i, j,ref check);
                    }
        }

        /// <summary>
        /// Load's the DataGridView's cells by start values
        /// </summary>
        public void InitializeMatrixProgramly(string[][] Massive,string[][] symbols)
        {
            Massive[0] = new string[5] { "", "", "", "", "" };
            Massive[1] = new string[5] { "", "", "", "", "" };
            Massive[2] = new string[5] { "A", "G", "E", "N", "T" };
            Massive[3] = new string[5] { "", "", "", "", "" };
            Massive[4] = new string[5] { "", "", "", "", "" };
            symbols[0] = new string[5] { "A", "F", "O", "L", "B" };
            symbols[1] = new string[5] { "S", "M", "N", "K", "C" };
        }

        /// <summary>
        /// Display Matrix on component DataGridView.
        /// </summary>
        /// <param name="Massive">Matrix.</param>
        /// <param name="dtgrdvw">DataGridView.</param>
        public void DisplayMatrix(string[][] Massive, DataGridView dtgrdvw)
        {
            try
            {
                int max_col = Massive.Length;
                for (int i = 0; i < Massive.Length; i++)
                    if (Massive[i].Length > max_col)
                        max_col = Massive[i].Length;
                dtgrdvw.ColumnCount = 0;
                dtgrdvw.ColumnCount = max_col;
                dtgrdvw.Rows.Add(Massive.Length);
                dtgrdvw.Visible = true;
                for (int i = 0; i < max_col; i++)
                {
                    dtgrdvw.AutoSize = false;
                    dtgrdvw.Columns[i].Width = 35;
                    //dtgrdvw.Rows[i].Height = 30;
                    dtgrdvw.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                for (int i = 0; i < Massive.Length; i++)
                    for (int j = 0; j < Massive[i].Length; j++)
                        dtgrdvw.Rows[i].Cells[j].Value = Massive[i][j].ToString();
            }
            catch { }
        }

        /// <summary>
        /// Adds the coordinats to word-vector
        /// </summary>
        /// <param name="i">Row</param>
        /// <param name="j">Column</param>
        public void AddCheck(int i, int j,ref int[][] check)
        {
            var m = check[check.Length - 2].Length;
            var n = check[check.Length - 1].Length;
            Array.Resize(ref check[check.Length - 2], m + 1);
            Array.Resize(ref check[check.Length - 1], n + 1);
            check[0][m] = i;
            check[1][n] = j;
        }

        /// <summary>
        /// Fixes if the selected cell is near main word
        /// </summary>
        /// <param name="i">Row index</param>
        /// <param name="m">Column index</param>
        /// <returns>true if everything is good</returns>
        public bool Fix(int i, int m,int[][] check)
        {
            if ((i + 1) == check[0][m] || (i - 1) == check[0][m])
                return true;
            else return false;
        }

    }
}
