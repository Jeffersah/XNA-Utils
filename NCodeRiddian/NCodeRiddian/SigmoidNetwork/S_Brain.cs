using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian.SigmoidNetwork
{
    public class S_Brain : NeuralNetwork
    {
        public S_Neuron[][] Neurons;

        public int M_NeuronID;

        public S_Brain(int inputCount, int outputCount, int numberOfIntermediateLayers, int intermediateLayerCount)
        {
            int nid = 0;
            Neurons = new S_Neuron[numberOfIntermediateLayers + 2][];
            Neurons[0] = new S_Neuron[inputCount];
            for(int i = 0; i < inputCount; i++)
            {
                Neurons[0][i] = new S_Neuron(nid++, GlobalRandom.NextBetween(-10,10));
            }
            for(int layer = 0; layer < numberOfIntermediateLayers; layer++)
            {
                Neurons[layer + 1] = new S_Neuron[intermediateLayerCount];
                for (int i = 0; i < intermediateLayerCount; i++)
                    Neurons[layer + 1][i] = new S_Neuron(nid++, GlobalRandom.NextBetween(-10, 10));
            }
            Neurons[numberOfIntermediateLayers + 1] = new S_Neuron[outputCount];
            for(int i = 0; i < outputCount; i++)
            {
                Neurons[numberOfIntermediateLayers + 1][i] = new S_Neuron(nid++, GlobalRandom.NextBetween(-10, 10));
            }
            for(int layer = 1; layer < Neurons.Length; layer++)
            {
                for(int i = 0; i < Neurons[layer-1].Length; i++)
                {
                    for (int j = 0; j < Neurons[layer].Length; j++)
                    {
                        Neurons[layer][j].AddConnection(Neurons[layer - 1][i], GlobalRandom.NextBetween(-1, 1));
                    }
                }
            }
            M_NeuronID = nid;
        }
        public S_Brain(S_Brain clone)
        {
            Neurons = new S_Neuron[clone.Neurons.Length][];
            Neurons[0] = new S_Neuron[clone.Neurons[0].Length];
            for (int i = 0; i < Neurons[0].Length; i++)
            {
                Neurons[0][i] = new S_Neuron(clone.Neurons[0][i]);
            }
            for (int layer = 0; layer < clone.Neurons.Length - 2; layer++)
            {
                Neurons[layer + 1] = new S_Neuron[clone.Neurons[layer+1].Length];
                for (int i = 0; i < clone.Neurons[layer + 1].Length; i++)
                    Neurons[layer + 1][i] = new S_Neuron(clone.Neurons[layer + 1][i]);
            }
            Neurons[clone.Neurons.Length-1] = new S_Neuron[clone.Neurons[clone.Neurons.Length - 1].Length];
            for (int i = 0; i < clone.Neurons[clone.Neurons.Length - 1].Length; i++)
            {
                Neurons[clone.Neurons.Length - 1][i] = new S_Neuron(clone.Neurons[clone.Neurons.Length - 1][i]);
            }
            M_NeuronID = clone.M_NeuronID;
            for (int layer = 0; layer < Neurons.Length; layer++)
            {
                for (int i = 0; i < Neurons[layer].Length; i++)
                    Neurons[layer][i].CloneConnections(this, clone.Neurons[layer][i]);
            }
        }

        public S_Brain(S_Brain Parent1, S_Brain Parent2)
        {
            Neurons = new S_Neuron[Parent1.Neurons.Length][];
            Neurons[0] = new S_Neuron[Parent1.Neurons[0].Length];
            for (int i = 0; i < Neurons[0].Length; i++)
            {
                Neurons[0][i] = new S_Neuron(RandomParent(Parent1, Parent2).Neurons[0][i]);
            }
            for (int layer = 0; layer < Parent1.Neurons.Length - 2; layer++)
            {
                Neurons[layer + 1] = new S_Neuron[Parent1.Neurons[layer + 1].Length];
                for (int i = 0; i < Parent1.Neurons[layer + 1].Length; i++)
                    Neurons[layer + 1][i] = new S_Neuron(RandomParent(Parent1, Parent2).Neurons[layer + 1][i]);
            }
            Neurons[Parent1.Neurons.Length - 1] = new S_Neuron[Parent1.Neurons[Parent1.Neurons.Length - 1].Length];
            for (int i = 0; i < Parent1.Neurons[Parent1.Neurons.Length - 1].Length; i++)
            {
                Neurons[Parent1.Neurons.Length - 1][i] = new S_Neuron(RandomParent(Parent1, Parent2).Neurons[Parent1.Neurons.Length - 1][i]);
            }
            M_NeuronID = Parent1.M_NeuronID;
            for (int layer = 0; layer < Neurons.Length; layer++)
            {
                for (int i = 0; i < Neurons[layer].Length; i++)
                    Neurons[layer][i].CloneConnections(this, RandomParent(Parent1, Parent2).Neurons[layer][i]);
            }
        }

        public S_Brain RandomParent(S_Brain Parent1, S_Brain Parent2)
        {
            if (GlobalRandom.random.Next(500) % 2 == 0)
                return Parent1;
            return Parent2;
        }

        public override void Reset()
        {
            for (int i = 1; i < Neurons.Length; i++)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                    Neurons[i][j].hasValue = false;
            }
        }

        public S_Neuron getNeuron(int id)
        {
            if (id > M_NeuronID || id < 0)
                return null;
            for(int i = 0; i < Neurons.Length; i++)
            {
                for (int j = 0; j < Neurons[i].Length; j++)
                    if (Neurons[i][j].Neuron_Id == id)
                        return Neurons[i][j];
            }
            return null;
        }

        public void Mutate(double bias, double cAdjust, int mCV)
        {
            int tgt = GlobalRandom.random.Next(M_NeuronID);
            S_Neuron target = getNeuron(tgt);
            double adb = GlobalRandom.NextBetween(-bias, bias);
            target.Bias += adb;
            if(target.connections.Count > 0)
            { 
                S_Connection adjc = target.connections[GlobalRandom.random.Next(target.connections.Count)];
                adjc.Weight += GlobalRandom.NextBetween(-cAdjust, cAdjust);
                adjc.Weight = MathHelper.Clamp((float)adjc.Weight, -mCV, mCV);
            }
        }

        public S_Neuron[] GetInputsNG()
        {
            return Neurons[0];
        }
        public S_Neuron[] GetOutputsNG()
        {
            return Neurons[Neurons.Length - 1];
        }

        public override Neuron[] GetInputs()
        {
            return Neurons[0];
        }

        public override Neuron[] GetOutputs()
        {
            return Neurons[Neurons.Length - 1];
        }
    }
}
