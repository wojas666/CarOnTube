using UnityEngine;
using System.Collections;

public class TubeControl : MonoBehaviour {
    [Tooltip("If the larger value, the lower sensitivity of the mouse.")]
    public float mouseSensivity;
    
    private bool dragActive;

    private float x_m;
    private float x_c;
    private float delta_x;

    private Vector3 rotation;

	// Use this for initialization
	void Start () {
        dragActive = false;
        x_m = 0f;
        x_c = 0f;
        delta_x = 0f; 
	}
	
	// Update is called once per frame
	void Update () {

	    if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            x_m = Input.mousePosition.x;
            dragActive = true;
        }

        if (dragActive)
        {
            x_c = Input.mousePosition.x;
            delta_x = (x_c - x_m)/2f;

            x_m = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            // Reset all start and end mouse possition to stop 
            // scrollview position changed.
            dragActive = false;
            x_m = 0f;
            x_c = 0f;
            delta_x = 0f;
        }

        rotation.x = rotation.x + (delta_x/mouseSensivity);

        foreach(Transform child in transform)
        {
            Quaternion rotations = Quaternion.Euler(rotation);
            child.transform.rotation = rotations * child.transform.rotation;
        }

        rotation = new Vector3(0, 0, 0);
    }
}
