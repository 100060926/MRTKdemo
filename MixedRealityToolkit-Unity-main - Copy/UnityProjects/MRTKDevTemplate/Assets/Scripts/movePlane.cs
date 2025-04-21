using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlane : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10, initialDistance = 0;
    public bool moving = false;
    public Vector3 distance, perpendicularDistance, normalizedDistance;
    //Private Variables
    Vector3 center;
    Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3(0,0,0);
        playerRigidbody = GetComponent<Rigidbody>();
        distance = center - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerRigidbody.useGravity = false;

            //Refresh the distance vector every second to get the proper normalized vector(normalizedDistance) below
            distance = center - transform.position;
            normalizedDistance = distance.normalized;

            //We get the perpendicular vector to normalizedDistance, this vector will be in every frame tangent to the circumference that the object
            //attached to this script will travel
            perpendicularDistance = new Vector3(normalizedDistance.x, 0 , normalizedDistance.z);

            //We modify the velocity component of our object matching it with the tangent vector, and multiplied by the distance to make the
            //circumference radio equal to the initial distance they were before starting the movement
            playerRigidbody.velocity = perpendicularDistance * initialDistance;
    }
}
