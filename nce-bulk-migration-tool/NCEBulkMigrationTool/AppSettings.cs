namespace NCEBulkMigrationTool;

internal record AppSettings
{
    public string AppId { get; init; } = string.Empty;

    public string Upn { get; init; } = string.Empty;

    public string Domain
    {
        get
        {
            var index = this.Upn.LastIndexOf("@");
            if (index == -1) { return string.Empty; }
            return this.Upn[++index..];
        }
    }
}