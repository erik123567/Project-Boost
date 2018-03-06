using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audio;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;
    public GameObject rocketPrefab;
    public GameObject launchPad;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    void ProcessInput()
    {
        Thrust();
        Rotate();
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
 
        float rotationThisFrame = Time.deltaTime * rcsThrust;
        if (Input.GetKey(KeyCode.D))
        {
            

            // print("going right");
            transform.Rotate(Vector3.back * rotationThisFrame );

        }
        else if (Input.GetKey(KeyCode.A))
        {
           
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = true; // resume physics rotation
    }

    private void Thrust()
    {
        float thrustThisFrame = Time.deltaTime * mainThrust;
        if (Input.GetKey(KeyCode.Space))
        {
            //  print("thrusting");
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            else
            {
                audio.Stop();
            }



        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collider");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("OK");
                break;
            case "Fuel":
                Debug.Log("got  fuel");
                Destroy(collision.gameObject);
                break;
            default:
                Debug.Log("dead");
                Instantiate(rocketPrefab, launchPad.transform);
                Destroy(gameObject);
                
                Debug.Log("here");
                break;
        }
    }
}
