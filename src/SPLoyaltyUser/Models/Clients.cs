using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPLoyaltyUser.Models
{
    public partial class Clients
    {
        public Clients()
        {
            ClientCards = new HashSet<ClientCards>();
            InverseReplaced = new HashSet<Clients>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CardOrAccountNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CardExpirationDate { get; set; }
        [StringLength(50)]
        public string SerialTrack3 { get; set; }
        [Column("CVV")]
        [StringLength(3)]
        public string Cvv { get; set; }
        [StringLength(50)]
        public string Fname { get; set; }
        [StringLength(50)]
        public string Lname { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FormFilledDate { get; set; }
        public int? StoresId { get; set; }
        public int? IssuingCountryId { get; set; }
        public int? ClientGroupId { get; set; }
        public int? PriceLevelId { get; set; }
        public int? ReplacedId { get; set; }
        [StringLength(50)]
        public string RecordAddedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RecordAddedDateTime { get; set; }
        public bool? AbbyyImported { get; set; }
        public int? AccountInactiveCodeId { get; set; }
        public int? CardTypeId { get; set; }
        public int LegalEntityId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReplacedDateTime { get; set; }
        public bool? IsDataChangedUponReplacement { get; set; }
        public byte? GenderId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PensionDate { get; set; }
        [Column("msrepl_tran_version")]
        public Guid MsreplTranVersion { get; set; }
        public int LoyaltyId { get; set; }

        [ForeignKey("ReplacedId")]
        [InverseProperty("InverseReplaced")]
        public Clients Replaced { get; set; }
        [InverseProperty("Clients")]
        public ClientsPublic ClientsPublic { get; set; }
        [InverseProperty("Client")]
        public ICollection<ClientCards> ClientCards { get; set; }
        [InverseProperty("Replaced")]
        public ICollection<Clients> InverseReplaced { get; set; }
    }
}
