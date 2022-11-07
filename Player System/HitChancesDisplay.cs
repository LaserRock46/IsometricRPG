using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Project.PlayerSystem
{
    public class HitChancesDisplay : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private CharacterSystem.Character _character;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private TMP_Text[] _tags;
        [SerializeField] private Color _hitchancesColor = Color.white;
        [SerializeField] [Range(1,100)] private float _alphaOverValueMultiplier;

        [SerializeField] private Canvas _canvas;
        private RectTransform _canvasRect;      

        #endregion

        #region Functions
        
        #endregion

        #region Methods
        void Start()
        {
            _canvasRect = _canvas.GetComponent<RectTransform>();     
        }
        void Update()
        {
            StateUpdate();
        }
        void StateUpdate()
        {
            if (_character != null)
            {
                if (_character.WeaponSystemNode.WeaponWorldObject != null)
                {
                    if (_canvas.gameObject.activeSelf == false)
                    {
                        _canvas.gameObject.SetActive(true);
                    }
                    OverheadsUpdate();
                }
                else if (_canvas.gameObject.activeSelf == true)
                {
                    _canvas.gameObject.SetActive(false);
                }
                
            }
        }
        void OverheadsUpdate()
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                if(i < _character.WeaponSystemNode.WeaponHitscan.PossibleTargets.Count)
                {                  
                    if(_tags[i].enabled == false && Utilities.UIFunctions.IsBehindScreen(_character.WeaponSystemNode.WeaponHitscan.PossibleTargets[i].transform.position) == false)
                    {
                        _tags[i].enabled = true;
                    }
                    string chances = string.Format("{0}%", _character.WeaponSystemNode.WeaponHitscan.PossibleTargetsHitchance[i]);
                    _tags[i].text = chances;
                    _hitchancesColor.a = (float)_character.WeaponSystemNode.WeaponHitscan.PossibleTargetsHitchance[i] / _alphaOverValueMultiplier;
                    _tags[i].color = _hitchancesColor;
                  
                    _tags[i].rectTransform.anchoredPosition = Utilities.UIFunctions.WorldPositionToCanvas(_character.WeaponSystemNode.WeaponHitscan.PossibleTargets[i].transform.position + _offset, _canvasRect);      
                  
                }
                else if (_tags[i].enabled == true)
                {
                    _tags[i].enabled = false;                 
                }               
            }
        }
        #endregion
    }
}
