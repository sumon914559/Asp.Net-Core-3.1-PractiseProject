using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Model.Interfaces
{
  public interface ITrackable
    {
        DateTimeOffset CreatedAt { set; get; }

        string CreatedBy { set; get; }

        DateTimeOffset UpdatedAt { set; get; }
        string UpdatedBy { set; get; }
    }
}
