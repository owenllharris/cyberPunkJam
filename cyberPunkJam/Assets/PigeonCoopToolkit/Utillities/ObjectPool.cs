using UnityEngine;
using System.Collections.Generic;


namespace PigeonCoopUtil
{
    [AddComponentMenu("Generic Scripts/Object pool")]
    public class ObjectPool : MonoBehaviour
    {

        public GameObject[] PrefabsContainedInPool;
        public int InitialPoolSize;
        public int AddOnResize;

        private List<PoolableObject> _poolObjects;
        private List<PoolableObject> _unavailablePoolObjects;
        private HashSet<PoolableObject> _unabailablePoolObjectsHashed;

        private bool _initialised = false;

        void FixedUpdate()
        {
            if(_initialised == false)
                return;

            for(int i = _unavailablePoolObjects.Count-1; i >= 0; i--)
            {
                if (_unavailablePoolObjects[i].ReadyToReturnToPool())
                {
                    ReturnToPool(_unavailablePoolObjects[i]);
                }
            }

        }

        public void InitialisePool()
        {
            if (IsInitialised())
            {
                Debug.LogError("ObjectPool: Already initialised.");
                return;
            }

            if (PrefabsContainedInPool.Length == 0)
            {
                Debug.LogError("ObjectPool: No objects in prefab array to add to pool.");
                return;
            }

            _poolObjects = new List<PoolableObject>(PrefabsContainedInPool.Length * InitialPoolSize);
            _unavailablePoolObjects = new List<PoolableObject>(PrefabsContainedInPool.Length * InitialPoolSize);
            _unabailablePoolObjectsHashed = new HashSet<PoolableObject>();

            AddToPool(InitialPoolSize);
            _initialised = true;
        }

        public void ShutdownPool()
        {
            if (!IsInitialised())
            {
                Debug.LogError("ObjectPool: is not yet initialised.");
                return;
            }

            if (_unavailablePoolObjects != null && _unavailablePoolObjects.Count != 0)
            {
                Debug.LogError("ObjectPool: There are still objects that have not been returned to the pool.");
                return;
            }

            for (int i = 0; i < _poolObjects.Count; i++)
            {
                if (_poolObjects[i] != null)
                    Destroy(_poolObjects[i]);

                _poolObjects[i] = null;
            }

            _poolObjects = null;
            _unavailablePoolObjects = null;
            _unabailablePoolObjectsHashed = null;
        }

        public T TakeFromPool<T>(Vector3 position, Quaternion rotation) where T:PoolableObject
        {
            if (!AnyAvailable())
            {
                AddToPool(AddOnResize);
            }

            T po = (T)Take();

            if (po != null)
            {
                po.transform.parent = null;
                po.gameObject.SetActive(true);
                po.TakeFromPool(position, rotation);
            }

            return po;
        }


        public PoolableObject TakeFromPool(Vector3 position, Quaternion rotation)
        {
            if (!AnyAvailable())
            {
                AddToPool(AddOnResize);
            }

            PoolableObject po = Take();

            if (po != null)
            {
                po.transform.parent = null;
                po.gameObject.SetActive(true);
                po.TakeFromPool(position, rotation);
            }

            return po;
        }

        public void ReturnToPool(PoolableObject po)
        {
            if (_unabailablePoolObjectsHashed.Contains(po) == false)
            {
                Debug.LogError("ObjectPool: Trying to return an object that doesn't belong in this pool..");
                return;
            }

            if (po != null)
            {
                po.transform.parent = transform;
                po.gameObject.SetActive(false);
                Return(po);
                po.ReturnToPool();
            }

        }

        public bool IsInitialised()
        {
            return _initialised;
        }

        private PoolableObject Take()
        {
            PoolableObject po = GetRandom(_poolObjects);
            _poolObjects.Remove(po);
            _unavailablePoolObjects.Add(po);
            _unabailablePoolObjectsHashed.Add(po);
            return po;
        }

        private void Return(PoolableObject po)
        {
            _unavailablePoolObjects.Remove(po);
            _unabailablePoolObjectsHashed.Remove(po);
            _poolObjects.Add(po);
        }

        private bool AnyAvailable()
        {
            return _poolObjects != null && _poolObjects.Count > 0;
        }

        private void AddToPool(int numToAdd)
        {
            for (int i = 0; i < numToAdd; i++)
            {
                GameObject tempInstantiatedPrefab = Instantiate(GetRandom(PrefabsContainedInPool)) as GameObject;

                if (tempInstantiatedPrefab == null)
                {
                    Debug.LogError("ObjectPool: One of the objects in my prefab array is null.");
                    return;
                }

                PoolableObject tempInstantiatedPrefabComp = tempInstantiatedPrefab.GetComponent<PoolableObject>();

                if (tempInstantiatedPrefabComp == null)
                {
                    Debug.LogError(string.Format("ObjectPool: {0} must have a Monobehaviour that implements PoolableObject attached to it.", tempInstantiatedPrefab.name));
                    return;
                }

                tempInstantiatedPrefabComp.ReturnToPool();
                _poolObjects.Add(tempInstantiatedPrefabComp);
                tempInstantiatedPrefabComp.gameObject.SetActive(false);
                tempInstantiatedPrefabComp.transform.parent = transform;
            }

            Shuffle(_poolObjects);
        }



        private static T GetRandom<T>(T[] list)
        {
            return list[Random.Range(0, list.Length)];
        }

        private static T GetRandom<T>(List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count; i > 1; i--)
            {
                // Pick random element to swap.
                int j = Random.Range(0, list.Count);
                // Swap.
                T tmp = list[j];
                list[j] = list[i - 1];
                list[i - 1] = tmp;
            }
        }
    }

    public abstract class PoolableObject : MonoBehaviour
    {
        public abstract void TakeFromPool(Vector3 position, Quaternion rotation);
        public abstract void ReturnToPool();

        public abstract bool ReadyToReturnToPool();
    }
}