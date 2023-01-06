using System;
using Simple.Nonogram.Extension;

namespace Simple.Nonogram.Core
{
    public class Cell
    {
        private CellState _state;

        public CellState State => _state;

        public Cell(CellState state)
        {
            _state = state;
        }

        public Cell() : this(CellState.Blank) { }

        public void SetState(CellState state)
        {
            if(!Enum.IsDefined(typeof(CellState), state))
            {
                DebugExtension.LogError($"InvalidEnumArgumentException: ({state} not found in {typeof(CellState)}");
                return;
            }

            if (State != state)
            {
                _state = state;
            }
        }
    }
}
