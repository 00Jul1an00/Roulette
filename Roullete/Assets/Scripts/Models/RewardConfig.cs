using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Models 
{
    [CreateAssetMenu(fileName = "Reward", menuName = "Configs/Reward")]
    public class RewardConfig : ScriptableObject
    {
        [field: SerializeField] public Rewards RewardName { get; private set; }
        [field: SerializeField] public Sprite RewardSprite { get; private set; }
        [field: SerializeField] public int MinCount { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; }
        [field: SerializeField] public int RoulleteStep { get; private set; }
    }
}