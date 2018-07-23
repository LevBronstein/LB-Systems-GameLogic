using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	public class LBPlayerController : MonoBehaviour 
	{
		LBCharacterController ctrl;

		// Use this for initialization
		void Start () 
		{
			ctrl = GetComponent<LBCharacterController> ();
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (Input.GetKey (KeyCode.UpArrow)) 
				ctrl.SetControlValue ("Movement_Speed", 2.0f);
			else 
				ctrl.SetControlValue ("Movement_Speed", 0.0f);
			
			if (Input.GetKey (KeyCode.LeftArrow))
				ctrl.SetControlValue ("Movement_Direction", new Vector3 (1.0f, 0, 0));
			else 
			{
				if (Input.GetKey (KeyCode.RightArrow)) 
					ctrl.SetControlValue ("Movement_Direction", new Vector3(-1.0f,0,0));
				else
					ctrl.SetControlValue ("Movement_Direction", new Vector3(0,0,0));
			}
		}
	}
}