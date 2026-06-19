using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using kube;
public class RatingMenu : MonoBehaviour
{
    public ServerPlayers[] items;
    public struct ServerPlayers{
        public string playerName;
        public int kills;
        public int points;
        public int level;
        public int deads;
        public int exp;
        public int id;
        public int skin;
        public string clothes;
        public string accountDate;
    }

    public CareerPlayerInfo playerInfoMenu;
    public UIScrollView container;
    public GameObject itemPrefab;
    public static RatingMenu I;
    private void Awake() => I = this;
    public void OnEnable(){
        Kube.SS.Request(809,new Dictionary<string,string>(),new ServerCallback(OnTopDoneLoaded));
    }
    public void OnTopDoneLoaded(string answer){
         JsonData json = JsonMapper.ToObject(answer);
         List<ServerPlayers> list = new List<ServerPlayers>();
         for (int i = 0; i < json.Count; i++){
            if (int.Parse(json[i]["kills"].ToString()) >= 5 && int.Parse(json[i]["playerOldExp"].ToString()) > 0 && json[i]["ban"].ToString() != "true")
            {
                ServerPlayers item = new ServerPlayers();
                item.playerName = AuxFunc.DecodeRussianName(json[i]["userName"].ToString());
                item.kills = int.Parse(json[i]["kills"].ToString());
                item.points = int.Parse(json[i]["points"].ToString());
                item.exp = int.Parse(json[i]["playerOldExp"].ToString());
                item.level = int.Parse(json[i]["level"].ToString());
                item.clothes = json[i]["playerClothes"].ToString();
                item.skin = int.Parse(json[i]["playerSkin"].ToString());
                item.deads = int.Parse(json[i]["deads"].ToString());
                item.accountDate = json[i]["dataAccount"].ToString();
                item.id = int.Parse(json[i]["id"].ToString());
                list.Add(item);
            }
         }
         items = list.ToArray();
         Invalidate();
    }

    private void Invalidate()
    {
        KGUITools.removeAllChildren(this.container.gameObject, true);
        Array.Sort(items, new Comparison<ServerPlayers>(sortingOrKills));
        int num = Mathf.Min(500, this.items.Length);
        for (int i = 0; i < num; i++)
        {
            if (items[i].kills >= 5 && items[i].exp > 0){
           GameObject gameObject = container.gameObject.AddChild(itemPrefab);
			RatingItem component = gameObject.GetComponent<RatingItem>();
			component.nnName.text = (i + 1).ToString() + ". " + items[i].playerName;
            component.kills.text = items[i].kills.ToString();
			component.exp.text = items[i].exp.ToString();
            component.points.text = items[i].points.ToString();
            string levName = string.Empty;
            if (items[i].level >= Localize.RankName.Length)
            {
                items[i].level = Localize.RankName.Length - 1;
                levName = Localize.RankName[items[i].level];
            }else{
                levName = Localize.RankName[items[i].level];
            }
            object[] meanData =new object[]
            {
               items[i].playerName,
               items[i].accountDate,
               items[i].id,
               items[i].level,
               levName,
               items[i].kills,
               items[i].points,
               items[i].deads,
               items[i].clothes,
               items[i].exp,
               items[i].skin
            };
             component.data = meanData;
            }
        }
        container.ResetPosition();
        container.GetComponent<UIGrid>().Reposition();
    }
     private static int sortingOrKills(ServerPlayers left, ServerPlayers right)
	{
		return right.kills.CompareTo(left.kills);
	}
    public void RemoveAllList(){
         foreach(RatingItem itemsObj in container.GetComponentsInChildren<RatingItem>()){
            Destroy(itemsObj.gameObject);
        }
        container.ResetPosition();
        items = null;
    }
    public void OpenMenu(){
        gameObject.SetActive(true);
    }
    public void CloseMenu(){
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
