using System;
using EFProject.Context;
using EFProject.Providers;
using System.Linq;

namespace EFProject
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                OutputAll();

                string choice = string.Empty;
                Console.WriteLine("{0}{0}1 - Добавить студента{0}2 - Удалить студента", Environment.NewLine);
                while(true)
                {
                    choice = Console.ReadLine();

                    if (choice != "1" && choice != "2")
                    {
                        Console.WriteLine("Выберите один из представленных вариантов.");
                    }
                    else
                    {
                        break;
                    }
                }

                if (choice.Equals("1"))
                {
                    AddStudent();
                }
                else
                {
                    RemoveStudent();
                }
            }
        }

        private static int ChooseGroup()
        {
            int groupCount = OutputAndCountGroups();

            int choice;
            Console.WriteLine("Выберите группу:");
            while (true)
            {
                bool parseSuccess = int.TryParse(Console.ReadLine(), out choice);

                if (!parseSuccess || choice > groupCount || choice < 1)
                {
                    Console.WriteLine("Выберите один из представленных вариантов.");
                }
                else
                {
                    break;
                }
            }
            return GroupIdByIndex(choice);
        }

        private static void RemoveStudent()
        {

            int groupRefId = ChooseGroup();

            int studentCount = OutputAndCountStudents(groupRefId);

            if(studentCount == 0)
            {
                Console.WriteLine("Группа пустая.");
                Console.ReadKey();
                return;
            }

            int choice;
            Console.WriteLine("Выберите студента:");
            while (true)
            {
                bool parseSuccess = int.TryParse(Console.ReadLine(), out choice);

                if (!parseSuccess || choice > studentCount || choice < 1)
                {
                    Console.WriteLine("Выберите один из представленных вариантов.");
                }
                else
                {
                    break;
                }
            }

            int studentId = StudentIdByIndex(choice, groupRefId);

            UniProvider.RemoveStudent(studentId);

        }

        private static void AddStudent()
        {
            int groupRefId = ChooseGroup();
            string firstName, lastName, middleName;
            Console.Clear();
            Console.WriteLine("Введите фамилию:");

            while (true)
            {
                lastName = Console.ReadLine();
                if (!lastName.All(c => char.IsLetter(c)))
                {
                    Console.WriteLine("Фамилия должна состоять только из букв.");
                }
                else if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine("Это поле нельзя оставлять пустым.");
                }
                else if (lastName.Length > 30)
                {
                    Console.WriteLine("Фамилия слишком большая.");
                }
                else
                {
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine("Введите имя:");
            while (true)
            {
                firstName = Console.ReadLine();
                if (!firstName.All(c => char.IsLetter(c)))
                {
                    Console.WriteLine("Имя должно состоять только из букв.");
                }
                else if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine("Это поле нельзя оставлять пустым.");
                }
                else if (firstName.Length > 30)
                {
                    Console.WriteLine("Имя слишком большое.");
                }
                else
                {
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine("Введите отчество/второе имя:");
            while (true)
            {
                middleName = Console.ReadLine();
                if (!middleName.All(c => char.IsLetter(c)))
                {
                    Console.WriteLine("Имя/отчество должно состоять только из букв.");
                }
                else if (string.IsNullOrEmpty(middleName))
                {
                    middleName = null;
                    break;
                }
                else if (middleName.Length > 30)
                {
                    Console.WriteLine("Имя/отчество слишком большое.");
                }
                else
                {
                    break;
                }
            }

            UniProvider.AddStudent(groupRefId, firstName, lastName, middleName);
        }

        private static int OutputAndCountGroups()
        {
            Console.Clear();
            int result;
            using (var context = new UniContext())
            {
                int index = 1;
                foreach (var g in context.Groups)
                {
                    Console.WriteLine($"{index++}. {g.Name}");
                }
                result = context.Groups.Count();
            }
            return result;
        }

        private static int OutputAndCountStudents(int groupRefId)
        {
            Console.Clear();
            int result;
            using (var context = new UniContext())
            {
                int index = 1;
                var students = context.Students
                    .Where(s => s.GroupRefId.Equals(groupRefId))
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ThenBy(s => s.MiddleName);

                foreach (var s in students)
                {
                    Console.WriteLine($"{index++}. {s.LastName} {s.FirstName} {s.MiddleName ?? string.Empty}");
                }
                result = students.Count();
            }
            return result;
        }

        private static void OutputAll()
        {
            Console.Clear();
            using (var context = new UniContext())
            {
                foreach (var g in context.Groups)
                {

                    var students = context.Students.Where(s => s.GroupRefId.Equals(g.Id))
                        .OrderBy(s => s.LastName)
                        .ThenBy(s => s.FirstName)
                        .ThenBy(s => s.MiddleName);

                    Console.WriteLine($"{g.Name} ({students.Count()})");

                    int index = 1;
                    foreach (var s in students)
                    {
                        Console.WriteLine($"  {index++}. {s.LastName} {s.FirstName} {s.MiddleName ?? string.Empty}");
                    }
                }
            }
        }

        private static int GroupIdByIndex(int index)
        {
            int result;
            using (var context = new UniContext())
            {
                result = context.Groups.OrderBy(g => g.Name).ToArray()[index-1].Id;
            }
            return result;
        }

        private static int StudentIdByIndex(int index, int groupRefId)
        {
            int result;
            using (var context = new UniContext())
            {
                result = context.Students.Where(s => s.GroupRefId.Equals(groupRefId))
                    .OrderBy(s => s.LastName)
                    .ThenBy(s => s.FirstName)
                    .ThenBy(s => s.MiddleName)
                    .ToArray()[index-1].Id;
            }
            return result;
        }
    }
}
