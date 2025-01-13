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
            // Material Editing (Scene Command)
            MaterialEditingReceiver materialReceiver = new MaterialEditingReceiver(_presenter);
            MaterialEditingCommand materialCommand = new MaterialEditingCommand(materialReceiver);
            _invoker.SetCommand(ToggleButton.MaterialEditing, materialCommand);

            // // Object Selecting (Scene Command)
            // var objectReceiver = new ObjectSelectingReceiver();
            // var objectCommand = new ObjectSelectingCommand(objectReceiver);
            // _invoker.SetCommand(ToggleButton.ObjectSelecting, objectCommand);

            // // Generate Grid (Normal Command)
            // var generateReceiver = new GenerateGridReceiver();
            // var generateCommand = new GenerateGridCommand(generateReceiver);
            // _invoker.SetCommand(ButtonType.GenerateGrid, generateCommand);
        }


        public void ButtonClicked<TEnum>(TEnum type) where TEnum : Enum
        {
            _invoker.ExecuteCommand(type);
        }

        private bool IsSceneRegistrationRequire<TEnum>(TEnum type) where TEnum : Enum
        {
            var memberInfo = type.GetType().GetMember(type.ToString())[0];
            var attribute = memberInfo.GetCustomAttribute<SceneRegistrationAttribute>();
            return attribute?.RequiresRegistration ?? false;
        }
    }
}
