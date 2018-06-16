using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	public class LBMechanicBase: ScriptableObject
	{
		public string mechanicname = "LBMechanic";
		public bool needstick = false;
		public bool autoactivated = false;

		protected bool isactive = false;

		protected GameObject parent;

		public virtual string MechanicName
		{
			get 
			{return mechanicname;}
		}

		public bool bIsActive
		{
			get 
			{return isactive;}
		}

		public bool bNeedsTick
		{
			get 
			{return needstick;}
		}
			
		public bool bAutoActivated
		{
			get 
			{return autoactivated;}
		}

		public virtual void InitMechanic()
		{
			isactive = false;
		}

		public virtual void LockMechanic (GameObject p)
		{
			parent = p;
		}

		public void OnEnable()
		{
			InitMechanic();
		}

		//Checks conditions -- if this mechanic can be activated right now
		public virtual bool CanActivateMechanic()
		{
			return true;
		}

		//Tries to activate this mechanic
		public virtual bool ActivateMechanic()
		{
			isactive = true;
			return true;
		}

		//Checks conditions -- if this mechanic can be deactivated right now
		public virtual bool CanDeactivateMechanic()
		{
			isactive = false;
			return true;
		}

		//Tries to deactivate this mechanic
		public virtual bool DeactivateMechanic()
		{
			isactive = false;
			return true;
		}

		public virtual void Tick()
		{
		}
	}
}