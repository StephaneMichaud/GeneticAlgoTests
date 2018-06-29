
using UnityEngine;

public class MovementCamera : MonoBehaviour {

    public Transform objetSuivi;
    // Use this for initialization
    public Vector3 offset;
	// Update is called once per frame
	void Update ()
    {
        transform.position = objetSuivi.position;
        transform.rotation = objetSuivi.rotation;
        transform.position = objetSuivi.position + offset;
	}
}
