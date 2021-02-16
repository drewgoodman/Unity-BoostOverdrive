using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 110f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip levelLoadSFX;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem levelLoadParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    GameObject musicSource;

    enum State { Entering, Active, Dying, Transcending };
    State state = State.Entering;
    bool isThrusting = false;
    bool isGrounded = true;
    bool collisionsAreEnabled = true;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Active)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugInput();
        }

    }

    void FixedUpdate()
    {
        if (isThrusting && state == State.Active)
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        }
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    void ApplyThrust()
    {
        isThrusting = true;
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
            mainEngineParticles.Play();
        }
    }

    void StopApplyingThrust()
    {
        isThrusting = false;
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    void RespondToRotateInput()
    {

        // if (isGrounded) { return; } // no rotating on the launch pad!

        rigidBody.angularVelocity = Vector3.zero;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Active || !collisionsAreEnabled) { return; } //ignore collisions

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                isGrounded = true;
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                print("Ouch!");
                break;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (state != State.Active) { return; }
        if (collision.gameObject.tag == "Friendly")
        {
            isGrounded = false;
        }
    }

    void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelLoadSFX);
        levelLoadParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void ReadyToPlay() // called from camera once finished panning
    {
        musicSource = GameObject.FindGameObjectWithTag("MusicController");
        musicSource.GetComponent<MusicController>().PlayMusic();
        state = State.Active;
    }

    void RespondToDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsAreEnabled = !collisionsAreEnabled;
        }
    }

}
