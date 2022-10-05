using MC.BaseService.MessageBase;
using MC.BusinessService.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MC.LiquidacionService.Messages
{
    [DataContract(Namespace = "http://www.MillensCorp.com/types/")]
    public class Liquidacion_Response : ResponseBase
    {
        [DataMember]
        public DtoLiquidacion oDtoLiquidacion;

        [DataMember]
        public DtoLiquidacion oDtoLiquidacionCasco;

        [DataMember]
        public List<DtoDatosLiquidacion> olstDtoLiquidacion;

        [DataMember]
        public DtoTransaccion oDtoTransaccion;

    }
}
