using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CuStore.CRMService.OperationContracts
{
    [DataContract]
    public class CustomerData
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string ExternalCode { get; set; }

        [DataMember]
        public int Points { get; set; }

        [DataMember]
        public decimal Ratio { get; set; }
    }
}