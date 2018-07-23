using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using LBMechanics;

namespace LBMechanics
{
	[System.Serializable]
	public struct MechanicGroup
	{
		public string GroupName;
		public LBMechanicBase[] Mechanics;
		public uint DefaultActiveMechanic;
	}

	public class LBMechanicsExecutor : MonoBehaviour 
	{

		public MechanicGroup[] MechanicGroups;

		// Use this for initialization
		void Start () 
		{
			Init ();	
		}

		void Update()
		{
			Tick ();

			DebugFunc ();
		}
			
		public void ActivateMechanic(string name)
		{
			LBMechanicBase m = FindMechanic (name);

			if (m == null)
				return;

			if (m.CanActivateMechanic())
				m.ActivateMechanic ();
		}

		public void ActivateMechanic(string group, string name)
		{
			LBMechanicBase m = FindMechanic(group, name);

			if (m == null)
				return;

			if (m.CanActivateMechanic())
				m.ActivateMechanic ();
		}

		public void DeactivateAllGroup(LBMechanicBase mech)
		{
			string g = FindGroup (mech);

			if (g == "")
				return;

			DeactivateAllGroup (g);
		}

		public void DeactivateAllGroup(string group)
		{
			int i, j;

			for (i = 0; i < MechanicGroups.Length; i++) 
			{
				if (MechanicGroups[i].GroupName==group)
				{
					for (j = 0; j < MechanicGroups[i].Mechanics.Length; j++) 
					{
						MechanicGroups [i].Mechanics [j].DeactivateMechanic ();
					}
				}
			}
		}

		void Init()
		{
			int i, j;

			for (i = 0; i < MechanicGroups.Length; i++) 
			{
				for (j = 0; j < MechanicGroups[i].Mechanics.Length; j++) 
				{
					MechanicGroups [i].Mechanics [j].InitMechanic ();
					MechanicGroups [i].Mechanics [j].LockMechanic (gameObject);
				}
				MechanicGroups [i].Mechanics [MechanicGroups [i].DefaultActiveMechanic].ActivateMechanic ();
			}
		}

		void Tick()
		{
			int i, j;

			for (i = 0; i < MechanicGroups.Length; i++) 
			{
				for (j = 0; j < MechanicGroups[i].Mechanics.Length; j++) 
				{
					if (MechanicGroups [i].Mechanics [j].bNeedsTick && MechanicGroups [i].Mechanics [j].bIsActive)
						MechanicGroups [i].Mechanics [j].Tick ();

					if (MechanicGroups [i].Mechanics [j].bAutoActivated) 
					{
						if (MechanicGroups [i].Mechanics [j].CanActivateMechanic ()) 
						{
							MechanicGroups [i].Mechanics [j].ActivateMechanic ();
							Debug.Log ("Activating "+MechanicGroups [i].Mechanics [j].MechanicName);
						}
					}
						
				}
			}
		}

		//Find mechanic in group by a full name <GroupName:MechName>
		public LBMechanicBase FindMechanic(string name)
		{
			string group, mech;

			string [] s = name.Split(':');
			group = s [0];
			mech = s [1];

			return FindMechanic (group, mech);
		}

		//Find a certain mechanic in a certain group
		public LBMechanicBase FindMechanic(string group, string mech)
		{
			int i, j;

			for (i = 0; i < MechanicGroups.Length; i++) 
			{
				if (MechanicGroups[i].GroupName==group)
				{
					for (j = 0; j < MechanicGroups[i].Mechanics.Length; j++) 
					{
						if (MechanicGroups[i].Mechanics[j].MechanicName == mech)
							return MechanicGroups[i].Mechanics[j];
					}
				}
			}

			return null;
		}

		//Find active mechanic in a group, where <mech> is nested (for groups with exclusive activation)
		public LBMechanicBase FindActiveMechanic(LBMechanicBase mech)
		{
			string group = FindGroup (mech);

			if (group == "")
				return null;

			return FindActiveMechanic (group);
		}

		//Find active mechanic in a certain group (for groups with exclusive activation)
		public LBMechanicBase FindActiveMechanic(string group)
		{
			int i, j;

			for (i = 0; i < MechanicGroups.Length; i++) 
			{
				if (MechanicGroups[i].GroupName==group)
				{
					for (j = 0; j < MechanicGroups[i].Mechanics.Length; j++) 
					{
						if (MechanicGroups[i].Mechanics[j].bIsActive)
							return MechanicGroups[i].Mechanics[j];
					}
				}
			}

			return null;
		}
			
		public string FindGroup(LBMechanicBase mech)
		{
			int i, j;

			for (i = 0; i < MechanicGroups.Length; i++) 
			{
				for (j = 0; j < MechanicGroups[i].Mechanics.Length; j++) 
				{
					if (MechanicGroups [i].Mechanics [j] == mech)
						return MechanicGroups [i].GroupName;
				}
			}

			return "";
		}
			
		void DebugFunc()
		{
			if (Input.GetKeyDown (KeyCode.UpArrow)) 
			{
				ActivateMechanic ("Default:XW_Grnd_Stand_To_Move");
			}

			if (Input.GetKeyUp(KeyCode.UpArrow)) 
			{
				ActivateMechanic ("Default:XW_Grnd_Move_To_Stand");
			}

			if (Input.GetKeyDown (KeyCode.Alpha1)) 
			{
				ActivateMechanic ("Default:XW_Trans_Test");
			}

			if (Input.GetKeyDown (KeyCode.Space)) 
			{
				ActivateMechanic ("Default:XW_Grnd_Move_To_Air");
			} 
		}

	}
}