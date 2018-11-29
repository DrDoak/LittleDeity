﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLua : MonoBehaviour {
	private static ListLua m_instance;

	public GameObject Halberd;
	public GameObject HalberdTargeter;

	public static ListLua Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}
	}
}
