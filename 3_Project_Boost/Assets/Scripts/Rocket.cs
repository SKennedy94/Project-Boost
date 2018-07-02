using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    #region Variables
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 100f;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    public int level = 0;
    #endregion Variables

    #region Messages
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        //SceneManager.LoadScene(0);
    }
	
	// Update is called once per frame
	void Update () {
        ApplyThrust();
        RotateShip();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("you are safe");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                audioSource.Pause();
                level = 0;
                Invoke("LoadNextScene", 2f);
                break;
        }
    }
    #endregion Messages

    #region Methods
    //apply relatice force if space is held
    private void ApplyThrust()
    {
        if (state == State.Alive)
        {
            if (Input.GetKey(KeyCode.Space)) //can trhust while rotating
            {

                rigidBody.AddRelativeForce(Vector3.up * mainThrust);

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Pause();
            }
        }
    }
    //rotates the ship left or right if a or d is pressed
    private void RotateShip()
    {
        if (state == State.Alive)
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
    }
    //loads the next scene based on the level variable
    private void LoadNextScene()
    {
        level++;
        state = State.Alive;

        if (level > 1)
        {
            print("beat all levels");
            level = 0;
        }

        SceneManager.LoadScene(level); //allow for more than two levels
    }
    #endregion Methods
}
