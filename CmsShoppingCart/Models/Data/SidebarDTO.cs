using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShoppingCart.Models.Data
{
    [Table("tblsidebar")]
    public class SidebarDTO
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
    }
}