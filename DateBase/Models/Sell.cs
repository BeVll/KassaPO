using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBase.Models
{
    public class Sell
    {
        [Key]
        public int Id { get; set; }
        public long Product_Id { get; set; }
        public long ProductCode { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
        public int Cashier_id { get; set; }
        public DateTime DateTime { get; set; }
    }
}
