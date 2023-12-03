using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаб9МСШІ
{
    public class NeuralLayer
    {
        public List<Neuron> NeuronList;
        public NeuralLayer(int inputsNumber, int neuronsNumber)
        {
            CreateNeurons(inputsNumber, neuronsNumber);
        }
        private void CreateNeurons(int inputsNumber, int neuronsNumber)
        {
            NeuronList = new List<Neuron>();
            for (int i = 0; i < neuronsNumber; i++)
            {
                Neuron neuron = new Neuron(inputsNumber);
                neuron.randomizeWeights();
                NeuronList.Add(neuron);
            }
        }
    }
}
