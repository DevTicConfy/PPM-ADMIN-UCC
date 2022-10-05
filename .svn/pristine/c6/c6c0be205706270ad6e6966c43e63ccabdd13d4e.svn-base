using MC.BaseService.MessageBase;
using MC.BusinessService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MC.LiquidacionService.Messages
{
    [DataContract(Namespace = "http://www.MillensCorp.com/types/")]
    public class Liquidacion_Request : RequestBase
    {
        [DataMember]
        public string sSecuencia = string.Empty;
        [DataMember]
        public int iTipoVehiculo = 0;
        [DataMember]
        public bool bMensualidad = false;
        [DataMember]
        public bool bReposicion = false;
        [DataMember]
        public string sIdtarjeta = string.Empty;

        [DataMember]
        public string sConvenios = string.Empty;

    }
}
