namespace SmartRecruitmentPlatform.Backend.Services.JobMatching;

public sealed class MatchingWeightOptions
{
    public decimal Skills { get; set; } = 50m;
    public decimal Experience { get; set; } = 25m;
    public decimal Education { get; set; } = 15m;
    public decimal Location { get; set; } = 10m;

    public decimal Total => Skills + Experience + Education + Location;
}
