using Assets.Scripts.Models;
using Assets.Scripts.Views;
using System;
using UnityEngine;
using Assets.Scripts.Infastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Scripts.Controllers
{
    public class RoulleteController : IDisposable
    {
        private const int ROULLETE_COOLDOWN_AFTER_REWARD_SEC = 2;

        private readonly RoulleteView _view;
        private readonly RoulleteRotationController _rotationController;
        private readonly RewardAnimationController _rewardAnimationController;
        private readonly CancellationTokenSource _cancellationToken = new();

        public RoulleteController(
            RoulleteView view,
            RewardContainer rewardContainer,
            RewardsPool rewardsPool,
            RewardSpawnRadiusConfig rewardSpawnRadiusConfig,
            Vector3 arrowPosition)
        {
            _view = view;

            _rotationController = new RoulleteRotationController(view, 
                rewardContainer,
                _cancellationToken.Token);

            _rewardAnimationController = new RewardAnimationController(view, 
                rewardsPool, 
                arrowPosition,
                rewardSpawnRadiusConfig,
                _cancellationToken.Token);

            Subscribe();
            StartRoullete();
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
            Unsubscribe();
        }

        private void Subscribe()
        {
            _view.GetStartButton().onClick.AddListener(OnStartButtonClick);
        }

        private void Unsubscribe()
        {
            _view.GetStartButton().onClick.RemoveListener(OnStartButtonClick);
        }

        private async void StartRoullete()
        {
            try
            {
                await _rotationController.CooldownState();
                _rotationController.ActiveState();
            }
            catch (OperationCanceledException) { }
        }

        private async void OnStartButtonClick()
        {
            try
            {
                int rewardValue = await _rotationController.RewardSelectionState();
                await _rewardAnimationController.StartRewardAnimation(rewardValue);
                await Task.Delay(ROULLETE_COOLDOWN_AFTER_REWARD_SEC * 1000, _cancellationToken.Token);
                _cancellationToken.Token.ThrowIfCancellationRequested();
                StartRoullete();
            }
            catch (OperationCanceledException) { }
        }
    }
}
