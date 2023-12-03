using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаб9МСШІ
{
    public class Neuron
    {
        public double[] inputs;
        public double[] weights;
        public double error = 0;

        private double biasWeight;
        public Neuron(int inputsNumber)
        {
            inputs = new double[inputsNumber];
            weights = new double[inputsNumber];
        }

        private Random r = new Random();

        public double output
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < weights.Length; i++)
                {
                    sum += weights[i] * inputs[i];
                }
                sum += biasWeight;
                return Sigmoid.output(sum);
            }
        }

        public void randomizeWeights()
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = r.NextDouble();
            }
            biasWeight = r.NextDouble();
        }

        public void adjustWeights(double learningRate)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += error * inputs[i] * learningRate;
            }
            biasWeight += error * learningRate;
        }
    }
}
