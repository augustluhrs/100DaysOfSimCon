using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullSpawnManager : MonoBehaviour
{
    public GameObject skullPrefab;
    public float spawnRate = 5f;
    private float _time = 0f;

    void FixedUpdate()
    {
        _time += Time.deltaTime;

        if (_time >= spawnRate) {

            Instantiate(skullPrefab, transform.position, transform.rotation);

            _time -= spawnRate;
        }
    }
}
