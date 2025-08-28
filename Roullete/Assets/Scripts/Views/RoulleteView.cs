using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class RoulleteView : MonoBehaviour
    {
        [SerializeField] private Image _roulleteImage;
        [SerializeField] private List<TMP_Text> _rewardsText;
        [SerializeField] private TMP_Text _rewardCounter;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _startButtonText;
        [SerializeField] private Button _startButton;

        private int _rewardCount = 0;

        public Image GetRoulleteImage() => _roulleteImage;
        public List<TMP_Text> GetRewardsText() => _rewardsText;
        public TMP_Text GetRewardCounter() => _rewardCounter;
        public Image GetRewardImage() => _rewardImage;
        public TMP_Text GetStartButtonText() => _startButtonText;
        public Button GetStartButton() => _startButton;

        public void UpdateRewardCounter(int value)
        {
            _rewardCount += value;
            _rewardCounter.text = _rewardCount.ToString();
        }

        public void ResetRewardCounter()
        {
            _rewardCount = 0;
            _rewardCounter.text = "0";
        }
    }
}
