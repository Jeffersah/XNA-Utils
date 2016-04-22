using System;
using System.Collections.Generic;

namespace NCodeRiddian.oldnetwork
{
    public class Network<E>
    {
        public const byte C_INPUT = 0;
        public const byte C_HIDDEN = 1;
        public const byte C_OUTPUT = 2;
        public const byte C_ALL = 3;

        public List<InputNeuron<E>> InNeurons;
        public List<HiddenNeuron<E>> HiddenNeurons;
        public List<OutputNeuron<E>> OutNeurons;
        private int totalConnections;

        public static bool forceKill = false;

        public Network()
        {
            InNeurons = new List<InputNeuron<E>>();
            HiddenNeurons = new List<HiddenNeuron<E>>();
            OutNeurons = new List<OutputNeuron<E>>();
        }

        public InputNeuron<E> AddInputNeuron()
        {
            InputNeuron<E> tmp = new InputNeuron<E>();
            InNeurons.Add(tmp);
            return tmp;
        }

        public HiddenNeuron<E> AddHiddenNeuron(HiddenNeuron<E>.NeuronOperation op)
        {
            HiddenNeuron<E> tmp = new HiddenNeuron<E>(op);
            HiddenNeurons.Add(tmp);
            return tmp;
        }

        public OutputNeuron<E> AddOutputNeuron(OutputNeuron<E>.DoOperation op)
        {
            OutputNeuron<E> tmp = new OutputNeuron<E>(op);
            OutNeurons.Add(tmp);
            return tmp;
        }

        public void DisconnectH(int i, int i2)
        {
            HiddenNeurons[i].Disconnect(HiddenNeurons[i2]);
            totalConnections--;
        }

        public void DisconnectIH(int i, int i2)
        {
            InNeurons[i].Disconnect(HiddenNeurons[i2]);
            totalConnections--;
        }

        public void DisconnectHO(int i, int i2)
        {
            HiddenNeurons[i].Disconnect(OutNeurons[i2]);
            totalConnections--;
        }

        public bool ConnectIH(int i, int i2)
        {
            bool v = InNeurons[i].ConnectOutput(HiddenNeurons[i2]);
            if (v)
                totalConnections++;
            return v;
        }

        public bool ConnectH(int i, int i2)
        {
            bool v = HiddenNeurons[i].ConnectOutput(HiddenNeurons[i2]);
            if (v)
                totalConnections++;
            return v;
        }

        public bool ConnectHO(int i, int i2)
        {
            bool v = HiddenNeurons[i].ConnectOutput(OutNeurons[i2]);
            if (v)
                totalConnections++;
            return v;
        }

        public bool ConnectIO(int i, int i2)
        {
            bool v = InNeurons[i].ConnectOutput(OutNeurons[i2]);
            if (v)
                totalConnections++;
            return v;
        }

        public void Fire(int i, params E[] fireparam)
        {
            InNeurons[i].fireNeuron(fireparam);
        }

        public void FireWithLimit(int i, int lim, params E[] fireparam)
        {
            InNeurons[i].fireNeuronLimit(lim, fireparam);
        }

        public void Remove(int i)
        {
            for (int n = HiddenNeurons[i].getInputs().Count - 1; n >= 0; n--)
            {
                HiddenNeurons[i].getInputs()[n].RemoveOutput(HiddenNeurons[i]);
            }
            for (int n = HiddenNeurons[i].getOutputs().Count - 1; n >= 0; n--)
            {
                HiddenNeurons[i].getOutputs()[n].RemoveInput(HiddenNeurons[i]);
            }
            HiddenNeurons.RemoveAt(i);
        }

        public void RemoveAndCross(int i)
        {
            for (int n = HiddenNeurons[i].getInputs().Count - 1; n >= 0; n--)
            {
                foreach (Neuron con in HiddenNeurons[i].getOutputs())
                {
                    HiddenNeurons[i].getInputs()[n].ConnectOutput(con);
                }
                HiddenNeurons[i].getInputs()[n].RemoveOutput(HiddenNeurons[i]);
            }
            for (int n = HiddenNeurons[i].getOutputs().Count - 1; n >= 0; n--)
            {
                HiddenNeurons[i].getOutputs()[n].RemoveInput(HiddenNeurons[i]);
            }
            HiddenNeurons.RemoveAt(i);
        }

        public int Count(byte nm)
        {
            switch (nm)
            {
                case 0:
                    return InNeurons.Count;
                case 1:
                    return HiddenNeurons.Count;
                case 2:
                    return OutNeurons.Count;
                case 3:
                    return InNeurons.Count + HiddenNeurons.Count + OutNeurons.Count;
                default:
                    return -1;
            }
        }

        public Network<E> CloneNetwork()
        {
            Network<E> opt = new Network<E>();

            foreach (HiddenNeuron<E> hn in HiddenNeurons)
            {
                opt.AddHiddenNeuron(hn.operation);
                List<E> adprm = new List<E>();
                adprm.AddRange(hn.AdditionalParameters);
                opt.HiddenNeurons[opt.HiddenNeurons.Count - 1].AdditionalParameters = adprm;
            }
            foreach (InputNeuron<E> n in InNeurons)
            {
                opt.AddInputNeuron();
            }
            foreach (OutputNeuron<E> n in OutNeurons)
            {
                opt.AddOutputNeuron(n.operation);
            }

            for (int n = 0; n < InNeurons.Count; n++)
            {
                foreach (Neuron nrn in InNeurons[n].getOutputs())
                {
                    if (nrn is HiddenNeuron<E>)
                    {
                        if (!opt.ConnectIH(n, HiddenNeurons.IndexOf((HiddenNeuron<E>)nrn)))
                            Console.Out.WriteLine("InToHidden Fail");
                    }
                    else if (nrn is OutputNeuron<E>)
                    {
                        if (!opt.ConnectIO(n, OutNeurons.IndexOf((OutputNeuron<E>)nrn)))
                            Console.Out.WriteLine("InToOut Fail");
                    }
                }
            }

            for (int n = 0; n < HiddenNeurons.Count; n++)
            {
                foreach (Neuron nrn in HiddenNeurons[n].getOutputs())
                {
                    if (nrn is HiddenNeuron<E>)
                    {
                        if (!opt.ConnectH(n, HiddenNeurons.IndexOf((HiddenNeuron<E>)nrn)))
                            Console.Out.WriteLine("HiddenToHidden Fail");
                    }
                    else if (nrn is OutputNeuron<E>)
                    {
                        if (!opt.ConnectHO(n, OutNeurons.IndexOf((OutputNeuron<E>)nrn)))
                            Console.Out.WriteLine("HiddenToOut Fail");
                    }
                }
            }
            //Console.Out.WriteLine(ToString() + " :: " + opt.ToString());
            return opt;
        }

        public override string ToString()
        {
            return "Network i" + Count(0) + " h" + Count(1) + " o" + Count(2) + " c" + totalConnections;
        }
    }
}