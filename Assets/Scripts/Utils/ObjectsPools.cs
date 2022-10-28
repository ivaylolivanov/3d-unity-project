using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public enum ObjectPoolType
    {
        Bullet,
        Bubble,
    }

    public class ObjectsPools : MonoBehaviour
    {
        [Header("Bullets pool")]
        [SerializeField] private string _bulletsParentName;
        [SerializeField] private Bullet _bulletPrefabTemplate;
        [SerializeField] private int _bulletPoolSize = 100;

        [Space][Header("Bubbles pool")]
        [SerializeField] private string _bubblesParentName;
        [SerializeField] private Bubble _bubblePrefabTemplate;
        [SerializeField] private int _bubblePoolSize = 100;

        private GameObject _bulletsParent;
        private static Queue<Bullet> _bulletPool;

        private GameObject _bubblesParent;
        private static Queue<Bubble> _bubblePool;

        protected void OnEnable()
        {
            _bulletsParent ??= new GameObject(_bulletsParentName);
            _bulletPool = new Queue<Bullet>();
            for (int i = 0; i < _bulletPoolSize; ++i)
            {
                Bullet bulletInstance = Instantiate(
                    _bulletPrefabTemplate,
                    _bulletsParent.transform);
                bulletInstance.transform.position = Vector3.zero;
                bulletInstance.gameObject.SetActive(false);

                _bulletPool.Enqueue(bulletInstance);
            }

            _bubblesParent ??= new GameObject(_bubblesParentName);
            _bubblePool = new Queue<Bubble>();
            for (int i = 0; i < _bubblePoolSize; ++i)
            {
                Bubble bubbleInstance = Instantiate(
                    _bubblePrefabTemplate,
                    _bubblesParent.transform);
                bubbleInstance.transform.position = Vector3.zero;
                bubbleInstance.gameObject.SetActive(false);

                _bubblePool.Enqueue(bubbleInstance);
            }
        }

        protected void OnDisable()
        {
            _bulletPool.Clear();
            _bulletPool = null;

            _bubblePool.Clear();
            _bubblePool = null;
        }

        public static GameObject GetInstanceAsGameObject(Vector3 position, ObjectPoolType type)
        {
            GameObject result = null;

            switch(type)
            {
                case ObjectPoolType.Bullet:
                {
                    result = GetBulletInstance(position)?.gameObject;
                } break;

                case ObjectPoolType.Bubble:
                {
                    result = GetBubbleInstance(position)?.gameObject;
                } break;
            }

            return result;
        }

        public static Bullet GetBulletInstance(Vector3 position)
        {
            if (_bulletPool.Count <= 0)
                return null;

            var bulletInstance = _bulletPool.Dequeue();
            bulletInstance.gameObject.SetActive(true);
            bulletInstance.transform.position = position;
            bulletInstance.Reset();

            return bulletInstance;
        }

        public static Bubble GetBubbleInstance(Vector3 position)
        {
            if (_bubblePool.Count <= 0)
                return null;

            var bubbleInstance = _bubblePool.Dequeue();
            bubbleInstance.gameObject.SetActive(true);
            bubbleInstance.transform.position = position;

            return bubbleInstance;
        }

        public static void DisableInstance<T>(T instance)
        {
            if (instance.GetType() == typeof(Bullet))
            {
                Bullet bulletInstance = instance as Bullet;

                bulletInstance.transform.position = Vector3.zero;
                bulletInstance.gameObject.SetActive(false);

                _bulletPool.Enqueue(bulletInstance);
            }
            else if (instance.GetType() == typeof(Bubble))
            {
                Bubble bubbleInstance = instance as Bubble;

                bubbleInstance.transform.position = Vector3.zero;
                bubbleInstance.gameObject.SetActive(false);

                bubbleInstance.OnTriggerEntered = null;
                bubbleInstance.OnTriggerExited  = null;

                _bubblePool.Enqueue(bubbleInstance);
            }
        }

        public static void DisableInstance(Bullet bulletInstance)
        {
            bulletInstance.transform.position = Vector3.zero;
            bulletInstance.gameObject.SetActive(false);

            _bulletPool.Enqueue(bulletInstance);
        }
    }
}
