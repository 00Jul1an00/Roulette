using Assets.Scripts.Models;
using Assets.Scripts.Views;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Controllers
{
    public class RoulleteRotationController
    {
        private const int ROULLETE_COOLDOWN_SEC = 10;
        private const int ROULLETE_ANIMATION_SEC = 5;
        private const string ROULLETE_ACTIVE_BUTTON_TEXT = "Испытать удачу";

        private readonly RoulleteView _view;
        private readonly RewardContainer _rewardContainer;
        private readonly CancellationToken _token;

        private RewardConfig _lastReward;

        public RoulleteRotationController(RoulleteView view,
            RewardContainer rewardContainer,
            CancellationToken token)
        {
            _view = view;
            _rewardContainer = rewardContainer;
            _token = token;
        }

        public async Task CooldownState()
        {
            _view.GetRewardCounter().gameObject.SetActive(false);
            _view.GetRewardImage().gameObject.SetActive(true);
            _view.ResetRewardCounter();
            _view.GetStartButton().interactable = false;
            await WaitCooldownDuration();
        }

        public void ActiveState()
        {
            _view.GetStartButtonText().text = ROULLETE_ACTIVE_BUTTON_TEXT;
            _view.GetStartButton().interactable = true;
        }

        public async Task<int> RewardSelectionState()
        {
            _view.GetStartButton().interactable = false;
            int rewardValue = await StartRotationAnimation();
            _token.ThrowIfCancellationRequested();
            _view.GetRewardImage().gameObject.SetActive(false);
            _view.GetRewardCounter().gameObject.SetActive(true);
            return rewardValue;
        }

        private async Task<int> StartRotationAnimation()
        {
            int rewardIndex = Random.Range(0, _view.GetRewardsText().Count);
            var winningSlot = _view.GetRewardsText()[rewardIndex];
            int rewardValue = int.Parse(winningSlot.text);
            Debug.Log($"WIN:{rewardValue}");
            int sectorsCount = _view.GetRewardsText().Count;
            float anglePerSector = 360f / sectorsCount;
            float startAngle = 15;
            float targetAngle = -(startAngle + anglePerSector * rewardIndex);
            int fullRotations = 5;
            float totalRotation = 360f * fullRotations + targetAngle;

            _view.GetRoulleteImage().transform
                .DORotate(new Vector3(0, 0, totalRotation), ROULLETE_ANIMATION_SEC, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuart);

            await Task.Delay(ROULLETE_ANIMATION_SEC * 1000, _token);
            _token.ThrowIfCancellationRequested();

            return rewardValue;
        }

        private async Task WaitCooldownDuration()
        {
            int timer = 0;

            List<RewardConfig> possibleRewardsList = _rewardContainer.RewardConfigs
                .Where(r => r.RewardName != _lastReward?.RewardName)
                .ToList();
            int randIndex = Random.Range(0, possibleRewardsList.Count);
            RewardConfig reward = possibleRewardsList[randIndex];

            while (timer < ROULLETE_COOLDOWN_SEC)
            {
                _view.GetStartButtonText().text = (ROULLETE_COOLDOWN_SEC - timer).ToString();
                RandomizeRewards(timer + 1 == ROULLETE_COOLDOWN_SEC, reward);
                await Task.Delay(1000, _token);
                _token.ThrowIfCancellationRequested();
                timer++;
            }

            _lastReward = reward;
        }

        private void RandomizeRewards(bool isLastRandom, RewardConfig selectedReward)
        {
            RewardConfig reward = selectedReward;

            if (!isLastRandom)
            {
                int randIndex = Random.Range(0, _rewardContainer.RewardConfigs.Count);
                reward = _rewardContainer.RewardConfigs[randIndex];
            }

            _view.GetRewardImage().sprite = reward.RewardSprite;

            List<int> usedRewardsCount = new();

            for (int i = 0; i < _view.GetRewardsText().Count; i++)
            {
                int randRewardValue = Random.Range(1, reward.MaxCount / reward.RoulleteStep + 1);

                while (usedRewardsCount.Contains(randRewardValue))
                {
                    randRewardValue = Random.Range(1, reward.MaxCount / reward.RoulleteStep + 1);
                }

                usedRewardsCount.Add(randRewardValue);
                _view.GetRewardsText()[i].text = (randRewardValue * reward.RoulleteStep).ToString();
            }


        }
    }
}
