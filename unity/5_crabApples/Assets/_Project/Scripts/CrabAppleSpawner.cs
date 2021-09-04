using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAppleSpawner : MonoBehaviour
{
    public GameObject apple;
    // public GameObject crab;
    private float _timer = 0f;
    private float _spawnRate;
    private SpawnerManager _tree;

    void Start()
    {
        _timer += Random.value * 3;
        _tree = gameObject.transform.parent.GetComponent<SpawnerManager>();
    }

    void FixedUpdate()
    {
        _timer += Time.deltaTime;
        _spawnRate = _tree.spawnRate;
        // Debug.Log(_timer + " " + _spawnRate);

        if (_timer >= _spawnRate) {
            GameObject _apple;
            _apple = Instantiate(apple, transform.position, transform.rotation);
            // _apple.transform.parent = transform.parent;
            _timer = 0f;
            _timer += Random.value * 3;
        }
    }
}
