
using UnityEngine;

namespace XR.Break
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        [SerializeField]
        TMPro.TextMeshPro scoreLabel;

        private uint score;
        protected uint Score
        {
            get => score;
            set
            {
                score = value;
                if (scoreLabel != null)
                {
                    scoreLabel.text = "Score: " + score.ToString("N0"); // 123,000,000
                }
            }
        }

        // Prevent Non-singleton construction
        protected ScoreManager() { }

        private void Awake()
        {
            ResetScore();
        }

        public void Show()
        {
            SetScoreVisibility(true);
        }

        public void Hide()
        {
            SetScoreVisibility(false);
        }

        public void SetScoreVisibility(bool isVisible)
        {
            if (scoreLabel != null)
            {
                scoreLabel.gameObject.SetActive(isVisible);
            }
        }

        public void AddScore(uint score)
        {
            Score += score;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}