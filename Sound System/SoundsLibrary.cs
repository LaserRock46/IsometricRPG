using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Project.SoundSystem
{
    [CreateAssetMenu(fileName = "New Sounds Library", menuName = "Project/SoundSystem/Sounds Library")]
    [System.Serializable]
    public class SoundsLibrary : ScriptableObject
    {
        private Dictionary<PhysicMaterial, int> _soundsSetDictionary;     

        public SoundsSet GetSoundsSet(PhysicMaterial key)
        {          
            return _soundsSets[_soundsSetDictionary[key]];
        }

        [SerializeField] private SoundsSet[] _soundsSets;


        [System.Serializable]
        public class SoundsSet
        {  
            [AllowNesting][ReadOnly]
            [SerializeField] private string _setName = "Empty";

            [AllowNesting][HorizontalLine] 

            [OnValueChanged(nameof(ChangeSetName))]
            [SerializeField] private PhysicMaterial _key;
            public PhysicMaterial Keys { get => _key; private set => _key = value; }

            [AllowNesting][HorizontalLine] 

            [SerializeField] private AudioClip[] _footstepWalk;
            [SerializeField] private AudioClip[] _footstepRun;
            [SerializeField] private AudioClip[] _footstepCrouch;
            [SerializeField] private AudioClip[] _footstepLanding;
            [SerializeField] private AudioClip[] _impact;
            [SerializeField] private AudioClip[] _heavyImpact;

            public AudioClip[] FootstepWalk { get => _footstepWalk; private set => _footstepWalk = value; }
            public AudioClip[] FootstepRun { get => _footstepRun; private set => _footstepRun = value; }
            public AudioClip[] FootstepCrouch { get => _footstepCrouch; private set => _footstepCrouch = value; }
            public AudioClip[] FootstepLanding { get => _footstepLanding; private set => _footstepLanding = value; }
            public AudioClip[] Impact { get => _impact; private set => _impact = value; }
            public AudioClip[] HeavyImpact { get => _heavyImpact; private set => _heavyImpact = value; }
       
            public AudioClip GetRandomFromVariant(AudioClip[] variant)
            {
                int index = Random.Range(0, variant.Length);
                return variant[index];
            }
            private void ChangeSetName()
            {
                _setName = _key.name;
            }
        }

        private void OnEnable()
        {
            if (_soundsSets == null) return;
            _soundsSetDictionary = new Dictionary<PhysicMaterial, int>();

            for (int set = 0; set < _soundsSets.Length; set++)
            {
                _soundsSetDictionary.Add(_soundsSets[set].Keys,set);
            }
        }
       
    }
}
