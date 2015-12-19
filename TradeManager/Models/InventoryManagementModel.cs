using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace TradeManager.Models
{
    public class InventoryManagementModel
    {
        public int ID { get; set; }
        [Required]
        public string SKU { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string LocationStock { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Stock Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StockTime { get; set; }
        [Required]
        public string Description { get; set; }
        //new Entries
        [Required]
        public string CategoryID { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double StartPrice { get; set; }
        //[Required]
        //[DataType(DataType.Currency)]
        //public double BuyItNowPrice { get; set; }
        public string ListingDuration { get; set; }
        [Required]
        public string LocationCity { get; set; }
        public string UserID { get; set; }


    }
    public class InventoryDBContext : DbContext
    {
        public DbSet<InventoryManagementModel> Inventories { get; set; }
    }
}