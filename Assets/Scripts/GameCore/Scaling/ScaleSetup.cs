using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.GameCore.Scaling
{
    public class ScaleSetup : MonoBehaviour
    {
        private void Awake()
        {
            SetScale(GetComponent<CanvasScaler>());
            SetScale(GetComponent<OrthographicScaler>());
            Destroy(this);
        }

        private void SetScale(CanvasScaler scaler)
        {
            if (scaler != null)
                scaler.matchWidthOrHeight = ScalePreferences.MatchWidthOrHeight;
        }

        private void SetScale(OrthographicScaler scaler)
        {
            if (scaler != null)
                scaler.MatchWidthOrHeight = ScalePreferences.MatchWidthOrHeight;
        }
    }
}
