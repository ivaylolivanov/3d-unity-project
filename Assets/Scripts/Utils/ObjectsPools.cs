using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class ObjectsPools : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefabTemplate;
        [SerializeField] private int _bulletPoolSize = 100;

        private static Queue<Bullet> _bulletPool;

        protected void OnEnable()
        {
            _bulletPool = new Queue<Bullet>();
            for (int i = 0; i < _bulletPoolSize; ++i)
            {
                Bullet bulletInstance = Instantiate(_bulletPrefabTemplate, transform);
                bulletInstance.transform.position = Vector3.zero;
                bulletInstance.gameObject.SetActive(false);

                _bulletPool.Enqueue(bulletInstance);
            }
        }

        protected void OnDisable()
        {
            _bulletPool.Clear();
            _bulletPool = null;
        }

        public static Bullet GetBulletInstance(Vector3 position)
        {
            if(_bulletPool.Count <= 0)
                return null;

            var bulletInstance = _bulletPool.Dequeue();
            bulletInstance.gameObject.SetActive(true);
            bulletInstance.transform.position = position;
            bulletInstance.Reset();

            return bulletInstance;
        }

        public static void DisableInstance(Bullet bulletInstance)
        {
            bulletInstance.transform.position = Vector3.zero;
            bulletInstance.gameObject.SetActive(false);

            _bulletPool.Enqueue(bulletInstance);
        }
    }
}
