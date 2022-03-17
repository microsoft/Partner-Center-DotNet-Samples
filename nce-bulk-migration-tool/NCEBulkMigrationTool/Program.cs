
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
    }).Build();

await RunAsync(host.Services);

await host.RunAsync();

static async Task RunAsync(IServiceProvider serviceProvider)
{
    Directory.CreateDirectory($"{Constants.InputFolderPath}/subscriptions/processed");
    Directory.CreateDirectory($"{Constants.InputFolderPath}/migrations/processed");
    Directory.CreateDirectory(Constants.OutputFolderPath);

    Console.WriteLine("Please choose an option");

    Console.WriteLine("1. Export customers");
    Console.WriteLine("2. Export subscriptions with migration eligibility");
    Console.WriteLine("3. Upload migrations");
    Console.WriteLine("4. Export migration status");
    Console.WriteLine("5. Export NCE subscriptions");

SelectOption:
    var option = Console.ReadLine();

    if (!short.TryParse(option, out short input) || !(input >= 1 && input <= 5))
    {
        Console.WriteLine("Invalid input, Please try again! Possible values are {1, 2, 3, 4, 5}");
        goto SelectOption;
    }

    Stopwatch stopwatch = Stopwatch.StartNew();

    var result = input switch
    {
        1 => await serviceProvider.GetRequiredService<ICustomerProvider>().ExportCustomersAsync(),
        2 => await serviceProvider.GetRequiredService<ISubscriptionProvider>().ExportLegacySubscriptionsAsync(),
        3 => await serviceProvider.GetRequiredService<INewCommerceMigrationProvider>().UploadNewCommerceMigrationsAsync(),
        4 => await serviceProvider.GetRequiredService<INewCommerceMigrationProvider>().ExportNewCommerceMigrationStatusAsync(),
        5 => await serviceProvider.GetRequiredService<ISubscriptionProvider>().ExportModernSubscriptionsAsync(),
        _ => throw new InvalidOperationException("Invalid input")
    };

    stopwatch.Stop();
    Console.WriteLine($"Completed the operation in {stopwatch.Elapsed}");
}