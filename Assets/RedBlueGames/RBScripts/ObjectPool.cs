namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Simple object pooling class
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        [Tooltip("GameObject to use as the pooled object")]
        [SerializeField]
        private GameObject pooledObject;

        [Tooltip("The number of game objects to use in the pool")]
        [SerializeField]
        private int poolSize;

        [Tooltip("Specify whether or not the pool can grow to satisfy requests for objects when all pooled objects are used")]
        [SerializeField]
        private bool grow;

        private List<GameObject> pool;

        /// <summary>
        /// Gets the next available pooled object. If full, and flagged to grow, it will create a new object and add it to the pool.
        /// </summary>
        /// <returns>The pooled object.</returns>
        public GameObject GetPooledObject()
        {
            for (int i = 0; i < this.pool.Count; i++)
            {
                if (!this.pool[i].activeInHierarchy)
                {
                    return this.pool[i];
                }
            }

            if (this.grow)
            {
                return this.AddObjectToPool();
            }
            else
            {
                throw new System.InsufficientMemoryException("Tried to get more objects from pool than available, and pool is not flagged to grow.");
            }
        }

        private void Awake()
        {
            this.pool = new List<GameObject>();
            for (int i = 0; i < this.poolSize; i++)
            {
                this.AddObjectToPool();
            }
        }

        private GameObject AddObjectToPool()
        {
            GameObject obj = (GameObject)Instantiate(this.pooledObject);
            obj.SetActive(false);
            this.pool.Add(obj);

            return obj;
        }
    }
}