using IdentityServer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models.Dto
{
    public class ClientSecretDto
    {
        [Required]
        public string Type { get; set; } = "SharedSecret";

        [Required]
        public string Value { get; set; }

        public string HashType { get; set; }

        public HashType HashTypeEnum => Enum.TryParse(HashType, true, out HashType result) ? result : Enums.HashType.Sha256;

        public DateTime? Expiration { get; set; }
    }
}
