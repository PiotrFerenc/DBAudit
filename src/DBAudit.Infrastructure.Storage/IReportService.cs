using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage;

public interface IReportService
{
    List<ReportView> All(Guid dbId);
    void Add(ReportView report);
    void AddCounter(Guid dbId, (string Title, string Value) counter);
    Option<ReportView> Get(Guid dbId);
    void Remove(Guid dbId);
}