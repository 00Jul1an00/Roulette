using UnityEngine;

namespace Assets.Scripts.Models
{
    [CreateAssetMenu(fileName = "RewardSpawnRadius", menuName = "Configs/RewardSpawnRadius")]
    public class RewardSpawnRadiusConfig : ScriptableObject
    {
        [field: SerializeField] public float MinRadius { get; private set; }
        [field: SerializeField] public float MaxRadius { get; private set; }
    }
}
