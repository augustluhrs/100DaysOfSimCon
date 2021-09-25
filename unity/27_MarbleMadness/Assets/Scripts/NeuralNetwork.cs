using System;
using System.Collections.Generic;
using UnityEngine;

//based on The One's Youtube video "Tutorial on Programming an Evolving Neural Network in C# w/ Unity3D"
//thanks so much!

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] layers; //layers of the NN
    private float[][] neurons; //the neuron matrix
    private float[][][] weights; //the weights matrix
    private float fitness; //fitness of the NN
    private float normalizedFitness;
    //initialize NN with random weights
    public NeuralNetwork(int[] layers)
    {
        //deep copy of the layers
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        //generate the matrices
        InitNeurons();
        InitWeights();
    }

    //deep copy constructor
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }

    //copies the weights, duh
    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                { 
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    //create the neuron matrix
    private void InitNeurons()
    {
        //Initialize
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]); //add each layer to the neuron list
        }

        neurons = neuronsList.ToArray();
    }

    //create the weights matrix
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();

        //iterate over all neurons that have weights connected to previous layer
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i-1];

            //iterate over all neurons in this layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                
                //iterate over all neurons in the previous layer and set random weights
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights);
            }


            weightsList.Add(layerWeightsList.ToArray());
        }

        weights = weightsList.ToArray();
    }

    //feedforward system using given input array
    public float[] FeedForward(float[] inputs)
    {
        //add the inputs to the neuron matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }
        
        //iteraate over all neurons and compute feedforward values
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    value += weights[i-1][j][k] * neurons[i-1][k];
                }

                neurons[i][j] = (float)Math.Tanh(value); //hyperbolic tangent activation -- look into differences?
            }
        }

        return neurons[neurons.Length-1]; //returns the last layer, output
    }

    //mutate the weights
    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //mutate weight value
                    float randomNumber = UnityEngine.Random.Range(0f, 100f); //to get percentage chance out of 100

                    if (randomNumber <= 1f) //1% chance of weight flipping polarity
                    {
                        weight *= -1f;
                    }
                    else if (randomNumber <= 2f) //1% chance of new random weight
                    {
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f); 
                    }
                    else if (randomNumber <= 3f) //1% chance of increasing by random %
                    {
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 4f) //1% chance of decreasing by random %
                    {
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    //fitness functions
    public void AddFitness(float fit)
    {
        fitness += fit;
    }
    public void SetFitness(float fit)
    {
        fitness = fit;
    }
    public float GetFitness()
    {
        return fitness;
    }
    public void NormalizeFitness(float sum)
    {
        normalizedFitness = fitness / sum;
    }
    public float GetNormalizedFitness()
    {
        return normalizedFitness;
    }
    // compare and sort NNs based on fitness -- ascending
    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if (fitness > other.fitness)
        {
            return 1;
        }
        else if (fitness < other.fitness)
        {
            return -1;
        }
        else
        {
            return 0;
        }

    }
}
