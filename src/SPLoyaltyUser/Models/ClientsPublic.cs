using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPLoyaltyUser.Models
{
    public partial class ClientsPublic
    {
        public int Id { get; set; }
        public int ClientsId { get; set; }
        [MaxLength(8000)]
        public byte[] PwdHash { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PwdExpirationDate { get; set; }
        [StringLength(100)]
        public string AddressLine1 { get; set; }
        public int? SubTownId { get; set; }
        public int? TownId { get; set; }
        [StringLength(5)]
        public string Zip { get; set; }
        public int? CountryId { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }
        [StringLength(15)]
        public string PhoneOther { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public bool? AcceptEmail { get; set; }
        public bool? AcceptMail { get; set; }
        [Column("AcceptSMS")]
        public bool? AcceptSms { get; set; }
        [Column("AcceptSMSMailEmail")]
        public bool? AcceptSmsmailEmail { get; set; }
        [Column("msrepl_tran_version")]
        public Guid MsreplTranVersion { get; set; }
        public byte? DefaultLanguageCodeId { get; set; }
        public short? EducationCodeId { get; set; }
        public short? FamilyStatusId { get; set; }
        public short? IncomeCodeId { get; set; }
        public short? OccupationCodeId { get; set; }
        public short? PhoneCountryPrefixId { get; set; }
        public short? PhoneOtherCountryPrefixId { get; set; }
        [StringLength(50)]
        public string RecordModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RecordModifiedDateTime { get; set; }
        public bool? IsPhoneConfirmed { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool? IsAddressConfirmed { get; set; }
        public bool? AcceptTargetedEmail { get; set; }
        public bool? DgtlStmp { get; set; }
        public bool IsEmailVerified { get; set; }
        public short? EmailChangeAttemptsInLastDateCnt { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EmailChangeAttemptsLastDate { get; set; }
        public int? FavoriteStoresId { get; set; }
        public bool? IsPhoneVerified { get; set; }

        [ForeignKey("ClientsId")]
        [InverseProperty("ClientsPublic")]
        public Clients Clients { get; set; }
    }
}
