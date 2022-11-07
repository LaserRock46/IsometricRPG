using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.SoundSystem
{
    [CreateAssetMenu(fileName = "New Universal Weapon Sound Set", menuName = "Project/SoundSystem/Universal Weapon Sound Set")]
    [System.Serializable]
    public class UniversalWeaponSoundSetSO : ScriptableObject
    {
        [SerializeField] private AudioClip _reload;
        [SerializeField] private AudioClip _draw;
        [SerializeField] private AudioClip _holster;
    }
}
