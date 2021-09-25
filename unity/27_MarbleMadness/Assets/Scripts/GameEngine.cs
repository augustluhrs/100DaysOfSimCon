using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public GameObject marblePrefab;
    [SerializeField] GameObject startMarble;
    Vector3 startPos;
    public GameObject goal;

    private bool isRacing = false; //when game is on/off vs when it's repopulating
    [SerializeField] float generationTime = 20;
    private int populationSize = 20;
    private int generationNumber = 0;
    private int[] layers = new int[] {9, 8, 8, 4}; //9 inputs, 2 hidden layers of 8, 4 outputs
    private List<NeuralNetwork> nets = new List<NeuralNetwork>();
    private List<NeuralNetwork> newNets = new List<NeuralNetwork>();
    private List<Marble> marbleList = null;

    void Start()
    {
        // startPos = startMarble.transform.position;

        //since can't assign scene to prefab, have to find the goal
        goal = GameObject.Find("GoalCube");
        GameObject start = GameObject.Find("StartPodium");
        //hopefully solves issue of flying out on goal
        startPos = start.transform.position + new Vector3 (0, 1, 0);
    }

    void Timer()
    {
       isRacing = false; 
    }

    void Update()
    {
        if (isRacing == false) //if new round/generation, not currently racing
        {
            if (generationNumber == 0) //if first generation
            {
                InitMarbleNeuralNetwork();
            }
            else 
            {
                /*
                nets.Sort(); //sorts the nets in ascending order
                //debugging checking for fitness
                for (int i = 0; i < populationSize; i++)
                {
                    Debug.Log(nets[i].GetFitness());                    

                }
                //original mutation stage for nets
                
                for (int i = 0; i < populationSize / 2; i++)
                {
                    nets[i] = new NeuralNetwork(nets[i+(populationSize / 2)]); //because only taking best half?
                    nets[i].Mutate(); //so the worst half becomes a mutated copy of the better half?

                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //hmm not ideal, need to figure out better way
                }
                */

                //newNets.Clear();
                
                //Shiffman's "Improved Selection Pool" method
                //get total fitness
                float sumFitness = 0;
                for (int i = 0; i < populationSize; i++)
                {
                    sumFitness += nets[i].GetFitness();
                }
                //assign each a normalized fitness
                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].NormalizeFitness(sumFitness);
                }
                //pool creation
                for (int i = 0; i < populationSize; i++)
                {
                    //pickPool();
                    for (int j = 0; j < nets[i].GetNormalizedFitness() * 10; j++)
                    {
                        newNets.Add(new NeuralNetwork(nets[i]));
                    }
                }
                //selection
                nets.Clear();

                for (int i = 0; i < populationSize; i++)
                {
                    NeuralNetwork n = newNets[Random.Range(0, newNets.Count)];
                    nets.Add(new NeuralNetwork(n));
                }
                //mutation
                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].Mutate();
                    //newNets[i].Mutate(); //might need to lessen rate
                }
                /*
                for (int i = 0; i < populationSize; i++)
                {
                    nets.RemoveAt(0);
                    nets.Add(newNets[i]);
                }
                */
                //newNets = new List<NeuralNetwork>(); //hopefully resets...
                sumFitness = 0;
                newNets.Clear();

                //reset fitness for new generation
                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            generationNumber++;
            Debug.Log("GENERATION: " + generationNumber);

            isRacing = true;
            Invoke("Timer", generationTime); //adjust later
            CreateMarbles();
        }
    }

    private void CreateMarbles()
    {
        if (marbleList != null) //if there is a current population of marbles, i.e. not at beginning
        {
            for (int i = 0; i < marbleList.Count; i++){
                GameObject.Destroy(marbleList[i].gameObject); //get rid of old generation
            }
        }

        marbleList = new List<Marble>();

        for (int i = 0; i < populationSize; i++)
        { //sets up each marble with its corrsponding NN and instantiates them at start
            Marble marble = ((GameObject)Instantiate(marblePrefab, new Vector3(startPos.x, startPos.y, startPos.z), Quaternion.identity)).GetComponent<Marble>();
            marble.Init(nets[i], goal); //TODO check this later
            marbleList.Add(marble);
        }
    }

    void InitMarbleNeuralNetwork()
    {
        //he has a thing about setting the population to 20, but i don't think we need it?

        //nets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }

    void pickPool()
    {
        int index = 0;
        float r = UnityEngine.Random.Range(0f, 1f);

        while (r > 0)
        {
            r = r - nets[index].GetNormalizedFitness();
            index++;
        }
        Debug.Log(nets[index-1].GetNormalizedFitness());
        Debug.Log(index);
        Debug.Log(nets[index-1]);
        // NeuralNetwork newBoy = new NeuralNetwork(nets[index-1]);
        // newNets.Add(new NeuralNetwork(nets[index-1]));
        newNets.Add(nets[index-1]);
        
        // newNets.Add(newBoy);

        
    }
}
