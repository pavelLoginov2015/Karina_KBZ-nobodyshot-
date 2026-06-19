using LitJson;
using UnityEngine;
using kube.data;

public interface IPlatform
{
	platformType platform { get; }

	string playerUID { get; }
	string secretKey{get;}

	Texture moneyIconTx { get; }

	bool hasMoneyIcon { get; }

	string moneyName { get; }

	float moneyValue { get; }

	string locale { get; }
	string current_version {get;}
	string updateUrlGame {get; set;}
	QuestViralScript questViral {get;set;}

	void Init(GameObject go, string func);



	
}
