// <copyright file="Program.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Globalization;
using MCARefreshBulkAttestationCLITool;
using Microsoft.Extensions.Hosting;
using MCARefreshBulkAttestationCLITool.Interfaces;
using MCARefreshBulkAttestationCLITool.Providers;
using Refit;
using Polly;
using Serilog;
using Serilog.Events;
using static MCARefreshBulkAttestationCLITool.McaHttpClientExtensions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Refit.Destructurers;
using Serilog.Exceptions;

public class Program
{
    private static async Task Main(string[] args)
    {
        var logLevel = args.Any(a => a.Equals("debug", StringComparison.OrdinalIgnoreCase)) ? LogEventLevel.Debug : LogEventLevel.Information;

        var dt = DateTime.Now;
        var logFile = $"Logs/log_{DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss", CultureInfo.InvariantCulture)}.txt";

        Console.WriteLine("Welcome to the Microsoft Customer Agreement Bulk Attestation Tool!"); 

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(logFile, logLevel)
            .Enrich
                .WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers(new[] { new ApiExceptionDestructurer(destructureHttpContent: true) }))
            .CreateLogger();

        try
        {
            var appSettings = ConfigureApplicationSettings(args);

            using IHost host = Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((hostBuilder, services) =>
            {
                var config = hostBuilder.Configuration;

                services.AddRefitClient<ICustomerAgreementsClient>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.partnercenter.microsoft.com"))
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)))
                    .AddHttpMessageHandler<PartnerCenterAuthorizationHandler>();

                services.AddSingleton<PartnerCenterAuthorizationHandler>();
                services.AddSingleton(appSettings);
                services.AddSingleton<ITokenProvider, TokenProvider>();
                services.AddSingleton<IFileProvider, CsvProvider>();
                services.AddSingleton<ICustomerProvider, CustomerProvider>();
            }).Build();

            await RunAsync(host.Services);

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nApplication startup failed. See log file {logFile} for more details.");
            Console.WriteLine($"Exception message: {ex.Message}");
            Log.Fatal(ex, "Application startup failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static AppSettings ConfigureApplicationSettings(string[] args)
    {
        string? appId = null;
        string? upn = null;

        bool bypassMfaCheck = args.Any(a => a.Equals("noMfa", StringComparison.OrdinalIgnoreCase));

        AppId:
            Console.WriteLine("\nEnter Application Id");
            appId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(appId) || !Guid.TryParse(appId, out _))
            {
                Console.WriteLine("Invalid input, please try again!");
                goto AppId;
            }

        Upn:
            Console.WriteLine("\nEnter your User Principal Name");
            upn = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(upn))
            {
                Console.WriteLine("Invalid input, please try again!");
                goto Upn;
            }

        return new AppSettings { ApplicationId = appId, UserPrincipalName = upn, IsMfaExcluded = bypassMfaCheck };
    }

    private static async Task RunAsync(IServiceProvider serviceProvider)
    {
        var partnerTenantId = await serviceProvider.GetRequiredService<ITokenProvider>().GetTenantIdAsync();

    ShowOptions:
        Console.WriteLine("\nPlease choose an option:");

        Console.WriteLine("1. Fetch customer agreement records");
        Console.WriteLine("2. Update customer agreement records");
        Console.WriteLine("3. Exit\n");

    SelectOption:
        var option = Console.ReadLine();

        if (!short.TryParse(option, out short input) || !(input >= 1 && input <= 3))
        {
            Console.WriteLine("Invalid input, please try again!");
            goto SelectOption;
        }

        if (input == 3)
        {
            Console.WriteLine("Exiting the tool!");
            Environment.Exit(Environment.ExitCode);
        }

        Stopwatch stopwatch = Stopwatch.StartNew();

        var result = input switch
        {
            1 => await serviceProvider.GetRequiredService<ICustomerProvider>().FetchAndSaveCustomerAgreementRecords(),
            2 => await serviceProvider.GetRequiredService<ICustomerProvider>().UpdateCustomerAgreementRecords(partnerTenantId),
            _ => throw new InvalidOperationException("Invalid input")
        };

        stopwatch.Stop();
        Console.WriteLine($"Completed the operation {input} in {stopwatch.Elapsed}");
        Console.WriteLine("========================================================");

        goto ShowOptions;
    }
}
