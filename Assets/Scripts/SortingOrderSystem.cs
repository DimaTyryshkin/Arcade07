using GamePackages.Core.Validation;
using NaughtyAttributes;
using UnityEngine;

namespace Action7
{
	public class SortingOrderSystem : MonoBehaviour
	{
		[SerializeField, IsntNull] Transform sortingOrderRoot;

		[Button()]
		void Start()
		{
			SortingOrder.MaxY = 1000;
			var allSortingOrders = sortingOrderRoot.GetComponentsInChildren<SortingOrder>();
			if (allSortingOrders.Length == 0)
				return;

			SortingOrder.MaxY = allSortingOrders[0].transform.position.y;

			foreach (var sortingOrder in allSortingOrders)
			{
				float y = sortingOrder.transform.position.y;
				if (y > SortingOrder.MaxY)
					SortingOrder.MaxY = y;
			}

			foreach (var sortingOrder in allSortingOrders)
				sortingOrder.Start();
		}
	}
}