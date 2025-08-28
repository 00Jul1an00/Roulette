using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Views;

namespace Assets.Scripts.Infastructure
{
    public class RewardsPool : MonoBehaviour
    {
        [SerializeField] private RewardView _rewardPrefab;
        [SerializeField] private int _initialSize = 20;

        private readonly Queue<RewardView> _pool = new();

        private void Start()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                var obj = CreateNewObject();
                _pool.Enqueue(obj);
            }
        }

        private RewardView CreateNewObject()
        {
            var obj = Instantiate(_rewardPrefab, transform);
            obj.gameObject.SetActive(false);
            return obj;
        }

        public RewardView Get()
        {
            if (_pool.Count == 0)
                _pool.Enqueue(CreateNewObject());

            var obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Release(RewardView obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            _pool.Enqueue(obj);
        }
    }
}
