using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Services;

public class LawyerService : ILawyerService
{

    private readonly ILawyerRepository _lawyerRepository;

    public LawyerService(ILawyerRepository lawyerRepository)
    {
        _lawyerRepository = lawyerRepository;
    }
}
