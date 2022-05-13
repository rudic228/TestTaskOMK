using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Document
    {
        [Key]
        public string Id { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
        public DateTime DateDocument { get; set; }
        public string DocumentType { get; set; }
        public string DeliveryNumber { get; set; }
        public string Barcode { get; set; }
        public string Workshop { get; set; }
        public string Stage { get; set; }
        public string RolePersonChange { get; set; }
        public string Note { get; set; }
    }
}
