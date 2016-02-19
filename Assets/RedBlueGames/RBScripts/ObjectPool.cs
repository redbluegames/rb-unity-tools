using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	// [NotNull]
	public GameObject
		PooledObject;
	public int PoolSize;
	public bool Grow;

	List<GameObject> pool;

	void Awake()
	{
		pool = new List<GameObject>();
		for( int i = 0 ; i < PoolSize ; i++ )
		{
			AddObjectToPool();
		}
	}

	public GameObject GetPooledObject()
	{
		for( int i = 0 ; i < pool.Count ; i++ )
		{
			if( !pool[ i ].activeInHierarchy )
			{
				return pool[ i ];
			}
		}

		if( Grow )
		{
			return AddObjectToPool();
		}
		else
		{
			throw new System.InsufficientMemoryException( "Tried to get more objects from pool than available, and pool is not flagged to grow." );
		}
	}

	GameObject AddObjectToPool()
	{
		GameObject obj = (GameObject)Instantiate (PooledObject);
		obj.SetActive( false );
		pool.Add( obj );

		return obj;
	}
}
