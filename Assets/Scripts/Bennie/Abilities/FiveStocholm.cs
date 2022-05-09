using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

namespace Abilities{
    public class FiveStocholm : MonoBehaviour
{
    Camera myCam;
    // Start is called before the first frame update
    void Start()
    {
        myCam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StockholmSyndrome();
        }
    }

    private void StockholmSyndrome()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(GetMouseRay(), out hit, Mathf.Infinity))
        {
            if(hit.collider.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<EnemyPattern>().Stockholm();
            }
        }

    }
    private Ray GetMouseRay()
    {
        return myCam.ScreenPointToRay(Input.mousePosition);
    }
}
}
