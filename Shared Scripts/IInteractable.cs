using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.SharedScripts
{
    public interface IInteractable
    {
        public string Description { get; }
        Vector3 Position { get; }
        public void Interact();
    }
}
