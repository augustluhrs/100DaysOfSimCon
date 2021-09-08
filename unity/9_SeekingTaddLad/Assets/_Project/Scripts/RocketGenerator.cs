using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketGenerator : MonoBehaviour
{
    int lifetime = 10;
    float timeElapsed = 0;
    int timer = 0;
    public int generation = 1;
    public float bestFitness = 0;
    public float mutationRate = 0.02f;

    public Text gen;
    public Text fitness;
    // RocketMover[] rockets;
    public GameObject target;
    public List<RocketMover> matingPool = new List<RocketMover>();

    public GameObject rocketPrefab;
    public int numAgents = 20;
    public GameObject[] rockets = new GameObject[20];
    GameObject rocketContainer;

    void Start()
    {
        // rockets.add(new RocketMover());
        rocketContainer = new GameObject();
        rocketContainer.transform.position = gameObject.transform.position;
        rocketContainer.transform.rotation = gameObject.transform.rotation;

        for (int i = 0; i < numAgents; i++) {
            GameObject newRocket = Instantiate(rocketPrefab, gameObject.transform.position, gameObject.transform.rotation);
            newRocket.transform.parent = rocketContainer.transform;
            rockets[i] = newRocket;
        }

    }

    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 1) {
            timeElapsed -= 1;
            timer++;
            
            //still running
            if(timer <= lifetime) {
                for (int i = 0; i < rockets.Length; i++) {
                    Vector3 force = rockets[i].GetComponent<RocketMover>().Run();
                    // rockets[i].GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
                    rockets[i].GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

                }
            } else { //new generation
                //fitness
                for (int i = 0; i < rockets.Length; i++) {
                    float d = Vector3.Distance(rockets[i].transform.position, target.transform.position);
                    // Debug.Log(d);
                    rockets[i].GetComponent<RocketMover>().fitness = Mathf.Pow((1.0f/d), 2.0f);
                    // float fit = rockets[i].GetComponent<RocketMover>().fitness;
                    // Debug.Log("Then: " + Mathf.Pow(1.0f/d, 2.0f));
                    // if (bestFitness < fit) {
                    //     bestFitness = fit;
                    // }
                }

                //selection
                for (int i = 0; i < rockets.Length; i++) {
                    float n = rockets[i].GetComponent<RocketMover>().fitness * 100000.0f;
                    // float n = rockets[i].GetComponent<RocketMover>().fitness;
                    if (bestFitness < n) {
                        bestFitness = n;
                    }
                    for (int j = 0; j < n; j++) {
                        matingPool.Add(rockets[i].GetComponent<RocketMover>());
                    }
                }

                //reproduction
                GameObject[] newRockets = new GameObject[20];
                for (int i = 0; i < rockets.Length; i++) {
                    int a = (int)Mathf.Floor(Random.Range(0, matingPool.Count));
                    int b = (int)Mathf.Floor(Random.Range(0, matingPool.Count));
                    DNA child = matingPool[a].dna.Crossover(matingPool[b].dna);
                    child.Mutate(mutationRate);
                    GameObject babyRocket = Instantiate(rocketPrefab, gameObject.transform.position, gameObject.transform.rotation);
                    babyRocket.GetComponent<RocketMover>().dna = child;
                    newRockets[i] = babyRocket;
                }
                for (int i = 0; i < numAgents; i++){
                    Destroy(rockets[i]);
                    rockets[i] = newRockets[i];
                }
                // rockets = newRockets;
                Debug.Log(matingPool.Count);
                matingPool.Clear();

                generation++;
                timer = 0;

                gen.text = "Generation: " + generation.ToString();
                fitness.text = "Best Fitness: " + bestFitness.ToString();
            }


        }
    }
}
