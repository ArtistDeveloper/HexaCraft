using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class CommandInvoker
    {
        private HCPresenter _presenter;

        private readonly Dictionary<Enum, ICommand> _commands;

        private readonly Dictionary<ISceneCommand, Action<SceneView>> _sceneActionWrappers;


        public CommandInvoker(HCPresenter presenter)
        {
            _commands = new Dictionary<Enum, ICommand>();
            _sceneActionWrappers = new Dictionary<ISceneCommand, Action<SceneView>>();
            _presenter = presenter;
            _presenter.disposeAction += ClearAllSceneActions;
        }

        public void SetCommand(Enum type, ICommand command)
        {
            _commands[type] = command;
        }

        public void ExecuteCommand(Enum type, bool? isActive = null)
        {
            if (!_commands.TryGetValue(type, out var command))
            {
                Debug.LogAssertion("No Command");
                return;
            }

            if (command is IBasicCommand basicCommand)
            {
                basicCommand.Execute();
            }
            else if (command is ISceneCommand sceneCommand && isActive.HasValue)
            {
                HandleSceneCommand(sceneCommand, isActive.Value);
            }
        }

        private void HandleSceneCommand(ISceneCommand command, bool isActive)
        {
            if (isActive)
            {
                if (!_sceneActionWrappers.ContainsKey(command))
                {
                    RegisterSceneCommand(command);
                }
            }
            else
            {
                UnregisterSceneCommand(command);
            }
        }

        private void RegisterSceneCommand(ISceneCommand command)
        {
            var wrapper = CreateSceneActionWrapper(command);
            _sceneActionWrappers[command] = wrapper;
            SceneView.duringSceneGui += wrapper;
            SceneView.RepaintAll();
        }

        private void UnregisterSceneCommand(ISceneCommand command)
        {
            if (_sceneActionWrappers.ContainsKey(command))
            {
                SceneView.duringSceneGui -= _sceneActionWrappers[command];
                _sceneActionWrappers.Remove(command);
            }
        }

        private Action<SceneView> CreateSceneActionWrapper(ISceneCommand command)
        {
            return (SceneView sceneView) => command.Execute(sceneView, Event.current);
        }

        public void ClearAllSceneActions()
        {
            foreach (var wrapper in _sceneActionWrappers.Values)
            {
                SceneView.duringSceneGui -= wrapper;
            }
            _sceneActionWrappers.Clear();
        }

        /// <summary>
        /// 필요시 Attribute 활용하여 SceneView 등록 필요한 로직 구분
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsSceneRegistrationRequire<TEnum>(TEnum type) where TEnum : Enum
        {
            var memberInfo = type.GetType().GetMember(type.ToString())[0];
            var attribute = memberInfo.GetCustomAttribute<SceneRegistrationAttribute>();
            return attribute?.RequiresRegistration ?? false;
        }
    }
}
