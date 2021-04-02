using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolValue", menuName = "Data/Values/Bool")]
public class BoolValue : ScriptableObject, ISerializationCallbackReceiver
{
	public bool defaultValue;
	[NonSerialized] private bool runtimeValue;

	public event Action<bool> OnValueChanged;

	public bool RuntimeValue
	{
		get { return runtimeValue; }
		set
		{
			if (value == runtimeValue)
				return;

			runtimeValue = value;
			OnValueChanged?.Invoke(runtimeValue);
		}
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		runtimeValue = defaultValue;
	}
}
