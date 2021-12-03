using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NorthwindApiWithId.Models
{
    [DataContract(Name = "User")]
    [Serializable]
    public class UserDTO
    {
        [DataMember]
        public int PKID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}