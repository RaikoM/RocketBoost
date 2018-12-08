using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidbody;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput();
		PlayRocketSound();
	}

	private void ProcessInput() {
		if (Input.GetKey(KeyCode.Space)){
			rigidbody.AddRelativeForce(Vector3.up);
		} 
		if (Input.GetKey(KeyCode.A)){
			transform.Rotate(Vector3.forward);
		} else if (Input.GetKey(KeyCode.D)){
			transform.Rotate(-Vector3.forward);
		}
	}

	private void PlayRocketSound() {
		if (Input.GetKeyDown(KeyCode.Space)){
			audioSource.Play();
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			audioSource.Stop();
		}
	}
}
