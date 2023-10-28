using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Constants;


[TsConstantClass]
public class UserEndpoints 
{
    public const string Path = "user";
    public const string CredentialsLogin = nameof(CredentialsLogin);
    public const string TokenLogin = nameof(TokenLogin);


    //public void DoIt()
    //{

    //}
}




// should have controller class members that are static here with static refs to 