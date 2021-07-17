using System;

using UnityEngine;

namespace Simple.Nonogram
{
    public class Cell
    {
        public CellState State { get; private set; }

        public Cell(CellState state)
        {
            State = state;
        }

        public Cell() : this(CellState.Empty) { }

        public void SetType(CellState state)
        {
            if (Enum.IsDefined(typeof(CellState), state) && State != state)
            {
                State = state;
            }
            else
            {
                Debug.LogError($"{DateTime.Now}. Error CellType! {state} not found in {typeof(CellState)}");
            }
        }
    }
}
