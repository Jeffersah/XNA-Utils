using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian.BackrefNetwork
{
    public class B_Neuron:Neuron
    {
        public int Neuron_Id;

        public List<B_Connection> connections;
        public double Value;
        public bool hasValue;
        public B_Neuron(int Neuron_Id)
        {
            connections = new List<B_Connection>();
            this.Neuron_Id = Neuron_Id;
            Value = 0;
            hasValue = false;
        }
        public B_Neuron(B_Neuron cloneme)
        {
            connections = new List<B_Connection>(cloneme.connections.Count);
            Neuron_Id = cloneme.Neuron_Id;
            Value = 0;
            hasValue = false;
        }

        public void CloneConnections(B_Brain brain, B_Neuron cloneme)
        {
            connections = new List<B_Connection>(cloneme.connections.Count);
            foreach (B_Connection c in cloneme.connections)
            {
                connections.Add(new B_Connection(brain.getNeuron(c.target.Neuron_Id), c.Weight));
            }
        }

        public void AddConnection(B_Neuron neuron, double amt)
        {
            connections.Add(new B_Connection(neuron, amt));
        }
        public double Read()
        {
            if (hasValue)
                return Value;
            else
            {
                Value = 0;
                foreach(B_Connection bc in connections)
                {
                    Value += bc.Trigger();
                }
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

    public class B_Connection
    {
        public B_Neuron target;
        public double Weight;

        public B_Connection(B_Neuron n, double v)
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
