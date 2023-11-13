using EFCoreBase.RefresherService;

namespace ProcedureMakerServer.Services;



public interface IProcedureEntityRefresher : IRefresherServe
{

}

public class ProcedureEntityRefresher : RefresherServe<ProcedureContext> , IProcedureEntityRefresher
{
    public ProcedureEntityRefresher(ProcedureContext context) : base(context)
    {

    }
}
