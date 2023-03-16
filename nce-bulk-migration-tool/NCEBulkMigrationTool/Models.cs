﻿// -----------------------------------------------------------------------
// <copyright file="Models.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCEBulkMigrationTool;

internal record ResourceCollection<T>(IEnumerable<T> Items, int TotalCount, string ContinuationToken);

internal record Customer(Guid Id, CompanyProfile CompanyProfile);

internal record CompanyProfile(Guid TenantId, string Domain, string CompanyName);

internal record Subscription
{
    public string Id { get; init; } = string.Empty;

    public string FriendlyName { get; init; } = string.Empty;

    public string OfferId { get; init; } = string.Empty;

    public string OfferName { get; init; } = string.Empty;

    public int Quantity { get; set; }

    public string ParentSubscriptionId { get; init; } = string.Empty;

    public DateTime EffectiveStartDate { get; init; }

    public DateTime CommitmentEndDate { get; init; }

    public SubscriptionStatus Status { get; init; }

    public BillingCycleType BillingCycle { get; set; }

    public string TermDuration { get; set; } = string.Empty;

    public string PartnerId { get; set; } = string.Empty;

    public ItemType ProductType { get; init; } = new ItemType();

    public bool AutoRenewEnabled { get; init; }

    public NewCommerceEligibility MigrationEligibility { get; set; } = new NewCommerceEligibility();

    public string MigratedFromSubscriptionId { get; init; } = string.Empty;
}

internal record MigrationRequest
{
    public string PartnerTenantId { get; init; } = string.Empty;

    public string IndirectResellerMpnId { get; init; } = string.Empty;

    public string CustomerName { get; init; } = string.Empty;

    public Guid CustomerTenantId { get; init; }

    public string LegacySubscriptionId { get; init; } = string.Empty;

    public string LegacySubscriptionName { get; init; } = string.Empty;

    public string LegacyProductName { get; init; } = string.Empty;

    public DateTime ExpirationDate { get; init; }

    public bool AutoRenewEnabled { get; init; }

    public bool MigrationEligible { get; set; }

    public string NcePsa { get; set; } = string.Empty;

    public string CurrentTerm { get; init; } = string.Empty;

    public string CurrentBillingPlan { get; init; } = string.Empty;

    public int CurrentSeatCount { get; set; }

    public bool StartNewTermInNce { get; init; }

    public string Term { get; init; } = string.Empty;

    public string BillingPlan { get; init; } = string.Empty;

    public int SeatCount { get; set; }

    public DateTime? CustomTermEndDate { get; set; }

    public bool AddOn { get; init; }

    public string BaseSubscriptionId { get; init; } = string.Empty;

    public string MigrationIneligibilityReason { get; set; } = string.Empty;
}

internal record ModernSubscription
{
    public string PartnerTenantId { get; init; } = string.Empty;

    public string IndirectResellerMpnId { get; init; } = string.Empty;

    public string CustomerName { get; init; } = string.Empty;

    public Guid CustomerTenantId { get; init; }

    public string SubscriptionId { get; init; } = string.Empty;

    public string SubscriptionName { get; init; } = string.Empty;

    public string ProductName { get; init; } = string.Empty;

    public DateTime ExpirationDate { get; init; }

    public string Psa { get; set; } = string.Empty;

    public string Term { get; init; } = string.Empty;

    public string BillingPlan { get; init; } = string.Empty;

    public int SeatCount { get; set; }

    public string MigratedFromSubscriptionId { get; init; } = string.Empty;
}

public record ItemType
{
    public string Id { get; init; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}

public record NewCommerceMigration
{
    public string Id { get; set; } = string.Empty;

    public DateTime? StartedTime { get; set; }

    public DateTime? CompletedTime { get; set; }

    public string CurrentSubscriptionId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string CustomerTenantId { get; set; } = string.Empty;

    public string CatalogItemId { get; set; } = string.Empty;

    public string NewCommerceSubscriptionId { get; set; } = string.Empty;

    public string NewCommerceOrderId { get; set; } = string.Empty;

    public DateTime? SubscriptionEndDate { get; set; }

    public int Quantity { get; set; }

    public string TermDuration { get; set; } = string.Empty;

    public string BillingCycle { get; set; } = string.Empty;

    public bool PurchaseFullTerm { get; set; }

    public DateTime? CustomTermEndDate { get; set; }

    public string ExternalReferenceId { get; set; } = string.Empty;

    public IEnumerable<NewCommerceMigration> AddOnMigrations { get; set; } = Enumerable.Empty<NewCommerceMigration>();
}

public record NewCommerceMigrationError
{
    public int Code { get; set; } = 0;

    public string Description { get; set; } = string.Empty; 
}

public record MigrationResult
{
    public string PartnerTenantId { get; init; } = string.Empty;

    public string IndirectResellerMpnId { get; init; } = string.Empty;

    public string CustomerName { get; init; } = string.Empty;

    public Guid CustomerTenantId { get; init; }

    public string LegacySubscriptionId { get; init; } = string.Empty;

    public string LegacySubscriptionName { get; init; } = string.Empty;

    public string LegacyProductName { get; init; } = string.Empty;

    public DateTime ExpirationDate { get; init; }

    public bool AddOn { get; init; }

    public string BaseSubscriptionId { get; init; } = string.Empty;

    public string MigrationStatus { get; init; } = string.Empty;

    public bool StartedNewTermInNce { get; init; }

    public string NCETermDuration { get; init; } = string.Empty;

    public string NCEBillingPlan { get; init; } = string.Empty;

    public int NCESeatCount { get; set; }

    public DateTime? CustomTermEndDate { get; set; }

    public int? ErrorCode { get; set; } = null;

    public string ErrorReason { get; init; } = string.Empty;

    public string NCESubscriptionId { get; init; } = string.Empty;

    public string BatchId { get; init; } = string.Empty;

    public string MigrationId { get; init; } = string.Empty;
}

public record NewCommerceEligibility
{
    public string CurrentSubscriptionId { get; set; } = string.Empty;

    public bool IsEligible { get; set; }

    public string TermDuration { get; set; } = string.Empty;

    public string BillingCycle { get; set; } = string.Empty;

    public string CatalogItemId { get; set; } = string.Empty;

    public DateTime? CustomTermEndDate { get; set; }

    public IEnumerable<NewCommerceEligibilityError> Errors { get; set; } = Enumerable.Empty<NewCommerceEligibilityError>();

    public IEnumerable<NewCommerceEligibility> AddOnMigrations { get; set; } = Enumerable.Empty<NewCommerceEligibility>();
}

public record NewCommerceEligibilityError
{
    public NewCommerceEligibilityErrorCode Code { get; set; }

    public string Description { get; set; } = string.Empty;
}

public record CustomerErrorResponse
{
    public int Code { get; set; }

    public string Message { get; set; } = string.Empty;
}

public record NewCommerceMigrationSchedule
{
    public string Id { get; set; } = string.Empty;

    public string CurrentSubscriptionId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string CustomerTenantId { get; set; } = string.Empty;

    public string CatalogItemId { get; set; } = string.Empty;

    public DateTime? SubscriptionEndDate { get; set; }

    public int Quantity { get; set; }

    public string TermDuration { get; set; } = string.Empty;

    public string BillingCycle { get; set; } = string.Empty;

    public bool PurchaseFullTerm { get; set; }

    public DateTime? CustomTermEndDate { get; set; }

    public DateTime? TargetDate { get; set; }

    public bool? MigrateOnRenewal { get; set; }

    public DateTimeOffset CreatedTime { get; set; }

    public DateTimeOffset LastModifiedTime { get; set; }

    public string ExternalReferenceId { get; set; } = string.Empty;

    public string NewCommerceMigrationId { get; set; } = string.Empty;

    public string ErrorDescription { get; set; } = string.Empty;

    public int? ErrorCode { get; set; }

    public IEnumerable<NewCommerceMigrationSchedule> AddOnMigrationSchedules { get; set; } = Enumerable.Empty<NewCommerceMigrationSchedule>();
}

internal record ScheduleMigrationRequest
{
    public string PartnerTenantId { get; init; } = string.Empty;

    public string IndirectResellerMpnId { get; init; } = string.Empty;

    public string CustomerName { get; init; } = string.Empty;

    public Guid CustomerTenantId { get; init; }

    public string LegacySubscriptionId { get; init; } = string.Empty;

    public string LegacySubscriptionName { get; init; } = string.Empty;

    public string LegacyProductName { get; init; } = string.Empty;

    public DateTime ExpirationDate { get; init; }

    public bool AutoRenewEnabled { get; init; }

    public bool AddOn { get; init; }

    public string BaseSubscriptionId { get; init; } = string.Empty;

    public bool MigrationEligible { get; set; }

    public string NcePsa { get; set; } = string.Empty;

    public string CurrentTerm { get; init; } = string.Empty;

    public string CurrentBillingPlan { get; init; } = string.Empty;

    public int CurrentSeatCount { get; set; }

    public bool StartNewTermInNce { get; init; }

    public string Term { get; init; } = string.Empty;

    public string BillingPlan { get; init; } = string.Empty;

    public int SeatCount { get; set; }

    public DateTime? CustomTermEndDate { get; set; }

    public DateTime? TargetDate { get; set; }

    public bool? MigrateOnRenewal { get; set; }

    public string MigrationIneligibilityReason { get; set; } = string.Empty;
}

public record ScheduleMigrationResult
{
    public string PartnerTenantId { get; init; } = string.Empty;

    public string IndirectResellerMpnId { get; init; } = string.Empty;

    public string CustomerName { get; init; } = string.Empty;

    public Guid CustomerTenantId { get; init; }

    public string LegacySubscriptionId { get; init; } = string.Empty;

    public string LegacySubscriptionName { get; init; } = string.Empty;

    public string LegacyProductName { get; init; } = string.Empty;

    public DateTime ExpirationDate { get; init; }

    public bool AddOn { get; init; }

    public bool StartedNewTermInNce { get; init; }

    public string NCETermDuration { get; init; } = string.Empty;

    public string NCEBillingPlan { get; init; } = string.Empty;

    public int NCESeatCount { get; set; }

    public DateTime? CustomTermEndDate { get; set; }

    public DateTime? TargetDate { get; set; }

    public bool? MigrateOnRenewal { get; set; }

    public string MigrationScheduleStatus { get; init; } = string.Empty;

    public string MigrationScheduleId { get; init; } = string.Empty;
    
    public string BatchId { get; init; } = string.Empty;

    public int? ErrorCode { get; set; } = null;

    public string ErrorReason { get; init; } = string.Empty;
}

public enum NewCommerceEligibilityErrorCode
{
    SubscriptionStatusNotActive = 0,

    SubscriptionBeingProcessingInOms = 1,

    SubscriptionTooCloseToTermEnd = 2,

    SubscriptionPromotionsPresent = 3,

    SubscriptionAddOnsPresent = 4,

    NewCommerceProductUnavailable = 5,

    TermDurationBillingCycleCombinationNotSupported = 6,

    SubscriptionActiveForLessThanOneMonth = 7,

    TradeStatusNotAllowed = 8,

    InvalidPartnerIdOnRecord = 9,

    MigrationsWithinLast24hOfTermOnlyAllowedForFullTerm = 10,

    InvalidCustomTermEndDate = 11,

    CustomTermEndDateMustAlignWithExistingSubscriptionOrCalendarMonth = 12,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum SubscriptionStatus
{
    None = 0,

    Active = 1,

    Suspended = 2,

    Deleted = 3,

    Expired = 4,

    Pending = 5,

    Disabled = 6,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum BillingCycleType
{
    Unknown = 0,

    Monthly = 1,

    Annual = 2,

    None = 3,

    OneTime = 4,

    Triennial = 5,

    Biennial = 6,
}

internal record Constants
{
    public const string InputFolderPath = "ncebulkmigration/input";

    public const string OutputFolderPath = "ncebulkmigration/output";

    public const string PartnerCenterClientHeader = "MS-PartnerCenter-Client";

    public const string ClientName = "BulkMigrationTool";
}

internal record Routes
{
    /// <summary>
    /// Base url for the partner center Apis.
    /// </summary>
    public const string BaseUrl = "https://api.partnercenter.microsoft.com";

    /// <summary>
    /// Get customers route.
    /// </summary>
    public const string GetCustomers = "/v1/customers?size=500";

    /// <summary>
    /// Get subscriptions route, 0 represents customerId.
    /// </summary>
    public const string GetSubscriptions = "/v1/customers/{0}/subscriptions";

    /// <summary>
    /// Get new commerce migrations route, 0 represents customerId, 1 represents newCommerceMigrationId.
    /// </summary>
    public const string GetNewCommerceMigration = "/v1/customers/{0}/migrations/newcommerce/{1}";

    /// <summary>
    /// Validate migration eligibility route, 0 represents customerId.
    /// </summary>
    public const string ValidateMigrationEligibility = "/v1/customers/{0}/migrations/newcommerce/validate";

    /// <summary>
    /// Post new commerce migration, 0 represents customerId.
    /// </summary>
    public static string PostNewCommerceMigration = "/v1/customers/{0}/migrations/newcommerce";

    /// <summary>
    /// Get new commerce migrations route, 0 represents customerId, 1 represents newCommerceMigrationScheduleId.
    /// </summary>
    public const string GetNewCommerceMigrationSchedule = "/v1/customers/{0}/migrations/newcommerce/schedules/{1}";

    /// <summary>
    /// Post new commerce migration schedule, 0 represents customerId.
    /// </summary>
    public const string PostNewCommerceMigrationSchedule = "/v1/customers/{0}/migrations/newcommerce/schedules";

    /// <summary>
    /// Update new commerce migration schedule, 0 represents customerId, 1 represents newCommerceMigrationScheduleId.
    /// </summary>
    public const string UpdateNewCommerceMigrationSchedule = "/v1/customers/{0}/migrations/newcommerce/schedules/{1}";

    /// <summary>
    /// Get new commerce migration schedules route.
    /// </summary>
    public const string GetNewCommerceMigrationSchedules = "/v1/migrations/newcommerce/schedules";

    /// <summary>
    /// Cancel new commerce migration schedule, 0 represents customerId, 1 represents newCommerceMigrationScheduleId.
    /// </summary>
    public const string CancelNewCommerceMigrationSchedule = "/v1/customers/{0}/migrations/newcommerce/schedules/{1}/cancel";
}