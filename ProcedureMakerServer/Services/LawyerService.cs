using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository;

namespace ProcedureMakerServer.Services;

public class LawyerService : ILawyerService
{

    private readonly LawyerRepository _lawyerRepository;

    public LawyerService(LawyerRepository lawyerRepository)
    {
        _lawyerRepository = lawyerRepository;
    }
}
