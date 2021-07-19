using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public static UnityEvent OnStart = new UnityEvent();
    public static UnityEvent<int> OnEnd = new UnityEvent<int>();

    public string playerOneName, playerTwoName;
    public Text whichOne, gaugeTextOne, gaugeTextTwo;

    public GameObject root, inGame, gameOver;

    private void Start()
    {
        root.SetActive(true);
        inGame.SetActive(false);
        gameOver.SetActive(false);

        OnEnd.AddListener(EndGame);
        gaugeTextOne.text = playerOneName;
        gaugeTextTwo.text = playerTwoName;
    }

    public void StartGame()
    {
        OnStart.Invoke();
        root.SetActive(false);
        inGame.SetActive(true);
        gameOver.SetActive(false);
    }

    public void EndGame(int whoLost)
    {
        if (whoLost == 2)
            whichOne.text = playerOneName;
        else whichOne.text = playerTwoName;

        root.SetActive(false);
        inGame.SetActive(false);
        gameOver.SetActive(true);
    }

    public void SetPlayerOne(string name)
    {
        playerOneName = name;
        gaugeTextOne.text = name;
    }

    public void SetPlayerTwo(string name)
    {
        playerTwoName = name;
        gaugeTextTwo.text = name;
    }
}
