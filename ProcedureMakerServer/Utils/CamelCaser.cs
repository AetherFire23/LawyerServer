using System.Text;

namespace ProcedureMakerServer.Utils;

public static class CamelCaser
{
    public static string LowerCaseFirstLetter(this string self)
    {
        var s = self.ToLower().ToCharArray()[0];

        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < self.Length; i++)
        {
            if (i == 0)
            {
                builder.Append(s);
            }
            else
            {
                char current = self[i];
                builder.Append(current);
            }
        }

        return builder.ToString();
    }
}
