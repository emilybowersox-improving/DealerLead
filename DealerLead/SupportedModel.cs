using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class SupportedModel
    {
        [Key]
        [Column("ModelId")]
        public int Id { get; set; }

        [Required]
        [Column("ModelName")]
        public string Name { get; set; }



/*        [ForeignKey("MakeId")]*/
        public int MakeID { get; set; }

        public SupportedMake Make { get; set;
        }



        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }


    }
}
