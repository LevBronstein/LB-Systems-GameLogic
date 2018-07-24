using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*A mechanic, which is able to switch to and from mechanics with animation blending*/
namespace LBMechanics
{
	[System.Serializable]
	public struct MechanicParameter
	{
		public string Name;
		public object Value;
		public bool UseControlValue;

		public MechanicParameter(string name)
		{
			Name = name;
			Value = null;
			UseControlValue = false;
		}

		public MechanicParameter(string name, object value)
		{
			Name = name;
			Value = value;
			UseControlValue = false;
		}

		public MechanicParameter(string name, object value, bool busecv)
		{
			Name = name;
			Value = value;
			UseControlValue = busecv;
		}
	}

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

		/*Binds this mechanism to a certain gameobject (its parent)*/
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

			if (!base.CanActivateMechanic ())
				return false;

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

			if (Animation != "")
				animator.CrossFade(Animation,AnimBlendTime);

			return true;
		}

		//Switch to a given mechanic
		void SwitchMechanic()
		{
			DeactivateMechanic ();

			mechexec.ActivateMechanic (mechexec.FindGroup(this), TransfersTo);
		}

		/************************************** Control value handling *******************************/

		protected object GetControlValue(string valuename)
		{
			LBCharacterController ctrl;

			ctrl = (LBCharacterController)mechexec;

			if (ctrl == null)
				return null;

			object value;

			value = ctrl.GetControlValue (valuename);

			return value;
		}

		protected object GetControlValue(MechanicParameter param)
		{
			LBCharacterController ctrl;

			if (param.UseControlValue) 
			{
				ctrl = (LBCharacterController)mechexec;

				if (ctrl == null)
					return null;

				object value;

				value = ctrl.GetControlValue (param.Name);

				return value;
			}

			return null;
		}

		//пофиксить, если нет анимации
		public override void Tick()
		{
			if (animator.GetCurrentAnimatorStateInfo (AnimLayer).normalizedTime >= 1) 
			{
				SwitchMechanic ();
			}
		}
	}

	[CreateAssetMenu(fileName = "NewConditionalTransitionMechanic", menuName = "LBMechanics/TransitionMechanic")]
	public class LBConditionalTransitionMechanic: LBTransitionMechanic
	{

		public override bool CanActivateMechanic()
		{
			return base.CanActivateMechanic () && CheckConditions ();
		}

		protected virtual bool CheckConditions()
		{
			return true;
		}
	}
}
