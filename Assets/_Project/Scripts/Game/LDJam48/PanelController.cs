using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util.Var.Events;

namespace LDJam48
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private StringEventReference activateRequest;
        [SerializeField] private StringEventReference deactivateRequest;

        [SerializeField] private UnityEvent onActivate;
        [SerializeField] private UnityEvent onDeactivate;

        private HashSet<string> _ongoingRequests = new HashSet<string>(); 

        private void OnEnable()
        {
            _ongoingRequests.Clear();

            activateRequest.OnEventTriggered += OnActivateRequest;
            deactivateRequest.OnEventTriggered += OnDeactivateRequest;
        }

        private void OnDisable()
        {
            activateRequest.OnEventTriggered -= OnActivateRequest;
            deactivateRequest.OnEventTriggered -= OnDeactivateRequest;
        }

        private void OnActivateRequest(string requestor)
        {
            bool wasEmpty = _ongoingRequests.Count == 0;
            _ongoingRequests.Add(requestor);
            
            // if this is the first to ask for it to be activated then trigger the callback
            if (wasEmpty)
            {
                onActivate?.Invoke();
            }
        }
        
        private void OnDeactivateRequest(string requestor)
        {
            _ongoingRequests.Remove(requestor);
            
            // only trigger callback if everyone who requested activation have requested deactivation
            if (_ongoingRequests.Count == 0)
            {
                onDeactivate?.Invoke();
            }
        }
    }
}