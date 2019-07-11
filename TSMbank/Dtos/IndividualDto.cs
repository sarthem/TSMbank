using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.Dtos
{
    public class IndividualDto
    {
        public string Id { get; set; }

        [StringLength(255)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(255)]
        [Required]
        public string LastName { get; set; }

        [StringLength(255)]
        [Required]
        public string FathersName { get; set; }

        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdentificationCardNo { get; set; }
        public string SSN { get; set; }
        public string VatNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public IndividualStatus Status { get; set; }
        public int PrimaryAddressId { get; set; }
        public int? SecondaryAddressId { get; set; }
    }
}