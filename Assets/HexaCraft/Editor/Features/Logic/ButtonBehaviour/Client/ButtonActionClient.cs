using System;
using System.Reflection;

namespace HexaCraft
{
    public class ButtonActionClient
    {
        private CommandInvoker _invoker;

        private HCPresenter _presenter;


        public ButtonActionClient(HCPresenter presenter)
        {
            _presenter = presenter;
            _invoker = new CommandInvoker();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            MaterialChange materialReceiver = new MaterialChange(_presenter);
            MaterialEditingCommand materialCommand = new MaterialEditingCommand(materialReceiver);
            _invoker.SetCommand(ToggleButton.MaterialEditing, materialCommand);

            ObjectSelection objectReceiver = new ObjectSelection(_presenter);
            ObjectSelectingCommand objectCommand = new ObjectSelectingCommand(objectReceiver);
            _invoker.SetCommand(ToggleButton.ObjectSelecting, objectCommand);

            // // Generate Grid (Normal Command)
            // var generateReceiver = new GenerateGridReceiver();
            // var generateCommand = new GenerateGridCommand(generateReceiver);
            // _invoker.SetCommand(ButtonType.GenerateGrid, generateCommand);
        }

        public void ButtonClicked<TEnum>(TEnum type) where TEnum : Enum
        {
            _invoker.ExecuteCommand(type);
        }
    }
}
