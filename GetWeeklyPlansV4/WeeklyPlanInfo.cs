using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GetWeeklyPlansV4;

public class WeeklyPlanInfo
{
    public string Title { get; set; }
    public string DownloadLink { get; set; }
}

public class WeeklyPlanInfos : ObservableCollection<WeeklyPlanInfo>
{
    public WeeklyPlanInfos()
    {
        
    }
}