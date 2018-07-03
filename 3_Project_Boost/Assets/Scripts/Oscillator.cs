using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;

    //TODO remove from inspector later
    [Range(0, 1)][SerializeField] float movementFactor;  //0 for not moved, 1 for fully moved
    [SerializeField] private float period = 2f;
    Vector3 startingPos;

    [HideInInspector]
    const float tau = 6.28f;

	// Use this for initialization
	void Start ()
    {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //oscilation
        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles);

        movementFactor = rawSinWave / 2 + 0.5f;

        //set movement factor
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
