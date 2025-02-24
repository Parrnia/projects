using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Domain.Interfaces;
public interface IChangeDateAware
{
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}
