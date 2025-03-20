using UnityEngine;

namespace HexaCraft
{
    public interface IPresenterFactory
    {
        public IPresenter CreatePresenter(IHCMainView view);
    }
}
