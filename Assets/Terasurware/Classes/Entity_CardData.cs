using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_CardData : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int ID;
		public int nameID;
		public int advance;
		public int coin;
		public int rarity;
		public int eventID;
		public int star;
		public int price;
	}
}

