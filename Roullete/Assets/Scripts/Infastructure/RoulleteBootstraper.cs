using Assets.Scripts.Controllers;
using Assets.Scripts.Models;
using Assets.Scripts.Views;
using UnityEngine;

namespace Assets.Scripts.Infastructure
{
    public class RoulleteBootstraper : MonoBehaviour
    {
        [SerializeField] private RoulleteView _roulleteView;
        [SerializeField] private RewardContainer _rewardContainer;
        [SerializeField] private RewardsPool _rewardsPool;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private RewardSpawnRadiusConfig _rewardSpawnRadiusConfig;

        private RoulleteController _roulleteConroller;

        private void Start()
        {
            _roulleteConroller = new(_roulleteView, 
                _rewardContainer, 
                _rewardsPool,
                _rewardSpawnRadiusConfig,
                _arrowTransform.position);
        }

        private void OnDestroy()
        {
            _roulleteConroller.Dispose();
            _roulleteConroller = null;
        }
    }
}