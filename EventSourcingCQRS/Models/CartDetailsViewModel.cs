using System.Collections.Generic;
using System.Linq;
using EventSourcingCQRS.ReadModel.Cart;
using EventSourcingCQRS.ReadModel.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventSourcingCQRS.Models
{
    public class CartDetailsViewModel
    {
        public Cart Cart { get; internal set; }

        public IEnumerable<CartItem> CartItems { get; internal set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<SelectListItem> AvailableProducts
        {
            get
            {
                return Products.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                })
                .ToList();
            }
        }
    }
}
