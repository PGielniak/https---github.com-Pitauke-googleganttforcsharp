using System.Text;
using System;
using System.Collections.Generic;

namespace JsonHelper
{
    public static class JSONHelper
    {
        /// <summary>
        /// This  class contains one method that builds a string literal needed to pass to the Google Visualization API in order to generate gantt chart
        /// </summary>
        /// <param name="list">
        /// This parameter should be passed from your Mapper class or Api </param>
        /// <returns>String that will be later transformed into javascript DataTable</returns>
        public static string BuildArray(IEnumerable<IGoogleVisualizable> convertedList)
        {
        StringBuilder sb = new StringBuilder(String.Empty);

        sb.Append("[[{\"label\": \"TaskID\", \"type\": \"string\"},");
        sb.Append("{\"label\": \"Task Name\", \"type\": \"string\"},");
        sb.Append("{\"label\": \"Resource\", \"type\": \"string\"},");
        sb.Append("{\"label\": \"Start Date\", \"type\": \"date\"},");
        sb.Append("{\"label\": \"End Date\", \"type\": \"date\"},");
       sb.Append("{\"label\": \"Duration\", \"type\": \"number\"},"); 
        sb.Append("{\"label\": \"Percent Complete\", \"type\": \"number\"},");
        sb.Append("{\"label\": \"Dependencies\", \"type\": \"string\"}],");


        foreach (var item in convertedList)
        {   
            
            //Null checks for dates. To see how Gantt handles calculating start date, end date and duration please refer to https://developers.google.com/chart/interactive/docs/gallery/ganttchart
            if(item.StartDate==null && item.EndDate==null)
            {
                sb.AppendLine($"[\"{item.TaskName}{item.ID}\",\"{item.TaskName}\",\"{item.Resource}\",\"\",\"\",\"\",\"{item.PrcComplete}\",\"{item.Dependencies}\"],");
            }
            else if(item.StartDate==null)
            { 
                sb.AppendLine($"[\"{item.TaskName}{item.ID}\",\"{item.TaskName}\",\"{item.Resource}\",\"\",\"Date({item.EndDate.Value.Year},{item.EndDate.Value.Month-1},{item.EndDate.Value.Day})\",\"\",\"{item.PrcComplete}\",\"{item.Dependencies}\"],");
            }
            else if(item.EndDate==null)
            {
                 sb.AppendLine($"[\"{item.TaskName}{item.ID}\",\"{item.TaskName}\",\"{item.Resource}\",\"Date({item.StartDate.Value.Year},{item.StartDate.Value.Month-1},{item.StartDate.Value.Day})\",\"\",\"{item.PrcComplete}\",\"{item.Dependencies}\"],");
            } 
            else
            {
                 sb.AppendLine($"[\"{item.TaskName}{item.ID}\",\"{item.TaskName}\",\"{item.Resource}\",\"Date({item.StartDate.Value.Year},{item.StartDate.Value.Month-1},{item.StartDate.Value.Day})\",\"Date({item.EndDate.Value.Year},{item.EndDate.Value.Month-1},{item.EndDate.Value.Day})\",\"\",\"{item.PrcComplete}\",\"{item.Dependencies}\"],");
            }

                

            }
        sb.Length-=3;
        sb.Append("]");

        return sb.ToString();
        }

    }

}
