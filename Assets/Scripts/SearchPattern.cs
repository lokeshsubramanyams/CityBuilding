using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SearchPattern : MonoBehaviour
{
	public Vector2 rowsColumns;
	public int searchLeft;
	public int searchRight;
	public int searchTop;
	public int searchBottom;

	public enum SearchDirection
	{
		LEFT,
		RIGHT,
		TOP,
		BOTTOM
	}

	void Start()
	{
		if(rowsColumns!=Vector2.zero)
		{
			searchLeft = Mathf.RoundToInt((rowsColumns.x-1)*0.5f);
			searchRight = Mathf.RoundToInt((rowsColumns.x-1)*0.5f)-((rowsColumns.x%2==0)?1:0);
			searchTop = Mathf.RoundToInt((rowsColumns.y-1)*0.5f)-((rowsColumns.y%2==0)?1:0);
			searchBottom = Mathf.RoundToInt((rowsColumns.y-1)*0.5f);
			
		}
	}

	void Update()
	{
	/*	if(rowsColumns!=Vector2.zero)
		{
			searchLeft = Mathf.RoundToInt((rowsColumns.x-1)*0.5f);
			searchRight = Mathf.RoundToInt((rowsColumns.x-1)*0.5f)-((rowsColumns.x%2==0)?1:0);
			searchTop = Mathf.RoundToInt((rowsColumns.y-1)*0.5f)-((rowsColumns.y%2==0)?1:0);
			searchBottom = Mathf.RoundToInt((rowsColumns.y-1)*0.5f);
			
		}*/

	}

	/*public bool ValidePosition(GridNode node)
	{

	}*/

	bool SearchTop(GridNode node,int currentponter)
	{
		if(node.isFilled)return false;
		else 
		{
			bool leftcheck = SearchLeft(node.left,0);
			bool rightcheck = SearchRight(node.right,0);
			if(!leftcheck || !rightcheck)return false;
			if(currentponter>=searchTop)return true;

			return SearchTop(node.top,currentponter++);
		}
	}

	bool SearchLeft(GridNode node,int currentponter)
	{
		if(node.isFilled)return false;
		else 
		{
			if(currentponter>=searchLeft)return true;
			return SearchLeft(node.left,currentponter++);
		}
	}
	bool SearchBottom(GridNode node,int currentponter)
	{

		if(node.isFilled)return false;
		else 
		{
			bool leftcheck = SearchLeft(node.left,0);
			bool rightcheck = SearchRight(node.right,0);
			if(!leftcheck || !rightcheck)return false;
			if(currentponter>=searchBottom )return true;
			
			return SearchTop(node.bottom,currentponter++);
		}
		
	}
	bool SearchRight(GridNode node,int currentponter)
	{
		if(node.isFilled)return false;
		else 
		{
			if(currentponter>=searchRight)return true;
			return  SearchRight(node.right,currentponter++);
		}
	}

	public bool TotalSearch(GridNode node)
	{
		bool top = SearchTop(node,searchTop,0);
		bool bottom = SearchBottom(node,searchBottom,0);
		return (top && bottom);
	}


	bool SearchTop(GridNode node,int dept,int pointer)
	{
		if(node == null)return false;
		if(node.isFilled)return false;
		else 
		{
			if(pointer>=dept)return true;
			int searchcount = 0;

			searchcount += Search (GetNode(node,SearchDirection.TOP),SearchDirection.LEFT,searchLeft,0);
			searchcount += Search (GetNode(node,SearchDirection.TOP),SearchDirection.RIGHT,searchRight,0);

			if(searchcount>=2)
				return	SearchTop(GetNode(node,SearchDirection.TOP),dept,pointer++);
			else return false;
		}
	}

	bool SearchBottom(GridNode node,int dept,int pointer)
	{
		if(node == null)return false;
		if(node.isFilled)return false;
		else 
		{
			if(pointer>=dept)return true;
			int searchcount = 0;
			searchcount += Search (GetNode(node,SearchDirection.BOTTOM),SearchDirection.LEFT,searchLeft,0);
			searchcount += Search (GetNode(node,SearchDirection.BOTTOM),SearchDirection.RIGHT,searchRight,0);
			if(searchcount>=2)
				return	SearchBottom(GetNode(node,SearchDirection.BOTTOM),dept,pointer++);
			else return false;
		}
	}

	int Search(GridNode node, SearchDirection dir,int dept,int pointer)
	{
		if(node == null)return 0;
		if(node.isFilled)return 0;
		else 
		{
			if(pointer>=dept)return 1;
			return Search (GetNode(node,dir),dir,dept,pointer++);
		}
	}


	GridNode GetNode(GridNode node,SearchDirection dir)
	{
		GridNode n = null;
		switch(dir)
		{
		case SearchDirection.TOP 	:n= node.top;break;
		case SearchDirection.BOTTOM :n= node.bottom;break;
		case SearchDirection.LEFT 	:n= node.left;break;
		case SearchDirection.RIGHT  :n= node.right;break;
		}
		return n;
	}


}
