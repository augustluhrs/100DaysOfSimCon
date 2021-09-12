using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public Mesh mesh;

    public GameObject prefab;

    private float _timer;

    //from Brackey's video https://www.youtube.com/watch?v=8zo5a_QvJtk
    void Start()
    {
        /*
        gameObject.transform.GetChild(0).gameObject.SetActive(false);

        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Instantiate(prefab, vertices[i] + transform.position, Quaternion.identity);
        }
        */
    }

    private void FixedUpdate()
    {
        _timer += Time.deltaTime;
        
        //if (_timer >= 2.5)
        if (_timer >= 2)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);

            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                Instantiate(prefab, vertices[i] + transform.position, Quaternion.identity);
            }

            _timer = 0;
        }
    }

}
