using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppCore.Models;

public class BaseQuery
{
    private const int MaxPageCount = 50;

    private int _page = 1;

    private int _pageCount = MaxPageCount;
    public Guid? Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual DateTime? CreateAtFrom { get; set; }
    public virtual DateTime? CreateAtTo { get; set; }
    public virtual Guid? CreatorId { get; set; }

    public int Page
    {
        get => _page;
        set => _page = value < 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageCount;
        set => _pageCount = value > MaxPageCount || value < 0 ? MaxPageCount : value;
    }

    public string OrderBy { get; set; } = "CreatedAt desc";

    public int Skip()
    {
        return PageSize * (Page - 1);
    }
}

public class BaseDto
{
    public Guid Id { get; set; }
    public ulong No { get; set; }
    [JsonIgnore] public Guid? CreatorId { get; set; }
    [JsonIgnore] public Guid? EditorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }
    public AccountCreator Creator { get; set; }
    public AccountCreator Editor { get; set; }
}

public class AccountCreator
{
    public Guid Id { get; set; }
    // public AccountCreatorRole? Role { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Avatar { get; set; }
    public string HerbalifeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; }
}

public enum AccountCreatorRole : byte
{
    [Display(Name = "system_enum_account_role_system_admin")]
    SystemAdmin = 1,

    [Display(Name = "system_enum_account_role_admin")]
    Admin = 2,

    [Display(Name = "system_enum_account_role_nco")]
    Nco = 3,

    [Display(Name = "system_enum_account_role_ncc")]
    Ncc = 4,

    [Display(Name = "system_enum_account_role_warehouse_staff")]
    WarehouseStaff = 5,

    [Display(Name = "system_enum_account_role_admin_hal")]
    AdminHal = 6,
}