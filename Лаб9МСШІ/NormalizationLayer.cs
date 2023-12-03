using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаб9МСШІ
{
    public class NormalizationLayer
    {
        private double Xmin;
        private double Xmax;
        private double Dmin;
        private double Dmax;
        public int InputNeurons { get; }
        public int OutputNeurons { get; }
        public NormalizationLayer(List<DataRow> rows,int inputNeurons,int outputNeurons)
        {
            InputNeurons = inputNeurons;
            OutputNeurons = outputNeurons;
            NormalizeData(rows);
        }

        private void NormalizeData(List<DataRow> rows)
        {
            var xInputs = new double[rows.Count*InputNeurons];
            int a=0;
            foreach (DataRow row in rows)
            {
                for (int i = 0; i < InputNeurons; i++)
                {
                    xInputs[a+i] = Convert.ToDouble(row[i]);
                }
                a += InputNeurons;
            }
            Xmin = xInputs.Min();
            Xmax = xInputs.Max();
            var dInputs = new double[rows.Count * OutputNeurons];
            a = 0;
            foreach (DataRow row in rows)
            {
                for (int i = 0; i < OutputNeurons; i++)
                {
                    dInputs[a + i] = Convert.ToDouble(row[InputNeurons+i]);
                }
                a += OutputNeurons;
            }
            Dmax = dInputs.Max();
            Dmin = dInputs.Min();
        }
        private double GetNormalizedX(double x)
        {
            double result=x-Xmin;
            result=result/(Xmax-Xmin);
            return result;
        }
        private double GetNormalizedD(double d)
        {
            double result = d - Dmin;
            result = result / (Dmax - Dmin);
            return result;
        }
        private double GetNormalizedY(double y)
        {
            double result = y*(Dmax-Dmin);
            result=result+Dmin;
            return result;
        }
        public double[] NormalizedResults(double[] results)
        {
            var result=new double[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                result[i] = GetNormalizedY(results[i]);
            }
            return result;
        }
        public List<DataRow> GetNormalizedRows(List<DataRow> rows)
        { 
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < InputNeurons; j++)
                {
                    rows[i][j] = GetNormalizedX(Convert.ToDouble(rows[i][j]));
                }
                for (int k = InputNeurons; k < OutputNeurons+InputNeurons; k++)
                {
                    rows[i][k] = GetNormalizedD(Convert.ToDouble(rows[i][k]));
                }
            }
            return rows;
        }
        public double[] GetMaxDerrors(List<DataRow> rows)
        {
            var d=new double[rows.Count*OutputNeurons];
            var y = new double[rows.Count * OutputNeurons];
            int a = 0;
            foreach (DataRow row in rows)
            {
                for (int i = 0; i < OutputNeurons; i++)
                {
                    d[a + i] = Convert.ToDouble(row[InputNeurons + i]);
                    y[a + i] = Convert.ToDouble(row[InputNeurons + i+OutputNeurons]);
                }
                a += OutputNeurons;
            }
            var result1=new double[d.Length];
            var result2 = new double[y.Length];
            for (int i = 0; i < d.Length; i++)
            {
                result1[i] = Math.Abs(y[i] - d[i]);
                result2[i] = result1[i]/Dmax*100;
            }
            return new double[] { result1.Max(), result2.Max() };
        }
    }
}
