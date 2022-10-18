using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBase.Models
{
    public class GoodItem
    {
        public int Id { get; set; }
        public long ProductCode { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Count { get; set; }
        public double Discount { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
    }
}
