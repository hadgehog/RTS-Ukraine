using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour 
{
	
	private Player player;
	
	void Start () 
	{
		player=transform.root.GetComponent<Player>();
	}
	
	void Update () 
	{
		if(player.human)
		{
			MoveCamera();
			RotateCamera();
			MouseActivity();
		}
	}
	
	private void MoveCamera()
	{
		float x_pos=Input.mousePosition.x;
		float y_pos=Input.mousePosition.y;
		Vector3 movement = new Vector3(0,0,0);
		bool mouseScroll = false;
		
		//horisontal camera move
		if(x_pos>=0&&x_pos<ResourceManager.ScrollWidth)
		{
			movement.x -= ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanLeft);
    		mouseScroll = true;
		}
		else
			if(x_pos <= Screen.width && x_pos > Screen.width - ResourceManager.ScrollWidth)
		{
			movement.x += ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanRight);
    		mouseScroll = true;
		}
		
		//vertical camera move
		if(y_pos>= 0 && y_pos < ResourceManager.ScrollWidth)
		{
			movement.z -= ResourceManager.ScrollSpeed;
			 player.hud.SetCursorState(CursorState.PanDown);
    		mouseScroll = true;
		}
		else
			if(y_pos <= Screen.height && y_pos > Screen.height - ResourceManager.ScrollWidth)
		{
			movement.z += ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanUp);
    		mouseScroll = true;
		}
		
		movement = Camera.mainCamera.transform.TransformDirection(movement);
		movement.y = 0;
		
		//up down
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
		
		Vector3 current = Camera.main.camera.transform.position;
		Vector3 destination = current;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;
		
		if(!mouseScroll) 
		{
    		player.hud.SetCursorState(CursorState.Select);
		}
		
		//check up down
		if(destination.y>ResourceManager.MaxCameraHeight)
		{
			destination.y = ResourceManager.MaxCameraHeight;
		}
		
		if(destination.y<ResourceManager.MinCameraHeight)
		{
			destination.y = ResourceManager.MinCameraHeight;
		}
		
		//if position changed move camera
		if(destination != current)
		{
			Camera.main.camera.transform.position = Vector3.MoveTowards(current, destination, Time.deltaTime* ResourceManager.ScrollSpeed);
		}
		
	}
	
	private void RotateCamera()
	{
		Vector3 current = Camera.main.camera.transform.eulerAngles;
		Vector3 destination = current;
		
		if(Input.GetKey(KeyCode.Q)) 
	    	destination.y -= ResourceManager.RotateAmount;
		if(Input.GetKey(KeyCode.E))
	    	destination.y += ResourceManager.RotateAmount;
		
 
		//if a change in position is detected perform the necessary update
		if(destination != current) 
		{
	    	Camera.main.camera.transform.eulerAngles = Vector3.MoveTowards(current, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		
		}
	}
	
	private void MouseActivity()
	{
		if(Input.GetMouseButtonDown(0))
			LeftMouseClick();
		else
			if(Input.GetMouseButtonDown(1))
				RightMouseClick();
		
		MouseHover();
	}
	
	private void LeftMouseClick()
	{
		if(player.hud.MouseInBounds()) 
		{
        GameObject hitObject = FindHitObject();
        Vector3 hitPoint = FindHitPoint();
        if(hitObject && hitPoint != ResourceManager.InvalidPosition) 
			{
            if(player.SelectedObject) 
				player.SelectedObject.MouseClick(hitObject, hitPoint, player);
            else 
				if(hitObject.name!="Ground") 
				{
                WorldObject worldObject = hitObject.transform.parent.GetComponent<WorldObject>();
	                if(worldObject) 
					{
	                    //we already know the player has no selected object
	                    player.SelectedObject = worldObject;
	                    worldObject.SetSelection(true, player.hud.GetPlayingArea());
	                }
            	}
        	}
    	}
	}
	
	private void RightMouseClick() 
	{
    if(player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject) 
		{
	        player.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
	        player.SelectedObject = null;
    	}
	}
	
	private GameObject FindHitObject() 
	{
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit hit;
	    if(Physics.Raycast(ray, out hit)) 
				return hit.collider.gameObject;
	    return null;
	}
	
	private Vector3 FindHitPoint() 
	{
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit hit;
	    if(Physics.Raycast(ray, out hit)) 
			return hit.point;
	    return ResourceManager.InvalidPosition;
	}
	
	private void MouseHover()
	{
		if(player.hud.MouseInBounds())
		{
			GameObject hoverObject = FindHitObject();
			if(hoverObject)
			{
				if(player.SelectedObject)
				{
					player.SelectedObject.SetHoverState(hoverObject);
				}
				else
				{
					if(hoverObject.name!="Ground")
					{
						Player owner = hoverObject.transform.root.GetComponent<Player>();
						if(owner)
						{
							Unit unit = hoverObject.transform.parent.GetComponent<Unit>();
                    		Building building = hoverObject.transform.parent.GetComponent<Building>();
                    		if(owner.username == player.username && (unit || building)) 
								player.hud.SetCursorState(CursorState.Select);
						}
					}
				}
			}
		}
	}
}
