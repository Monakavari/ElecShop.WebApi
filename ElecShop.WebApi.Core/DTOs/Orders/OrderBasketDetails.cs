using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.Core.DTOs.Orders
{
    public class OrderBasketDetails
    {
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}
