using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;

    //TODO remove from inspector later
    float movementFactor;  //0 for not moved, 1 for fully moved
    [SerializeField] private float period = 2f;
    private float cycles;

    Vector3 startingPos;

    [HideInInspector]
    const float tau = 6.28f;

	// Use this for initialization
	void Start ()
    {
        startingPos = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        //oscilation
        //protect against period is zero
        if (period >= Mathf.Epsilon)
        {
            cycles = Time.time / period;
        }
        else
        {
            print("cannot have a period of zero");
            return;
        }

        float rawSinWave = Mathf.Sin(cycles);

        movementFactor = rawSinWave / 2 + 0.5f;

        //set movement factor
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
