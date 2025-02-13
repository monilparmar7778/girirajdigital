using Giriraj_digital.Models;
namespace Giriraj_digital.Repositories
{
	public interface ITimeTableRepository
	{
		void SaveInput(InputModel input);
		InputModel GetInput();
		void SaveTimeTable(TimeTableModel timetable);
		TimeTableModel GetTimeTable();
	}
}
