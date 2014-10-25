using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Player : MonoBehaviour 
{
	public string username;
	public bool human;
	public HUD hud;
	public WorldObject SelectedObject {get; set;}
	public int startMoney, startWood, startRocks, startIron, startPeople, startLove,
	startMoneyLimit, startWoodLimit, startRocksLimit, startIronLimit, startPeopleLimit, startLoveLimit;
	private Dictionary <ResourceType, int> resources, resourceLimits;
	
	// Use this for initialization
	void Start () 
	{
		hud = GetComponentInChildren<HUD>();
		AddStartResourceLimits();
		AddStartResources();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(human)
			hud.SetResourceValues(resources, resourceLimits);
	}
	
	void Awake()
	{
		resources = InitResourceList();
		resourceLimits = InitResourceList();
	}
	
	private Dictionary <ResourceType, int> InitResourceList()
	{
		Dictionary <ResourceType, int> list = new Dictionary <ResourceType, int>();
	    list.Add(ResourceType.Money, 0);
	    list.Add(ResourceType.Wood, 0);
		list.Add(ResourceType.Rocks, 0);
	    list.Add(ResourceType.Iron, 0);
		list.Add(ResourceType.People, 0);
	    list.Add(ResourceType.Love, 0);
	    return list;
	}
	
	private void AddStartResourceLimits() 
	{
	    IncrementResourceLimit(ResourceType.Money, startMoneyLimit);
	    IncrementResourceLimit(ResourceType.Wood, startWoodLimit);
		IncrementResourceLimit(ResourceType.Rocks, startRocksLimit);
	    IncrementResourceLimit(ResourceType.Iron, startIronLimit);
		IncrementResourceLimit(ResourceType.People, startPeopleLimit);
	    IncrementResourceLimit(ResourceType.Love, startLoveLimit);
	}
 
	private void AddStartResources() 
	{
	    AddResource(ResourceType.Money, startMoney);
	    AddResource(ResourceType.Wood, startWood);
		AddResource(ResourceType.Rocks, startRocks);
	    AddResource(ResourceType.Iron, startIron);
		AddResource(ResourceType.People, startPeople);
	    AddResource(ResourceType.Love, startLove);
	}
	
	public void AddResource(ResourceType type, int amount) 
	{
    	resources[type] += amount;
	}
 
	public void IncrementResourceLimit(ResourceType type, int amount) 
	{
	    resourceLimits[type] += amount;
	}
	
	public void AddUnit(string unitName, Vector3 spawnPoint, Vector3 rallyPoint, Quaternion rotation) 
	{
    	Units units = GetComponentInChildren< Units >();
	    GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName),spawnPoint, rotation);
	    newUnit.transform.parent = units.transform;
	    Unit unitObject = newUnit.GetComponent< Unit >();
	    if(unitObject && spawnPoint != rallyPoint) 
			unitObject.StartMove(rallyPoint);
	}
}
