using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audio;
    enum State { Alive, Dying, Trancending};
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;
    public GameObject rocketPrefab;
    public GameObject launchPad;
    State state = State.Alive;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(state == State.Alive)
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
        if(state != State.Alive)
        {
            return;
        }
        print("collider");
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Fuel":
                Debug.Log("got  fuel");
                Destroy(collision.gameObject);
                break;
            case "Finish":
                state = State.Trancending;
                Invoke("LoadNextScene", 1f); // parametericze
                break;
            default:
                state = State.Dying;
                Invoke("ReloadCurrent", 1f);
                Debug.Log("here");
                break;
        }
    }

    private void ReloadCurrent()
    {

        SceneManager.LoadScene(0);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
