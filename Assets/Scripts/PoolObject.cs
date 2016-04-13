using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour {

	public virtual void OnObjectReuse (){
	}

	public void Destroy(){
		gameObject.SetActive (false);
	}
}
