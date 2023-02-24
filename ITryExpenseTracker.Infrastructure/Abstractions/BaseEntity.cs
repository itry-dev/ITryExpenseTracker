using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITryExpenseTracker.Core.Abstractions;

namespace ITryExpenseTracker.Infrastructure.Abstractions;

public class BaseEntity: IModelEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    public DateTime? Updated { get; set; }

    [MaxLength(100)]
    public string? UpdatedBy { get; set; }

    public DateTime? Deleted { get; set; }

    [MaxLength(100)]
    public string? DeletedBy { get; set; }
}
