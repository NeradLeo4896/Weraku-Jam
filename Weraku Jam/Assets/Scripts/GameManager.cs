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
	public float jumpSpeed = 1.0f;
	public int jumpsLeft;
	public int maxJumps = 2;
	public bool isOnGround = false;
	public bool canJump = true;

	//! UI
	public Button startButton;
	public Button retryButton;
	public Button quitButton;

	//! Environment
	public GameObject bg1;
	public GameObject bg2;
	public float environmentSpeed = 1.0f;

	//! Camera
	public GameObject mainCamera;
	public float mainCameraSpeed = 0.5f;
	public float cameraXOffset = 10.0f;

	//! Tilesets
	public List<GameObject> tileSet = new List<GameObject> ();

	//! State machines
	public GameState gameState = GameState.Menu;

	#endregion

	#region Functions

	void EnvironmentUpdate ()
	{
		bg1.transform.position = new Vector3 (bg1.transform.position.x - environmentSpeed * 0.5f * runSpeed, bg1.transform.position.y, bg1.transform.position.z);
		bg2.transform.position = new Vector3 (bg2.transform.position.x - environmentSpeed * 0.5f * runSpeed, bg2.transform.position.y, bg2.transform.position.z);

		if (bg1.transform.position.x < -19.2f)
			bg1.transform.position = new Vector3 (19.2f, bg1.transform.position.y, bg1.transform.position.z);
		if (bg2.transform.position.x < -19.2f)
			bg2.transform.position = new Vector3 (19.2f, bg2.transform.position.y, bg2.transform.position.z);

		for (int i = 0; i < tileSet.Count; i++) {
			tileSet [i].transform.position = new Vector2 (tileSet [i].transform.position.x - environmentSpeed * runSpeed, tileSet [i].transform.position.y);
			if (tileSet [i].transform.position.x < -34.0f) {
				tileSet [i].SetActive (false);
			}
		}
	}

	void PlayerUpdate ()
	{
		//! Controls
		if (Input.GetKeyDown (KeyCode.Space) && isOnGround && canJump) {
			playerAnim.Play ("Jump_1x", -1, 0.0f);
			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, playerRigidBody.velocity.y + jumpSpeed);
			jumpsLeft--;
		} else if (Input.GetKeyDown (KeyCode.Space) && !isOnGround && canJump) {
			canJump = false;
			playerAnim.Play ("Jump_1x", -1, 0.0f);
			playerRigidBody.velocity = new Vector2 (playerRigidBody.velocity.x, playerRigidBody.velocity.y + jumpSpeed);
			jumpsLeft--;
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
		jumpsLeft = maxJumps;
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
	}


}
