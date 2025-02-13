using Giriraj_digital.Models;

namespace Giriraj_digital.Repositories
{
	public class TimeTableRepository : ITimeTableRepository
	{
		private static InputModel? _inputModel;
		private static TimeTableModel? _timeTableModel;

		public void SaveInput(InputModel input)
		{
			_inputModel = input;
		}

		public InputModel GetInput()
		{
			// Ensure we return a default InputModel if no data exists
			return _inputModel ?? new InputModel
			{
				TotalSubjects = 0,
				WorkingDays = 5,
				SubjectsPerDay = 6
			};
		}

		public void SaveTimeTable(TimeTableModel timetable)
		{
			_timeTableModel = timetable;
		}

		public TimeTableModel GetTimeTable()
		{
			// Ensure we return a default TimeTableModel if no data exists
			return _timeTableModel ?? new TimeTableModel
			{
				Subjects = new List<string> { "Subject 1", "Subject 2", "Subject 3" },
				SubjectHours = new Dictionary<string, int>(),
				TotalHours = _inputModel?.TotalHours ?? 30
			};
		}
	}
}
