using System.Collections;
using UnityEngine;

namespace Scripts.Runtime.Utils
{
    /// <summary>
    /// this class is responsible for repositioning the UI forward to the player when a scene is loaded.
    /// </summary>
    public class ReposUIForwardPlayer : MonoBehaviour
    {
        /// <summary>
        /// player object
        /// </summary>
        [SerializeField] private GameObject player;

        /// <summary>
        /// flag to check if the UI is in same object as the script
        /// </summary>
        [SerializeField] bool uiIsOnGameObject = true;

        /// <summary>
        /// ui canvas object
        /// </summary>
        [SerializeField] private GameObject uiCanvas;

        /// <summary>
        /// forward distance from player to UI
        /// </summary>
        [SerializeField] private float forwardDistance = 2.25f;

        /// <summary>
        /// speed to reposition the UI
        /// </summary>
        [SerializeField] private float speedDynamicReposition = 2.5f;

        /// <summary>
        /// threshold to reposition the UI
        /// </summary>
        [SerializeField] float smallThreshold = 2.75f;
        [SerializeField] float bigThreshold = 55f;
        
        
        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("MainCamera");
            if(uiIsOnGameObject) uiCanvas = gameObject;
            else if(uiCanvas == null) uiCanvas = GameObject.FindGameObjectWithTag("UI");

            if (uiCanvas == null || player == null)
            {
                Debug.LogWarning("UI Canvas or Player not found. Script will not work.");
                return;
            }
            
            StartCoroutine(RepositionUIForwardToPlayerCoroutine());
        }
        
        
        /// <summary>
        /// Repos the UI forward to the player. fuking crazy stuff happening here. dont touch.
        /// </summary>
        /// <returns> new position for the UI in the 360 sphere </returns>
        private IEnumerator RepositionUIForwardToPlayerCoroutine()
        {
            while (true)
            {
                
                Vector3 targetPosition = player.transform.position + player.transform.forward * forwardDistance;

                Vector3 lastPlayerVector = player.transform.position - uiCanvas.transform.position;
                Vector3 actualPlayerVector = player.transform.position - targetPosition;
                
                float angle = Vector3.Angle(lastPlayerVector, actualPlayerVector);
                
                if (angle > bigThreshold)
                {
                    while (angle >= smallThreshold)
                    {
                        try
                        {
                            uiCanvas.transform.position =
                                Vector3.Lerp(uiCanvas.transform.position, targetPosition, Time.deltaTime * speedDynamicReposition);
                        
                            Vector3 relativePos = player.transform.position - uiCanvas.transform.position;
                            Quaternion rotation = Quaternion.LookRotation(relativePos * -1);
                        
                            uiCanvas.transform.rotation = Quaternion.Lerp(uiCanvas.transform.rotation, rotation,
                                Time.deltaTime * speedDynamicReposition * 2.5f);
                        
                            targetPosition = player.transform.position + player.transform.forward * forwardDistance;
                        
                            lastPlayerVector = player.transform.position - uiCanvas.transform.position;
                            actualPlayerVector = player.transform.position - targetPosition;
                        
                            angle = Vector3.Angle(lastPlayerVector, actualPlayerVector);
                        }
                        catch (MissingReferenceException)
                        {
                            Debug.LogWarning("Some object is missing. Closing coroutine.");
                            yield break;
                        }
                        
                        yield return null;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
            
        }
    }
}


