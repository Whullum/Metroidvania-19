using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableDictionaryExample : MonoBehaviour {
	// The dictionaries can be accessed throught a property
	[SerializeField]
	private StringStringDictionary m_stringStringDictionary;

	public IDictionary<string, string> StringStringDictionary
	{
		get { return m_stringStringDictionary; }
		set { m_stringStringDictionary.CopyFrom (value); }
	}

	public ObjectColorDictionary m_objectColorDictionary;
	public StringColorArrayDictionary m_objectColorArrayDictionary;

	[SerializeField]
	public StringIntDictionary m_stringMyClass;
	public IDictionary<string, int> StringInt
	{
		get { return m_stringMyClass; }
		set { m_stringMyClass.CopyFrom(value); }
	}

	void Reset ()
	{
		// access by property
		StringStringDictionary = new Dictionary<string, string>() { {"first key", "value A"}, {"second key", "value B"}, {"third key", "value C"} };
		m_objectColorDictionary = new ObjectColorDictionary() { {gameObject, Color.blue}, {this, Color.red} };
		StringInt = new Dictionary<string, int>() { { "testing", 1}, { "testing", 2} };
	}
}
