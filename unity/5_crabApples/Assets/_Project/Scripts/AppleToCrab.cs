using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleToCrab : MonoBehaviour
{
    private float _timer = 0f;
    public float transformationTime = 3.5f;
    // public GameObject crab;
    private bool hasTransformed = false;

    void Start()
    {
        _timer += Random.value * 2;
    }

    void FixedUpdate()
    {
        // transform.position = transform.GetChild(0).position;
        _timer += Time.deltaTime;

        if (!hasTransformed && _timer >= transformationTime) {
            // Destroy(gameObject.transform.GetChild(0).GetChild(1).GetComponent<Rigidbody>());
            // Destroy(gameObject.transform.GetChild(0).GetChild(1).GetComponent<CapsuleCollider>());
            GameObject _apple = gameObject.transform.GetChild(0).gameObject;

            // Instantiate(crab, _apple.transform.position, _apple.transform.rotation);
            // Debug.Log(gameObject.transform.GetChild(0).gameObject.name);
            GameObject _crab = gameObject.transform.GetChild(1).gameObject;
            _crab.SetActive(true);
            Debug.LogFormat("_crab Local:{0}\n_crab World:{1}\nApple Local:{2}\nApple World:{3} ",
                _crab.transform.localPosition,
                _crab.transform.position,
                _apple.transform.localPosition,
                _apple.transform.position);

            // Debug.Log(gameObject.transform.GetChild(0).gameObject.name);
            // Debug.Log(_crab.name);
            // Debug.Debug.LogFormat("");
            // _crab.transform.localPosition = gameObject.transform.GetChild(0).localPosition - gameObject.transform.GetChild(0).GetComponent<Rigidbody>().centerOfMass;
            // _crab.transform.position = _apple.transform.position + (Vector3.Scale(_apple.GetComponent<Rigidbody>().centerOfMass.normalized, _apple.transform.localScale)) ;

            _crab.transform.localPosition = _apple.transform.localPosition;
            // gameObject.transform.position = gameObject.transform.GetChild(0).position;
            // _crab.transform.localPosition = Vector3.zero;


            Debug.LogFormat("_crab Local:{0}\n_crab World:{1}\nApple Local:{2}\nApple World:{3}\nCenter:{4} ",
                _crab.transform.localPosition,
                _crab.transform.position,
                _apple.transform.localPosition,
                _apple.transform.position,
                _apple.GetComponent<Rigidbody>().centerOfMass);
            // Instantiate(_crab, transform.position, transform.localRotation);
            // Instantiate(_crab, transform.parent, true);

            Destroy(gameObject.transform.GetChild(0).gameObject);
            hasTransformed = true;
        }
    }
}
