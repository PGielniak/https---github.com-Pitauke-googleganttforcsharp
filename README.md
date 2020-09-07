# https---github.com-Pitauke-googleganttforcsharp

# googleganttforcsharp

This class library let's you convert from an IEnumerable type that implements IGoogleVisualizable interface to a string representing Json string needed to generate Gantt Chart from Google Visualization API

## Classes

IGoogleVisualizable- This interface should be implemented by a helper class in your projects that can transform multiple collections into one

JSONHelper- This class has one method that accepts IEnumerable<IGoogleVisualizable> and returns a string that can be transformed in your JavaScript code.
  
## Example

```
public class ListMapper : IGoogleVisualizable // implement the IGoogleVisualizable interface
    {

//initialize database context
        public PDbContext _context; 


        public ListMapper(PSADbContext context)
        {
            _context = context;
        }

      
// these properties are required 

        public int ID { get; set; }
        public string Resource { get; set; }
        public string TaskName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public int PrcComplete { get; set; }
        public string Dependencies { get; set; }


      //initialize all lists that need to be merged into one. In this case I have 3 tables that are related to each other.  
      
        public List<IGoogleVisualizable> ConvertLists()
        {


            var projects = _context.Projects.ToList();
            var items = _context.Items.ToList();
            var problems = _context.Problems.ToList();

//Initialize mapped list
            List<IGoogleVisualizable> mappedList = new List<IGoogleVisualizable>();

        
            foreach (var item in projects)
            {

//Map properties. My original project doesn't have the property Resource so I assign it a string literal as all my projects/items/problems should have the same resource
                mappedList.Add(new ListMapper(_context)
                {
                    ID = item.ProjectID,
                    Resource = "projectResource",
                    TaskName = item.ProjectName,
                    StartDate = item.startTime,
                    EndDate = item.endTime,
                    Duration = item.Duration,
                    PrcComplete = item.PrcComplete,
                    Dependencies = String.Empty
                });




            }


            foreach (var itemq in items)
            {
                mappedList.Add(new ListMapper(_context)
                {

                    ID = itemq.ItemID,
                    Resource = "itemResource",
                    TaskName = itemq.Name,
                    StartDate = itemq.startTime,
                    EndDate = itemq.endTime,
                    Duration = itemq.Duration,
                    PrcComplete = itemq.PrcComplete,
                    Dependencies = $"{itemq.Project.ProjectName}{itemq.Project.ProjectID}"
                });

            }

            foreach (var itemw in problems)
            {
                mappedList.Add(new ListMapper(_context)
                {
                    ID = itemw.ProblemID,
                    Resource = "problemResource",
                    TaskName = itemw.Name,
                    StartDate = itemw.startTime,
                    EndDate = itemw.endTime,
                    Duration = itemw.Duration,
                    PrcComplete = itemw.PrcComplete,
                    Dependencies = $"{itemw.Item.Name}{itemw.Item.ItemID}"
                });

            }

            return mappedList;
        }

      
    }

```

## API

```
    
       [HttpGet]
        public string GetValues()
        {
            //initialize the list
            IGoogleVisualizable listMapper = new ListMapper(_context);
            
            //Call the helper class to convert the list
            List<IGoogleVisualizable> mappedList = listMapper.ConvertLists();

            //Call dll class to convert the list to a json string

            string value = JSONHelper.BuildArray(mappedList);
           

            return value;


        }
   
```        
        
## HTML

```<html>
<head>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['gantt'] });
        google.charts.setOnLoadCallback(drawChart);

        function daysToMilliseconds(days) {
          return days * 24 * 60 * 60 * 1000;
        }

        function drawChart() {

         
      
             var jsonData = $.ajax({
                 url: "http:/localhost:5002/api",
              dataType: "text",
              async: false
              }).responseText;



          
//The returned string must be parsed as JSON
           var array = JSON.parse(jsonData);

             //
             //transform the JSON to JavaScript DataTable
var datatabledata = new google.visualization.arrayToDataTable(array);



//Optional options for the Chart. See  https://developers.google.com/chart/interactive/docs/gallery/ganttchart for more options
          var options = {
              height: 1000,
              gantt: {
                  arrow: {
                      angle: 60,
                     width: 1,
                      color: '#ffe0b200',
                  },
                  criticalPathEnabled: false
              }
          };

//Create a new Chart object
          var chart = new google.visualization.Gantt(document.getElementById('chart_div'));

//Draw the chart
          chart.draw(datatabledata, options);
}
    </script>

   
</head>

<body>
    <div id="chart_div"></div>
</body>
</html>
```

# Results


