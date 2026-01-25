using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvApp.Models
{
    internal class CoverLetter
    {
        public string? AddressTo { get; set; }
        public string? Salutation { get; set; }
        public string? JobSource { get; set; }
        public string? Position { get; set; }
        public string? CompanyLocation { get; set; } = string.Empty;
        public string? Skills { get; set; }
        public bool? Organization { get; set; }
        public bool? ConvertToPdf { get; set; }
    }
}
