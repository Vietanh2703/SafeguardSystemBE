namespace SafeguardSystem.DAL.Constants;

public static class SecurityGuardConstants
{
    // Trạng thái của bảo vệ
    public const string PENDING = "PENDING"; // Chờ xét duyệt
    public const string ACTIVE = "ACTIVE"; // Đang làm việc
    public const string INACTIVE = "INACTIVE"; // Ngừng hoạt động
    public const string SUSPENDED = "SUSPENDED"; // Bị đình chỉ
}