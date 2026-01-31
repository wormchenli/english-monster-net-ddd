using System.Security.Cryptography;
using System.Text;

namespace CommonUtils;

public class HashHelper
{
    public static string GenerateSha256Hash(Stream inputStream)
    {
        // helper function
        if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
        if (!inputStream.CanRead) throw new ArgumentException("Stream must be readable.", nameof(inputStream));
        if (!inputStream.CanSeek) throw new ArgumentException("Stream must be seekable (CanSeek == true).", nameof(inputStream));

        var originalPosition = inputStream.Position;

        try
        {
            inputStream.Position = 0;
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(inputStream);

            // return BitConverter.ToString(hashBytes);

            var sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();

        }
        catch (Exception e)
        {
            // Console.WriteLine(e);
            throw new InvalidOperationException("Failed to compute SHA256 hash.", e);
        }
        finally
        {
            inputStream.Position = originalPosition;
        }
    }
}