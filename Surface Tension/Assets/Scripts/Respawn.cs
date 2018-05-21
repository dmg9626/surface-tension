﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour {

    private Vector2 spawnPoint;

	// Use this for initialization
	public void manualRespawn() {
        spawnPoint = GameObject.Find("Spawn Point").transform.position;
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawnPoint;
        }
    }

    public void manualReset()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("prototype");
        }
    }
	
}
