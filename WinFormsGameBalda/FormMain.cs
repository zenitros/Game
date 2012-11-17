using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WinFormsGameBalda
{
    public partial class FormMain : Form
    {
        private static Files objFiles = new Files();
        private Words objWords = new Words();
        private static StringBuilder sb;
        private bool player;
        private static int Score1 = 0, Score2 = 0;
        private string[][] Massive = new string[5][];
        private int[][] check = new int[2][];
        private string[][] symbols = new string[2][];

        public StringBuilder StrBuild
        {
            get { return sb; }
        }
        public bool indicationPl
        {
            get { return player; }
        }
        public int ScorePL1
        {
            get { return Score1; }
            set { Score1 = value; }
        }
        public int ScorePL2
        {
            get { return Score2; }
            set { Score2 = value; }
        }

        public FormMain()
        {
        }

        public FormMain(bool access)
        {
            for (int i = 0; i < check.Length; i++)
            {
                check[i] = new int[0];
            }
            InitializeComponent();
            radTitleBar1.Text = "Molchanov, Teslya, Shevchenko";
            objWords.InitializeMatrixProgramly(Massive,symbols);
            objWords.DisplayMatrix(Massive, dtgridView);
            objWords.InitializeStartWord(Massive,ref check);
            objWords.DisplayMatrix(symbols, dtGrVwSymb);
            objFiles.CreateTemporaryFile();
        }
        
        private void dtGrVwSymb_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                dtGrVwSymb.DoDragDrop(dtGrVwSymb.Rows
                [e.RowIndex].Cells[e.ColumnIndex].Value.ToString(),
                DragDropEffects.Copy);
            }
            catch { }
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
                            if (objWords.Fix(i, m,check))
                            {
                                // displays the new value
                                dtgridView.Rows[i].Cells[j].Value = value;
                                pr = true;
                                objWords.AddCheck(i, j,ref check);
                                if (check.Length == 25)
                                {
                                    if (Score1 > Score2)
                                    {
                                        MessageBox.Show("Player1 Win!", "Game over!");
                                        objFiles.AddMaxScore(Score1);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Player2 Win!", "Game over!");
                                        objFiles.AddMaxScore(Score2);
                                    }
                                    Application.Exit();
                                }
                                dtGrVwSymb.Enabled = false;
                                break;
                            }
                    if (!pr)
                        MessageBox.Show("You must drag the symbol near main word!");
                }
                return;
            }
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            bcgWorker.RunWorkerAsync();
        }

        private void bcgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sb = new StringBuilder();
                sb.Clear();
                var selectedCellCount =
                dtgridView.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 2)
                {
                    if (dtgridView.AreAllCellsSelected(true))
                    {
                        MessageBox.Show("Select some cells, but not all of them!", "Selected Cells");
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
                        if (objFiles.ReadFromFile())
                        {
                            if (objFiles.AddToFile(sb.ToString()))
                            { sb.Clear(); throw new ArgumentException("This word is alredy used!"); }
                            objFiles.AddScore(player);
                        }
                    }
                }
                else { MessageBox.Show("You should select char's more than 2!"); }

                dtGrVwSymb.Enabled = true;
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
            catch (ArgumentException ex) { MessageBox.Show(ex.Message); }
            catch { MessageBox.Show("Fatal error!", "Check the word"); }
        }
    }
}
