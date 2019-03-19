using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Global_LocalAlignment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> sequence, query;
        string[,] bigArray;
        Random rnd = new Random();
        bool isGlobal = true, isAuto = true;
        private int indel, mismatch, match = 0, totalValue = 0, seqLen, queryLen, maxValue = 0, maxRow = 0, maxCol = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.PerformClick();
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length == 0 && textBox6.Text.Length < 0)
            {
                textBox6.Text = "";
            }
            else
            {
                label15.Text = textBox6.Text.Length.ToString();
            }

        }        

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text.Length == 0 && textBox7.Text.Length < 0)
            {
                textBox7.Text = "";
            }
            else
            {
                label16.Text = textBox7.Text.Length.ToString();
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = true;
            groupBox3.Enabled = false;
            isAuto = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            groupBox3.Enabled = true;
            isAuto = false;
        }

        /// <summary>
        ///  20 tane Sequence ve 10 tanede query için random string oluşturur.
        /// </summary>       
        private void btnGenerateRandomNucleatideArray_Click(object sender, EventArgs e)
        {
            if (isAuto)
            {
                seqLen = rnd.Next(15, 21);
                queryLen = rnd.Next(5, 11);
                bigArray = new string[seqLen, queryLen];

                textBox1.Clear();
                textBox2.Clear();

                sequence = new List<string>();
                query = new List<string>();
                for (int i = 0; i < seqLen; i++)
                {
                    switch (rnd.Next(1, 5))
                    {
                        case 1:
                            { sequence.Add("A"); }
                            break;
                        case 2:
                            { sequence.Add("T"); }
                            break;
                        case 3:
                            { sequence.Add("G"); }
                            break;
                        case 4:
                            { sequence.Add("C"); }
                            break;
                    }

                }
                for (int i = 0; i < queryLen; i++)
                {
                    switch (rnd.Next(1, 5))
                    {
                        case 1:
                            { query.Add("A"); }
                            break;
                        case 2:
                            { query.Add("T"); }
                            break;
                        case 3:
                            { query.Add("G"); }
                            break;
                        case 4:
                            { query.Add("C"); }
                            break;
                    }

                }

                foreach (var item in sequence)
                {
                    textBox1.Text += item;
                }
                foreach (var item in query)
                {
                    textBox2.Text += item;
                }
                label11.Text = textBox1.Text.Length.ToString();
                label12.Text = textBox2.Text.Length.ToString();
            }

            if (!isAuto)
            {
                seqLen = textBox6.Text.Length;
                queryLen = textBox7.Text.Length;
                bigArray = new string[seqLen, queryLen];

                sequence = new List<string>();
                query = new List<string>();

                foreach (var value in textBox6.Text)
                {
                    string item = value.ToString();


                    //Alt kısımlar ATGC kontrolünü sağlıyor. Bunlar olmazsa program her şeyi karşılaştırır. Bunlar varken sadece ATGCleri kontrol eder.

                    //if (item.ToUpper() != "A" && item.ToUpper() != "T" && item.ToUpper() != "G" && item.ToUpper() != "C")
                    //{
                    //    MessageBox.Show("Lütfen (A , T , G , C) karakterlerinden bir dataset giriniz.");
                    //    textBox6.Clear();
                    //    break;
                    //}
                    //else
                    //{
                        sequence.Add(item.ToString());
                    //}
                }
                foreach (var value in textBox7.Text)
                {
                    string item = value.ToString();
                    //if (item.ToUpper() != "A" && item.ToUpper() != "T" && item.ToUpper() != "G" && item.ToUpper() != "C")
                    //{
                    //    MessageBox.Show("Lütfen (A , T , G , C) karakterlerinden bir dataset giriniz.");
                    //    textBox7.Clear();
                    //    break;
                    //}
                    //else
                    //{
                        query.Add(item.ToString());
                    //}
                }
            }

        }
        /// <summary>
        ///  Bu fonksiyon Üretilen rastegele Sequence ve Query değerlerini ilk satır ve sütuna yazıyor.
        /// </summary>       
        private void btnFillGap_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("a", "b"); dataGridView1.Columns.Add("a", "b");
            dataGridView1.Refresh();
            totalValue = 0;

            if (txtGap.Text == "" || txtMM.Text == "" || txtMatch.Text == "")
            {
                MessageBox.Show("Please fill the Gap,Mismatch and Match values with positive or negative INTEGER values.");
                txtGap.Clear();
                txtMM.Clear();
                txtMatch.Clear();
            }
            else
            {
                for (int i = 0; i < seqLen; i++)
                {
                    dataGridView1.Columns.Add(i.ToString(), i.ToString());
                }
                DataGridViewRow row = new DataGridViewRow();
                for (int i = 0; i <= queryLen; i++)
                {
                    row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    dataGridView1.Rows.Add(row);
                }
                dataGridView1.Rows[1].Cells[0].Value = "-";
                for (int i = 0; i < queryLen; i++)
                {
                    dataGridView1.Rows[i + 2].Cells[0].Value = query[i].ToString();
                }

                dataGridView1.Rows[0].Cells[0].Value = "X";
                dataGridView1.Rows[0].Cells[1].Value = "-";
                int columnValue = 2;
                foreach (var value in sequence)
                {
                    dataGridView1.Rows[0].Cells[columnValue].Value = value;
                    columnValue++;
                }
                int height = bigArray.GetLength(0); // burası bana kaç satır olacağını söylüyor.      

                indel = Convert.ToInt32(txtGap.Text);
                mismatch = Convert.ToInt32(txtMM.Text);
                match = Convert.ToInt32(txtMatch.Text);

                for (int i = 0; i < query.Count; i++)
                {
                    bigArray[0, i] = sequence[i].ToString();
                }
                for (int i = 0; i < query.Count; i++)
                {
                    bigArray[i, 0] = query[i].ToString();
                }

                if (isGlobal) // Eğer Global Alignment seçiliyse
                {
                    dataGridView1[1, 1].Value = 0;

                    for (int i = 2; i < seqLen + 2; i++) // bu satır satır yazdırmak için.
                    {
                        totalValue += indel;
                        dataGridView1[i, 1].Value = totalValue;
                    }
                    totalValue = 0;
                    for (int i = 2; i < queryLen + 2; i++) // bu satır satır yazdırmak için.
                    {
                        totalValue += indel;
                        dataGridView1[1, i].Value = totalValue;
                    }
                }
                else // Eğer Local Alignment seçiliyse
                {
                    dataGridView1[1, 1].Value = 0;

                    for (int i = 2; i < seqLen + 2; i++) // bu satır satır yazdırmak için.
                    {
                        totalValue += indel;
                        dataGridView1[i, 1].Value = 0;
                    }
                    totalValue = 0;
                    for (int i = 2; i < queryLen + 2; i++) // bu satır satır yazdırmak için.
                    {
                        totalValue += indel;
                        dataGridView1[1, i].Value = 0;
                    }
                }
            }

        }
        /// <summary>
        /// Bu butonun işlevinde asıl olay diagonal, yatay ve dikey yönde bakarak 3 tane değeri ve MATCH yada MISMATCH değeri findMax fonksiyonuna yolluyor.
        /// Devamında da fonksiyondan gelen değeri gerekli hücreye atıyor.
        /// </summary>        
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (isGlobal)
            {
                for (int i = 2; i < seqLen + 2; i++) // i = sütun   j = satır.
                {
                    for (int j = 2; j < queryLen + 2; j++)
                    {
                        if (dataGridView1[i, 0].Value.ToString() == dataGridView1[0, j].Value.ToString())
                        {
                            dataGridView1[i, j].Value = findMax(Convert.ToInt32(dataGridView1[i - 1, j - 1].Value), Convert.ToInt32(dataGridView1[i - 1, j].Value), Convert.ToInt32(dataGridView1[i, j - 1].Value), true);
                        }
                        else
                        {
                            dataGridView1[i, j].Value = findMax(Convert.ToInt32(dataGridView1[i - 1, j - 1].Value), Convert.ToInt32(dataGridView1[i - 1, j].Value), Convert.ToInt32(dataGridView1[i, j - 1].Value), false);
                        }
                    }
                }
                findMaxOfMatrix();
            }
            if (!isGlobal)
            {
                for (int i = 2; i < seqLen + 2; i++)
                {
                    for (int j = 2; j < queryLen + 2; j++)
                    {
                        if (dataGridView1[i, 0].Value.ToString() == dataGridView1[0, j].Value.ToString())
                        {
                            dataGridView1[i, j].Value = findMax(Convert.ToInt32(dataGridView1[i - 1, j - 1].Value), Convert.ToInt32(dataGridView1[i - 1, j].Value), Convert.ToInt32(dataGridView1[i, j - 1].Value), true);
                            if (Convert.ToInt32(dataGridView1[i, j].Value) < 0)
                            {
                                dataGridView1[i, j].Value = 0;
                            }
                        }
                        else
                        {
                            dataGridView1[i, j].Value = findMax(Convert.ToInt32(dataGridView1[i - 1, j - 1].Value), Convert.ToInt32(dataGridView1[i - 1, j].Value), Convert.ToInt32(dataGridView1[i, j - 1].Value), false);
                            if (Convert.ToInt32(dataGridView1[i, j].Value) < 0)
                            {
                                dataGridView1[i, j].Value = 0;
                            }
                        }
                    }
                }
                findMaxOfMatrix();
            }
        }
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("a", "b"); dataGridView1.Columns.Add("a", "b");
            dataGridView1.Refresh();
            if (sequence == null)
            {
                //do Nothing
            }
            else
            {
                sequence.Clear();
            }
            if (query == null)
            {
                //Do Nothing again.
            }
            else
            {
                query.Clear();
            }

            Array.Clear(bigArray, 0, bigArray.Length);
            indel = 0;
            mismatch = 0;
            match = 0;
            totalValue = 0;
            textBox1.Clear();
            textBox2.Clear();
            txtGap.Clear();
            txtMM.Clear();
            txtMatch.Clear();
        }
        /// <summary>
        ///  Bu fonksiyon dışarında 3 tane int ve 1 tanede durum için bool değer alıyorum.
        ///  Devamında gelen 3 integer değere Durum değişkenine göre Match yada Mismatch ve ayrıca GAP değerlerini ekliyorum.
        ///  Ve return değer olarak tablonun hücresine yazıyorum.
        /// </summary>
        /// <param name="v1"></param> Bu benim Diagonaldeki değerim
        /// <param name="v2"></param> Bu benim dikeydeki değerim
        /// <param name="v3"></param> Bu benim yataydaki değerim
        /// <param name="isMatch"></param> Buda benim A-T gibi mismatch'mi yoksa G-G gibi match'mi olduğunu anlamamı sağlayan değişken.
        /// <returns></returns>
        private int findMax(int v1, int v2, int v3, bool isMatch)
        {
            if (isMatch)
            {
                v1 = v1 + match;
                v2 = v2 + indel;
                v3 = v3 + indel;
            }
            if (!isMatch)
            {
                v1 = v1 + mismatch;
                v2 = v2 + indel;
                v3 = v3 + indel;
            }

            if (v1 >= v2)
            {
                if (v1 >= v3)
                {
                    return v1;
                }
                else
                {
                    return v3;
                }
            }
            else if (v2 >= v3)
            {
                return v2;
            }
            else
            {
                return v3;
            }
        }
        /// <summary>
        ///  Eğer True is Global ve eğer False ise Local alingment yapılacak.
        /// </summary>      
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            isGlobal = !isGlobal;
        }
        /// <summary>
        /// En son satırdaki en büyük sayı lazım. Onu işaretlemek için bu var.
        /// </summary>        
        private void findMaxOfMatrix()
        {
            if (!isGlobal) // Localde en büyüğü buluyoruz.
            {
                maxValue = Convert.ToInt32(dataGridView1.Rows[queryLen + 1].Cells[1].Value);
                for (int i = 2; i <= seqLen+1; i++)
                {
                    if (maxValue <= Convert.ToInt32(dataGridView1[i, queryLen + 1].Value))
                    {
                        maxValue = Convert.ToInt32(dataGridView1[i, queryLen + 1].Value);
                        maxRow = queryLen + 1;
                        maxCol = i;
                    }
                }
                dataGridView1.Rows[maxRow].Cells[maxCol].Style.BackColor = Color.Maroon;
                findParents(maxValue, maxCol, maxRow);
            }

            else
            {
                maxValue = Convert.ToInt32(dataGridView1.Rows[queryLen + 1].Cells[seqLen + 1].Value);
                dataGridView1.Rows[queryLen + 1].Cells[seqLen + 1].Style.BackColor = Color.Maroon;
                findParents(maxValue, seqLen + 1, queryLen + 1);
            }
        }
        /// <summary>
        /// Sondaki hücreye hangi hücreden ulaştığımızı bulmak için 3 tane hücreye bakan fonksiyon.
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        private void findParents(int cellValue, int colIndex, int rowIndex)
        {
            //Solundakine v2
            //diagonale v0
            //üstündekine v1 diyorum.
            if(colIndex == 1 || rowIndex == 1)
            {
                dataGridView1.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.Maroon;
            }

            else if (dataGridView1.Rows[rowIndex].Cells[0].Value.ToString() == dataGridView1.Rows[0].Cells[colIndex].Value.ToString())
            {
                //Match var.                
                if (Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex - 1].Value) == cellValue - match ) //v0
                {
                    cellValue = Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex - 1].Value);
                    dataGridView1.Rows[rowIndex - 1].Cells[colIndex - 1].Style.BackColor = Color.Maroon;
                    findParents(cellValue, colIndex - 1, rowIndex - 1);

                }
                else if (Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex].Value) == cellValue - indel ) //v2
                {
                    cellValue = Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex].Value);
                    dataGridView1.Rows[rowIndex - 1].Cells[colIndex].Style.BackColor = Color.Maroon;
                    findParents(cellValue, colIndex, rowIndex - 1);

                }
                else if (Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[colIndex - 1].Value) == cellValue - indel ) //v1
                {
                    cellValue = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[colIndex - 1].Value);
                    dataGridView1.Rows[rowIndex].Cells[colIndex - 1].Style.BackColor = Color.Maroon;
                    findParents(cellValue, colIndex - 1, rowIndex);

                }

            }
          else  if (dataGridView1.Rows[rowIndex].Cells[0].Value.ToString() != dataGridView1.Rows[0].Cells[colIndex].Value.ToString())
            {
                //Match var.

                
                if (Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex].Value) == cellValue - indel ) //v2
                {
                    cellValue = Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex].Value);
                    dataGridView1.Rows[rowIndex - 1].Cells[colIndex].Style.BackColor = Color.Maroon;
                    findParents(cellValue, colIndex, rowIndex - 1);

                }
                else if (Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[colIndex - 1].Value) == cellValue - indel) //v1
                {
                    cellValue = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[colIndex - 1].Value);
                    dataGridView1.Rows[rowIndex].Cells[colIndex - 1].Style.BackColor = Color.Maroon;
                    findParents(cellValue, colIndex - 1, rowIndex);
                }
               else if (Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex - 1].Value) == cellValue - mismatch) //v0
                {
                    cellValue = Convert.ToInt32(dataGridView1.Rows[rowIndex - 1].Cells[colIndex - 1].Value);
                    dataGridView1.Rows[rowIndex - 1].Cells[colIndex - 1].Style.BackColor = Color.Maroon;
                    findParents(cellValue, colIndex - 1, rowIndex - 1);

                }

            }

        }

















    }
}
