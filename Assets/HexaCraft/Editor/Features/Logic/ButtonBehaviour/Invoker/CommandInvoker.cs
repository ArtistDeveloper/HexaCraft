using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class CommandInvoker
    {
        private readonly Dictionary<Enum, ICommand> _commands;
        private readonly Dictionary<ISceneCommand, Action<SceneView>> _sceneActionWrappers;

        public CommandInvoker()
        {
            _commands = new Dictionary<Enum, ICommand>();
            _sceneActionWrappers = new Dictionary<ISceneCommand, Action<SceneView>>();
        }

        public void SetCommand(Enum type, ICommand command)
        {
            _commands[type] = command;
        }

        public void ExecuteCommand(Enum type)
        {
            if (!_commands.TryGetValue(type, out var command))
            {
                Debug.LogAssertion("No Command");
                return;
            }

            if (command is ISceneCommand sceneCommand)
            {
                ExecuteSceneCommand(sceneCommand);
            }
            else if (command is IBasicCommand basicCommand)
            {
                basicCommand.Execute();
            }
        }

        private void ExecuteSceneCommand(ISceneCommand command)
        {
            if (_sceneActionWrappers.ContainsKey(command))
            {
                SceneView.duringSceneGui -= _sceneActionWrappers[command];
                _sceneActionWrappers.Remove(command);
            }
            else
            {
                var wrapper = CreateSceneActionWrapper(command);
                _sceneActionWrappers[command] = wrapper;
                SceneView.duringSceneGui += wrapper;
                SceneView.RepaintAll();
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
