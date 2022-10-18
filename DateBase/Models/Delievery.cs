using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBase.Models
{
    public class Delievery
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Product_Id { get; set; }
        public long Product_Code { get; set; }
        public string Product_Name { get; set; }
        public double Count { get; set; }
        public string Company { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
    }
}
