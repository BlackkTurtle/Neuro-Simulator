using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаб9МСШІ
{
    public class NeuralNetwork
    {
        private int _inputNeurons;
        private double learningRate { get; set; }
        private List<NeuralLayer> hiddenLayers;
        private NeuralLayer outputLayer;
        public NeuralNetwork(int inputNeurons, int hiddenLayers, int[] hiddenLayersNeurons, int outputNeurons, double learningRate)
        {
            _inputNeurons = inputNeurons;
            this.learningRate = learningRate;
            CreateHiddenLayers(inputNeurons, hiddenLayers, hiddenLayersNeurons);
            this.outputLayer = new NeuralLayer(hiddenLayersNeurons[hiddenLayers-1], outputNeurons);
        }
        private void CreateHiddenLayers(int inputNeurons, int hiddenLayers, int[] hiddenLayersNeurons)
        {
            this.hiddenLayers = new List<NeuralLayer>();
            for (int i = 0; i < hiddenLayers; i++)
            {
                if (i == 0)
                {
                    var neuralLayer = new NeuralLayer(inputNeurons, hiddenLayersNeurons[i]);
                    this.hiddenLayers.Add(neuralLayer);
                }
                else
                {
                    var neuralLayer = new NeuralLayer(hiddenLayersNeurons[i-1], hiddenLayersNeurons[i]);
                    this.hiddenLayers.Add(neuralLayer);
                }
            }
        }
        public double[] GetResult(DataRow row)
        {
            var inputs = new double[_inputNeurons];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = Convert.ToDouble(row[i]);
            }
            for (int i = 0; i < hiddenLayers[0].NeuronList.Count; i++)
            {
                hiddenLayers[0].NeuronList[i].inputs = inputs;
            }
            for (int i = 1; i < hiddenLayers.Count; i++)
            {
                inputs = new double[hiddenLayers[i - 1].NeuronList.Count];
                for (int k = 0; k < inputs.Length; k++)
                {
                    inputs[k] = hiddenLayers[i - 1].NeuronList[k].output;
                }
                for (int j = 0; j < hiddenLayers[i].NeuronList.Count; j++)
                {
                    hiddenLayers[i].NeuronList[j].inputs = inputs;
                }
            }
            inputs = new double[hiddenLayers[hiddenLayers.Count - 1].NeuronList.Count];
            for (int k = 0; k < inputs.Length; k++)
            {
                inputs[k] = hiddenLayers[hiddenLayers.Count - 1].NeuronList[k].output;
            }
            for (int i = 0; i < outputLayer.NeuronList.Count; i++)
            {
                outputLayer.NeuronList[i].inputs = inputs;
            }
            var results = new double[outputLayer.NeuronList.Count];
            for (int j = 0; j < outputLayer.NeuronList.Count; j++)
            {
                results[j] = outputLayer.NeuronList[j].output;
            }
            return results;
        }
        public double Train(List<DataRow> rows)
        {
            double result=0;
            foreach (DataRow row in rows)
            {
                double[] outputs=new double[outputLayer.NeuronList.Count];
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i]= Convert.ToDouble(row[i+_inputNeurons]);
                }
                ForwardPropagation(row);
                for (int j = 0; j < outputLayer.NeuronList.Count; j++)
                {
                    var res = Math.Abs(outputs[j]- outputLayer.NeuronList[j].output);
                    if (res > result) { result = res; } 
                }
                for (int i = 0; i < outputLayer.NeuronList.Count; i++)
                {
                    outputLayer.NeuronList[i].error = Sigmoid.derivative(outputLayer.NeuronList[i].output) * (outputs[i] - outputLayer.NeuronList[i].output);
                    outputLayer.NeuronList[i].adjustWeights(learningRate);
                }
                for (int i = 0; i < outputLayer.NeuronList.Count; i++)
                {
                    for (int j = 0; j < hiddenLayers[hiddenLayers.Count - 1].NeuronList.Count; j++)
                    {
                        hiddenLayers[hiddenLayers.Count - 1].NeuronList[j].error = Sigmoid.derivative(hiddenLayers[hiddenLayers.Count - 1].NeuronList[j].output) * outputLayer.NeuronList[i].error * outputLayer.NeuronList[i].weights[j];
                        hiddenLayers[hiddenLayers.Count - 1].NeuronList[j].adjustWeights(learningRate);
                    }
                }
                for (int i = hiddenLayers.Count - 2; i > 0; i--)
                {
                    for (int j = 0; j < hiddenLayers[i].NeuronList.Count; j++)
                    {
                        for (int k = 0; k < hiddenLayers[i + 1].NeuronList.Count; k++)
                        {
                            hiddenLayers[i].NeuronList[j].error = Sigmoid.derivative(hiddenLayers[i].NeuronList[j].output) * hiddenLayers[i + 1].NeuronList[k].error * hiddenLayers[i + 1].NeuronList[k].weights[j];
                            hiddenLayers[i].NeuronList[j].adjustWeights(learningRate);
                        }
                    }
                }
            }
            return result;
        }
        private void ForwardPropagation(DataRow row)
        {
            var inputs = new double[_inputNeurons];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = Convert.ToDouble(row[i]);
            }
            for (int i = 0; i < hiddenLayers[0].NeuronList.Count; i++)
            {
                hiddenLayers[0].NeuronList[i].inputs = inputs;
            }
            for (int i = 1; i < hiddenLayers.Count; i++)
            {
                inputs = new double[hiddenLayers[i - 1].NeuronList.Count];
                for (int k = 0; k < inputs.Length; k++)
                {
                    inputs[k] = hiddenLayers[i - 1].NeuronList[k].output;
                }
                for (int j = 0; j < hiddenLayers[i].NeuronList.Count; j++)
                {
                    hiddenLayers[i].NeuronList[j].inputs = inputs;
                }
            }
            inputs = new double[hiddenLayers[hiddenLayers.Count - 1].NeuronList.Count];
            for (int k = 0; k < inputs.Length; k++)
            {
                inputs[k] = hiddenLayers[hiddenLayers.Count - 1].NeuronList[k].output;
            }
            for (int i = 0; i < outputLayer.NeuronList.Count; i++)
            {
                outputLayer.NeuronList[i].inputs = inputs;
            }
        } 
    }
}
