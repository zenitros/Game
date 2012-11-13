using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsGameBalda
{
    public partial class Form1 : Form
    {
        private string[][] Massive = new string[5][];
        private int[][] check = new int[2][];
        private string[] symbols = new string[5];
        public Form1()
        {
            for (int i = 0; i < check.Length; i++)
            {
                check[i] = new int[0];
            }
            InitializeComponent();
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
            Massive[2] = new string[5] { "S","E","R","G","E" };
            Massive[3] = new string[5] { "", "", "", "", "" };
            Massive[4] = new string[5] { "", "", "", "", "" };
            symbols = new string[5] { "A", "B", "C", "D", "E" };
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
                    //dtgrdvw.Columns[i].Name = i.ToString();
                    dtgridView.AutoSize = false;
                    dtgrdvw.Columns[i].Width = 35;
                    dtgridView.Rows[i].Height = 30;
                    dtgrdvw.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                for (int i = 0; i < Massive.Length; i++)
                    for (int j = 0; j < Massive[i].Length; j++)
                        dtgrdvw.Rows[i].Cells[j].Value = Massive[i][j].ToString();
            }
            catch { MessageBox.Show("error"); }
        }
        private void UpdateFont()
        {
            //Change cell font
            foreach (DataGridViewColumn c in dtgridView.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 10F, GraphicsUnit.Pixel);
            }
        }
        //from what will be copy
        private void dtGrVwSymb_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            dtGrVwSymb.DoDragDrop(dtGrVwSymb.Rows
            [e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),
            DragDropEffects.Copy);

            // refresh the controll
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
                                MessageBox.Show("Drag'n'drop was successfully end");
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

        private void MnItmExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
