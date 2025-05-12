using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface IReportService
{
    List<ReportView> All(Guid dbId);
    void Add(ReportView report);
    void AddCounter(Guid dbId, (string Title, string Value, Guid Id) counter);
    Option<ReportView> GetByDbId(Guid id);
    Option<ReportView> GetByEnvId(Guid envId);
    void Remove(Guid dbId);
}