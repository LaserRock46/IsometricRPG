using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.SharedScripts
{
    [CreateAssetMenu(fileName = "New TauntsSO", menuName = "Project/SharedScripts/TauntsSO")]
    [System.Serializable]
    public class TauntsSO : ScriptableObject
    {
        [SerializeField][Range(0,100)] private int _chanceToEvoke;
        [SerializeField] private string[] _taunts;
        private TauntsDisplay _tauntsDisplay;

        public string[] Taunts { get => _taunts; private set => _taunts = value; }
        public TauntsDisplay TauntsDisplay { get => _tauntsDisplay; set => _tauntsDisplay = value; }
        public int ChanceToEvoke { get => _chanceToEvoke; private set => _chanceToEvoke = value; }

        public bool CanEvoke()
        {
            return Random.Range(0, 100) < _chanceToEvoke;
        }
    }
}
