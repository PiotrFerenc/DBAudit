using DBAudit.Infrastructure.Contracts.Entities;
using LanguageExt;

namespace DBAudit.Infrastructure.Storage.Binary;

public class ReportService(IDbAuditStorage<ReportView> storage) : IReportService
{
    public List<ReportView> All(Guid dbId) => storage.Where(x => x.DatabaseId == dbId);
    public void Add(ReportView report) => storage.SaveItem(report);
    public Option<ReportView> GetByEnvId(Guid envId) => storage.Find(x => x.EnvId == envId);
    public void Remove(Guid dbId) => storage.RemoveByKey(x => x.DatabaseId == dbId);
    public void AddCounter(Guid dbId, (string Title, string Value) counter) => storage.Update(r => r.Counters.Add(counter), x => x.DatabaseId == dbId);
    public Option<ReportView> GetByDbId(Guid dbId) => storage.Find(x => x.DatabaseId == dbId);
}