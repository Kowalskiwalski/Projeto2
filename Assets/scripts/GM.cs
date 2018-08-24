using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {

	public static GM instance = null;

	public float yMinLive = -6.7f;

	public Transform spawnPoint;

	public GameObject playerPrefab; 

	PlayerCtrl player;

	public  float  timeToRespawn = 2f;

	public float maxTime = 120f;

	bool timerOn = true;

	float timeLeft;

	public UI ui;

	GameData data = new GameData();

	void Awake(){
		if (instance == null){
			instance = this;
		}
	}

	void Start () {
		if (player == null){
			RespawnPlayer();
		}

		timeLeft = maxTime;
	}
	
	void Update () {
		if(player == null){
			GameObject obj = GameObject.FindGameObjectWithTag("Player");
			if(obj != null){
				player = obj.GetComponent<PlayerCtrl>();
			}
		}
		UpdateTimer();
		DisplayHudData();
	}

	public void RestartLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ExitToMainMenu(){
		LoadScene("MainMenu");
	}

	public void CloseApp(){
		Application.Quit();
	}

	public void LoadScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}

	void UpdateTimer(){
		if(timerOn){
			timeLeft = timeLeft - Time.deltaTime;
			if (timeLeft <= 0){
				timeLeft = 0;
				ExpirePlayer();
			}
		}
	}

	void DisplayHudData(){
		ui.hud.txtCoinCount.text = "x " + data.coinCount;
		ui.hud.txtLifeCount.text = "x " + data.lifeCount;
		ui.hud.txtTimer.text = timeLeft.ToString("F1");
	}

	public void IncrementCoinCount(){
		data.coinCount ++;
	}

	public void RespawnPlayer(){
		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
		
	}

	public void decrementLives(){
		data.lifeCount --;
	}

	public void KillPlayer(){
		if (player != null) {
			Destroy(player.gameObject);
			decrementLives();
			if(data.lifeCount > 0){
				Invoke("RespawnPlayer", timeToRespawn);
			}
			else {
				GameOver();
			}
		}
	}
	
	public void ExpirePlayer(){
		if (player != null) {
			Destroy(player.gameObject);
		}
		GameOver();
	}
	void GameOver(){
		timerOn = false;
		ui.gameOver.gameOverPanel.SetActive(true);
	}

	public void LevelComplete(){
		Destroy(player.gameObject);
		timerOn = false;
		ui.levelComplete.LevelCompletePanel.SetActive(true);
	}
}
