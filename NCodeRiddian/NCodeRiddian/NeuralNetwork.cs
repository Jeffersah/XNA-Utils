using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    public abstract class NeuralNetwork
    {
        public abstract void Reset();
        public abstract Neuron[] GetInputs();
        public abstract Neuron[] GetOutputs();
    }
    public abstract class Neuron
    {
        public abstract double GetValue();
        public abstract void InputValue(double d);
    }
}
