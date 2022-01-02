using Simple.Nonogram.Core;
using Simple.Nonogram.Extension;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class AuxiliaryLinesRenderer
    {
        private Color _start;
        private Color _end;

        private readonly Board _board;
        private readonly Transform _transform;
        private readonly Material _material;
        private readonly float _width;
        private readonly float _offsetX;
        private readonly float _offsetY;

        public AuxiliaryLinesRenderer(Board board, Transform transform, Material material, Color start, Color end)
        {
            _board = board;
            _transform = transform;
            _material = material;
            _start = start;
            _end = end;
            _width = 0.05f;
            _offsetX = _board.SpriteSize.Width / (int)Number.Two;
            _offsetY = _board.SpriteSize.Height / (int)Number.Two;
        }

        public void Draw()
        {
            const string auxiliaryLines = "Auxiliary lines";
            const string verticalLines = "Vertical lines";
            const string horizontalLines = "Horizontal lines";
            const string userBoardName = "UserBoard line";
            const string numberBoardName = "NumberBoard line";

            GameObject auxiliaryLine = new GameObject(auxiliaryLines);
            GameObject verticalLinesContainer = new GameObject(verticalLines);
            GameObject horizontalLinesContainer = new GameObject(horizontalLines);

            auxiliaryLine.transform.parent = _transform;
            verticalLinesContainer.transform.parent = auxiliaryLine.transform;
            horizontalLinesContainer.transform.parent = auxiliaryLine.transform;

            DrawOnBoard(verticalLinesContainer, horizontalLinesContainer, userBoardName);
            DrawOnNumberBoard(verticalLinesContainer, horizontalLinesContainer, numberBoardName);
        }

        private void DrawOnBoard(GameObject verticalLinesContainer, GameObject horizontalLinesContainer, string name)
        {
            DrawVerticalLines(_board.UserBoardView, verticalLinesContainer, name, Direction.Negative);
            DrawHorizontalLines(_board.UserBoardView, horizontalLinesContainer, name, Direction.Negative);
        }

        private void DrawOnNumberBoard(GameObject verticalLinesContainer, GameObject horizontalLinesContainer, string name)
        {
            DrawVerticalLines(_board.Top, verticalLinesContainer, name, Direction.Positive);
            DrawHorizontalLines(_board.Left, horizontalLinesContainer, name, Direction.Positive);
        }

        private void DrawVerticalLines(ICell[,] cells, GameObject verticalLinesContainer, string verticalLineName, Direction direction)
        {
            DrawLines(cells, verticalLinesContainer, verticalLineName, Direction.Positive, direction);
        }

        private void DrawHorizontalLines(ICell[,] cells, GameObject horizontalLinesContainer, string horizontalLineName, Direction direction)
        {
            DrawLines(cells, horizontalLinesContainer, horizontalLineName, Direction.Negative, direction);
        }

        private void DrawLines(ICell[,] cells, GameObject container, string name, Direction verticalDirection = Direction.Positive, Direction horizonralDirection = Direction.Positive)
        {
            const int stepDivision = (int)Number.Five;

            ICell[,] tempCells = verticalDirection == Direction.Positive ? cells : ArrayExtension.Transpose(cells);
            int count = (int)Mathf.Ceil((float)tempCells.GetLength((int)Dimension.Height) / stepDivision - (int)Number.One);
            int lineIndex = (int)Number.Zero;

            for (int i = (int)Number.Zero; i < count; i++)
            {
                LineRenderer line = GetLine($"{name} {i}", container);

                lineIndex++;
                DrawLine(line, tempCells, lineIndex, stepDivision, verticalDirection, horizonralDirection);
            }
        }

        private LineRenderer GetLine(string name, GameObject linesContainer)
        {
            LineRenderer line = new GameObject(name).AddComponent<LineRenderer>();

            line.transform.parent = linesContainer.transform;
            line.material = _material;
            line.startColor = _start;
            line.endColor = _end;
            line.startWidth = _width;
            line.endWidth = _width;

            return line;
        }

        private void DrawLine(LineRenderer line, ICell[,] cells, int lineIndex, int stepDivision, Direction vertical, Direction horizonral)
        {
            const int firstXIndex = (int)Number.Zero;
            const int firstPoint = (int)Number.Zero;
            const int secondPoint = (int)Number.One;

            Direction startDirectionOffsetX = vertical == Direction.Negative && horizonral == Direction.Positive ? Direction.Positive : Direction.Negative;
            Direction endDirectionOffsetX = vertical == Direction.Negative && horizonral == Direction.Negative ? Direction.Positive : Direction.Negative;
            Direction startDirectionOffsetY = vertical == Direction.Positive && horizonral == Direction.Positive ? Direction.Negative : Direction.Positive;
            Direction endDirectionOffsetY = vertical == Direction.Positive && horizonral == Direction.Negative ? Direction.Negative : Direction.Positive;
            int lastXIndex = (int)(cells.GetLength((int)Dimension.Width) - Number.One);
            int yIndex = lineIndex * stepDivision;

            line.SetPosition(secondPoint, GetPoint(cells, firstXIndex, yIndex, startDirectionOffsetX, startDirectionOffsetY));
            line.SetPosition(firstPoint, GetPoint(cells, lastXIndex, yIndex, endDirectionOffsetX, endDirectionOffsetY));
        }

        private Vector3 GetPoint(ICell[,] cells, int xIndex, int yIndex, Direction directionOffsetX, Direction directionOffsetY)
        {
            const float epsilon = 0.01f;

            float x = ((MonoBehaviour)cells[xIndex, yIndex]).transform.position.x + (float)directionOffsetX * _offsetX;
            float y = ((MonoBehaviour)cells[xIndex, yIndex]).transform.position.y + (float)directionOffsetY * _offsetY;
            float z = ((MonoBehaviour)cells[xIndex, yIndex]).transform.position.z - epsilon;

            return new Vector3(x, y, z);
        }
    }
}
