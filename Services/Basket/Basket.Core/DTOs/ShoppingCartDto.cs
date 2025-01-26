using Basket.Core.Entities;

namespace Basket.Core.DTOs
{
	public class ShoppingCartDto
	{
		public string UserName { get; set; }
		public List<ShoppingCartItem> Items { get; set; }

		public ShoppingCartDto() { }

		public ShoppingCartDto(string userName)
		{
			UserName = userName;
		}

		public decimal TotalPrice
		{
			get
			{
				decimal totalPrice = 0;

				foreach (var item in Items)
				{
					totalPrice += item.Price * item.Quantity;
				}

				return totalPrice;
			}
		}
	}
}
