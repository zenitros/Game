using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//OOP
//Async

namespace WinFormsGameBalda
{
    public partial class FormMain : Form
    {
        private static bool pr,player=false;
        private static int Score1=0, Score2=0;
        private string[][] Massive = new string[5][];
        private int[][] check = new int[2][];
        private string[] symbols = new string[5];
        public FormMain()
        {
            for (int i = 0; i < check.Length; i++)
            {
                check[i] = new int[0];
            }
            InitializeComponent();
            radTitleBar1.Text = "Molchanov, Teslya, Shevchenko";
            InitializeMatrixProgramly();
            DisplayMatrix(Massive, dtgridView);
            for (int i = 0; i < Massive.Length; i++)
                for (int j = 0; j < Massive[i].Length; j++)
                    if (Massive[i][j] != "")
                    {
                        AddCheck(i, j);
                    }
            // Display Symbol Matrix
            dtGrVwSymb.ColumnCount = symbols.Length;
            dtGrVwSymb.Rows[0].Height = 30;
            for (int i = 0; i < symbols.Length; i++)
            {
                dtGrVwSymb.Columns[i].Width = 35;
            }
            dtGrVwSymb.Rows.Add(1);
            for (int j = 0; j < symbols.Length; j++)
                dtGrVwSymb.Rows[0].Cells[j].Value = symbols[j].ToString();
        }
        private void InitializeMatrixProgramly()
        {
            Massive[0] = new string[5] { "", "", "", "", "" };
            Massive[1] = new string[5] { "", "", "", "", "" };
            Massive[2] = new string[5] { "A","G","E","N","T" };
            Massive[3] = new string[5] { "", "", "", "", "" };
            Massive[4] = new string[5] { "", "", "", "", "" };
            symbols = new string[5] { "A", "F", "O", "L", "B" };
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
                int max_col = Massive[0].Length;
                for (int i = 0; i < Massive.Length; i++)
                    if (Massive[i].Length > max_col)
                        max_col = Massive[i].Length;
                dtgrdvw.ColumnCount = 0;
                dtgrdvw.ColumnCount = max_col;
                dtgrdvw.Rows.Add(Massive.Length);
                dtgrdvw.Visible = true;
                for (int i = 0; i < max_col; i++)
                {
                    dtgridView.AutoSize = false;
                    dtgrdvw.Columns[i].Width = 35;
                    dtgridView.Rows[i].Height = 30;
                    dtgrdvw.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                for (int i = 0; i < Massive.Length; i++)
                    for (int j = 0; j < Massive[i].Length; j++)
                        dtgrdvw.Rows[i].Cells[j].Value = Massive[i][j].ToString();
            }
            catch { MessageBox.Show("Error"); }
        }
        
        private void dtGrVwSymb_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            dtGrVwSymb.DoDragDrop(dtGrVwSymb.Rows
            [e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),
            DragDropEffects.Copy);
        }

        private void dtgridView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.String)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void dtgridView_DragDrop(object sender, DragEventArgs e)
        {            
            int i, j;
            if (e.Data.GetDataPresent(typeof(System.String)))
            {
                bool pr = false;
                // get value
                var value = (System.String)e.Data.GetData(typeof(System.String));
                // get coordinat's of mouse
                Point clientPoint = dtgridView.PointToClient(new Point(e.X, e.Y));
                // get the RowIndex
                i = dtgridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
                // get the ColumnIndex
                j = dtgridView.HitTest(clientPoint.X, clientPoint.Y).ColumnIndex;
                if (dtgridView.Rows[i].Cells[j].Value.Equals(""))
                {
                    //check the rows
                    for (int m = 0; m < check[1].Length; m++)
                        if (j == check[1][m])
                            if (Fix(i, m))
                            {
                                // displays the new value
                                dtgridView.Rows[i].Cells[j].Value = value;
                                pr = true;
                                AddCheck(i, j);
                                break;
                            }
                    if (!pr)
                        MessageBox.Show("You must drag the symbol near main word!");
                    return;
                }
                else return;
            }
        }

        private void AddCheck(int i, int j)
        {
            var m = check[check.Length - 2].Length;
            var n = check[check.Length - 1].Length;
            Array.Resize(ref check[check.Length - 2], m + 1);
            Array.Resize(ref check[check.Length - 1], n + 1);
            check[0][m] = i;
            check[1][n] = j;
        }

        private bool Fix(int i, int m)
        {
            if ((i + 1) == check[0][m] || (i - 1) == check[0][m])
                return true;
            else return false;
        }

        private void btnEditPl1_Click(object sender, EventArgs e)
        {
            if (btnEditPl1.Text == "Edit info")
            {
                radTxtBxNm1.Enabled = true;
                btnEditPl1.Text = "Ok";
            }
            else { radTxtBxNm1.Enabled=false; btnEditPl1.Text = "Edit info"; }
        }

        private void btnEditPl2_Click(object sender, EventArgs e)
        {
            if (btnEditPl2.Text == "Edit info")
            {
                radTxtBxNm2.Enabled = true;
                btnEditPl2.Text = "Ok";
            }
            else { radTxtBxNm2.Enabled = false; btnEditPl2.Text = "Edit info"; }
        }

        private void Check_Click(object sender, EventArgs e)
        {
            pr = false;
            var sb = new StringBuilder();
            var selectedCellCount =
            dtgridView.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 2)
            {
                if (dtgridView.AreAllCellsSelected(true))
                {
                    MessageBox.Show("Select some cells,but not all of them!", "Selected Cells");
                }
                else
                {
                    for (int i = selectedCellCount - 1;
                        i >= 0; --i)
                    {
                        var a = dtgridView.SelectedCells[i].RowIndex;
                        var b = dtgridView.SelectedCells[i].ColumnIndex;
                        sb.Append(dtgridView.Rows[a].Cells[b].Value);
                    }
                    MessageBox.Show(sb.ToString(), "Your string");
                    btnSubmit.Enabled = true;
                    ReadFromFile(sb);
                }
            }
        }

        /// <summary>
        /// Reads from file and checking the word on existing in vocabualary
        /// </summary>
        /// <param name="sb">word</param>
        private void ReadFromFile(StringBuilder sb)
        {
            using (var reader = new StreamReader("Voc.txt"))
            {
                var str = "";
                for (int i = 0; (str = reader.ReadLine()) != null; i++)
                    if (str == sb.ToString())
                    {
                        AddScore();
                        pr = true;
                    }
            }
            if (!pr) MessageBox.Show("This word, does not exist!", "Warning");
        }

        private void AddScore()
        {
            if(!player) Score1 +=100;
            else Score2+=100;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            btnSubmit.Enabled = false;
            radTxtBxScorePl1.Text = Score1.ToString();
            radTxtBxScorePl2.Text = Score2.ToString();
            if (player)
            {
                MessageBox.Show("Player1 playes now!");
                player = false;
            }
            else
            {
                MessageBox.Show("Player2 playes now!");
                player = true;
            }
        }
    }
}
