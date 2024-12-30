using UnityEngine;

public interface IPresenterFactory
{
    public IPresenter CreatePresenter(IView view);
}
