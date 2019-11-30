using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserApi.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
