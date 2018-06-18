using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	[CreateAssetMenu(fileName = "NewGroundMovementMechanic", menuName = "LBMechanics/GroundMovementMechanic")]
	public class LBGroundMovementMechanic: LBTransitionMechanic
	{
		Rigidbody rb;

		public bool IsTrasitive;

		public Vector3 MovementDir;
		public float MovementSpeed;

		/***************************************** Init stuff *****************************************/

		public override void InitMechanic()
		{
			base.InitMechanic ();

			//mechanicname="GroundMovementMechanic";
		}

		public override void LockMechanic (GameObject p)
		{
			base.LockMechanic(p);

			rb = parent.GetComponent<Rigidbody> ();
			//mechexec = parent.GetComponent<LBMechanicsExecutor> ();
		}

		/************************************** Mechanic-related stuff *******************************/

		public override void Tick()
		{
			//Debug.Log (mechanicname + ": Hello!");
			if (IsTrasitive)
				base.Tick ();

			PerformMovement();
		}

		public override bool ActivateMechanic()
		{
			base.ActivateMechanic ();

			//animator.Play (MovementAnim);
			animator.CrossFade(Animation,AnimBlendTime);

			return true;
		}

		/************************************** Game logic ******************************************/

		void PerformMovement()
		{
			rb.MovePosition(rb.position + MovementDir * MovementSpeed);
		}
	}
}