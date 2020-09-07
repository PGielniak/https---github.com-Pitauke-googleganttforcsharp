using System;
using System.Collections.Generic;

namespace JsonHelper
{
    /// <summary>
    /// This interface should be implemented in the mapper class of your project
    /// </summary>
    public interface IGoogleVisualizable
    {
         public int ID { get; set; }
         public string Resource { get; set; }
         public string TaskName { get; set; }
         public DateTime? StartDate { get; set; }
         public DateTime? EndDate { get; set; }
         public int? Duration { get; set; }

         public int PrcComplete { get; set; }
         public string Dependencies { get; set; }

         public List<IGoogleVisualizable> ConvertLists();

    }
}