using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taggly.Logging.Enums;

namespace Taggly.Logging.Abstractions;

public interface ILogLevelProvider
{
    LogLevel CurrentLevel { get; }
    void SetLevel(LogLevel level);
}
