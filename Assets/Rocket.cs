using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidbody;
	AudioSource audioSource;
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 50f;
	[SerializeField] float levelLoadDelay = 2f;
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip levelLoad;
	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem deathParticles;
	[SerializeField] ParticleSystem levelLoadParticles;

	enum State { Alive, Dying, Transcending };
	State state = State.Alive;
	bool collisionsDisabled = false;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Alive) {
			RespondToThrustInput();
			RespondToRotateInput();
		}
		if (Debug.isDebugBuild) {
			RespondToDebugKeys();
		}
		
	}

	private void OnCollisionEnter(Collision collision) {
		
		if (state != State.Alive || collisionsDisabled) { 
			return; }

		switch (collision.gameObject.tag){
			case "Friendly": 
				break;
			case "Finish":
				StartSuccessSequence();
				break;
			default:
				StartDeathSequence();
				break;
		}
	}

	private void StartDeathSequence() {
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(death);
		deathParticles.Play();
		Invoke("LoadFirstLevel", levelLoadDelay);
	}

	private void StartSuccessSequence() {
		state = State.Transcending;
		audioSource.Stop();
		audioSource.PlayOneShot(levelLoad);
		levelLoadParticles.Play();
		Invoke("LoadNextLevel", levelLoadDelay);
	}

	private void LoadNextLevel() {
		SceneManager.LoadScene(1);
	}

	private void LoadFirstLevel() {
		SceneManager.LoadScene(0);
	}

	private void RespondToRotateInput() {
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

	private void RespondToThrustInput() {
		float thrustThisFrame = mainThrust * Time.deltaTime;
		if (Input.GetKey(KeyCode.Space)){
			ApplyThrust(thrustThisFrame);
		} else {
			audioSource.Stop();
			mainEngineParticles.Stop();
		}
	}

	private void ApplyThrust(float thrustThisFrame) {
		rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(mainEngine);
		}
		mainEngineParticles.Play();
		
	}

	private void RespondToDebugKeys() {
		AdvanceToNextLevel();
		DisableCollider();
	}

	private void AdvanceToNextLevel() {
		if (Input.GetKeyDown(KeyCode.L)){
			Invoke("LoadNextLevel", 0f);
		}
	}

	private void DisableCollider() {
		if (Input.GetKeyDown(KeyCode.C)){
			collisionsDisabled = !collisionsDisabled;
		}
	}
}
