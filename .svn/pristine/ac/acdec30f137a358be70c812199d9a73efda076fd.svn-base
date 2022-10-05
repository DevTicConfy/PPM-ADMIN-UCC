using MC.LiquidacionService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MC.LiquidacionService.ServiceContracts
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface ILiquidacionService
    {
        [OperationContract]
        Liquidacion_Response getDatosLiquidacion(Liquidacion_Request request);
    }
}
