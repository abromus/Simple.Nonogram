using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class PictureBoard : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public void SetData(float x, float y)
        {
            ResizePicture(x, y);
        }

        private void ResizePicture(float x, float y)
        {
            CellUtils.SetSizeWithCurrentAnchors(_rectTransform, x, y);
        }
    }
}
