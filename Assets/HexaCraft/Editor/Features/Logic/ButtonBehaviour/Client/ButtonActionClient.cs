using System;
using System.Reflection;
using Unity.VisualScripting;

namespace HexaCraft
{
    public class ButtonActionClient
    {
        private CommandInvoker _invoker;

        private HCPresenter _presenter;


        public ButtonActionClient(HCPresenter presenter)
        {
            _presenter = presenter;
            _invoker = new CommandInvoker(_presenter);
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

            var pathManipulationReceiver = new PathManipulation(_presenter);
            var pathManipulationCommand = new PathManipulationCommand(pathManipulationReceiver);
            _invoker.SetCommand(ToggleButton.PathEditing, pathManipulationCommand);

            var pathGeneration = new PathGeneration(_presenter);
            var pathGenerationCommand = new PathGenerationCommand(pathGeneration);
            _invoker.SetCommand(Button.PathGeneration, pathGenerationCommand);
        }

        public void ButtonClicked(Button type)
        {
            _invoker.ExecuteCommand(type);
        }

        public void ToggleButtonClicked(ToggleButton type)
        {
            bool isActive = _presenter.GetToggleActiveState(type);
            _invoker.ExecuteCommand(type, isActive);
        }
    }
}
