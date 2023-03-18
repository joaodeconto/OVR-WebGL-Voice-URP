namespace BWV
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class MessagePanel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text messagePrefab;
        [SerializeField] private Transform messageParent;
        [SerializeField] private int poolSize = 10;
        [SerializeField] private float messageDuration = 5f;

        private Queue<TMP_Text> messagePool = new Queue<TMP_Text>();
        private List<TMP_Text> messageLog = new List<TMP_Text>();

        #endregion

        #region Unity Methods

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                TMP_Text newMessage = Instantiate(messagePrefab, messageParent);
                newMessage.gameObject.SetActive(false);
                messagePool.Enqueue(newMessage);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a message to the panel and logs it.
        /// </summary>
        public void AddMessage(string message)
        {
            StartCoroutine(ShowMessage(message));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Displays a message on the panel for a set amount of time before removing it.
        /// </summary>
        private IEnumerator ShowMessage(string message)
        {
            TMP_Text newMessage = GetMessageFromPool();
            newMessage.text = message;
            newMessage.gameObject.SetActive(true);
            messageLog.Add(newMessage);

            yield return new WaitForSeconds(messageDuration);

            messageLog.Remove(newMessage);
            newMessage.gameObject.SetActive(false);
            messagePool.Enqueue(newMessage);
        }

        /// <summary>
        /// Gets a message from the pool or creates a new one if the pool is empty.
        /// </summary>
        private TMP_Text GetMessageFromPool()
        {
            if (messagePool.Count > 0)
            {
                TMP_Text message = messagePool.Dequeue();
                return message;
            }
            else
            {
                TMP_Text newMessage = Instantiate(messagePrefab, messageParent);
                newMessage.gameObject.SetActive(false);
                return newMessage;
            }
        }

        #endregion
    }
}