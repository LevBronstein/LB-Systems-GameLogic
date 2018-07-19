using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A mechanic, which is able to translate (move) gameobject*/
namespace LBMechanics
{
	public abstract class LBMovementMechanic: LBTransitionMechanic
	{
		protected Rigidbody rb;

		public bool IsTrasitive;

		public Vector3 MovementDir;
		public float MovementSpeed;
		public float Acceleration;

		public override void LockMechanic (GameObject p)
		{
			base.LockMechanic(p);

			rb = parent.GetComponent<Rigidbody> ();
			//mechexec = parent.GetComponent<LBMechanicsExecutor> ();
		}

		public override void Tick()
		{
			//Debug.Log (mechanicname + ": Hello!");
			if (IsTrasitive)
				base.Tick ();

			PerformMovement();
		}

		public Vector3 Direction
		{
			get
			{
				return MovementDir;
			}
			set 
			{
				MovementDir = value;
			}
		}

		public float Speed
		{
			get
			{
				return MovementSpeed;
			}
			set 
			{
				MovementSpeed = value;
			}
		}

		protected virtual void PerformMovement()
		{}
	}

	/*A specialized mechanic for handling ground movment*/
	[CreateAssetMenu(fileName = "NewGroundMovementMechanic", menuName = "LBMechanics/GroundMovementMechanic")]
	public class LBGroundMovementMechanic: LBMovementMechanic
	{
		public override bool ActivateMechanic()
		{
			base.ActivateMechanic ();

			//animator.Play (MovementAnim);
			animator.CrossFade(Animation,AnimBlendTime);

			return true;
		}

		protected override void PerformMovement()
		{
			float f;
			float t;

			t = Acceleration/(MovementSpeed - rb.velocity.magnitude);

			f = Mathf.Lerp (rb.velocity.magnitude, MovementSpeed, t);
			rb.velocity = MovementDir * f;
		}
	}

	[CreateAssetMenu(fileName = "NewAirMovementMechanic", menuName = "LBMechanics/AirMovementMechanic")]
	public class LBAirMovementMechanic: LBMovementMechanic
	{
		public override bool ActivateMechanic()
		{
			base.ActivateMechanic ();

			//animator.Play (MovementAnim);
			animator.CrossFade(Animation,AnimBlendTime);

			return true;
		}
			
		protected override void PerformMovement()
		{
			rb.MovePosition(rb.position + MovementDir * MovementSpeed);
		}
	}
		
/*****************************************Transition mechanics**********************************************************/

	/*A basic transition mechanic for handling movement transitions*/
	[CreateAssetMenu(fileName = "NewMovementTransitionMechanic", menuName = "LBMechanics/MovementTransitionMechanic")]
	public class LBMovementTransitionMechanic: LBTransitionMechanic
	{
		protected Rigidbody rb;

		public bool CheckIsOnGround;
		public bool CheckIsInAir;
		public bool CheckSpeedMagnitude;
		public float MagnitudeLimit;
	
		public override void LockMechanic (GameObject p)
		{
			base.LockMechanic(p);

			rb = parent.GetComponent<Rigidbody> ();

			if (rb == null)
				Debug.LogWarning (MechanicName + ": Cannot lock mechanic -- rigid body not found!");
		}

		public override bool CanActivateMechanic()
		{
			if (base.CanActivateMechanic () == false)
				return false;

			if (CheckIsOnGround) 
			{
				return CheckBasePlacement ();
			}
				
			if (CheckIsInAir) 
			{
				return !CheckBasePlacement ();
			}

			if (CheckSpeedMagnitude) 
			{
				return false;
			}

			return false;
		}

		bool CheckBasePlacement()
		{
			Collider c;
			Ray r;
			RaycastHit hit;

			c = parent.GetComponent<Collider>();

			if (c == null)
				return false;

			r = new Ray (c.bounds.center, Vector3.down);

			Debug.DrawRay (r.origin, r.direction, Color.green);

			if (Physics.Raycast (r.origin, r.direction, out hit, c.bounds.extents.y+0.05f)) 
			{
				//Debug.Log (hit.transform.gameObject.name);
				if (hit.transform.gameObject.name != parent.name)
					return true;
			}

			return false;
		}

		bool CheckSpeedValue()
		{
			if (rb.velocity.magnitude <= MagnitudeLimit)
				return true;

			return false;
		}			
	}
}