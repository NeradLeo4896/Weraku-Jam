using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//! State machines
public enum GameState
{
	Menu = 0,
	Play = 1,
	GameOver = 2
}

public class GameManager : MonoBehaviour
{
	#region Declarations

	//! Player
	public GameObject player;
	public Animator playerAnim;
	Rigidbody2D playerRigidBody;
	public float runSpeed = 1.0f;
	public float maxRunSpeed = 10.0f;
	public float runSpeedScaling = 0.0001f;
	public float jumpSpeed = 1.0f;
	public bool isOnGround = false;
	public bool canJump = true;

	//! UI
	public Button startButton;
	public Button retryButton;
	public Button quitButton;

	//! Environment
	public float environmentSpeed = 1.0f;
	public GameObject bg1;
	public GameObject bg2;
	public float bgSpeed = 0.3f;
	public GameObject tsunami;
	public float tsunamiDestinationX = -5f;
	public float tsunamiLerpRate = 0.1f;

	//! Camera
	public GameObject mainCamera;
	public float mainCameraSpeed = 0.5f;
	public float cameraXOffset = 10.0f;

	//! tilesets
	public List<GameObject> tileset = new List<GameObject> ();
	public float randomTilesetOffset = 1.0f;
	int tilesetIndex;
	float spawnPoint = 18f;
	int[] currentTilesetIndex = new int[] { 0, 1 };
	int currentTilesetIndexCounter = 0;

	//! Scoring
	public GameObject coin;
	public Text scoreText;
	public int score = 0;


	//! State machines
	public GameState gameState = GameState.Menu;

	#endregion

	#region Functions

	void UIUpdate ()
	{
		scoreText.text = "" + score;
	}

	void SpawnTilesets (int a)
	{
		//! Choose new tileset
		do {
			tilesetIndex = Random.Range (0, tileset.Count);
		} while(tilesetIndex == currentTilesetIndex [0] || tilesetIndex == currentTilesetIndex [1]);
			
		currentTilesetIndex [currentTilesetIndexCounter] = tilesetIndex;
		currentTilesetIndexCounter++;
		if (currentTilesetIndexCounter >= currentTilesetIndex.Length) {
			currentTilesetIndexCounter = 0;
		}
		tileset [tilesetIndex].SetActive (true);
		tileset [tilesetIndex].transform.position = new Vector2 (spawnPoint + Random.Range (0f, randomTilesetOffset + runSpeed), tileset [a].transform.position.y);
	}

	void EnvironmentUpdate ()
	{
		//! Background
		bg1.transform.position = new Vector3 (bg1.transform.position.x - environmentSpeed * bgSpeed * runSpeed, bg1.transform.position.y, bg1.transform.position.z);
		bg2.transform.position = new Vector3 (bg2.transform.position.x - environmentSpeed * bgSpeed * runSpeed, bg2.transform.position.y, bg2.transform.position.z);

		if (bg1.transform.position.x < -19.2f)
			bg1.transform.position = new Vector3 (19.2f - runSpeed, bg1.transform.position.y, bg1.transform.position.z);
		if (bg2.transform.position.x < -19.2f)
			bg2.transform.position = new Vector3 (19.2f - runSpeed, bg2.transform.position.y, bg2.transform.position.z);

		//! Tilesets
		for (int i = 0; i < tileset.Count; i++) {
			//! Moving
			if (tileset [i].activeSelf == true) {
				tileset [i].transform.position = new Vector2 (tileset [i].transform.position.x - environmentSpeed * runSpeed, tileset [i].transform.position.y);
			}
			//! When out of bounds
			if (tileset [i].transform.position.x < -34.0f) {
				tileset [i].SetActive (false);
				tileset [i].transform.position = new Vector2 (30f, tileset [i].transform.position.y);
				SpawnTilesets (i);
			}
		}

		//! Coin
		coin.transform.position = new Vector2 (coin.transform.position.x - environmentSpeed * runSpeed, coin.transform.position.y);
		if (coin.transform.position.x < -10f) {
			coin.SetActive (true);
			coin.transform.position = new Vector2 (10f, Random.Range (0f, 2.0f));
		}

		//! Speed up
		if (runSpeed < maxRunSpeed) {
			runSpeed += runSpeedScaling;
		}

		//! Others
		tsunami.transform.position = Vector2.Lerp (tsunami.transform.position, new Vector2 (tsunamiDestinationX, tsunami.transform.position.y), tsunamiLerpRate);

	}

	void PlayerUpdate ()
	{
		//! Controls
		if (Input.GetKeyDown (KeyCode.Space) && isOnGround && canJump) {
			playerAnim.Play ("Jump_1x", -1, 0.0f);
			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, playerRigidBody.velocity.y + jumpSpeed);
		} else if (Input.GetKeyDown (KeyCode.Space) && !isOnGround && canJump) {
			canJump = false;
			playerAnim.Play ("Jump_1x", -1, 0.0f);
			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, playerRigidBody.velocity.y + jumpSpeed);
		}

		//! Game state to player state
		switch (gameState) {
		case GameState.GameOver:
			player.SetActive (false);
			break;
		}

		//! Game over state
		if (player.transform.position.y < -13f) {
			gameState = GameState.GameOver;
		}
	}

	void CameraUpdate ()
	{
		if (player.activeSelf && mainCamera.transform.position.y > -10f)
			mainCamera.transform.position = Vector2.Lerp (mainCamera.transform.position, new Vector2 (player.transform.position.x + cameraXOffset, player.transform.position.y), mainCameraSpeed);

	}

	#endregion

	void Awake ()
	{
		startButton.onClick.AddListener (() => {
			gameState = GameState.Play;
			startButton.gameObject.SetActive (false);
			playerAnim.Play ("Run_1x");
		});
		retryButton.onClick.AddListener (() => {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		});
		quitButton.onClick.AddListener (() => {
			Application.Quit ();
		});
		playerAnim = player.GetComponent<Animator> ();
		playerRigidBody = player.GetComponent<Rigidbody2D> ();		 
	}

	// Use this for initialization
	void Start ()
	{
		for (int i = 2; i < tileset.Count; i++) {
			tileset [i].SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		switch (gameState) {
		case GameState.Menu:
			PlayerUpdate ();
			break;
		case GameState.Play:
			EnvironmentUpdate ();
			CameraUpdate ();
			PlayerUpdate ();
			break;
		case GameState.GameOver:
			retryButton.gameObject.SetActive (true);
			quitButton.gameObject.SetActive (true);
			break;
		}
		UIUpdate ();
	}


}
