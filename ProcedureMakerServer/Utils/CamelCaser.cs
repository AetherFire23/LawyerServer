using System.Text;

namespace ProcedureMakerServer.Utils;

public static class CamelCaser
{
	public static string LowerCaseFirstLetter(this string self)
	{
		char s = self.ToLower().ToCharArray()[0];

		StringBuilder builder = new StringBuilder();

		for (int i = 0; i < self.Length; i++)
		{
			if (i == 0)
			{
				_ = builder.Append(s);
			}
			else
			{
				char current = self[i];
				_ = builder.Append(current);
			}
		}

		return builder.ToString();
	}
}
