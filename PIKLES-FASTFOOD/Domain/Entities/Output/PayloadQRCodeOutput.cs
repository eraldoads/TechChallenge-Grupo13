using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Output
{
    public class PayloadQRCodeOutput
    {
        public string description { get; set; }
        public string external_reference { get; set; }
        public List<ItemPagamento> items { get; set; }
        public string notification_url { get; set; }
        public Sponsor sponsor { get; set; }
        public string title { get; set; }
        public double total_amount { get; set; }
    }
    public class ItemPagamento
    {
        public string title { get; set; }
        public string description { get; set; }
        public double unit_price { get; set; }
        public int quantity { get; set; }
        public string unit_measure { get; set; }
        public double total_amount { get; set; }
    }

    public class Sponsor
    {
        public int id { get; set; }
    }
}
