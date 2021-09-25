using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanControl : MonoBehaviour
{
    [SerializeField] float speedFactor;
    Vector3 startPos;
    public Vector3 inputForce;
    int score;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        score = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        inputForce = new Vector3(x, 0, z);

        GetComponent<Rigidbody>().AddForce(inputForce * speedFactor);

        if (transform.position.y < -10)
        {
            transform.position = startPos;
        }
    }

    private void OnCollisionEnter(Collision other) {
        // Debug.Log(other.transform.name + ": " + other.transform.tag);

        if (other.transform.tag == "goal"){
            transform.position = startPos;
            score += 1;
            Debug.Log("score: " + score);
        }
    }
}
