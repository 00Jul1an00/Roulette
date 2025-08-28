using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [CreateAssetMenu(fileName = "RewardContainer", menuName = "Configs/RewardContainer")]
    public class RewardContainer : ScriptableObject
    {
        [field: SerializeField] public List<RewardConfig> RewardConfigs;
    }
}
