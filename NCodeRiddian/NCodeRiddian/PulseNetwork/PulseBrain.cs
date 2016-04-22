using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian.PulseNetwork
{
    public class PulseBrain : NeuralNetwork
    {
        public List<PulseNeuron> Inputs;
        public List<List<PulseNeuron>> noninputs;

        public int M_NeuronID;

        public PulseBrain(int inputCount, int outputCount, int numberOfIntermediateLayers, int intermediateLayerCount)
        {
            int Nextid = 0;

            Inputs = new List<PulseNeuron>(inputCount);
            for(int i = 0; i < inputCount; i++)
            {
                Inputs.Add(new PulseNeuron(GlobalRandom.NextBetween(0, 1), GlobalRandom.NextBetween(0, 1), Nextid++));
            }
            noninputs = new List<List<PulseNeuron>>(1 + numberOfIntermediateLayers);
            for(int intermed = 0; intermed < numberOfIntermediateLayers; intermed++)
            {
                noninputs.Add(new List<PulseNeuron>(intermediateLayerCount));
                for (int i = 0; i < intermediateLayerCount; i++)
                {
                    noninputs[intermed].Add(new PulseNeuron(GlobalRandom.NextBetween(0, 1), GlobalRandom.NextBetween(0, 1), Nextid++));
                }
            }
            noninputs.Add(new List<PulseNeuron>(outputCount));
            for (int i = 0; i < outputCount; i++)
            {
                noninputs[numberOfIntermediateLayers].Add(new PulseNeuron(GlobalRandom.NextBetween(0, 1), GlobalRandom.NextBetween(0, 1), Nextid++));
            }
            List<PulseNeuron> prevlayer = Inputs;
            for(int i = 0; i < noninputs.Count; i++)
            {
                for(int from = 0; from < prevlayer.Count; from++)
                {
                    for(int to = 0; to < noninputs[i].Count; to++)
                    {
                        prevlayer[from].AddConnection(noninputs[i][to], GlobalRandom.NextBetween(0, 1));
                    }
                }
                prevlayer = noninputs[i];
            }
            M_NeuronID = Nextid;
        }
        public PulseBrain(PulseBrain clone)
        {
            Inputs = new List<PulseNeuron>(clone.Inputs.Count);
            noninputs = new List<List<PulseNeuron>>(clone.noninputs.Count);
            for (int i = 0; i < clone.Inputs.Count; i++)
            {
                Inputs.Add(new PulseNeuron(this, clone.Inputs[i]));
            }
            for(int l = 0; l < clone.noninputs.Count; l++)
            {
                noninputs.Add(new List<PulseNeuron>(clone.noninputs[l].Count));
                for(int i = 0; i < clone.noninputs[l].Count; i++)
                {
                   noninputs[l].Add(new PulseNeuron(this, clone.noninputs[l][i]));
                }
            }
            for (int i = 0; i < clone.Inputs.Count; i++)
            {
                Inputs[i].CloneConnections(this, clone.Inputs[i]);
            }
            for (int l = 0; l < clone.noninputs.Count; l++)
            {
                for (int i = 0; i < clone.noninputs[l].Count; i++)
                {
                    noninputs[l][i].CloneConnections(this, clone.noninputs[l][i]);
                }
            }
            M_NeuronID = clone.M_NeuronID;
        }

        public PulseNeuron getNeuron(int id)
        {
            for(int i = 0; i < noninputs.Count; i++)
            {
                for(int j = 0; j < noninputs[i].Count; j++)
                {
                    if (noninputs[i][j].Neuron_Id == id)
                        return noninputs[i][j];
                }
            }
            for(int i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].Neuron_Id == id)
                    return Inputs[i];
            }
            return null;
        }

        public void Mutate(double aeAdjust, double elAdjust, double cAdjust, int mCV)
        {
            PulseNeuron target = getNeuron(GlobalRandom.random.Next(M_NeuronID));
            target.ActivationEnergy += GlobalRandom.NextBetween(-aeAdjust, aeAdjust);
            if (target.ActivationEnergy < 0)
                target.ActivationEnergy = 0;
            target.EnergyDecay += GlobalRandom.NextBetween(-elAdjust, elAdjust);
            if (target.EnergyDecay < 0)
                target.EnergyDecay = 0;
            if(target.connections.Count > 0)
            { 
                Connection adjc = target.connections[GlobalRandom.random.Next(target.connections.Count)];
                adjc.amt += GlobalRandom.NextBetween(-cAdjust, cAdjust);
                adjc.amt = MathHelper.Clamp((float)adjc.amt, -mCV, mCV);
            }
        }

        public override void Reset()
        {

        }

        public List<PulseNeuron> GetInputsNG()
        {
            return Inputs;
        }
        public List<PulseNeuron> GetOutputsNG()
        {
            return noninputs[noninputs.Count - 1];
        }

        public override Neuron[] GetInputs()
        {
            return Inputs.ToArray();
        }

        public override Neuron[] GetOutputs()
        {
            return noninputs[noninputs.Count - 1].ToArray();
        }
    }
}
