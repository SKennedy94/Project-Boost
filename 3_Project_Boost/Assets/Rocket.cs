using UnityEngine;

public class Rocket : MonoBehaviour {
    #region Variables
    Rigidbody rigidBody;
    AudioSource audioSource;
    #endregion Variables

    #region Messages
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}
    #endregion Messages

    #region Methods
    //handles inputs
    private void ProcessInput()
    {
        ApplyThrust();
        RotateShip();
    }
    //apply relatice force if space is held
    private void ApplyThrust()
    {
        if (Input.GetKey(KeyCode.Space)) //can trhust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up);

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
    //rotates the ship left or right if a or d is pressed
    private void RotateShip()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }
    #endregion Methods
}
