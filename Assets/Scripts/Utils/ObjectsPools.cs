using System;
using System.Collections.Generic;
using UnityEngine;

using Object = System.Object;

namespace Utils
{
    public class ObjectsPools : MonoBehaviour
    {
        [Header("Bullets pool")]
        [SerializeField] private string _bulletsParentName;
        [SerializeField] private Bullet _bulletPrefabTemplate;
        [SerializeField] private uint _bulletPoolSize = 100;

        [Space][Header("Bubbles pool")]
        [SerializeField] private string _bubblesParentName;
        [SerializeField] private Bubble _bubblePrefabTemplate;
        [SerializeField] private uint _bubblePoolSize = 100;

        private GameObject _bulletsParent;
        private GameObject _bubblesParent;

        private static Dictionary<Type, Queue<Object>> _pools;

        protected void OnEnable()
        {
            _bulletsParent ??= new GameObject(_bulletsParentName);
            _bulletsParent.transform.SetParent(transform);

            _bubblesParent ??= new GameObject(_bubblesParentName);
            _bubblesParent.transform.SetParent(transform);

            _pools = new Dictionary<Type, Queue<Object>>();
            CreateAndFillPool<Bullet>(
                _bulletPrefabTemplate,
                _bulletsParent.transform,
                _bulletPoolSize);

            CreateAndFillPool<Bubble>(
                _bubblePrefabTemplate,
                _bubblesParent.transform,
                _bubblePoolSize);
        }

        protected void OnDisable()
        {
            _pools.Clear();
            _pools = null;
        }

        public static T GetInstance<T>(Vector3 position)
            where T : MonoBehaviour
        {
            var instance = default(T);

            Type type = typeof(T);

            if (!_pools.ContainsKey(type))
                return instance;

            if (_pools[type].Count <= 0)
                return instance;

            instance = (T)_pools[type].Dequeue();
            instance.gameObject.SetActive(true);
            instance.transform.position = position;
            if (type == typeof(Bullet))
                (instance as Bullet).Reset();

            return instance;
        }

        public static void DisableInstance<T>(T instance)
            where T : MonoBehaviour
        {
            Type type = typeof(T);

            if (!_pools.ContainsKey(type))
                return;

            if (_pools[type] == null)
                _pools[type] = new Queue<Object>();

            // instance.transform.position = Vector3.zero;
            instance.gameObject.SetActive(false);

            _pools[type].Enqueue(instance);
        }

        private void CreateAndFillPool<T>(MonoBehaviour template,
                                          Transform parent, uint count)
        {
            Type type = typeof(T);

            _pools.Add(type, new Queue<Object>());

            for (int i = 0; i < count; ++i)
            {
                var instance = Instantiate(template, parent.transform);
                instance.transform.position = Vector3.zero;
                instance.gameObject.SetActive(false);

                _pools[type].Enqueue((Object)instance);
            }
        }
    }
}
