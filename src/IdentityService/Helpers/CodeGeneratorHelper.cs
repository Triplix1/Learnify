using System.Text;

namespace IdentityService.Helpers;

public static class CodeGeneratorHelper
{
    public static string Generate(int length)
    {
        StringBuilder sb = new StringBuilder(length);
        Random rand = new Random();
        
        for (int i = 0; i < length; i++)
        {
            var code = rand.Next(65, 122);
            sb.Append((char)code);
        }

        return sb.ToString();
    }
}