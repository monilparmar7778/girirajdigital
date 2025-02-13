using Giriraj_digital.Models;
using Giriraj_digital.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Giriraj_digital.Controllers
{
	public class TimeTableController : Controller
	{
		private readonly ITimeTableRepository _repository;

		public TimeTableController(ITimeTableRepository repository)
		{
			_repository = repository;
		}

		public ActionResult Index()
		{
			return View(new InputModel());
		}

		[HttpPost]
		public IActionResult SubmitInput(InputModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", model);

			_repository.SaveInput(model);
			return RedirectToAction("AllocateSubjects");
		}

		public ActionResult AllocateSubjects()
		{
			var input = _repository.GetInput();
			if (input == null || input.TotalSubjects <= 0)
			{
				ModelState.AddModelError("", "Invalid input data.");
				return RedirectToAction("Index");
			}

			var subjectsList = new List<string>();
			for (int i = 1; i <= input.TotalSubjects; i++)
			{
				subjectsList.Add($"Subject {i}");
			}

			var subjectModel = new TimeTableModel
			{
				TotalHours = input.TotalHours,
				Subjects = subjectsList,
				SubjectHours = subjectsList.ToDictionary(s => s, s => 0)
			};

			return View(subjectModel);
		}

		[HttpPost]
		public ActionResult SubmitSubjects(TimeTableModel model)
		{
			if (model == null || model.SubjectHours == null)
			{
				ModelState.AddModelError("", "Invalid data received.");
				return View("AllocateSubjects", model);
			}

			var input = _repository.GetInput();
			if (input == null)
			{
				ModelState.AddModelError("", "Unable to retrieve input data.");
				return View("AllocateSubjects", model);
			}

			int totalHours = input.TotalHours;

			if (model.SubjectHours.Values.Sum() != totalHours)
			{
				ModelState.AddModelError("", $"Total allocated hours must be {totalHours}.");
				return View("AllocateSubjects", model);
			}

			_repository.SaveTimeTable(model);
			return RedirectToAction("GenerateTimeTable");
		}

		public ActionResult GenerateTimeTable()
		{
			var timetable = _repository.GetTimeTable();
			var input = _repository.GetInput();

			if (timetable == null || input == null)
				return RedirectToAction("Index");

			timetable.TimeTable = new string[input.SubjectsPerDay, input.WorkingDays];

			// Create a list of subjects based on allocated hours
			List<string> subjectsList = new List<string>();
			foreach (var subject in timetable.SubjectHours ?? new Dictionary<string, int>())
			{
				subjectsList.AddRange(Enumerable.Repeat(subject.Key, subject.Value));
			}

			Random rand = new Random();
			for (int i = 0; i < input.SubjectsPerDay; i++)
			{
				for (int j = 0; j < input.WorkingDays; j++)
				{
					if (subjectsList.Count == 0)
					{
						timetable.TimeTable[i, j] = "N/A"; // Handle empty slots
					}
					else
					{
						int index = rand.Next(subjectsList.Count);
						timetable.TimeTable[i, j] = subjectsList[index]; // Assign subject name
						subjectsList.RemoveAt(index);
					}
				}
			}

			return View(timetable);
		}

	}
}
