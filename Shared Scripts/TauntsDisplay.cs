using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Utilities;
using TMPro;

namespace Project.SharedScripts
{
    public class TauntsDisplay : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private ObjectPoolSO _tauntText;
        [SerializeField] private TauntsSO[] _taunts;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _displayTime;

        private List<Transform> _activeSenders = new List<Transform>();

        [SerializeField] private Canvas _canvas;
        private RectTransform _canvasRect;
        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            _canvasRect = _canvas.GetComponent<RectTransform>();
            foreach (var item in _taunts)
            {
                item.TauntsDisplay = this;
            }
        }
        public void EvokeTaunt(TauntsSO tauntsSO, Transform sender)
        {
            if(_activeSenders.Contains(sender) == false && tauntsSO.CanEvoke())
            {
                _activeSenders.Add(sender);
                _tauntText.Get(out ObjectPoolReference objectPoolReference);
                RectTransform tauntTransform = objectPoolReference.GetComponent<RectTransform>();
                tauntTransform.SetParent(_canvasRect);
                TMP_Text tauntText = objectPoolReference.GetComponent<TMP_Text>();

                int random = Random.Range(0, tauntsSO.Taunts.Length);
                tauntText.text = tauntsSO.Taunts[random];
                StartCoroutine(TauntDisplay(sender,tauntText.rectTransform));
            }
        }
        IEnumerator TauntDisplay(Transform sender, RectTransform tauntTransform)
        {
            float elapsed = 0f;

            while (elapsed < _displayTime)
            {             
                elapsed += Time.deltaTime;
                tauntTransform.anchoredPosition = UIFunctions.WorldPositionToCanvas(sender.transform.position + _offset, _canvasRect, 1.15f);
                yield return null;
            }

            _activeSenders.Remove(sender);
            tauntTransform.gameObject.SetActive(false);
        }

        #endregion
    }
}
