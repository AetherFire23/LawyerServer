using EFCoreBase.RefresherService;

namespace ProcedureMakerServer.Services;



public class ProcedureEntityRefresher : RefresherServe<ProcedureContext>
{
    public ProcedureEntityRefresher(ProcedureContext context) : base(context)
    {
    }
}
