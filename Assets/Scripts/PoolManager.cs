/* POOLMANAGER.CS
 * (C) COPYRIGHT "JAJA GAMES", 2.016
 * ------------------------------------------------------------------------------------------------------------------------------------
 * EXPLANATION: 
 * 
 * ------------------------------------------------------------------------------------------------------------------------------------
 * FUNCTIONS LIST:
 * 
 * CREATEPOOL (GAMEOBJECT, INT)
 * ------------------------------------------------------------------------------------------------------------------------------------
 * MODIFICATIONS:
 * DATA			DESCRIPCTION
 * ----------	-----------------------------------------------------------------------------------------------------------------------
 * 13/04/2016	added to Pool Manager gameobject in main scene
 * ------------------------------------------------------------------------------------------------------------------------------------
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	public class ObjectInstance {

		GameObject gameObject;
		Transform transform;

		bool hasPoolObjectComponent;
		PoolObject poolObjectScript;

		public ObjectInstance(GameObject objectInstance){

			gameObject = objectInstance;
			transform = gameObject.transform;
			gameObject.SetActive(false);

			if (gameObject.GetComponent<PoolObject>()){
				hasPoolObjectComponent = true;
				poolObjectScript = gameObject.GetComponent<PoolObject>();
			}
		}

		public void Reuse(Vector3 position, Quaternion rotation){

			if (hasPoolObjectComponent) {
				poolObjectScript.OnObjectReuse ();
			}

			gameObject.SetActive (true);
			transform.position = position;
			transform.rotation = rotation;
		}

		public void SetParent (Transform parent)
		{
			transform.parent = parent;
		}
	}

	Dictionary<int,Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>> ();
	static PoolManager _instace;

	public static PoolManager instance {
		
		get { 
			if (_instace == null) 
				_instace = FindObjectOfType<PoolManager> ();

			return _instace;
		}
	}

	public void CreatePool(GameObject prefab, int poolSize){

		int poolKey = prefab.GetInstanceID ();

		GameObject poolHolder = new GameObject (prefab.name + "pool");
		poolHolder.transform.parent = transform;

		if (!poolDictionary.ContainsKey (poolKey)) 
		{
			poolDictionary.Add (poolKey, new Queue<ObjectInstance> ());
			for (int i = 0; i < poolSize; i++){
				ObjectInstance newObject = new ObjectInstance( Instantiate (prefab) as GameObject);
				poolDictionary [poolKey].Enqueue (newObject);
				newObject.SetParent (poolHolder.transform);
			}
		}
	}

	public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation){
		int poolKey = prefab.GetInstanceID ();

		if (poolDictionary.ContainsKey (poolKey)) {
			ObjectInstance objectToReuse = poolDictionary [poolKey].Dequeue ();
			poolDictionary [poolKey].Enqueue (objectToReuse);

			objectToReuse.Reuse (position, rotation);
		}
	}


}
