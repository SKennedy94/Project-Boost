using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
#region Variables
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip Death;
    [SerializeField] AudioClip Win;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem winParticles;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;
#endregion Variables

#region Messages
    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        RespondToThrustInput();
        RespondToRotateInput();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                StartWinSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    #endregion Messages

#region Methods
    //check if thrust input is pressed if so call applythrust
    private void RespondToThrustInput()
    {
        if (state == State.Alive)
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
    }
    //apply relatice force in up direction
    private void ApplyThrust()
    {
        //can thrust while rotating

        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
    //check if a or d is pressed, if so call rotate ship
    private void RespondToRotateInput()
    {
        if (state == State.Alive)
        {
            RotateShip();
        }
    }
    //rotates the ship left or right
    private void RotateShip()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
    //loads the next scene based on the level variable
    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); //allow for more than two levels
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0); //allow for more than two levels
    }
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        mainEngineParticles.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(Death);
        Invoke("LoadFirstLevel", 1f);
    }
    private void StartWinSequence()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
        winParticles.Play();
        audioSource.PlayOneShot(Win);
        state = State.Transcending;
        Invoke("LoadNextScene", 1f);
    }
#endregion Methods
}
