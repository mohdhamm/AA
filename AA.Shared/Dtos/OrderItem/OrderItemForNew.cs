using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AA.Shared.Enums;

namespace AA.Shared.Dtos.OrderItem
{
	public class OrderItemForNew : IValidatableObject
	{
		public int Quantity { get; set; }

		public string ProductType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Enum.TryParse(ProductType, true, out ProductType result))
            {
                yield return new ValidationResult("Invalid Product type", new[] { nameof(ProductType) });
            }

            ProductType = result.ToString();
        }
    }
}