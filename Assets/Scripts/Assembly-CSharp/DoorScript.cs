using UnityEngine;
using kube;

public class DoorScript : MonoBehaviour
{
    private ItemPropsScript IPS;

    private NetworkObjectScript NO;

    public GameObject door;

    public int doorStrength = 10;

    public GameObject soundOpen;

    public GameObject soundClose;

    private bool initialized;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!initialized)
        {
            IPS = base.transform.root.gameObject.GetComponent<ItemPropsScript>();
            if (NO == null)
            {
                NO = Kube.BCS.NO;
            }
            initialized = true;
        }
    }

    private void Update()
    {
    }

    private void Activate(PlayerScript ps)
    {
        Init();
        if (IPS.state == 0)
        {
            if (base.transform.InverseTransformDirection(ps.transform.position - base.transform.position).x > 0f)
            {
                NO.ChangeItemState(IPS.id, 1);
            }
            else
            {
                NO.ChangeItemState(IPS.id, 2);
            }
        }
        else
        {
            NO.ChangeItemState(IPS.id, 0);
        }
    }

    private void ChangeItemState(int newState)
    {
        Init();
        IPS.state = newState;
        if (!(door != null))
        {
            return;
        }
        switch (newState)
        {
            case 0:
                {
                    door.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    for (int i = 0; i < (int)IPS.doorSize.x; i++)
                    {
                        for (int j = 0; j < (int)IPS.doorSize.y; j++)
                        {
                            Vector3 point = new Vector3(0f, j, i);
                            point = base.transform.rotation * point;
                            Kube.WHS.cubes[Mathf.RoundToInt(base.transform.position.x + point.x), Mathf.RoundToInt(base.transform.position.y + point.y), Mathf.RoundToInt(base.transform.position.z + point.z)].prop = CubeProps.closedDoor;
                        }
                    }
                    if (Time.timeSinceLevelLoad > 5f)
                    {
                        Object.Instantiate(soundClose, base.transform.position, Quaternion.identity);
                    }
                    break;
                }
            case 1:
                door.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                ClearWorldProps();
                if (Time.timeSinceLevelLoad > 5f)
                {
                    Object.Instantiate(soundOpen, base.transform.position, Quaternion.identity);
                }
                break;
            case 2:
                door.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                ClearWorldProps();
                if (Time.timeSinceLevelLoad > 5f)
                {
                    Object.Instantiate(soundOpen, base.transform.position, Quaternion.identity);
                }
                break;
        }
    }

    private void ClearWorldProps()
    {
        for (int i = 0; i < (int)IPS.doorSize.x; i++)
        {
            for (int j = 0; j < (int)IPS.doorSize.y; j++)
            {
                Vector3 point = new Vector3(0f, j, i);
                point = base.transform.rotation * point;
                Kube.WHS.cubes[Mathf.RoundToInt(base.transform.position.x + point.x), Mathf.RoundToInt(base.transform.position.y + point.y), Mathf.RoundToInt(base.transform.position.z + point.z)].prop =CubeProps.no;
            }
        }
    }

    private void MonsterHit(Vector3 monsterPos)
    {
        int num = Random.Range(0, doorStrength);
        if (num == 1 && IPS.state == 0)
        {
            if (base.transform.InverseTransformDirection(monsterPos - base.transform.position).x > 0f)
            {
                NO.ChangeItemState(IPS.id, 1);
            }
            else
            {
                NO.ChangeItemState(IPS.id, 2);
            }
        }
        else if (num == 1 && IPS.state != 0)
        {
            NO.ChangeItemState(IPS.id, 0);
        }
    }

    private void OnDestroy()
    {
        if ((bool)Kube.WHS)
        {
            ClearWorldProps();
        }
    }
}
