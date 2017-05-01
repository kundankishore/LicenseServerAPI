using System;
using System.Collections.Generic;

namespace LicenseService.Models
{
    public class Account
    {
        public string DfeNumber { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string CustomerType { get; set; }
        public string CustomerSubType { get; set; }
        public string AddressStreet1 { get; set; }
        public string AddressStreet2 { get; set; }
        public string AddressStreet3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string AccountName { get; set; }
        public string Mainphone { get; set; }
        public string OfstedURN { get; set; }
        public string LowerAgeRange { get; set; }
        public string UpperAgeRange { get; set; }
        public string Governance { get; set; }
        public string LaNumber { get; set; }
        public Guid LocalAuthority { get; set; }
        public Guid SchoolGroupGuid { get; set; }
        public string ZipOrPostCode { get; set; }
        public string Status { get; set; }
    }
}