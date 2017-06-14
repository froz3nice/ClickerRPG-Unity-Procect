using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Text;
using UnityEngine.UI;

public class ItemDatabase : MonoBehaviour {
    public GameObject itemExample;

    private List<DatabaseItem> database = new List<DatabaseItem>();
    private JsonData itemData;
	// Use this for initialization
	void Start () {
        
    }

    public void AddItemsFromDataBaseToGameManager()
    {
        for(int i = 0; i < database.Count;i++)
        {
            GameObject itemToAdd = Instantiate(itemExample);
            ItemStatus status = itemToAdd.GetComponent<ItemStatus>();
            ItemStats stats = itemToAdd.GetComponent<ItemStats>();
            status.id = database[i].ID;
            itemToAdd.name = database[i].Title;
            for (int j = 0; j < GameManager.instance.heroes.Length; j++)
                if (GameManager.instance.heroes[i].GetComponent<HeroStateMachine>().playerStats.theName == database[i].OwnedBy)
                {
                    status.ownedByHero = GameManager.instance.heroes[i];
                    break;
                }
            status.Type = database[i].Type;
            status.IsEquipped = database[i].IsEquipped;
            itemToAdd.tag = database[i].Tag;
            stats.coins = database[i].Coins;
            stats.stamina = database[i].Stamina;
            stats.agility = database[i].Agility;
            stats.intellect = database[i].Intellect;
            stats.dexterity = database[i].Dexterity;
            stats.slug = database[i].Slug;
            stats.amountOfMpToGive = database[i].MpToGive;
            stats.amountOfHpToGive = database[i].HpToGive;
            stats.ConsumableDescription = database[i].ConsumableDescription;
            itemToAdd.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Items/" + database[i].Slug);

            GameManager.instance.boughtItems.Add(itemToAdd);
        }
    }
	
	public void ConstrutItemDatabase()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        database.Clear();
        for (int i = 0; i < itemData.Count; i++)
        {
            DatabaseItem item = new DatabaseItem((int)itemData[i]["ID"], itemData[i]["Title"].ToString(), itemData[i]["OwnedBy"].ToString(), itemData[i]["Type"].ToString(), (bool)itemData[i]["IsEquipped"], itemData[i]["Tag"].ToString(), (int)itemData[i]["Coins"], (int)itemData[i]["Stamina"], (int)itemData[i]["Agility"], (int)itemData[i]["Dexterity"], (int)itemData[i]["Intellect"], itemData[i]["Slug"].ToString(), (int)itemData[i]["HpToGive"], (int)itemData[i]["MpToGive"], itemData[i]["ConsumableDescription"].ToString());
            database.Add(item);
        }
    }

    public void getItemsFromGameManager()
    {
        database.Clear();
        List<GameObject> items = GameManager.instance.boughtItems;
        for(int i = 0; i < items.Count; i++)
        {
            ItemStatus status = items[i].GetComponent<ItemStatus>();
            ItemStats stats = items[i].GetComponent<ItemStats>();
            DatabaseItem item = new DatabaseItem(status.id, items[i].name, (status.ownedByHero != null)? status.ownedByHero.GetComponent<HeroStateMachine>().playerStats.theName : "null", status.Type, status.IsEquipped, items[i].tag, stats.coins, stats.stamina, stats.agility, stats.dexterity, stats.intellect, stats.slug, stats.amountOfHpToGive, stats.amountOfMpToGive, stats.ConsumableDescription);
            database.Add(item);
        }
    }

    public void WriteItemsToJsonFile()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[\n");
        for(int i = 0; i < database.Count; i++)
        {
            JsonData item = JsonMapper.ToJson(database[i]);
            if (i != database.Count - 1)
                sb.Append(item.ToString() + ",\n");
            else
                sb.Append(item.ToString() + "\n");
        }
        sb.Append("]");
        File.WriteAllText(Application.dataPath + "/StreamingAssets/Items.json", sb.ToString());
    }
}

public class DatabaseItem
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string OwnedBy { get; set; }
    public string Type { get; set; }
    public bool IsEquipped { get; set; }
    public string Tag { get; set; }
    public int Coins { get; set; }
    public int Stamina { get; set; }
    public int Agility { get; set; }
    public int Dexterity { get; set; }
    public int Intellect { get; set; }
    public string Slug { get; set; }
    public int HpToGive { get; set; }
    public int MpToGive { get; set; }
    public string ConsumableDescription { get; set; }

    public DatabaseItem()
    {
        ID = -1;
    }

    public DatabaseItem(int id, string title, string ownedBy, string type, bool isEquipped, string tag, int coins, int stamina, int agility, int dexterity, int intellect, string slug, int hpToGive, int mpToGive, string consumableDescription)
    {
        ID = id;
        Title = title;
        OwnedBy = ownedBy;
        Type = type;
        IsEquipped = isEquipped;
        Tag = tag;
        Coins = coins;
        Stamina = stamina;
        Agility = agility;
        Intellect = intellect;
        Slug = slug;
        HpToGive = hpToGive;
        MpToGive = mpToGive;
        ConsumableDescription = consumableDescription;
    }

}
