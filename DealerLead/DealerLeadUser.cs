﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class DealerLeadUser
    {
        [Key]
        [Column("UserId")]
        public int Id { get; set; }

        // constraint: unqiue
        [Column("AzureADId")]
        public Guid Oid { get; set; }


        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }
   
    }
}