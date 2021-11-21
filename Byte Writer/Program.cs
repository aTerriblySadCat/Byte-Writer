using System.Text;
using System.Text.RegularExpressions;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Please enter the path and name of the file that you'd like to write to:");
string filePath = Console.ReadLine();

while (true)
{
	Console.WriteLine();
	Console.WriteLine("Input nothing and press to close the application.");
	Console.Write("Now enter the location (in bytes) at where you'd like to start writing: ");
	string startPosStr = Console.ReadLine();
	if(startPosStr == null || startPosStr == "")
	{
		break;
	}
	long startPos = long.Parse(startPosStr);

	Regex regex = new Regex("(0[xX][0-9a-fA-F]+)|([0-9a-fA-F]+)");
	using (Stream s = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.Read))
	{
		s.Position = startPos;

		while (true)
		{
			Console.WriteLine();
			Console.WriteLine("Input nothing and press enter to go to another location.");
			Console.WriteLine("Enter the bytes you'd like to write, separated by a space (or one at a time):");
			string inputStr = Console.ReadLine();

			if (inputStr == null || inputStr == "")
			{
				break;
			}
			else
			{
				MatchCollection matches = regex.Matches(inputStr);
				if (matches.Count > 0)
				{
					foreach(Match match in matches)
					{
						if(match.Success)
						{
							string bStr = match.Value;
							if(bStr.StartsWith("0x"))
							{
								bStr = bStr.Remove(0, 2);
							}

							byte[] b = { byte.Parse(bStr, System.Globalization.NumberStyles.HexNumber) };
							s.Write(b, 0, 1);
						}
						else if(match.Value != " ")
						{
							Console.WriteLine(match.Value + " is not a valid hexadecimal number!");
						}
					}
				}
				else
				{
					Console.WriteLine("Please enter any bytes!");
					Console.WriteLine("Valid examples are 0x90F3 or 90F3.");
				}
			}
		}
	}
}