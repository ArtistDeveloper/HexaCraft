using System;
using UnityEngine;

namespace HexaCraft
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SceneRegistrationAttribute : Attribute
    {
        public bool RequiresRegistration { get; }

        public SceneRegistrationAttribute(bool requiresRegistration)
        {
            RequiresRegistration = requiresRegistration;
        }
    }
}