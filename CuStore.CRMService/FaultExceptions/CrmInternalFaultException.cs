using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace CuStore.CRMService.FaultExceptions
{
    [DataContract]
    public class CrmInternalFaultException : FaultException
    {
        public CrmInternalFaultException(string reason) : base(reason)
        {
        }
    }
}