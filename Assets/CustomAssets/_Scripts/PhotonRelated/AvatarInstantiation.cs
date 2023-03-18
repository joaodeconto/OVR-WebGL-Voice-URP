using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

namespace BWV.Player
{
    public class AvatarInstantiation : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public Transform SpawnPosition;
        public float PositionOffset = 2.0f;
        public GameObject[] PrefabsToInstantiate;
        public List<Transform> SpawnPoints;

        public bool AutoSpawn = true;
        public bool UseRandomOffset = true;

        public delegate void OnCharacterInstantiated(GameObject character);
        public static event OnCharacterInstantiated CharacterInstantiated;

        [SerializeField]
        private byte manualInstantiationEventCode = 1;

        protected int lastUsedSpawnPointIndex = -1;

#pragma warning disable 649
        [SerializeField]
        private bool manualInstantiation;

        [SerializeField]
        private bool differentPrefabs;

        [SerializeField] private string localPrefabSuffix;
        [SerializeField] private string remotePrefabSuffix;
#pragma warning restore 649

        public override void OnJoinedRoom()
        {
            if (!this.AutoSpawn)
            {
                return;
            }
            if (this.PrefabsToInstantiate != null)
            {
                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                if (actorNumber < 1)
                {
                    actorNumber = 1;
                }
                int index = (actorNumber - 1) % this.PrefabsToInstantiate.Length;
                Vector3 spawnPos = new(SpawnPosition.position.x + (actorNumber * 1), SpawnPosition.position.y, SpawnPosition.position.z) ;
                Quaternion spawnRotation = SpawnPosition.rotation;

                if (this.manualInstantiation)
                {
                    this.ManualInstantiation(index, spawnPos, spawnRotation);
                }
                else
                {
                    GameObject o = this.PrefabsToInstantiate[index];
                    o = PhotonNetwork.Instantiate(o.name, spawnPos, spawnRotation);
                    if (CharacterInstantiated != null)
                    {
                        CharacterInstantiated(o);
                    }
                }
            }
        }

        private void ManualInstantiation(int index, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = this.PrefabsToInstantiate[index];
            GameObject player;
            if (this.differentPrefabs)
            {
                player = Instantiate(Resources.Load(string.Format("{0}{1}", prefab.name, this.localPrefabSuffix)) as GameObject, position, rotation);
            }
            else
            {
                player = Instantiate(prefab, position, rotation);
            }
            player.name += this.localPrefabSuffix;
            GameManager.Instance.SetMyPlayer(player);

            PhotonView photonView = player.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                GameManager.AvatarUrlSO.CurrentUrl = GameManager.AvatarUrlSO.GetAvatarUrl(Random.Range(0, 5));
                object[] data =
                {
                    index, player.transform.position, player.transform.rotation, photonView.ViewID, GameManager.AvatarUrlSO.CurrentUrl
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                PhotonNetwork.RaiseEvent(this.manualInstantiationEventCode, data, raiseEventOptions, SendOptions.SendReliable);
                if (CharacterInstantiated != null)
                {
                    CharacterInstantiated(player);
                }
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId.");

                Destroy(player);
            }
        }             

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == this.manualInstantiationEventCode)
            {
                object[] data = photonEvent.CustomData as object[];
                int prefabIndex = (int)data[0];
                GameObject prefab = this.PrefabsToInstantiate[prefabIndex];
                Vector3 position = (Vector3)data[1];
                Quaternion rotation = (Quaternion)data[2];
                GameObject player;
                if (this.differentPrefabs)
                {
                    player = Instantiate(Resources.Load(string.Format("{0}{1}", prefab.name, this.remotePrefabSuffix)) as GameObject, position, rotation);
                }
                else
                {
                    player = Instantiate(prefab, position, Quaternion.identity);
                }
                
                PhotonView photonView = player.GetComponent<PhotonView>();
                photonView.ViewID = (int)data[3];
                player.name += this.remotePrefabSuffix + photonView.ViewID;
                WebAvatarLoader wb = player.GetComponent<WebAvatarLoader>();
                AvatarSettingsSO so = ScriptableObject.CreateInstance<AvatarSettingsSO>();
                so.avatarUrl = (string)data[4];                
                wb.avatarSettings = so;
                
            }
        }       
    }
}