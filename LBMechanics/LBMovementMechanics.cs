using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	[CreateAssetMenu(fileName = "NewGroundMovementMechanic", menuName = "LBMechanics/GroundMovementMechanic")]
	public class LBGroundMovementMechanic: LBTransitionMechanic
	{
		Animator animator;
		Rigidbody rb;

		public Vector3 MovementDir;
		public float MovementSpeed;

		public string MovementAnim;
		public float AnimBlendTime = 0.1f;

		/***************************************** Init stuff *****************************************/

		public override void InitMechanic()
		{
			base.InitMechanic ();

			//mechanicname="GroundMovementMechanic";
		}

		public override void LockMechanic (GameObject p)
		{
			base.LockMechanic(p);

			animator = parent.GetComponent<Animator> ();
			rb = parent.GetComponent<Rigidbody> ();
			//mechexec = parent.GetComponent<LBMechanicsExecutor> ();
		}

		/************************************** Mechanic-related stuff *******************************/

		public override void Tick()
		{
			//Debug.Log (mechanicname + ": Hello!");
			PerformMovement();
		}

		public override bool ActivateMechanic()
		{
			base.ActivateMechanic ();

			//animator.Play (MovementAnim);
			animator.CrossFade(MovementAnim,1);

			return true;
		}

		/************************************** Game logic ******************************************/

		void PerformMovement()
		{
			rb.MovePosition(rb.position + MovementDir * MovementSpeed);
		}
	}
}