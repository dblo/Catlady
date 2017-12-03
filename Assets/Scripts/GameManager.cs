using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level = 0;
    public Player player;
    public GameObject catPrefab;
    public Transform spwanPoint;
    private int missionCounter = 0;
    public int missionPerLevel = 2;
    private Mission mission;
    public NightTimePanel nightPanel;
    public GameOverPanel gameOverPanel;
    private GameBoard gameBoard;
    public int boxesAddedPerLevel;
    public int lifes;
    public Text lifesText;
    private float playTime = 0; //Not incremented in menus
    private bool inMenus = true;

    void Awake()
    {
        nightPanel.gameObject.SetActive(true);
        mission = GetComponent<Mission>();
        gameBoard = GetComponent<GameBoard>();
    }

    void Start()
    {
        SleepScreen();
        UpdateLifesText();
    }

    private void Update()
    {
        if (!inMenus)
        {
            playTime += Time.deltaTime;
        }
    }

    internal void PlayerReachedMissionPoint()
    {
        if (mission.MissionCompleted())
        {
            missionCounter--;
            NewMission();
        }
    }

    internal bool TimerExpired()
    {
        lifes--;
        UpdateLifesText();
        if (lifes > 0)
            return true;

        GameOver();
        return false;
    }

    internal int GetMissionTime()
    {
        if (level > 3)
            return 8;
        else if (level > 1)
            return 12;
        return 20;
    }

    private void GameOver()
    {
        inMenus = true;
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.Show(level, playTime);
    }

    internal void PlayerWentToBed()
    {
        if (missionCounter == 0)
        {
            mission.MissionCompleted();
            OnLevelComplete();
        }
    }

    private void OnLevelComplete()
    {
        inMenus = true;
        player.gameObject.SetActive(false);
        SleepScreen();
    }

    private void SleepScreen()
    {
        nightPanel.gameObject.SetActive(true);
        nightPanel.Show(level);
    }

    private void UpdateLifesText()
    {
        lifesText.text = "Lifes: " + lifes.ToString();
    }

    internal void StartNextLevel()
    {
        level++;
        missionCounter = missionPerLevel;
        gameBoard.AddBoxes(boxesAddedPerLevel);
        ActivatePlayer();
        NewMission();
        inMenus = false;

        if (level > 1) //No cat on lvl 1
        {
            Vector3 spawnPos = gameBoard.GetRandomWorldCoordWithinOuterWalls();
            Instantiate(catPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void NewMission()
    {
        //If enabling wait, handle player reaching bed before new mission is given
        //yield return new WaitForSeconds(1);
        if (missionCounter <= 0)
            mission.BedMission();
        else
        {
            mission.NewMission();
        }
    }

    private void ActivatePlayer()
    {
        player.gameObject.SetActive(true);
        player.transform.position = spwanPoint.position;
    }
}