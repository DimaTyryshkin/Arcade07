using GamePackages.Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace Action7
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class TileSortingOrder : MonoBehaviour
	{
		[SerializeField] string groupName;
		[SerializeField] float offsetY;
		[SerializeField] bool isDynamic;

		int order;
		Transform thisTransform;
		SpriteRenderer spriteRenderer;
		SortingOrderSystem system;
		public string GroupName => groupName;
		public bool IsDynamic => isDynamic;

		public int Order
		{
			get => order;
			set
			{
				if (order != value)
				{
					order = value;
					spriteRenderer.sortingOrder = order != -1 ? order : 0;
				}
			}
		}

		public void Start()
		{ 
			if (!isDynamic && Application.isPlaying)
				Destroy(this);
		}

		void LateUpdate()
		{
			Order = GetOrderFromY(transform.position.y);
		}

		public void Init(SortingOrderSystem system)
		{
			Assert.IsNotNull(system);
			this.system = system;
			
			thisTransform = transform;
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			Order = -1;
		}
		
		public int CalculateOrder()
		{
			if (order == -1)
			{
				Vector3 pos = transform.position;

				Assert.IsFalse(isDynamic && !string.IsNullOrEmpty(GroupName));
				bool groupingAvailable = !isDynamic && !string.IsNullOrEmpty(GroupName);
				if (groupingAvailable)
				{
					Vector3Int cell = system.WorldToCell(pos);
					cell.y--;

					var tileDown = system.TileByCell(cell);
					if (tileDown && tileDown.GroupName == GroupName)
					{
						//GizmosDrawer.Inst.GetMarker(tileDown.transform.position).Text("Tile").Duration(10);
						//GizmosDrawer.Inst.AddArrow(transform.position, tileDown.transform.position).Duration = 10;
						Order = tileDown.CalculateOrder();//+ 1 * system.heightToLayerFactor;
						return Order;
					}
				}

				Order = GetOrderFromY(pos.y);
			}

			return order;
		}

		int GetOrderFromY(float y)
		{
			return system.GetOrderFromY(y + offsetY);
		}
	}
}