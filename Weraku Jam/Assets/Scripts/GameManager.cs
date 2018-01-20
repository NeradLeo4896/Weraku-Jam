using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	#region Declarations

	//public GameObject player;
	public GameObject bg1;
	public GameObject bg2;

	public float bgSpeed = 1.0f;

	#endregion

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		bg1.transform.position = new Vector3(bg1.transform.position.x - bgSpeed, bg1.transform.position.y, bg1.transform.position.z);
		bg2.transform.position = new Vector3(bg2.transform.position.x - bgSpeed, bg2.transform.position.y, bg2.transform.position.z);

		if(bg1.transform.position.x < -19.2f)
			bg1.transform.position = new Vector3(19.2f, bg1.transform.position.y, bg1.transform.position.z);
		if(bg2.transform.position.x < -19.2f)
			bg2.transform.position = new Vector3(19.2f, bg2.transform.position.y, bg2.transform.position.z);;
		
	}
	
}
