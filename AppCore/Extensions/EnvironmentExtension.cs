namespace AppCore.Extensions;

public static class EnvironmentExtension
{
    // ==================================== GLOBAL ======================================
    public static string GetAppLogFolder() =>
        Environment.GetEnvironmentVariable("LOG_FOLDER") ?? "Logs";

    public static string GetTimeZone() =>
        Environment.GetEnvironmentVariable("TZ") ?? string.Empty;

    public static string GetEnvironment() =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    public static bool IsSendOtp() => Environment.GetEnvironmentVariable("IS_SEND_OTP") == "True";
    public static bool IsProduction() => GetEnvironment() == "Production";
    public static bool IsStaging() => GetEnvironment() == "Staging";
    public static bool IsDevelopment() => GetEnvironment() == "Development";

    public static string GetAppConnectionString() =>
        Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? string.Empty;

    public static string GetPath() =>
        Environment.GetEnvironmentVariable("DOMAIN_PATH") ?? string.Empty;

    public static string GetDomain() =>
        Environment.GetEnvironmentVariable("DOMAIN") ?? string.Empty;

    public static string GetPayOSApiUri() =>
        Environment.GetEnvironmentVariable("PAY_OS_API_URI") ?? string.Empty;
    
    public static string GetPayOSApiKey() =>
        Environment.GetEnvironmentVariable("PAY_OS_API_KEY") ?? string.Empty;
    
    public static string GetPayOSClientID() =>
        Environment.GetEnvironmentVariable("PAY_OS_CLIENT_ID") ?? string.Empty;
    
    public static string GetPayOSSecretKey() =>
        Environment.GetEnvironmentVariable("PAY_OS_SECRET_KEY") ?? string.Empty;
   
    public static string GetJwtIssuer() =>
        Environment.GetEnvironmentVariable("JWT_ISSUER") ?? string.Empty;

    public static string GetJwtAudience() =>
        Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? string.Empty;

    public static string GetJwtAccessTokenSecret() =>
        Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_SECRET") ?? string.Empty;

    public static double GetJwtAccessTokenExpires() =>
        Convert.ToDouble(Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_EXPIRES") ?? "0");

    public static double GetJwtRefreshTokenExpires() =>
        Convert.ToDouble(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_EXPIRES") ?? "0");

    public static string GetS3AccessKey() =>
        Environment.GetEnvironmentVariable("S3_ACCESS_KEY") ?? string.Empty;

    public static string GetS3SecretKey() =>
        Environment.GetEnvironmentVariable("S3_SECRET_KEY") ?? string.Empty;

    public static string GetS3ServiceUrl() =>
        Environment.GetEnvironmentVariable("S3_SERVICE_URL") ?? string.Empty;

    public static string GetBucketName() =>
        Environment.GetEnvironmentVariable("S3_BUCKET_NAME") ?? string.Empty;

    public static string GetS3EndpointUrl() =>
        Environment.GetEnvironmentVariable("S3_ENDPOINT_URL") ?? string.Empty;

    public static string GetFireBaseCertFilePath()
    {
        if (IsProduction())
            return Environment.GetEnvironmentVariable("FB_CERT_FILE_PATH_PRODUCTION");
        return Environment.GetEnvironmentVariable(
            IsStaging() ? "FB_CERT_FILE_PATH_STAGING" : "FB_CERT_FILE_PATH_DEVELOPMENT"
        ) ?? string.Empty;
    }
}