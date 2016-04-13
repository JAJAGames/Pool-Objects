﻿using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour {

	public GameObject prefab;

	void Start(){
		PoolManager.instance.CreatePool (prefab, 3);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			PoolManager.instance.ReuseObject (prefab, Vector3.zero, Quaternion.identity);			
		}
	}
}
