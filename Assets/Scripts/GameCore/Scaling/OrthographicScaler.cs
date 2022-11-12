using UnityEngine;

namespace Simple.Nonogram.GameCore.Scaling
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class OrthographicScaler : MonoBehaviour
    {
        private const float LogarithmBase = 2f;

        public enum ScaleMode
        {
            ConstantPixelSize,
            ScaleWithScreenSize,
            ConstantPhysicalSize
        }

        public enum ScreenMatchMode
        {
            MatchWidthOrHeight = 0,
            Expand = 1,
            Shrink = 2
        }

        public enum Unit
        {
            Centimeters,
            Millimeters,
            Inches,
            Points,
            Picas
        }

        [Tooltip("Определяет, как масштабируются элементы пользовательского интерфейса на Canvas.")]
        [SerializeField] private ScaleMode _uiScaleMode = ScaleMode.ConstantPixelSize;

        [Tooltip("Если спрайт имеет параметр 'Pixels Per Unit', то один пиксель в спрайте будет покрывать одну единицу в пользовательском интерфейсе.")]
        [SerializeField] private float _referencePixelsPerUnit = 100f;

        [Tooltip("Масштабирует все элементы пользовательского интерфейса на Canvas с помощью этого коэффициента.")]
        [SerializeField] private float _scaleFactor = 1f;

        [Tooltip("Разрешение, для которого разработан макет пользовательского интерфейса. Если разрешение экрана больше, пользовательский интерфейс будет увеличен, а если меньше, пользовательский интерфейс будет уменьшен. Это делается в соответствии с режимом Screen Match Mode.")]
        [SerializeField] private Vector2 _referenceResolution = new Vector2(800f, 600f);

        [Tooltip("Режим, используемый для масштабирования области Canvas, если соотношение сторон текущего разрешения не соответствует эталонному разрешению.")]
        [SerializeField] private ScreenMatchMode _screenMatchMode = ScreenMatchMode.MatchWidthOrHeight;

        [Tooltip("Определяет, использует ли масштабирование ширину или высоту в качестве эталона или их сочетание.")]
        [Range(0, 1), SerializeField] private float _matchWidthOrHeight = 0f;

        [SerializeField] private bool _centerizePosition = true;

        [Tooltip("Физическая единица, в которой указываются позиции и размеры.")]
        [SerializeField] private Unit _physicalUnit = Unit.Points;

        [Tooltip("Предполагаемое значение DPI, если значение DPI экрана неизвестно.")]
        [SerializeField] private float _fallbackScreenDpi = 96f;

        [Tooltip("Количество пикселей на дюйм, используемых для спрайтов, для которых параметр 'Pixels Per Unit' соответствует параметру 'Reference Pixels Per Unit'.")]
        [SerializeField] private float _defaultSpriteDpi = 96f;

        private Camera _camera;
        private float _previousOrthographicSize = 1f;
        private float _previousReferencePixelsPerUnit = 100f;

        public ScaleMode UiScaleMode
        {
            get => _uiScaleMode;
            set => _uiScaleMode = value;
        }

        public float ReferencePixelsPerUnit
        {
            get => _referencePixelsPerUnit;
            set => _referencePixelsPerUnit = value;
        }

        public float ScaleFactor
        {
            get => _scaleFactor;
            set => _scaleFactor = value;
        }

        public Vector2 ReferenceResolution
        {
            get => _referenceResolution;
            set => _referenceResolution = value;
        }

        public ScreenMatchMode ScreenMode
        {
            get => _screenMatchMode;
            set => _screenMatchMode = value;
        }

        public float MatchWidthOrHeight
        {
            get => _matchWidthOrHeight;
            set => _matchWidthOrHeight = value;
        }

        public bool CenterizePosition
        {
            get => _centerizePosition;
            set => _centerizePosition = value;
        }

        public Unit PhysicalUnit
        {
            get { return _physicalUnit; }
            set { _physicalUnit = value; }
        }

        public float FallbackScreenDpi
        {
            get { return _fallbackScreenDpi; }
            set { _fallbackScreenDpi = value; }
        }

        public float DefaultSpriteDpi
        {
            get { return _defaultSpriteDpi; }
            set { _defaultSpriteDpi = value; }
        }

        public float ScreenScale => _previousOrthographicSize * _previousReferencePixelsPerUnit;
        public Vector2 ScreenSize => new Vector2(Screen.width, Screen.height) / ScreenScale;

        private void Awake()
        {
            UpdateCamera();

            if (_centerizePosition)
            {
                var position = transform.localPosition;
                position.x = Screen.width / (2 * ScreenScale);
                position.y = Screen.height / (2 * ScreenScale);
                transform.localPosition = position;
            }
        }

        private void Update()
        {
            Handle();
        }

        private void OnEnable()
        {
            UpdateCamera();
        }

        private void OnDisable()
        {
            SetOrthographicSize(1f);
            SetReferencePixelsPerUnit(100f);
        }

        private void UpdateCamera()
        {
            _camera = GetComponent<Camera>();

            Handle();
        }

        private void Handle()
        {
            if (_camera == null)
                return;

            switch (_uiScaleMode)
            {
                case ScaleMode.ConstantPixelSize:
                    HandleConstantPixelSize();
                    break;
                case ScaleMode.ScaleWithScreenSize:
                    HandleScaleWithScreenSize();
                    break;
                case ScaleMode.ConstantPhysicalSize:
                    HandleConstantPhysicalSize();
                    break;
            }
        }

        private void HandleConstantPixelSize()
        {
            SetOrthographicSize(_scaleFactor);
            SetReferencePixelsPerUnit(_referencePixelsPerUnit);
        }

        private void HandleScaleWithScreenSize()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);

            var orthographicSize = _screenMatchMode == ScreenMatchMode.MatchWidthOrHeight
                ? GetSizeByMatchWidthOrHeight(screenSize)
                : _screenMatchMode == ScreenMatchMode.Expand
                    ? GetSizeByExpand(screenSize)
                    : _screenMatchMode == ScreenMatchMode.Shrink
                        ? GetSizeByShrink(screenSize)
                        : 0f;

            SetOrthographicSize(orthographicSize);
            SetReferencePixelsPerUnit(_referencePixelsPerUnit);
        }

        private float GetSizeByMatchWidthOrHeight(Vector2 screenSize)
        {
            #region Description
            // Перед тем, как взять среднее значение высоты и ширины, происходит их логарифмирование.
            // Затем происходит преобразование обратно в исходное пространство.
            // Преобразование в логарифмическое пространство и из него необходимо для лучшего результата.
            // Если одна ось имеет разрешение 2x, а другая - 0.5x, оно должно выравняться, если значение widthOrHeight равно 0.5.
            // В обычном пространстве среднее значение будет (0.5 + 2) / 2 = 1.25.
            // В логарифмическом пространстве среднее значение равно (-1 + 1) / 2 = 0.
            #endregion

            float orthographicSize;
            float logWidth = Mathf.Log(screenSize.x / _referenceResolution.x, LogarithmBase);
            float logHeight = Mathf.Log(screenSize.y / _referenceResolution.y, LogarithmBase);
            float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, _matchWidthOrHeight);

            orthographicSize = Mathf.Pow(LogarithmBase, logWeightedAverage);

            return orthographicSize;
        }

        private float GetSizeByExpand(Vector2 screenSize)
        {
            return Mathf.Min(screenSize.x / _referenceResolution.x, screenSize.y / _referenceResolution.y);
        }

        private float GetSizeByShrink(Vector2 screenSize)
        {
            return Mathf.Max(screenSize.x / _referenceResolution.x, screenSize.y / _referenceResolution.y);
        }

        private void HandleConstantPhysicalSize()
        {
            var currentDpi = Screen.dpi;
            var dpi = currentDpi == 0f ? _fallbackScreenDpi : currentDpi;
            var targetDpi = GetTargetDpi(_physicalUnit);

            var orthographicSize = dpi / targetDpi;
            var referencePixelsPerUnit = _referencePixelsPerUnit * targetDpi / _defaultSpriteDpi;

            SetOrthographicSize(orthographicSize);
            SetReferencePixelsPerUnit(referencePixelsPerUnit);
        }

        private float GetTargetDpi(Unit unit)
        {
            var targetDpi = 1f;

            switch (unit)
            {
                case Unit.Centimeters:
                    targetDpi = 2.54f;
                    break;
                case Unit.Millimeters:
                    targetDpi = 25.4f;
                    break;
                case Unit.Inches:
                    targetDpi = 1f;
                    break;
                case Unit.Points:
                    targetDpi = 72f;
                    break;
                case Unit.Picas:
                    targetDpi = 6f;
                    break;
            }

            return targetDpi;
        }

        private void SetOrthographicSize(float orthographicSize)
        {
            _camera.orthographicSize = Screen.height / (2 * orthographicSize);
            _previousOrthographicSize = orthographicSize;
        }

        private void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
        {
            if (referencePixelsPerUnit == _previousReferencePixelsPerUnit)
                return;

            _previousReferencePixelsPerUnit = referencePixelsPerUnit;
        }
    }
}