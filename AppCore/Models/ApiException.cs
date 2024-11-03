using System.Runtime.Serialization;

namespace AppCore.Models;

[Serializable]
public class ApiException : Exception
{
    protected ApiException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(
        serializationInfo, streamingContext)
    {
    }

    public ApiException(string message, StatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
        Message = string.IsNullOrEmpty(message) ? StatusCode.ToString() : message;
    }

    public ApiException(string message, StatusCode statusCode, object result) : base(message)
    {
        StatusCode = statusCode;
        Message = string.IsNullOrEmpty(message) ? StatusCode.ToString() : message;
        Data = result;
    }

    public ApiException(StatusCode statusCode) : base(statusCode.ToString())
    {
        StatusCode = statusCode;
        Message = MessageKey.ServerError;
    }

    public ApiException(string message) : base(message)
    {
        StatusCode = StatusCode.SERVER_ERROR;
        Message = string.IsNullOrEmpty(message) ? StatusCode.ToString() : message;
    }

    public ApiException() : base(StatusCode.SERVER_ERROR.ToString())
    {
    }

    public StatusCode StatusCode { get; set; } = StatusCode.SERVER_ERROR;
    public override string Message { get; } = MessageKey.ServerError;
    public new object Data { get; }
}

public enum StatusCode
{
    SUCCESS = 200,
    CREATED = 201,
    BAD_REQUEST = 400,
    UNAUTHORIZED = 401,
    FORBIDDEN = 403,
    NOT_FOUND = 404,
    NOT_ACTIVE = 405,
    CHANGE_PASSWORD = 406,
    NOT_VERIFY = 490,
    TIME_OUT = 408,
    ALREADY_EXISTS = 409,
    CHOOSE_ACCOUNT = 410,
    UNPROCESSABLE_ENTITY = 422,
    SERVER_ERROR = 500
}

public class MessageKey
{
    //Notify Content
    public const string BirthDayTitle = "system_notification_happy_birthday_for_user_1";
    public const string BirthDayContent = "system_notification_happy_birthday_for_user_2";
    public const string CelebrateMemberTitle = "system_notification_celebrate_1";
    public const string CelebrateMemberContent = "system_notification_celebrate_2";
    public const string CelebrateNcTitle = "system_notification_celebrate_3";
    public const string CelebrateNcContent = "system_notification_celebrate_4";
    public const string RequestLeaveGroup1 = "system_notification_nutrtition_club_1";
    public const string RequestLeaveGroup2 = "system_notification_nutrtition_club_2";
    public const string RequestLeaveGroup3 = "system_notification_nutrtition_club_3";

    //
    public const string CrossSiteRequestForgery = "system_message_cross_site_request_forgery";

    public const string EmailAlreadyExists = "system_message_email_already_exists";
    public const string EmailNotFounds = "system_message_email_not_found";
    public const string EmailTemplateNotFounds = "system_message_email_template_not_found";
    public const string PhoneNumberAlreadyExists = "system_message_phone_number_already_exists";
    public const string PersonalIdentityNumberAlreadyExists = "system_message_personal_identity_number_already_exists";
    public const string BadRequest = "system_message_bad_request";
    public const string DateTimeBadRequest = "system_message_date_time_bad_request";
    public const string QuantityBadRequest = "system_message_quantity_bad_request";
    public const string QuantityRequired = "system_message_quantity_required";
    public const string LocationChangeRequestRequired = "system_message_location_change_request_required";


    //
    public const string PlanNotFounds = "system_message_plan_not_found";
    public const string MapNotFounds = "system_message_map_not_found";
    public const string TerritoryNotFounds = "system_message_territory_not_found";
    public const string SubTerritoryNotFounds = "system_message_sub_territory_not_found";
    public const string OutletNotFounds = "system_message_outlet_not_found";
    public const string MicroTerritoryNotFounds = "system_message_micro_territory_not_found";
    public const string DatasetNotFounds = "system_message_dataset_not_found";
    public const string VisitPlanFounds = "system_message_visit_plan_not_found";
    public const string VisitPlanOutputFounds = "system_message_visit_plan_output_not_found";
    public const string SaleNumberInValid = "system_message_sale_number_in_valid";
    public const string OutletNumberInValid = "system_message_outlet_number_in_valid";
    public const string FrequencyInValid = "system_message_frequency_in_valid";
    public const string OptimizeInValid = "system_message_optimize_in_valid";


    public const string InvalidUsernameOrPassword = "system_message_invalid_username_or_password";
    public const string InvalidPasswordStruct = "system_message_invalid_password_struct";
    public const string InvalidFileType = "system_message_invalid_file_type";
    public const string AccountNotActivated = "system_message_account_not_activated";
    public const string AccountDeactive = "nc_delete_account_popup_blocked";
    public const string AccountIsLoggedInOnAnotherDevice = "system_message_account_is_logged_in_on_another_device";
    public const string Unauthorized = "system_message_unauthorized";
    public const string Forbidden = "system_message_forbidden";
    public const string NotActive = "system_message_not_active";
    public const string NotVerify = "system_message_not_verify";
    public const string AccountHasBeenLocked = "system_message_account_has_been_locked";
    public const string ProductGroupIsAlreadyExists = "system_message_product_group_already_exists";
    public const string NameIsAlreadyExists = "system_message_name_is_already_exists";
    public const string NameOrPackingIsAlreadyExists = "system_message_name_or_packing_is_already_exists";
    public const string SkuIsAlreadyExists = "system_message_sku_is_already_exists";


    public const string ChooseFile = "system_message_choose_file";
    public const string NotFound = "system_message_not_found";
    public const string LanguagesNotFound = "system_message_languages_not_found";
    public const string NutritionClubNotFound = "system_message_nutritionClub_not_found";
    public const string UserNotFound = "system_message_user_not_found";
    public const string UserIsNotPartOfNc = "system_message_user_is_not_part_of_nc";

    public const string InvoiceNotFound = "system_message_invoice_not_found";
    public const string InvoiceItemNotFound = "system_message_invoice_item_not_found";

    public const string AttendanceNotFound = "system_message_attendance_not_found";
    public const string AccountNotFound = "system_message_account_not_found";
    public const string ProductGroupNotFound = "system_message_product_group_not_found";
    public const string MessageNotFound = "system_message_not_found";
    public const string NcoNotFound = "system_message_nco_not_found";
    public const string HealthDiaryNotFound = "system_message_health_diary_not_found";
    public const string NccNotFound = "system_message_ncc_not_found";
    public const string VersionNotFound = "system_message_version_not_found";


    public const string ProductNotFound = "system_message_product_not_found";
    public const string ProductsIsRequired = "system_message_products_required";
    public const string MediaFileNotFound = "system_message_media_file_not_found";
    public const string PostNotFound = "system_message_post_not_found";
    public const string PackageNotFound = "system_message_package_not_found";
    public const string CategoryItemNotFound = "system_message_category-item_not_found";
    public const string CheckInNotFound = "system_message_check_in_not_found";

    public const string AreaNotFound = "system_message_area_not_found";
    public const string ProvinceNotFound = "system_message_province_not_found";
    public const string DistrictNotFound = "system_message_district_not_found";
    public const string LocationChangeRequestNotFound = "system_message_location_change_request_not_found";
    public const string ChallengeNotFound = "system_message_challenge_not_found";
    public const string CategoryNotFound = "system_message_category_not_found";
    public const string StartAtOrDurationNotFound = "system_message_start_at_or_duration_not_found";
    public const string DownlineNotFound = "system_message_downline_not_found";
    public const string ChatRoomNotFound = "system_message_chat_room_not_found";
    public const string MemberChatNotFound = "system_message_member_chat_not_found";
    public const string GroupNotFound = "system_message_group_not_found";


    public const string ServerError = "system_message_server_error";
    public const string HerbalifeServerError = "system_message_herbalife_server_error";

    public const string EmailIsAlreadyExistInSystemHerbalife =
        "system_message_email_is_already_exist_in_system_herbalife";

    public const string EmailAlreadyExistsInSystemHerbalifeButNotInHub =
        "system_message_email_already_exists_in_system_herbalife_but_not_in_hub";


    public const string RefreshTokenNotFound = "system_message_refresh_token_not_found";
    public const string TokenIsStillValid = "system_message_token_is_still_valid";
    public const string TokenExpired = "system_message_token_expired";
    public const string TokenInCorrect = "system_message_token_in_correct";
    public const string PersonalIdentityNumberInValid = "system_message_personalidentity_number_invalid";

    public const string PersonalIdentificationNumberThatIsSameAsAnotherPerson =
        "system_message_personalidentity_number_is_same_as_another_person";

    public const string PersonalIdentificationDoesNotMatchUser =
        "system_message_personal_identification_does_not_match_user";

    public const string PersonalIdentificationOfNco = "system_message_personal_identification_of_nco";


    public const string IsAlreadyExists = "system_message_is_already_exists";
    public const string NccHaveParticipatedInAll3Nc = "system_message_ncc_have_participated_in_all_3_nc";
    public const string UserIsAlreadyAMemberOfnc = "system_message_user_is_already_a_member_of_nc";
    public const string MediaFileIsAlreadyExists = "system_message_media_file_is_already_exists";


    public const string AccountAlreadyExists = "system_message_account_already_exists";
    public const string PackageIsAlreadyExists = "system_message_package_is_already_exists";
    public const string CheckInIsAlreadyExists = "system_message_check_in_is_already_exists";
    public const string UserIsAlreadyExists = "system_message_is_user_already_exists";


    public const string HerbalifeIdOrPackageNameIsAlreadyExists =
        "system_message_herbalife_id_or_package_name_is_already_exists";

    public const string CategoryItemIsAlreadyExists = "system_message_category_item_is_already_exists";
    public const string CanNotSendOtpSms = "system_message_can_not_send_otp_sms";


    // General
    public const string RoleRequired = "system_message_role_required";
    public const string StatusRequired = "system_message_status_required";
    public const string TypeRequired = "system_message_type_required";
    public const string CategoryRequired = "system_message_category_required";
    public const string SuccessfulDeleted = "system_message_successful_deleted";
    public const string FailedLogged = "system_message_logging_failed";
    public const string InvalidOtp = "system_message_invalid_otp";
    public const string CheckEmail = "system_message_check_mail_for_new_notification";
    public const string TooManyRequests = "system_message_too_many_requests";
    public const string Required = "system_message_required";
    public const string MinValueRequired = "system_message_min_value_required";
    public const string MaxValueRequired = "system_message_max_value_required";

    public const string Coordinates = "system_message_invalid_coordinates";


    public const string NotValidEmail = "system_message_not_valid_email";
    public const string ThisEmailBelongsToNco = "system_message_this_email_belongs_to_nco";
    public const string ThisEmailBelongsToMemberHerbalife = "system_message_this_email_be_longs_to_member_herbalife";
    public const string ThisEmailBelongsToUser = "system_message_this_email_be_longs_to_user";


    public const string ThisIdentityNumberBelongsToNco = "system_message_this_email_belongs_to_nco";
    public const string ThisEmailInactiveInSystem = "system_message_this_email_inactive_in_System";


    public const string NotValidPhoneNumber = "system_message_not_valid_phone_number";
    public const string NotValidRegion = "system_message_not_valid_region";
    public const string NotValidSubRegion = "system_message_not_valid_sub_region";
    public const string NotValidType = "system_message_not_valid_type";
    public const string NotValidStatus = "system_message_not_valid_status";
    public const string GroupNameRequired = "system_message_group_name_required";
    public const string PackageNameRequired = "system_message_package_name_required";
    public const string ProductNameRequired = "system_message_product_name_required";
    public const string CategoryItemIdRequired = "system_message_category_item_id_required";
    public const string ProductGroupIdRequired = "system_message_product_group_id_required";
    public const string PackingRequired = "system_message_packing_required";
    public const string MinPackageDuration = "system_message_min_package_duration";
    public const string MinProductDuration = "system_message_min_product_duration";
    public const string PointRequired = "system_message_poit_required";
    public const string PriceRequired = "system_message_price_required";
    public const string OriginRequired = "system_message_origin_required";
    public const string SkuRequired = "system_message_sku_required";
    public const string PackageIncludedRequired = "system_message_package_include_required";
    public const string SizeRequired = "system_message_size_required";
    public const string StoreRequired = "system_message_store_required";
    public const string DescriptionRequired = "system_message_description_required";
    public const string ContentRequired = "system_message_content_required";
    public const string UsageRequired = "system_message_usage_required";
    public const string TitleRequired = "system_message_title_required";
    public const string TimeTypeRequired = "system_message_time_type_required";
    public const string ParticipantsRequired = "system_message_paticipants_required";
    public const string MemberRequired = "system_message_member_required";
    public const string NutritionClubRequired = "system_message_nutrition_club_required";
    public const string PackageRequired = "system_message_package_required";
    public const string SessionRequired = "system_message_session_required";
    public const string ReasonForDeletingRequired = "system_message_reason_for_deleting_required";
    public const string HeightRequired = "system_message_height_required";
    public const string WeightRequired = "system_message_weight_required";
    public const string FatPercentageRequired = "system_message_fat_percentage_required";
    public const string VisceralFatRequired = "system_message_visceral_fat_required";
    public const string AmountOfBonesFatRequired = "system_message_amount_of_bones_required";
    public const string MetabolismBalanceRequired = "system_message_metabolism_balance_required";
    public const string AmountOfMuscleRequired = "system_message_amount_of_muscle_required";
    public const string BalanceIndexRequired = "system_message_balance_index_required";
    public const string WaterPercentageRequired = "system_message_water_percentage_required";
    public const string PhoneNumberRequired = "system_message_phone_number_required";
    public const string EmailRequired = "system_message_email_required";
    public const string GenderRequired = "system_message_gender_required";
    public const string DateOfBirthRequired = "system_message_date_of_birth_required";
    public const string InviteRequired = "system_message_invite_required";
    public const string StarRequired = "system_message_star_required";
    public const string HerbalifeIdRequired = "system_message_herbalife_id_required";
    public const string DurationRequired = "system_message_duration_required";
    public const string NumberOfUsesRequired = "system_message_number_of_uses_required";
    public const string ReferenceNotFound = "system_message_reference_not_found";


    public const string TooManyTimeInvalidOtp = "system_message_too_many_time_invalid_otp";
    public const string TooManyTimeResendOtp = "system_message_too_many_time_resend_otp";
    public const string AccountAlreadyActive = "system_message_account_already_active";

    // Account
    public const string UsernameRequired = "system_message_username_required";
    public const string FullnameRequired = "system_message_fullname_required";
    public const string ReasonRequired = "system_message_reason_required";


    // Article
    public const string ArticleNotFound = "system_message_article_not_found";
    public const string PublishDateTimeRequired = "system_message_publish_date_time_required";

    // Master import
    public const string NotSupportedFileType = "system_message_not_supported_file_type";
    public const string CannotReadFile = "system_message_can_not_read_file";
    public const string NotFoundTemplate = "system_message_not_found_template";

    // Member
    public const string MemberNotFound = "system_message_member_not_found";
    public const string UpdatePermissionForNccFailed = "system_message_update_permission_for_ncc_failed";

    //Challenge member
    public const string ChallengeMemberNotFound = "system_message_challenge_member_not_found";
    public const string ChallengeMemberNoteNotFound = "system_message_challenge_member_note_not_found";

    public const string ChallengeMemberNotActive = "system_message_challenge_member_not_active";
    public const string ChallengeMemberIsAlreadyExists = "system_message_challenge_member_is_already_exists";

    public const string CheckInAlreadyExists = "system_message_check_in_already_exists";


    // Chat room
    public const string ChatRoomAlreadyExists = "system_message_chat_room_already_exists";

    // Chat room member
    public const string ChatRoomMemberNotFound = "system_message_chat_room_member_not_found";

    // Chat message
    public const string ChatMessageNotFound = "system_message_chat_message_not_found";

    // Notification
    public const string NotificationNotFound = "system_message_notification_not_found";

    // Simulation
    public const string SimulationNotFound = "system_message_simulation_not_found";

    // NutritionClubMember
    public const string NutritionClubMemberNotFound = "system_message_nutrition_club_member_not_found";

    // Event
    public const string EventNotFound = "system_message_event_not_found";

    // Request leave group
    public const string RequestLeaveGroupNotFound = "system_message_request_leave_group_not_found";
    public const string RequestLeaveGroupAlreadyExists = "system_message_request_leave_group_already_exists";
    public const string RequestLeaveGroupNotActive = "system_message_request_leave_group_not_active";

    //Sticker
    public const string StickerNotFound = "system_message_sticker_not_found";

    // Required
    public const string NameRequired = "system_message_name_required";
    public const string AddressRequired = "system_message_address_required";
    public const string AreaRequired = "system_message_area_required";
    public const string ProvinceRequired = "system_message_province_required";
    public const string DistrictRequired = "system_message_district_required";
    public const string WardRequired = "system_message_ward_required";
    public const string ProvinceOrDistrictRequired = "system_message_province_or_district_required";
    public const string NcoRequired = "system_message_nco_required";
    public const string StartTimeRequired = "system_message_start_time_required";
    public const string EndTimeRequired = "system_message_end_time_required";
    public const string DateTimeRequired = "system_message_date_time_required";
    public const string DetailRequired = "system_message_detail_required";

    public const string RentExpenseRequired = "system_message_rent_expense_required";
    public const string ElectronicFeeRequired = "system_message_electronic_fee_required";

    public const string WaterFeeRequired = "system_message_water_fee_required";

    public const string ImportFeeRequired = "system_message_import_fee_required";

    public const string OtherFeeRequired = "system_message_other_fee_required";

    public const string DiscountRequired = "system_message_discount_required";

    public const string RevenueOtherRequired = "system_message_revenue_other_required";
    public const string RevenueRequired = "system_message_revenue_required";
    public const string GrandTotalRequired = "system_message_grand_total_required";
    public const string SubTotalRequired = "system_message_sub_total_required";


    public const string AssumedProfitRequired = "system_message_assumed_profit_required";
    public const string CanNotStopTheChallengeIsOver = "system_message_can_not_stop__the_challenge_is_over";


    public const string NutritionClubIdRequired = "system_message_nutrition_club_id_required";

    // Challenge WL
    public const string AssistantExceeding = "system_message_assistant_exceeding";
    public const string MemberExceeding = "system_message_member_exceeding";
    public const string CheerleaderExceeding = "system_message_cheerleader_exceeding";
    public const string BackgroundRequired = "system_message_background_required";
    public const string TotalDistanceRequired = "system_message_total_distance_required";
    public const string MinimumDistancePerDayRequired = "system_message_minimum_distance_per_day_required";
    public const string TargetWeightRequired = "system_message_target_weight_required";
    public const string RoundsRequired = "system_message_rounds_required";

    // Newsfeed WL
    public const string NewsfeedNotFound = "system_message_newsfeed_not_found";

    // Group WL
    public const string ChallengeRequired = "system_message_challenge_required";

    public const string ThumbnailRequired = "system_message_thumbnail_required";

    //Spouse
    public const string PleaseChooseAccount = "system_message_please_choose_account";
    public const string NcoNotificationChallengeDetail = "nco_notification_challenge_detail";
}