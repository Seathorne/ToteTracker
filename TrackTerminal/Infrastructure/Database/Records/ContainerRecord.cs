namespace BoxSearch.Infrastructure.Database.Records;

internal record ContainerRecord(
    Guid? ContainerID,
    decimal? ContainerType,
    string? ContainerName,
    DateTime? CreatedDate);