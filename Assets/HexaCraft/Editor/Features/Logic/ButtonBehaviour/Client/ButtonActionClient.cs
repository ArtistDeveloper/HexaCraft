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
            var materialReceiver = new MaterialChange(_presenter);
            var materialCommand = new MaterialChangeCommand(materialReceiver);
            _invoker.SetCommand(ToggleButton.MaterialChange, materialCommand);

            var objectReceiver = new ObjectSelection(_presenter);
            var objectCommand = new ObjectSelectionCommand(objectReceiver);
            _invoker.SetCommand(ToggleButton.ObjectSelection, objectCommand);

            var generateReceiver = new GridGeneration(_presenter);
            var generateCommand = new GridGenerationCommand(generateReceiver);
            _invoker.SetCommand(Button.GridGeneration, generateCommand);

            var pathReceiver = new PathManipulation(_presenter);
            var pathCommand = new PathManipulationCommand(pathReceiver);
            _invoker.SetCommand(ToggleButton.PathEditing, pathCommand);
        }

        public void ButtonClicked<TEnum>(TEnum type) where TEnum : Enum
        {
            _invoker.ExecuteCommand(type);
        }
    }
}
