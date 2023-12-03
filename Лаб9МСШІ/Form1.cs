using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаб9МСШІ
{
    public partial class Form1 : Form
    {
        private NeuralNetwork neuralNetwork;
        private NormalizationLayer normalizationLayer;
        private int[] hiddenLayerNeurons = new int[] { 1, 0, 0, 0, 0, 0, 0 };
        private int hiddenLayers = 1;
        private int lastLayerNeurons = 1;
        private int firstLayerNeurons=1;
        private double learningRate = 1;
        private double trainTillpercent = 5;
        private int epochquantity = 1000;
        public Form1()
        {
            InitializeComponent();
        }
        private void learningRateTextChanged(object sender, EventArgs e)
        {
            try
            {
                learningRate = Convert.ToDouble(textBox12.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void epochQuantityTextChanged(object sender, EventArgs e)
        {
            try
            {
                epochquantity = Convert.ToInt32(textBox11.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            List<DataGridViewTextBoxColumn> columns = new List<DataGridViewTextBoxColumn>();
            for (int i = 0; i < firstLayerNeurons; i++)
            {
                var column = new DataGridViewTextBoxColumn();
                column.Name = "x " + i.ToString();
                column.ValueType = typeof(string);
                columns.Add(column);
            }
            for (int i = 0; i < lastLayerNeurons; i++)
            {
                var column = new DataGridViewTextBoxColumn();
                column.Name = "d " + i.ToString();
                column.ValueType = typeof(string);
                columns.Add(column);
            }
            foreach (DataGridViewColumn column in columns)
            {
                dataGridView1.Columns.Add(column);
                dataGridView2.Columns.Add(column.Clone() as DataGridViewColumn);
            }
            for (int i = 0; i < lastLayerNeurons; i++)
            {
                var column = new DataGridViewTextBoxColumn();
                column.Name = "y " + i.ToString();
                column.ValueType = typeof(string);
                column.ReadOnly = true;
                dataGridView2.Columns.Add(column);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                firstLayerNeurons = Convert.ToInt32(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayers = Convert.ToInt32(textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayerNeurons[0] = Convert.ToInt32(textBox3.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayerNeurons[1] = Convert.ToInt32(textBox4.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayerNeurons[2] = Convert.ToInt32(textBox5.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayerNeurons[3] = Convert.ToInt32(textBox6.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayerNeurons[4] = Convert.ToInt32(textBox7.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayerNeurons[5] = Convert.ToInt32(textBox8.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                hiddenLayerNeurons[6] = Convert.ToInt32(textBox9.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lastLayerNeurons = Convert.ToInt32(textBox10.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Add();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count != 0)
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn dgvCol in dataGridView1.Columns)
            {
                DataColumn dtCol = new DataColumn(dgvCol.Name, dgvCol.ValueType);
                dtCol.Caption = dgvCol.HeaderText;

                dt.Columns.Add(dtCol);
            }
            var rows = new List<DataRow>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                {
                    row[j]=dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
                rows.Add(row);
            }
            normalizationLayer = new NormalizationLayer(rows,firstLayerNeurons,lastLayerNeurons);
            var normalizedRows = normalizationLayer.GetNormalizedRows(rows);
            neuralNetwork = new NeuralNetwork(firstLayerNeurons, hiddenLayers, hiddenLayerNeurons, lastLayerNeurons, learningRate);
            for (int i = 0; i < epochquantity; i++)
            {
                neuralNetwork.Train(normalizedRows);
            }
            MessageBox.Show("NeuralNetwork trained.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn dgvCol in dataGridView2.Columns)
            {
                DataColumn dtCol = new DataColumn(dgvCol.Name, typeof(string));
                dtCol.Caption = dgvCol.HeaderText;

                dt.Columns.Add(dtCol);
            }
            var rows = new List<DataRow>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < dataGridView2.Rows[i].Cells.Count-lastLayerNeurons; j++)
                {
                    row[j] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                }
                rows.Add(row);
            }
            var normalizedRows = normalizationLayer.GetNormalizedRows(rows);
            for (int i = 0; i < normalizedRows.Count; i++)
            {
                var row = normalizedRows[i];
                var results = normalizationLayer.NormalizedResults(neuralNetwork.GetResult(row));
                for (int j = 0; j < results.Length; j++)
                {
                    dataGridView2.Rows[i].Cells["y "+j.ToString()].Value=results[j];
                }
            }
            rows = new List<DataRow>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < dataGridView2.Rows[i].Cells.Count; j++)
                {
                    row[j] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                }
                rows.Add(row);
            }
            var derrors = normalizationLayer.GetMaxDerrors(rows);
            label9.Text = derrors[0].ToString();
            label10.Text = derrors[1].ToString();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            try
            {
                epochquantity = Convert.ToInt32(textBox11.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn dgvCol in dataGridView1.Columns)
            {
                DataColumn dtCol = new DataColumn(dgvCol.Name, dgvCol.ValueType);
                dtCol.Caption = dgvCol.HeaderText;

                dt.Columns.Add(dtCol);
            }
            var rows = new List<DataRow>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                {
                    row[j] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
                rows.Add(row);
            }
            var normalizedRows = normalizationLayer.GetNormalizedRows(rows);
            for (int i = 0; i < epochquantity; i++)
            {
                neuralNetwork.Train(normalizedRows);
            }
            MessageBox.Show("NeuralNetwork trained.");
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trainTillpercent = Convert.ToDouble(textBox13.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn dgvCol in dataGridView1.Columns)
            {
                DataColumn dtCol = new DataColumn(dgvCol.Name, dgvCol.ValueType);
                dtCol.Caption = dgvCol.HeaderText;

                dt.Columns.Add(dtCol);
            }
            var rows = new List<DataRow>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                {
                    row[j] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
                rows.Add(row);
            }
            normalizationLayer = new NormalizationLayer(rows, firstLayerNeurons, lastLayerNeurons);
            var normalizedRows = normalizationLayer.GetNormalizedRows(rows);
            neuralNetwork = new NeuralNetwork(firstLayerNeurons, hiddenLayers, hiddenLayerNeurons, lastLayerNeurons, learningRate);
            for (int i = 0; i < epochquantity; i++)
            {
                var result=neuralNetwork.Train(normalizedRows);
                if (result <= trainTillpercent / 100) 
                {
                    MessageBox.Show(i.ToString());
                    break; 
                }
            }
            MessageBox.Show("NeuralNetwork trained.");
        }
    }
}
