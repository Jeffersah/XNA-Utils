using System;
using System.Collections.Generic;

namespace NCodeRiddian.oldnetwork
{
    public class Neuron
    {
        protected List<Neuron> inputs;
        protected List<Neuron> outputs;

        public Neuron()
        {
            inputs = new List<Neuron>();
            outputs = new List<Neuron>();
        }

        public bool ConnectOutput(Neuron n)
        {
            if (inputs.Contains(n) || outputs.Contains(n))
                return false;

            outputs.Add(n);
            n.inputs.Add(this);
            return true;
        }

        public bool ConnectInput(Neuron n)
        {
            if (outputs.Contains(n) || inputs.Contains(n))
                return false;

            n.outputs.Add(this);
            inputs.Add(n);
            return true;
        }

        public void Disconnect(Neuron n)
        {
            if (inputs.Remove(n))
                n.outputs.Remove(this);
            if (outputs.Remove(n))
                n.inputs.Remove(this);
        }

        public void Disconnect(Neuron n, bool input)
        {
            if (input)
            {
                inputs.Remove(n);
                n.outputs.Remove(this);
            }
            else
            {
                n.inputs.Remove(n);
                outputs.Remove(this);
            }
        }

        public List<Neuron> getInputs()
        {
            return inputs;
        }

        public List<Neuron> getOutputs()
        {
            return outputs;
        }

        public void RemoveInput(Neuron n)
        {
            inputs.Remove(n);
        }

        public void RemoveOutput(Neuron n)
        {
            outputs.Remove(n);
        }
    }

    public class HiddenNeuron<E> : Neuron
    {
        public delegate bool NeuronOperation(HiddenNeuron<E> n, E[] parameters, List<E> NeuronSpecificParameters);

        public NeuronOperation operation;

        public List<E> AdditionalParameters;

        public HiddenNeuron(NeuronOperation op)
        {
            operation = op;
            AdditionalParameters = new List<E>();
        }

        public void setOperation(NeuronOperation operation)
        {
            this.operation = operation;
            AdditionalParameters = new List<E>();
        }

        public void setOperation(NeuronOperation operation, List<E> AdParam)
        {
            this.operation = operation;
            AdditionalParameters = AdParam;
        }

        public void fireNeuronLimit(int limit, params E[] parameters)
        {
            if (limit <= 0)
                Network<E>.forceKill = true;
            E[] myParams = new E[parameters.Length];
            for (int x = 0; x < parameters.Length; x++)
            {
                myParams[x] = parameters[x];
            }
            if (operation(this, myParams, AdditionalParameters))
            {
                for (int x = outputs.Count - 1; x >= 0; x--)
                {
                    if (Network<E>.forceKill)
                        return;
                    if (outputs[x] is HiddenNeuron<E>)
                    {
                        ((HiddenNeuron<E>)outputs[x]).fireNeuronLimit(limit - 1, myParams);
                    }
                    else if (outputs[x] is OutputNeuron<E>)
                    {
                        ((OutputNeuron<E>)outputs[x]).fireNeuron(myParams);
                    }
                    else
                    {
                        throw new NeuronTypeMismatchException("Type of target Neuron doesn't match this neuron!");
                    }
                }
            }
        }

        public void fireNeuron(params E[] parameters)
        {
            E[] myParams = new E[parameters.Length];
            for (int x = 0; x < parameters.Length; x++)
            {
                myParams[x] = parameters[x];
            }
            if (operation(this, myParams, AdditionalParameters))
            {
                for (int x = outputs.Count - 1; x >= 0; x--)
                {
                    if (outputs[x] is HiddenNeuron<E>)
                    {
                        ((HiddenNeuron<E>)outputs[x]).fireNeuron(myParams);
                    }
                    else if (outputs[x] is OutputNeuron<E>)
                    {
                        ((OutputNeuron<E>)outputs[x]).fireNeuron(myParams);
                    }
                    else
                    {
                        throw new NeuronTypeMismatchException("Type of target Neuron doesn't match this neuron!");
                    }
                }
            }
        }
    }

    public class InputNeuron<E> : Neuron
    {
        public void fireNeuronLimit(int limit, params E[] parameters)
        {
            Network<E>.forceKill = false;
            for (int x = outputs.Count - 1; x >= 0; x--)
            {
                if (outputs[x] is HiddenNeuron<E>)
                {
                    ((HiddenNeuron<E>)outputs[x]).fireNeuronLimit(limit - 1, parameters);
                }
                else if (outputs[x] is OutputNeuron<E>)
                {
                    ((OutputNeuron<E>)outputs[x]).fireNeuron(parameters);
                }
                else
                {
                    throw new NeuronTypeMismatchException("Type of target Neuron doesn't match this neuron!");
                }
            }
        }

        public void fireNeuron(params E[] parameters)
        {
            for (int x = outputs.Count - 1; x >= 0; x--)
            {
                if (outputs[x] is HiddenNeuron<E>)
                {
                    ((HiddenNeuron<E>)outputs[x]).fireNeuron(parameters);
                }
                else if (outputs[x] is OutputNeuron<E>)
                {
                    ((OutputNeuron<E>)outputs[x]).fireNeuron(parameters);
                }
                else
                {
                    throw new NeuronTypeMismatchException("Type of target Neuron doesn't match this neuron!");
                }
            }
        }
    }

    public class OutputNeuron<E> : Neuron
    {
        public delegate void DoOperation(Neuron n, params E[] parameters);

        public DoOperation operation;

        public OutputNeuron(DoOperation op)
        {
            operation = op;
        }

        public void fireNeuron(params E[] parameters)
        {
            operation(this, parameters);
        }
    }

    public class NeuronTypeMismatchException : Exception
    {
        public NeuronTypeMismatchException(string s)
            : base(s)
        {
        }
    }
}