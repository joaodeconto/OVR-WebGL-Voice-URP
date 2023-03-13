using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace Metaversando.WorkSpace
{
    public class InstantiateAvatar : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        //public PlayerDataScriptableObject _playerData;
        public bool gambiLogin = false;
        
        #endregion

        #region Private Serialize Fields

        [SerializeField] private ulong _userId;
        [SerializeField] private string _userName;
        [SerializeField] private Transform _parentRef;
        [SerializeField] private GameObject _avatarPrefab;
        #endregion

        #region Mono Callbacks

        public void SendAvatarNetwork()
        {
            StartCoroutine(InstantiateWhenUserIdIsFound());          
        }
        #endregion

        #region Photon CallBacks/Methods
       
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            object[] instantiationData = info.photonView.InstantiationData;

        }

        #endregion //Photon CallBacks/Methods

        #region Instantiate Avatar

        void InstantiateNetworkedAvatar()
        {
            Int64 userId = Convert.ToInt64(_userId);
            object[] objects = new object[5];
            objects[0] = userId;
            objects[1] =  "AvatarUrl";
            objects[2] = _userName;
            GameObject _myAvatar = PhotonNetwork.Instantiate(_avatarPrefab.name, _parentRef.position, _parentRef.rotation, 0, objects);
            Transform _ = _myAvatar.transform;
            _.parent = _parentRef;
            _.localPosition = Vector3.zero;
            _.localRotation = Quaternion.Euler(0,0,0);
            _myAvatar.SetActive(true);
            Debug.Log("Called Instantiate AVATAR");
        }

        IEnumerator InstantiateWhenUserIdIsFound()
        {            
            Debug.LogError("ID = " + _userId + " Name = " + _userName);
            InstantiateNetworkedAvatar();
            return null;
        }
        #endregion //Instantiate Avatar

        #region Global Methods
        public void Quit()
        {
            UnityEngine.Application.Quit();
        }
        #endregion
    }
}