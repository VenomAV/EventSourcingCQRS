using EventSourcingCQRS.ReadModel.Cart;
using EventSourcingCQRS.ReadModel.Customer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcingCQRS.Models
{
    public class CartIndexViewModel
    {
        public IEnumerable<Cart> Carts { get; set; }

        public IEnumerable<Customer> Customers { get; set; }

        public IEnumerable<SelectListItem> AvailableCustomers
        {
            get
            {
                return Customers.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                })
                .ToList();
            }
        }
    }
}
