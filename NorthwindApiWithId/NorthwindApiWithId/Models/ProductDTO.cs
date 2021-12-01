using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NorthwindApiWithId.Models
{
    [DataContract(Name = "Product")]
    [Serializable]
    public class ProductDTO
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public Nullable<int> SupplierID { get; set; }
        [DataMember]
        public Nullable<int> CategoryID { get; set; }
        [DataMember]
        public string QuantityPerUnit { get; set; }
        [DataMember]
        public Nullable<decimal> UnitPrice { get; set; }
        [DataMember]
        public Nullable<short> UnitsInStock { get; set; }
        [DataMember]
        public Nullable<short> UnitsOnOrder { get; set; }
        [DataMember]
        public Nullable<short> ReorderLevel { get; set; }
        [DataMember]
        public bool Discontinued { get; set; }
    }
}