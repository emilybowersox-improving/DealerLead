using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class Vehicle
    {
        [Key]
        [Column("VehicleId")]
        public int Id { get; set; }


        [Required]
        /* [Column("ModelId")]*/
        [Display(Name = "Model")]
        public int ModelId { get; set; }

        public SupportedModel Model { get; set; }


        [Required]
        [Column("MSRP")]
        public decimal MSRP { get; set; }

        [Required]
        [Display(Name = "Stock Number")]
        [Column("StockNumber")]
        public string StockNumber { get; set; }

        [Required]
        [Column("Color")]
        public string Color { get; set; }


        [Required]
        [Display(Name = "Dealership")]
        [Column("DealershipId")]
        public int DealershipId { get; set; }
        public Dealership Dealership { get; set; }


        [Display(Name = "Sell Date")]
        [Column("SellDate")]
        public DateTime? SellDate { get; set; }


        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }
    }
}
