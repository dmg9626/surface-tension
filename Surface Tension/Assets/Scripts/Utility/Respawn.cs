using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour {

    GameObject[] objects;
    Dictionary<GameObject, Vector2> blockPositions = new Dictionary<GameObject, Vector2>();
    Dictionary<GameObject, Quaternion> blockRotations = new Dictionary<GameObject, Quaternion>();

	void Start ()
    {
        objects = GameObject.FindGameObjectsWithTag("Object");

        foreach (GameObject block in objects)
        {
            blockPositions.Add(block, block.transform.position);
            blockRotations.Add(block, block.transform.rotation);
        }
	}
	
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (other.CompareTag("Object"))
        {
            other.gameObject.transform.position = blockPositions[other.gameObject];
            other.gameObject.transform.rotation = blockRotations[other.gameObject];
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
    }
}
