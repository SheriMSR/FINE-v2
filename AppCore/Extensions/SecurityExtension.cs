using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AppCore.Extensions;

public static class SecurityExtension
{
    public static string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(5);
    }

    public static string GenerateRandomString()
    {
        var randomGenerator = RandomNumberGenerator.Create();
        var data = new byte[16];
        randomGenerator.GetBytes(data);
        return BitConverter.ToString(data);
    }

    public static string HashPassword(this string password, string salt)
    {
        // return BCrypt.Net.BCrypt.HashPassword(password + salt, 5, true);
        return BCrypt.Net.BCrypt.HashPassword(password + salt, 5);
    }

    public static bool VerifyPassword(this string password, string salt, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password + salt, hash);
    }

    public static string GenerateRandomString(int length)
    {
        var randomGenerator = RandomNumberGenerator.Create();
        var data = new byte[length];
        randomGenerator.GetBytes(data);
        return BitConverter.ToString(data).Replace("-", "");
    }

    public static string GenerateOtp()
    {
        var random = new Random();
        var code = random.Next(0, 1000000);
        return code.ToString("D6");
    }

    public static string EncryptData(this string key, string value)
    {
        // Get bytes of plaintext string
        var encodeKey = Encoding.UTF8.GetBytes(key);
        var plainBytes = Encoding.UTF8.GetBytes(value);

        // Get parameter sizes
        var nonceSize = AesGcm.NonceByteSizes.MaxSize;
        var tagSize = AesGcm.TagByteSizes.MaxSize;
        var cipherSize = plainBytes.Length;

        // We write everything into one big array for easier encoding
        var encryptedDataLength = 4 + nonceSize + 4 + tagSize + cipherSize;
        var encryptedData = encryptedDataLength < 1024
            ? stackalloc byte[encryptedDataLength]
            : new byte[encryptedDataLength].AsSpan();

        // Copy parameters
        BinaryPrimitives.WriteInt32LittleEndian(encryptedData[..4], nonceSize);
        BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4), tagSize);
        var nonce = encryptedData.Slice(4, nonceSize);
        var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
        var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

        RandomNumberGenerator.Fill(nonce);

        // Encrypt
        using var aes = new AesGcm(encodeKey);
        aes.Encrypt(nonce, plainBytes.AsSpan(), cipherBytes, tag);

        // Encode for transmission
        return Convert.ToBase64String(encryptedData);
    }

    public static string DecryptData(this string key, string value)
    {
        var encodeKey = Encoding.UTF8.GetBytes(key);
        // Decode
        var encryptedData = Convert.FromBase64String(value).AsSpan();

        // Extract parameter sizes
        var nonceSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData[..4]);
        var tagSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4));
        var cipherSize = encryptedData.Length - 4 - nonceSize - 4 - tagSize;

        // Extract parameters
        var nonce = encryptedData.Slice(4, nonceSize);
        var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
        var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

        // Decrypt
        var plainBytes = cipherSize < 1024
            ? stackalloc byte[cipherSize]
            : new byte[cipherSize];
        using var aes = new AesGcm(encodeKey);
        aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

        // Convert plain bytes back into string
        return Encoding.UTF8.GetString(plainBytes);
    }

    public static string GenerateRandomPassword()
    {
        var length = 10;
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
        var password = new StringBuilder();
        var random = new Random();

        for (var i = 0; i < length; i++)
        {
            var randomIndex = random.Next(0, validChars.Length);
            password.Append(validChars[randomIndex]);
        }

        return password.ToString();
    }


    public static bool IsMatchRegex(this string key)
    {
        var validateRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        return validateRegex.IsMatch(key);
    }

    public static string ReplaceSpecialCharacter(this string key)
    {
        var regex = new Regex("[=+\\-[\x09\x0D]");
        return regex.Replace(key, string.Empty, 1);
    }
}