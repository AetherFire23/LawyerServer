using Newtonsoft.Json;

namespace ProcedureMakerServer.Utils;

public static class JsonHelper
{

	public static JsonSerializerSettings AcceptLoops = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.All };


}
