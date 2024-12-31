using UnityEngine;

namespace HexaCraft
{
    public class HCPresenterFactory : IPresenterFactory
    {
        public IPresenter CreatePresenter(IView view)
        {
            return new HCPresenter(view);
        }
    }
}
