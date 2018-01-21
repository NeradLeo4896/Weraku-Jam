using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameManager gmScript;

	void Awake ()
	{
		
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Platform") {
			gmScript.isOnGround = true;
			gmScript.canJump = true;
			switch (gmScript.gameState) {
			case GameState.Menu:
				gmScript.playerAnim.Play ("Idle_1x");
				break;
			case GameState.Play:
				gmScript.playerAnim.Play ("Run_1x");
				break;
			}
		}
	}

	void OnCollisionExit2D (Collision2D col)
	{
		if (col.gameObject.tag == "Platform") {
			gmScript.isOnGround = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Coin") {
			gmScript.score++;
			gmScript.coin.SetActive (false);
		}
	}
}
