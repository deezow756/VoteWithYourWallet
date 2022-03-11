using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace VoteWithYourWallet.Models
{
    [Table("Signature")]
    public class Signature
    {
        [Key]
        [Column(Order = 1)]
        public int CauseId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string IdentityUserId { get; set; }
        public string Name { get; set; }

        [ForeignKey("IdentityUserId")]
        public IdentityUser IdentityUser { get; set; }
        [ForeignKey("CauseId")]
        public Cause Cause { get; set; }
    }
}
