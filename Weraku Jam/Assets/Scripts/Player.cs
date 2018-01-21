using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	#region Declarations

	Animator anim;
	Rigidbody2D rb;
	public float jumpSpeed = 1.0f;

	#endregion

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y + jumpSpeed);
		}
	}
}
