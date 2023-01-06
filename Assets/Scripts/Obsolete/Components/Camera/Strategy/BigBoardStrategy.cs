using System;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class BigBoardStrategy : BoundsStrategy
    {
        public BigBoardStrategy(Camera camera, CameraBounds cameraBounds) : base(camera, cameraBounds, new Tuple<bool, bool>(false, false)) { }
    }
}