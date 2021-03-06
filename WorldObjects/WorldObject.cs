﻿using UnityEngine;
using System.Collections;
using RTS;

public class WorldObject : MonoBehaviour 
{
	
	public string objectName;
	public int cost, sellValueM, sellValueR, sellValueW, hitPoints, maxHitPoints;
	public bool isRallyPoint=false;
	public Texture2D buildImage;
	protected Player player;
	protected string[] actions = {};
	protected bool currentlySelected = false;
	protected Bounds selectionBounds;
	protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
	
	protected virtual void Awake()
	{
		selectionBounds = ResourceManager.InvalidBounds;
		CalculateBounds();
	}
	
	protected virtual void Start () 
	{
		player = transform.root.GetComponentInChildren<Player>();
	}
	
	protected virtual void Update () 
	{
		
	}
	
	protected virtual void OnGUI()
	{
		if(currentlySelected)
			DrawSelection();
	}
	
	public virtual void SetSelection(bool selected, Rect playingArea)
	{
		currentlySelected = selected;
		if(selected) 
			this.playingArea = playingArea;
	}
	
	public string[] GetActions() 
	{
    	return actions;
	}
 
	public virtual void PerformAction(string actionToPerform)
	{
		
	}
	
	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		if(currentlySelected && hitObject && hitObject.name != "Ground") 
		{
        	WorldObject worldObject = hitObject.transform.parent.GetComponent<WorldObject>();
        	//clicked on another selectable object
        	if(worldObject) 
				ChangeSelection(worldObject, controller);
    	}
	}
	
	public void CalculateBounds() 
	{
    	selectionBounds = new Bounds(transform.position, Vector3.zero);
    	foreach(Renderer r in GetComponentsInChildren<Renderer>()) 
		{
	        selectionBounds.Encapsulate(r.bounds);
	    }
	}
	
	private void ChangeSelection(WorldObject worldObject, Player controller) 
	{
	    //this should be called by the following line, but there is an outside chance it will not
	    SetSelection(false, playingArea);
	    if(controller.SelectedObject) 
			controller.SelectedObject.SetSelection(false, playingArea);
	    controller.SelectedObject = worldObject;
	    worldObject.SetSelection(true, controller.hud.GetPlayingArea());
	}
	
	private void DrawSelection()
	{
		GUI.skin = ResourceManager.SelectBoxSkin;
	    Rect selectBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
	    //Draw the selection box around the currently selected object, within the bounds of the playing area
	    GUI.BeginGroup(playingArea);
	    DrawSelectionBox(selectBox);
	    GUI.EndGroup();
	}
	
	protected virtual void DrawSelectionBox(Rect selectBox) 
	{
    	GUI.Box(selectBox, "");
	}
	
	public virtual void SetHoverState(GameObject hoverObject)
	{
		if(player && player.human && currentlySelected)
			if(hoverObject.name!="Ground")
				player.hud.SetCursorState(CursorState.Select);
	}
	
	public bool IsOwnedBy(Player owner) 
	{
	    if(player && player.Equals(owner)) 
		{
	        return true;
	    } 
		else 
		{
	        return false;
	    }
	}

}
