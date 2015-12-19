using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TradeManager.Models
{
    public class MarketResearchModel
    {
        public int ID { get; set; }
        [Required]
        public string Keyword { get; set; }
        public double PriceFrom { get; set; }
        public double PriceTo { get; set; }
        public double AveragePrice { get; set; }
        public string UserID { get; set; }
    }
    
}