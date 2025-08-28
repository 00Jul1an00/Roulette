using UnityEngine;

namespace Assets.Scripts.Views
{
    public class RewardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public SpriteRenderer GetSpriteRenderer() => _spriteRenderer;
    }
}