using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int level = 0;
    public Player player;
    public Transform spwanPoint;
    private int missionCounter = 0;
    private int missionPerLevel = 3;

    internal void PlayerWentToBed()
    {
        if(missionCounter == 0)
        {
            OnLevelComplete();
        }
    }

    private void OnLevelComplete()
    {
        throw new NotImplementedException();
    }

    void Start () {
	}

    private void StartNextLevel()
    {
        level++;
        missionCounter = missionPerLevel;
        SpawnBoxes();
        ActivatePlayer();
    }

    private void ActivatePlayer()
    {
        player.gameObject.SetActive(true);
        player.transform.position = spwanPoint.position;
    }

    private void SpawnBoxes()
    {
        
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartNextLevel();
            Debug.Log("Return");
        }
	}
}
