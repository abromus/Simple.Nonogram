using UnityEngine;

namespace Simple.Nonogram.GameCore.Scaling
{
    public class ScalePreferences : MonoBehaviour
    {
        public static float MatchWidthOrHeight { get; private set; }

        [SerializeField] private float _switchRatio = 0.56f;
        [SerializeField] private bool _simpleCompare;
        [SerializeField] private Vector2 _pref;
        [SerializeField] private float _lessMatchValue;
        [SerializeField] private float _greaterMatchValue = 1f;

        private void Awake()
        {
            var ratio = (float)Screen.height / Screen.width;

            if (_simpleCompare)
            {
                var prefRatio = _pref.y / _pref.x;
                MatchWidthOrHeight = ratio > prefRatio ? _lessMatchValue : _greaterMatchValue;
            }
            else
            {
                MatchWidthOrHeight = ratio > _switchRatio ? _lessMatchValue : _greaterMatchValue;
            }
        }
    }
}