namespace Giriraj_digital.Models
{
	public class TimeTableModel
	{
		public List<string> Subjects { get; set; } = new List<string>();
		public Dictionary<string, int> SubjectHours { get; set; } = new Dictionary<string, int>();
		public string[,] TimeTable { get; set; }

		public int TotalSubjects { get; set; }	
		public int TotalHours { get; set; }
	}
}
