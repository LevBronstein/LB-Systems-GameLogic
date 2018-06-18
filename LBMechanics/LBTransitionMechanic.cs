using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	[CreateAssetMenu(fileName = "NewTransitionMechanic", menuName = "LBMechanics/TransitionMechanic")]
	public class LBTransitionMechanic: LBMechanicBase
	{
		protected Animator animator;
		protected LBMechanicsExecutor mechexec;

		public string Animation; //The name of the animation, which is present in the Animator component
		public int AnimLayer; //The layer, where this animations is located

		public float AnimBlendTime = 0.1f;

		public string[] SwitchesFrom;
		public string TransfersTo;

		/***************************************** Init stuff *****************************************/

		public override void LockMechanic (GameObject p)
		{
			base.LockMechanic(p);

			mechexec = parent.GetComponent<LBMechanicsExecutor> ();

			if (mechexec == null)
				Debug.LogWarning (MechanicName + ": Cannot lock mechanic -- mechanic executor not found!");

			animator = parent.GetComponent<Animator> ();

			if (animator == null)
				Debug.LogWarning (MechanicName + ": Cannot lock mechanic -- animator not found!");
		}

		/************************************** Mechanic-related stuff *******************************/

		public override bool CanActivateMechanic()
		{
			int i;
			LBMechanicBase m;

			m=mechexec.FindActiveMechanic (this);

			if (m == null || SwitchesFrom.Length == 0)
				return true;	

			for (i = 0; i < SwitchesFrom.Length; i++) 
			{
				if (m.MechanicName == SwitchesFrom [i] && m.CanDeactivateMechanic()) // if we can switch from that mechanic to this
					return true;
			}

			return false;
		}

		//Deactivate all other mechanics
		public override bool ActivateMechanic()
		{
			mechexec.DeactivateAllGroup (this); //turn off all other mechanics

			base.ActivateMechanic (); //activate current mechanic

			animator.CrossFade(Animation,AnimBlendTime);

			return true;
		}

		//Switch to a given mechanic
		void SwitchMechanic()
		{
			DeactivateMechanic ();

			mechexec.ActivateMechanic (mechexec.FindGroup(this), TransfersTo);
		}

		public override void Tick()
		{
			if (animator.GetCurrentAnimatorStateInfo (AnimLayer).normalizedTime >= 1) 
			{
				SwitchMechanic ();
			}
		}
	}
}
