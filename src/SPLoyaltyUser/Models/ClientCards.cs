using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPLoyaltyUser.Models
{
    public partial class ClientCards
    {
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CardNumber { get; set; }
        public int? ClientId { get; set; }
        public bool IsVirtual { get; set; }
        public int CardStatusCodeId { get; set; }
        public bool IsPrimary { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CardExpirationDate { get; set; }
        public int CardTypeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RecordAddedDateTime { get; set; }
        [StringLength(50)]
        public string RecordAddedBy { get; set; }
        [Column("msrepl_tran_version")]
        public Guid MsreplTranVersion { get; set; }
        public int? ReplacedById { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReplacedDateTime { get; set; }

        [ForeignKey("ClientId")]
        [InverseProperty("ClientCards")]
        public Clients Client { get; set; }
    }
}
