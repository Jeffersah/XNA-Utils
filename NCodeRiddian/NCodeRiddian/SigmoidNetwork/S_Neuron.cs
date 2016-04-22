using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian.SigmoidNetwork
{
    public class S_Neuron : Neuron
    {
        public int Neuron_Id;

        public List<S_Connection> connections;
        public double Value;
        public bool hasValue;

        public double Bias;

        public S_Neuron(int Neuron_Id, double Bias)
        {
            connections = new List<S_Connection>();
            this.Neuron_Id = Neuron_Id;
            Value = 0;
            this.Bias = Bias;
            hasValue = false;
        }
        public S_Neuron(S_Neuron cloneme)
        {
            connections = new List<S_Connection>(cloneme.connections.Count);
            Neuron_Id = cloneme.Neuron_Id;
            Value = 0;
            Bias = cloneme.Bias;
            hasValue = false;
        }

        public void CloneConnections(S_Brain brain, S_Neuron cloneme)
        {
            connections = new List<S_Connection>(cloneme.connections.Count);
            foreach (S_Connection c in cloneme.connections)
            {
                connections.Add(new S_Connection(brain.getNeuron(c.target.Neuron_Id), c.Weight));
            }
        }

        public void AddConnection(S_Neuron neuron, double amt)
        {
            connections.Add(new S_Connection(neuron, amt));
        }
        public double Read()
        {
            if (hasValue)
                return Value;
            else
            {
                Value = 0;
                foreach(S_Connection bc in connections)
                {
                    Value += bc.Trigger();
                }
                Value += Bias;
                Value = 1 / (1 + Math.Exp(Value));
                hasValue = true;
                return Value;
            }
        }

        public override double GetValue()
        {
            return Read();
        }

        public override void InputValue(double d)
        {
            Value = d;
            hasValue = true;
        }
    }

    public class S_Connection
    {
        public S_Neuron target;
        public double Weight;

        public S_Connection(S_Neuron n, double v)
        {
            target = n;
            Weight = v;
        }

        public double Trigger()
        {
            return Weight * target.Read();
        }
    }
}
