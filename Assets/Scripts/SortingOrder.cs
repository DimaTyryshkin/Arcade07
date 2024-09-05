using UnityEngine;

namespace Action7
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SortingOrder : MonoBehaviour
	{
		[SerializeField] bool isDynamic;
		[SerializeField] int offset;
        
		Transform thisTransform;
		SpriteRenderer spriteRenderer;
         
		public static float MaxY { get; set; }

		public void Start()
		{
			thisTransform = transform;
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();

			UpdateOrder();

			if (!isDynamic && Application.isPlaying)
				Destroy(this);
		}

		void LateUpdate()
		{
			UpdateOrder();
		}

		void UpdateOrder()
		{
			spriteRenderer.sortingOrder = offset + GetOrderFromY(thisTransform.position.y);
		}

		public static int GetOrderFromY(float yPosition)
		{


			if (yPosition > MaxY)
				Debug.LogError(yPosition > MaxY);


			return (int)(-(yPosition - MaxY));
		}
	}
}