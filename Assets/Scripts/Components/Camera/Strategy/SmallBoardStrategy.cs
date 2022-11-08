using System;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class SmallBoardStrategy : BoundsStrategy
    {
        public SmallBoardStrategy(Camera camera, CameraBounds cameraBounds) :
            base(camera, cameraBounds, new Tuple<bool, bool>(true, true))
        { }
    }
}
