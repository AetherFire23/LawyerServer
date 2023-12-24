using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using MailKit.Net.Imap;
using MailKit.Security;

namespace ProcedureMakerServer.Scratches;

public static class MailKitTests
{
    private const string GMailAccount = "richerf3212@gmail.com";
    public static async Task DoTest()
    {

    }

    private static async Task OAuthAsync(ImapClient client)
    {
        ClientSecrets clientSecrets = new ClientSecrets
        {
            ClientId = "XXX.apps.googleusercontent.com",
            ClientSecret = "XXX"
        };

        GoogleAuthorizationCodeFlow codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            DataStore = new FileDataStore("CredentialCacheFolder", false),
            Scopes = new[] { "https://mail.google.com/" },
            ClientSecrets = clientSecrets
        });

        // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
        LocalServerCodeReceiver codeReceiver = new LocalServerCodeReceiver();
        AuthorizationCodeInstalledApp authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

        UserCredential credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

        if (credential.Token.IsExpired(SystemClock.Default))
            _ = await credential.RefreshTokenAsync(CancellationToken.None);

        // Note: We use credential.UserId here instead of GMailAccount because the user *may* have chosen a
        // different GMail account when presented with the browser window during the authentication process.
        SaslMechanism oauth2;

        if (client.AuthenticationMechanisms.Contains("OAUTHBEARER"))
            oauth2 = new SaslMechanismOAuthBearer(credential.UserId, credential.Token.AccessToken);
        else
            oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

        await client.AuthenticateAsync(oauth2);
    }
}
