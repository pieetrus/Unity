using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;


    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip succes;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem succesParticles;
    [SerializeField] ParticleSystem deathParticles;

    enum State { Alive, Dying, Transcending}

    Rigidbody rigidbody;
    private AudioSource audioSource; 
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); 
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

    }


    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) //zignoruj kolejne kolizje gdy już zginąłeś
            return;


        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccesSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        deathParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        state = State.Dying;
        Invoke("LoadFirstLevel", levelLoadDelay);
    }
    private void StartSuccesSequence()
    {
        state = State.Transcending;
        succesParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(succes);
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying) //żeby się nie nakładały
            audioSource.PlayOneShot(mainEngine);

        mainEngineParticles.Play();
    }
    private void RespondToRotateInput()
    {
        rigidbody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))    
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
        rigidbody.freezeRotation = false; // resume physics control of rotation

    }


}
