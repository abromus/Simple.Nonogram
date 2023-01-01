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
            _offsetX = _board.SpriteSize.Width / 2;
            _offsetY = _board.SpriteSize.Height / 2;
        }

        public void Draw()
        {
            var auxiliaryLines = "Auxiliary lines";
            var verticalLines = "Vertical lines";
            var horizontalLines = "Horizontal lines";
            var userBoardName = "UserBoard line";
            var numberBoardName = "NumberBoard line";

            var auxiliaryLine = new GameObject(auxiliaryLines);
            var verticalLinesContainer = new GameObject(verticalLines);
            var horizontalLinesContainer = new GameObject(horizontalLines);

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
            var stepDivision = 5;

            var tempCells = verticalDirection == Direction.Positive ? cells : ArrayExtension.Transpose(cells);
            var count = (int)Mathf.Ceil((float)tempCells.GetLength((int)Dimension.Height) / stepDivision - 1);
            var lineIndex = 0;

            for (int i = 0; i < count; i++)
            {
                var line = GetLine($"{name} {i}", container);

                lineIndex++;
                DrawLine(line, tempCells, lineIndex, stepDivision, verticalDirection, horizonralDirection);
            }
        }

        private LineRenderer GetLine(string name, GameObject linesContainer)
        {
            var line = new GameObject(name).AddComponent<LineRenderer>();

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
            var firstXIndex = 0;
            var firstPoint = 0;
            var secondPoint = 1;

            var startDirectionOffsetX = vertical == Direction.Negative && horizonral == Direction.Positive ? Direction.Positive : Direction.Negative;
            var endDirectionOffsetX = vertical == Direction.Negative && horizonral == Direction.Negative ? Direction.Positive : Direction.Negative;
            var startDirectionOffsetY = vertical == Direction.Positive && horizonral == Direction.Positive ? Direction.Negative : Direction.Positive;
            var endDirectionOffsetY = vertical == Direction.Positive && horizonral == Direction.Negative ? Direction.Negative : Direction.Positive;
            var lastXIndex = (int)(cells.GetLength((int)Dimension.Width) - 1);
            var yIndex = lineIndex * stepDivision;

            line.SetPosition(secondPoint, GetPoint(cells, firstXIndex, yIndex, startDirectionOffsetX, startDirectionOffsetY));
            line.SetPosition(firstPoint, GetPoint(cells, lastXIndex, yIndex, endDirectionOffsetX, endDirectionOffsetY));
        }

        private Vector3 GetPoint(ICell[,] cells, int xIndex, int yIndex, Direction directionOffsetX, Direction directionOffsetY)
        {
            var epsilon = 0.01f;

            var x = ((MonoBehaviour)cells[xIndex, yIndex]).transform.position.x + (float)directionOffsetX * _offsetX;
            var y = ((MonoBehaviour)cells[xIndex, yIndex]).transform.position.y + (float)directionOffsetY * _offsetY;
            var z = ((MonoBehaviour)cells[xIndex, yIndex]).transform.position.z - epsilon;

            return new Vector3(x, y, z);
        }
    }
}
