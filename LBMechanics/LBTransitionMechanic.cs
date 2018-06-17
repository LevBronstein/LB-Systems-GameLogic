using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	[CreateAssetMenu(fileName = "NewTransitionMechanic", menuName = "LBMechanics/TransitionMechanic")]
	public class LBTransitionMechanic: LBMechanicBase
	{
		protected LBMechanicsExecutor mechexec;

		public string[] SwitchesFrom;
		public string TransfersTo;

		/***************************************** Init stuff *****************************************/

		public override void LockMechanic (GameObject p)
		{
			base.LockMechanic(p);

			mechexec = parent.GetComponent<LBMechanicsExecutor> ();

			if (mechexec == null)
				Debug.LogWarning (MechanicName + ": Cannot lock mechanic -- mechanic executor not found!");
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

			return true;
		}
	}
}
