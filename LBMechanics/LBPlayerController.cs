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

		Vector3 GetMovementDirection()
		{
			Vector3 v;

			v = Vector3.zero;

			if (Input.GetKey (KeyCode.UpArrow))
				v = v + Vector3.forward;

			if (Input.GetKey (KeyCode.LeftArrow))
				v = v + Vector3.left;

			if (Input.GetKey (KeyCode.RightArrow))
				v = v + Vector3.right;

			if (Input.GetKey (KeyCode.DownArrow))
				v = v + Vector3.back;

			return v;
		}

		// Update is called once per frame
		void Update () 
		{
			Vector3 v;

			v = GetMovementDirection ();

			ctrl.SetControlValue ("Movement_Direction", v.normalized);

			ctrl.SetControlValue ("Movement_Speed", v.magnitude);
		}
	}
}