// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

Console.WriteLine("Welcome to NCE Bulk Migration Tool!");
string? appId;
string? upn;

if (args.Length == 2)
{
    appId = args[0];
    upn = args[1];
}
else
{
AppId:
    Console.WriteLine("Enter AppId");
    appId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(appId) || !Guid.TryParse(appId, out _))
    {
        Console.WriteLine("Invalid input, Please try again!");
        goto AppId;
    }

Upn:
    Console.WriteLine("Enter Upn");
    upn = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(upn))
    {
        Console.WriteLine("Invalid input, Please try again!");
        goto Upn;
    }
}

var appSettings = new AppSettings()
{
    AppId = appId,
    Upn = upn,
};

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((services) =>
    {
        services.AddSingleton(appSettings);
        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddSingleton<ICustomerProvider, CustomerProvider>();
        services.AddSingleton<ISubscriptionProvider, SubscriptionProvider>();
        services.AddSingleton<INewCommerceMigrationProvider, NewCommerceMigrationProvider>();
        services.AddSingleton<INewCommerceMigrationScheduleProvider, NewCommerceMigrationScheduleProvider>();
    }).Build();

await RunAsync(host.Services);

await host.RunAsync();

static async Task RunAsync(IServiceProvider serviceProvider)
{
ShowOptions:
    Console.WriteLine("Please choose an option");

    Console.WriteLine("1. Export customers");
    Console.WriteLine("2. Export subscriptions with migration eligibility");
    Console.WriteLine("3. Upload migrations");
    Console.WriteLine("4. Export migration status");
    Console.WriteLine("5. Export NCE subscriptions");
    Console.WriteLine("6. Export subscriptions with migration eligibility to schedule migrations");
    Console.WriteLine("7. Upload migration schedules");
    Console.WriteLine("8. Export schedule migrations");
    Console.WriteLine("9. Cancel schedule migrations");
    Console.WriteLine("10. Exit");

SelectOption:
    var option = Console.ReadLine();

    if (!short.TryParse(option, out short input) || !(input >= 1 && input <= 10))
    {
        Console.WriteLine("Invalid input, Please try again! Possible values are {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}");
        goto SelectOption;
    }

    if (input == 10)
    {
        Console.WriteLine("Exiting the app!");
        Environment.Exit(Environment.ExitCode);
    }

    Directory.CreateDirectory($"{Constants.InputFolderPath}/subscriptions/processed");
    Directory.CreateDirectory($"{Constants.InputFolderPath}/migrations/processed");
    Directory.CreateDirectory($"{Constants.InputFolderPath}/subscriptionsforschedule/processed");
    Directory.CreateDirectory($"{Constants.InputFolderPath}/cancelschedulemigrations/processed");
    Directory.CreateDirectory(Constants.OutputFolderPath);

    Stopwatch stopwatch = Stopwatch.StartNew();

    var result = input switch
    {
        1 => await serviceProvider.GetRequiredService<ICustomerProvider>().ExportCustomersAsync(),
        2 => await serviceProvider.GetRequiredService<ISubscriptionProvider>().ExportLegacySubscriptionsAsync(),
        3 => await serviceProvider.GetRequiredService<INewCommerceMigrationProvider>().UploadNewCommerceMigrationsAsync(),
        4 => await serviceProvider.GetRequiredService<INewCommerceMigrationProvider>().ExportNewCommerceMigrationStatusAsync(),
        5 => await serviceProvider.GetRequiredService<ISubscriptionProvider>().ExportModernSubscriptionsAsync(),
        6 => await serviceProvider.GetRequiredService<INewCommerceMigrationScheduleProvider>().ValidateAndGetSubscriptionsToScheduleMigrationAsync(),
        7 => await serviceProvider.GetRequiredService<INewCommerceMigrationScheduleProvider>().UploadNewCommerceMigrationSchedulesAsync(),
        8 => await serviceProvider.GetRequiredService<INewCommerceMigrationScheduleProvider>().ExportNewCommerceMigrationSchedulesAsync(),
        9 => await serviceProvider.GetRequiredService<INewCommerceMigrationScheduleProvider>().CancelNewCommerceMigrationSchedulesAsync(),
        _ => throw new InvalidOperationException("Invalid input")
    };

    stopwatch.Stop();
    Console.WriteLine($"Completed the operation {input} in {stopwatch.Elapsed}");
    Console.WriteLine("========================================================");

    goto ShowOptions;
}