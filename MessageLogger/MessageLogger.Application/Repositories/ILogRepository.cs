using MessageLogger.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Application.Repositories;
public interface ILogRepository
{
    Task<bool> CreateAsync(LogMessage log);

    Task<LogMessage?> GetByIdAsync(Guid id);

    Task<IEnumerable<LogMessage>> GetAllAsync();
}
