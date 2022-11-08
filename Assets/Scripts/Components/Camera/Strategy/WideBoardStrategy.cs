using System;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class WideBoardStrategy : BoundsStrategy
    {
        public WideBoardStrategy(Camera camera, CameraBounds cameraBounds) :
            base(camera, cameraBounds, new Tuple<bool, bool>(false, true))
        { }
    }
}
