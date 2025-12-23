using System.ComponentModel.DataAnnotations;

namespace EasyBuy.Models
{
    public class Customer
    {
        [Key]
        public int constomer_id {  get; set; }
        public string constomer_name { get; set; }
        public string? constomer_phone { get; set; }
        public string constomer_email { get; set; }
        public string constomer_password { get; set; }
        public string? constomer_gender { get; set; }
        public string? constomer_country { get; set; }
        public string? constomer_city { get; set; }
        public string? constomer_address { get; set; }
        public string? constomer_image { get; set; }
    }
}
