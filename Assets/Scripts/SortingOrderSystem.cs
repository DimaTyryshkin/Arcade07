using System;
using System.Collections.Generic;
using GamePackages.Core.Validation;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Action7
{
	public class SortingOrderSystem : MonoBehaviour
	{
		[SerializeField, IsntNull] Grid grid;
		[SerializeField, IsntNull] Tilemap tilemap;

		Dictionary<Vector3Int, TileSortingOrder> cellToTile;

		readonly int heightToLayerFactor = 16;
		public readonly int PixelsPerUnit = 16;
		float MaxY { get; set; }

		void OnDrawGizmos()
		{
			//Gizmos.color = Color.red;
			//GizmosExtension.DrawBounds(GetTimeMapWorldBounds(tilemap));
		}

		
		void Start()
		{
			Calculate();
		}

		Bounds GetTimeMapWorldBounds(Tilemap tilemap)
		{
			Vector3 size = tilemap.cellBounds.size;
			size.x *= grid.cellSize.x;
			size.y *= grid.cellSize.y;
			size.z *= grid.cellSize.z;

			Vector3 center = tilemap.transform.position;
			Vector3 offset = tilemap.cellBounds.center;
			offset.x *= grid.cellSize.x;
			offset.y *= grid.cellSize.y;

			
			return new Bounds(center + offset, size);
		}

		public int GetOrderFromY(float yPosition)
		{
			if (yPosition > MaxY)
				Debug.LogError(yPosition > MaxY);


			float fromTopToPosition = -(yPosition - MaxY);
			return (int)(fromTopToPosition * heightToLayerFactor);
		}

		public TileSortingOrder TileByCell(Vector3 world)
		{
			var cell = grid.WorldToCell(world);
			return cellToTile.GetValueOrDefault(cell);
		}
		
		public Vector3Int WorldToCell(Vector3 world)
		{
			return grid.WorldToCell(world); 
		}
		
		public TileSortingOrder GetByCell(Vector3 world)
		{
			var cell = grid.WorldToCell(world);
			return cellToTile[cell];
		}

		[Button()]
		public void Calculate()
		{
			MaxY = GetTimeMapWorldBounds(tilemap).max.y; 
			cellToTile = new Dictionary<Vector3Int, TileSortingOrder>();
			var allSortingOrders = grid.GetComponentsInChildren<TileSortingOrder>(true);
			if (allSortingOrders.Length == 0)
				return;

			MaxY = Mathf.Max(MaxY, allSortingOrders[0].transform.position.y);

			foreach (var sortingOrder in allSortingOrders)
			{
				sortingOrder.Init(this);

				if (!sortingOrder.IsDynamic)
				{
					Vector3Int cell = grid.WorldToCell(sortingOrder.transform.position);
					if (cellToTile.ContainsKey(cell))
						throw new Exception();

					cellToTile[cell] = sortingOrder;

					float y = sortingOrder.transform.position.y;
					if (y > MaxY)
						MaxY = y;
				}
			}

			foreach (var sortingOrder in allSortingOrders)
				sortingOrder.CalculateOrder();

			cellToTile = null;
		}
	}
}