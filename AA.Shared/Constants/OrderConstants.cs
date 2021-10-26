using System.Collections.Generic;
using AA.Shared.Enums;

namespace AA.Shared.Constants
{
	public static class OrderConstants
	{
		public static Dictionary<ProductType, double> ItemWidths => new Dictionary<ProductType, double>()
		{
			{ProductType.Calendar, 10 },
			{ProductType.Canvas, 16 },
			{ProductType.Cards, 4.7 },
			{ProductType.Mug, 94 },
			{ProductType.PhotoBook, 19 },
		};
	}
}