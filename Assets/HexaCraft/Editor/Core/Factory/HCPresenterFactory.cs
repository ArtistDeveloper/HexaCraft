using UnityEngine;

namespace HexaCraft
{
    public class HCPresenterFactory
    {
        public IPresenter CreatePresenter(HCGenerationEditor view)
        {
            return new HCPresenter(view);
        }
    }
}
