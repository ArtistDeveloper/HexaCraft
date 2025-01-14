using UnityEngine;

namespace HexaCraft
{
    public class GridGeneration
    {
        private HCPresenter _presenter;

        private HexGridGenerator _hexGridGenerator;

        public GridGeneration(HCPresenter presenter)
        {
            _presenter = presenter;
            _hexGridGenerator = new HexGridGenerator();
        }

        public void GenerateGrid()
        {
            _hexGridGenerator.GenerateGrid(
                _presenter.GetHexPrefab(),
                _presenter.GetGridRadius(),
                _presenter.GetHexCircumscribedRadius()
                );
        }
    }
}
