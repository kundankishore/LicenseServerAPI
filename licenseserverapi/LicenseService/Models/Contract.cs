using System;
using System.Collections.Generic;

namespace LicenseService.Models
{
    public class Contract
    {
        public Guid CrmId { get; set; }
        
        public IList<SubList> SubList;
    }

   
}

public class SubList
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid SupportedBy { get; set; }
    public string ProductName { get; set; }
    public Guid ProductId { get; set; }
}
