using Assets.Scripts.Views;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Infastructure;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Models;

namespace Assets.Scripts.Controllers
{
    public class RewardAnimationController
    {
        private const int MAX_SPAWN_REWARDS_OBJECTS = 20;

        private readonly RoulleteView _view;
        private readonly RewardsPool _rewardsPool;
        private readonly Vector3 _arrowPosition;
        private readonly RewardSpawnRadiusConfig _rewardSpawnRadiusConfig;
        private readonly CancellationToken _token;

        public RewardAnimationController(RoulleteView view, 
            RewardsPool rewardsPool, 
            Vector3 arrowPosition,
            RewardSpawnRadiusConfig rewardSpawnRadiusConfig,
            CancellationToken token)
        {
            _view = view;
            _rewardsPool = rewardsPool;
            _arrowPosition = arrowPosition;
            _rewardSpawnRadiusConfig = rewardSpawnRadiusConfig;
            _token = token;
        }

        public async Task StartRewardAnimation(int rewardValue)
        {
            int objectsCount = Mathf.Min(MAX_SPAWN_REWARDS_OBJECTS, rewardValue);
            int valuePerObject = rewardValue / objectsCount;
            int remainder = rewardValue % objectsCount;

            List<int> rewardDistribution = new();
            for (int i = 0; i < objectsCount; i++)
            {
                rewardDistribution.Add(valuePerObject);
            }
            for (int i = 0; i < remainder; i++)
            {
                rewardDistribution[i] += 1;
            }

            List<Task> animations = new();
            foreach (int reward in rewardDistribution)
            {
                animations.Add(AnimateRewardObject(reward));
            }

            await Task.WhenAll(animations);
        }

        private async Task AnimateRewardObject(int rewardValue)
        {
            RewardView reward = _rewardsPool.Get();
            reward.transform.position = _arrowPosition;
            reward.transform.localScale = Vector3.zero;
            reward.GetSpriteRenderer().sprite = _view.GetRewardImage().sprite;

            float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            float radius = UnityEngine.Random.Range(_rewardSpawnRadiusConfig.MinRadius, _rewardSpawnRadiusConfig.MaxRadius);
            Vector3 targetPos = _arrowPosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            await reward.transform.DOMove(targetPos, 0.5f)
                .SetEase(Ease.OutBack)
                .AsyncWaitForCompletion();
            _token.ThrowIfCancellationRequested();

            await reward.transform.DOScale(Vector3.one, 0.3f)
                .AsyncWaitForCompletion();
            _token.ThrowIfCancellationRequested();

            await Task.Delay(UnityEngine.Random.Range(1000, 2500), _token);
            _token.ThrowIfCancellationRequested();

            await reward.transform.DOMove(_view.GetRoulleteImage().transform.position, 0.7f)
                .SetEase(Ease.InBack)
                .AsyncWaitForCompletion();
            _token.ThrowIfCancellationRequested();

            _rewardsPool.Release(reward);
            _view.UpdateRewardCounter(rewardValue);
        }
    }
}
