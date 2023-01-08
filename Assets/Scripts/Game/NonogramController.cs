using System.Collections.Generic;
using Simple.Nonogram.Nonograms;
using Simple.Nonogram.Settings;

namespace Simple.Nonogram.Game
{
    public class NonogramController
    {
        private NonogramSettings _nonogramSettings;
        private NonogramMeta _nonogramMeta;
        private List<NonogramInfo> _tutorialNonograms;

        public List<NonogramInfo> TutorialNonograms => _tutorialNonograms;

        public NonogramInfo CurrentNonogram { get; internal set; }

        public NonogramController(NonogramSettings nonogramSettings)
        {
            _nonogramSettings = nonogramSettings;
            _nonogramMeta = new NonogramMeta();

            GetTutorialNonograms();
        }

        private void GetTutorialNonograms()
        {
            _tutorialNonograms = _nonogramMeta.Load(_nonogramSettings.PathToTutorialFolder);
        }
    }
}
