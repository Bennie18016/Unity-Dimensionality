using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

namespace Abilities
{
    public class TwoShadowForm : MonoBehaviour
    {

        MeshRenderer mesh;
        public bool shadow;
        float lastShadow;
        float shadowCool;
        PhotonView PV;
        // Start is called before the first frame update
        void Start()
        {
            mesh = GetComponentInChildren<MeshRenderer>();
            shadowCool = 30;
            lastShadow = 30;
            PV = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
            if (PV.IsMine)
            {
                if (!shadow && lastShadow > shadowCool)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Shadow();
                        PV.RPC("Shadow", RpcTarget.AllBuffered);
                    }
                }
                lastShadow += 1 * Time.deltaTime;
            }

        }

        [PunRPC]
        private void Shadow()
        {
            shadow = true;
            mesh.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            StartCoroutine(ShadowCool());
        }

        IEnumerator ShadowCool()
        {
            yield return new WaitForSeconds(10);
            mesh.shadowCastingMode = ShadowCastingMode.On;
            shadow = false;
            lastShadow = 0;
        }
    }
}
