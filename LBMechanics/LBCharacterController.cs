using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LBMechanics
{
	[System.Serializable]
	public struct ControlValue
	{
		public string Name;
		public object Value;
	}

	public class LBCharacterController : LBMechanicsExecutor 
	{
		public ControlValue[] ControlValues;

		public bool ControlValueExists(string valuename)
		{
			int i;

			for (i = 0; i < ControlValues.Length; i++) 
			{
				if (ControlValues [i].Name == valuename)
					return true;
			}

			return false;
		}

		public object GetControlValue(string valuename)
		{
			int i;

			for (i = 0; i < ControlValues.Length; i++) 
			{
				if (ControlValues [i].Name == valuename)
					return ControlValues [i].Value;
			}

			return null;
		}

		public void SetControlValue(string valuename, object value)
		{
			int i;

			for (i = 0; i < ControlValues.Length; i++) 
			{
				if (ControlValues [i].Name == valuename)
					ControlValues [i].Value = value;
			}
		}
	}
}
