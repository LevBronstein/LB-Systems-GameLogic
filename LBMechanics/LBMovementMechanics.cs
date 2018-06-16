using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	[CreateAssetMenu(fileName = "NewGroundMovementMechanic", menuName = "LBMechanics/GroundMovementMechanic")]
	public class LBGroundMovementMechanic: LBMechanicBase
	{
		LBMechanicsExecutor mechexec;
		Animator animator;
		Rigidbody rb;

		public string[] SwitchesFrom;

		public Vector3 MovementDir;
		public float MovementSpeed;

		public string MovementAnim;

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
			mechexec = parent.GetComponent<LBMechanicsExecutor> ();
		}

		public override void Tick()
		{
			//Debug.Log (mechanicname + ": Hello!");
			PerformMovement();
		}

		public override bool ActivateMechanic()
		{
			mechexec.DeactivateAllGroup (this);

			base.ActivateMechanic ();

			animator.Play (MovementAnim);

			return true;
		}

		void PerformMovement()
		{
			rb.MovePosition(rb.position + MovementDir * MovementSpeed);
		}
	}
}