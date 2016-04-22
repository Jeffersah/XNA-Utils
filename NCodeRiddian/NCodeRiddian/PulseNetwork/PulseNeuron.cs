using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian.PulseNetwork
{
    public class PulseNeuron : Neuron
    {
        public int Neuron_Id;

        public double ActivationEnergy;
        public double EnergyDecay;
        double energy;

        public List<Connection> connections;

        public PulseNeuron(double AE, double ED, int Neuron_Id)
        {
            ActivationEnergy = AE;
            EnergyDecay = ED;
            energy = 0;
            connections = new List<Connection>();
            this.Neuron_Id = Neuron_Id;
        }
        public PulseNeuron(PulseBrain brain, PulseNeuron cloneme)
        {
            ActivationEnergy = cloneme.ActivationEnergy;
            EnergyDecay = cloneme.EnergyDecay;
            connections = new List<Connection>(cloneme.connections.Count);
            Neuron_Id = cloneme.Neuron_Id;
        }

        public void CloneConnections(PulseBrain brain, PulseNeuron cloneme)
        {
            foreach(Connection c in cloneme.connections)
            {
                connections.Add(new Connection(brain.getNeuron(c.target.Neuron_Id), c.amt));
            }
        }

        public void AddConnection(PulseNeuron neuron, double amt)
        {
            connections.Add(new Connection(neuron, amt));
        }

        public void Decay()
        {
            if (energy > 0)
                energy -= Math.Min(energy, EnergyDecay);
            else if (energy < 0)
                energy += Math.Min(-energy, EnergyDecay);
        }

        public void Pulse(double amt)
        {
            energy += amt;
            //if (energy < 0)
            //    energy = 0;
            if (energy >= ActivationEnergy)
                Trigger();
        }

        public double GetEnergy()
        {
            return energy;
        }

        public void Trigger()
        {
            energy = 0;
            foreach(Connection c in connections)
            {
                c.Trigger();
            }
        }

        public override double GetValue()
        {
            return energy;
        }

        public override void InputValue(double d)
        {
            Pulse(d);
        }
    }

    public class Connection
    {
        public PulseNeuron target;
        public double amt;

        public Connection(PulseNeuron n, double v)
        {
            target = n;
            amt = v;
        }

        public void Trigger()
        {
            target.Pulse(amt);
        }
    }
}
