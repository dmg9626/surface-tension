using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour {

    
    GameObject[] objects;
    //A dictionary to store an objects position in the scene, the the object itself being the key and the position being the value
    Dictionary<GameObject, Vector2> blockPositions = new Dictionary<GameObject, Vector2>();
    
    //Dictionary to store the objects starting rotation. Does this the same way as the previous dictionary
    Dictionary<GameObject, Quaternion> blockRotations = new Dictionary<GameObject, Quaternion>();

	void Start ()
    {
        //The objects GameObject array now holds objects all objects in the scene tagged with "Object"
        objects = GameObject.FindGameObjectsWithTag("Object");

        //Adds another index to the dictionaries for each object
        foreach (GameObject block in objects)
        {
            blockPositions.Add(block, block.transform.position);
            blockRotations.Add(block, block.transform.rotation);
        }
	}
	
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Reloads the scene when the player dies
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //Moves the object back to its original position, with its original rotation, with a (0,0) velocity
        //and its angular velocity to 0.
        else if (other.CompareTag("Object"))
        {
            other.gameObject.transform.position = blockPositions[other.gameObject];
            other.gameObject.transform.rotation = blockRotations[other.gameObject];
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            other.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }
}
