using UnityEngine;

namespace HexaCraft
{
    public class ButtonActionClient
    {
        private MaterialEditingReceiver _matEditReceiver;
        

        private MaterialEditingCommand _matCommand;

        public ButtonActionClient()
        {
            MakeReceiverAndCommand();
        }

        public void ButtonPushed()
        {

        }

        private void MakeReceiverAndCommand()
        {
            _matEditReceiver = new MaterialEditingReceiver();
            _matCommand = new MaterialEditingCommand(_matEditReceiver);
        }

        private void IsSceneRegistrationRequire()
        {
            
        }
    }
}
