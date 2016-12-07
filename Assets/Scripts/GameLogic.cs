using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    int score;
    public Text scoreText;
    public Text highscoreText;
    public GameObject endGamePanel;
    public GridSystem gridSystem;

    List<Cube.CubeColor> cubeColorList = new List<Cube.CubeColor>();

    void Start () {
        score = 0;
        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        endGamePanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddScore(int x)
    {
        score += x;
        scoreText.text = "Score: " + score;
    }

    public void EndGame()
    {
        endGamePanel.SetActive(true);

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
         
            PlayerPrefs.SetInt("Highscore", score);        
        }
        
    }

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void OnApplicationQuit()
    {
        
    }


}
