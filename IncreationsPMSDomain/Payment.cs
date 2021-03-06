﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IncreationsPMSDomain
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentRefNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public int SubContractorId { get; set; }
        public string SubName { get; set; }
        public string Address { get; set; }
        public decimal WorkAmount { get; set; }
        public decimal AcceptedAmount { get; set; }
        public int PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
        public string ChequeNo { get; set; }
        public string VoucherNo { get; set; }
        public string SpecialRemarks { get; set; }
        public int ProjectWorkDetailsId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal PayableAmount { get ; set; }
    }
}
