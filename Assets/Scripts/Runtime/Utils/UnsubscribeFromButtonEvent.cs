using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Runtime.Utils
{
    /// <summary>
    /// this class is responsible for unsubscribing from the button event.
    /// </summary>
    public class UnsubscribeFromButtonEvent : MonoBehaviour
    {
        /// <summary>
        /// boolean to check if the button is in the gameobject.
        /// </summary>
        [SerializeField] bool buttonInTheSameObject = true;
        
        /// <summary>
        /// button to unsubscribe from.
        /// </summary>
        [SerializeField] private Button button;

        private void Start()
        {
            if (buttonInTheSameObject)
            {
                button = GetComponent<Button>();
            }
            
            button.onClick.AddListener(Unsubscribe);
        }

        /// <summary>
        /// unsubscribe from the button event.
        /// </summary>
        private void Unsubscribe()
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }
    }
}
