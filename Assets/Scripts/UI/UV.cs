using UnityEngine;
using UnityEngine.Sprites;

namespace Simple.Nonogram.UI
{
    public class UV
    {
        private readonly Vector2[] _startUvs;
        private readonly Vector2[] _middleUvs;
        private readonly Vector2[] _endUvs;
        private readonly Vector2[] _fullUvs;

        public UV(Sprite activeSprite)
        {
            var isActiveSprite = activeSprite != null;

            var outer = isActiveSprite ? DataUtility.GetOuterUV(activeSprite) : new Vector4();
            var inner = isActiveSprite ? DataUtility.GetInnerUV(activeSprite) : new Vector4();

            var uvTopLeft = isActiveSprite ? new Vector2(outer.x, outer.y) : Vector2.zero;
            var uvBottomLeft = isActiveSprite ? new Vector2(outer.x, outer.w) : new Vector2(0, 1);
            var uvTopCenterLeft = isActiveSprite ? new Vector2(inner.x, inner.y) : new Vector2(0.5f, 0);
            var uvTopCenterRight = isActiveSprite ? new Vector2(inner.z, inner.y) : new Vector2(0.5f, 0);
            var uvBottomCenterLeft = isActiveSprite ? new Vector2(inner.x, inner.w) : new Vector2(0.5f, 1);
            var uvBottomCenterRight = isActiveSprite ? new Vector2(inner.z, inner.w) : new Vector2(0.5f, 1);
            var uvTopRight = isActiveSprite ? new Vector2(outer.z, outer.y) : new Vector2(1, 0);
            var uvBottomRight = isActiveSprite ? new Vector2(outer.z, outer.w) : Vector2.one;

            _startUvs = new[] { uvTopLeft, uvBottomLeft, uvBottomCenterLeft, uvTopCenterLeft };
            _middleUvs = new[] { uvTopCenterLeft, uvBottomCenterLeft, uvBottomCenterRight, uvTopCenterRight };
            _endUvs = new[] { uvTopCenterRight, uvBottomCenterRight, uvBottomRight, uvTopRight };
            _fullUvs = new[] { uvTopLeft, uvBottomLeft, uvBottomRight, uvTopRight };
        }

        public Vector2[] GetUvs(SegmentType type)
        {
            return type switch
            {
                SegmentType.Start => _startUvs,
                SegmentType.End => _endUvs,
                SegmentType.Full => _fullUvs,
                _ => _middleUvs
            };
        }
    }
}
