namespace NutritionAdvisor.Domain.Entities;

public class AiRecommendationLog
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    public DateTime RequestedAt { get; private set; }
    public string InputSnapshotJson { get; private set; } = null!;
    public string OutputSnapshotJson { get; private set; } = null!;

    public bool Accepted { get; private set; }
}