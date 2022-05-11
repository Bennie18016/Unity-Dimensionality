using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerController;
using Photon.Pun;

public class Build : MonoBehaviour
{
    GameObject buildText;
    GameObject woodText;
    GameObject ropeText;
    GameObject rockText;
    float reach;

    public int[] wood;
    int woodStage;

    public int[] rope;
    int ropeStage;

    public int[] rock;
    int rockStage;
    bool done;
    public Material finishMaterial;

    public bool tutorial;
    public bool boss;

    void Start()
    {
        buildText = createText("BuildText", "Click F to build", new Vector3(0, -75, 0));
        buildText.SetActive(false);
        woodText = createText("WoodText", null, new Vector3(0, -90, 0));
        woodText.SetActive(false);
        ropeText = createText("RopeText", null, new Vector3(0, -105, 0));
        ropeText.SetActive(false);
        rockText = createText("RockText", null, new Vector3(0, -120, 0));
        rockText.SetActive(false);

        reach = 5f;
    }

    private GameObject createText(string name, string speech, Vector3 position)
    {
        GameObject gameOBJ = new GameObject();
        gameOBJ.name = name + " text";
        gameOBJ.AddComponent<Text>();
        gameOBJ.GetComponent<Text>().text = speech;
        gameOBJ.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        GameObject c = GameObject.Find("Canvas");
        gameOBJ.transform.SetParent(c.transform);
        RectTransform transform = gameOBJ.GetComponent<RectTransform>();
        transform.anchoredPosition = position;
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        return gameOBJ;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfDone();
        ChangeName();
        CheckDistance();
        CheckIfBuilt();
    }

    private void CheckIfBuilt()
    {
        if (wood.Length == woodStage && rope.Length == ropeStage && rock.Length == rockStage)
        {
            gameObject.GetComponent<PhotonView>().RPC("Done", RpcTarget.AllBuffered);
        }
    }

    private void CheckDistance()
    {
        if (DistanceFromBuild(ClosestPlayer()) < reach && !done && ClosestPlayer().GetComponent<PhotonView>().IsMine)
        {
            buildText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (woodText != null)
                {
                    CheckMaterial("wood");
                }
                if (ropeText != null)
                {
                    CheckMaterial("rope");
                }
                if (rockText != null)
                {
                    CheckMaterial("rock");
                }
            }
        }
        else
        {
            buildText.SetActive(false);
            if (woodText != null) { woodText.SetActive(false); }
            if (ropeText != null) { ropeText.SetActive(false); }
            if (rockText != null) { rockText.SetActive(false); }
        }
    }

    private void ChangeName()
    {
        if (woodStage < wood.Length && woodText != null)
        {
            woodText.GetComponent<Text>().text = wood[woodStage] + " wood";
        }
        else if (woodStage == wood.Length && woodText != null)
        {
            Destroy(woodText);
        }

        if (ropeStage < rope.Length && ropeText != null)
        {
            ropeText.GetComponent<Text>().text = rope[ropeStage] + " rope";
        }
        else if (ropeStage == rope.Length && ropeText != null)
        {
            Destroy(ropeText);
        }

        if (rockStage < rock.Length && rockText != null)
        {
            rockText.GetComponent<Text>().text = rock[rockStage] + " rock";
        }
        else if (rockStage == rock.Length && rockText != null)
        {
            Destroy(rockText);
        }
    }

    private void CheckIfDone()
    {
        if (woodStage < wood.Length && woodText != null) { woodText.SetActive(true); }
        if (ropeStage < rope.Length && ropeText != null) { ropeText.SetActive(true); }
        if (rockStage < rock.Length && rockText != null) { rockText.SetActive(true); }
    }

    private void CheckMaterial(string material)
    {
        PhotonView player = ClosestPlayer().GetComponent<PhotonView>();
        if (material.ToLower() == "wood")
        {
            if (wood.Length >= 1 && woodStage < wood.Length)
            {
                GetComponent<PhotonView>().RPC("BuildStageWood", RpcTarget.All);
            }
        }
        if (material.ToLower() == "rope")
        {
            if (rope.Length >= 1 && ropeStage < rope.Length)
            {
                GetComponent<PhotonView>().RPC("BuildStageRope", RpcTarget.All);
            }
        }
        if (material.ToLower() == "rock")
        {
            if (rock.Length >= 1 && rockStage < rock.Length)
            {
                GetComponent<PhotonView>().RPC("BuildStageRock", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void BuildStageWood()
    {
        PhotonView player = ClosestPlayer().GetComponent<PhotonView>();
        Inventory i = player.gameObject.GetComponent<Inventory>();
        if (i.wood >= wood[woodStage] && player.IsMine)
        {
            i.wood -= wood[woodStage];
            woodStage++;
        }
    }

    [PunRPC]
    private void BuildStageRope()
    {
        PhotonView player = ClosestPlayer().GetComponent<PhotonView>();
        Inventory i = player.gameObject.GetComponent<Inventory>();
        if (i.rope >= rope[ropeStage] && player.IsMine)
        {
            i.rope -= rope[ropeStage];
            ropeStage++;
        }
    }

    [PunRPC]
    private void BuildStageRock()
    {
        PhotonView player = ClosestPlayer().GetComponent<PhotonView>();
        Inventory i = player.gameObject.GetComponent<Inventory>();
        if (i.rock >= rock[rockStage] && player.IsMine)
        {
            i.rock -= rock[rockStage];
            rockStage++;
        }
    }

    [PunRPC]
    private void Done()
    {
        if (!done)
        {
            if (woodText != null) { woodText.SetActive(false); }
            if (rockText != null) { rockText.SetActive(false); }
            if (ropeText != null) { ropeText.SetActive(false); }
            buildText.SetActive(false);
            gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshRenderer>().material = finishMaterial;
            gameObject.AddComponent<BoxCollider>().isTrigger = enabled;
            if (tutorial) { gameObject.AddComponent<TutorialSail>(); }
            if (boss) { gameObject.AddComponent<BossSail>(); }
            done = true;
            StartCoroutine(delete());
        }

    }

    IEnumerator delete()
    {
        yield return new WaitForSeconds(3);
        Destroy(this);
    }

    private GameObject ClosestPlayer()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 enemyPos = transform.position;

        foreach (GameObject target in targets)
        {
            Vector3 diff = target.transform.position - enemyPos;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = target;
                distance = curDistance;
            }
        }
        return closest;
    }


    private float DistanceFromBuild(GameObject player)
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

        private float DistanceFromBuild(GameObject[] players)
    {
        foreach(GameObject player in players){
            return Vector3.Distance(player.transform.position, transform.position);
        }
        return 0;
    }
}
