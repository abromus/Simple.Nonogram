using System;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class HighBoardStrategy : BoundsStrategy
    {
        public HighBoardStrategy(Camera camera, CameraBounds cameraBounds) :
            base(camera, cameraBounds, new Tuple<bool, bool>(true, false))
        { }
    }
}
