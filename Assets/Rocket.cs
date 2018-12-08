using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidbody;
	AudioSource audioSource;
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 50f;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		Thrust();
		Rotate();
	}

	private void Rotate() {
		rigidbody.freezeRotation = true; // take manual control of rotation

		// var rcsThrust = 100f;
		float rotationThisFrame = rcsThrust * Time.deltaTime;

		if (Input.GetKey(KeyCode.A)){
			transform.Rotate(Vector3.forward * rotationThisFrame);
		} else if (Input.GetKey(KeyCode.D)){
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}

		rigidbody.freezeRotation = false; // resume physics control of rotation
	}

	private void Thrust() {
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.Space)){
			rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
		} else {
			audioSource.Stop();
		}
	}
}
