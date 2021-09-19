using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stampede : MonoBehaviour
{
    public GameObject cowPrefab;
    public float width = 20f;
    public float spawnRate = .2f;
    public Vector3 movement = new Vector3(0, 0, 0.05f);
    float _timer = 0;
    List<GameObject> cows = new List<GameObject>();
    Quaternion rot = new Quaternion();
    void Start()
    {
        rot = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    void FixedUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnRate)
        {
            Vector3 adjustedPos = Quaternion.Inverse(rot) * new Vector3(Random.Range(-width, width), 0, 0);
            //Vector3 adjustedPos = rot * new Vector3(Random.Range(-width, width), 0, 0);
            //could have just used localPos?
            GameObject cow = Instantiate(cowPrefab, transform.position + adjustedPos, transform.rotation);
            cows.Add(cow);
            
            _timer -= spawnRate;
        }

        foreach(GameObject c in cows)
        {
            //c.transform.localPosition += movement;

            c.transform.position += rot * movement;

        }
    }
}
