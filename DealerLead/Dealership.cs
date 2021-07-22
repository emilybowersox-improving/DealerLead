using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class Dealership
    {
        [Key]
        [Column("DealershipId")]
        public int Id { get; set; }

        [Required]
        [Column("DealershipName")]
        public string Name {get; set;}

        [Required]
        [Column("StreetAddress1")]
        [Display(Name = "Address 1")]
        public string Address1 {get; set;}

        [Column("StreetAddress2")]
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Required]
        [Column("City")]
        public string City { get; set; }

        /*   [Column("State")]
           public string State { get; set; }*/

        [Required]
        [Column("State")]
        [Display(Name = "State")]
        public string StateAbbreviation { get; set; }
        public SupportedState State { get; set; }

        [Required]
        [Column("Zipcode")]
        public string Zip { get; set; }

/*        [ScaffoldColumn(false)]*/
        
        [Column("CreatingUserId")]
        public int CreatorId { get; set; }


        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }
    }
}
